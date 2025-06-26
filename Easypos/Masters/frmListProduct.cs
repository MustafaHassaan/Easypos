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
using Easypos.Masters.Subforms;
using Querylaeyr.Modelmaster;
using Centeralized;
using System.Windows.Controls;
using Control = System.Windows.Forms.Control;
//using Org.BouncyCastle.Utilities.Collections;

namespace Easypos.Masters
{
    public partial class frmListProduct : Form
    {
        public string UID { get; set; }
        public string CID { get; set; }
        Product _Pro;
        Items _Proidtem;
        Centergetingfunctions _ON;
        CompanyInfo _com;
        CompanyInfo CI;
        public frmListProduct()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            _Pro = new Product();
            Getproducts();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Add";
            }
            else
            {
                Btnaddedit.Text = "اضافة";
            }
            _ON = new Centergetingfunctions();
            _Proidtem = new Items();
            _com = new CompanyInfo();
            _com.GetCompany();
            if (_com.ISUSEBarcode)
            {
                lblProductNo.Enabled = false;
                txtProductCode.Enabled = false;
                txtBarcode.Enabled = false;
                _Pro.Getdatabyid();
                lblProductNo.Text = Convert.ToString(int.Parse(_Pro.ProductNo.ToString()) + 1);
                txtProductCode.Text = Convert.ToString(int.Parse(_Pro.ProductNo.ToString()) + 1);
                txtBarcode.Text = Convert.ToString(int.Parse(_Pro.ProductNo.ToString()) + 1);
            }
            Getitems();
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
        public void Getproducts()
        {
            var GD = _Pro.Getdata();
            DGV.DataSource = GD;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                DGV.Columns[0].HeaderText = "Id";
                DGV.Columns[1].HeaderText = "Code";
                DGV.Columns[2].HeaderText = "Description";
                DGV.Columns[3].HeaderText = "Barcode";
                DGV.Columns[4].HeaderText = "Price";
                DGV.Columns[5].HeaderText = "Amount";
                DGV.Columns[6].HeaderText = "Category";
                DGV.Columns[7].HeaderText = "Unit";
                DGV.Columns[8].HeaderText = "Unitid";
                DGV.Columns[8].Visible = false;
                DGV.Columns[9].HeaderText = "CategoryNo";
                DGV.Columns[9].Visible = false;
                DGV.Columns[10].HeaderText = "Showpos";
                DGV.Columns[10].Visible = false;
                DGV.Columns[11].HeaderText = "Allowinv";
                DGV.Columns[11].Visible = false;
                DGV.Columns[12].HeaderText = "Checkedit";
                DGV.Columns[12].Visible = false;
                DGV.Columns[13].HeaderText = "ReorderLevel";
                DGV.Columns[13].Visible = false;
            }
            else
            {
                DGV.Columns[0].HeaderText = "المسلسل";
                DGV.Columns[1].HeaderText = "رمز المنتج";
                DGV.Columns[2].HeaderText = "الوصف";
                DGV.Columns[3].HeaderText = "الباركود";
                DGV.Columns[4].HeaderText = "السعر";
                DGV.Columns[5].HeaderText = "الكميه";
                DGV.Columns[6].HeaderText = "التصنيف";
                DGV.Columns[7].HeaderText = "الوحده";
                DGV.Columns[8].HeaderText = "Unitid";
                DGV.Columns[8].Visible = false;
                DGV.Columns[9].HeaderText = "CategoryNo";
                DGV.Columns[9].Visible = false;
                DGV.Columns[10].HeaderText = "Showpos";
                DGV.Columns[10].Visible = false;
                DGV.Columns[11].HeaderText = "Allowinv";
                DGV.Columns[11].Visible = false;
                DGV.Columns[12].HeaderText = "Checkedit";
                DGV.Columns[12].Visible = false;
                DGV.Columns[13].HeaderText = "ReorderLevel";
                DGV.Columns[13].Visible = false;
            }

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            Clearfileds();
        }
        public void Clearfileds()
        {
            Getproducts();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Btnaddedit.Text = "Add";
            }
            else
            {
                Btnaddedit.Text = "اضافة";
            }
            lblProductNo.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtBarcode.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            textBox1.Text = string.Empty;
            txtCategory.Text = string.Empty;
            txtSearch.Text = string.Empty;
            txtReorderLevel.Text = string.Empty;
            txtStocksOnHand.Text = string.Empty;
            ShowInPOS.Checked = false;
            allowInventory.Checked = false;
            DGVProitems.DataSource = null;
            DGVProitems.Rows.Clear();
            if (_com.ISUSEBarcode)
            {
                lblProductNo.Enabled = false;
                txtProductCode.Enabled = false;
                txtBarcode.Enabled = false;
                _Pro.Getdatabyid();
                lblProductNo.Text = Convert.ToString(int.Parse(_Pro.ProductNo.ToString()) + 1);
                txtProductCode.Text = Convert.ToString(int.Parse(_Pro.ProductNo.ToString()) + 1);
                txtBarcode.Text = Convert.ToString(int.Parse(_Pro.ProductNo.ToString()) + 1);
            }
            CBItems.SelectedIndex = 0;
        }
        private void frmListProduct_Load(object sender, EventArgs e)
        {
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Frmlistunit flu = new Frmlistunit(this);
            flu.ShowDialog();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            frmSelectCategory flc = new frmSelectCategory(this);
            flc.ShowDialog();
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
                lblProductNo.Text = DGV.CurrentRow.Cells[0].Value.ToString();
                txtProductCode.Text = DGV.CurrentRow.Cells[1].Value.ToString();
                txtDescription.Text = DGV.CurrentRow.Cells[2].Value.ToString();
                txtBarcode.Text = DGV.CurrentRow.Cells[3].Value.ToString();
                txtUnitPrice.Text = DGV.CurrentRow.Cells[4].Value.ToString();
                txtStocksOnHand.Text = DGV.CurrentRow.Cells[5].Value.ToString();
                txtCategory.Text = DGV.CurrentRow.Cells[6].Value.ToString();
                textBox1.Text = DGV.CurrentRow.Cells[7].Value.ToString();
                UID = DGV.CurrentRow.Cells[8].Value.ToString();
                CID = DGV.CurrentRow.Cells[9].Value.ToString();
                var SIP = DGV.CurrentRow.Cells[10].Value.ToString();
                if (SIP == "1")
                {
                    ShowInPOS.Checked = true;
                }
                else
                {
                    ShowInPOS.Checked = false;
                }
                var Allowinv = DGV.CurrentRow.Cells[11].Value.ToString();
                if (Allowinv == "1")
                {
                    allowInventory.Checked = true;
                }
                else
                {
                    allowInventory.Checked = false;
                }
                txtReorderLevel.Text = DGV.CurrentRow.Cells[13].Value.ToString();
                var DT = _Proidtem.Getdatabyquery(int.Parse(lblProductNo.Text));
                if (DT != null)
                {
                    if (DGVProitems.Rows.Count > 0)
                    {
                        DGVProitems.Rows.Clear();
                    }
                    DGVProitems.AllowUserToAddRows = false;
                    var Qur = @"Select 
                                productitems.itemid,
                                items.Itemname,
                                productitems.Quantity
                                From productitems
                                Left Outer Join Items
                                on productitems.itemid = items.ID Where productitems.Proid = " + int.Parse(lblProductNo.Text);
                    var GDT = _Proidtem.Getitemsbydatatable(Qur);
                    foreach (DataRow row in GDT.Rows)
                    {
                        DGVProitems.Rows.Add(row["itemid"].ToString(), 
                                             row["Itemname"].ToString(),
                                             row["Quantity"].ToString());
                    }
                    //DGVProitems.DataSource = GDT;
                }
                else
                {
                    if (DGVProitems.Rows.Count > 0)
                    {
                        int i = 0;
                        while (DGVProitems.Rows.Count >= i)
                        {
                            DGVProitems.Rows.Remove(DGVProitems.CurrentRow);
                            i++;
                        }
                    }
                }
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                var GS = _Pro.Getdatabysearch(txtSearch.Text);
                //DGV.Columns[0].HeaderText = "المسلسل";
                //DGV.Columns[1].HeaderText = "رمز المنتج";
                //DGV.Columns[2].HeaderText = "الوصف";
                //DGV.Columns[3].HeaderText = "الباركود";
                //DGV.Columns[4].HeaderText = "السعر";
                //DGV.Columns[5].HeaderText = "الكميه";
                //DGV.Columns[6].HeaderText = "التصنيف";
                //DGV.Columns[7].HeaderText = "الوحده";
                //DGV.Columns[8].HeaderText = "Unitid";
                //DGV.Columns[8].Visible = false;
                //DGV.Columns[9].HeaderText = "CategoryNo";
                //DGV.Columns[9].Visible = false;
                //DGV.Columns[10].HeaderText = "Showpos";
                //DGV.Columns[10].Visible = false;
                //DGV.Columns[11].HeaderText = "Allowinv";
                //DGV.Columns[11].Visible = false;
                //DGV.Columns[12].HeaderText = "Checkedit";
                //DGV.Columns[12].Visible = false;
                //DGV.Columns[13].HeaderText = "ReorderLevel";
                //DGV.Columns[13].Visible = false;
                DGV.DataSource = GS;
            }
            else
            {
                Getproducts();
            }
        }
        public void AddProduct()
        {
            _Pro.ProductCode = txtProductCode.Text;
            _Pro.Description = txtDescription.Text;
            _Pro.Barcode = txtBarcode.Text;
            _Pro.UnitPrice = double.Parse(txtUnitPrice.Text);
            _Pro.CategoryNo = int.Parse(CID);
            _Pro.Unitid = int.Parse(UID);
            if (allowInventory.Checked)
            {
                _Pro.AllowInventory = true;
            }
            if (ShowInPOS.Checked)
            {
                _Pro.ShowInPOS = true;
            }
            if (!string.IsNullOrEmpty(txtStocksOnHand.Text))
            {
                _Pro.StocksOnHand = int.Parse(txtStocksOnHand.Text);
            }
            if (!string.IsNullOrEmpty(txtReorderLevel.Text))
            {
                _Pro.ReorderLevel = int.Parse(txtReorderLevel.Text);
            }
            var Qur = @"Insert into Product(ProductCode,Description,Barcode,UnitPrice,CategoryNo,Unitid,StocksOnHand,ShowInPOS,AllowInventory,Checkedit,ReorderLevel)
                        Values(" + _Pro.ProductCode +",'"+_Pro.Description+"',"+
                                 _Pro.Barcode +","+ _Pro.UnitPrice + "," + 
                                 _Pro.CategoryNo + ","+ _Pro.Unitid +"," +
                                 _Pro.StocksOnHand + "," +
                                 _Pro.ShowInPOS + "," + _Pro.AllowInventory + "," +
                                 _Pro.Checkedit + "," + _Pro.ReorderLevel +")";
            var Pro = _Pro.Crudopration(Qur);
            if (Pro)
            {
                var proid = _Pro.Getdatabyid();
                if (proid != null)
                {
                    var id = int.Parse(proid[0].ToString());
                    AddProductitems(id);
                }
            }
        }
        public void AddProductitems(int id)
        {
            for (int i = 0; i < DGVProitems.Rows.Count; i++)
            {
                var itemid = DGVProitems.Rows[i].Cells["itemid"].Value;
                var Data = DGVProitems.Rows[i].Cells["itemname"].Value;
                var Qty = DGVProitems.Rows[i].Cells["Quantity"].Value;
                if (Data != null)
                {
                    var Qur = "INSERT INTO Productitems(Proid,Itemid,Quantity) VALUES(" + id + "," + itemid + "," + Qty + ")";
                    var Pro = _Pro.Crudopration(Qur);
                }
            }
        }
        public void EditProduct()
        {
            _Pro.ProductNo = int.Parse(lblProductNo.Text);
            _Pro.ProductCode = txtProductCode.Text;
            _Pro.Description = txtDescription.Text;
            _Pro.Barcode = txtBarcode.Text;
            _Pro.UnitPrice = double.Parse(txtUnitPrice.Text);
            _Pro.CategoryNo = int.Parse(CID);
            _Pro.Unitid = int.Parse(UID);
            var Qur = @"Update Product Set
                        ProductCode = "+ _Pro.ProductCode + ","+
                       "Description = '"+ _Pro.Description + "',"+
                       "Barcode = " + _Pro.Barcode + ","+
                       "UnitPrice = " + _Pro.UnitPrice + ","+
                       "CategoryNo = " + _Pro.CategoryNo + ","+
                       "Unitid = " + _Pro.Unitid + " "+
                       "Where ProductNo = " + _Pro.ProductNo;
            var Pro = _Pro.Crudopration(Qur);
            if (Pro)
            {
                var id = _Pro.ProductNo;
                Deleteproductoptions();
            }
        }
        public void Deleteproductitems(int id)
        {
            var Qur = @"Delete From Productitems Where Proid = " + id;
            var Deldata = _Pro.Crudopration(Qur);
        }
        public void Deleteproductoptions()
        {
            var Qur = @"Delete From Stock Where Proid = " + _Pro.ProductNo;
            var Deldata = _Pro.Crudopration(Qur);
        }
        private void Btnaddedit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert Description", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("برجاء ادخال وصف الصنف", "خطأ");
                    return;
                }
            }
            if (string.IsNullOrEmpty(txtCategory.Text))
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English") {
                    MessageBox.Show("Please insert Category", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("برجاء ادخال فئة الصنف", "خطأ");
                    return;
                }
            }
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert unit", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("برجاء ادخال وحدة الصنف", "خطأ");
                    return;
                }
            }
            if (string.IsNullOrEmpty(txtUnitPrice.Text) || 
                txtUnitPrice.Text.Trim() == "" || 
                txtUnitPrice.Text == "0" || 
                txtUnitPrice.Text == "0.0" || 
                txtUnitPrice.Text == "0.00")
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please enter the item price", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("برجاء ادخال سعر الصنف", "خطأ");
                    return;
                }
            }
            else
            {
                if (Btnaddedit.Text == "اضافة" || Btnaddedit.Text == "Add")
                {
                    AddProduct();
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("The product has not been Added.", "Success");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("تم الحفظ بنجاح", "تم");
                        return;
                    }
                }
                else
                {
                    Deleteproductitems(int.Parse(lblProductNo.Text));
                    AddProductitems(int.Parse(lblProductNo.Text));
                    EditProduct();
                    Clearfileds();
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("The product has not been Edit.", "Success");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("تم التعديل بنجاح", "تم");
                        return;
                    }
                }
            }
        }
        public void DeleteStock()
        {
            var Qur = @"Delete From Stock Where Proid = " + _Pro.ProductNo;
            var Deldata = _Pro.Crudopration(Qur);
        }
        private void Btndel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblProductNo.Text))
            {
                var Qur = @"Delete From Product Where ProductNo = " + int.Parse(lblProductNo.Text);
                var Deldata = _Pro.Crudopration(Qur);
                if (Deldata)
                {
                    Deleteproductitems(int.Parse(lblProductNo.Text));
                    DeleteStock();
                    Clearfileds();
                    if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                    {
                        MessageBox.Show("The product has not been deleted.", "Success");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("تم حذف المنتج بنجاح", "حذف منتج");
                        return;
                    }
                }
            }
            else
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("The product has not been deleted.", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("لم يتم حذف المنتج", "حذف منتج");
                    return;
                }
            }

        }
        private void txtStocksOnHand_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e,sender);
        }
        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        private void txtReorderLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ON.Usenumber(e, sender);
        }
        private void DGVProitems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGVProitems.Columns[e.ColumnIndex].Name == "Delete")
            {
                var RI = DGVProitems.CurrentRow.Index;
                var Data = DGVProitems.Rows[RI].Cells["itemname"].Value;
                if (Data != null)
                {
                    DGVProitems.Rows.RemoveAt(DGVProitems.CurrentRow.Index);
                    DGVProitems.Refresh();
                }
            }
            else if (DGVProitems.Columns[e.ColumnIndex].Name == "itemname")
            {
                Getitems();
            }
        }
        public void Getitems()
        {
            var Qur = "SELECT ID,Itemname FROM items";
            var Dtbl = _Proidtem.Getitemsbydatatable(Qur);
            DataRow dr = Dtbl.NewRow();
            dr["ID"] = 0;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                dr["Itemname"] = "--Choose--";
            }
            else
            {
                dr["Itemname"] = "--اختر--";
            }
            Dtbl.Rows.InsertAt(dr, 0);
            CBItems.DataSource = Dtbl;
            CBItems.ValueMember = "ID";
            CBItems.DisplayMember = "Itemname";
        }
        private void Btnadd_Click(object sender, EventArgs e)
        {
            if (CBItems.SelectedIndex == 0)
            {
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please insert product items", "Error");
                    return;
                }
                else
                {
                    MessageBox.Show("برجاء اختيار الصنف الاولي", "خطأ");
                    return;
                }
            }
            else
            {
                DGVProitems.Rows.Add(CBItems.SelectedValue.ToString(),CBItems.Text,1);
            }
        }
    }
}
