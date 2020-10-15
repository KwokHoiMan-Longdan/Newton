using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Reserve_Invoices_2.Intercompany;

namespace Reserve_Invoices_2
{
    public class Delivery
    {
        public SAP sap;

        public Documents oDeliveryNotes;
        public Documents oDraft;
        public List<int> AllocatedLineNums;
        public List<int> ZeroBatchLineNums;
        public List<int> NonInvItemLineNums;
        public List<int> NonBatchLineNums;

        public int? DraftDocEntry = null;
        public int? STDraftDocEntry = null;

        public static List<int> DeliveryDraftsTaskList = new List<int>();

        // Stock from our own warehouses
        public List<StockTransferItem> WarehouseTransferItems = new List<StockTransferItem>();

        // Stock directly into the shop
        public List<GoodsReceiptItem> GoodsReceiptItems = new List<GoodsReceiptItem>();

        // Kimson delivery/PO items
        public List<KimsonDeliveryItem> KimsonDeliveryItems = new List<KimsonDeliveryItem>();

        public static string CreateDraft(SAP sap, Invoice invoice, PriceModeDocumentEnum pricemode)
        {
            // Which price mode
            var lines = invoice.GetInvoicesByPriceMode(pricemode);
            string results = null;

            if (lines.Length > 0)
            {
                // Group by shop

                var shopitems =

                    from i in lines
                    group i by new { i.BaseCard, i.WhsCode } into g
                    select new
                    {
                        Shop = g.Key.BaseCard,
                        Whscode = g.Key.WhsCode,
                        Items = g,
                        DocDate = g.First().DocDate
                    };

                // Memory
                var shopItemsList = shopitems.ToArray();


                // Create draft in SAP
                foreach (var shop in shopItemsList)
                {
                    Documents delivery = (Documents)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                    Documents invoices = (Documents)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                    delivery.DocObjectCodeEx = "15";        // 15 = Delivery
                    delivery.CardCode = shop.Shop;
                    delivery.DocDate = shop.DocDate.Value;
                    //delivery.BPL_IDAssignedToInvoice = 1;

                    // Price mode
                    delivery.PriceMode = pricemode;

                    var items = shop.Items.ToArray();

                    //Console.WriteLine($"Creating draft for {shop.Shop}, items: {items.Count()}");

                    for (int i = 0; i < items.Count(); i++)
                    {
                        if (i > 0)
                            delivery.Lines.Add();

                        // Items 
                        delivery.Lines.ItemCode = items[i].ItemCode;
                        //delivery.Lines.Quantity = Convert.ToDouble(items[i].Quantity);
                        delivery.Lines.Quantity = Convert.ToDouble(items[i].OpenQty.Value);
                        delivery.Lines.WarehouseCode = shop.Whscode;
                        delivery.Lines.UoMEntry = items[i].UomEntry.Value;

                        // References
                        delivery.Lines.BaseEntry = items[i].DocEntry;
                        delivery.Lines.BaseLine = items[i].LineNum;
                        delivery.Lines.BaseType = 13;       // 13 = Invoices

                        // Pricing
                        if (pricemode == PriceModeDocumentEnum.pmdGross)
                            delivery.Lines.GrossPrice = Convert.ToDouble(items[i].GrossPrice.Value);
                        if (pricemode == PriceModeDocumentEnum.pmdNet)
                            delivery.Lines.UnitPrice = Convert.ToDouble(items[i].NetPrice.Value);

                    }

                    int result = delivery.Add();

                    if (result != 0)
                        results = results + sap.oCompany.GetLastErrorDescription() + Environment.NewLine;
                    else
                    {
                        int newDocEntry = int.Parse(sap.oCompany.GetNewObjectKey());
                        DeliveryDraftsTaskList.Add(newDocEntry);

                        results = results + "OK - " + sap.oCompany.GetNewObjectKey() + Environment.NewLine;
                    }
                }
            }
            else
                results = "No invoices for " + pricemode + ".";

            return results;
        }

