using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reserve_Invoices_2
{
    public partial class Form2 : Form
    {
        Form1 form;
        public Form2()
        {
            InitializeComponent();
            form = new Form1();
            form.SimpleForm = this;
        }

        private void lnkAdvanced_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            form.Show();
            this.Hide();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            form.ExtInvoke(int.Parse(txtLoops.Text));
        }
    }
}
