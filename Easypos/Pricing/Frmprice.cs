using Centeralized;
using Easypos.Masters;
using Easypos.Payment;
using Easypos.Salesforms;
using Querylaeyr.Modelmaster;
using Querylaeyr.Modelsales;
using Querylaeyr.Paymodel;
using Querylaeyr.ResturantModles;
using Reporting.Sales.Normal.Big;
using Reporting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using Querylaeyr.Pricingmodels;
using Reporting.Priceoffer;
using Dataaccesslaeyr;
using org.apache.xerces.impl.dv.xs;

namespace Easypos.Pricing
{
    public partial class Frmprice : Form
    {
        Centergetingfunctions CF;
        Vatinfo VI;
        public Price Prc;
        pricedetailes Pd;
        public List<Pricelist> PDL;
        public int Invnum { get; set; }
        public string St { get; set; }
        public string Paymethode { get; set; }
        public DateTime Time { get; set; }
        Thirdparty Cust;
        Sales sal;
        Salesdetails Sd;
        DataAccess Da;
        bool Diskflag = false;
        bool Totflag = false;
        public Frmprice()
        {
            InitializeComponent();
            Cust = new Thirdparty();
            CF = new Centergetingfunctions();
            CF.Comp = new CompanyInfo();
            CF.Comp.GetCompany();
            VI = new Vatinfo();
            PDL = new List<Pricelist>();
            VI.GetVat();
            Prc = new Price();
            Pd = new pricedetailes();
            if (VI.ISDiscafterpro)
            {
                txtDiscount.Enabled = true;
                txtTotal.Enabled = true;
                DGV.Columns["Discount"].Visible = false;
            }
            if (VI.ISDiscforpro)
            {
                txtDiscount.Enabled = false;
                txtTotal.Enabled = false;
                DGV.Columns["Discount"].Visible = true;
            }
            sal = new Sales();
            Sd = new Salesdetails();
        }
        public void Printinv()
        {
            try
            {
                Frmreporting FR = new Frmreporting();
                Priceoffer Rep = new Priceoffer();
                Dataset Ds = new Dataset();
                byte[] QRCode = null;
                string Totaldiscount = "";
                string Totalafterdiscount = "";
                string date = "";
                Invnum = Prc.ID;
                if (Invnum != 0)
                {
                    CF.Getpriceoffer(Invnum);
                    PDL = CF.PDL;
                    int Countid = 0;
                    var invtyper = "";
                    var Wordofnumber = "";
                    var Datefrom = "";
                    var Dateto = "";
                    Cust = new Thirdparty();
                    foreach (var item in PDL)
                    {
                        var Tot = Convert.ToDecimal(item.TotalAmount);
                        decimal GTot = Convert.ToDecimal(string.Format("{0:0.00}", Tot));
                        ConvertNumbersToArabicAlphabet a = new ConvertNumbersToArabicAlphabet(GTot.ToString());
                        Wordofnumber = a.GetNumberAr();
                        Totaldiscount = item.Discount.ToString();
                        Totalafterdiscount = item.TotalAmount.ToString();
                        Datefrom = item.Datefrom.ToString();
                        Dateto = item.Dateto.ToString();
                        clientID.Text = item.Name;
                        Cust.Taxnumber = item.Taxnumber;
                        Cust.Address = item.Address;
                        Cust.MobileNumber = item.MobileNumber;
                        txtTBV.Text = item.NonVatTotal.ToString();
                        txtDiscount.Text = item.Discount.ToString();
                        txtTax.Text = item.VatAmount.ToString();
                        txtTotal.Text = item.TotalAmount.ToString();
                        byte[] Logo = Convert.FromBase64String(CF.Comp.Logo);
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

                    Rep.SetParameterValue("Datefrom", Datefrom);
                    Rep.SetParameterValue("Dateto", Dateto);
                    Rep.SetParameterValue("Date", date);
                    Rep.SetParameterValue("CompanyName", CF.Comp.CName);
                    Rep.SetParameterValue("Totalafterdiscount", Totalafterdiscount);
                    Rep.SetParameterValue("Totaldiscount", Totaldiscount);
                    Rep.SetParameterValue("CompanyName", CF.Comp.CName);
                    Rep.SetParameterValue("Cashirname", St);
                    Rep.SetParameterValue("Address", CF.Comp.Address);
                    Rep.SetParameterValue("Taxnum", CF.Comp.Taxnumber);
                    Rep.SetParameterValue("Proname", CF.Comp.CRN);
                    Rep.SetParameterValue("Custaddress", Cust.Address);
                    Rep.SetParameterValue("Custtax", Cust.Taxnumber);
                    Rep.SetParameterValue("Phone", Cust.MobileNumber);
                    Rep.SetParameterValue("English_Shop_name", CF.Comp.CENName);
                    Rep.SetParameterValue("Wordofnumber", Wordofnumber);
                }
                else
                {
                    MessageBox.Show("لا يوجد فاتورة لطباعتها", "خطأ");
                }
                FR.ShowDialog();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة عرض الاسعار" + "','" + "Printinv" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getcustomers()
        {
            try
            {
                var GD = CF.Getthirdparties();
                DataRow dr = GD.NewRow();
                dr["ID"] = 0;
                dr["Name"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                clientID.DataSource = GD;
                clientID.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة عرض الاسعار" + "','" + "Getcustomers" + "','" + ex.Message.ToString() + "')";
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
                cmbproducts.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة عرض الاسعار" + "','" + "Productfilldata" + "','" + ex.Message.ToString() + "')";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة عرض الاسعار" + "','" + "Unitfilldata" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void DGSum()
        {
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
            if (VI.ISVAT)
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
        public void Changetot()
        {

        }
        public void Changedisc()
        {
            if (VI.ISVAT)
            {
                //المبلغ الضريبي
                var TAD = Convert.ToDouble(txtTBV.Text) - Convert.ToDouble(txtDiscount.Text);
                var Tax = TAD * (VI.VatPercent / 100);
                var GTax = Math.Round(Convert.ToDecimal(Tax), 2).ToString();
                //                var GT = Math.Round(Convert.ToDecimal(GTax), MidpointRounding.ToEven);

                txtTax.Text = Convert.ToString(GTax);

                //المبلغ بعد الضريبة
                var TAV = Convert.ToDouble(txtTax.Text) + TAD;
                txtTotal.Text = Convert.ToString(TAV);
            }
            else
            {
                var TB = Convert.ToDouble(txtTBV.Text);
                var Tot = Convert.ToDouble(txtDiscount.Text);
                var Res = TB - Tot;
                txtTotal.Text = Res.ToString();
            }
        }
        public void AddRondomdgv()
        {
            var Totbd = Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtPrice.Text);
            DGV.Rows.Add(cmbproducts.SelectedValue,
             cmbproducts.Text,
             unitTypes.Text,
             unitTypes.SelectedValue,
             txtQuantity.Text,
             txtPrice.Text, 0,
             Totbd);
        }
        public void Chachdgv()
        {
            Boolean Flag = false;
            int DI;
            for (DI = 0; DI < DGV.Rows.Count; DI++)
            {
                var PC = DGV.Rows[DI].Cells[0].Value.ToString();
                var Qty = Convert.ToDecimal(DGV.Rows[DI].Cells[4].Value);
                if (PC == cmbproducts.SelectedValue.ToString())
                {
                    Flag = true;
                    break;
                }
            }
            if (Flag)
            {
                DGV.Rows[DI].Cells[4].Value = Convert.ToDecimal(DGV.Rows[DI].Cells[4].Value.ToString()) + Convert.ToDecimal(txtQuantity.Text);
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
        public void AddProducttodgv()
        {
            if (unitTypes.Text == "" || unitTypes.SelectedIndex == 0)
            {
                MessageBox.Show("برجاء ادخال وحدات لكي يمكن اضافة المنتجات", "خطأ");
                return;
            }
            if (txtQuantity.Text == "" || txtQuantity.Text == "0" || txtQuantity.Text == ".")
            {
                MessageBox.Show("ادخل الكمية", "خطأ");
                return;
            }
            if (DGV.Rows.Count > 0)
            {
                Chachdgv();
            }
            else
            {
                AddRondomdgv();
            }
            DGSum();
        }
        public void Cleardata()
        {
            txtPrice.Clear();
            txtQuantity.Clear();
        }
        public void Selectfromunit()
        {
            if (cmbproducts.SelectedValue != null)
            {
                Cleardata();
                if (cmbproducts.SelectedIndex != 0)
                {
                    var data = CF.fillunitid(int.Parse(cmbproducts.SelectedValue.ToString()));
                    if (data != null)
                    {
                        unitTypes.SelectedValue = data["Unitid"].ToString();
                        txtPrice.Text = data["UnitPrice"].ToString();
                    }
                    var GS = CF.GetStock(int.Parse(cmbproducts.SelectedValue.ToString()));
                    if (GS != null)
                    {
                        lblStockOnHand.Text = GS.ToString();
                    }
                }
                else
                {
                    Getunits();
                    Cleardata();
                }
            }
            else
            {
                Getunits();
                Cleardata();
            }
        }
        public void SaveAndEdit()
        {
            //for (int i = 0; i < DGV.Rows.Count; i++)
            //{
            //    SPay.sald.InvoiceNo = SPay.sal.Invoiceno;
            //    SPay.sald.ProductNo = int.Parse(DGV.Rows[i].Cells[0].Value.ToString());
            //    SPay.sald.TDDesc = DGV.Rows[i].Cells[1].Value.ToString();
            //    SPay.sald.Unitid = int.Parse(DGV.Rows[i].Cells[3].Value.ToString());
            //    SPay.sald.ItemPrice = int.Parse(DGV.Rows[i].Cells[5].Value.ToString());
            //    SPay.sald.Quantity = int.Parse(DGV.Rows[i].Cells[4].Value.ToString());
            //    SPay.sald.Subtotal = SPay.sald.ItemPrice * SPay.sald.Quantity;
            //    SPay.sald.Discount = int.Parse(DGV.Rows[i].Cells[6].Value.ToString());
            //    SPay.sald.Totafterdiscount = SPay.sald.Subtotal - SPay.sald.Discount;
            //    SPay.sald.Total = int.Parse(DGV.Rows[i].Cells[7].Value.ToString());
            //    SPay.sald.Savesalesdetailes();
            //}
        }
        public void Salesoprations()
        {
            //SPay.sal.Savesales();
            //SPay.sal.Getlastbyid();
            SaveAndEdit();
        }
        public void Getinvoice()
        {
            try
            {
                if (Invnum != 0)
                {
                    DGV.Rows.Clear();
                    CF.Getpriceoffer(Invnum);
                    PDL = CF.PDL;
                    foreach (var item in PDL)
                    {
                        Prc.ID = item.ID;
                        DTF.Text = item.Datefrom.ToString();
                        DTT.Text = item.Dateto.ToString();
                        clientID.Text = item.Name;
                        txtTBV.Text = item.NonVatTotal.ToString();
                        txtDiscount.Text = item.Discount.ToString();
                        txtTax.Text = item.VatAmount.ToString();
                        txtTotal.Text = item.TotalAmount.ToString();
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة عرض الاسعار" + "','" + "Getinvoice" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void Frmprice_Load(object sender, EventArgs e)
        {
            Getproducts();
            Getunits();
            Getcustomers();
            Getinvoice();
        }
        private void products_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selectfromunit();
        }
        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
            //if (!string.IsNullOrEmpty(txtDiscount.Text) && txtDiscount.Text != ".")
            //{
            //    Changedisc();
            //}
        }
        private void Btnbilllist_Click(object sender, EventArgs e)
        {
            Frmpricelist FPL = new Frmpricelist();
            FPL.Show();
        }
        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarcode.Text))
            {
                cmbproducts.SelectedValue = txtBarcode.Text;
            }
            else
            {
                Getproducts();
                Getunits();
            }
        }
        private void products_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Selectfromunit();
        }
        private void Btnadd_Click(object sender, EventArgs e)
        {
            AddProducttodgv();
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
            if (e.KeyChar == (char)13)
            {
                AddProducttodgv();
            }
        }
        private void txtTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
            //    if (!string.IsNullOrEmpty(txtTotal.Text) && txtTotal.Text != ".")
            //    {
            //    txtDiscount.Text = "0";
            //    txtTax.Text = "0";

            //    var Price = Convert.ToDouble(txtTotal.Text);
            //    var TBV = Price / 1.15;
            //    var GTBV = Math.Round(Convert.ToDecimal(TBV), 2).ToString();
            //    if (VI.ISVAT)
            //    {
            //        var Tax = Convert.ToDouble(GTBV) * (VI.VatPercent / 100);
            //        var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
            //        //var GT = Math.Round(Convert.ToDecimal(GTax), MidpointRounding.ToEven);

            //        txtTax.Text = GTax.ToString();

            //        var Disc = Convert.ToDouble(txtTBV.Text) - TBV;
            //        var GDisc = Math.Round(Convert.ToDouble(Disc), 2).ToString();
            //        txtDiscount.Text = Convert.ToString(GDisc);
            //    }
            //    else
            //    {
            //        var TB = Convert.ToDouble(txtTBV.Text);
            //        var Tot = Convert.ToDouble(txtTotal.Text);
            //        var Res = TB - Tot;
            //        txtDiscount.Text = Res.ToString();
            //    }
            //}
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
        private void Btnsave_Click(object sender, EventArgs e)
        {
            if (Btnsave.Text == "حفظ")
            {
                Savedata();
            }
            else
            {
                Editdata();
            }
        }
        private void Btnprint_Click(object sender, EventArgs e)
        {
            Printinv();
        }
        private void Btnsaveandprint_Click(object sender, EventArgs e)
        {
            if (Btnsaveandprint.Text == "حفظ وطباعة")
            {
                Savedata();
                Printinv();
            }
            else
            {
                Editdata();
                Printinv();
            }
        }
        private void Btncustomers_Click(object sender, EventArgs e)
        {
            frmListThirdParty FTP = new frmListThirdParty();
            FTP.radioClient.Checked = true;
            FTP.Show();
        }
        public void Savedata()
        {
            try
            {
                if (clientID.SelectedIndex == 0)
                {
                    MessageBox.Show("برجاء ادخال العميل", "خطأ");
                    return;
                }
                else if (DGV.Rows.Count == 0)
                {
                    MessageBox.Show("لا يوجد طلبات", "خطأ");
                    return;
                }
                else
                {
                    Prc.Datefrom = DTF.Value.ToString("yyyy-MM-dd");
                    Prc.Dateto = DTT.Value.ToString("yyyy-MM-dd");
                    Prc.NonVatTotal = double.Parse(txtTBV.Text);
                    Prc.Discount = double.Parse(txtDiscount.Text);
                    Prc.VatAmount = double.Parse(txtTax.Text);
                    Prc.TotalAmount = double.Parse(txtTotal.Text);
                    Prc.ThirdPartyID = int.Parse(clientID.SelectedValue.ToString());
                    Prc.Savesales();
                    Prc.Getlastbyid();
                    for (int i = 0; i < DGV.Rows.Count; i++)
                    {
                        Pd.Priceid = Prc.ID;
                        Pd.ProductNo = int.Parse(DGV.Rows[i].Cells[0].Value.ToString());
                        Pd.TDDesc = DGV.Rows[i].Cells[1].Value.ToString();
                        Pd.Unitid = int.Parse(DGV.Rows[i].Cells[3].Value.ToString());
                        Pd.ItemPrice = double.Parse(DGV.Rows[i].Cells[5].Value.ToString());
                        Pd.Quantity = int.Parse(DGV.Rows[i].Cells[4].Value.ToString());
                        Pd.Subtotal = Pd.ItemPrice * Pd.Quantity;
                        Pd.Discount = double.Parse(DGV.Rows[i].Cells[6].Value.ToString());
                        Pd.Totafterdiscount = Pd.Subtotal - Pd.Discount;
                        Pd.Total = decimal.Parse(DGV.Rows[i].Cells[7].Value.ToString());
                        Pd.Savepricedetailes();
                    }
                    MessageBox.Show("تم الحفظ بنجاح", "تم");
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة عرض الاسعار" + "','" + "Savedata" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Editdata()
        {
            try
            {
                if (clientID.SelectedIndex == 0)
                {
                    MessageBox.Show("برجاء ادخال العميل", "خطأ");
                    return;
                }
                else if (DGV.Rows.Count == 0)
                {
                    MessageBox.Show("لا يوجد طلبات", "خطأ");
                    return;
                }
                else
                {
                    Prc.Datefrom = DTF.Value.ToString("yyyy-MM-dd");
                    Prc.Dateto = DTT.Value.ToString("yyyy-MM-dd");
                    Prc.NonVatTotal = double.Parse(txtTBV.Text);
                    Prc.Discount = double.Parse(txtDiscount.Text);
                    Prc.VatAmount = double.Parse(txtTax.Text);
                    Prc.TotalAmount = double.Parse(txtTotal.Text);
                    Prc.ThirdPartyID = int.Parse(clientID.SelectedValue.ToString());
                    Prc.Editsales(Invnum);
                    Prc.Deletesalesdetailes(Invnum);
                    for (int i = 0; i < DGV.Rows.Count; i++)
                    {
                        Pd.Priceid = Invnum;
                        Pd.ProductNo = int.Parse(DGV.Rows[i].Cells[0].Value.ToString());
                        Pd.TDDesc = DGV.Rows[i].Cells[1].Value.ToString();
                        Pd.Unitid = int.Parse(DGV.Rows[i].Cells[3].Value.ToString());
                        Pd.ItemPrice = double.Parse(DGV.Rows[i].Cells[5].Value.ToString());
                        Pd.Quantity = int.Parse(DGV.Rows[i].Cells[4].Value.ToString());
                        Pd.Subtotal = Pd.ItemPrice * Pd.Quantity;
                        Pd.Discount = double.Parse(DGV.Rows[i].Cells[6].Value.ToString());
                        Pd.Totafterdiscount = Pd.Subtotal - Pd.Discount;
                        Pd.Total = decimal.Parse(DGV.Rows[i].Cells[7].Value.ToString());
                        Pd.Savepricedetailes();
                    }
                    MessageBox.Show("تم التعديل بنجاح", "تم");
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة عرض الاسعار" + "','" + "Editdata" + "','" + ex.Message.ToString() + "')";
                Da = new DataAccess();
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void Btnchange_Click(object sender, EventArgs e)
        {
            sal.TDate = DateTime.Now.ToString("yyyy-MM-dd");
            sal.TTime = DateTime.Now.ToString("HH:MM:ss");
            sal.NonVatTotal = double.Parse(txtTBV.Text);
            sal.Discount = double.Parse(txtDiscount.Text);
            sal.VatAmount = double.Parse(txtTax.Text);
            sal.TotalAmount = double.Parse(txtTotal.Text);
            sal.ThirdPartyID = int.Parse(clientID.SelectedValue.ToString());
            sal.Billtype = "مسودة";
            sal.Savesales();
            sal.Getlastbyid();
            for (int i = 0; i < DGV.Rows.Count; i++)
            {
                Sd.InvoiceNo = sal.Invoiceno;
                Sd.ProductNo = int.Parse(DGV.Rows[i].Cells[0].Value.ToString());
                Sd.TDDesc = DGV.Rows[i].Cells[1].Value.ToString();
                Sd.Unitid = int.Parse(DGV.Rows[i].Cells[3].Value.ToString());
                Sd.ItemPrice = decimal.Parse(DGV.Rows[i].Cells[5].Value.ToString());
                Sd.Quantity = int.Parse(DGV.Rows[i].Cells[4].Value.ToString());
                Sd.Subtotal = decimal.Parse(Pd.ItemPrice.ToString()) * Pd.Quantity;
                Sd.Discount = decimal.Parse(DGV.Rows[i].Cells[6].Value.ToString());
                Sd.Totafterdiscount = decimal.Parse(Pd.Subtotal.ToString()) - decimal.Parse(Pd.Discount.ToString());
                Sd.Total = decimal.Parse(DGV.Rows[i].Cells[7].Value.ToString());
                Sd.Savesalesdetailes();
            }
            MessageBox.Show("تم التحويل بنجاح", "تم");
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.CurrentCell.ColumnIndex == 4 || DGV.CurrentCell.ColumnIndex == 5)
            {
                var C1 = Convert.ToDouble(DGV.CurrentRow.Cells[4].Value.ToString());
                var C2 = Convert.ToDouble(DGV.CurrentRow.Cells[5].Value);
                var Res = C1 * C2;
                DGV.CurrentRow.Cells[7].Value = Res;
                DGSum();
            }
            if (DGV.CurrentCell.ColumnIndex == 7)
            {
                var C1 = Convert.ToDouble(DGV.CurrentRow.Cells[4].Value.ToString());
                var C2 = Convert.ToDouble(DGV.CurrentRow.Cells[5].Value);
                var Res = C1 * C2;
                DGV.CurrentRow.Cells[7].Value = Res;
                DGSum();
            }
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            Totflag = true;
            var Price = Convert.ToDouble(txtTotal.Text);
            var TBV = Price / 1.15;
            var GTBV = Math.Round(Convert.ToDecimal(TBV), 2).ToString();
            if (VI.ISVAT)
            {
                var Tax = Convert.ToDouble(GTBV) * (VI.VatPercent / 100);
                var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
                //var GT = Math.Round(Convert.ToDecimal(GTax), MidpointRounding.ToEven);

                txtTax.Text = GTax.ToString();

                var Disc = Convert.ToDouble(txtTBV.Text) - TBV;
                var GDisc = Math.Round(Convert.ToDouble(Disc), 2).ToString();
                txtDiscount.Text = Convert.ToString(GDisc);
            }
            else
            {
                var TB = Convert.ToDouble(txtTBV.Text);
                var Tot = Convert.ToDouble(txtTotal.Text);
                var Res = TB - Tot;
                txtDiscount.Text = Res.ToString();
            }
            Totflag = false;
        }
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if (!Totflag)
            {
                var ST = Convert.ToDouble(txtTBV.Text) - Convert.ToDouble(txtDiscount.Text);
                if (VI.ISVAT)
                {
                    var Tax = Convert.ToDouble(ST) * (VI.VatPercent / 100);
                    var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
                    txtTax.Text = GTax.ToString();
                    var Price = ST + Tax;
                    var GPrice = Math.Round(Convert.ToDouble(Price), 2).ToString();
                    txtTotal.Text = GPrice.ToString();
                }
                else
                {
                    txtTotal.Text = ST.ToString();
                }
            }
        }
    }
}
