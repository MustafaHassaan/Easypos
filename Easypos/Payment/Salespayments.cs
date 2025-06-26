using Centeralized;
using Dataaccesslaeyr;
using Easypos.Purchases;
using Easypos.Salesforms;
using Easypos.Salesforms.Cashier;
using Easypos.Salesforms.Normal;
using Easypos.Tailoring;
using Easypos.Vouchers;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Querylaeyr.Modelmaster;
using Querylaeyr.Modelsales;
using Querylaeyr.Paymodel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Easypos.Payment
{
    public partial class Salespayments : Form
    {
        Centergetingfunctions CF;
        public Sales sal;
        public Salesdetails sald;
        frmMSalesBill Sb;
        frmPOS Pos;
        Frmarrestvochers Pay;
        frmPurchases Pur;
        public Paymentmodel Pm;
        Transactions trans;
        public Contractors Cntr;
        public int Invnum { get; set; }
        public string Typeprinting { get; set; }
        DataAccess DA;
        public Salespayments()
        {
            InitializeComponent();
            CF = new Centergetingfunctions();
            sal = new Sales();
            Sb = (Application.OpenForms["frmMSalesBill"] as frmMSalesBill);
            Pos = (Application.OpenForms["frmPOS"] as frmPOS);
            Pay = (Application.OpenForms["Frmarrestvochers"] as Frmarrestvochers);
            Pur = (Application.OpenForms["frmPurchases"] as frmPurchases);

            sald = new Salesdetails();
            Pm = new Paymentmodel();
            trans = new Transactions();
            Cntr = new Contractors();
            DA = new DataAccess();
        }
        public void AddReciver()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (Pay.Vochertype.Text == "سند ايصال مشتريات")
                {
                    Pm.InvoiceNo = int.Parse(Pay.Billnumber.Text);
                    Pm.Remaining = Pay.Creditor;
                }
                Pm.Date = Pay.date.Value.ToString("yyyy-MM-dd");
                Pm.Time = DateTime.Now.ToString("HH:mm:ss");
                Pm.ThirdPartyID = int.Parse(Pay.CBMThirdparty.SelectedValue.ToString());
                Pm.Cash = double.Parse(txtCash.Text);
                Pm.Bank = double.Parse(txtBank.Text);
                Pm.Paid = Pm.Cash + Pm.Bank;
                Pm.Type = Pay.Vochertype.Text;
                Pm.PaymentMethod = "نقدي";
                Pm.Note = Pay.Purnottxt.Text;
                Pm.Savepaymentout();
                Pm.GetPayoutbyid();
                var Qur = @"Insert Into Transactions(Invoiceno,TDate,Type,ThirdPartyID) Values(" +
                "" + Pm.paymentNo + "," +
                "'" + Pm.Date + "'," +
                "'" + Pm.Type + "'," +
                "" + Pm.ThirdPartyID + " " +
                ")";
                trans.Crudopration(Qur);
                Pay.Printreciver(Pm.paymentNo);
                Pay.Clearfieldes();
                Cursor = Cursors.Default;
                MessageBox.Show("تم حفظ السند بنجاح", "تم");
                this.Close();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "AddReciver" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void EditReciver()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var Qst = @"Delete From Transactions Where Invoiceno = " + Pm.paymentNo;
                CF.Deletebyid(Qst);
                var Qur = @"Insert Into Transactions(Invoiceno,TDate,Type,ThirdPartyID) Values(" +
                            "" + Pm.paymentNo + "," +
                            "'" + Pm.Date + "'," +
                            "'" + Pm.Type + "'," +
                            "" + Pm.ThirdPartyID + " " +
                            ")";
                trans.Crudopration(Qur);
                Pm.Date = Pay.date.Value.ToString("yyyy-MM-dd");
                Pm.Time = DateTime.Now.ToString("HH:mm:ss");
                Pm.ThirdPartyID = int.Parse(Pay.CBMThirdparty.SelectedValue.ToString());
                Pm.Cash = double.Parse(txtCash.Text);
                Pm.Bank = double.Parse(txtBank.Text);
                Pm.Paid = Pm.Cash + Pm.Bank;
                Pm.Remaining = 0.00;
                Pm.Type = Pay.Vochertype.Text;
                Pm.PaymentMethod = "نقدي";
                Pm.Note = Pay.Purnottxt.Text;
                Pm.Editpaymentout();
                Pay.Clearfieldes();
                Pay.Printreciver(Pm.paymentNo);
                Cursor = Cursors.Default;
                MessageBox.Show("تم تعديل السند بنجاح", "تم");
                this.Close();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "EditReciver" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        
        public void Editvouchers()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (Pay.Vochertypes.Text != "فاتورة مبيعات")
                {
                    var Qst = @"Delete From Transactions Where Invoiceno = " + Pm.paymentNo;
                    CF.Deletebyid(Qst);
                    AddTransactions();
                    Pm.Date = Pay.date.Value.ToString("yyyy-MM-dd");
                    Pm.Time = DateTime.Now.ToString("HH:mm:ss");
                    Pm.ThirdPartyID = int.Parse(clients.SelectedValue.ToString());
                    Pm.Cash = double.Parse(txtCash.Text);
                    Pm.Bank = double.Parse(txtBank.Text);
                    Pm.Paid = Pm.Cash + Pm.Bank;
                    Pm.Remaining = 0.00;
                    Pm.Type = Pay.Vochertypes.Text;
                    Pm.PaymentMethod = "نقدي";
                    Pm.Note = Pay.Note.Text;
                    Pm.Editpayment();
                    Pay.Clearfieldes();
                    Pay.Printvoucher(Pm.paymentNo);
                }
                else
                {
                    var Qst = @"Delete From Transactions Where Invoiceno = " + Pm.InvoiceNo;
                    CF.Deletebyid(Qst);
                    sal.Invoiceno = Pm.InvoiceNo;
                    Pm.Date = Pay.date.Value.ToString("yyyy-MM-dd");
                    Pm.Time = DateTime.Now.ToString("HH:mm:ss");
                    Pm.ThirdPartyID = int.Parse(clients.SelectedValue.ToString());
                    Pm.Cash = double.Parse(txtCash.Text);
                    Pm.Bank = double.Parse(txtBank.Text);
                    Pm.Paid = Pm.Cash + Pm.Bank;
                    Pm.Remaining = 0.00;
                    Pm.Type = Pay.Vochertypes.Text;
                    Pm.PaymentMethod = "نقدي";
                    Pm.Note = Pay.Note.Text;
                    sal.TDate = Pm.Date;
                    Paytype = Pay.Vochertypes.Text;
                    sal.ThirdPartyID = Pm.ThirdPartyID;
                    AddTransactions();
                    Pm.Editpayment();
                    Pay.Clearfieldes();
                }
                Cursor = Cursors.Default;
                MessageBox.Show("تم تعديل السند بنجاح", "تم");
                this.Close();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "Editvouchers" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Editsales()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                sal.Getsalesbyid(Invnum);
                sald.InvoiceNo = sal.Invoiceno;
                Pm.InvoiceNo = sal.Invoiceno;
                sal.Editsales();
                AddPayment();
                Pm.GetPaybyid();
                AddTransactions();
                if (Pos != null)
                {
                    Pos.SaveAndEdit();
                    Pos.Clearfieldes();
                    Pos.Getcustomers();
                    Pos.Invnum = sal.Invoiceno;
                    var Msg = MessageBox.Show("هل تريد طباعة هذه الفاتورة ؟", "رسالة الحفظ", MessageBoxButtons.YesNo);
                    if (Msg == DialogResult.Yes)
                    {
                        Pos.Printinv();
                    }
                }
                if (Sb != null)
                {
                    Sb.SaveAndEdit();
                    AddContractors();
                    if (Typeprinting == "تعديل وطباعه")
                    {
                        Sb.Invnum = sal.Invoiceno;
                        Sb.Printinv();
                    }
                    Sb.Getproducts();
                    Sb.Getunits();
                    Sb.Getcustomers();
                    Sb.DGV.Rows.Clear();
                    Sb.DGSum();
                    Sb.Cleardata();
                    Sb.Btnsave.Text = "حفظ";
                    Sb.Btnsaveandprint.Text = "حفظ وطباعة";
                }
                Cursor = Cursors.Default;
                //MessageBox.Show("تم تعديل الفاتورة بنجاح", "تم");
                this.Close();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "Editsales" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Deletesalesdetailes()
        {
            try
            {
                var Qsd = @"Delete From Salesdetailes Where InvoiceNo = " + Invnum;
                var Qst = @"Delete From Transactions Where Type = 'فاتورة مبيعات' And InvoiceNo = " + Invnum;
                var QPay = @"Delete From Payment Where InvoiceNo = " + Invnum;
                var QCntr = @"Delete From contractors Where InvoiceNo = " + Invnum;
                var QSI = @"Delete From itemsales Where invoiceno = " + Invnum;
                CF.Deletebyid(Qsd);
                CF.Deletebyid(Qst);
                CF.Deletebyid(QPay);
                CF.Deletebyid(QCntr);
                CF.Deletebyid(QSI);
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "Deletesalesdetailes" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Deletepurdetailes()
        {
            try
            {
                var Qsd = @"Delete From purchasedetailes Where InvoiceNo = " + Invnum;
                var Qst = @"Delete From Transactions Where Type = 'فاتورة مشتريات' And InvoiceNo = " + Invnum;
                var QPay = @"Delete From Paymentout Where InvoiceNo = " + Invnum;
                CF.Deletebyid(Qsd);
                CF.Deletebyid(Qst);
                CF.Deletebyid(QPay);
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "Deletepurdetailes" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }

        public void Savesales()
        {
            try
            {
                AddPayment();
                Pm.GetPaybyid();
                //Pm.InvoiceNo = sal.Invoiceno;
                AddTransactions();
                if (Pos != null)
                {
                    var Msg = MessageBox.Show("هل تريد طباعة هذه الفاتورة ؟", "رسالة الحفظ", MessageBoxButtons.YesNo);
                    if (Msg == DialogResult.Yes)
                    {
                        Pos.Printinv();
                    }
                    Pos.Getcustomers();
                    Pos.Clearfieldes();
                    Newtailor open = Application.OpenForms["Newtailor"] as Newtailor;
                    if (open != null)
                    {
                        open.comboBox2.Text = sal.Invoiceno.ToString();
                        open.textBox2.Text = txtTotal.Text;
                    }
                }
                if (Sb != null)
                {
                    AddContractors();
                    if (Typeprinting == "حفظ وطباعة")
                    {
                        Sb.Invnum = sal.Invoiceno;
                        Sb.Printinv();
                    }
                    Sb.Getproducts();
                    Sb.Getunits();
                    Sb.Getcustomers();
                    Sb.DGV.Rows.Clear();
                    Sb.DGSum();
                    Sb.Cleardata();
                    Sb.Btnsave.Text = "حفظ";
                    Sb.Btnsaveandprint.Text = "حفظ وطباعة";
                }
                this.Close();
                //MessageBox.Show("تم حفظ الفاتورة بنجاح", "تم");
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "Savesales" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void EditPurchases()
        {
            try
            {
                sal.Getpurbyid(Invnum);
                sald.InvoiceNo = sal.Invoiceno;
                Pm.InvoiceNo = sal.Invoiceno;
                Pur.SaveAndEdit();
                AddPayout();
                AddTransactions();
                if (Pur != null)
                {
                    if (Typeprinting == "تعديل وطباعه")
                    {
                        Pur.Invnum = sal.Invoiceno;
                        Pur.Printinv();
                    }
                }
                MessageBox.Show("تم تعديل الفاتورة بنجاح", "تم");
                this.Close();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "EditPurchases" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void SavePurchases()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Pm.InvoiceNo = sal.Invoiceno;
                AddPayout();
                AddTransactions();
                if (Pur != null)
                {
                    if (Typeprinting == "حفظ وطباعة")
                    {
                        Pur.Invnum = sal.Invoiceno;
                        Pur.Printinv();
                    }
                }
                this.Close();
                Cursor = Cursors.Default;
                MessageBox.Show("تم حفظ الفاتورة بنجاح", "تم");
                Pur.Clearall();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "SavePurchases" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void AddPayment()
        {
            try
            {
                Pm.InvoiceNo = sal.Invoiceno;
                Pm.Date = sal.TDate;
                Pm.Time = sal.TTime;
                Pm.ThirdPartyID = sal.ThirdPartyID;
                Pm.Cash = double.Parse(txtCash.Text);
                Pm.Bank = double.Parse(txtBank.Text);
                Pm.Paid = Pm.Cash + Pm.Bank;
                var Totres = double.Parse(txtTotal.Text.Trim());
                Pm.Remaining = Totres - Pm.Paid;
                Pm.Type = "فاتورة مبيعات";
                if (Totres == Pm.Paid)
                {
                    Pm.PaymentMethod = "نقدي";
                }
                else
                {
                    Pm.PaymentMethod = "اجل";
                }
                Pm.Savepayment();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "AddPayment" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void AddPayout()
        {
            try
            {
                Pm.Date = sal.TDate;
                Pm.Time = sal.TTime;
                Pm.ThirdPartyID = sal.ThirdPartyID;
                Pm.Cash = double.Parse(txtCash.Text);
                Pm.Bank = double.Parse(txtBank.Text);
                Pm.Paid = Pm.Cash + Pm.Bank;
                var Totres = double.Parse(txtTotal.Text.Trim());
                Pm.Remaining = Totres - Pm.Paid;
                Pm.Type = "فاتورة مشتريات";
                if (Totres == Pm.Paid)
                {
                    Pm.PaymentMethod = "نقدي";
                }
                else
                {
                    Pm.PaymentMethod = "اجل";
                }
                Pm.Savepaymentout();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "AddPayout" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public string Paytype { get; set; }
        public void AddTransactions()
        {
            try
            {
                if (string.IsNullOrEmpty(Paytype))
                {
                    if (Pur != null)
                    {
                        Paytype = "فاتورة مشتريات";
                    }
                    else
                    {
                        Paytype = "فاتورة مبيعات";
                    }
                }
                var Pmcash = double.Parse(txtCash.Text);
                var Pmbank = double.Parse(txtBank.Text);
                var Tot = Pmcash + Pmbank;
                //if (Pmcash != 0 && Pmbank != 0)
                //{
                //    var Qurcash = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype) Values(" +
                //    "" + sal.Invoiceno + "," +
                //    "" + Pm.paymentNo + "," +
                //    "'" + sal.TDate + "'," +
                //    "'" + Paytype + "'," +
                //    "" + sal.ThirdPartyID + ", " +
                //    "" + Pmcash + ", " +
                //    "'" + "نقدي" + "' " +
                //    ")";
                //    trans.Crudopration(Qurcash);

                //    var Qurpmcash = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype) Values(" +
                //                    "" + sal.Invoiceno + "," +
                //                    "" + Pm.paymentNo + "," +
                //                    "'" + sal.TDate + "'," +
                //                    "'" + "سند قبض" + "'," +
                //                    "" + sal.ThirdPartyID + ", " +
                //                    "" + Pmcash + ", " +
                //                    "'" + "نقدي" + "' " +
                //                    ")";
                //    trans.Crudopration(Qurpmcash);

                //    var Qurpmbank = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype) Values(" +
                //                        "" + sal.Invoiceno + "," +
                //                        "" + Pm.paymentNo + "," +
                //                        "'" + sal.TDate + "'," +
                //                        "'" + "سند قبض" + "'," +
                //                        "" + sal.ThirdPartyID + ", " +
                //                        "" + Pmbank + ", " +
                //                        "'" + "بنكي" + "' " +
                //                        ")";
                //    trans.Crudopration(Qurpmbank);
                //}
                if (Pmcash > 0)
                {
                    var Qurcash = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype) Values(" +
                                    "" + sal.Invoiceno + "," +
                                    "" + Pm.paymentNo + "," +
                                    "'" + sal.TDate + "'," +
                                    "'" + Paytype + "'," +
                                    "" + sal.ThirdPartyID + ", " +
                                    "" + Pmcash + ", " +
                                    "'" + "نقدي" + "' " +
                                    ")";
                    trans.Crudopration(Qurcash);


                    //var Qurtrans = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype) Values(" +
                    //"" + sal.Invoiceno + "," +
                    //"" + Pm.paymentNo + "," +
                    //"'" + sal.TDate + "'," +
                    //"'" + "سند قبض" + "'," +
                    //"" + sal.ThirdPartyID + ", " +
                    //"" + Pmcash + ", " +
                    //"'" + "نقدي" + "' " +
                    //")";
                    //trans.Crudopration(Qurtrans);
                }
                if (Pmbank > 0)
                {
                    var Qurbank = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype) Values(" +
                                    "" + sal.Invoiceno + "," +
                                    "" + Pm.paymentNo + "," +
                                    "'" + sal.TDate + "'," +
                                    "'" + Paytype + "'," +
                                    "" + sal.ThirdPartyID + ", " +
                                    "" + Pmbank + ", " +
                                    "'" + "بنكي" + "' " +
                                    ")";
                    trans.Crudopration(Qurbank);



                    //var Qurtrans = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype) Values(" +
                    //"" + sal.Invoiceno + "," +
                    //"" + Pm.paymentNo + "," +
                    //"'" + sal.TDate + "'," +
                    //"'" + "سند قبض" + "'," +
                    //"" + sal.ThirdPartyID + ", " +
                    //"" + Pmbank + ", " +
                    //"'" + "بنكي" + "' " +
                    //")";
                    //trans.Crudopration(Qurtrans);
                }
                //else
                //{
                //    var Qurbank = @"Insert Into Transactions(Invoiceno,Paynum,TDate,Type,ThirdPartyID,Paid,Paytype) Values(" +
                //                    "" + sal.Invoiceno + "," +
                //                    "" + Pm.paymentNo + "," +
                //                    "'" + sal.TDate + "'," +
                //                    "'" + Paytype + "'," +
                //                    "" + sal.ThirdPartyID + ", " +
                //                    "" + Tot + ", " +
                //                    "'" + "اجل" + "' " +
                //                    ")";
                //    trans.Crudopration(Qurbank);
                //}
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "AddTransactions" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void AddContractors()
        {
            try
            {
                var Qur = @"Insert Into Contractors(Purchaseorder,
                                                Refrancenumber,
                                                Projectname,
                                                Invoiceno
                                                ) Values(" +
                                    "'" + Cntr.Purchaseorder + "'," +
                                    "'" + Cntr.Refrancenumber + "'," +
                                    "'" + Cntr.Projectname + "'," +
                                    " " + sal.Invoiceno + ")";
                Cntr.Crudopration(Qur);
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الدفوعات" + "','" + "AddContractors" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void AddCash_Click(object sender, EventArgs e)
        {
            txtCash.Text = (decimal.Parse(txtTotal.Text) - decimal.Parse(txtBank.Text)).ToString();
            if (Pos != null || Sb != null)
            {
                txtRem.Text = Convert.ToString((Convert.ToDouble(txtTotal.Text) - Convert.ToDouble(txtTotalPay.Text)));
            }
        }
        private void AddBank_Click(object sender, EventArgs e)
        {
            txtBank.Text = (decimal.Parse(txtTotal.Text) - decimal.Parse(txtCash.Text)).ToString();
            if (Pos != null || Sb != null)
            {
                txtRem.Text = Convert.ToString((Convert.ToDouble(txtTotal.Text) - Convert.ToDouble(txtTotalPay.Text)));
            }
        }
        private void txtCash_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e,sender);
        }
        private void txtBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
        }
        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCash.Text))
            {
                txtCash.Text = "0";
            }
            if (txtCash.Text == ".")
            {
                return;
            }
            txtTotalPay.Text = Convert.ToString((Convert.ToDouble(txtCash.Text) + Convert.ToDouble(txtBank.Text)));
            if (Pos != null || Sb != null)
            {
                txtRem.Text = Convert.ToString((Convert.ToDouble(txtTotal.Text) - Convert.ToDouble(txtTotalPay.Text)));
            }
        }
        private void txtBank_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBank.Text))
            {
                txtBank.Text = "0";
            }
            if (txtBank.Text == ".")
            {
                return;
            }
            txtTotalPay.Text = Convert.ToString((Convert.ToDouble(txtCash.Text) + Convert.ToDouble(txtBank.Text)));
            if (Pos != null || Sb != null)
            {
                txtRem.Text = Convert.ToString((Convert.ToDouble(txtTotal.Text) - Convert.ToDouble(txtTotalPay.Text)));
            }
        }
        private void Btnsave_Click(object sender, EventArgs e)
        {
            if (Pay == null)
            {
                if (!string.IsNullOrEmpty(txtTotalPay.Text) && !string.IsNullOrEmpty(txtTotal.Text))
                {

                    var Totpayment = double.Parse(txtTotalPay.Text);
                    var Total = double.Parse(txtTotal.Text);
                    if (clients.SelectedValue.ToString() == "10")
                    {
                        if (Totpayment < Total)
                        {
                            MessageBox.Show("لا يمكن البيع بالأجل لعميل افتراضي","خطأ");
                            return;
                        }
                    }
                    if (Totpayment > Total)
                    {
                        MessageBox.Show("لا يمكن دفع هذا المبلغ لأنه اكبر من سعر الفاتورة", "خطأ");
                        return;
                    }
                    else
                    {
                        if (Pur != null)
                        {
                            if (Typeprinting == "حفظ" || Typeprinting == "حفظ وطباعه")
                            {
                                SavePurchases();
                            }
                            else
                            {
                                Deletepurdetailes();
                                EditPurchases();
                            }
                        }
                        else
                        {
                            if (Typeprinting == "حفظ" || Typeprinting == "حفظ وطباعة" || Typeprinting == "حفظ كاشير")
                            {
                                if (Pos != null)
                                {
                                    Pos.Paid = txtTotalPay.Text;
                                    Pos.Dept = txtRem.Text;
                                    Pos.Cashieroprations();
                                }
                                if (Sb != null)
                                {
                                    Sb.Salesoprations();
                                }
                                Savesales();
                            }
                            else
                            {
                                if (Invnum == 0)
                                {
                                    Invnum = sal.Invoiceno;
                                }
                                Deletesalesdetailes();
                                Editsales();
                            }
                        }
                    }
                }
            }
            else
            {
                // سندات القبض
                if (Pay.TC.SelectedTab == Pay.TC.TabPages["TPRecive"])
                {
                    if (Pay.Btnsave.Text == "حفظ")
                    {
                        //Addvouchers();
                    }
                    else
                    {
                        Editvouchers();
                    }
                }
                else
                {
                    if (Pay.Btnadd.Text == "حفظ")
                    {
                        AddReciver();
                    }
                    else
                    {
                        EditReciver();
                    }
                }
            }
        }
        private void Salespayments_Load(object sender, EventArgs e)
        {
            if (Pay != null)
            {
                // سندات القبض
                if (Pay.TC.SelectedTab == Pay.TC.TabPages["TPRecive"])
                {
                    lblTitle.Text = Pay.Vochertypes.Text;
                    if (Pay.Vochertypes.SelectedIndex == 0)
                    {
                        MessageBox.Show("برجاء اختيار نوع السند", "خطأ");
                        this.Close();
                        return;
                    }
                    this.clients.DataSource = Pay.Clients.DataSource;
                    this.clients.SelectedValue = Pay.Clients.SelectedValue;
                    AddBank.Visible = false;
                    AddCash.Visible = false;
                    if (Pay.Clients.SelectedIndex == 0)
                    {
                        MessageBox.Show("برجاء اختيار العميل", "خطأ");
                        this.Close();
                        return;
                    }
                    Label1.Visible = false;
                    txtTotal.Visible = false;
                }
                // سندات الدفع
                else
                {
                    lblTitle.Text = Pay.Vochertype.Text;
                    if (Pay.Vochertype.SelectedIndex == 0)
                    {
                        MessageBox.Show("برجاء اختيار نوع السند", "خطأ");
                        this.Close();
                        return;
                    }
                    this.clients.DataSource = Pay.CBMThirdparty.DataSource;
                    this.clients.SelectedValue = Pay.CBMThirdparty.SelectedValue;
                    AddBank.Visible = false;
                    AddCash.Visible = false;
                    if (Pay.CBMThirdparty.SelectedIndex == 0)
                    {
                        MessageBox.Show("برجاء اختيار العميل او المورد", "خطأ");
                        this.Close();
                        return;
                    }
                    Label1.Visible = false;
                    txtTotal.Visible = false;
                }
            }
        }
    }
}
