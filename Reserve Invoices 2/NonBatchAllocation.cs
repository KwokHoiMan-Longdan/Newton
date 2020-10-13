using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class NonBatchAllocation
    {
        List<StockCumalationItem> StockCumalation = new List<StockCumalationItem>();

        public double AllocateNonBatchStock(string itemcode, string shopwhscode, double iqty)
        {
            decimal d_iqty = Convert.ToDecimal(iqty);

            using (var ctx = new DevEntities())
            {
                var stock = ctx.sp_SAP_ShopStockAvailability(itemcode, shopwhscode).FirstOrDefault();

                if (stock != null)
                {
                    decimal av_qty = stock.AVAILQTY.Value - Convert.ToDecimal(StockCumalation.Where(x => x.ItemCode == itemcode).Sum(x => x.InvQuantity));

                    if (av_qty >= d_iqty)
                    {
                        var q = Convert.ToDouble(d_iqty);

                        StockCumalation.Add(new StockCumalationItem
                        {
                            ItemCode = itemcode,
                            InvQuantity = q
                        });

                        return q;
                    }
                }

                return 0;
            }
        }

        public class StockCumalationItem
        {
            public string ItemCode { get; set; }
            public double InvQuantity { get; set; }
        }
    }
}