        public string CreateDraft()                 // Inter-branch draft
        {
            string results = null;

            if (WarehouseTransferItems.Count > 0)
            {

                // Which transfer mode 
                var linesInterBranch =
                    WarehouseTransferItems
                    .Where(x => x.IsSameBranch == false).ToArray();    // Stock from warehouses as Delivery/GRPO


                if (linesInterBranch.Length > 0)
                {
                    // Create draft in SAP for inter-branch transfer

                    Documents deliveryDraft = (Documents)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                    string cardcode = StockInfo.GetShopCardcodeByWhscode(WarehouseTransferItems.First().ToWarehouse);

                    deliveryDraft.DocObjectCodeEx = "15";           // 15 = Delivery
                    deliveryDraft.CardCode = cardcode;
                    deliveryDraft.DocDate = oDraft.DocDate;
                    deliveryDraft.BPL_IDAssignedToInvoice = 1;      // Warehouses always in branch 1




                    //var items = WarehouseTransferItems.ToArray();
                    var items = linesInterBranch;

                    for (int i = 0; i < items.Count(); i++)
                    {
                        if (i > 0)
                            deliveryDraft.Lines.Add();

                        // Items 
                        deliveryDraft.Lines.ItemCode = items[i].ItemCode;
                        deliveryDraft.Lines.Quantity = items[i].InvQuantity;
                        deliveryDraft.Lines.WarehouseCode = items[i].FromWarehouse;
                        deliveryDraft.Lines.UoMEntry = items[i].InvUomEntry;

                        // Accounting
                        deliveryDraft.Lines.COGSAccountCode = "500006";

                        // Pricing
                        //var pricemode = deliveryDraft.PriceMode;
                        //if (pricemode == PriceModeDocumentEnum.pmdGross)
                        //    deliveryDraft.Lines.GrossPrice = Convert.ToDouble(items[i].grosss.Value);
                        //if (pricemode == PriceModeDocumentEnum.pmdNet)
                        //    delivery.Lines.UnitPrice = Convert.ToDouble(items[i].NetPrice.Value);

                    }

                    // Accounting
                    Warehouse whsU49 = new Warehouse(sap, "U49");
                    Warehouse whsF = new Warehouse(sap, "F");
                    Warehouse whsLT = new Warehouse(sap, "LT");
                    whsU49.SetStockAccount("130000");
                    whsF.SetStockAccount("130000");
                    whsLT.SetStockAccount("130000");

                    int result = deliveryDraft.Add();

                    if (result != 0)
                        results = results + sap.oCompany.GetLastErrorDescription() + Environment.NewLine;
                    else
                    {
                        DraftDocEntry = int.Parse(sap.oCompany.GetNewObjectKey());
                        results = results + "OK - " + DraftDocEntry + Environment.NewLine;
                    }

                    whsU49.RevertInitialAccounts();
                    whsF.RevertInitialAccounts();
                    whsLT.RevertInitialAccounts();
                }
                else
                    results = "No items for warehouse inter-branch transfer.";

            }
            else
                results = "No items for warehouse transfer.";

            return results;
        }

