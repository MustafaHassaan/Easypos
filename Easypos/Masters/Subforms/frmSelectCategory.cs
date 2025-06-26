using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Easypos.Masters;
using Querylaeyr.Modelmaster;
using Dataaccesslaeyr;
using Centeralized;
using System.Linq;

namespace Easypos.Masters.Subforms
{
    public partial class frmSelectCategory : Form
    {
        private frmListProduct FLP = null;
        //private frmListAddons LAO = null;
        Category cat;
        DataAccess DA;
        CompanyInfo CI;
        public frmSelectCategory(Form callingForm)
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            DA = new DataAccess();
            FLP = callingForm as frmListProduct;
            //LAO = callingForm as frmListAddons;
            cat = new Category();
            var CD = cat.Getdata();
            DGV.DataSource = CD;
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
        public void Getetinfobysearch()
        {
            try
            {
                var UD = cat.Getdatabysearch(txtCatName.Text);
                DGV.DataSource = UD;
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
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة اختيار الاصناف" + "','" + "Getetinfobysearch" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmSelectCategory_Load(object sender, EventArgs e)
        {
        }
        private void txtCatName_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCatName.Text))
            {
                Getetinfobysearch();
            }
            else
            {
                var CD = cat.Getdata();
                DGV.DataSource = CD;
            }
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FLP.CID = DGV.CurrentRow.Cells[0].Value.ToString();
            FLP.txtCategory.Text = DGV.CurrentRow.Cells[1].Value.ToString();
            //LAO.Catid = DGV.CurrentRow.Cells[0].Value.ToString();
            //LAO.txtCategory.Text = DGV.CurrentRow.Cells[1].Value.ToString();
            this.Close();
        }
    }
}
