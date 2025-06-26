using Centeralized;
using Dataaccesslaeyr;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Querylaeyr.Accountant;
using Querylaeyr.Modelmaster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easypos.TransactionsAccountant
{
    public partial class Acountfilter : Form
    {
        Centergetingfunctions CF;
        Thirdparty thir;
        DataAccess Da;
        List<Accountanat> ACL;
        public Acountfilter()
        {
            InitializeComponent();
            CF = new Centergetingfunctions();
            thir = new Thirdparty();
            ACL = new List<Accountanat>(); 
            Da = new DataAccess();
        }
        private void Acountfilter_Load(object sender, EventArgs e)
        {
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (!RBCust.Checked && !RBSup.Checked)
            {
                MessageBox.Show("برجاء اختيار نوع الكشف","خطأ");
                return;
            }
            else
            {
                Customeraccount CA = new Customeraccount();
                CA.textBox6.Text = DTT.Value.ToString("yyyy-MM-dd");
                CA.textBox5.Text = DTF.Value.ToString("yyyy-MM-dd");
                if (RBCust.Checked)
                {
                    CA.Accountname = RBCust.Text;
                    CF.GetAccountantlist(int.Parse(clients.SelectedValue.ToString()),
                     CA.textBox5.Text,
                     CA.textBox6.Text);
                    CF.GetBalance(int.Parse(clients.SelectedValue.ToString()),
                                         CA.textBox5.Text);
                    ACL = CF.AC;
                    CA.DGV.Rows.Add(null, null, "رصيد افتتاحي", 0.00, CF.Balance, CF.Balance);
                    foreach (var item in ACL)
                    {
                        CA.textBox1.Text = item.MobileNumber;
                        CA.textBox2.Text = item.Name;
                        CA.textBox3.Text = item.Address;
                        CA.textBox4.Text = item.Taxnumber;
                        var BT = item.Billtype;
                        if (BT != "مسوده")
                        {
                            var BN = 0;
                            if (item.Billtype == "فاتورة مبيعات")
                            {
                                BN = item.Invoiceno;
                            }
                            else if(item.Billtype == "سند قبض")
                            {
                                BN = item.ID;
                            }
                            decimal Creditor = 0;
                            decimal Dibtor = 0;
                            if (item.Billtype == "فاتورة مبيعات")
                            {
                                Creditor = item.TotalAmount;
                                Dibtor = 0;
                            }
                            if (item.Billtype == "سند قبض")
                            {
                                Dibtor = item.Paid;
                                Creditor = 0;
                            }
                            CA.DGV.Rows.Add(BN, item.TDate, item.Type, Dibtor, Creditor, 0.00);
                        }
                    }
                    Double Balance = 0.00;
                    for (int i = 0; i < CA.DGV.Rows.Count; i++)
                    {
                        var Det = CA.DGV.Rows[i].Cells[2].Value.ToString();
                        var Tot = Convert.ToDouble(CA.DGV.Rows[i].Cells[3].Value.ToString());
                        var Pay = Convert.ToDouble(CA.DGV.Rows[i].Cells[4].Value.ToString());
                        if (Det == "رصيد افتتاحي")
                        {
                            Balance = Pay;
                            var GBalnce = Math.Round(Balance, 2);
                            CA.DGV.Rows[i].Cells[5].Value = GBalnce;
                        }
                        if (Det == "فاتورة مبيعات")
                        {
                            Balance = Balance + (Tot - Pay);
                            var GBalnce = Math.Round(Balance, 2);
                            CA.DGV.Rows[i].Cells[5].Value = GBalnce;
                        }
                        if (Det == "سند قبض")
                        {
                            Balance = Balance + Tot;
                            var GBalnce = Math.Round(Balance, 2);
                            CA.DGV.Rows[i].Cells[5].Value = GBalnce;
                        }
                        if (Det == "فاتورة مرتجع")
                        {
                            Balance = Balance + Pay;
                            var GBalnce = Math.Round(Balance, 2);
                            //CA.DGV.Rows[i].Cells[5].Value = GBalnce;
                        }
                        if (Det == "سند صرف")
                        {
                            Balance = Balance - Tot;
                            var GBalnce = Math.Round(Balance, 2);
                            //CA.DGV.Rows[i].Cells[5].Value = GBalnce;
                        }
                    }
                }
                if (RBSup.Checked)
                {
                    CA.Accountname = RBSup.Text;
                    CF.GetAccountantpayoutlist(int.Parse(clients.SelectedValue.ToString()),
                     CA.textBox5.Text,
                     CA.textBox6.Text);
                    CF.GetBalance(int.Parse(clients.SelectedValue.ToString()),
                                         CA.textBox5.Text);
                    ACL = CF.AC;
                    CA.DGV.Rows.Add(null, null, "رصيد افتتاحي", 0.00, CF.Balance, CF.Balance);
                    foreach (var item in ACL)
                    {
                        CA.textBox1.Text = item.MobileNumber;
                        CA.textBox2.Text = item.Name;
                        CA.textBox3.Text = item.Address;
                        CA.textBox4.Text = item.Taxnumber;
                        var BT = item.Billtype;
                        if (BT != "مسوده")
                        {
                            var BN = 0;
                            if (item.Invoiceno == 0)
                            {
                                BN = item.ID;
                            }
                            else
                            {
                                BN = item.Invoiceno;
                            }
                            CA.DGV.Rows.Add(BN, item.TDate, item.Type, item.TotalAmount, item.Paid, null);
                        }
                    }
                    Double Balance = 0.00;
                    for (int i = 0; i < CA.DGV.Rows.Count; i++)
                    {
                        var Det = CA.DGV.Rows[i].Cells[2].Value.ToString();
                        var Tot = Convert.ToDouble(CA.DGV.Rows[i].Cells[3].Value.ToString());
                        var Pay = Convert.ToDouble(CA.DGV.Rows[i].Cells[4].Value.ToString());
                        if (Det == "رصيد افتتاحي")
                        {
                            Balance = Pay;
                            var GBalnce = Math.Round(Balance, 2);
                            CA.DGV.Rows[i].Cells[5].Value = GBalnce;
                        }
                        if (Det == "فاتورة مشتريات")
                        {
                            Balance = Balance + (Tot - Pay);
                            var GBalnce = Math.Round(Balance, 2);
                            CA.DGV.Rows[i].Cells[5].Value = GBalnce;
                        }
                        if (Det == "فاتورة مرتجع")
                        {
                            Balance = Balance + Pay;
                            var GBalnce = Math.Round(Balance, 2);
                            CA.DGV.Rows[i].Cells[5].Value = GBalnce;
                        }
                        if (Det == "سند صرف")
                        {
                            Balance = Balance - Tot;
                            var GBalnce = Math.Round(Balance, 2);
                            CA.DGV.Rows[i].Cells[5].Value = GBalnce;
                        }
                    }
                }
                CA.Show();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Phonenumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
        }
        public void Getthirdparties()
        {
            try
            {
                DataTable GD = new DataTable();
                if (RBCust.Checked)
                {
                    GD = CF.Getthirdparties();
                }
                else if (RBSup.Checked)
                {
                    GD = CF.GetSuppliers();
                }
                else
                {
                    MessageBox.Show("برجاء اختيار العميل او المورد", "خطأ");
                    return;
                }
                DataRow dr = GD.NewRow();
                dr["ID"] = 0;
                dr["Name"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                clients.DataSource = GD;
                clients.ValueMember = "ID";
                clients.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة فلترة الحسابات" + "','" + "Getthirdparties" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void Phonenumber_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Phonenumber.Text))
            {
                var GDT = thir.Getdatabynum(Phonenumber.Text);
                clients.DataSource = GDT;
            }
            else
            {
                Getthirdparties();
            }
        }
        private void RBCust_CheckedChanged(object sender, EventArgs e)
        {
            Getthirdparties();
        }
        private void RBSup_CheckedChanged(object sender, EventArgs e)
        {
            Getthirdparties();
        }
    }
}
