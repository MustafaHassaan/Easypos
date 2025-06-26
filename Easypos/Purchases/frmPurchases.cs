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
using System.IO;
using Centeralized;
using Querylaeyr.Modelmaster;
using System.Windows.Forms.VisualStyles;
using Easypos.Payment;
using System.Windows.Documents;
using Querylaeyr.Modelsales;
using Querylaeyr.Paymodel;
using Reporting.Sales.Normal.Big;
using Reporting;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Reporting.Purchases;
using System.Security.Cryptography;
using Dataaccesslaeyr;

namespace Easypos.Purchases
{
    public partial class frmPurchases : Form
    {
        Centergetingfunctions CF;
        Vatinfo VI;
        Salespayments SPay;
        public List<Saleslist> SL;
        public int Invnum { get; set; }
        public string St { get; set; }
        Thirdparty Cust;
        CompanyInfo Comp;
        DataAccess Da;
        Product _Pro;
        Category Cat;
        public void Getinvoice()
        {
            try
            {
                if (Invnum != 0)
                {
                    DGV.Rows.Clear();
                    CF.Getpurchaselist(Invnum);
                    SL = CF.SL;
                    foreach (var item in SL)
                    {
                        DTP.Text = item.TDate.ToString();
                        DateTime dt = Convert.ToDateTime(item.TDate.ToString() + " " + item.TTime.ToString());
                        Time = dt;
                        clientID.Text = item.Name;
                        txtTBV.Text = item.NonVatTotal.ToString();
                        txtDiscount.Text = item.Discount.ToString();
                        txtTax.Text = item.VatAmount.ToString();
                        txtTotal.Text = item.TotalAmount.ToString();
                        Paymethode = item.PaymentMethod;
                        DGV.Rows.Add(
                            item.ProductNo,
                            item.TDDesc,
                            item.UnitType,
                            item.Unitid,
                            item.Quantity,
                            item.ItemPrice,
                            item.Discountdet,
                            item.Total
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "Get invoice" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public DateTime Time { get; set; }
        public string Paymethode { get; set; }
        public void Generatqrcode()
        {
            var Tot = double.Parse(txtTotal.Text);
            var Tax = double.Parse(txtTax.Text);
            var GDtlv = CF.DQ(Tot, Tax, Time);
            QRPic.Image = GDtlv;
        }
        public int Pronum { get; set; }
        public int Procode { get; set; }
        public int Probcode { get; set; }
        public int Proid { get; set; }
        public void Getpronumbers()
        {
            _Pro.Getdatabyid();
            Pronum = int.Parse(_Pro.ProductNo.ToString()) + 1;
            Procode = Pronum;
            Probcode = Pronum;
        }
        public frmPurchases()
        {
            Cat = new Category();
            Comp = new CompanyInfo();
            CF = new Centergetingfunctions();
            Da = new DataAccess();
            VI = new Vatinfo();
            Cust = new Thirdparty();
            Comp.GetCompany();
            Changelang();
            InitializeComponent();
            Getcustomers();
            VI.GetVat();
            Getcategories();
            Getunits();
            paymentMethod.SelectedIndex = 1;
            paymentType.SelectedIndex = 0;
            _Pro = new Product();
            //Producttype.SelectedIndex = 0;
        }
        public void Changelang()
        {
            if (Comp.Systemlang == "الانجليزية" || Comp.Systemlang == "English")
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
        public void Clearprice()
        {
            txtTBV.Clear();
            txtTBV.Text = "0";
            txtDiscount.Clear();
            txtDiscount.Text = "0";
            txtTax.Clear();
            txtTax.Text = "0";
            txtTotal.Clear();
            txtTotal.Text = "0";
        }
        public void DGSum()
        {
            try
            {
                Clearprice();
                double sumSubtotal = 0;
                double sumDiscount = 0;
                double sumTax = 0;
                double sum = 0;
                for (int i = 0; i < DGV.Rows.Count; i++)
                {
                    sumDiscount += Convert.ToDouble(DGV.Rows[i].Cells[6].Value);
                    sumSubtotal += Convert.ToDouble(DGV.Rows[i].Cells[7].Value);
                }
                txtTBV.Text = sumSubtotal.ToString();
                txtDiscount.Text = sumDiscount.ToString();
                var TotAfterDiscount = sumSubtotal - sumDiscount;
                if (ISVAT.Checked)
                {
                    var tax = (VI.VatPercent / 100) * TotAfterDiscount;
                    txtTax.Text = tax.ToString();
                    sum = TotAfterDiscount + tax;
                }
                else
                {
                    sum = TotAfterDiscount;
                }
                txtTotal.Text = sum.ToString();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "DGSum" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Cleardata()
        {
            txtprice.Clear();
            txtcount.Clear();
            Proname.Clear();
            //lblStockOnHand.Text = "0";
            Getunits();
            Getcategories();
            QRPic.Image = null;
        }
        public void AddProduct()
        {
            Getpronumbers();
            _Pro.ProductCode = Convert.ToString(Procode);
            _Pro.Barcode = Convert.ToString(Probcode);
            _Pro.Description = Proname.Text;
            _Pro.UnitPrice = double.Parse(txtprice.Text);
            _Pro.Unitid = int.Parse(unitTypes.SelectedValue.ToString());
            _Pro.StocksOnHand = int.Parse(txtcount.Text);
            _Pro.AllowInventory = true;
            _Pro.ShowInPOS = true;
            _Pro.CategoryNo = int.Parse(Cattype.SelectedValue.ToString());
            var Qur = @"Insert into Product(ProductCode,Description,Barcode,UnitPrice,CategoryNo,Unitid,StocksOnHand,ShowInPOS,AllowInventory)
                        Values(" + _Pro.ProductCode + ",'" 
                                 + _Pro.Description + "'," +
                                 _Pro.Barcode + "," + 
                                 _Pro.UnitPrice + "," +
                                 _Pro.CategoryNo + "," 
                                 + _Pro.Unitid + "," +
                                 _Pro.StocksOnHand + "," +
                                 _Pro.ShowInPOS + "," 
                                 + _Pro.AllowInventory +")";
            var Pro = _Pro.Crudopration(Qur);
            if (Pro)
            {
                var proid = _Pro.Getdatabyid();
                if (proid != null)
                {
                    Proid = int.Parse(proid[0].ToString());
                }
            }
        }
        public void AddRondomdgv()
        {
            var Totbd = Convert.ToDecimal(txtcount.Text) * Convert.ToDecimal(txtprice.Text);
            //DGV.Rows.Add(Proid,
            DGV.Rows.Add(0,
             Proname.Text,
             unitTypes.Text,
             unitTypes.SelectedValue,
             txtcount.Text,
             txtprice.Text, 0,
             Totbd, null, "منتجات");
        }
        public void Chachdgv()
        {
            Boolean Flag = false;
            int DI;
            for (DI = 0; DI < DGV.Rows.Count; DI++)
            {
                var PC = DGV.Rows[DI].Cells[0].Value.ToString();
                var Qty = Convert.ToDecimal(DGV.Rows[DI].Cells[4].Value);
                if (PC == Proid.ToString())
                {
                    Flag = true;
                    break;
                }
            }
            if (Flag)
            {
                DGV.Rows[DI].Cells[4].Value = Convert.ToDecimal(DGV.Rows[DI].Cells[4].Value.ToString()) + Convert.ToDecimal(txtcount.Text);
                var Qty = DGV.Rows[DI].Cells[4].Value;
                var Price = DGV.Rows[DI].Cells[5].Value;
                var Totbd = ((Convert.ToDecimal(Qty) * Convert.ToDecimal(Price)) - Convert.ToDecimal(DGV.Rows[DI].Cells[6].Value));
                DGV.Rows[DI].Cells[7].Value = Totbd;
            }
            else
            {
                AddRondomdgv();
            }
            Flag = false;
        }
        public void Getcustomers()
        {
            try
            {
                var GD = CF.GetSuppliers();
                DataRow dr = GD.NewRow();
                dr["ID"] = 0;
                dr["Name"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                clientID.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "Getcustomer" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getdataandshow()
        {
            lblproduct.Visible = true;
            //product.Visible = true;
            lblunit.Visible = true;
            unitTypes.Visible = true;
            lblprice.Visible = true;
            txtprice.Visible = true;
            lblcount.Visible = true;
            txtcount.Visible = true;
            //if (Producttype.Text == "منتجات")
            //{
            //    Getproducts();
            //    Getunits();
            //}
            //else
            //{
            //    Getproductitems();
            //}
        }
        public void Getproductitems()
        {
            try
            {
                var GDI = CF.Productitemsfilldata();
                DataRow dr = GDI.NewRow();
                dr["ID"] = 0;
                dr["Itemname"] = "--اختر--";
                GDI.Rows.InsertAt(dr, 0);
                //product.DataSource = GDI;
                //product.DisplayMember = "Itemname";
                //product.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "Getproductitems" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getproducts()
        {
            try
            {
                var GD = CF.Productfilldata();
                DataRow dr = GD.NewRow();
                dr["ProductNo"] = 0;
                dr["Description"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                //product.DataSource = GD;
                //product.DisplayMember = "Description";
                //product.ValueMember = "ProductNo";
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "Getproduct" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getunits()
        {
            try
            {
                var GD = CF.Unitfilldata();
                DataRow dr = GD.NewRow();
                dr["ID"] = 0;
                dr["UnitType"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                unitTypes.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "Getunits" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getcategories()
        {
            try
            {
                var GD = CF.Catfilldata();
                DataRow dr = GD.NewRow();
                dr["CategoryNo"] = 0;
                dr["CategoryName"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                Cattype.DisplayMember = "CategoryName";
                Cattype.ValueMember = "CategoryNo";
                Cattype.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "Getunits" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Btnbilllist_Click(object sender, EventArgs e)
        {
            Purbilllist PL = new Purbilllist();
            PL.Show();
        }
        private void Producttype_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cleardata();
            Getdataandshow();
        }
        private void Producttype_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Getdataandshow();
        }
        public void Selectfromunit()
        {
            //if (product.SelectedValue != null)
            //{
            //    Cleardata();
            //    if (product.SelectedIndex != 0)
            //    {
            //        if (Producttype.Text == "منتجات")
            //        {
            //            var data = CF.fillunitid(int.Parse(product.SelectedValue.ToString()));
            //            if (data != null)
            //            {
            //                unitTypes.SelectedValue = data["Unitid"].ToString();
            //                txtprice.Text = data["UnitPrice"].ToString();
            //                var GS = CF.GetStock(int.Parse(product.SelectedValue.ToString()));
            //                if (GS != null)
            //                {
            //                    lblStockOnHand.Text = GS.ToString();
            //                }
            //            }
            //        }
            //        else
            //        {
            //            var data = CF.fillitemsunitid(int.Parse(product.SelectedValue.ToString()));
            //            if (data != null)
            //            {
            //                unitTypes.SelectedValue = data["UID"].ToString();
            //                txtprice.Text = data["Itemprice"].ToString();
            //                var GS = CF.GetStockitems(int.Parse(product.SelectedValue.ToString()));
            //                if (GS != null)
            //                {
            //                    lblStockOnHand.Text = GS.ToString();
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        Getunits();
            //        Cleardata();
            //    }
            //}
            //else
            //{
            //    Getunits();
            //    Cleardata();
            //}
        }
        private void product_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Cleardata();
            Selectfromunit();
        }
        public void AddProducttodgv()
        {
            var Prc = txtprice.Text;
            var CDP = Convert.ToDouble(txtprice.Text);
            if (string.IsNullOrEmpty(Proname.Text))
            {
                MessageBox.Show("برجاء المنتج لكي يمكن اضافة المنتجات", "خطأ");
                return;
            }
            else if (unitTypes.Text == "" || unitTypes.SelectedIndex == 0)
            {
                MessageBox.Show("برجاء ادخال وحدات لكي يمكن اضافة المنتجات", "خطأ");
                return;
            }
            else if (txtcount.Text == "" || txtcount.Text == "0" || txtcount.Text == ".")
            {
                MessageBox.Show("ادخل الكمية", "خطأ");
                return;
            }
            else if (Prc == "" || Prc == "0" || Prc == "." || CDP < 0)
            {
                MessageBox.Show("ادخل السعر صحيح", "خطأ");
                return;
            }
            else
            {
                //if (DGV.Rows.Count > 0)
                //{
                //    Chachdgv();
                //}
                //else
                //{
                //    AddRondomdgv();
                //}
                AddRondomdgv();
                DGSum();
                Cleardata();
            }
        }
        private void txtcount_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
            if (e.KeyChar == (char)13)
            {
                AddProducttodgv();
            }
        }
        private void Btnadd_Click(object sender, EventArgs e)
        {
            //Getpronumbers();
            //AddProduct();
            AddProducttodgv();
            //Cleardata();
        }
        private void ISVAT_CheckedChanged(object sender, EventArgs e)
        {
            DGSum();
        }
        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Columns[e.ColumnIndex].Name == "Delete")
            {
                int rowIndex = DGV.CurrentCell.RowIndex;
                DGV.Rows.RemoveAt(rowIndex);
                DGSum();
            }
        }
        public void AddProducttostok(int Procode, int Qty)
        {
            try
            {
                var GPD = CF.Getdatabyid(Procode);
                if (GPD != null)
                {
                    var Quantity = Qty + CF.Pro.StocksOnHand;
                    var Qur = @"Update Product Set
                            StocksOnHand = " + Quantity + " " +
                               "Where ProductNo = " + Procode;
                    CF.Pro.Crudopration(Qur);
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "AddProductstock" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void AddItemstostok(int Procode, int Qty)
        {
            try
            {
                var GPD = CF.Getdataitemsbyid(Procode);
                if (GPD != null)
                {
                    var Quantity = Qty + CF.Pro.StocksOnHand;
                    var Qur = @"Update items Set
                            Itemqty = " + Quantity + " " +
                               "Where ID = " + Procode;
                    CF.Pro.Crudopration(Qur);
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "Additemstok" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void SaveAndEdit()
        {
            for (int i = 0; i < DGV.Rows.Count; i++)
            {
                SPay.sald.InvoiceNo = SPay.sal.Invoiceno;
                SPay.sald.ProductNo = int.Parse(DGV.Rows[i].Cells[0].Value.ToString());
                SPay.sald.TDDesc = DGV.Rows[i].Cells[1].Value.ToString();
                SPay.sald.Unitid = int.Parse(DGV.Rows[i].Cells[3].Value.ToString());
                SPay.sald.ItemPrice = int.Parse(DGV.Rows[i].Cells[5].Value.ToString());
                SPay.sald.Quantity = int.Parse(DGV.Rows[i].Cells[4].Value.ToString());
                SPay.sald.Subtotal = decimal.Parse(SPay.sald.ItemPrice.ToString()) * decimal.Parse(SPay.sald.Quantity.ToString());
                SPay.sald.Discount = int.Parse(DGV.Rows[i].Cells[6].Value.ToString());
                SPay.sald.Totafterdiscount = SPay.sald.Subtotal - SPay.sald.Discount;
                SPay.sald.Total = int.Parse(DGV.Rows[i].Cells[7].Value.ToString());
                var Protype = DGV.Rows[i].Cells[9].Value.ToString();
                if (paymentType.Text != "مسودة")
                {
                    if (Protype == "منتجات")
                    {
                        AddProducttostok(SPay.sald.ProductNo, int.Parse(SPay.sald.Quantity.ToString()));
                    }
                    else
                    {
                        AddItemstostok(SPay.sald.ProductNo, int.Parse(SPay.sald.Quantity.ToString()));
                    }
                }
                SPay.sald.Savepurchasedetailes();
            }
        }
        public void Printinv()
        {
            try
            {
                Comp = new CompanyInfo();
                Comp.GetCompany();
                Frmreporting FR = new Frmreporting();
                Purchasebill Rep = new Purchasebill();
                Dataset Ds = new Dataset();
                byte[] QRCode = null;
                string date = "";
                string Supplierbillnumber = "";
                string Totaldiscount = "";
                string Totalafterdiscount = "";
                if (Invnum != 0)
                {

                    if (ISVAT.Checked)
                    {
                        Generatqrcode();
                        QRCode = CF.CTB(QRPic.Image);
                    }
                    CF.Getpurchaselist(Invnum);
                    SL = CF.SL;
                    int Countid = 0;
                    var invtyper = "";
                    var Wordofnumber = "";
                    foreach (var item in SL)
                    {
                        Paymethode = item.PaymentMethod;
                        Supplierbillnumber = item.Supplierbill;
                        date = item.TDate;
                        Countid++;
                        Totalafterdiscount = Convert.ToString(item.NonVatTotal - item.Discount);
                        Totaldiscount = item.Discount.ToString();
                        invtyper = item.Billtype;
                        if (invtyper == "صدرت")
                        {
                            invtyper = item.Invoiceno.ToString();
                        }
                        else
                        {
                            invtyper = item.Billtype;
                        }
                        var Tot = Convert.ToDecimal(item.TotalAmount);
                        decimal GTot = Convert.ToDecimal(string.Format("{0:0.00}", Tot));
                        ConvertNumbersToArabicAlphabet a = new ConvertNumbersToArabicAlphabet(GTot.ToString());
                        Wordofnumber = a.GetNumberAr();
                        DTP.Text = item.TDate.ToString();
                        DateTime dt = Convert.ToDateTime(item.TDate.ToString() + " " + item.TTime.ToString());
                        Time = dt;
                        clientID.Text = item.Name;
                        Cust.Taxnumber = item.Taxnumber;
                        Cust.Address = item.Address;
                        Cust.MobileNumber = item.MobileNumber;
                        txtTBV.Text = item.NonVatTotal.ToString();
                        txtDiscount.Text = item.Discount.ToString();
                        txtTax.Text = item.VatAmount.ToString();
                        txtTotal.Text = item.TotalAmount.ToString();
                        byte[] Logo = Convert.FromBase64String(Comp.Logo);
                        Ds.Bill.Rows.Add(new object[] {
                        invtyper,
                        item.ItemPrice,
                        item.Quantity,
                        item.Discountdet,
                        item.Total,
                        item.Name,
                        QRCode,
                        Countid,
                        item.TDDesc,
                        item.UnitType,
                        item.VatAmount,
                        item.TotalAmount,
                        Logo
                    });
                    }

                    Rep.SetDataSource(Ds);


                    FR.CRV.ReportSource = Rep;
                    FR.CRV.Refresh();

                    Rep.SetParameterValue("Date", date);
                    Rep.SetParameterValue("CompanyName", Comp.CName);
                    Rep.SetParameterValue("CompanyName", Comp.CName);
                    Rep.SetParameterValue("Cashirname", St);
                    Rep.SetParameterValue("Taxnum", Comp.Taxnumber);
                    Rep.SetParameterValue("Proname", Comp.CRN);
                    Rep.SetParameterValue("MobileNo", Cust.MobileNumber);
                    Rep.SetParameterValue("Custtax", Cust.Taxnumber);
                    Rep.SetParameterValue("English_Shop_name", Comp.CENName);
                    Rep.SetParameterValue("Supplierbillnumber", Supplierbillnumber);
                    Rep.SetParameterValue("Wordofnumber", Wordofnumber);
                    Rep.SetParameterValue("Totalafterdiscount", Totalafterdiscount);
                    Rep.SetParameterValue("Totaldiscount", Totaldiscount);
                }
                else
                {
                    MessageBox.Show("لا يوجد فاتورة لطباعتها", "خطأ");
                }
                FR.ShowDialog();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "Printinv" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Chkfildesandsave(string Printing)
        {
            try
            {
                SL = CF.SL;
                string Date = "";
                string time = "";
                if (clientID.SelectedIndex == 0)
                {
                    MessageBox.Show("برجاء ادخال المورد", "خطأ");
                    return;
                }
                else if (DGV.Rows.Count == 0)
                {
                    MessageBox.Show("لا يوجد طلبات", "خطأ");
                    return;
                }
                else
                {
                    SPay = new Salespayments();
                    SPay.Typeprinting = Printing;
                    SPay.clients.DataSource = clientID.DataSource;
                    SPay.clients.SelectedValue = clientID.SelectedValue;
                    SPay.txtTotal.Text = txtTotal.Text;
                    SPay.sal.NonVatTotal = double.Parse(txtTBV.Text);
                    SPay.sal.Discount = double.Parse(txtDiscount.Text);
                    SPay.sal.VatAmount = double.Parse(txtTax.Text);
                    SPay.sal.TotalAmount = double.Parse(txtTotal.Text);
                    SPay.sal.ThirdPartyID = int.Parse(SPay.clients.SelectedValue.ToString());
                    SPay.sal.Billtype = paymentType.Text;
                    SPay.sal.Supplierbill = BStxt.Text;
                    if (Btnsave.Text == "حفظ")
                    {
                        SPay.sal.TDate = DateTime.Now.ToString("yyyy-MM-dd");
                        SPay.sal.TTime = DateTime.Now.ToString("HH:mm:ss");
                        SPay.sal.Savepurchase();
                        SPay.sal.GetPurlastbyid();
                        SaveAndEdit();
                    }
                    else
                    {
                        foreach (var item in SL)
                        {
                            Date = item.TDate;
                            time = item.TTime;
                        }
                        SPay.sal.TDate = Date;
                        SPay.sal.TTime = time;
                        if (Invnum != 0)
                        {
                            SPay.Invnum = Invnum;
                            SPay.Pm.Getpurdata(SPay.Invnum);
                            SPay.txtCash.Text = SPay.Pm.Cash.ToString();
                            SPay.txtBank.Text = SPay.Pm.Bank.ToString();
                        }
                    }
                    SPay.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المشتريات" + "','" + "Chkfildesandsave" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void Btnsave_Click(object sender, EventArgs e)
        {
            Chkfildesandsave(Btnsave.Text);
        }
        private void Btnsaveandprint_Click(object sender, EventArgs e)
        {
            Chkfildesandsave(Btnsaveandprint.Text);
        }
        public void Clearall()
        {
            CF = new Centergetingfunctions();
            VI = new Vatinfo();
            Cust = new Thirdparty();
            Comp = new CompanyInfo();
            Getcustomers();
            Comp.GetCompany();
            VI.GetVat();
            paymentMethod.SelectedIndex = 1;
            paymentType.SelectedIndex = 0;
            //Producttype.SelectedIndex = 0;
            BStxt.Text = "0";
            DGV.DataSource = null;
            DGV.Rows.Clear();
            QRPic.Image = null;
            ISVAT.Checked = false;
            DGSum();
        }
        private void Btnprint_Click(object sender, EventArgs e)
        {
            Printinv();
        }
    }
}
