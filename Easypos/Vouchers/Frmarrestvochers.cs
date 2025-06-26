using Centeralized;
using CrystalDecisions.Windows.Forms;
using Dataaccesslaeyr;
using Easypos.Payment;
using MetroFramework.Forms;
using MySqlX.XDevAPI;
using Querylaeyr.Modelmaster;
using Querylaeyr.Paymodel;
using Reporting;
using Reporting.Vouchers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Easypos.Vouchers
{
    public partial class Frmarrestvochers : MetroForm
    {
        Centergetingfunctions CF;
        Thirdparty Cust;
        Paymentmodel Pm;
        Transactions trans;
        DataAccess Da;
        public int Transbynum { get; set; }
        public Frmarrestvochers()
        {
            InitializeComponent();
            Da = new DataAccess();
            CF = new Centergetingfunctions();
            CF.Comp = new CompanyInfo();
            CF.Comp.GetCompany();
            Cust = new Thirdparty();
            Getcustomers();
            Vochertypes.SelectedIndex = 0;
            Pm = new Paymentmodel();
            Getvouchersave();
            Getvoucherpaysave();
            Thirdparties();
            Vochertype.SelectedIndex = 0;
            Cmbpricetype.SelectedIndex = 0;
            CmbPaymethod.SelectedIndex = 0;
            trans = new Transactions();
        }
        public double Creditor { get; set; }
        public string Thirtype { get; set; }
        public void Thirdparties()
        {
            try
            {
                DataTable GD;
                if (RBCust.Checked)
                {
                    GD = Cust.Getdatabycustomer();
                    DataRow dr = GD.NewRow();
                    dr["ID"] = 0;
                    dr["Name"] = "--اختر--";
                    GD.Rows.InsertAt(dr, 0);
                }
                else if (RBSup.Checked)
                {
                    GD = Cust.Getdatabysuppliers();
                    DataRow dr = GD.NewRow();
                    dr["ID"] = 0;
                    dr["Name"] = "--اختر--";
                    GD.Rows.InsertAt(dr, 0);
                }
                else
                {
                    GD = Cust.Getdata();
                    DataRow dr = GD.NewRow();
                    dr["ID"] = 0;
                    dr["Name"] = "--اختر--";
                    GD.Rows.InsertAt(dr, 0);
                }

                CBMThirdparty.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "Thirdparties" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
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
                Clients.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "Getthirdparties" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getvouchersave()
        {
            try
            {
                var GD = Pm.Getdata();
                DGSales.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "Getvouchersave" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getvoucherpaysave()
        {
            try
            {
                var GD = Pm.Getrecivdata();
                DGVPur.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "Getvoucherpaysave" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Clearfieldes()
        {
            txtpayout.Clear();
            txtinvnum.Clear();
            txtmony.Clear();
            CmbPaymethod.SelectedIndex = 0;
            Btnsave.Enabled = true;
            Thirdparties();
            Getcustomers();
            Getvouchersave();
            Getvoucherpaysave();
            Pm.paymentNo = 0;
            Vochertypes.SelectedIndex = 0;
            Vochertype.SelectedIndex = 0;
            date.Value = DateTime.Now;
            Purdate.Value = DateTime.Now;
            Note.Clear();
            Purnottxt.Clear();
            Btnsave.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            Btnadd.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            Btnsave.Text = "حفظ";
            Btnadd.Text = "حفظ";
            Btnsave.BackColor = Color.FromArgb(0, 173, 31);
            Btnadd.BackColor = Color.FromArgb(0, 173, 31);
            label9.Visible = false;
            lblbill.Visible = false;
            Invnum.Visible = false;
            Billnumber.Visible = false;
            Invnum.Items.Clear();
            Billnumber.Items.Clear();
            txtpay.Clear();
            txtinv.Clear();
            Cmbpricetype.SelectedIndex = 0;
            txtprice.Clear();
        }
        public void Printvoucher(int id)
        {
            try
            {
                Pm.Printvoucher(id);
                Frmreporting FR = new Frmreporting();
                ReceiptVoucher Objrpt = new ReceiptVoucher();
                Dataset Dsx = new Dataset();
                Dsx.Vochers.Rows.Add(new object[] {
                        Pm.paymentNo,Pm.Date,Pm.Paid,Pm.Name,
                        Pm.Paid,Pm.Type,null,null,
                        null,null
                    });
                Objrpt.SetDataSource(Dsx);
                Objrpt.SetParameterValue("CompanyName", CF.Comp.CName);
                Objrpt.SetParameterValue("Address", CF.Comp.Address);
                Objrpt.SetParameterValue("Taxnum", CF.Comp.Taxnumber);
                Objrpt.SetParameterValue("PhoneNo", CF.Comp.PhoneNo);
                Objrpt.SetParameterValue("Proname", CF.Comp.CRN);
                Objrpt.SetParameterValue("English_Shop_name", CF.Comp.CENName);
                FR.CRV.ReportSource = Objrpt;
                FR.Show();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "Printvoucher" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Printreciver(int id)
        {
            try
            {
                Pm.Printreciver(id);
                Frmreporting FR = new Frmreporting();
                PaymentVoucher Objrpt = new PaymentVoucher();
                Dataset Dsx = new Dataset();
                Dsx.Vochers.Rows.Add(new object[] {
                        Pm.paymentNo,Pm.Date,Pm.Paid,Pm.Name,
                        Pm.Paid,Pm.Type,null,null,
                        null,null
                    });
                Objrpt.SetDataSource(Dsx);
                Objrpt.SetParameterValue("CompanyName", CF.Comp.CName);
                Objrpt.SetParameterValue("Address", CF.Comp.Address);
                Objrpt.SetParameterValue("Taxnum", CF.Comp.Taxnumber);
                Objrpt.SetParameterValue("PhoneNo", CF.Comp.PhoneNo);
                Objrpt.SetParameterValue("Proname", CF.Comp.CRN);
                Objrpt.SetParameterValue("English_Shop_name", CF.Comp.CENName);
                FR.CRV.ReportSource = Objrpt;
                FR.Show();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "Printreciver" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getinvoices()
        {
            try
            {
                if (Vochertypes.Text == "سند ايصال مبيعات")
                {
                    //if (Clients.SelectedIndex == 0)
                    //{
                    //    Vochertypes.SelectedIndex = 0;
                    //    MessageBox.Show("برجاء اختيار العميل", "خطأ");
                    //    return;
                    //}
                    //else
                    //{

                    //}
                    label9.Visible = true;
                    Invnum.Visible = true;
                    var Cid = int.Parse(Clients.SelectedValue.ToString());
                    var GD = Pm.Getvoucherbythird(Cid);
                    if (GD != null)
                    {
                        var PML = Pm.invoices;
                        if (PML != null)
                        {
                            foreach (var item in PML)
                            {
                                Creditor = item.Remaining;
                                Invnum.Items.Add(item.InvoiceNo);
                            }
                        }
                    }
                }
                else
                {
                    label9.Visible = false;
                    Invnum.Visible = false;
                    lblbill.Visible = false;
                    Billnumber.Visible = false;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "Getinvoices" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getreciver()
        {
            try
            {
                if (Vochertype.Text == "سند ايصال مشتريات")
                {
                    if (CBMThirdparty.SelectedIndex == 0)
                    {
                        Vochertypes.SelectedIndex = 0;
                        MessageBox.Show("برجاء اختيار المورد", "خطأ");
                        return;
                    }
                    else
                    {
                        var Cid = int.Parse(CBMThirdparty.SelectedValue.ToString());
                        var GD = Pm.Getreciverbythird(Cid);
                        if (GD != null)
                        {
                            var PML = Pm.invoices;
                            if (PML != null)
                            {
                                foreach (var item in PML)
                                {
                                    Creditor = item.Remaining;
                                    Billnumber.Items.Add(item.InvoiceNo);
                                }
                            }
                        }
                    }
                }
                else
                {
                    label9.Visible = false;
                    Invnum.Visible = false;
                    lblbill.Visible = false;
                    Billnumber.Visible = false;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "Getreciver" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void Frmarrestvochers_Load(object sender, EventArgs e)
        {
        }
        private void DGSales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }
        private void Btnsave_Click(object sender, EventArgs e)
        {
            if (Btnsave.Text == "تعديل")
            {
                var Qst = @"Update Transactions 
                            set
                            TDate = '"+ date.Value.ToString("yyyy-MM-dd")+"',"+
                            "Type = '"+ Vochertypes.Text + "',"+
                            "ThirdPartyID = "+ Clients.SelectedValue +","+
                            "Paid = "+ double.Parse(txtprice.Text) +","+
                            "Paytype = '"+ Cmbpricetype.Text +"',"+
                            "Note = '"+ Note.Text +"' "+
                            "Where ID = " + int.Parse(txtpay.Text);
                CF.Deletebyid(Qst);
                if (Cmbpricetype.Text == "نقدي")
                {
                    var QPay = @"Update Payment 
                            set
                            Cash = "+ double.Parse(txtprice.Text)+"," +
                            "Bank = " + 0.00 + "," +
                            "Paid = " + double.Parse(txtprice.Text) + "," +
                            "Remaining = " + 0.00 + "," +
                            "Date = '" + date.Value.ToString("yyyy-MM-dd") + "'," +
                            "Time = '" + DateTime.Now.ToString("HH:mm:ss") + "'," +
                            "ThirdPartyID = '" + Clients.SelectedValue + "'," +
                            "PaymentMethod = '" + Cmbpricetype.Text + "'," +
                            "Type = '" + Vochertypes.Text + "' " +
                            "Note = '" + Note.Text + " Where paymentNo = " + Transbynum;
                    CF.Deletebyid(QPay);
                }
                if (Cmbpricetype.Text == "بنكي")
                {
                    var QPay = @"Update Payment 
                            set
                            Cash = " + 0.00+ "," +
                            "Bank = " + double.Parse(txtprice.Text) + "," +
                            "Paid = " + double.Parse(txtprice.Text) + "," +
                            "Remaining = " + 0.00 + "," +
                            "Date = '" + date.Value.ToString("yyyy-MM-dd") + "'," +
                            "Time = '" + DateTime.Now.ToString("HH:mm:ss") + "'," +
                            "ThirdPartyID = '" + Clients.SelectedValue + "'," +
                            "PaymentMethod = '" + Cmbpricetype.Text + "'," +
                            "Type = '" + Vochertypes.Text + "' " +
                            "Note = '" + Note.Text + " Where paymentNo = " + Transbynum;
                    CF.Deletebyid(QPay);
                }
                Clearfieldes();
                MessageBox.Show("تم تعديل السند بنجاح","تعديل");
                Getvouchersave();
            }
            else
            {
                Addvouchers();
            }
        }
        public void Addvouchers()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (Vochertypes.Text == "سند ايصال مبيعات")
                {
                    Pm.InvoiceNo = int.Parse(Invnum.Text);
                    Pm.Remaining = Creditor;
                }
                Pm.Date = date.Value.ToString("yyyy-MM-dd");
                Pm.Time = DateTime.Now.ToString("HH:mm:ss");
                Pm.ThirdPartyID = int.Parse(Clients.SelectedValue.ToString());
                var Price = double.Parse(txtprice.Text);
                if (Cmbpricetype.Text == "نقدي")
                {
                    Pm.Cash = Price;
                    Pm.Bank = 0.00;
                    Pm.Remaining = 0.00;
                }
                if (Cmbpricetype.Text == "بنكي")
                {
                    Pm.Cash = 0.00;
                    Pm.Bank = Price;
                    Pm.Remaining = 0.00;
                }
                Pm.Type = Vochertypes.Text;
                if (Pm.InvoiceNo != 0)
                {
                    if (Pm.Paid > Pm.Remaining)
                    {
                        MessageBox.Show("برجاء العلم أن المبلغ المدفوع اكبر من المتبقي علما بأن المتبقي هو : " + Pm.Remaining + " ", "خطأ");
                        return;
                    }
                    else
                    {
                        Pm.Remaining = Pm.Remaining - Pm.Paid;
                        Pm.PaymentMethod = "اجل";
                    }
                }
                Pm.Paid = Price;
                Pm.PaymentMethod = Cmbpricetype.Text;
                Pm.Note = Note.Text;
                Pm.Savepayment();
                Pm.GetPaybyid();
                AddTransactions();
                Printvoucher(Pm.paymentNo);
                Clearfieldes();
                Cursor = Cursors.Default;
                MessageBox.Show("تم حفظ السند بنجاح", "تم");
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "Addvouchers" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void AddTransactions()
        {
            try
            {
                if (Cmbpricetype.Text == "نقدي")
                {
                    var Qurcash = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype,Note) Values(" +
                                    "" + Pm.InvoiceNo + "," +
                                    "" + Pm.paymentNo + "," +
                                    "'" + Pm.Date + "'," +
                                    "'" + Vochertypes.Text + "'," +
                                    "" + Pm.ThirdPartyID + ", " +
                                    "" + Pm.Cash + ", " +
                                    "'" + "نقدي" + "', " +
                                    "'" + Note.Text + "' " +
                                    ")";
                    trans.Crudopration(Qurcash);
                }
                if (Cmbpricetype.Text == "بنكي")
                {
                    var Qurbank = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype,Note) Values(" +
                                    "" + Pm.InvoiceNo + "," +
                                    "" + Pm.paymentNo + "," +
                                    "'" + Pm.Date + "'," +
                                    "'" + Vochertypes.Text + "'," +
                                    "" + Pm.ThirdPartyID + ", " +
                                    "" + Pm.Bank + ", " +
                                    "'" + "بنكي" + "', " +
                                    "'" + Note.Text + "' " +
                                    ")";
                    trans.Crudopration(Qurbank);
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "AddTransactions" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void DGSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var vt = DGSales.CurrentRow.Cells[4].Value.ToString();
            if (DGSales.Rows.Count > 0)
            {
                var Paytype = DGSales.CurrentRow.Cells[5].Value.ToString();
                if (vt != "فاتورة مبيعات")
                {

                    Btnsave.Enabled = true;
                    Btnsave.IconChar = FontAwesome.Sharp.IconChar.Pen;
                    Btnsave.Text = "تعديل";
                    Btnsave.BackColor = Color.FromArgb(255, 184, 128);
                }
                else
                {
                    Btnsave.Enabled = false;
                }
                Pm.paymentNo = int.Parse(DGSales.CurrentRow.Cells[0].Value.ToString());
                txtpay.Text = Pm.paymentNo.ToString();
                Pm.InvoiceNo = int.Parse(DGSales.CurrentRow.Cells[1].Value.ToString());
                txtinv.Text = Pm.InvoiceNo.ToString();
                Transbynum = int.Parse(DGSales.CurrentRow.Cells[2].Value.ToString());
                date.Text = DGSales.CurrentRow.Cells[3].Value.ToString();
                Clients.Text = DGSales.CurrentRow.Cells[5].Value.ToString();
                Vochertypes.Text = DGSales.CurrentRow.Cells[4].Value.ToString();
                txtprice.Text = DGSales.CurrentRow.Cells[6].Value.ToString();
                Cmbpricetype.Text = DGSales.CurrentRow.Cells[7].Value.ToString();
                Note.Text = DGSales.CurrentRow.Cells[8].Value.ToString();
            }
        }
        private void Vochertypes_SelectedValueChanged(object sender, EventArgs e)
        {
            //Getinvoices();
        }
        private void Btnclear_Click(object sender, EventArgs e)
        {
            Clearfieldes();
        }
        private void Btndelete_Click(object sender, EventArgs e)
        {
            if (Pm.paymentNo == 0)
            {
                MessageBox.Show("برجاء اختيار السند", "الحذف");
                return;
            }
            else
            {
                var Msg = MessageBox.Show("هل تريد حذف السند", "الحذف", MessageBoxButtons.YesNo);
                if (Msg == DialogResult.Yes)
                {
                    var Qst = @"Delete From Transactions Where ID = " + int.Parse(txtpay.Text);
                    var QPay = @"Delete From Payment Where paymentNo = " + int.Parse(txtinv.Text);
                    CF.Deletebyid(Qst);
                    CF.Deletebyid(QPay);
                    MessageBox.Show("تم حذف السند بنجاح", "الحذف");
                    Clearfieldes();
                }
                else
                {
                    return;
                }
            }
        }
        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (Pm.paymentNo == 0)
            {
                MessageBox.Show("برجاء اختيار السند", "الحذف");
                return;
            }
            else
            {
                Printvoucher(Pm.paymentNo);
            }
        }
        private void Vochertypes_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Getinvoices();
        }
        private void Vochertype_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Vochertype.SelectedIndex != 0)
            {
                if (Vochertype.Text == "سند دفع لمورد")
                {
                    if (Thirtype != "1")
                    {
                        MessageBox.Show("برجاء اختيار مورد وليس عميل", "خطأ");
                        Vochertype.SelectedIndex = 0;
                        return;
                    }
                }
                if (Vochertype.Text == "سند دفع لعميل")
                {
                    if (Thirtype != "2")
                    {
                        MessageBox.Show("برجاء اختيار عميل وليس مورد", "خطأ");
                        Vochertype.SelectedIndex = 0;
                        return;
                    }
                    else
                    {

                    }
                }
                if (Vochertype.Text == "سند ايصال مشتريات")
                {
                    if (Thirtype != "1")
                    {
                        MessageBox.Show("برجاء اختيار مورد وليس عميل", "خطأ");
                        Vochertype.SelectedIndex = 0;
                        return;
                    }
                    else
                    {
                        Getreciver();
                        lblbill.Visible = true;
                        Billnumber.Visible = true;
                    }
                }
            }
            else
            {
                lblbill.Visible = false;
                Billnumber.Visible = false;
                Vochertype.SelectedIndex = 0;
                return;
            }
        }
        private void Thirdparty_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var CID = int.Parse(CBMThirdparty.SelectedValue.ToString());
            Cust.Getdatabyid(CID);
            Thirtype = Cust.Type.ToString();
        }
        private void Btnadd_Click(object sender, EventArgs e)
        {
            if (Btnadd.Text == "تعديل")
            {
                var Qst = @"Update Transactions 
                            set
                            TDate = '" + Purdate.Value.ToString("yyyy-MM-dd") + "'," +
                            "Type = '" + Vochertype.Text + "'," +
                            "ThirdPartyID = " + CBMThirdparty.SelectedValue + "," +
                            "Paid = " + double.Parse(txtmony.Text) + "," +
                            "Paytype = '" + CmbPaymethod.Text + "'," +
                            "Note = '" + Purnottxt.Text + "' " +
                            "Where Invoiceno = " + int.Parse(txtpayout.Text);
                CF.Deletebyid(Qst);
                if (CmbPaymethod.Text == "نقدي")
                {
                    var QPay = @"Update paymentout 
                            set
                            Cash = " + double.Parse(txtmony.Text) + "," +
                            "Bank = " + 0.00 + "," +
                            "Paid = " + double.Parse(txtmony.Text) + "," +
                            "Remaining = " + 0.00 + "," +
                            "Date = '" + Purdate.Value.ToString("yyyy-MM-dd") + "'," +
                            "Time = '" + DateTime.Now.ToString("HH:mm:ss") + "'," +
                            "ThirdPartyID = '" + CBMThirdparty.SelectedValue + "'," +
                            "PaymentMethod = '" + CmbPaymethod.Text + "'," +
                            "Type = '" + Vochertype.Text + "', " +
                            "Note = '" + Purnottxt.Text + "' Where paymentNo = " + int.Parse(txtpayout.Text);
                    CF.Deletebyid(QPay);
                }
                if (Cmbpricetype.Text == "بنكي")
                {
                    var QPay = @"Update paymentout 
                            set
                            Cash = " + 0.00 + "," +
                           "Bank = " + double.Parse(txtprice.Text) + "," +
                           "Paid = " + double.Parse(txtprice.Text) + "," +
                           "Remaining = " + 0.00 + "," +
                           "Date = '" + Purdate.Value.ToString("yyyy-MM-dd") + "'," +
                           "Time = '" + DateTime.Now.ToString("HH:mm:ss") + "'," +
                           "ThirdPartyID = '" + CBMThirdparty.SelectedValue + "'," +
                           "PaymentMethod = '" + CmbPaymethod.Text + "'," +
                           "Type = '" + Vochertype.Text + "', " +
                           "Note = '" + Purnottxt.Text + "' Where paymentNo = " + int.Parse(txtpayout.Text);
                    CF.Deletebyid(QPay);
                }
                Clearfieldes();
                MessageBox.Show("تم تعديل السند بنجاح", "تعديل");
                Getvouchersave();
            }
            else
            {
                AddReciver();
            }
        }
        public void AddReciver()
        {
            try
            {
                Pm.Date = Purdate.Value.ToString("yyyy-MM-dd");
                Pm.Time = DateTime.Now.ToString("HH:mm:ss");
                Pm.ThirdPartyID = int.Parse(CBMThirdparty.SelectedValue.ToString());
                if (CmbPaymethod.Text == "نقدي")
                {
                    Pm.Cash = double.Parse(txtmony.Text);
                    Pm.Bank = 0.00;
                }
                else
                {
                    Pm.Cash = 0.00;
                    Pm.Bank = double.Parse(txtmony.Text);
                }
                Pm.Paid = double.Parse(txtmony.Text);
                Pm.Type = Vochertype.Text;
                Pm.PaymentMethod = CmbPaymethod.Text;
                Pm.Note = Purnottxt.Text;
                Pm.Savepaymentout();
                Pm.GetPayoutbyid();
                var Qur = @"Insert Into Transactions(Invoiceno,TDate,Type,ThirdPartyID,Paid,Paytype,Note) Values(" +
                "" + Pm.paymentNo + "," +
                "'" + Pm.Date + "'," +
                "'" + Pm.Type + "'," +
                "" + Pm.ThirdPartyID + ", " +
                "" + double.Parse(txtmony.Text) + "," +
                "'" + CmbPaymethod.Text + "'," +
                "'" + Purnottxt.Text + "' " +
                ")";
                trans.Crudopration(Qur);
                Printreciver(Pm.paymentNo);
                Clearfieldes();
                MessageBox.Show("تم حفظ السند بنجاح", "تم");
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاشعارات" + "','" + "AddReciver" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void DGVPur_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Btnadd.IconChar = FontAwesome.Sharp.IconChar.Pen;
            //Btnadd.Text = "تعديل";
            //Btnadd.BackColor = Color.FromArgb(255, 184, 128);
            //if (DGSales.Rows.Count > 0)
            //{
            //    var invoiceNo = int.Parse(DGVPur.CurrentRow.Cells[1].Value.ToString());
            //    Btnadd.IconChar = FontAwesome.Sharp.IconChar.Pen;
            //    Btnadd.Text = "تعديل";
            //    Btnadd.BackColor = Color.FromArgb(255, 184, 128);
            //}
        }
        private void DGVPur_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGVPur.Rows.Count > 0)
            {
                var Paytype = DGVPur.CurrentRow.Cells[6].Value.ToString();
                if (Paytype != "فاتورة مشتريات")
                {
                    Btnadd.IconChar = FontAwesome.Sharp.IconChar.Pen;
                    Btnadd.Text = "تعديل";
                    Btnadd.BackColor = Color.FromArgb(255, 184, 128);
                    Pm.paymentNo = int.Parse(DGVPur.CurrentRow.Cells[0].Value.ToString());
                    txtpayout.Text = Pm.paymentNo.ToString();
                    Pm.InvoiceNo = int.Parse(DGVPur.CurrentRow.Cells[1].Value.ToString());
                    txtinvnum.Text = Pm.InvoiceNo.ToString();
                    Purdate.Text = DGVPur.CurrentRow.Cells[2].Value.ToString();
                    CBMThirdparty.Text = DGVPur.CurrentRow.Cells[3].Value.ToString();
                    txtmony.Text = DGVPur.CurrentRow.Cells[4].Value.ToString();
                    Vochertype.Text = DGVPur.CurrentRow.Cells[6].Value.ToString();
                    CmbPaymethod.Text = DGVPur.CurrentRow.Cells[5].Value.ToString();
                    Purnottxt.Text = DGVPur.CurrentRow.Cells[7].Value.ToString();
                }
                else
                {
                    MessageBox.Show("لا يمكن تعديل او حذف فواتير مشتريات", "خطأ");
                    return;
                }
            }
        }
        private void Btnempty_Click(object sender, EventArgs e)
        {
            Clearfieldes();
        }
        private void Btndel_Click(object sender, EventArgs e)
        {
            if (Pm.paymentNo == 0)
            {
                MessageBox.Show("برجاء اختيار السند", "الحذف");
                return;
            }
            else
            {
                var Msg = MessageBox.Show("هل تريد حذف السند", "الحذف", MessageBoxButtons.YesNo);
                if (Msg == DialogResult.Yes)
                {
                    var Qst = @"Delete From Transactions Where Invoiceno = " + Pm.paymentNo;
                    var QPay = @"Delete From Paymentout Where paymentNo = " + Pm.paymentNo;
                    CF.Deletebyid(Qst);
                    CF.Deletebyid(QPay);
                    MessageBox.Show("تم حذف السند بنجاح", "الحذف");
                    Clearfieldes();
                }
                else
                {
                    return;
                }
            }
        }
        private void Btnprint_Click(object sender, EventArgs e)
        {
            Printreciver(Pm.paymentNo);
        }

        private void txtprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
        }

        private void RBCust_CheckedChanged(object sender, EventArgs e)
        {
            Thirdparties();
        }

        private void RBSup_CheckedChanged(object sender, EventArgs e)
        {
            Thirdparties();
        }
    }
}
