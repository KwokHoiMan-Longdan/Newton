using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public static class Extensions
    {
        public static int ClearBatches(this Documents doc)
        {
            var lines = doc.Lines;

            for (int i = 0; i < lines.Count; i++)
            {
                lines.SetCurrentLine(i);

                for (int j = 0; j < lines.BatchNumbers.Count; j++)
                {
                    lines.BatchNumbers.SetCurrentLine(j);
                    lines.BatchNumbers.Quantity = 0;
                }

                for (int j = 0; j < lines.BinAllocations.Count; j++)
                {
                    lines.BinAllocations.SetCurrentLine(j);
                    lines.BinAllocations.Quantity = 0;
                }
            }

            return doc.Update();
        }

        public static int ClearBatches(this SAPbobsCOM.StockTransfer doc)
        {
            var lines = doc.Lines;

            for (int i = 0; i < lines.Count; i++)
            {
                lines.SetCurrentLine(i);

                for (int j = 0; j < lines.BatchNumbers.Count; j++)
                {
                    lines.BatchNumbers.SetCurrentLine(j);
                    lines.BatchNumbers.Quantity = 0;
                }

                for (int j = 0; j < lines.BinAllocations.Count; j++)
                {
                    lines.BinAllocations.SetCurrentLine(j);
                    lines.BinAllocations.Quantity = 0;
                }
            }

            return doc.Update();
        }

        public static int AllocateBatchesBins(this Documents doc, SAP sap)
        {
            var allocator = new BatchBinAllocation();
            List<string> frozenItems = new List<string>();
            var lines = doc.Lines;

            for (int i = 0; i < lines.Count; i++)
            {
                lines.SetCurrentLine(i);

                // Item info
                var item = new Item(sap, lines.ItemCode);

                // Unfreeze items
                if (item.oItem.Frozen == BoYesNoEnum.tYES)
                {
                    item.oItem.Frozen = BoYesNoEnum.tNO;
                    if (item.oItem.Update() == 0)
                        frozenItems.Add(lines.ItemCode);
                }

                // Inventory item
                if (item.oItem.InventoryItem == BoYesNoEnum.tYES)
                {

                    // Managed by batches
                    if (item.oItem.ManageBatchNumbers == BoYesNoEnum.tYES)
                    {
                        BatchBinAllocation.BatchAllocationItem[] batchesAvailable;
                        switch (sap.oCompany.CompanyDB)
                        {
                            default: // Longdan
                                batchesAvailable = allocator.AllocateBatches(lines.ItemCode, lines.WarehouseCode, lines.InventoryQuantity)?.ToArray();
                                break;

                            case "KIMSON":
                                batchesAvailable = allocator.AllocateBatches_KS(lines.ItemCode, lines.WarehouseCode, lines.InventoryQuantity)?.ToArray();
                                break;
                        }


                        // If there are batches available
                        if (batchesAvailable != null
                            && batchesAvailable.Count() > 0)
                        {

                            for (int j = 0; j < batchesAvailable.Count(); j++)
                            {
                                if (j > 0)
                                    lines.BatchNumbers.Add();

                                lines.BatchNumbers.BatchNumber = batchesAvailable[j].BatchNumber;
                                lines.BatchNumbers.Quantity = batchesAvailable[j].InvQuantity;
                                lines.BatchNumbers.BaseLineNumber = lines.LineNum;

                                // Bins
                                BatchBinAllocation.BinAllocationItem[] binsAvailable;
                                switch (sap.oCompany.CompanyDB)
                                {
                                    default: // Longdan
                                        binsAvailable = batchesAvailable[j].AllocateBins(allocator.BinCumulation2).ToArray();
                                        break;

                                    case "KIMSON":
                                        binsAvailable = batchesAvailable[j].AllocateBins_KS(allocator.BinCumulation2).ToArray();
                                        break;
                                }


                                for (int k = 0; k < binsAvailable.Count(); k++)
                                {
                                    if (k > 0)
                                        lines.BinAllocations.Add();

                                    lines.BinAllocations.BaseLineNumber = lines.LineNum;
                                    lines.BinAllocations.BinAbsEntry = binsAvailable[k].BinAbsEntry;
                                    lines.BinAllocations.Quantity = binsAvailable[k].InvQuantity;
                                    lines.BinAllocations.SerialAndBatchNumbersBaseLine = j;

                                    // Tracking
                                    allocator.BinCumulation2.Add(new BatchBinAllocation.BinAllocationItem
                                    {
                                        BatchAbsEntry = binsAvailable[k].BatchAbsEntry,
                                        BinAbsEntry = binsAvailable[k].BinAbsEntry,
                                        BinCode = binsAvailable[k].BinCode,
                                        InvQuantity = binsAvailable[k].InvQuantity
                                    });
                                }
                            }
                        }
                    }
                }
            }

            int result = doc.Update();

            // Unfreeze items
            foreach (string itemcode in frozenItems)
            {
                var item = new Item(sap, itemcode);
                item.oItem.Frozen = BoYesNoEnum.tNO;
                item.oItem.Update();
                Console.WriteLine($"{itemcode} has reverted back to frozen.");
            }

            return result;
        }


        public static int AllocateBatchesBins(this SAPbobsCOM.StockTransfer doc, SAP sap)
        {
            var allocator = new BatchBinAllocation();
            List<string> frozenItems = new List<string>();
            var lines = doc.Lines;

            for (int i = 0; i < lines.Count; i++)
            {
                lines.SetCurrentLine(i);

                // Item info
                var item = new Item(sap, lines.ItemCode);

                // Unfreeze items
                if (item.oItem.Frozen == BoYesNoEnum.tYES)
                {
                    item.oItem.Frozen = BoYesNoEnum.tNO;
                    if (item.oItem.Update() == 0)
                        frozenItems.Add(lines.ItemCode);
                }

                // Inventory item
                if (item.oItem.InventoryItem == BoYesNoEnum.tYES)
                {

                    // Managed by batches
                    if (item.oItem.ManageBatchNumbers == BoYesNoEnum.tYES)
                    {
                        BatchBinAllocation.BatchAllocationItem[] batchesAvailable;
                        switch (sap.oCompany.CompanyDB)
                        {
                            default: // Longdan
                                batchesAvailable = allocator.AllocateBatches(lines.ItemCode, lines.FromWarehouseCode, lines.InventoryQuantity)?.ToArray();
                                break;

                            case "KIMSON":
                                batchesAvailable = allocator.AllocateBatches_KS(lines.ItemCode, lines.FromWarehouseCode, lines.InventoryQuantity)?.ToArray();
                                break;
                        }


                        // If there are batches available
                        if (batchesAvailable != null
                            && batchesAvailable.Count() > 0)
                        {

                            for (int j = 0; j < batchesAvailable.Count(); j++)
                            {
                                if (j > 0)
                                    lines.BatchNumbers.Add();

                                lines.BatchNumbers.BatchNumber = batchesAvailable[j].BatchNumber;
                                lines.BatchNumbers.Quantity = batchesAvailable[j].InvQuantity;
                                lines.BatchNumbers.BaseLineNumber = lines.LineNum;

                                // Bins
                                BatchBinAllocation.BinAllocationItem[] binsAvailable;
                                switch (sap.oCompany.CompanyDB)
                                {
                                    default: // Longdan
                                        binsAvailable = batchesAvailable[j].AllocateBins(allocator.BinCumulation2).ToArray();
                                        break;

                                    case "KIMSON":
                                        binsAvailable = batchesAvailable[j].AllocateBins_KS(allocator.BinCumulation2).ToArray();
                                        break;
                                }


                                for (int k = 0; k < binsAvailable.Count(); k++)
                                {
                                    if (k > 0)
                                        lines.BinAllocations.Add();

                                    // From location
                                    lines.BinAllocations.BaseLineNumber = lines.LineNum;
                                    lines.BinAllocations.BinAbsEntry = binsAvailable[k].BinAbsEntry;
                                    lines.BinAllocations.Quantity = binsAvailable[k].InvQuantity;
                                    lines.BinAllocations.SerialAndBatchNumbersBaseLine = j;
                                    lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;

                                    // Tracking
                                    allocator.BinCumulation2.Add(new BatchBinAllocation.BinAllocationItem
                                    {
                                        BatchAbsEntry = binsAvailable[k].BatchAbsEntry,
                                        BinAbsEntry = binsAvailable[k].BinAbsEntry,
                                        BinCode = binsAvailable[k].BinCode,
                                        InvQuantity = binsAvailable[k].InvQuantity
                                    });
                                }

                                // To location
                                lines.BinAllocations.Add();
                                lines.BinAllocations.BaseLineNumber = lines.LineNum;
                                lines.BinAllocations.BinAbsEntry = StockInfo.GetSysBinAbsByWhscode(sap, lines.WarehouseCode);
                                lines.BinAllocations.Quantity = batchesAvailable[j].InvQuantity;
                                lines.BinAllocations.SerialAndBatchNumbersBaseLine = j;
                                lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
                            }
                        }
                    }
                }
            }

            int result = doc.Update();

            // Unfreeze items
            foreach (string itemcode in frozenItems)
            {
                var item = new Item(sap, itemcode);
                item.oItem.Frozen = BoYesNoEnum.tNO;
                item.oItem.Update();
                Console.WriteLine($"{itemcode} has reverted back to frozen.");
            }

            return result;
        }

        public static double TotalBatchQuantity(this BatchNumbers batches)
        {
            double batchQty = 0;

            for (int x = 0; x < batches.Count; x++)
            {
                batches.SetCurrentLine(x);
                batchQty += batches.Quantity;
            }

            return batchQty;
        }
    }
}
