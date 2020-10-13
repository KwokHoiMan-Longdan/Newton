using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class BatchBinAllocation
    {
        //SAP sap_;
        List<BatchAllocationItem> BatchCumulation = new List<BatchAllocationItem>();
        public List<BinAllocationItem> BinCumulation2 = new List<BinAllocationItem>();
        //public BatchBinAllocation(SAP sap)
        //{
        //    sap_ = sap;
        //}
        public IEnumerable<BatchAllocationItem> AllocateBatches(string itemcode, string whscode, double iqty)
        {

            //decimal diqty = Convert.ToDecimal(iqty);
            using (var ctx = new DevEntities())
            {
                var av_batches = ctx.sp_SAP_BatchAvailability(itemcode, whscode).ToArray();

                var av_totalqty = Convert.ToDouble(av_batches.Sum(x => x.get_avail_res_qty_col_alias.Value - x.get_avail_res_commit_qty_col_alias));


                if (av_totalqty >= iqty)
                {
                    // Batch enough for requested qty

                    var alc_batches = new List<BatchAllocationItem>();
                    double remain_qty = iqty;

                    foreach (var b in av_batches)
                    {
                        var bat_qty = Convert.ToDouble((b.get_avail_res_qty_col_alias.Value - b.get_avail_res_commit_qty_col_alias));

                        var bat_cum_qty = bat_qty - BatchCumulation.Where(x => x.AbsEntry == b.absentry).Sum(x => x.InvQuantity);

                        if (remain_qty > 0 && bat_cum_qty > 0)
                        {
                            if (remain_qty >= bat_cum_qty)
                            {
                                // Remaining more than batch qty

                                var alc_item = new BatchAllocationItem();
                                alc_item.ItemCode = b.itemcode;
                                alc_item.AbsEntry = b.absentry;
                                alc_item.BatchNumber = b.distnumber;
                                alc_item.InvQuantity = bat_cum_qty;
                                alc_item.WhsCode = whscode;

                                remain_qty = remain_qty - bat_cum_qty; // update tracking

                                alc_batches.Add(alc_item);
                            }
                            else
                            {
                                // Remaining more than batch qty batch_qty > remain_qty

                                var alc_item = new BatchAllocationItem();
                                alc_item.ItemCode = b.itemcode;
                                alc_item.AbsEntry = b.absentry;
                                alc_item.BatchNumber = b.distnumber;
                                alc_item.InvQuantity = remain_qty;
                                alc_item.WhsCode = whscode;

                                remain_qty = 0;

                                alc_batches.Add(alc_item);
                            }
                        }
                    }

                    // Store batch history
                    BatchCumulation.AddRange(alc_batches);

                    return alc_batches;
                }
                else

                    // Not enough batch qty
                    // Check other warehouses 
                    return null;
            }
        }

        public IEnumerable<BatchAllocationItem> AllocateBatches_KS(string itemcode, string whscode, double iqty)
        {
            decimal diqty = Convert.ToDecimal(iqty);
            using (var ctx = new DevEntities())
            {
                var av_batches = ctx.sp_SAP_BatchAvailability_KS(itemcode, whscode).ToArray();

                var av_totalqty = Convert.ToDouble(av_batches.Sum(x => x.get_avail_res_qty_col_alias - x.get_avail_res_commit_qty_col_alias));

                if (av_totalqty >= iqty)
                {
                    // Batch enough for requested qty

                    var alc_batches = new List<BatchAllocationItem>();
                    double remain_qty = iqty;

                    foreach (var b in av_batches)
                    {
                        var bat_qty = Convert.ToDouble((b.get_avail_res_qty_col_alias - b.get_avail_res_commit_qty_col_alias));

                        var bat_cum_qty = bat_qty - BatchCumulation.Where(x => x.AbsEntry == b.absentry).Sum(x => x.InvQuantity);

                        if (remain_qty > 0 && bat_cum_qty > 0)
                        {
                            if (remain_qty >= bat_cum_qty)
                            {
                                // Remaining more than batch qty

                                var alc_item = new BatchAllocationItem();
                                alc_item.ItemCode = b.itemcode;
                                alc_item.AbsEntry = b.absentry;
                                alc_item.BatchNumber = b.distnumber;
                                alc_item.InvQuantity = bat_cum_qty;
                                alc_item.WhsCode = whscode;

                                remain_qty = remain_qty - bat_cum_qty; // update tracking

                                alc_batches.Add(alc_item);
                            }
                            else
                            {
                                // Remaining more than abtch qty batch_qty > remain_qty

                                var alc_item = new BatchAllocationItem();
                                alc_item.ItemCode = b.itemcode;
                                alc_item.AbsEntry = b.absentry;
                                alc_item.BatchNumber = b.distnumber;
                                alc_item.InvQuantity = remain_qty;
                                alc_item.WhsCode = whscode;

                                remain_qty = 0;

                                alc_batches.Add(alc_item);
                            }
                        }
                    }

                    // Store batch history
                    BatchCumulation.AddRange(alc_batches);

                    return alc_batches;
                }
                else

                    // Not enough batch qty
                    // Check other warehouses 
                    return null;
            }
        }

        public sp_SAP_WarehouseStockAvailability_Result WarehouseBatches(string itemcode)
        {
            using (var ctx = new DevEntities())
            {
                return ctx.sp_SAP_WarehouseStockAvailability(itemcode).FirstOrDefault();
            }
        }





        public class BatchAllocationItem
        {
            public string ItemCode { get; set; }
            public string BatchNumber { get; set; }
            public int AbsEntry { get; set; }               // batch number absentry
            public double InvQuantity { get; set; }
            public string WhsCode { get; set; }
            public IEnumerable<BinAllocationItem> AllocateBins(List<BinAllocationItem> acc_bins)
            {
                using (var ctx = new DevEntities())
                {
                    var av_bins = ctx.sp_SAP_BinAvailability(AbsEntry, WhsCode).ToArray();

                    // Batch enough for requested qty

                    var alc_bins = new List<BinAllocationItem>();
                    double remain_qty = InvQuantity;

                    foreach (var b in av_bins)
                    {
                        var bin_qty = Convert.ToDouble(b.OnHandQty.Value);

                        var bin_cum_qty = bin_qty - acc_bins.Where(x => x.BinAbsEntry == b.AbsEntry && x.BatchAbsEntry == b.SnBMDAbs).Sum(x => x.InvQuantity);

                        if (remain_qty > 0 && bin_cum_qty > 0)
                        {
                            if (remain_qty >= bin_cum_qty)
                            {
                                // Remaining more than batch qty

                                var alc_item = new BinAllocationItem();
                                alc_item.BatchAbsEntry = b.SnBMDAbs;
                                alc_item.BinAbsEntry = b.AbsEntry;
                                alc_item.BinCode = b.BinCode;
                                alc_item.InvQuantity = bin_cum_qty;

                                remain_qty = remain_qty - bin_cum_qty; // update tracking

                                alc_bins.Add(alc_item);
                            }
                            else
                            {
                                // Remaining more than abtch qty batch_qty > remain_qty

                                var alc_item = new BinAllocationItem();
                                alc_item.BatchAbsEntry = b.SnBMDAbs;
                                alc_item.BinAbsEntry = b.AbsEntry;
                                alc_item.BinCode = b.BinCode;
                                alc_item.InvQuantity = remain_qty;

                                remain_qty = 0;

                                alc_bins.Add(alc_item);
                            }
                        }
                    }

                    return alc_bins;
                }
            }

            public IEnumerable<BinAllocationItem> AllocateBins_KS(List<BinAllocationItem> acc_bins)
            {
                using (var ctx = new DevEntities())
                {
                    var av_bins = ctx.sp_SAP_BinAvailability_KS(AbsEntry, WhsCode).ToArray();

                    // Batch enough for requested qty

                    var alc_bins = new List<BinAllocationItem>();
                    double remain_qty = InvQuantity;

                    foreach (var b in av_bins)
                    {
                        var bin_qty = Convert.ToDouble(b.OnHandQty.Value);

                        var bin_cum_qty = bin_qty - acc_bins.Where(x => x.BinAbsEntry == b.AbsEntry && x.BatchAbsEntry == b.SnBMDAbs).Sum(x => x.InvQuantity);

                        if (remain_qty > 0 && bin_cum_qty > 0)
                        {
                            if (remain_qty >= bin_cum_qty)
                            {
                                // Remaining more than batch qty

                                var alc_item = new BinAllocationItem();
                                alc_item.BatchAbsEntry = b.SnBMDAbs;
                                alc_item.BinAbsEntry = b.AbsEntry;
                                alc_item.BinCode = b.BinCode;
                                alc_item.InvQuantity = bin_cum_qty;

                                remain_qty = remain_qty - bin_cum_qty; // update tracking

                                alc_bins.Add(alc_item);
                            }
                            else
                            {
                                // Remaining more than abtch qty batch_qty > remain_qty

                                var alc_item = new BinAllocationItem();
                                alc_item.BatchAbsEntry = b.SnBMDAbs;
                                alc_item.BinAbsEntry = b.AbsEntry;
                                alc_item.BinCode = b.BinCode;
                                alc_item.InvQuantity = remain_qty;

                                remain_qty = 0;

                                alc_bins.Add(alc_item);
                            }
                        }
                    }

                    return alc_bins;
                }
            }

        }

        public class BinAllocationItem
        {
            public int BinAbsEntry { get; set; }
            public string BinCode { get; set; }
            public int BatchAbsEntry { get; set; }
            public double InvQuantity { get; set; }
        }
    }

}
