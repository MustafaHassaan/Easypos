using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Centeralized;
using Dataaccesslaeyr;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Querylaeyr.Modelmaster;

namespace Easypos.Masters.Subforms
{
    public partial class frmProductsOrder : Form
    {
        public static int SelectedCategory;
        Category Cat;
        Product Pro;
        DataAccess DA;
        CompanyInfo CI;
        public frmProductsOrder()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            DA = new DataAccess();
            Cat = new Category();
            Pro = new Product();
            Combofill();
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
        public void Combofill()
        {
            try
            {
                var CGData = Cat.GetCombodata();
                cmbCategory.DataSource = CGData;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة ترتيب الاصناف" + "','" + "Getetinfo" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        private void frmOrderProducts_Load(object sender, EventArgs e)
        {

        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pro.CategoryNo = int.Parse(cmbCategory.SelectedValue.ToString());
            var GP = Pro.Filldata();
            dgvProducts.DataSource = GP;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool SD = false;
            for (int i = 0; i < dgvProducts.Rows.Count - 1; i++)
            {
                var ID = Convert.ToInt32(dgvProducts.Rows[i].Cells["ID"].Value);
                var Order = Convert.ToInt32(dgvProducts.Rows[i].Cells["Order"].Value);
                var Qur = "Update Product Set " +
                                 "Product.Order = "+ Order + 
                                  " Where ProductNo = " + ID;
                SD = Pro.Crudopration(Qur);
            }
            if (SD)
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Items have been arranged successfully.", "Sort items");
                }
                else
                {
                    MessageBox.Show("تم ترتيب الاصناف بنجاح", "ترتيب الاصناف");
                }
            }
            else
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Items have been arranged successfully.", "Sort items");
                }
                else
                {
                    MessageBox.Show("تم ترتيب الاصناف بنجاح", "ترتيب الاصناف");
                }
            }
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