        public string CreateDraftStockTransfer()     // Stock transfers draft
        {
            string results = null;

            if (WarehouseTransferItems.Count > 0)
            {
                // Which price mode

                var linesStockTransfer =
                    WarehouseTransferItems
                    .Where(x => x.IsSameBranch == true).ToArray();  // Same branch stock transfer


                if (linesStockTransfer.Length > 0)
                {
                    // Create draft for stock transfer (warehouse branch = shop branch = '1')
                    var items = linesStockTransfer;

                    var st = (SAPbobsCOM.StockTransfer)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransferDraft);
                    st.DocObjectCode = BoObjectTypes.oStockTransfer;

                    st.DocDate = DateTime.Today;
                    st.FromWarehouse = items[0].FromWarehouse;
                    st.ToWarehouse = items[0].ToWarehouse;

                    string text = null;

                    for (int i = 0; i < items.Count(); i++)
                    {
                        if (i > 0)
                            st.Lines.Add();

                        // Items
                        st.Lines.ItemCode = items[i].ItemCode;
                        st.Lines.Quantity = items[i].InvQuantity;
                        st.Lines.UoMEntry = items[i].InvUomEntry;
                        st.Lines.WarehouseCode = items[i].ToWarehouse;
                        st.Lines.FromWarehouseCode = items[i].FromWarehouse;

                        text += $"{items[i].ItemCode}, {items[i].InvQuantity} ({items[i].InvUomEntry}), {items[i].ToWarehouse}, {items[i].FromWarehouse}" + Environment.NewLine;
                    }


                    // Accounting
                    Warehouse whsU49 = new Warehouse(sap, "U49");
                    Warehouse whsF = new Warehouse(sap, "F");
                    Warehouse whsLT = new Warehouse(sap, "LT");
                    whsU49.SetStockAccount("130000-U49");
                    whsF.SetStockAccount("130000-F");
                    whsLT.SetStockAccount("130000-LT");

                    int result = st.Add();

                    if (result != 0)
                        results = results + sap.oCompany.GetLastErrorDescription() + Environment.NewLine;
                    else
                    {
                        STDraftDocEntry = int.Parse(sap.oCompany.GetNewObjectKey());
                        results = results + "OK - " + STDraftDocEntry + Environment.NewLine;
                    }

                    whsU49.RevertInitialAccounts();
                    whsF.RevertInitialAccounts();
                    whsLT.RevertInitialAccounts();

                }
                else
                    results = "No items for warehouse stock transfer";
            }
            else
                results = "No items for warehouse stock transfer";

            return results;
        }


        public string CreateDraftKimson(SAP sapks)
        {
            string results = null;
            string LongdanCardCode = "LDCustomer";
            string WarehouseCode = "F";

            if (KimsonDeliveryItems.Count > 0)
            {
                // Create draft in SAP

                Documents deliveryDraftKS = (Documents)sapks.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                deliveryDraftKS.DocObjectCodeEx = "15";           // 15 = Delivery
                deliveryDraftKS.CardCode = LongdanCardCode;
                deliveryDraftKS.DocDate = oDraft.DocDate;

                var items = KimsonDeliveryItems.ToArray();

                for (int i = 0; i < items.Count(); i++)
                {
                    if (i > 0)
                        deliveryDraftKS.Lines.Add();

                    // Items 
                    deliveryDraftKS.Lines.ItemCode = items[i].ItemCode;
                    deliveryDraftKS.Lines.Quantity = items[i].Quantity;
                    deliveryDraftKS.Lines.WarehouseCode = WarehouseCode;
                    deliveryDraftKS.Lines.UoMEntry = items[i].UoMEntry;
                    deliveryDraftKS.Lines.VolumeUnit = 4;

                    // Accounting
                    deliveryDraftKS.Lines.COGSAccountCode = "500007";
                }

                int result = deliveryDraftKS.Add();

                if (result != 0)
                    results = results + sapks.oCompany.GetLastErrorDescription() + Environment.NewLine;
                else
                    results = results + "OK - " + sapks.oCompany.GetNewObjectKey() + Environment.NewLine;


            }
            else
                results = "No items for Kimson import.";

            return results;
        }

        public bool LoadDraft(SAP sap, int docentry)
        {
            this.sap = sap;
            oDraft = (Documents)sap.oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
            return oDraft.GetByKey(docentry);
        }

