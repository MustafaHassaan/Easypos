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
using System.Windows.Media;
using Querylaeyr.Modelmaster;
using Centeralized;

namespace Easypos.Masters
{
    public partial class frmListExpenses : Form
    {
        Expense Exp;
        Centergetingfunctions _ON;
        Vatinfo VI;
        Expencestype EPT;
        CompanyInfo CI;
        public double Taxes { get; set; }
        public double Taxvat { get; set; }
        public frmListExpenses()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            VI = new Vatinfo();
            VI.GetVat();
            if (VI.ISVAT)
            {
                Vatcheck.Enabled = true;
                label4.Enabled = true;
                txtVat.Enabled = true;
            }
            Exp = new Expense();
            _ON = new Centergetingfunctions();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnsave.Text = "Add";
            }
            else
            {
                Btnsave.Text = "اضافة";
            }
            GetExpencetype();
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
        public void GetExpencetype()
        {
            EPT = new Expencestype();
            var Dtbl = EPT.Getdata();
            DataRow dr = Dtbl.NewRow();
            dr["Id"] = 0;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                dr["Expencestypename"] = "--Choose--";
            }
            else
            {
                dr["Expencestypename"] = "--اختر--";
            }
            Dtbl.Rows.InsertAt(dr, 0);
            comboBox1.DataSource = Dtbl;
            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "Expencestypename";
        }
        public void Clearfeildes()
        {
            txtDescription.Clear();
            txtAmount.Clear();
            DTF.Value = DateTime.Now;
            DGV.DataSource = null;
            DGV.Rows.Clear();
            Getdataexpeneses();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnsave.Text = "Add";
            }
            else
            {
                Btnsave.Text = "اضافة";
            }
            Vatcheck.Checked = false;
            txtVat.Clear();
        }
        public void Getdataexpeneses()
        {
            var GD = Exp.Getdata();
            DGV.DataSource = GD;
            DGV.Columns[0].HeaderText = "ID";
            DGV.Columns[0].Visible = false;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[1].HeaderText = "Description";
                DGV.Columns[2].HeaderText = "Price";
                DGV.Columns[3].HeaderText = "Date";
                DGV.Columns[4].HeaderText = "Vat";
                DGV.Columns[5].HeaderText = "Expences type";
            }
            else
            {
                DGV.Columns[1].HeaderText = "وصف المصروف";
                DGV.Columns[2].HeaderText = "المبلغ";
                DGV.Columns[3].HeaderText = "التاريخ";
                DGV.Columns[4].HeaderText = "الضريبة";
                DGV.Columns[5].HeaderText = "نوع المصروف";
            }
        }
        public void LoadCategorysearch(string Qur)
        {
            var GD = Exp.Getdatabysearch(Qur);
            DGV.DataSource = GD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "ID";
                DGV.Columns[0].Visible = false;
                DGV.Columns[1].HeaderText = "Description";
                DGV.Columns[2].HeaderText = "Price";
                DGV.Columns[3].HeaderText = "Date";
                DGV.Columns[4].HeaderText = "Amount After Vat";
                DGV.Columns[5].HeaderText = "Expences type";
            }
            else
            {
                DGV.Columns[0].HeaderText = "ID";
                DGV.Columns[0].Visible = false;
                DGV.Columns[1].HeaderText = "وصف المصروف";
                DGV.Columns[2].HeaderText = "المبلغ";
                DGV.Columns[3].HeaderText = "التاريخ";
                DGV.Columns[4].HeaderText = "المبلغ بعد الضريبة";
                DGV.Columns[5].HeaderText = "نوع المصروف";
            }

        }
        private void frmListExpenses_Load(object sender, EventArgs e)
        {
            Getdataexpeneses();
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Btnsave_Click(object sender, EventArgs e)
        {           
            Exp.Description = txtDescription.Text;
            Exp.Amount = double.Parse(txtAmount.Text);
            Exp.CDate = DTF.Value.ToString("yyyy-MM-dd");
            Exp.Vat = Taxvat;
            Exp.Typeid = int.Parse(comboBox1.SelectedValue.ToString());
            if (Btnsave.Text == "اضافة" || Btnsave.Text == "Add")
            {
                if (Exp.ID == null)
                {
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Please insert expences", "Wrong");
                    }
                    else
                    {
                        MessageBox.Show("برجاء ادخال المصروف", "خطأ");
                    }
                }
                else
                {
                    var Qur = @"";
                    if (Exp.Typeid != 0)
                    {
                        Qur += @"INSERT INTO expenses(Description,Amount,CDate,Vat,Typeid) 
                                              VALUES('" + Exp.Description + "', " +
                          " " + Exp.Amount + " , " +
                          " '" + Exp.CDate + "'," +
                          " " + Exp.Vat + ", " +
                          " " + Exp.Typeid + " " +
                          " )";
                    }
                    else
                    {
                        Qur += @"INSERT INTO expenses(Description,Amount,CDate,Vat,Typeid) 
                                              VALUES('" + Exp.Description + "', " +
                          " " + Exp.Amount + " , " +
                          " '" + Exp.CDate + "'," +
                          " " + Exp.Vat + ", " +
                          " " + null + " " +
                          " )";
                    }
                    Exp.Crudopration(Qur);
                    Clearfeildes();
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Added Hase been Successfully", "Success");
                    }
                    else
                    {
                        MessageBox.Show("تم الاضافة بنجاح", "اضافة مصروف");
                    }
                }
            }
            else
            {
                var Qur = @"";
                if (Exp.Typeid != 0)
                {
                    Qur += @"Update expenses Set
                            Description = '" + Exp.Description + "', " +
                            "Amount = " + Exp.Amount + ", " +
                            "CDate = '" + Exp.CDate + "', " +
                            "Vat = " + Exp.Vat + ", " +
                            "Typeid = " + Exp.Typeid + " " +
                            "where ID = " + Exp.ID;
                }
                else
                {
                    Qur += @"Update expenses Set
                            Description = '" + Exp.Description + "', " +
                            "Amount = " + Exp.Amount + ", " +
                            "CDate = '" + Exp.CDate + "', " +
                            "Vat = " + Exp.Vat + ", " +
                            "Typeid = " + null + " " +
                            "where ID = " + Exp.ID;
                }

                Exp.Crudopration(Qur);
                Clearfeildes();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Edit Hase been Successfully", "Success");
                }
                else
                {
                    MessageBox.Show("تم التعديل بنجاح", "تعديل مصروف");
                }
            }
        }
        private void Btnnew_Click(object sender, EventArgs e)
        {
            Clearfeildes();
        }
        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e,sender);
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Rows.Count > 0)
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    Btnsave.Text = "Edit";
                }
                else
                {
                    Btnsave.Text = "تعديل";
                }
                Exp.ID = int.Parse(DGV.CurrentRow.Cells[0].Value.ToString());
                txtDescription.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                txtAmount.Text = DGV.CurrentRow.Cells[2].Value.ToString();
                var vat = DGV.CurrentRow.Cells[4].Value.ToString();
                DTF.Text = DGV.CurrentRow.Cells[3].Value.ToString();
                if (vat != "0")
                {
                    Vatcheck.Checked = true;
                    txtVat.Text = DGV.CurrentRow.Cells[4].Value.ToString();
                }
                else
                {
                    Vatcheck.Checked = false;
                    txtVat.Text = DGV.CurrentRow.Cells[4].Value.ToString();
                }
            }
        }
        private void Btndel_Click(object sender, EventArgs e)
        {
            if (Exp.ID != null)
            {
                var Qur = @"Delete From expenses where ID = " + Exp.ID;
                Exp.Crudopration(Qur);
                Clearfeildes();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Delete Hase been Successfully", "Success");
                }
                else
                {
                    MessageBox.Show("تم الحذف بنجاح", "حذف مصروف");
                }
            }
            else
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert Expences you want delete", "Error");
                }
                else
                {
                    MessageBox.Show("برجاء ادخال المصروف المراد حذفه", "خطأ");
                }
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                LoadCategorysearch(txtSearch.Text);
            }
            else
            {
                Getdataexpeneses();
            }
        }

        private void Vatcheck_CheckedChanged(object sender, EventArgs e)
        {
            if (Vatcheck.Checked)
            {
                if (!string.IsNullOrEmpty(txtAmount.Text))
                {
                    if (VI.ISVAT)
                    {
                        Taxes = Convert.ToDouble(txtAmount.Text) * (VI.VatPercent / 100);
                        var OGTax = Math.Round(Convert.ToDouble(Taxes), 2).ToString();
                        Taxvat = double.Parse(OGTax); 

                        //var Tax = Convert.ToDouble(txtAmount.Text) * (VI.VatPercent / 100);
                        var Tax = Convert.ToDouble(txtAmount.Text) / 1.15;
                        var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
                        //txtVat.Text = Convert.ToString(double.Parse(GTax) + Convert.ToDouble(txtAmount.Text));
                        txtVat.Text = Convert.ToString(double.Parse(GTax));
                    }
                }
                else
                {
                    Vatcheck.Checked = false;
                    return;
                }
            }
            else
            {
                Taxvat = 0;
                txtVat.Text = "0";
            }
        }
    }
}
