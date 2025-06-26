using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;

using MySql.Data.MySqlClient;
using Querylaeyr.Modelmaster;
using System.Windows.Media;
using Centeralized;
using Easypos.Tailoring;
using Easypos.Salesforms.Cashier;
using Easypos.Salesforms.Normal;


namespace Easypos.Masters
{
    public partial class frmListThirdParty : Form
    {
        public int catgoryID;
        public int type;
        Centergetingfunctions _ON;
        Thirdparty thir;
        CompanyInfo CI;
        public frmListThirdParty()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnsave.Text = "Add";
            }
            else
            {
                Btnsave.Text = "أضافة";
            }
            _ON = new Centergetingfunctions();
            thir = new Thirdparty();
        }
        public void Changelang()
        {
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                SetCulture("en");
            }
            else
            {
                SetCulture("ar");
            }
        }
        public void SetCulture(string cultureName)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture =
               System.Globalization.CultureInfo.GetCultureInfo(cultureName);
            var resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            GetChildren(this).ToList().ForEach(c => {
                resources.ApplyResources(c, c.Name);
            });
            this.Controls.Clear();
        }
        public IEnumerable<Control> GetChildren(Control control)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetChildren(ctrl)).Concat(controls);
        }
        public void LoadThirdData()
        {
            var Th = thir.Getdata();
            DGV.DataSource = Th;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            Clearfieldes();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmListThirdParty_Load(object sender, EventArgs e)
        {
            LoadThirdData();
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void radioAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadThirdData();
        }
        private void radioSuppliers_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                var DT = thir.Getdatabycustomersupplier(textBox2.Text, 1);
                DGV.DataSource = DT;
            }
            else
            {
                var GTPD = thir.Getdatabysuppliers();
                DGV.DataSource = GTPD;
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        public void Clearfieldes()
        {
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnsave.Text = "Add";
            }
            else
            {
                Btnsave.Text = "أضافة";
            }
            txtNumber.Text = string.Empty;
            txtName.Clear();
            txtAddress.Clear();
            txtCity.Clear();
            //txtEmail.Clear();
            textBox4.Clear();
            textBox3.Clear();
            textBox2.Clear();
            textBox1.Clear();
            txtOpeningBalance.Clear();
            txtMobile.Clear();
            txtComments.Clear();
            radioSuppliers.Checked = false;
            radioSupplier.Checked = false;
            radioClients.Checked = false;
            radioClient.Checked = false;
            radioAll.Checked = true;
            DGV.DataSource = null;
            DGV.Rows.Clear();
            if (Application.OpenForms["Frmtailoring"] == null &&
                Application.OpenForms["frmPOS"] == null &&
                Application.OpenForms["frmMSalesBill"] == null)
            {
                LoadThirdData();
                DGV.Columns[0].HeaderText = "رقم الموزع/العميل";
                DGV.Columns[1].HeaderText = "الاسم";
                DGV.Columns[2].HeaderText = "رقم الهاتف";
                DGV.Columns[3].HeaderText = "رقم الجوال";
                DGV.Columns[4].HeaderText = "البريد الالكتروني";
                DGV.Columns[5].HeaderText = "العنوان";
                DGV.Columns[6].HeaderText = "المدينه";
                DGV.Columns[7].HeaderText = "الرقم الضريبي";
                DGV.Columns[8].HeaderText = "رقم الدفتر";
                DGV.Columns[9].HeaderText = "رقم الفاتورة";
                DGV.Columns[10].HeaderText = "Typedata";
                DGV.Columns[10].Visible = false;
                DGV.Columns[11].HeaderText = "Comments";
                DGV.Columns[11].Visible = false;
                DGV.Columns[12].HeaderText = "الرصيد الافتتاحي";
            }
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnsave.Text = "Edit";
            }
            else
            {
                Btnsave.Text = "تعديل";
            }
            if (DGV.Rows.Count > 0)
            {
                txtNumber.Text = DGV.CurrentRow.Cells[0].Value.ToString();
                txtName.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                txtMobile.Text = DGV.CurrentRow.Cells[3].Value.ToString();
                txtAddress.Text = DGV.CurrentRow.Cells[4].Value.ToString();
                txtCity.Text = DGV.CurrentRow.Cells[5].Value.ToString();
                txtComments.Text = DGV.CurrentRow.Cells[6].Value.ToString();
                textBox4.Text = DGV.CurrentRow.Cells[10].Value.ToString();
                textBox3.Text = DGV.CurrentRow.Cells[11].Value.ToString();
                // Type Member
                var TM = DGV.CurrentRow.Cells[7].Value.ToString();
                if (TM == "2")
                {
                    radioClient.Checked = true;
                }
                else
                {
                    radioSupplier.Checked = true;
                }
                txtOpeningBalance.Text = DGV.CurrentRow.Cells[9].Value.ToString();
            }
            else
            {
                return;
            }
        }
        private void Btnsave_Click(object sender, EventArgs e)
        {
            if (!radioClient.Checked && !radioSupplier.Checked)
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please enter the type used.", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("من فضلك ادخل النوع المستخدم", "خطأ");
                    return;
                }
            }
            if (txtName.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert name", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("من فضلك ادخل الاسم", "خطأ");
                    return;
                }
            }
            if (txtMobile.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert Pone no.", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("من فضلك ادخل رقم الجوال", "خطأ");
                    return;
                }
            }
            if (txtOpeningBalance.Text == "")
            {
                txtOpeningBalance.Text = "0";
            }
            thir.Name = txtName.Text;
            thir.MobileNumber = txtMobile.Text;
            thir.Address = txtAddress.Text;
            thir.City = txtCity.Text;
            thir.Taxnumber = textBox4.Text;
            thir.Billnumber = textBox3.Text;
            thir.Comments = txtComments.Text;
            if (radioClient.Checked)
            {
                thir.Type = 2;
            }
            else
            {
                thir.Type = 1;
            }
            if (Btnsave.Text == "أضافة" || Btnsave.Text == "Add")
            {
                var Qur = @"INSERT INTO Thirdparty(Name,MobileNumber,Address,City,Taxnumber,Billnumber,Comments,Type)
                                              VALUES('" + thir.Name + "', " +
                          "'" + thir.MobileNumber + "'," +
                          "'" + thir.Address + "'," +
                          "'" + thir.City + "', " +
                          "'" + thir.Taxnumber + "', " +
                          "'" + thir.Billnumber + "', " +
                          "'" + thir.Comments + "', " +
                          " " + thir.Type + ")";
                thir.Crudopration(Qur);

                if (radioClient.Checked)
                {
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Successfully added", "Add Customer");
                    }
                    else
                    {
                        MessageBox.Show("تم الاضافة بنجاح", "اضافة عميل");
                    }
                }
                else
                {
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Successfully added", "Add Supplier");
                    }
                    else
                    {
                        MessageBox.Show("تم الاضافة بنجاح", "اضافة موزع");
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtNumber.Text))
                {
                    var Qur = @"Update Thirdparty Set
                            Name = '" + thir.Name + "'," +
                            "MobileNumber = '" + thir.MobileNumber + "'," +
                            "Address = '" + thir.Address + "'," +
                            "City = '" + thir.City + "', " +
                            "Taxnumber = '" + thir.Taxnumber + "', " +
                            "Billnumber = '" + thir.Billnumber + "', " +
                            "Comments = '" + thir.Comments + "', " +
                            "Type =  " + thir.Type + " " +
                            "Where ID = " + int.Parse(txtNumber.Text) + " ";
                    thir.Crudopration(Qur);
                    Clearfieldes();
                    if (radioClient.Checked)
                    {
                        if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                        {
                            MessageBox.Show("Successfully Edit", "Add Customer");
                        }
                        else
                        {
                            MessageBox.Show("تم التعديل بنجاح", "تعديل عميل");
                        }
                    }
                    else
                    {
                        if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                        {
                            MessageBox.Show("Successfully Edit", "Add Supplier");
                        }
                        else
                        {
                            MessageBox.Show("تم التعديل بنجاح", "تعديل موزع");
                        }
                    }
                }
                else
                {
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Please Choose Customer or Supplier", "Error");
                    }
                    else
                    {
                        MessageBox.Show("برجاء اختيار العميل او الموزع", "خطأ");
                    }
                }
            }


            if (Application.OpenForms["Frmtailoring"] != null)
            {
                Frmtailoring p = (Application.OpenForms["Frmtailoring"] as Frmtailoring);
                p.Getcustomers();
                p.clientID.Text = txtName.Text;
                this.Close();
                return;
                //p.textBox15.Text = txtMobile.Text;
            }
            if (Application.OpenForms["frmPOS"] != null)
            {
                frmPOS p = (Application.OpenForms["frmPOS"] as frmPOS);
                p.Getcustomers();
                p.clients.Text = txtName.Text;
                this.Close();
                return;

                //p.textBox15.Text = txtMobile.Text;
            }
            if (Application.OpenForms["frmMSalesBill"] != null)
            {
                frmMSalesBill p = (Application.OpenForms["frmMSalesBill"] as frmMSalesBill);
                p.Getcustomers();
                p.clientID.Text = txtName.Text;
                this.Close();
                return;

                //p.textBox15.Text = txtMobile.Text;
            }
            if (Application.OpenForms["Newtailor"] != null)
            {
                Newtailor p = (Application.OpenForms["Newtailor"] as Newtailor);
                p.Getcustomers();
                p.clientID.Text = txtName.Text;
                p.textBox15.Text = txtMobile.Text;
                this.Close();
                return;

                //p.textBox15.Text = txtMobile.Text;
            }
            Clearfieldes();
        }
        private void txtOpeningBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        private void radioClients_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                var DT = thir.Getdatabycustomersupplier(textBox2.Text, 2);
                DGV.DataSource = DT;
            }
            else
            {
                var GDT = thir.Getdatabycustomer();
                DGV.DataSource = GDT;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumber.Text.Trim()))
            {
                if (txtName.Text == "عميل افتراضي")
                {
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("The default client cannot be deleted.", "Error");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("لا يمكن حذف العميل الافتراضي", "خطأ");
                        return;
                    }
                }
                else
                {
                    var Qur = @"Delete From Thirdparty 
                            Where ID = " + int.Parse(txtNumber.Text) + " ";
                    thir.Crudopration(Qur);
                    Clearfieldes();
                    if (radioClient.Checked)
                    {
                        if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                        {
                            MessageBox.Show("Delete is Successfully", "Delete");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("تم الحذف بنجاح", "حذف عميل");
                            return;
                        }
                    }
                    else
                    {
                        if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                        {
                            MessageBox.Show("Delete is Successfully", "Delete");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("تم الحذف بنجاح", "حذف موزع");
                            return;
                        }
                    }
                }
            }
            else
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please Choose Customer or Supplier", "Error");
                }
                else
                {
                    MessageBox.Show("برجاء اختيار العميل او الموزع", "خطأ");
                }
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                var DT = thir.Getdatabyname(textBox2.Text);
                DGV.DataSource=DT;
            }
            else
            {
                LoadThirdData();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                var GDT = thir.Getdatabynum(textBox1.Text);
                DGV.DataSource = GDT;
            }
            else
            {
                LoadThirdData();
            }
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        private void txtComments_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
    }
}