        public void SortDraftItems()
        {
            if (oDraft != null)
            {
                ZeroBatchLineNums = new List<int>();
                NonInvItemLineNums = new List<int>();
                NonBatchLineNums = new List<int>();
                AllocatedLineNums = new List<int>();


                var lines = oDraft.Lines;

                for (int i = 0; i < lines.Count; i++)
                {

                    lines.SetCurrentLine(i);

                    // Item info
                    var item = new Item(sap, lines.ItemCode);

                    // Inventory item
                    if (item.oItem.InventoryItem == BoYesNoEnum.tYES)
                    {

                        // Managed by batches
                        if (item.oItem.ManageBatchNumbers == BoYesNoEnum.tYES)
                        {
                            var requireQty = Math.Round(lines.InventoryQuantity, 3);
                            double batchQty = Math.Round(lines.BatchNumbers.TotalBatchQuantity(), 3);


                            //if (lines.BatchNumbers.Quantity == 0
                            //    && lines.BinAllocations.Quantity == 0)
                            if (requireQty > batchQty)
                            {
                                // No batches allocated - no stock
                                ZeroBatchLineNums.Add(i);
                            }
                            else
                                AllocatedLineNums.Add(i);
                        }
                        else
                            NonBatchLineNums.Add(i);
                    }
                    else
                        NonInvItemLineNums.Add(i);
                }
            }
        }

        public void SortUnallocatedItems(SAP sap)
        {
            if (ZeroBatchLineNums.Count > 0)
            {

                var ZeroBatchLineNumsArray = ZeroBatchLineNums.ToArray();
                var line = oDraft.Lines;
                using (var ctx = new DevEntities())
                {
                    for (int i = 0; i < ZeroBatchLineNumsArray.Length; i++)
                    {
                        line.SetCurrentLine(ZeroBatchLineNumsArray[i]);

                        double requiredQty = line.InventoryQuantity - line.BatchNumbers.TotalBatchQuantity();
                        //decimal dInventoryQuantity = Convert.ToDecimal(line.InventoryQuantity);
                        decimal dReqInventoryQuantity = Convert.ToDecimal(requiredQty);

                        var fromwhs = StockInfo.WarehouseStockCheck(line.ItemCode, dReqInventoryQuantity);

                        if (fromwhs != null)
                        {

                            //  Get from warehouses.


                            var invuom = new Item(sap, line.ItemCode).oItem.InventoryUoMEntry;
                            WarehouseTransferItems.Add(new StockTransferItem
                            {
                                LineNum = line.LineNum,
                                ItemCode = line.ItemCode,
                                //InvQuantity = line.InventoryQuantity,
                                InvQuantity = Convert.ToDouble(dReqInventoryQuantity),
                                FromWarehouse = fromwhs.WhsCode,
                                ToWarehouse = line.WarehouseCode,
                                InvUomEntry = invuom
                            });
                        }
                        else
                        {

                            // Check KIMSON
                            string owner = StockInfo.IsItemLongdanKimson(sap, line.ItemCode);


                            switch (owner)
                            {
                                case "LONGDAN":

                                    // Direct goods in
                                    var grItem = new GoodsReceiptItem();
                                    grItem.ItemCode = line.ItemCode;
                                    //grItem.InvQuantity = line.InventoryQuantity;
                                    grItem.InvQuantity = Convert.ToDouble(dReqInventoryQuantity);
                                    grItem.ShopWhscode = line.WarehouseCode;

                                    grItem.Quantity = line.Quantity;
                                    grItem.UomCode = line.UoMCode;
                                    grItem.UomEntry = line.UoMEntry;

                                    GoodsReceiptItems.Add(grItem);

                                    break;


                                case "KIMSON":

                                    var k_item = new KimsonDeliveryItem();
                                    k_item.ItemCode = line.ItemCode;
                                    k_item.Quantity = line.Quantity;
                                    k_item.UoMEntry = line.UoMEntry;

                                    KimsonDeliveryItems.Add(k_item);

                                    break;
                            }

                            // Not KIMSON then GOODS IN


                        }
                    }
                }
            }
        }

