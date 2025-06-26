using Centeralized;
using MetroFramework.Forms;
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
    public partial class FrmExpencetype : MetroForm
    {
        Expencestype EPT;
        public int EPTId { get; set; }
        CompanyInfo CI;
        public FrmExpencetype()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            Btnaddedit.Text = "أضافة";
            Getdata();
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

        public void Getdata()
        {
            EPT = new Expencestype();
            var GD = EPT.Getdata();
            DGV.DataSource = GD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "Id";
                DGV.Columns[0].Visible = false;
                DGV.Columns[1].HeaderText = "Expencestypename";
            }
            else
            {
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[0].Visible = false;
                DGV.Columns[1].HeaderText = "نوع المصروف";
            }
        }

        public void Cleardata()
        {
            DGV.DataSource = null;
            DGV.Rows.Clear();
            Getdata();
            textBox1.Clear();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Add";

            }
            else
            {
                Btnaddedit.Text = "أضافة";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Cleardata();
        }

        private void Btnaddedit_Click(object sender, EventArgs e)
        {
            EPT = new Expencestype();
            EPT.Expencestypename = textBox1.Text;
            if (Btnaddedit.Text == "أضافة")
            {
                var Qur = @"Insert into expencestype(Expencestypename) Value('" + EPT.Expencestypename + "')";
                EPT.Crudopration(Qur);
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("The expense type has been successfully Added.", "Done");
                }
                else
                {
                    MessageBox.Show("تم اضافة نوع المصروف بنجاح", "تم");
                }
            }
            else
            {
                EPT.Id = EPTId;
                var Qur = @"Update expencestype Set Expencestypename = '" + EPT.Expencestypename + "' Where Id = " + EPT.Id;
                EPT.Crudopration(Qur);
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("The expense type has been successfully modified.", "Done");
                }
                else
                {
                    MessageBox.Show("تم تعديل نوع المصروف بنجاح", "تم");
                }
            }
            Cleardata();
        }

        private void Btndel_Click(object sender, EventArgs e)
        {
            EPT.Id = EPTId;
            var Qur = @"Delete From expencestype Where Id = " + EPT.Id;
            EPT.Crudopration(Qur);
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                MessageBox.Show("The expense type has been successfully deleted.", "Done");
            }
            else
            {
                MessageBox.Show("تم حذف نوع المصروف بنجاح", "تم");
            }
            Cleardata();
        }

        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Rows.Count > 0)
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    Btnaddedit.Text = "Edit";
                }
                else
                {
                    Btnaddedit.Text = "تعديل";
                }
                EPTId = int.Parse(DGV.CurrentRow.Cells[0].Value.ToString());
                textBox1.Text = DGV.CurrentRow.Cells[1].Value.ToString();
            }
        }
    }
}
