using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class Intercompany
    {
        /*
         * This class deals with KIMSON intercompany trasnfers.
         * 
         * 1. Create delivery to Longdan.
         * 2. Items with stock: create Goods Receipt PO in Longdan from Kimson.
         * 3. Items out of stock: Good Receipt to shops in Longdan.
         * 
         */

        static string NewDeliveryDocEntry = null;


        static string LongdanCardCode = "LDCustomer";
        static string WarehouseCode = "F";
        public static int CreateKimsonDelivery(SAP sapks, KimsonDeliveryItem[] ks_items)
        {
            var oCompany = sapks.oCompany;

            Documents delivery = (Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

            delivery.DocObjectCodeEx = "15";        // 15 = Delivery
            delivery.CardCode = LongdanCardCode;
            delivery.DocDate = DateTime.Today;

            for (int i = 0; i < ks_items.Count(); i++)
            {
                if (i > 0)
                    delivery.Lines.Add();

                // Items
                delivery.Lines.ItemCode = ks_items[i].ItemCode;
                delivery.Lines.Quantity = ks_items[i].Quantity;
                delivery.Lines.WarehouseCode = WarehouseCode;
                delivery.Lines.UoMEntry = ks_items[i].UoMEntry;

            }

            int result = delivery.Add();

            if (result == 0)
                NewDeliveryDocEntry = oCompany.GetNewObjectKey();

            return result;
        }

        public class KimsonDeliveryItem
        {
            public string ItemCode { get; set; }
            public double Quantity { get; set; }
            public int UoMEntry { get; set; }
        }
    }

}
