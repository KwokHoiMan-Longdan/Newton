using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Reserve_Invoices_2
{
    public partial class Form1 : Form
    {
        // -----------------------------------
        // Props
        // -----------------------------------
        public Form2 SimpleForm;

        private SAP[] Companies;
        private SAP Sap_Ld => Companies.SingleOrDefault(x => x.oCompany.CompanyDB == "LONGDAN");
        private SAP Sap_Ks => Companies.SingleOrDefault(x => x.oCompany.CompanyDB == "KIMSON");

        private Invoice Invoice = new Invoice();

        private Delivery Delivery = new Delivery();

        private StockTransfer StockTransfer = new StockTransfer();

        // -----------------------------------
        // Methods
        // -----------------------------------

        public Form1()
        {
            InitializeComponent();
            btnConnect_Click(null, null);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine("Connect companies...");


            // Array of specified companies
            string[] companies = SAP.Companies;

            // Initiate form property array
            Companies = new SAP[companies.Length];
            int c = 0;

            for (int i = 0; i < companies.Length; i++)
            {
                Companies[i] = new SAP(companies[i]);

                // Console
                if (Companies[i].oCompany.Connected)
                {
                    Console.WriteLine($"{Companies[i].oCompany.CompanyDB} connected.");
                    c++;
                }
            }


            // Console
            Console.WriteLine($"Connected to {c} companies." + Environment.NewLine);
        }

        private void btnGetInvoices_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(A) Getting invoices...");


            int top = int.Parse(txtTop.Text);
            Invoice.GetInvoices(top);

            Delivery.DeliveryDraftsTaskList.Clear();

            // Console
            Console.WriteLine($"{Invoice.Invoices.Length} invoices loaded (TOP = {top})." + Environment.NewLine);
        }

        private void btnCreateDrafts_Click(object sender, EventArgs e)
        {
            // Gross invoices
            Console.WriteLine($"(B) Creating drafts for gross price mode...");
            var result = Delivery.CreateDraft(Sap_Ld, Invoice, PriceModeDocumentEnum.pmdGross);

            // Console
            Console.WriteLine($"Drafts have been created." + Environment.NewLine);


            // Net invoices
            Console.WriteLine($"Creating drafts for net price mode...");
            result = Delivery.CreateDraft(Sap_Ld, Invoice, PriceModeDocumentEnum.pmdNet);


            txtTaskList.Text = string.Join(",", Delivery.DeliveryDraftsTaskList);



            // Console
            Console.WriteLine($"Drafts have been created." + Environment.NewLine);
        }

        private void btnLoadDraft_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(C) Load draft delivery...");

            Delivery = new Delivery();

            int docentry = int.Parse(txtDraft.Text);
            bool result = Delivery.LoadDraft(Sap_Ld, docentry);


            // Console
            if (result)
                Console.WriteLine($"Draft delivery {docentry} loaded." + Environment.NewLine);
            else
                Console.WriteLine($"Error: " + Sap_Ld.oCompany.GetLastErrorDescription() + Environment.NewLine);
        }

        private void btnClearBatchesDraft_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(D) Clearing batches...");
            int? result = null;



            if (Delivery != null)
                result = Delivery.oDraft.ClearBatches();



            // Console
            string resultMessage = null;

            switch (result)
            {
                case null:
                    resultMessage = "Delivery empty.";
                    break;

                case 0:
                    resultMessage = "Success.";
                    break;

                default:
                    resultMessage = "Error: " + Sap_Ld.oCompany.GetLastErrorDescription();
                    break;
            }

            Console.WriteLine($"{resultMessage}" + Environment.NewLine);

        }

        private void btnAllocateBatchesDraft_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(E) Allocating batches...");
            int? result = null;


            if (Delivery != null)
                result = Delivery.oDraft.AllocateBatchesBins(Sap_Ld);



            // Console
            if (result == 0)
                Console.WriteLine($"Batches has been allocated." + Environment.NewLine);
            else
                Console.WriteLine($"Error: " + Sap_Ld.oCompany.GetLastErrorDescription() + Environment.NewLine);
        }

        private void btnSortAction_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(F) Sorting draft delivery items...");


            Delivery.SortDraftItems();


            // Console
            Console.WriteLine($"Allocated: " + Delivery.AllocatedLineNums.Count);
            Console.WriteLine($"Unallocated: " + Delivery.ZeroBatchLineNums.Count);
            Console.WriteLine($"Non-batch managed: " + Delivery.NonBatchLineNums.Count);
            Console.WriteLine($"Non-inventory: " + Delivery.NonInvItemLineNums.Count);
            Console.WriteLine($"Draft delivery items have been sorted." + Environment.NewLine);
        }

        private void btnSortUnallocated_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(G) Sorting unallocated items...");


            if (Delivery.ZeroBatchLineNums.Count > 0)
            {

                // Sort
                Delivery.SortUnallocatedItems(Sap_Ld);


                // Console

                Console.WriteLine($"WarehouseTransferItems (Inter-branch delivery/GRPO): {Delivery.WarehouseTransferItems.Count}");
                foreach (var line in Delivery.WarehouseTransferItems)
                    Console.WriteLine($"{line.FromWarehouse.PadRight(5)} --> {line.ToWarehouse.PadRight(5)} : #{line.LineNum} {line.ItemCode} x {line.InvUomEntry} ({line.InvUomEntry}). Same branch = {line.IsSameBranch}, {line.FromBPLId} => {line.ToBPLId}");

                Console.WriteLine($"GoodsReceiptItems (Direct goods in): {Delivery.GoodsReceiptItems.Count}");
                foreach (var line in Delivery.GoodsReceiptItems)
                    Console.WriteLine($"{line.ItemCode.PadRight(9)} x{line.InvQuantity} ({line.UomEntry}) [x{line.Quantity} {line.UomCode}] ---> {line.ShopWhscode}");

                Console.WriteLine($"Kimson Items: {Delivery.KimsonDeliveryItems.Count}");
                foreach (var line in Delivery.KimsonDeliveryItems)
                    Console.WriteLine($"{line.ItemCode} x{line.Quantity} ({line.UoMEntry})");

                Console.WriteLine($"Unallocated items have been sorted." + Environment.NewLine);
            }
            else
            {


                // Console
                Console.WriteLine($"There are no unallocated items to sort." + Environment.NewLine);
            }
        }

        private void btnCreateInterBranchDraft_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(I) Creating inter-branch draft delivery for unallocated items...");


            string result = Delivery.CreateDraft();

            // Console
            Console.WriteLine($"{result}" + Environment.NewLine);
        }

        private void btnDraft2Regular_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(L) Copy draft delivery to regular delivery...");


            int result = Delivery.ProcessDraft2Regular();


            // Console
            if (result == 0)
                Console.WriteLine($"Copy successful." + Environment.NewLine);
            else
                Console.WriteLine($"Error: {Delivery.sap.oCompany.GetLastErrorDescription()}" + Environment.NewLine);
        }

        private void btnInterBranchGRPO_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(J) Creating inter-branch GRPO for unallocated items...");

            GoodsReceiptItem[] goodsItems = Delivery.CopyDeliveryXferItems2GRPO();

            if (goodsItems.Length > 0)
            {
                string cardcode = "LDHQS";
                DateTime docdate = Delivery.oDraft.DocDate;
                int bplid = Delivery.oDraft.BPL_IDAssignedToInvoice;
                //GoodsReceiptItem[] goodsItems = Delivery.CopyDeliveryXferItems2GRPO();

                int result = GoodsReceipt.CreateGRPO(Sap_Ld, goodsItems, cardcode, docdate, bplid);


                // Console
                if (result == 0)
                    Console.WriteLine($"Inter-branch GRPO created successfully." + Environment.NewLine);
                else
                    Console.WriteLine($"Error: {Sap_Ld.oCompany.GetLastErrorDescription()}" + Environment.NewLine);
            }
            else
                Console.WriteLine($"No Inter-branch GRPO needed.");
        }

        private void btnDirectGR_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(K) Creating direct goods-in to shop...");


            if (Delivery.GoodsReceiptItems.Count > 0)
            {

                int bplid = Delivery.oDraft.BPL_IDAssignedToInvoice;
                var result = GoodsReceipt.CreateGoodsReceipt(Sap_Ld, Delivery.GoodsReceiptItems.ToArray(), bplid);


                // Console
                if (result != 0)
                    Console.WriteLine($"Error: {Sap_Ld.oCompany.GetLastErrorDescription()}" + Environment.NewLine);
                else
                    Console.WriteLine($"Success: {Sap_Ld.oCompany.GetNewObjectKey()}" + Environment.NewLine);
            }
            else
                Console.WriteLine($"No items for direct goods receipt.");
        }

        private void btnNonBatchStockCheck_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(H) Checking non-batch item stock...");


            if (Delivery.NonBatchLineNums.Count > 0)
            {
                Delivery.SortNonBatchItems(Sap_Ld);

                // Console
                Console.WriteLine($"Non-batch items have been sorted." + Environment.NewLine);
            }
            else
                Console.WriteLine($"There are no non-batch items to be sorted." + Environment.NewLine);


        }


        private void btnKimsonImport_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine("(M) Creating Kimson delivery...");


            string result = Delivery.CreateDraftKimson(Sap_Ks);


            // Console
            Console.WriteLine($"{result}");
        }

        private void btnLoadDraftKS_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(N) Load draft delivery...");

            Delivery = new Delivery();

            int docentry = int.Parse(txtDraftKS.Text);
            bool result = Delivery.LoadDraft(Sap_Ks, docentry);


            // Console
            if (result)
                Console.WriteLine($"Draft delivery {docentry} loaded." + Environment.NewLine);
            else
                Console.WriteLine($"Error: " + Sap_Ld.oCompany.GetLastErrorDescription() + Environment.NewLine);
        }

        private void btnClearBatchesDraftKS_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(O) Clearing batches...");
            int? result = null;



            if (Delivery != null)
                result = Delivery.oDraft.ClearBatches();



            // Console
            string resultMessage = null;

            switch (result)
            {
                case null:
                    resultMessage = "Delivery empty.";
                    break;

                case 0:
                    resultMessage = "Success.";
                    break;

                default:
                    resultMessage = "Error: " + Sap_Ks.oCompany.GetLastErrorDescription();
                    break;
            }

            Console.WriteLine($"{resultMessage}" + Environment.NewLine);
        }

        private void btnAllocateBatchesDraftKS_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(P) Allocating batches...");
            int? result = null;


            if (Delivery != null)
                result = Delivery.oDraft.AllocateBatchesBins(Sap_Ks);



            // Console
            if (result == 0)
                Console.WriteLine($"Batches has been allocated." + Environment.NewLine);
            else
                Console.WriteLine($"Error: " + Delivery.sap.oCompany.GetLastErrorDescription() + Environment.NewLine);
        }

        private void btnSortActionKS_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(Q) Sorting draft delivery items...");


            Delivery.SortDraftItems();


            // Console
            Console.WriteLine($"Allocated: " + Delivery.AllocatedLineNums.Count);
            Console.WriteLine($"Unallocated: " + Delivery.ZeroBatchLineNums.Count);
            Console.WriteLine($"Non-batch managed: " + Delivery.NonBatchLineNums.Count);
            Console.WriteLine($"Non-inventory: " + Delivery.NonInvItemLineNums.Count);
            Console.WriteLine($"Draft delivery items have been sorted." + Environment.NewLine);
        }

        private void btnSortUnallocatedKS_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"Sorting unallocated items...");


            if (Delivery.ZeroBatchLineNums.Count > 0)
            {

                // Sort
                Delivery.SortUnallocatedItems(Sap_Ks);


                // Console

                Console.WriteLine($"WarehouseTransferItems (Inter-branch delivery/GRPO): {Delivery.WarehouseTransferItems.Count}");
                foreach (var line in Delivery.WarehouseTransferItems)
                    Console.WriteLine($"{line.FromWarehouse.PadRight(5)} --> {line.ToWarehouse.PadRight(5)} : #{line.LineNum} {line.ItemCode} x {line.InvUomEntry} ({line.InvUomEntry}). Same branch = {line.IsSameBranch}, {line.FromBPLId} => {line.ToBPLId}");

                Console.WriteLine($"GoodsReceiptItems (Direct goods in): {Delivery.GoodsReceiptItems.Count}");
                foreach (var line in Delivery.GoodsReceiptItems)
                    Console.WriteLine($"{line.ItemCode.PadRight(9)} x{line.InvQuantity} ({line.UomEntry}) [x{line.Quantity} {line.UomCode}] ---> {line.ShopWhscode}");

                Console.WriteLine($"Kimson Items: {Delivery.KimsonDeliveryItems.Count}");
                foreach (var line in Delivery.KimsonDeliveryItems)
                    Console.WriteLine($"{line.ItemCode} x{line.Quantity} ({line.UoMEntry})");

                Console.WriteLine($"Unallocated items have been sorted." + Environment.NewLine);
            }
            else
            {


                // Console
                Console.WriteLine($"There are no unallocated items to sort." + Environment.NewLine);
            }
        }

        private void btnDirectGRKS_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(R) Creating direct goods-in to freezer...");

            if (Delivery.ZeroBatchLineNums.Count > 0)
            {

                var grItems = Delivery.CreateGoodsReceiptItemsFromZeroBatchLineNums();

                var result = GoodsReceipt.CreateGoodsReceipt(Sap_Ks, grItems, null);


                // Console
                if (result != 0)
                    Console.WriteLine($"Error: {Sap_Ld.oCompany.GetLastErrorDescription()}" + Environment.NewLine);
                else
                    Console.WriteLine($"Success: {Sap_Ld.oCompany.GetNewObjectKey()}" + Environment.NewLine);
            }
            else
                Console.WriteLine($"No items for direct goods receipt.");
        }

        private void btnLongdanDelivery_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(S) Copying delivery to Longdan...");


            int result = Delivery.ProcessDraft2Regular();


            // Console
            if (result == 0)
                Console.WriteLine($"Copy successful. Key: {Delivery.sap.oCompany.GetNewObjectKey()}" + Environment.NewLine);
            else
                Console.WriteLine($"Error: {Delivery.sap.oCompany.GetLastErrorDescription()}" + Environment.NewLine);
        }

        private void btnLoadKimsonDelivery_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(T) Loading Kimson delivery note items...");


            bool result = Delivery.LoadKSDeliveryNote(Sap_Ks, int.Parse(txtKimsonDelivery.Text));



            // Console
            if (result == false)
                Console.WriteLine($"Error: cannot load delivery {txtKimsonDelivery.Text}.");
            else
                Console.WriteLine($"OK");
        }

        private void btnLongdanGRPO_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(U) Creating Longdan GRPO for Kimson items...");


            int result = Delivery.KSDeliveryNote2LDGRPO();


            // Console
            if (result != 0)
                Console.WriteLine($"Error: {Delivery.sap.oCompany.GetLastErrorDescription()}.");
            else
                Console.WriteLine($"OK");
        }


        //
        // Automation
        //

        int loop = 0;
        string draftKey = null;
        DateTime timer;
        private void btnAutomated_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine("Automation started" + Environment.NewLine);


            if (Delivery.CountOpenedDrafts() > 10)
            {
                Console.WriteLine("Too many opened draft deliveries. This indicate that there is a problem with the processing.");
                return;
            }

            string[] draftTaskList = null;



            //if (loop == 0)
            //{
            timer = DateTime.Now;


            // Get Invoices
            btnGetInvoices_Click(null, null);

            // Create drafts
            btnCreateDrafts_Click(null, null);

            //draftKey = Sap_Ld.oCompany.GetNewObjectKey();
            draftTaskList = txtTaskList.Text.Split(',');
            //}

            foreach (var draftKey in draftTaskList)
            {
                MainLoop(draftKey);

                //Console.WriteLine("Processing " + draftKey + "...");

                //txtDraft.Text = draftKey;

                //// Load draft
                //btnLoadDraft_Click(null, null);

                //if (loop != 0)
                //    btnClearBatchesDraft_Click(null, null);

                //// Allocate batches
                //btnAllocateBatchesDraft_Click(null, null);

                //// Filter item actions
                //btnSortAction_Click(null, null);

                //// Any non-batch items?
                //if (Delivery.NonBatchLineNums.Count > 0)
                //{
                //    // Check stock in shop and warehouse
                //    btnNonBatchStockCheck_Click(null, null);
                //}

                //// Any unallocated items?
                //if (Delivery.ZeroBatchLineNums.Count > 0)
                //{
                //    btnSortUnallocated_Click(null, null);
                //}

                //// Any Kimson items
                //if (Delivery.KimsonDeliveryItems.Count > 0)
                //{
                //    btnKimsonImport_Click(null, null);
                //    string draftKeyKs = Sap_Ks.oCompany.GetNewObjectKey();
                //    txtDraftKS.Text = draftKeyKs;

                //    // Load draft
                //    btnLoadDraftKS_Click(null, null);

                //    // Allocate batches
                //    btnAllocateBatchesDraftKS_Click(null, null);

                //    // Filter
                //    btnSortActionKS_Click(null, null);

                //    // Any direct goods
                //    if (Delivery.ZeroBatchLineNums.Count > 0)
                //    {
                //        btnDirectGRKS_Click(null, null);


                //        // Load draft
                //        btnLoadDraftKS_Click(null, null);

                //        // Clear batches
                //        btnClearBatchesDraftKS_Click(null, null);

                //        // Allocate batches
                //        btnAllocateBatchesDraftKS_Click(null, null);

                //        // Filter
                //        btnSortActionKS_Click(null, null);
                //    }

                //    if (Delivery.ZeroBatchLineNums.Count == 0)
                //    {
                //        // Delivery to Longdan
                //        btnLongdanDelivery_Click(null, null);

                //        // GRPO from Kimson
                //        string DeliveryKeyKS = Sap_Ks.oCompany.GetNewObjectKey();
                //        txtKimsonDelivery.Text = DeliveryKeyKS;


                //        // Load original delivery
                //        btnLoadDraft_Click(null, null);

                //        btnLoadKimsonDelivery_Click(null, null);
                //        btnLongdanGRPO_Click(null, null);
                //    }

                //    Console.WriteLine("Repeat loop");

                //    loop = 1;

                //    btnAutomated_Click(null, null);

                //    return;
                //}





                //// Any direct goods ins
                //if (Delivery.GoodsReceiptItems.Count > 0)
                //{
                //    btnDirectGR_Click(null, null);
                //}


                //// Any warehouse transfer items?
                //if (Delivery.WarehouseTransferItems.Count > 0)
                //{

                //    // Stock transfer

                //    btnCreateSTDraft_Click(null, null);

                //    if (Delivery.STDraftDocEntry.HasValue == true)
                //    {
                //        // Clear 
                //        txtSTDraft.Text = Delivery.STDraftDocEntry.Value.ToString();
                //        btnLoadSTDraft_Click(null, null);

                //        btnAllocateBatchesST_Click(null, null);

                //        btnDraft2RegularST_Click(null, null);
                //    }


                //    // Delivery

                //    btnCreateInterBranchDraft_Click(null, null);
                //    //string draftKeyInterco = Sap_Ld.oCompany.GetNewObjectKey();
                //    string draftKeyInterco = Delivery.DraftDocEntry.ToString();

                //    if (string.IsNullOrEmpty(draftKeyInterco) == false)
                //    {
                //        // GRPO
                //        btnInterBranchGRPO_Click(null, null);

                //        // Clear the interco delivery
                //        txtDraft.Text = draftKeyInterco;

                //        // Load draft
                //        btnLoadDraft_Click(null, null);

                //        // Allocate batches
                //        btnAllocateBatchesDraft_Click(null, null);

                //        // To regular
                //        btnDraft2Regular_Click(null, null);
                //    }






                //    // Load original reserve invoice delivery
                //    txtDraft.Text = draftKey;

                //    // Load draft
                //    btnLoadDraft_Click(null, null);

                //    // Clear batches
                //    btnClearBatchesDraft_Click(null, null);

                //    // Allocate batches
                //    btnAllocateBatchesDraft_Click(null, null);

                //    // Filter item actions
                //    btnSortAction_Click(null, null);

                //    // Any unallocated items?
                //    if (Delivery.ZeroBatchLineNums.Count == 0)
                //    {
                //        // Complete delivery
                //        btnDraft2Regular_Click(null, null);
                //        loop = 0;

                //        Console.WriteLine($"Delivery completed. {Math.Round((DateTime.Now - timer).TotalSeconds, 3)} s");
                //    }
                //}
                //else
                //{
                //    btnDraft2Regular_Click(null, null);
                //    loop = 0;
                //    Console.WriteLine($"Delivery completed. {Math.Round((DateTime.Now - timer).TotalSeconds, 3)} s");
                //}



            }
        }



        private void MainLoop(string draftKey)
        {
            Console.WriteLine("Processing " + draftKey + "...");

            txtDraft.Text = draftKey;

            // Load draft
            btnLoadDraft_Click(null, null);

            if (loop != 0)
                btnClearBatchesDraft_Click(null, null);

            // Allocate batches
            btnAllocateBatchesDraft_Click(null, null);

            // Filter item actions
            btnSortAction_Click(null, null);

            // Any non-batch items?
            if (Delivery.NonBatchLineNums.Count > 0)
            {
                // Check stock in shop and warehouse
                btnNonBatchStockCheck_Click(null, null);
            }

            // Any unallocated items?
            if (Delivery.ZeroBatchLineNums.Count > 0)
            {
                btnSortUnallocated_Click(null, null);
            }

            // Any Kimson items
            if (Delivery.KimsonDeliveryItems.Count > 0)
            {
                btnKimsonImport_Click(null, null);
                string draftKeyKs = Sap_Ks.oCompany.GetNewObjectKey();
                txtDraftKS.Text = draftKeyKs;

                // Load draft
                btnLoadDraftKS_Click(null, null);

                // Allocate batches
                btnAllocateBatchesDraftKS_Click(null, null);

                // Filter
                btnSortActionKS_Click(null, null);

                // Any direct goods
                if (Delivery.ZeroBatchLineNums.Count > 0)
                {
                    btnDirectGRKS_Click(null, null);


                    // Load draft
                    btnLoadDraftKS_Click(null, null);

                    // Clear batches
                    btnClearBatchesDraftKS_Click(null, null);

                    // Allocate batches
                    btnAllocateBatchesDraftKS_Click(null, null);

                    // Filter
                    btnSortActionKS_Click(null, null);
                }

                if (Delivery.ZeroBatchLineNums.Count == 0)
                {
                    // Delivery to Longdan
                    btnLongdanDelivery_Click(null, null);

                    // GRPO from Kimson
                    string DeliveryKeyKS = Sap_Ks.oCompany.GetNewObjectKey();
                    txtKimsonDelivery.Text = DeliveryKeyKS;


                    // Load original delivery
                    btnLoadDraft_Click(null, null);

                    btnLoadKimsonDelivery_Click(null, null);
                    btnLongdanGRPO_Click(null, null);
                }

                Console.WriteLine("Repeat loop");

                loop = 1;

                //btnAutomated_Click(null, null);
                MainLoop(draftKey);

                return;
            }





            // Any direct goods ins
            if (Delivery.GoodsReceiptItems.Count > 0)
            {
                btnDirectGR_Click(null, null);
            }


            // Any warehouse transfer items?
            if (Delivery.WarehouseTransferItems.Count > 0)
            {

                // Stock transfer

                btnCreateSTDraft_Click(null, null);

                if (Delivery.STDraftDocEntry.HasValue == true)
                {
                    // Clear 
                    txtSTDraft.Text = Delivery.STDraftDocEntry.Value.ToString();
                    btnLoadSTDraft_Click(null, null);

                    btnAllocateBatchesST_Click(null, null);

                    btnDraft2RegularST_Click(null, null);
                }


                // Delivery

                btnCreateInterBranchDraft_Click(null, null);
                //string draftKeyInterco = Sap_Ld.oCompany.GetNewObjectKey();
                string draftKeyInterco = Delivery.DraftDocEntry.ToString();

                if (string.IsNullOrEmpty(draftKeyInterco) == false)
                {
                    // GRPO
                    btnInterBranchGRPO_Click(null, null);

                    // Clear the interco delivery
                    txtDraft.Text = draftKeyInterco;

                    // Load draft
                    btnLoadDraft_Click(null, null);

                    // Allocate batches
                    btnAllocateBatchesDraft_Click(null, null);

                    // To regular
                    btnDraft2Regular_Click(null, null);
                }

            }




            // Load original reserve invoice delivery
            txtDraft.Text = draftKey;

            // Load draft
            btnLoadDraft_Click(null, null);

            // Clear batches
            btnClearBatchesDraft_Click(null, null);

            // Allocate batches
            btnAllocateBatchesDraft_Click(null, null);

            // Filter item actions
            btnSortAction_Click(null, null);

            // Any unallocated items?
            if (Delivery.ZeroBatchLineNums.Count == 0)
            {
                // Complete delivery
                btnDraft2Regular_Click(null, null);
                //loop = 0;

                Console.WriteLine($"Delivery completed. {Math.Round((DateTime.Now - timer).TotalSeconds, 3)} s");
            }
            //}
            //else
            //{
            //    btnDraft2Regular_Click(null, null);
            //    //loop = 0;
            //    Console.WriteLine($"Delivery completed. {Math.Round((DateTime.Now - timer).TotalSeconds, 3)} s");
            //}
        }



        private void btnAutomateN_Click(object sender, EventArgs e)
        {
            int loopCount = int.Parse(txtLoopCount.Text);

            DateTime bigTimer = DateTime.Now;

            for (int x = 0; x < loopCount; x++)
            {
                Console.Clear();
                Console.WriteLine($"===============================================");
                Console.WriteLine($"STARTING LOOP {x + 1} OF {loopCount}");
                Console.WriteLine($"Time elaspsed { Math.Floor((DateTime.Now - bigTimer).TotalHours) } H  { Math.Floor((DateTime.Now - bigTimer).TotalMinutes) } m  { (DateTime.Now - bigTimer).Seconds} s");

                if (x > 0)
                    Console.WriteLine($"Average time/loop {Math.Round((DateTime.Now - bigTimer).TotalSeconds / (x), 1)} s.");

                Console.WriteLine($"===============================================");

                btnAutomated_Click(null, null);

                Console.WriteLine($"COMPLETED LOOP {x + 1}" + Environment.NewLine);

                Thread.Sleep(1000);
            }

            Console.WriteLine($"===============================================");
            Console.WriteLine($"Completed {loopCount} loops.");
            Console.WriteLine($"Average time per loop {Math.Round((DateTime.Now - bigTimer).TotalSeconds / loopCount, 1)} s.");
            Console.WriteLine($"TOTAL time elaspsed { Math.Floor((DateTime.Now - bigTimer).TotalHours) } H  { Math.Floor((DateTime.Now - bigTimer).TotalMinutes) } m  { (DateTime.Now - bigTimer).Seconds} s");
            Console.WriteLine($"===============================================");
        }





        private void btnCreateSTDraft_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(V) Creating stock transfer draft unallocated items...");


            string result = Delivery.CreateDraftStockTransfer();

            // Console
            Console.WriteLine($"{result}" + Environment.NewLine);
        }

        private void btnLoadSTDraft_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine("(W) Load stock transfer draft...");

            StockTransfer = new StockTransfer();

            int docentry = int.Parse(txtSTDraft.Text);
            bool result = StockTransfer.LoadDraft(Sap_Ld, docentry);


            // Console
            if (result)
                Console.WriteLine($"Draft stock transfer {docentry} loaded." + Environment.NewLine);
            else
                Console.WriteLine($"Error: " + Sap_Ld.oCompany.GetLastErrorDescription() + Environment.NewLine);
        }

        private void btnClearBatchesST_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(X) Clearing batches...");
            int? result = null;



            if (StockTransfer != null)
                result = StockTransfer.oDraft.ClearBatches();



            // Console
            string resultMessage = null;

            switch (result)
            {
                case null:
                    resultMessage = "Stock Transfer empty.";
                    break;

                case 0:
                    resultMessage = "Success.";
                    break;

                default:
                    resultMessage = "Error: " + Sap_Ld.oCompany.GetLastErrorDescription();
                    break;
            }

            Console.WriteLine($"{resultMessage}" + Environment.NewLine);


        }

        private void btnAllocateBatchesST_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(Y) Allocating batches...");
            int? result = null;


            if (StockTransfer != null)
                result = StockTransfer.oDraft.AllocateBatchesBins(Sap_Ld);



            // Console
            if (result == 0)
                Console.WriteLine($"Batches has been allocated." + Environment.NewLine);
            else
                Console.WriteLine($"Error: " + Sap_Ld.oCompany.GetLastErrorDescription() + Environment.NewLine);
        }

        private void btnDraft2RegularST_Click(object sender, EventArgs e)
        {
            // Console
            Console.WriteLine($"(Z) Copy draft to regular stock transfer...");


            int result = StockTransfer.ProcessDraft2Regular();


            // Console
            if (result == 0)
                Console.WriteLine($"Copy successful." + Environment.NewLine);
            else
                Console.WriteLine($"Error: {StockTransfer.sap.oCompany.GetLastErrorDescription()}" + Environment.NewLine);
        }

        private void btnClearDraft_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Clear draft. Result = " + Delivery.ClearDraft());
            Console.WriteLine(Sap_Ld.oCompany.GetLastErrorDescription());
        }

        private void btnClearAllDrafts_Click_1(object sender, EventArgs e)
        {
            Delivery.ClearAllDrafts(Sap_Ld);
        }

        public void ExtInvoke(int loopCount)
        {
            txtLoopCount.Text = loopCount.ToString();
            btnAutomateN_Click(null, null);
        }

        private void lnkSimple_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SimpleForm.Show();
            this.Hide();
        }
    }
}