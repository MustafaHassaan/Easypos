using Centeralized;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using Reporting;
using Reporting.Accountant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easypos.TransactionsAccountant
{
    public partial class Customeraccount : Form
    {
        CompanyInfo Comp;
        public string Accountname { get; set; }
        public Customeraccount()
        {
            InitializeComponent();
            Comp = new CompanyInfo();
            Comp.GetCompany();
        }
        public Image img { get; set; }
        private void Customeraccount_Load(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Accountantreport AR = new Accountantreport();
            Frmreporting FQR = new Frmreporting();
            Dataset ds = new Dataset();
            byte[] Logo = Convert.FromBase64String(Comp.Logo);
            int i = 0;
            for (i = 0; i < DGV.Rows.Count; i++)
            {
                var DTD = DGV.Rows[i].Cells[1].Value;
                if (DTD == null)
                {
                    DTD = textBox5.Text;
                    DGV.Rows[i].Cells[0].Value = 0;
                }
                ds.Accountants.Rows.Add(new object[] {
                    DGV.Rows[i].Cells[0].Value.ToString(),
                    DTD,
                    DGV.Rows[i].Cells[2].Value.ToString(),
                    DGV.Rows[i].Cells[3].Value.ToString(),
                    DGV.Rows[i].Cells[4].Value.ToString(),
                    DGV.Rows[i].Cells[5].Value.ToString(),
                    Logo
                });
            }
            AR.SetDataSource(ds);
            FQR.CRV.ReportSource = AR;
            FQR.CRV.Refresh();


            AR.SetParameterValue("CompanyName", Comp.CName);
            AR.SetParameterValue("Address", Comp.Address);
            AR.SetParameterValue("PhoneNo", "PN");
            AR.SetParameterValue("MobileNo", "MN");
            AR.SetParameterValue("Taxnum", Comp.Taxnumber);
            AR.SetParameterValue("Fromdate", textBox5.Text);
            AR.SetParameterValue("Todate", textBox6.Text);
            AR.SetParameterValue("Clientname", textBox2.Text);
            AR.SetParameterValue("Clientaddress", textBox3.Text);
            AR.SetParameterValue("Taxnumber", textBox4.Text);
            if (Accountname == "كشف حساب عميل")
            {
                AR.SetParameterValue("TOF", "كشف حساب عميل");
            }
            else
            {
                AR.SetParameterValue("TOF", "كشف حساب مورد");
            }
            AR.SetParameterValue("English_Shop_name", Comp.CENName);
            AR.SetParameterValue("Proname", Comp.CRN);
            FQR.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
