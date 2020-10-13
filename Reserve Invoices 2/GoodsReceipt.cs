using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class GoodsReceipt
    {
        public static int CreateGRPO(SAP sap, GoodsReceiptItem[] items, string cardcode, DateTime docdate, int? bplid)
        {
            // Cost price list number
            int priceListNum = 2;

            var grpo = (Documents)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);


            // Headers
            grpo.CardCode = cardcode;
            grpo.DocDate = docdate;

            if (bplid.HasValue)
                grpo.BPL_IDAssignedToInvoice = bplid.Value;

            // Console
            Console.WriteLine($"{grpo.CardCode} {grpo.DocDate.ToString("dd/MM/yyyy")} BPLId: {grpo.BPL_IDAssignedToInvoice}");

            List<string> frozenItems = new List<string>();
            List<string> unpurchasableItems = new List<string>();

            for (int i = 0; i < items.Length; i++)
            {

                var item = new Item(sap, items[i].ItemCode);

                // Inactive item?
                if (item.oItem.Frozen == BoYesNoEnum.tYES)
                {
                    item.oItem.Frozen = BoYesNoEnum.tNO;
                    if (item.oItem.Update() == 0)
                        frozenItems.Add(items[i].ItemCode);
                }

                // Unpurchasable items?
                if (item.oItem.PurchaseItem == BoYesNoEnum.tNO)
                {
                    item.oItem.PurchaseItem = BoYesNoEnum.tYES;
                    if (item.oItem.Update() == 0)
                        unpurchasableItems.Add(items[i].ItemCode);
                }


                if (i > 0)
                    grpo.Lines.Add();

                grpo.Lines.ItemCode = items[i].ItemCode;
                grpo.Lines.InventoryQuantity = items[i].InvQuantity;
                grpo.Lines.UoMEntry = item.oItem.InventoryUoMEntry;

                //grpo.Lines.WarehouseCode = items[i].ShopWhscode;

                // Cost price
                item.oItem.PriceList.SetCurrentLine(priceListNum);

                grpo.Lines.UnitPrice = item.oItem.PriceList.Price;
                grpo.Lines.Currency = item.oItem.PriceList.Currency;


                if (item.oItem.ManageBatchNumbers == BoYesNoEnum.tYES)
                {
                    // Batches
                    grpo.Lines.BatchNumbers.BatchNumber = DateTime.Today.ToString("yyMMdd") + "GRPO" + DateTime.Now.Second.ToString("00");
                    grpo.Lines.BatchNumbers.Quantity = items[i].InvQuantity;
                }

                // Console
                Console.WriteLine($"{grpo.Lines.ItemCode} x{grpo.Lines.Quantity} [{grpo.Lines.BatchNumbers.BatchNumber} x{grpo.Lines.BatchNumbers.Quantity}]");
            }

            var result = grpo.Add();


            // Refreeze items
            foreach (string itemcode in frozenItems)
            {
                var item = new Item(sap, itemcode);
                item.oItem.Frozen = BoYesNoEnum.tNO;
                item.oItem.Update();
                Console.WriteLine($"{itemcode} has reverted back to frozen.");
            }

            // Re-unpurchase items
            foreach (string itemcode in unpurchasableItems)
            {
                var item = new Item(sap, itemcode);
                item.oItem.PurchaseItem = BoYesNoEnum.tNO;
                item.oItem.Update();
                Console.WriteLine($"{itemcode} has reverted back to unpurchasable.");
            }

            return result;
        }

        public static int CreateGoodsReceipt(SAP sap, GoodsReceiptItem[] items, int? bplid)
        {
            // Cost price list number
            int priceListNum = 2;

            var gr = (Documents)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);

            if (bplid != null)
                gr.BPL_IDAssignedToInvoice = bplid.Value;

            for (int i = 0; i < items.Length; i++)
            {

                var item = new Item(sap, items[i].ItemCode);

                if (i > 0)
                    gr.Lines.Add();

                gr.Lines.ItemCode = items[i].ItemCode;
                gr.Lines.Quantity = items[i].InvQuantity;
                gr.Lines.WarehouseCode = items[i].ShopWhscode;

                // Cost price
                item.oItem.PriceList.SetCurrentLine(priceListNum);

                gr.Lines.UnitPrice = item.oItem.PriceList.Price;
                gr.Lines.Currency = item.oItem.PriceList.Currency;


                if (item.oItem.ManageBatchNumbers == BoYesNoEnum.tYES)
                {
                    // Batches
                    gr.Lines.BatchNumbers.BatchNumber = DateTime.Today.ToString("yyMMdd") + "GoodsReptKS" + DateTime.Now.Second.ToString("00");
                    gr.Lines.BatchNumbers.Quantity = items[i].InvQuantity;
                }
            }

            return gr.Add();
        }

    }

    public class GoodsReceiptItem
    {
        public string ItemCode { get; set; }
        public double InvQuantity { get; set; }
        public string ShopWhscode { get; set; }
        public double Quantity { get; set; }
        public string UomCode { get; set; }
        public int UomEntry { get; set; }
    }
}