        public void SortNonBatchItems(SAP sap)
        {
            if (NonBatchLineNums.Count > 0)
            {
                var NonBatchLineNumsArray = NonBatchLineNums.ToArray();
                var line = oDraft.Lines;
                var allocatorNonbatch = new NonBatchAllocation();

                for (int i = 0; i < NonBatchLineNumsArray.Length; i++)
                {
                    line.SetCurrentLine(NonBatchLineNumsArray[i]);

                    // Any stock in the shop?
                    double qty = allocatorNonbatch.AllocateNonBatchStock(line.ItemCode, line.WarehouseCode, line.InventoryQuantity);


                    if (qty == 0)
                    {
                        decimal dInventoryQuantity = Convert.ToDecimal(line.InventoryQuantity);
                        var fromwhs = StockInfo.WarehouseStockCheck(line.ItemCode, dInventoryQuantity);

                        if (fromwhs != null)
                        {

                            //  Get from warehouses.

                            var invuom = new Item(sap, line.ItemCode).oItem.InventoryUoMEntry;
                            WarehouseTransferItems.Add(new StockTransferItem
                            {
                                LineNum = line.LineNum,
                                ItemCode = line.ItemCode,
                                InvQuantity = line.InventoryQuantity,
                                FromWarehouse = fromwhs.WhsCode,
                                ToWarehouse = line.WarehouseCode,
                                InvUomEntry = invuom
                            });
                        }
                        else
                        {
                            // Goods in
                            GoodsReceiptItem gitem = new GoodsReceiptItem();
                            gitem.ItemCode = line.ItemCode;
                            gitem.InvQuantity = line.InventoryQuantity;
                            gitem.Quantity = line.Quantity;
                            gitem.UomCode = line.UoMCode;
                            gitem.UomEntry = line.UoMEntry;
                            gitem.ShopWhscode = line.WarehouseCode;

                            GoodsReceiptItems.Add(gitem);
                        }
                    }
                }
            }
        }

        public int ProcessDraft2Regular()
        {
            return oDraft.SaveDraftToDocument(); ;
        }

        public GoodsReceiptItem[] CopyDeliveryXferItems2GRPO()
        {
            if (WarehouseTransferItems.Count > 0)
            {

                // Which price mode
                var lines =
                    WarehouseTransferItems
                    .Where(x => x.IsSameBranch == false).ToArray();


                if (lines.Length > 0)
                {

                    // Create draft in SAP

                    //Documents deliveryDraft = (Documents)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                    List<GoodsReceiptItem> goodsitems = new List<GoodsReceiptItem>();

                    var items = WarehouseTransferItems.ToArray();

                    for (int i = 0; i < items.Count(); i++)
                    {

                        // Items 
                        GoodsReceiptItem item = new GoodsReceiptItem();
                        item.ItemCode = items[i].ItemCode;
                        item.InvQuantity = items[i].InvQuantity;
                        item.ShopWhscode = items[i].ToWarehouse;

                        goodsitems.Add(item);
                    }

                    return goodsitems.ToArray();
                }
            }

            return null;
        }

        public GoodsReceiptItem[] CreateGoodsReceiptItemsFromZeroBatchLineNums()
        {
            var grItems = new List<GoodsReceiptItem>();

            if (ZeroBatchLineNums.Count > 0)
            {
                var ZeroBatchLineNumsArray = ZeroBatchLineNums.ToArray();
                var line = oDraft.Lines;

                for (int i = 0; i < ZeroBatchLineNumsArray.Length; i++)
                {
                    line.SetCurrentLine(ZeroBatchLineNumsArray[i]);

                    // Direct goods in
                    var grItem = new GoodsReceiptItem();
                    grItem.ItemCode = line.ItemCode;
                    grItem.InvQuantity = line.InventoryQuantity;
                    grItem.ShopWhscode = line.WarehouseCode;

                    grItem.Quantity = line.Quantity;
                    grItem.UomCode = line.UoMCode;
                    grItem.UomEntry = line.UoMEntry;

                    grItems.Add(grItem);
                }

            }

            return grItems.ToArray();
        }

