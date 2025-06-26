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

namespace Easypos.Masters
{
    public partial class frmListCategory : Form
    {
        int color = 0;
        Category cat;
        CompanyInfo CI;
        public frmListCategory()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Add";
            }
            else
            {
                Btnaddedit.Text = "اضافة";
            }
            cat = new Category();
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
        public int id { get; set; }
        public void LoadCategories()
        {
            var CD = cat.Getdata();
            DGV.DataSource = CD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "Id";
                DGV.Columns[1].HeaderText = "Category name";
                DGV.Columns[2].HeaderText = "Description";
                DGV.Columns[3].HeaderText = "Color";
            }
            else
            {
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[1].HeaderText = "اسم التصنيف";
                DGV.Columns[2].HeaderText = "الوصف";
                DGV.Columns[3].HeaderText = "اللون";
            }
        }
        public void LoadCategorysearch(string Qur)
        {
            var CD = cat.Getdatabysearch(Qur);
            DGV.DataSource = CD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "Id";
                DGV.Columns[1].HeaderText = "Category name";
                DGV.Columns[2].HeaderText = "Description";
                DGV.Columns[3].HeaderText = "Color";
            }
            else
            {
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[1].HeaderText = "اسم التصنيف";
                DGV.Columns[2].HeaderText = "الوصف";
                DGV.Columns[3].HeaderText = "اللون";
            }
        }
        public void Clearfieldes()
        {
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Add";
            }
            else
            {
                Btnaddedit.Text = "اضافة";
            }
            LoadCategories();
            lblCategoryNo.Text = string.Empty;
            txtCatName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            label4.BackColor = base.BackColor;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            Clearfieldes();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmListCategory_Load(object sender, EventArgs e)
        {
             LoadCategories();
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                lblCategoryNo.Text = DGV.CurrentRow.Cells[0].Value.ToString();
                txtCatName.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                txtDescription.Text = DGV.CurrentRow.Cells[2].Value.ToString();
                var Bcolor = int.Parse(DGV.CurrentRow.Cells[3].Value.ToString());
                label4.BackColor = System.Drawing.Color.FromArgb(Bcolor);
            }
            else
            {
                return;
            }
        }
        private void btnSelectcolor_Click(object sender, EventArgs e)
        {
            DialogResult colors = new DialogResult();
            colors = colorDialog1.ShowDialog();
            if (colors == DialogResult.OK)
            {
                label4.BackColor = colorDialog1.Color;
                color = colorDialog1.Color.ToArgb();
            }
        }
        private void Btnaddedit_Click(object sender, EventArgs e)
        {
            cat.CategoryName = txtCatName.Text;
            cat.Description = txtDescription.Text;
            cat.Color = color;

            if (Btnaddedit.Text == "اضافة" || Btnaddedit.Text == "Add")
            {
                if (txtCatName.Text == "")
                {
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Please edit Category", "Wrong");
                    }
                    else
                    {
                        MessageBox.Show("Please insert Category", "Wrong");
                    }
                }
                else
                {
                    var Qur = @"INSERT INTO category(CategoryName,Description,Color) 
                                              VALUES('" + cat.CategoryName + "', " +
                                              "'" + cat.Description + "', " +
                                              " " + cat.Color + ")";
                    cat.Crudopration(Qur);
                    Clearfieldes();
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Added successfully", "Success");

                    }
                    else
                    {
                        MessageBox.Show("تم الاضافة بنجاح", "اضافة تصنيف");
                    }
                }
            }
            else
            {
                cat.CategoryNo = int.Parse(lblCategoryNo.Text);
                var Qur = @"Update category Set
                            CategoryName = '" + cat.CategoryName + "', " +
                            "Description = '" + cat.Description + "', " +
                            "Color = '" + cat.Color + "' where CategoryNo = " + cat.CategoryNo;
                cat.Crudopration(Qur);
                Clearfieldes();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Edit successfully", "Success");
                }
                else
                {
                    MessageBox.Show("تم التعديل بنجاح", "تعديل تصنيف");
                }
            }
        }
        private void Btndel_Click(object sender, EventArgs e)
        {
            cat.CategoryNo = int.Parse(lblCategoryNo.Text);
            if (cat.CategoryNo != null)
            {
                var Qur = @"Delete From category where CategoryNo = " + cat.CategoryNo;
                cat.Crudopration(Qur);
                Clearfieldes();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Delete successfully", "Success");
                }
                else
                {
                    MessageBox.Show("تم الحذف بنجاح", "حذف تصنيف");
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
                LoadCategories();
            }
        }
    }
}
