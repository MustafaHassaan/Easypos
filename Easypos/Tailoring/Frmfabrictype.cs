using Centeralized;
using Easypos.Masters;
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
    public partial class Frmfabrictype : Form
    {
        CompanyInfo CI;
        private Frmfabric Ffab = null;
        public int id { get; set; }
        public Frmfabrictype(Form callingForm)
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            Getetinfo();
            if (callingForm != null) {
                Ffab = callingForm as Frmfabric;
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
        public void Getetinfo()
        {
            Fabrictype FT = new Fabrictype();
            var FTD = FT.Getdata();
            DGV.DataSource = FTD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[1].HeaderText = "Id";
                DGV.Columns[1].Visible = false;
                DGV.Columns[2].HeaderText = "Fabric type";
            }
            else
            {
                DGV.Columns[1].HeaderText = "المسلسل";
                DGV.Columns[1].Visible = false;
                DGV.Columns[2].HeaderText = "نوع القماش";
            }
        }
        public void Clear()
        {
            DGV.DataSource = null;
            DGV.Rows.Clear();
            id = 0;
            textBox1.Text = "";
            Getetinfo();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                button2.Text = "Save";
            }
            else
            {
                button2.Text = "حفظ";
            }
        }
        private void Add_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Rows.Count > 0)
            {
                id = int.Parse(DGV.CurrentRow.Cells[1].Value.ToString());
                if (DGV.Columns[e.ColumnIndex].Name == "Delete")
                {
                    Fabrictype FT = new Fabrictype();
                    var Qur = @"Delete from fabrictype where Id = " + id;
                    FT.Crudopration(Qur);
                    Clear();
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Delete is successfully", "Delete");
                    }
                    else
                    {
                        MessageBox.Show("تم الحذف بنجاح", "حذف نوع قماش");
                    }
                }
                else
                {
                    textBox1.Text = DGV.CurrentRow.Cells[2].Value.ToString();
                    if (Ffab != null)
                    {
                        Ffab.Typeid = id;
                        Ffab.textBox1.Text = textBox1.Text;
                        this.Close();
                    }
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        button2.Text = "Edit";
                    }
                    else
                    {
                        button2.Text = "تعديل";
                    }
                }
            }
            else
            {
                return;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert unit", "Error");
                }
                else
                {
                    MessageBox.Show("برجاء ادخال الوحدة", "خطأ");
                }
            }
            else
            {
                Fabrictype FT = new Fabrictype();
                FT.Typename = textBox1.Text;
                string Qur = "";
                if (button2.Text == "حفظ" || button2.Text == "Save") {
                    Qur += @"INSERT INTO fabrictype(Typename) VALUES('" + textBox1.Text + "')";
                }
                else
                {
                    Qur += @"Update fabrictype Set Typename = '" + textBox1.Text + "' Where Id = "+ id ;
                }
                FT.Crudopration(Qur);
                Clear();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Done Add Edit Successfully", "Done");
                }
                else
                {
                    MessageBox.Show("تم بنجاح", "اضافة وتعديل قماش");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
