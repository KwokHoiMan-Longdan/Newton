using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class StockInfo
    {
        public static int GetSysBinAbsByWhscode(SAP sap, string whscode)
        {
            var recordSet = (Recordset)sap.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordSet.DoQuery($"SELECT AbsEntry FROM [LONGDAN-Newton].DBO.OBIN WHERE SYSBIN = 'Y' AND WhsCode = '{whscode}'");

            return Convert.ToInt32(recordSet.Fields.Item(0).Value);
        }

        public static string IsItemLongdanKimson(SAP sap, string itemcode)
        {
            var item = (Items)sap.oCompany.GetBusinessObject(BoObjectTypes.oItems);

            var result = item.GetByKey(itemcode);

            if (result == true)
            {
                if (item.Properties[1] == BoYesNoEnum.tYES && item.Properties[2] == BoYesNoEnum.tNO)
                    return "KIMSON";

                if (item.Properties[1] == BoYesNoEnum.tNO && item.Properties[2] == BoYesNoEnum.tYES)
                    return "LONGDAN";
            }

            return null;
        }

        public static sp_SAP_WarehouseStockAvailability_Result WarehouseStockCheck(string itemcode, decimal invquantity)
        {
            using (var ctx = new DevEntities())
            {
                var fromwhs = ctx.sp_SAP_WarehouseStockAvailability(itemcode)
                            .Where(x => x.AVAILQTY.Value >= invquantity)
                            .OrderBy(x => x.PRIORITY)
                            .FirstOrDefault();

                return fromwhs;
            }
        }


        // Warehousing
        public static int? GetBPLIdFromWhscode(string shopwhscode)
        {
            using(var ctx = new DevEntities())
            {
                var whscodeObj = ctx.sp_SAP_GetBPLIdFromWhscode(shopwhscode).FirstOrDefault();

                return whscodeObj?.BPLid;
            }
        }


        public static string GetShopCardcodeByWhscode(string shopWhscode)
        {
            // Inter-branch transfer whscode to cardcode mappings

            switch (shopWhscode)
            {

                // BPLId != 1  (if 1 then no need for inter-branch transfers)

                case "CT":                  // Shop whscode
                    return "LDCT";          // Shop cardcode 
                
                case "EC":
                    return "LDEC";

                case "HX":
                    return "LDHX";

                case "KD":
                    return "LDK";

                case "PBCT":
                    return "PB3";

                case "PBEC":
                    return "PB2";

                case "SD":
                    return "LDSD";

                default: return shopWhscode;
            }
        }
    }
}
