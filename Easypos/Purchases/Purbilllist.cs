using Centeralized;
using Dataaccesslaeyr;
using Easypos.Salesforms.Normal;
using MySql.Data.MySqlClient;
using Querylaeyr.Modelsales;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Easypos.Purchases
{
    public partial class Purbilllist : Form
    {
        Centergetingfunctions CF;
        Sales sal;
        DataAccess Da;
        public Purbilllist()
        {
            InitializeComponent();
            Da = new DataAccess();
            CF = new Centergetingfunctions();
            Getcustomers();
            Getsalesinfo();
        }
        private void Getall()
        {
            IN.Clear();
            Getcustomers();
            Getsalesinfo();
            DTF.Value.ToShortDateString();
            DTT.Value.ToShortDateString();
        }
        public void Getcustomers()
        {
            try
            {
                var GD = CF.GetSuppliers();
                DataRow dr = GD.NewRow();
                dr["ID"] = 0;
                dr["Name"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                clientID.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة قائمة المشتريات" + "','" + "Getcustomers" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getsalesinfo()
        {
            try
            {
                sal = new Sales();
                var GD = sal.Getpurdata();
                DGV.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة قائمة المشتريات" + "','" + "Getsalesinfo" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getsalesbyfilters()
        {
            try
            {
                string datefrom = "";
                string dateto = "";
                int invnum = 0;
                int custid = 0;
                datefrom = DTF.Value.ToString("yyyy-MM-dd");
                dateto = DTT.Value.ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(IN.Text))
                {
                    invnum = int.Parse(IN.Text);
                }
                if (!string.IsNullOrEmpty(clientID.SelectedValue.ToString()))
                {
                    custid = int.Parse(clientID.SelectedValue.ToString());
                }
                var GD = sal.Getdatapurbyfilter(invnum, custid, datefrom, dateto);
                DGV.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة قائمة المشتريات" + "','" + "Getsalesbyfilters" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sal.Invoiceno = int.Parse(DGV.CurrentRow.Cells[1].Value.ToString());
            var ST = DGV.CurrentRow.Cells[9].Value.ToString();
            if (ST == "مسودة")
            {
                frmPurchases FMS = new frmPurchases();
                frmPurchases open = Application.OpenForms["frmPurchases"] as frmPurchases;
                if (open == null)
                {
                    FMS.Invnum = sal.Invoiceno;
                    FMS.Btnsave.Text = "تعديل";
                    FMS.Btnsaveandprint.Text = "تعديل وطباعه";
                    FMS.Show();
                    this.Close();
                }
                else
                {
                    open.Activate();
                    if (open.WindowState == FormWindowState.Maximized)
                    {
                        open.Invnum = sal.Invoiceno;
                        open.Btnsave.Text = "تعديل";
                        open.Btnsaveandprint.Text = "تعديل وطباعه";
                        open.Getinvoice();
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("لا يمكن التعديل على فاتورة صدرت", "خطأ");
                return;
            }
        }

        private void Btnsearch_Click(object sender, EventArgs e)
        {
            Getsalesbyfilters();
        }

        private void Btnall_Click(object sender, EventArgs e)
        {
            Getall();
        }
    }
}
