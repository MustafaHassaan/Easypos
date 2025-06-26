using Centeralized;
using Dataaccesslaeyr;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
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

namespace Easypos.Masters.Subforms
{
    public partial class Frmlistunit : Form
    {
        private frmListProduct FLP = null;
        private frmproductitemlist frmItems = null;
        DataAccess DA;
        Unit unt;
        CompanyInfo CI;
        public Frmlistunit(Form callingForm)
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            DA = new DataAccess();
            unt = new Unit();
            FLP = callingForm as frmListProduct;
            frmItems = callingForm as frmproductitemlist;
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
        public void Getetinfo()
        {
            try
            {
                var UD = unt.Getdata();
                DGV.DataSource = UD;
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    DGV.Columns[0].HeaderText = "Id";
                    DGV.Columns[1].HeaderText = "Unit name";
                }
                else
                {
                    DGV.Columns[0].HeaderText = "المسلسل";
                    DGV.Columns[1].HeaderText = "اسم الوحده";
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة قائمة الوحدات" + "','" + "Getetinfo" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Getetinfobysearch()
        {
            try
            {
                var Qur = "SELECT * FROM unittypes WHERE UnitType LIKE '" + txtCatName.Text + "%' ORDER BY UnitType";
                var UD = unt.Getdatabysearch(Qur);
                DGV.DataSource = UD;
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[1].HeaderText = "اسم الوحده";
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة قائمة الوحدات" + "','" + "Getetinfobysearch" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        private void Frmlistunit_Load(object sender, EventArgs e)
        {
            Getetinfo();
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtCatName_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCatName.Text))
            {
                Getetinfobysearch();
            }
            else
            {
                Getetinfo();
            }
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (FLP != null)
            {
                FLP.UID = DGV.CurrentRow.Cells[0].Value.ToString();
                FLP.textBox1.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                this.Close();
            }
            if (frmItems != null)
            {
                frmItems.textBox1.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                frmItems.unitid = int.Parse(DGV.CurrentRow.Cells[0].Value.ToString());
                this.Close();
            }
        }
    }
}
