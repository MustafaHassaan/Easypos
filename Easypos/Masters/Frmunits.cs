using Centeralized;
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

namespace Easypos.Masters
{
    public partial class Frmunits : Form
    {
        CompanyInfo CI;
        public Frmunits()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
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
            Unit unt = new Unit();
            var UD = unt.Getdata();
            DGV.DataSource = UD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "Id";
                DGV.Columns[1].HeaderText = "Unitname";
            }
            else
            {
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[1].HeaderText = "اسم الوحده";
            }
        }
        private void BA_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert unit", "Wrong");
                    return;
                }
                else
                {
                    MessageBox.Show("برجاء ادخال الوحدة", "خطأ");
                    return;
                }
            }
            else
            {
                Unit unt = new Unit();
                unt.UnitType = textBox1.Text;
                var Qur = @"INSERT INTO unittypes(UnitType) VALUES('" + unt.UnitType + "')";
                unt.Crudopration(Qur);
                Clear();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Added is successfully", "Success");
                    return;
                }
                else
                {
                    MessageBox.Show("تم الاضافة بنجاح", "اضافة وحدة");
                    return;
                }
            }
        }
        public void Clear()
        {
            DGV.DataSource = null;
            DGV.Rows.Clear();
            id = 0;
            textBox1.Text = "";
            Getetinfo();
        }
        public int id { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert unit", "Wrong");
                    return;
                }
                else
                {
                    MessageBox.Show("برجاء ادخال الوحدة", "خطأ");
                    return;
                }
            }
            else
            {

                Unit unt = new Unit();
                unt.ID = id;
                unt.UnitType = textBox1.Text;
                var Qur = @"UPDATE unittypes SET UnitType= '"+ unt.UnitType +"' WHERE ID = "+ unt.ID;
                unt.Crudopration(Qur);
                Clear();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Modified successfully", "Edit");
                    return;
                }
                else
                {
                    MessageBox.Show("تم التعديل بنجاح", "تعديل وحدة");
                    return;
                }
                
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert unit", "Wrong");
                    return;
                }
                else
                {
                    MessageBox.Show("برجاء ادخال الوحدة", "خطأ");
                    return;
                }
            }
            else
            {

                Unit unt = new Unit();
                unt.ID = id;
                unt.UnitType = textBox1.Text;
                var Qur = @"Delete From unittypes WHERE ID = " + unt.ID;
                unt.Crudopration(Qur);
                Clear();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Deleted is successfully", "Success");
                    return;
                }
                else
                {
                    MessageBox.Show("نم الحذف بنجاح", "تم");
                    return;
                }
            }
        }
        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Frmunits_Load(object sender, EventArgs e)
        {
            Getetinfo();
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Rows.Count > 0)
            {
                id = int.Parse(DGV.CurrentRow.Cells[0].Value.ToString());
                textBox1.Text = DGV.CurrentRow.Cells[1].Value.ToString();
            }
            else
            {
                return;
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
