using Centeralized;
using Easypos.Masters.Subforms;
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
    public partial class Frmfabric : Form
    {
        CompanyInfo CI;
        public int id { get; set; }
        public int Typeid { get; set; }
        public Frmfabric()
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
            Fabric Fab = new Fabric();
            var FTD = Fab.Getdata();
            DGV.DataSource = FTD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "Id";
                DGV.Columns[0].Visible = false;
                DGV.Columns[1].HeaderText = "Fabric name";
                DGV.Columns[2].HeaderText = "Fabric type";
                DGV.Columns[3].Visible = false;
            }
            else
            {
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[0].Visible = false;
                DGV.Columns[1].HeaderText = "اسم القماش";
                DGV.Columns[2].HeaderText = "نوع القماش";
                DGV.Columns[3].Visible = false;
            }
        }
        public void Clear()
        {
            DGV.DataSource = null;
            DGV.Rows.Clear();
            id = 0;
            textBox1.Text = "";
            txtItemname.Text = "";
            Getetinfo();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Save";
            }
            else
            {
                Btnaddedit.Text = "حفظ";
            }
            txtSearch.Text = "";
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Rows.Count > 0)
            {
                id = int.Parse(DGV.CurrentRow.Cells[0].Value.ToString());
                txtItemname.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                textBox1.Text = DGV.CurrentRow.Cells[2].Value.ToString();
                Typeid = int.Parse(DGV.CurrentRow.Cells[3].Value.ToString());
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    Btnaddedit.Text = "Edit";
                }
                else
                {
                    Btnaddedit.Text = "تعديل";
                }
            }
            else
            {
                return;
            }
        }
        private void Btnaddedit_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert fabrics", "Error");
                }
                else
                {
                    MessageBox.Show("برجاء ادخال القماش", "خطأ");
                }
            }
            else
            {
                Fabric FT = new Fabric();
                string Qur = "";
                if (Btnaddedit.Text == "حفظ" || Btnaddedit.Text == "Save")
                {
                    Qur += @"INSERT INTO fabric(Fabricname,Fabrictype) VALUES('" + txtItemname.Text + "',"+ Typeid + ")";
                }
                else
                {
                    Qur += @"Update fabric Set Fabricname = '" + txtItemname.Text + "', Fabrictype =" + Typeid + " Where Id = " + id;
                }
                FT.Crudopration(Qur);
                Clear();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Added is successfully", "Done");
                }
                else
                {
                    MessageBox.Show("تم بنجاح", "اضافة وتعديل قماش");
                }
            }
        }
        private void Btnunit_Click(object sender, EventArgs e)
        {
            Frmfabrictype flc = new Frmfabrictype(this);
            flc.ShowDialog();
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            Fabric FT = new Fabric();
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                var Qur = @"Select from fabric where Fabricname = " + txtSearch.Text;
                FT.Crudopration(Qur);
            }
            else
            {
                Clear();
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                Clear();
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Fabric FT = new Fabric();
            var Qur = @"Delete from fabric where Id = " + id;
            FT.Crudopration(Qur);
            Clear();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                MessageBox.Show("Deleted is successfully", "Done");
            }
            else
            {
                MessageBox.Show("تم الحذف بنجاح", "حذف قماش");
            }
        }
    }
}
