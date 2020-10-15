namespace Reserve_Invoices_2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnGetInvoices = new System.Windows.Forms.Button();
            this.txtTop = new System.Windows.Forms.TextBox();
            this.btnCreateDrafts = new System.Windows.Forms.Button();
            this.btnLoadDraft = new System.Windows.Forms.Button();
            this.txtDraft = new System.Windows.Forms.TextBox();
            this.btnClearBatchesDraft = new System.Windows.Forms.Button();
            this.btnAllocateBatchesDraft = new System.Windows.Forms.Button();
            this.btnSortAction = new System.Windows.Forms.Button();
            this.btnSortUnallocated = new System.Windows.Forms.Button();
            this.btnCreateInterBranchDraft = new System.Windows.Forms.Button();
            this.btnInterBranchGRPO = new System.Windows.Forms.Button();
            this.btnDraft2Regular = new System.Windows.Forms.Button();
            this.btnDirectGR = new System.Windows.Forms.Button();
            this.btnNonBatchStockCheck = new System.Windows.Forms.Button();
            this.btnAutomated = new System.Windows.Forms.Button();
            this.btnKimsonImport = new System.Windows.Forms.Button();
            this.btnAllocateBatchesDraftKS = new System.Windows.Forms.Button();
            this.btnClearBatchesDraftKS = new System.Windows.Forms.Button();
            this.txtDraftKS = new System.Windows.Forms.TextBox();
            this.btnLoadDraftKS = new System.Windows.Forms.Button();
            this.btnSortActionKS = new System.Windows.Forms.Button();
            this.btnDirectGRKS = new System.Windows.Forms.Button();
            this.btnLongdanDelivery = new System.Windows.Forms.Button();
            this.btnLoadKimsonDelivery = new System.Windows.Forms.Button();
            this.txtKimsonDelivery = new System.Windows.Forms.TextBox();
            this.btnLongdanGRPO = new System.Windows.Forms.Button();
            this.btnAutomateN = new System.Windows.Forms.Button();
            this.txtLoopCount = new System.Windows.Forms.TextBox();
            this.btnCreateSTDraft = new System.Windows.Forms.Button();
            this.txtSTDraft = new System.Windows.Forms.TextBox();
            this.btnLoadSTDraft = new System.Windows.Forms.Button();
            this.btnClearBatchesST = new System.Windows.Forms.Button();
            this.btnAllocateBatchesST = new System.Windows.Forms.Button();
            this.btnDraft2RegularST = new System.Windows.Forms.Button();
            this.btnClearDraft = new System.Windows.Forms.Button();
            this.btnClearAllDrafts = new System.Windows.Forms.Button();
            this.txtTaskList = new System.Windows.Forms.TextBox();
            this.lnkSimple = new System.Windows.Forms.LinkLabel();
            this.chkAuto = new System.Windows.Forms.CheckBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(117, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnGetInvoices
            // 
            this.btnGetInvoices.Location = new System.Drawing.Point(12, 51);
            this.btnGetInvoices.Name = "btnGetInvoices";
            this.btnGetInvoices.Size = new System.Drawing.Size(117, 23);
            this.btnGetInvoices.TabIndex = 3;
            this.btnGetInvoices.Text = "(A) Get Invoices";
            this.btnGetInvoices.UseVisualStyleBackColor = true;
            this.btnGetInvoices.Click += new System.EventHandler(this.btnGetInvoices_Click);
            // 
            // txtTop
            // 
            this.txtTop.Location = new System.Drawing.Point(135, 53);
            this.txtTop.Name = "txtTop";
            this.txtTop.Size = new System.Drawing.Size(100, 20);
            this.txtTop.TabIndex = 4;
            this.txtTop.Text = "100";
            // 
            // btnCreateDrafts
            // 
            this.btnCreateDrafts.Location = new System.Drawing.Point(12, 80);
            this.btnCreateDrafts.Name = "btnCreateDrafts";
            this.btnCreateDrafts.Size = new System.Drawing.Size(223, 23);
            this.btnCreateDrafts.TabIndex = 5;
            this.btnCreateDrafts.Text = "(B) Create Drafts";
            this.btnCreateDrafts.UseVisualStyleBackColor = true;
            this.btnCreateDrafts.Click += new System.EventHandler(this.btnCreateDrafts_Click);
            // 
            // btnLoadDraft
            // 
            this.btnLoadDraft.Location = new System.Drawing.Point(12, 140);
            this.btnLoadDraft.Name = "btnLoadDraft";
            this.btnLoadDraft.Size = new System.Drawing.Size(104, 23);
            this.btnLoadDraft.TabIndex = 6;
            this.btnLoadDraft.Text = "(C) Load Draft";
            this.btnLoadDraft.UseVisualStyleBackColor = true;
            this.btnLoadDraft.Click += new System.EventHandler(this.btnLoadDraft_Click);
            // 
            // txtDraft
            // 
            this.txtDraft.Location = new System.Drawing.Point(122, 142);
            this.txtDraft.Name = "txtDraft";
            this.txtDraft.Size = new System.Drawing.Size(71, 20);
            this.txtDraft.TabIndex = 7;
            // 
            // btnClearBatchesDraft
            // 
            this.btnClearBatchesDraft.Location = new System.Drawing.Point(12, 169);
            this.btnClearBatchesDraft.Name = "btnClearBatchesDraft";
            this.btnClearBatchesDraft.Size = new System.Drawing.Size(181, 23);
            this.btnClearBatchesDraft.TabIndex = 8;
            this.btnClearBatchesDraft.Text = "(D) Clear Batches";
            this.btnClearBatchesDraft.UseVisualStyleBackColor = true;
            this.btnClearBatchesDraft.Click += new System.EventHandler(this.btnClearBatchesDraft_Click);
            // 
            // btnAllocateBatchesDraft
            // 
            this.btnAllocateBatchesDraft.Location = new System.Drawing.Point(12, 198);
            this.btnAllocateBatchesDraft.Name = "btnAllocateBatchesDraft";
            this.btnAllocateBatchesDraft.Size = new System.Drawing.Size(181, 23);
            this.btnAllocateBatchesDraft.TabIndex = 9;
            this.btnAllocateBatchesDraft.Text = "(E) Allocate Batches";
            this.btnAllocateBatchesDraft.UseVisualStyleBackColor = true;
            this.btnAllocateBatchesDraft.Click += new System.EventHandler(this.btnAllocateBatchesDraft_Click);
            // 
            // btnSortAction
            // 
            this.btnSortAction.Location = new System.Drawing.Point(12, 227);
            this.btnSortAction.Name = "btnSortAction";
            this.btnSortAction.Size = new System.Drawing.Size(368, 49);
            this.btnSortAction.TabIndex = 10;
            this.btnSortAction.Text = "(F) Filter Item Actions";
            this.btnSortAction.UseVisualStyleBackColor = true;
            this.btnSortAction.Click += new System.EventHandler(this.btnSortAction_Click);
            // 
            // btnSortUnallocated
            // 
            this.btnSortUnallocated.Location = new System.Drawing.Point(12, 297);
            this.btnSortUnallocated.Name = "btnSortUnallocated";
            this.btnSortUnallocated.Size = new System.Drawing.Size(181, 23);
            this.btnSortUnallocated.TabIndex = 11;
            this.btnSortUnallocated.Text = "(G) Sort Unallocated";
            this.btnSortUnallocated.UseVisualStyleBackColor = true;
            this.btnSortUnallocated.Click += new System.EventHandler(this.btnSortUnallocated_Click);
            // 
            // btnCreateInterBranchDraft
            // 
            this.btnCreateInterBranchDraft.Location = new System.Drawing.Point(12, 334);
            this.btnCreateInterBranchDraft.Name = "btnCreateInterBranchDraft";
            this.btnCreateInterBranchDraft.Size = new System.Drawing.Size(181, 23);
            this.btnCreateInterBranchDraft.TabIndex = 12;
            this.btnCreateInterBranchDraft.Text = "(I) Create Inter-Branch Draft Delivery";
            this.btnCreateInterBranchDraft.UseVisualStyleBackColor = true;
            this.btnCreateInterBranchDraft.Click += new System.EventHandler(this.btnCreateInterBranchDraft_Click);
            // 
            // btnInterBranchGRPO
            // 
            this.btnInterBranchGRPO.Location = new System.Drawing.Point(199, 334);
            this.btnInterBranchGRPO.Name = "btnInterBranchGRPO";
            this.btnInterBranchGRPO.Size = new System.Drawing.Size(181, 23);
            this.btnInterBranchGRPO.TabIndex = 13;
            this.btnInterBranchGRPO.Text = "(J) Inter-Branch GRPO";
            this.btnInterBranchGRPO.UseVisualStyleBackColor = true;
            this.btnInterBranchGRPO.Click += new System.EventHandler(this.btnInterBranchGRPO_Click);
            // 
            // btnDraft2Regular
            // 
            this.btnDraft2Regular.Location = new System.Drawing.Point(12, 529);
            this.btnDraft2Regular.Name = "btnDraft2Regular";
            this.btnDraft2Regular.Size = new System.Drawing.Size(368, 44);
            this.btnDraft2Regular.TabIndex = 14;
            this.btnDraft2Regular.Text = "(L) Draft to Regular Delivery";
            this.btnDraft2Regular.UseVisualStyleBackColor = true;
            this.btnDraft2Regular.Click += new System.EventHandler(this.btnDraft2Regular_Click);
            // 
            // btnDirectGR
            // 
            this.btnDirectGR.Location = new System.Drawing.Point(12, 495);
            this.btnDirectGR.Name = "btnDirectGR";
            this.btnDirectGR.Size = new System.Drawing.Size(181, 23);
            this.btnDirectGR.TabIndex = 15;
            this.btnDirectGR.Text = "(K) Direct Goods-in to Shop";
            this.btnDirectGR.UseVisualStyleBackColor = true;
            this.btnDirectGR.Click += new System.EventHandler(this.btnDirectGR_Click);
            // 
            // btnNonBatchStockCheck
            // 
            this.btnNonBatchStockCheck.Location = new System.Drawing.Point(199, 297);
            this.btnNonBatchStockCheck.Name = "btnNonBatchStockCheck";
            this.btnNonBatchStockCheck.Size = new System.Drawing.Size(181, 23);
            this.btnNonBatchStockCheck.TabIndex = 16;
            this.btnNonBatchStockCheck.Text = "(H) Sort Non-Batch";
            this.btnNonBatchStockCheck.UseVisualStyleBackColor = true;
            this.btnNonBatchStockCheck.Click += new System.EventHandler(this.btnNonBatchStockCheck_Click);
            // 
            // btnAutomated
            // 
            this.btnAutomated.Location = new System.Drawing.Point(434, 12);
            this.btnAutomated.Name = "btnAutomated";
            this.btnAutomated.Size = new System.Drawing.Size(555, 48);
            this.btnAutomated.TabIndex = 17;
            this.btnAutomated.Text = "Automate 1 Loop";
            this.btnAutomated.UseVisualStyleBackColor = true;
            this.btnAutomated.Click += new System.EventHandler(this.btnAutomated_Click);
            // 
            // btnKimsonImport
            // 
            this.btnKimsonImport.Location = new System.Drawing.Point(394, 334);
            this.btnKimsonImport.Name = "btnKimsonImport";
            this.btnKimsonImport.Size = new System.Drawing.Size(181, 23);
            this.btnKimsonImport.TabIndex = 18;
            this.btnKimsonImport.Text = "(M) Kimson Item Imports";
            this.btnKimsonImport.UseVisualStyleBackColor = true;
            this.btnKimsonImport.Click += new System.EventHandler(this.btnKimsonImport_Click);
            // 
            // btnAllocateBatchesDraftKS
            // 
            this.btnAllocateBatchesDraftKS.Location = new System.Drawing.Point(581, 392);
            this.btnAllocateBatchesDraftKS.Name = "btnAllocateBatchesDraftKS";
            this.btnAllocateBatchesDraftKS.Size = new System.Drawing.Size(215, 23);
            this.btnAllocateBatchesDraftKS.TabIndex = 22;
            this.btnAllocateBatchesDraftKS.Text = "(P) Allocate Batches";
            this.btnAllocateBatchesDraftKS.UseVisualStyleBackColor = true;
            this.btnAllocateBatchesDraftKS.Click += new System.EventHandler(this.btnAllocateBatchesDraftKS_Click);
            // 
            // btnClearBatchesDraftKS
            // 
            this.btnClearBatchesDraftKS.Location = new System.Drawing.Point(581, 363);
            this.btnClearBatchesDraftKS.Name = "btnClearBatchesDraftKS";
            this.btnClearBatchesDraftKS.Size = new System.Drawing.Size(215, 23);
            this.btnClearBatchesDraftKS.TabIndex = 21;
            this.btnClearBatchesDraftKS.Text = "(O) Clear Batches";
            this.btnClearBatchesDraftKS.UseVisualStyleBackColor = true;
            this.btnClearBatchesDraftKS.Click += new System.EventHandler(this.btnClearBatchesDraftKS_Click);
            // 
            // txtDraftKS
            // 
            this.txtDraftKS.Location = new System.Drawing.Point(696, 336);
            this.txtDraftKS.Name = "txtDraftKS";
            this.txtDraftKS.Size = new System.Drawing.Size(100, 20);
            this.txtDraftKS.TabIndex = 20;
            // 
            // btnLoadDraftKS
            // 
            this.btnLoadDraftKS.Location = new System.Drawing.Point(581, 334);
            this.btnLoadDraftKS.Name = "btnLoadDraftKS";
            this.btnLoadDraftKS.Size = new System.Drawing.Size(109, 23);
            this.btnLoadDraftKS.TabIndex = 19;
            this.btnLoadDraftKS.Text = "(N) Load Draft";
            this.btnLoadDraftKS.UseVisualStyleBackColor = true;
            this.btnLoadDraftKS.Click += new System.EventHandler(this.btnLoadDraftKS_Click);
            // 
            // btnSortActionKS
            // 
            this.btnSortActionKS.Location = new System.Drawing.Point(581, 421);
            this.btnSortActionKS.Name = "btnSortActionKS";
            this.btnSortActionKS.Size = new System.Drawing.Size(368, 49);
            this.btnSortActionKS.TabIndex = 23;
            this.btnSortActionKS.Text = "(Q) Filter Item Actions";
            this.btnSortActionKS.UseVisualStyleBackColor = true;
            this.btnSortActionKS.Click += new System.EventHandler(this.btnSortActionKS_Click);
            // 
            // btnDirectGRKS
            // 
            this.btnDirectGRKS.Location = new System.Drawing.Point(581, 476);
            this.btnDirectGRKS.Name = "btnDirectGRKS";
            this.btnDirectGRKS.Size = new System.Drawing.Size(181, 23);
            this.btnDirectGRKS.TabIndex = 24;
            this.btnDirectGRKS.Text = "(R) Direct Goods-in to Freezer";
            this.btnDirectGRKS.UseVisualStyleBackColor = true;
            this.btnDirectGRKS.Click += new System.EventHandler(this.btnDirectGRKS_Click);
            // 
            // btnLongdanDelivery
            // 
            this.btnLongdanDelivery.Location = new System.Drawing.Point(581, 505);
            this.btnLongdanDelivery.Name = "btnLongdanDelivery";
            this.btnLongdanDelivery.Size = new System.Drawing.Size(368, 43);
            this.btnLongdanDelivery.TabIndex = 25;
            this.btnLongdanDelivery.Text = "(S) Delivery to Longdan";
            this.btnLongdanDelivery.UseVisualStyleBackColor = true;
            this.btnLongdanDelivery.Click += new System.EventHandler(this.btnLongdanDelivery_Click);
            // 
            // btnLoadKimsonDelivery
            // 
            this.btnLoadKimsonDelivery.Location = new System.Drawing.Point(12, 592);
            this.btnLoadKimsonDelivery.Name = "btnLoadKimsonDelivery";
            this.btnLoadKimsonDelivery.Size = new System.Drawing.Size(181, 23);
            this.btnLoadKimsonDelivery.TabIndex = 26;
            this.btnLoadKimsonDelivery.Text = "(T) Load Kimson Delivery Note";
            this.btnLoadKimsonDelivery.UseVisualStyleBackColor = true;
            this.btnLoadKimsonDelivery.Click += new System.EventHandler(this.btnLoadKimsonDelivery_Click);
            // 
            // txtKimsonDelivery
            // 
            this.txtKimsonDelivery.Location = new System.Drawing.Point(199, 592);
            this.txtKimsonDelivery.Name = "txtKimsonDelivery";
            this.txtKimsonDelivery.Size = new System.Drawing.Size(181, 20);
            this.txtKimsonDelivery.TabIndex = 27;
            // 
            // btnLongdanGRPO
            // 
            this.btnLongdanGRPO.Location = new System.Drawing.Point(12, 621);
            this.btnLongdanGRPO.Name = "btnLongdanGRPO";
            this.btnLongdanGRPO.Size = new System.Drawing.Size(368, 43);
            this.btnLongdanGRPO.TabIndex = 28;
            this.btnLongdanGRPO.Text = "(U) Kimson Delivery to Longdan GRPO";
            this.btnLongdanGRPO.UseVisualStyleBackColor = true;
            this.btnLongdanGRPO.Click += new System.EventHandler(this.btnLongdanGRPO_Click);
            // 
            // btnAutomateN
            // 
            this.btnAutomateN.Location = new System.Drawing.Point(434, 66);
            this.btnAutomateN.Name = "btnAutomateN";
            this.btnAutomateN.Size = new System.Drawing.Size(293, 37);
            this.btnAutomateN.TabIndex = 29;
            this.btnAutomateN.Text = "Automate N loops";
            this.btnAutomateN.UseVisualStyleBackColor = true;
            this.btnAutomateN.Click += new System.EventHandler(this.btnAutomateN_Click);
            // 
            // txtLoopCount
            // 
            this.txtLoopCount.Location = new System.Drawing.Point(733, 75);
            this.txtLoopCount.Name = "txtLoopCount";
            this.txtLoopCount.Size = new System.Drawing.Size(100, 20);
            this.txtLoopCount.TabIndex = 30;
            this.txtLoopCount.Text = "1";
            // 
            // btnCreateSTDraft
            // 
            this.btnCreateSTDraft.Location = new System.Drawing.Point(12, 381);
            this.btnCreateSTDraft.Name = "btnCreateSTDraft";
            this.btnCreateSTDraft.Size = new System.Drawing.Size(368, 23);
            this.btnCreateSTDraft.TabIndex = 31;
            this.btnCreateSTDraft.Text = "(V) Create Stock Transfer Draft";
            this.btnCreateSTDraft.UseVisualStyleBackColor = true;
            this.btnCreateSTDraft.Click += new System.EventHandler(this.btnCreateSTDraft_Click);
            // 
            // txtSTDraft
            // 
            this.txtSTDraft.Location = new System.Drawing.Point(122, 410);
            this.txtSTDraft.Name = "txtSTDraft";
            this.txtSTDraft.Size = new System.Drawing.Size(71, 20);
            this.txtSTDraft.TabIndex = 32;
            // 
            // btnLoadSTDraft
            // 
            this.btnLoadSTDraft.Location = new System.Drawing.Point(12, 408);
            this.btnLoadSTDraft.Name = "btnLoadSTDraft";
            this.btnLoadSTDraft.Size = new System.Drawing.Size(104, 23);
            this.btnLoadSTDraft.TabIndex = 33;
            this.btnLoadSTDraft.Text = "(W) Load Draft";
            this.btnLoadSTDraft.UseVisualStyleBackColor = true;
            this.btnLoadSTDraft.Click += new System.EventHandler(this.btnLoadSTDraft_Click);
            // 
            // btnClearBatchesST
            // 
            this.btnClearBatchesST.Location = new System.Drawing.Point(199, 408);
            this.btnClearBatchesST.Name = "btnClearBatchesST";
            this.btnClearBatchesST.Size = new System.Drawing.Size(181, 23);
            this.btnClearBatchesST.TabIndex = 34;
            this.btnClearBatchesST.Text = "(X) Clear Batches";
            this.btnClearBatchesST.UseVisualStyleBackColor = true;
            this.btnClearBatchesST.Click += new System.EventHandler(this.btnClearBatchesST_Click);
            // 
            // btnAllocateBatchesST
            // 
            this.btnAllocateBatchesST.Location = new System.Drawing.Point(12, 434);
            this.btnAllocateBatchesST.Name = "btnAllocateBatchesST";
            this.btnAllocateBatchesST.Size = new System.Drawing.Size(181, 23);
            this.btnAllocateBatchesST.TabIndex = 35;
            this.btnAllocateBatchesST.Text = "(Y) Allocate Batches";
            this.btnAllocateBatchesST.UseVisualStyleBackColor = true;
            this.btnAllocateBatchesST.Click += new System.EventHandler(this.btnAllocateBatchesST_Click);
            // 
            // btnDraft2RegularST
            // 
            this.btnDraft2RegularST.Location = new System.Drawing.Point(199, 434);
            this.btnDraft2RegularST.Name = "btnDraft2RegularST";
            this.btnDraft2RegularST.Size = new System.Drawing.Size(181, 23);
            this.btnDraft2RegularST.TabIndex = 36;
            this.btnDraft2RegularST.Text = "(Z) Draft to Regular ST";
            this.btnDraft2RegularST.UseVisualStyleBackColor = true;
            this.btnDraft2RegularST.Click += new System.EventHandler(this.btnDraft2RegularST_Click);
            // 
            // btnClearDraft
            // 
            this.btnClearDraft.Location = new System.Drawing.Point(199, 139);
            this.btnClearDraft.Name = "btnClearDraft";
            this.btnClearDraft.Size = new System.Drawing.Size(181, 23);
            this.btnClearDraft.TabIndex = 37;
            this.btnClearDraft.Text = "Clear draft";
            this.btnClearDraft.UseVisualStyleBackColor = true;
            this.btnClearDraft.Click += new System.EventHandler(this.btnClearDraft_Click);
            // 
            // btnClearAllDrafts
            // 
            this.btnClearAllDrafts.Location = new System.Drawing.Point(434, 139);
            this.btnClearAllDrafts.Name = "btnClearAllDrafts";
            this.btnClearAllDrafts.Size = new System.Drawing.Size(181, 23);
            this.btnClearAllDrafts.TabIndex = 38;
            this.btnClearAllDrafts.Text = "CLEAR ALL DRAFTS";
            this.btnClearAllDrafts.UseVisualStyleBackColor = true;
            this.btnClearAllDrafts.Click += new System.EventHandler(this.btnClearAllDrafts_Click_1);
            // 
            // txtTaskList
            // 
            this.txtTaskList.Location = new System.Drawing.Point(12, 106);
            this.txtTaskList.Name = "txtTaskList";
            this.txtTaskList.Size = new System.Drawing.Size(977, 20);
            this.txtTaskList.TabIndex = 39;
            // 
            // lnkSimple
            // 
            this.lnkSimple.AutoSize = true;
            this.lnkSimple.Location = new System.Drawing.Point(310, 17);
            this.lnkSimple.Name = "lnkSimple";
            this.lnkSimple.Size = new System.Drawing.Size(38, 13);
            this.lnkSimple.TabIndex = 40;
            this.lnkSimple.TabStop = true;
            this.lnkSimple.Text = "Simple";
            this.lnkSimple.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSimple_LinkClicked);
            // 
            // chkAuto
            // 
            this.chkAuto.AutoSize = true;
            this.chkAuto.Checked = true;
            this.chkAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAuto.Location = new System.Drawing.Point(241, 55);
            this.chkAuto.Name = "chkAuto";
            this.chkAuto.Size = new System.Drawing.Size(48, 17);
            this.chkAuto.TabIndex = 41;
            this.chkAuto.Text = "Auto";
            this.chkAuto.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(135, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(100, 23);
            this.btnDisconnect.TabIndex = 42;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 677);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.chkAuto);
            this.Controls.Add(this.lnkSimple);
            this.Controls.Add(this.txtTaskList);
            this.Controls.Add(this.btnClearAllDrafts);
            this.Controls.Add(this.btnClearDraft);
            this.Controls.Add(this.btnDraft2RegularST);
            this.Controls.Add(this.btnAllocateBatchesST);
            this.Controls.Add(this.btnClearBatchesST);
            this.Controls.Add(this.btnLoadSTDraft);
            this.Controls.Add(this.txtSTDraft);
            this.Controls.Add(this.btnCreateSTDraft);
            this.Controls.Add(this.txtLoopCount);
            this.Controls.Add(this.btnAutomateN);
            this.Controls.Add(this.btnLongdanGRPO);
            this.Controls.Add(this.txtKimsonDelivery);
            this.Controls.Add(this.btnLoadKimsonDelivery);
            this.Controls.Add(this.btnLongdanDelivery);
            this.Controls.Add(this.btnDirectGRKS);
            this.Controls.Add(this.btnSortActionKS);
            this.Controls.Add(this.btnAllocateBatchesDraftKS);
            this.Controls.Add(this.btnClearBatchesDraftKS);
            this.Controls.Add(this.txtDraftKS);
            this.Controls.Add(this.btnLoadDraftKS);
            this.Controls.Add(this.btnKimsonImport);
            this.Controls.Add(this.btnAutomated);
            this.Controls.Add(this.btnNonBatchStockCheck);
            this.Controls.Add(this.btnDirectGR);
            this.Controls.Add(this.btnDraft2Regular);
            this.Controls.Add(this.btnInterBranchGRPO);
            this.Controls.Add(this.btnCreateInterBranchDraft);
            this.Controls.Add(this.btnSortUnallocated);
            this.Controls.Add(this.btnSortAction);
            this.Controls.Add(this.btnAllocateBatchesDraft);
            this.Controls.Add(this.btnClearBatchesDraft);
            this.Controls.Add(this.txtDraft);
            this.Controls.Add(this.btnLoadDraft);
            this.Controls.Add(this.btnCreateDrafts);
            this.Controls.Add(this.txtTop);
            this.Controls.Add(this.btnGetInvoices);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnGetInvoices;
        private System.Windows.Forms.TextBox txtTop;
        private System.Windows.Forms.Button btnCreateDrafts;
        private System.Windows.Forms.Button btnLoadDraft;
        private System.Windows.Forms.TextBox txtDraft;
        private System.Windows.Forms.Button btnClearBatchesDraft;
        private System.Windows.Forms.Button btnAllocateBatchesDraft;
        private System.Windows.Forms.Button btnSortAction;
        private System.Windows.Forms.Button btnSortUnallocated;
        private System.Windows.Forms.Button btnCreateInterBranchDraft;
        private System.Windows.Forms.Button btnInterBranchGRPO;
        private System.Windows.Forms.Button btnDraft2Regular;
        private System.Windows.Forms.Button btnDirectGR;
        private System.Windows.Forms.Button btnNonBatchStockCheck;
        private System.Windows.Forms.Button btnAutomated;
        private System.Windows.Forms.Button btnKimsonImport;
        private System.Windows.Forms.Button btnAllocateBatchesDraftKS;
        private System.Windows.Forms.Button btnClearBatchesDraftKS;
        private System.Windows.Forms.TextBox txtDraftKS;
        private System.Windows.Forms.Button btnLoadDraftKS;
        private System.Windows.Forms.Button btnSortActionKS;
        private System.Windows.Forms.Button btnDirectGRKS;
        private System.Windows.Forms.Button btnLongdanDelivery;
        private System.Windows.Forms.Button btnLoadKimsonDelivery;
        private System.Windows.Forms.TextBox txtKimsonDelivery;
        private System.Windows.Forms.Button btnLongdanGRPO;
        private System.Windows.Forms.Button btnAutomateN;
        private System.Windows.Forms.TextBox txtLoopCount;
        private System.Windows.Forms.Button btnCreateSTDraft;
        private System.Windows.Forms.TextBox txtSTDraft;
        private System.Windows.Forms.Button btnLoadSTDraft;
        private System.Windows.Forms.Button btnClearBatchesST;
        private System.Windows.Forms.Button btnAllocateBatchesST;
        private System.Windows.Forms.Button btnDraft2RegularST;
        private System.Windows.Forms.Button btnClearDraft;
        private System.Windows.Forms.Button btnClearAllDrafts;
        private System.Windows.Forms.TextBox txtTaskList;
        private System.Windows.Forms.LinkLabel lnkSimple;
        private System.Windows.Forms.CheckBox chkAuto;
        private System.Windows.Forms.Button btnDisconnect;
    }
}

