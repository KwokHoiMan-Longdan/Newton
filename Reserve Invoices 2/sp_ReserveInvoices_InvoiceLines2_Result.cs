//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Reserve_Invoices_2
{
    using System;
    
    public partial class sp_ReserveInvoices_InvoiceLines2_Result
    {
        public string COMPANY { get; set; }
        public int DocEntry { get; set; }
        public Nullable<int> DocNum { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public int LineNum { get; set; }
        public Nullable<int> TargetType { get; set; }
        public Nullable<int> TrgetEntry { get; set; }
        public string LineStatus { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> InvQty { get; set; }
        public string UomCode { get; set; }
        public string InvntryUom { get; set; }
        public Nullable<int> UomEntry { get; set; }
        public Nullable<int> IUoMEntry { get; set; }
        public string WhsCode { get; set; }
        public string BaseCard { get; set; }
        public string ManBtchNum { get; set; }
        public string PriceMode { get; set; }
        public Nullable<decimal> NetPrice { get; set; }
        public Nullable<decimal> GrossPrice { get; set; }
        public Nullable<int> BPLId { get; set; }
        public Nullable<decimal> OpenQty { get; set; }
        public Nullable<decimal> OpenInvQty { get; set; }
    }
}