        public bool LoadKSDeliveryNote(SAP sapks, int docentry)
        {
            oDeliveryNotes = (Documents)sapks.oCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);

            return oDeliveryNotes.GetByKey(docentry);
        }

        public int KSDeliveryNote2LDGRPO()
        {
            string kimsonCardcode = "KIMSON";
            var ks_del = oDeliveryNotes;

            var ld_pdn = (Documents)sap.oCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

            ld_pdn.CardCode = kimsonCardcode;
            ld_pdn.DocDate = oDraft.DocDate;
            ld_pdn.BPL_IDAssignedToInvoice = oDraft.BPL_IDAssignedToInvoice;

            var p_lines = ld_pdn.Lines;
            var d_lines = ks_del.Lines;

            string shopAccounting = null;

            for (int i = 0; i < d_lines.Count; i++)
            {
                d_lines.SetCurrentLine(i);

                if (i > 0)
                    p_lines.Add();

                if (i == 0)
                    shopAccounting = oDraft.Lines.WarehouseCode;

                p_lines.ItemCode = d_lines.ItemCode;
                p_lines.Quantity = d_lines.Quantity;
                p_lines.UoMEntry = d_lines.UoMEntry;
                p_lines.UnitPrice = d_lines.UnitPrice;
                p_lines.WarehouseCode = oDraft.Lines.WarehouseCode;
                p_lines.VolumeUnit = 4;

                // Batches

                for (int j = 0; j < d_lines.BatchNumbers.Count; j++)
                {
                    if (j > 0)
                        p_lines.BatchNumbers.Add();

                    d_lines.BatchNumbers.SetCurrentLine(j);

                    p_lines.BatchNumbers.BatchNumber = d_lines.BatchNumbers.BatchNumber;
                    p_lines.BatchNumbers.Quantity = d_lines.BatchNumbers.Quantity;
                    p_lines.BatchNumbers.BaseLineNumber = d_lines.BatchNumbers.BaseLineNumber;
                    p_lines.BatchNumbers.ExpiryDate = d_lines.BatchNumbers.ExpiryDate;
                }
            }
            // Accounting
            Warehouse shopWhs = new Warehouse(sap, shopAccounting);
            //Warehouse whsF = new Warehouse(sap, "F");
            //Warehouse whsLT = new Warehouse(sap, "LT");
            shopWhs.SetAllocationAccount("208042");
            //whsF.SetAllocationAccount("208042");
            //whsLT.SetAllocationAccount("208042");

            int result = ld_pdn.Add();

            shopWhs.RevertInitialAccounts();
            //whsF.RevertInitialAccounts();
            //whsLT.RevertInitialAccounts();

            return result;
        }

        public int ClearDraft()
        {
            return oDraft.Remove();
        }

        public static void ClearAllDrafts(SAP sap)
        {
            using (var ctx = new DevEntities())
            {
                var deliveries = ctx.ReserveInvoices_OpenedDrafts.Select(x => x.DocEntry).ToArray();

                for (int i = 0; i < deliveries.Length; i++)
                {
                    Delivery d = new Delivery();
                    d.LoadDraft(sap, deliveries[i]);

                    int result = d.ClearDraft();

                    if (result == 0)
                        Console.WriteLine(deliveries[i] + " cleared.");
                    else
                        Console.WriteLine("Error clearing " + deliveries[i] + ". " + sap.oCompany.GetLastErrorDescription());

                    d = null;
                }
            }
        }

        public static int CountOpenedDrafts()
        {
            using (var ctx = new DevEntities())
            {
                var deliveries = ctx.ReserveInvoices_OpenedDrafts.Select(x => x.DocEntry).ToArray();

                return deliveries.Length;
            }
        }
    }
}
