using Centeralized;
using Dataaccesslaeyr;
using Easypos.Salesforms.Normal;
using Querylaeyr.Modelsales;
using Querylaeyr.Pricingmodels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easypos.Pricing
{
    public partial class Frmpricelist : Form
    {
        Centergetingfunctions CF;
        CompanyInfo Com;
        Price Prc;
        DataAccess DA;
        public string Cashiername { get; set; }
        public Frmpricelist()
        {
            InitializeComponent();
            DA = new DataAccess();
            CF = new Centergetingfunctions();
            Com = new CompanyInfo();
            Com.GetCompany();
            Getcustomers();
            Getsalesinfo();
        }
        public void Getcustomers()
        {
            try
            {
                var GD = CF.Getthirdparties();
                DataRow dr = GD.NewRow();
                dr["ID"] = 0;
                dr["Name"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                clientID.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "قائمة عرض الاسعار" + "','" + "Get Customers" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Getsalesinfo()
        {
            try
            {
                Prc = new Price();
                var GD = Prc.Getdata();
                DGV.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "قائمة عرض الاسعار" + "','" + "Getsalesinfo" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
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
        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Columns[e.ColumnIndex].Name == "Show")
            {
                Prc.ID = int.Parse(DGV.CurrentRow.Cells[2].Value.ToString());
                //var ST = DGV.CurrentRow.Cells[9].Value.ToString();
                //if (Com.Taxnumber == "")
                //{

                //}
                //else
                //{
                //    MessageBox.Show("لا يمكن التعديل على فاتورة صدرت", "خطأ");
                //    return;
                //}
                Frmprice FPrc = new Frmprice();
                Frmprice open = Application.OpenForms["Frmprice"] as Frmprice;
                if (open == null)
                {
                    FPrc.Invnum = Prc.ID;
                    FPrc.Btnsave.Text = "تعديل";
                    FPrc.St = Cashiername;
                    FPrc.Btnsaveandprint.Text = "تعديل وطباعه";
                    FPrc.Show();
                    this.Close();
                }
                else
                {
                    open.Activate();
                    if (open.WindowState == FormWindowState.Maximized)
                    {
                        open.Invnum = Prc.ID;
                        open.Btnsave.Enabled = false;
                        open.Btnsaveandprint.Enabled = false;
                        open.Getinvoice();
                        this.Close();
                    }
                }
            }
            else
            {
                Prc.ID = int.Parse(DGV.CurrentRow.Cells[2].Value.ToString());
                Frmprice FPrc = new Frmprice();
                FPrc.Prc.ID = Prc.ID;
                FPrc.St = Cashiername;
                FPrc.Printinv();
            }
        }
        private void Btnsearch_Click(object sender, EventArgs e)
        {
            Getsalesbyfilters();
        }
        public void Getsalesbyfilters()
        {
            try
            {
                string datefrom = "";
                string dateto = "";
                string Timefrom = "";
                string Timeto = "";
                int invnum = 0;
                int custid = 0;
                if (Searchbydate.Checked)
                {
                    datefrom = DTF.Value.ToString("yyyy-MM-dd");
                    dateto = DTT.Value.ToString("yyyy-MM-dd");
                }
                if (!string.IsNullOrEmpty(IN.Text))
                {
                    invnum = int.Parse(IN.Text);
                }
                if (!string.IsNullOrEmpty(clientID.SelectedValue.ToString()))
                {
                    custid = int.Parse(clientID.SelectedValue.ToString());
                }
                var GD = Prc.Getdatabyfilter(invnum, custid, datefrom, dateto);
                DGV.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "قائمة عرض الاسعار" + "','" + "Getsalesbyfilters" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        private void Btnall_Click(object sender, EventArgs e)
        {
            Getsalesinfo();
        }
    }
}
