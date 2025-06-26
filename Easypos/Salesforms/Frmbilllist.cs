using Centeralized;
using Dataaccesslaeyr;
using Easypos.Salesforms.Normal;
using Querylaeyr.Modelmaster;
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
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;


namespace Easypos.Salesforms
{
    public partial class Frmbilllist : Form
    {
        Centergetingfunctions CF;
        CompanyInfo Com;
        Sales sal;
        public int Cashid { get; set; }
        public string Cashier { get; set; }
        public Frmbilllist()
        {
            InitializeComponent();
            CF = new Centergetingfunctions();
            Com = new CompanyInfo();
            Com.GetCompany();
            Getcustomers();
            Getsalesinfo();
            DGVClculate();
        }
        private void Getall()
        {
            IN.Clear();
            Getcustomers();
            Getsalesinfo();
            DGVClculate();
            Searchbytime.Checked = false;
            Searchbydate.Checked = false;
            DTF.Value.ToShortDateString();
            DTT.Value.ToShortDateString();
            TTT.Value.ToShortTimeString();
            TTF.Value.ToShortTimeString();
        }
        public void Getcustomers()
        {
            var GD = CF.Getthirdparties();
            DataRow dr = GD.NewRow();
            dr["ID"] = 0;
            dr["Name"] = "--اختر--";
            GD.Rows.InsertAt(dr, 0);
            clientID.DataSource = GD;
        }
        public void Getsalesinfo()
        {
            DGV.Rows.Clear();
            sal = new Sales();
            var GD = sal.Getdata();
            foreach (DataRow row in GD.Rows)
            {
                DGV.Rows.Add(null,null, 
                    row[0].ToString(), 
                    row[1].ToString(), 
                    row[2].ToString(), 
                    row[3].ToString(),
                    row[4].ToString(),
                    row[5].ToString(),
                    row[6].ToString(),
                    row[8].ToString(),
                    row[9].ToString(),
                    row[7].ToString(),
                    row[10].ToString(),
                    row[11].ToString(),
                    null
                    );
            }
            //DGV.DataSource = GD;
        }
        public void Getsalesbyfilters()
        {
            DGV.Rows.Clear();
            string datefrom = "";
            string dateto = "";
            string Timefrom = "";
            string Timeto = "";
            string Note = "";
            string Phone = "";
            int invnum = 0;
            int custid = 0;
            if (Searchbytime.Checked)
            {
                Timefrom = TTF.Value.ToString("HH:mm:ss");
                Timeto = TTT.Value.ToString("HH:mm:ss");
            }
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
            if (!string.IsNullOrEmpty(txtPhone.Text))
            {
                Phone = txtPhone.Text;
            }
            if (!string.IsNullOrEmpty(txtNote.Text))
            {
                Note = txtNote.Text;
            }
            var GD = sal.Getdatabyfilter(invnum, custid, datefrom, dateto, Timefrom, Timeto,Phone,Note);
            foreach (DataRow row in GD.Rows)
            {
                DGV.Rows.Add(null, null,
                    row[0].ToString(),
                    row[1].ToString(),
                    row[2].ToString(),
                    row[3].ToString(),
                    row[4].ToString(),
                    row[5].ToString(),
                    row[6].ToString(),
                    row[8].ToString(),
                    row[9].ToString(),
                    row[7].ToString(),
                    row[10].ToString(),
                    row[11].ToString(),
                    null
                    );
            }
            //DGV.DataSource = GD;
            DGVClculate();
        }
        private void DGVClculate()
        {
            decimal DGSubtotal = 0;
            decimal DGVDisc = 0;
            decimal DGVTax = 0;
            decimal DGTotal = 0;
            if (DGV.Rows.Count > 0)
            {
                for (int i = 0; i < DGV.Rows.Count; i++)
                {
                    DGSubtotal += Convert.ToDecimal(DGV.Rows[i].Cells[5].Value.ToString());
                    DGVDisc += Convert.ToDecimal(DGV.Rows[i].Cells[6].Value.ToString());
                    DGVTax += Convert.ToDecimal(DGV.Rows[i].Cells[7].Value.ToString());
                    DGTotal += Convert.ToDecimal(DGV.Rows[i].Cells[8].Value.ToString());
                }
                lblsubtotal.Text = DGSubtotal.ToString();
                lbldisc.Text = DGVDisc.ToString();
                lbltax.Text = DGVTax.ToString();
                lbltot.Text = DGTotal.ToString();
            }
            else
            {
                lblsubtotal.Text = DGSubtotal.ToString();
                lbldisc.Text = DGVDisc.ToString();
                lbltax.Text = DGVTax.ToString();
                lbltot.Text = DGTotal.ToString();
            }
        }
        private void Frmbilllist_Load(object sender, EventArgs e)
        {
        }
        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
           this.Close();
        }
        private void Searchbydate_CheckedChanged(object sender, EventArgs e)
        {
            if (Searchbydate.Checked)
            {
                DTF.Visible = true;
                DTT.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
            }
            else
            {
                DTF.Visible = false;
                DTT.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
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
        private void Searchbytime_CheckedChanged(object sender, EventArgs e)
        {
            if (Searchbytime.Checked)
            {
                TTF.Visible = true;
                TTF.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                TTT.Visible = true;
                TTT.Visible = true;
            }
            else
            {
                TTF.Visible = false;
                TTF.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                TTT.Visible = false;
                TTT.Visible = false;
            }
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Columns[e.ColumnIndex].Name == "Show")
            {
                sal.Invoiceno = int.Parse(DGV.CurrentRow.Cells[2].Value.ToString());
                frmMSalesBill FMS = new frmMSalesBill();
                frmMSalesBill open = Application.OpenForms["frmMSalesBill"] as frmMSalesBill;
                if (open == null)
                {
                    FMS.Invnum = sal.Invoiceno;
                    FMS.Btnsave.Text = "تعديل";
                    FMS.Btnsaveandprint.Text = "تعديل وطباعه";
                    var ST = DGV.CurrentRow.Cells[12].Value.ToString();
                    if (ST == "مسوده")
                    {
                        FMS.Btnsaveandprint.Visible = true;
                        FMS.Btnsave.Visible = true;
                        FMS.Btnsave.Visible = true;
                        FMS.Btnsave.Text = "تعديل";
                        FMS.Btnsaveandprint.Text = "تعديل وطباعه";
                        FMS.BT = "مسوده";
                    }
                    else
                    {
                        FMS.Btnsaveandprint.Visible = false;
                        FMS.Btnsave.Visible = false;
                        FMS.Btnsave.Visible = false;
                    }
                    FMS.Show();
                    this.Close();
                }
                else
                {
                    open.Activate();
                    if (open.WindowState == FormWindowState.Maximized)
                    {
                        open.Invnum = sal.Invoiceno;
                        open.Getinvoice();
                        var ST = DGV.CurrentRow.Cells[12].Value.ToString();
                        if (ST == "مسوده")
                        {
                            open.Btnsaveandprint.Visible = true;
                            open.Btnsave.Visible = true;
                            open.Btnsave.Visible = true;
                            open.Btnsave.Text = "تعديل";
                            open.Btnsaveandprint.Text = "تعديل وطباعه";
                            open.Billtype.Text = ST;
                        }
                        else
                        {
                            open.Btnsaveandprint.Visible = false;
                            open.Btnsave.Visible = false;
                            open.Btnsave.Visible = false;
                            open.Billtype.Text = ST;
                        }
                        this.Close();
                    }
                }
            }
            else if (DGV.Columns[e.ColumnIndex].Name == "Delete")
            {
                sal.Invoiceno = int.Parse(DGV.CurrentRow.Cells[2].Value.ToString());
                var ST = DGV.CurrentRow.Cells[12].Value.ToString();
                if (Com.Taxnumber == "" || ST == "مسوده")
                {
                    var Qur = @"SELECT Invoiceno, 
                                   Paynum FROM transactions Where Invoiceno = " + sal.Invoiceno;
                    DataAccess DA = new DataAccess();
                    var Paynum = 0;
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        Paynum = int.Parse(GD["Paynum"].ToString());
                    }
                    var Qst = @"Delete From Transactions Where Invoiceno = " + sal.Invoiceno;
                    var Qsp = @"Delete From payment Where paymentNo = " + Paynum;
                    var Qsd = @"Delete From salesdetailes Where InvoiceNo = " + sal.Invoiceno;
                    var Qss = @"Delete From Sales Where Invoiceno = " + sal.Invoiceno;
                    CF.Deletebyid(Qst);
                    CF.Deletebyid(Qsp);
                    CF.Deletebyid(Qsd);
                    CF.Deletebyid(Qss);
                    MessageBox.Show("تم حذف الفاتورة بنجاح", "تم");
                    Getall();
                    return;
                }
                else
                {
                    MessageBox.Show("لا يمكن حذف على فاتورة صدرت", "خطأ");
                    return;
                }
            }
            else
            {
                sal.Invoiceno = int.Parse(DGV.CurrentRow.Cells[2].Value.ToString());
                frmMSalesBill FMS = new frmMSalesBill();
                FMS.Invnum = sal.Invoiceno;
                FMS.St = Cashier;
                FMS.Taxinvlist = double.Parse(DGV.CurrentRow.Cells[7].Value.ToString());
                FMS.Totinvlist = double.Parse(DGV.CurrentRow.Cells[8].Value.ToString());
                DateTime dt = Convert.ToDateTime(DGV.CurrentRow.Cells[3].Value.ToString() + " " + DGV.CurrentRow.Cells[4].Value.ToString());
                FMS.Timelist = dt;
                FMS.Printinv();
            }
        }
    }
}
