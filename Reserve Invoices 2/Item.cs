using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class Item
    {
        public Items oItem;
        public Item(SAP sap, string itemcode)
        {
            oItem = (Items)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
            oItem.GetByKey(itemcode);
        }
    }
}
