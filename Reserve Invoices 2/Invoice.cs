using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class Invoice
    {
        public sp_ReserveInvoices_InvoiceLines2_Result[] Invoices;

        public void GetInvoices(int top)
        {
            using (var ctx = new DevEntities())
            {
                Invoices = ctx.sp_ReserveInvoices_InvoiceLines2(top).ToArray();
            }
        }

        public sp_ReserveInvoices_InvoiceLines2_Result[] GetInvoicesByPriceMode(PriceModeDocumentEnum pricemode)
        {
            if (Invoices != null)
            {
                string priceModeCode = null; ;

                switch (pricemode)
                {
                    case PriceModeDocumentEnum.pmdGross:
                        priceModeCode = "G";
                        break;

                    case PriceModeDocumentEnum.pmdNet:
                        priceModeCode = "N";
                        break;

                    case PriceModeDocumentEnum.pmdNetAndGross:
                        Console.Write("pmdNetAndGross not yet implemented.");
                        Console.ReadKey();
                        break;
                }

                if (priceModeCode != null)
                    return Invoices.Where(x => x.PriceMode == priceModeCode).ToArray();
            }

            return null;
        }
    }
}
