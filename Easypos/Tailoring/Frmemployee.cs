using Centeralized;
using Querylaeyr.Modelmaster;
using Querylaeyr.Tailor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easypos.Tailoring
{
    public partial class Frmemployee : Form
    {
        public int id { get; set; }
        CompanyInfo CI;
        public Frmemployee()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            Getetinfo();
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
            empcreator unt = new empcreator();
            var UD = unt.Getdata();
            DGV.DataSource = UD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "Id";
                DGV.Columns[1].HeaderText = "Tailor name";
                DGV.Columns[2].HeaderText = "Phon number";
            }
            else
            {
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[1].HeaderText = "اسم الخياط";
                DGV.Columns[2].HeaderText = "رقم الهاتف";
            }
        }
        private void BA_Click(object sender, EventArgs e)
        {
            Clear();
        }
        public void Clear()
        {
            DGV.DataSource = null;
            DGV.Rows.Clear();
            id = 0;
            textBox1.Text = "";
            textBox2.Text = "";
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                button1.Text = "Save";
            }
            else
            {
                button1.Text = "حفظ";
            }
            Getetinfo();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            empcreator unt = new empcreator();
            var Qur = "";
            if (button1.Text == "حفظ" || button1.Text == "Save")
            {
                if (textBox1.Text == "")
                {
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Erorr Please insert Tailor", "Error");
                    }
                    else
                    {
                        MessageBox.Show("برجاء ادخال الخياط", "خطأ");
                    }
                }
                else
                {
                    Qur += @"INSERT INTO empcreator(Empname,Empphone) VALUES('" + textBox1.Text + "','" + textBox2.Text + "')";
                    unt.Crudopration(Qur);
                    Clear();
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Added is successfully", "Done");
                    }
                    else
                    {
                        MessageBox.Show("تم الاضافة بنجاح", "اضافة خياط");
                    }
                }
            }
            else
            {
                Qur += @"Update empcreator set 
                                     Empname = '" + textBox1.Text + "'," +
                                    "Empphone = '" + textBox2.Text + "' " +
                                    "Where Id = " + id;
                unt.Crudopration(Qur);
                Clear();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Edit is successfully", "Done");
                }
                else
                {
                    MessageBox.Show("تم التعديل بنجاح", "تعديل خياط");
                }
            }
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Rows.Count > 0)
            {
                id = int.Parse(DGV.CurrentRow.Cells[0].Value.ToString());
                textBox1.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                textBox2.Text = DGV.CurrentRow.Cells[2].Value.ToString();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    button1.Text = "Edit";
                }
                else
                {
                    button1.Text = "تعديل";
                }
            }
            else
            {
                return;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
           this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            empcreator unt = new empcreator();
            var Qur = "";
            Qur += @"Delete From empcreator Where Id = " + id;
            unt.Crudopration(Qur);
            Clear();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                MessageBox.Show("Delete is successfully", "Done");
            }
            else
            {
                MessageBox.Show("تم الحذف بنجاح", "تعديل خياط");
            }
        }
    }
}
