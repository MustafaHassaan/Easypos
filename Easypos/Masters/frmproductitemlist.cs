using Centeralized;
using Easypos.Masters.Subforms;
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
using System.Windows.Media;

namespace Easypos.Masters
{
    public partial class frmproductitemlist : Form
    {
        public int unitid { get; set; }
        Items Item;
        Centergetingfunctions _ON;
        CompanyInfo CI;
        public frmproductitemlist()
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
            Item = new Items();
            _ON = new Centergetingfunctions();
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
        public void Loaditems()
        {

            var CD = Item.Getdata();
            DGV.DataSource = CD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "Id";
                DGV.Columns[1].HeaderText = "Description";
                DGV.Columns[2].HeaderText = "Price";
                DGV.Columns[3].HeaderText = "Amount";
                DGV.Columns[4].HeaderText = "Unit";
                DGV.Columns[5].HeaderText = "Balance";
                DGV.Columns[6].HeaderText = "UID";
                DGV.Columns[6].Visible = false;
            }
            else
            {
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[1].HeaderText = "الوصف";
                DGV.Columns[2].HeaderText = "السعر";
                DGV.Columns[3].HeaderText = "الكمية";
                DGV.Columns[4].HeaderText = "الوحدة";
                DGV.Columns[5].HeaderText = "الرصيد الافتتاحي";
                DGV.Columns[6].HeaderText = "UID";
                DGV.Columns[6].Visible = false;
            }
        }
        public void Clearitems() {
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Add";
            }
            else
            {
                Btnaddedit.Text = "اضافة";
            }
            txtItemname.Clear();
            txtUnitPrice.Clear();
            txtStocksOnHand.Clear();
            textBox1.Clear();
            textBox2.Clear();
            txtSearch.Clear();
            DGV.DataSource = null;
            DGV.Rows.Clear();
            Loaditems();
        }
        public void Loaditems(string strSearch)
        {
            //try
            //{
            //    var Qur = @"SELECT 
            //                    items.ID, 
            //                    items.Itemname, 
            //                    items.ItemPrice, 
            //                    items.itemqty, 
            //                    items.OpeningBalance, 
            //                    unittypes.UnitType
            //                    FROM items 
            //                    LEFT outer JOIN unittypes 
            //                    On Items.UID = unittypes.ID
            //                    WHERE items.Itemname LIKE '" + strSearch + "%' ORDER BY Itemname";
            //    IUnitofwork IU = new IUnitofwork();
            //    var GP = IU.Load(Qur);
            //    ListViewItem x = null;
            //    ListView1.Items.Clear();
            //    if (GP != null)
            //    {
            //        while (GP.Read())
            //        {
            //            x = new ListViewItem(GP["ID"].ToString());
            //            x.SubItems.Add(GP["Itemname"].ToString());
            //            x.SubItems.Add(Strings.Format(GP["itemprice"], "#,##0.00"));
            //            x.SubItems.Add(GP["OpeningBalance"].ToString());
            //            x.SubItems.Add(GP["UnitType"].ToString());
            //            x.SubItems.Add(GP["itemqty"].ToString());
            //            ListView1.Items.Add(x);
            //        }
            //    }
                
            //}
            //catch (Exception ex)
            //{
            //    Interaction.MsgBox(ex.ToString());
            //}
            //finally
            //{
            //    SQLConn.cmd.Dispose();
            //    SQLConn.conn.Close();
            //}
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmproductitemlist_Load(object sender, EventArgs e)
        {
            Loaditems();
        }
        private void Btnaddedit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemname.Text) ||
                string.IsNullOrEmpty(txtUnitPrice.Text) ||
                string.IsNullOrEmpty(txtStocksOnHand.Text) ||
                string.IsNullOrEmpty(textBox1.Text) ||
                string.IsNullOrEmpty(textBox2.Text))
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert all fildes", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("برجاء ادخال جميع الحقول", "خطأ");
                    return;
                }
            }
            else
            {
                Item.Itemname = txtItemname.Text;
                Item.Itemprice = Convert.ToDouble(txtUnitPrice.Text);
                Item.Itemqty = double.Parse(txtStocksOnHand.Text);
                Item.UID = unitid;
                Item.OpeningBalance = int.Parse(textBox2.Text);
                Item.Remining = Convert.ToDouble(txtRemining.Text);
                if (Btnaddedit.Text == "اضافة" || Btnaddedit.Text == "Add")
                {
                    var Qur = @"INSERT INTO Items(Itemname,Itemprice,Itemqty,UID,OpeningBalance,Remining) 
                                              VALUES('" + Item.Itemname + "', " +
                                                         "" + Item.Itemprice + ", " +
                              " " + Item.Itemqty + ", " +
                              " " + Item.UID + ", " +
                              " " + Item.OpeningBalance + ", " +
                              " " + Item.Remining + " " + ")";
                    Item.Crudopration(Qur);
                    Clearitems();
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("Add is successfully", "Add");
                    }
                    else
                    {
                        MessageBox.Show("تم الاضافة بنجاح", "اضافة تصنيف");
                    }
                }
                else
                {
                    if (Item.ID != null)
                    {
                        var Qur = @"Update Items Set
                                Itemname = '" + Item.Itemname + "', " +
                                    "Itemprice = '" + Item.Itemprice + "', " +
                                    "Itemqty = " + Item.Itemqty + ", " +
                                    "UID = " + Item.UID + ", " +
                                    "OpeningBalance = " + Item.OpeningBalance + ", " +
                                    "Remining = " + Item.Remining + " " +
                                    "where ID = " + Item.ID;
                        Item.Crudopration(Qur);
                        Clearitems();
                        if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                        {
                            MessageBox.Show("Edit is successfully", "Edit");
                        }
                        else
                        {
                            MessageBox.Show("تم التعديل بنجاح", "تعديل تصنيف");
                        }
                    }
                }
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            Clearitems();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (Item.ID != null)
            {
                var Qur = @"Delete From Items 
                            where ID = " + Item.ID;
                Item.Crudopration(Qur);
                Clearitems();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Delete is successfully", "Delete");
                }
                else
                {
                    MessageBox.Show("تم الحذف بنجاح", "حذف تصنيف");
                }
            }
        }
        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e,sender);
        }
        private void txtStocksOnHand_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        private void Btnunit_Click(object sender, EventArgs e)
        {
            Frmlistunit flu = new Frmlistunit(this);
            flu.ShowDialog();
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
                Item.ID = int.Parse(DGV.CurrentRow.Cells[0].Value.ToString());
                txtItemname.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                txtUnitPrice.Text = DGV.CurrentRow.Cells[2].Value.ToString();
                txtStocksOnHand.Text = DGV.CurrentRow.Cells[3].Value.ToString();
                textBox1.Text = DGV.CurrentRow.Cells[4].Value.ToString();
                textBox2.Text = DGV.CurrentRow.Cells[5].Value.ToString();
                unitid = int.Parse(DGV.CurrentRow.Cells[6].Value.ToString());
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                var GS = Item.Getdatabysearch(txtSearch.Text);
                DGV.DataSource = GS;
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    DGV.Columns[0].HeaderText = "Id";
                    DGV.Columns[1].HeaderText = "Description";
                    DGV.Columns[2].HeaderText = "Price";
                    DGV.Columns[3].HeaderText = "Amount";
                    DGV.Columns[4].HeaderText = "Unit";
                    DGV.Columns[5].HeaderText = "Balance";
                    DGV.Columns[6].HeaderText = "UID";
                    DGV.Columns[6].Visible = false;
                }
                else
                {
                    DGV.Columns[0].HeaderText = "المسلسل";
                    DGV.Columns[1].HeaderText = "الوصف";
                    DGV.Columns[2].HeaderText = "السعر";
                    DGV.Columns[3].HeaderText = "الكمية";
                    DGV.Columns[4].HeaderText = "الوحدة";
                    DGV.Columns[5].HeaderText = "الرصيد الافتتاحي";
                    DGV.Columns[6].HeaderText = "UID";
                    DGV.Columns[6].Visible = false;
                }
            }
            else
            {
                Loaditems();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Itemsales FIS = new Itemsales();
            FIS.Show();
        }

        private void txtStocksOnHand_TextChanged(object sender, EventArgs e)
        {
            txtRemining.Text = txtStocksOnHand.Text;
        }
    }
}
