namespace Reserve_Invoices_2
{
    partial class Form2
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
            this.btnRun = new System.Windows.Forms.Button();
            this.txtLoops = new System.Windows.Forms.TextBox();
            this.lnkAdvanced = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtLoops
            // 
            this.txtLoops.Location = new System.Drawing.Point(93, 14);
            this.txtLoops.Name = "txtLoops";
            this.txtLoops.Size = new System.Drawing.Size(101, 20);
            this.txtLoops.TabIndex = 1;
            this.txtLoops.Text = "100";
            // 
            // lnkAdvanced
            // 
            this.lnkAdvanced.AutoSize = true;
            this.lnkAdvanced.Location = new System.Drawing.Point(138, 41);
            this.lnkAdvanced.Name = "lnkAdvanced";
            this.lnkAdvanced.Size = new System.Drawing.Size(56, 13);
            this.lnkAdvanced.TabIndex = 2;
            this.lnkAdvanced.TabStop = true;
            this.lnkAdvanced.Text = "Advanced";
            this.lnkAdvanced.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAdvanced_LinkClicked);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(206, 63);
            this.Controls.Add(this.lnkAdvanced);
            this.Controls.Add(this.txtLoops);
            this.Controls.Add(this.btnRun);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtLoops;
        private System.Windows.Forms.LinkLabel lnkAdvanced;
    }
}