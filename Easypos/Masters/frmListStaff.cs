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
using Centeralized;
using Querylaeyr.Modelmaster;

namespace Easypos.Masters
{
    public partial class frmListStaff : Form
    {
        Centergetingfunctions _ON;
        Staff _Stf;
        CompanyInfo CI;
        public frmListStaff()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            _ON = new Centergetingfunctions();
            _Stf = new Staff();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Edit";
            }
            else
            {
                Btnaddedit.Text = "أضافة";
            }
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
        public void LoadStaffData()
        {         
            var Th = _Stf.Getdata();
            DGV.DataSource = Th;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "Id";
                DGV.Columns[1].HeaderText = "Frist name";
                DGV.Columns[1].Visible = false;
                DGV.Columns[2].HeaderText = "Second name";
                DGV.Columns[2].Visible = false;
                DGV.Columns[3].HeaderText = "Full Name";
                DGV.Columns[4].HeaderText = "Phone number";
                DGV.Columns[5].HeaderText = "Address";
                DGV.Columns[6].HeaderText = "Username";
                DGV.Columns[7].HeaderText = "UPassword";
                DGV.Columns[7].Visible = false;
                DGV.Columns[8].HeaderText = "Role";
                DGV.Columns[8].Visible = false;
            }
            else
            {
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[1].HeaderText = "الاسم الاول";
                DGV.Columns[1].Visible = false;
                DGV.Columns[2].HeaderText = "الاسم الثاني";
                DGV.Columns[2].Visible = false;
                DGV.Columns[3].HeaderText = "الاسم";
                DGV.Columns[4].HeaderText = "رقم الجول";
                DGV.Columns[5].HeaderText = "العنوان";
                DGV.Columns[6].HeaderText = "اسم المستخدم";
                DGV.Columns[7].HeaderText = "UPassword";
                DGV.Columns[7].Visible = false;
                DGV.Columns[8].HeaderText = "نوع المستخدم";
                DGV.Columns[8].Visible = false;
            }
        }
        public void Clearfieldes() {
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Add";
            }
            else
            {
                Btnaddedit.Text = "أضافة";
            }
            lblProductNo.Text = string.Empty;
            txtContractNo.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPWD.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();
            txtProvince.Clear();
            textBox1.Clear();
            textBox2.Clear();
            DGV.DataSource = null;
            DGV.Rows.Clear();
            LoadStaffData();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStaffData();
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtContractNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e,sender);
        }
        private void Btnaddedit_Click(object sender, EventArgs e)
        {
            if (txtFirstname.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert Name", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("من فضلك ادخل الاسم", "خطأ");
                    return;
                }
            }
            if (txtLastname.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert Name", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("من فضلك ادخل الاسم", "خطأ");
                }
            }
            if (txtUsername.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert User Name", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("من فضلك ادخل اسم المستخدم", "خطأ");
                    return;
                }
            }
            if (txtPassword.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert password", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("من فضلك ادخل كلمة السر", "خطأ");
                    return;
                }
            }
            if (txtPassword.Text != txtConfirmPWD.Text)
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert password", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("من فضلك تحقق من كلمة السر", "خطأ");
                    return;
                }
            }
            _Stf.Firstname = txtFirstname.Text;
            _Stf.Lastname = txtLastname.Text;
            _Stf.Fullname = txtFirstname.Text + " " + txtLastname.Text;
            _Stf.Address = txtProvince.Text;
            _Stf.Phonenumber = txtContractNo.Text;
            _Stf.Username = txtUsername.Text;
            _Stf.UPassword = txtPassword.Text;
            _Stf.Role = "Admin";
            if (Btnaddedit.Text == "أضافة" || Btnaddedit.Text == "Add")
            {
                var Qur = @"INSERT INTO Staff(Firstname,Lastname,Fullname,Address,Phonenumber,Username,UPassword,Role)
                                              VALUES('" + _Stf.Firstname + "', " +
                                                      "'" + _Stf.Lastname + "'," +
                                                      "'" + _Stf.Fullname + "', " +
                                                      "'" + _Stf.Address + "', " +
                                                      "'" + _Stf.Phonenumber + "', " +
                                                      "'" + _Stf.Username + "', " +
                                                      "'" + _Stf.UPassword + "', " +
                                                      "'" + _Stf.Role + "' " +
                                                      ")";
                _Stf.Crudopration(Qur);
                Clearfieldes();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Added Successfully", "Add Employee");
                }
                else
                {
                    MessageBox.Show("تم الاضافة بنجاح", "اضافة موظف");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lblProductNo.Text))
                {
                    var Qur = @"Update Staff Set
                            Firstname = '" + _Stf.Firstname + "'," +
                            "Lastname = '" + _Stf.Lastname + "'," +
                            "Fullname = '" + _Stf.Fullname + "'," +
                            "Address = '" + _Stf.Address + "'," +
                            "Phonenumber = '" + _Stf.Phonenumber + "', " +
                            "Username = '" + _Stf.Username + "', " +
                            "UPassword = '" + _Stf.UPassword + "', " +
                            "Role = '" + _Stf.Role + "' " +
                            "Where StaffID = " + int.Parse(lblProductNo.Text) + " ";
                    _Stf.Crudopration(Qur);
                    Clearfieldes();
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Edit Successfully", "Edit Employee");
                    }
                    else
                    {
                        MessageBox.Show("تم التعديل بنجاح", "تعديل موظف");
                    }
                }
                else
                {
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Please Choose Employee", "Wrong");
                    }
                    else
                    {
                        MessageBox.Show("برجاء اختيار الموظف", "خطأ");
                    }
                }
            }
        }
        private void Btndel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblProductNo.Text))
            {
                var Qur = @"Delete From Staff 
                            Where StaffID = " + int.Parse(lblProductNo.Text) + " ";
                _Stf.Crudopration(Qur);
                Clearfieldes();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Delete is Successfully", "Delete");
                }
                else
                {
                    MessageBox.Show("تم الحذف بنجاح", "حذف موظف");
                }
            }
            else
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please Choose Employee", "Error");
                }
                else
                {
                    MessageBox.Show("برجاء اختيار الموظف", "خطأ");
                }
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            Clearfieldes();
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Edit";
            }
            else
            {
                Btnaddedit.Text = "تعديل";
            }
            if (DGV.Rows.Count > 0)
            {
                lblProductNo.Text = DGV.CurrentRow.Cells[0].Value.ToString();
                txtFirstname.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                txtLastname.Text = DGV.CurrentRow.Cells[2].Value.ToString();
                txtContractNo.Text = DGV.CurrentRow.Cells[4].Value.ToString();
                txtProvince.Text = DGV.CurrentRow.Cells[5].Value.ToString();
                txtUsername.Text = DGV.CurrentRow.Cells[6].Value.ToString();
                txtPassword.Text = DGV.CurrentRow.Cells[7].Value.ToString();
                txtConfirmPWD.Text = DGV.CurrentRow.Cells[7].Value.ToString();
            }
            else
            {
                return;
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                var GDT = _Stf.Getdatabynum(textBox1.Text);
                DGV.DataSource = GDT;
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    DGV.Columns[0].HeaderText = "Id";
                    DGV.Columns[1].HeaderText = "Frist name";
                    DGV.Columns[1].Visible = false;
                    DGV.Columns[2].HeaderText = "Second name";
                    DGV.Columns[2].Visible = false;
                    DGV.Columns[3].HeaderText = "Full name";
                    DGV.Columns[4].HeaderText = "Pnone number";
                    DGV.Columns[5].HeaderText = "Address";
                    DGV.Columns[6].HeaderText = "User name";
                    DGV.Columns[7].HeaderText = "UPassword";
                    DGV.Columns[7].Visible = false;
                    DGV.Columns[8].HeaderText = "Role";
                    DGV.Columns[8].Visible = false;
                }
                else
                {
                    DGV.Columns[0].HeaderText = "المسلسل";
                    DGV.Columns[1].HeaderText = "الاسم الاول";
                    DGV.Columns[1].Visible = false;
                    DGV.Columns[2].HeaderText = "الاسم الثاني";
                    DGV.Columns[2].Visible = false;
                    DGV.Columns[3].HeaderText = "الاسم";
                    DGV.Columns[4].HeaderText = "رقم الجول";
                    DGV.Columns[5].HeaderText = "العنوان";
                    DGV.Columns[6].HeaderText = "اسم المستخدم";
                    DGV.Columns[7].HeaderText = "UPassword";
                    DGV.Columns[7].Visible = false;
                    DGV.Columns[8].HeaderText = "نوع المستخدم";
                    DGV.Columns[8].Visible = false;
                }
            }
            else
            {
                LoadStaffData();
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                var GDT = _Stf.Getdatabyname(textBox2.Text);
                DGV.DataSource = GDT;
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    DGV.Columns[0].HeaderText = "Id";
                    DGV.Columns[1].HeaderText = "Frist name";
                    DGV.Columns[1].Visible = false;
                    DGV.Columns[2].HeaderText = "Second name";
                    DGV.Columns[2].Visible = false;
                    DGV.Columns[3].HeaderText = "Full name";
                    DGV.Columns[4].HeaderText = "Pnone number";
                    DGV.Columns[5].HeaderText = "Address";
                    DGV.Columns[6].HeaderText = "User name";
                    DGV.Columns[7].HeaderText = "UPassword";
                    DGV.Columns[7].Visible = false;
                    DGV.Columns[8].HeaderText = "Role";
                    DGV.Columns[8].Visible = false;
                }
                else
                {
                    DGV.Columns[0].HeaderText = "المسلسل";
                    DGV.Columns[1].HeaderText = "الاسم الاول";
                    DGV.Columns[1].Visible = false;
                    DGV.Columns[2].HeaderText = "الاسم الثاني";
                    DGV.Columns[2].Visible = false;
                    DGV.Columns[3].HeaderText = "الاسم";
                    DGV.Columns[4].HeaderText = "رقم الجول";
                    DGV.Columns[5].HeaderText = "العنوان";
                    DGV.Columns[6].HeaderText = "اسم المستخدم";
                    DGV.Columns[7].HeaderText = "UPassword";
                    DGV.Columns[7].Visible = false;
                    DGV.Columns[8].HeaderText = "نوع المستخدم";
                    DGV.Columns[8].Visible = false;
                }
            }
            else
            {
                LoadStaffData();
            }
        }
    }
}
