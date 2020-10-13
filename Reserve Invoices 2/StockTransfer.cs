using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class StockTransfer
    {
        public SAP sap;
        public static Form1 form;

        public SAPbobsCOM.StockTransfer oDraft;
        public static int CreateDraft(SAP sap, StockTransferItem[] items)
        {

            var st = (SAPbobsCOM.StockTransfer)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransferDraft);
            st.DocObjectCode = BoObjectTypes.oStockTransfer;

            //st.DocObjectCodeEx = "67";        // 67 = Stock transfer
            st.DocDate = DateTime.Today;
            st.FromWarehouse = items[0].FromWarehouse;
            st.ToWarehouse = items[0].ToWarehouse;
            //st.BPL_IDAssignedToInvoice = 1;

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

            int result = st.Add();
            return result;
        }

        public bool LoadDraft(SAP sap, int docentry)
        {
            this.sap = sap;
            oDraft = (SAPbobsCOM.StockTransfer)sap.oCompany.GetBusinessObject(BoObjectTypes.oStockTransferDraft);
            return oDraft.GetByKey(docentry);
        }

        public int ProcessDraft2Regular()
        {
            return oDraft.SaveDraftToDocument();
        }
    }




    public class StockTransferItem
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public double InvQuantity { get; set; }
        public string FromWarehouse { get; set; }
        public string ToWarehouse { get; set; }
        public int InvUomEntry { get; set; }

        public bool? IsSameBranch
        {
            get
            {
                var fromBPLId = StockInfo.GetBPLIdFromWhscode(FromWarehouse);
                var toBPLId = StockInfo.GetBPLIdFromWhscode(ToWarehouse);

                if (fromBPLId != null && toBPLId != null)
                {
                    return fromBPLId == toBPLId;
                }
                else
                    return null;
            }
        }

        public int? FromBPLId
        {
            get
            {
                var fromBPLId = StockInfo.GetBPLIdFromWhscode(FromWarehouse);

                if (fromBPLId != null)
                {
                    return fromBPLId;
                }
                else
                    return null;
            }
        }

        public int? ToBPLId
        {
            get
            {
                var toBPLId = StockInfo.GetBPLIdFromWhscode(ToWarehouse);

                if (toBPLId != null)
                {
                    return toBPLId;
                }
                else
                    return null;
            }
        }

    }
}
