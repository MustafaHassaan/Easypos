using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Querylaeyr.Accountant;
using Querylaeyr.Modelmaster;
using Querylaeyr.Modelsales;
using Querylaeyr.Pricingmodels;
using Querylaeyr.Tailor;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using Zatca;
using static System.Net.Mime.MediaTypeNames;

namespace Centeralized
{
    public class Centergetingfunctions
    {
        MySqlConnection Con;
        MySqlCommand Cmd;
        MySqlDataReader dr;
        DataAccess DA;
        Sales SH;
        public Product Pro;
        public List<Saleslist> SL;
        public List<Accountanat> AC;
        public List<Category> CL;
        public List<Pricelist> PDL;
        public List<Salesonetax> SOT;
        TLV tlv;
        public CompanyInfo Comp;
        public string Untname { get; set; }
        public decimal Balance { get; set; }
        public void Usenumber(KeyPressEventArgs e, object sender)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        public DataTable Productfilldata()
        {
            try
            {
                var Qur = @"SELECT ProductNo, 
                                   Description FROM product ";
                DA = new DataAccess();
                var GD = DA.Getdatatable(Qur);
                if (GD != null)
                {
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Product fill Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Productitemsfilldata()
        {
            try
            {
                var Qur = @"SELECT ID, 
                                   Itemname FROM Items ";
                DA = new DataAccess();
                var GD = DA.Getdatatable(Qur);
                if (GD != null)
                {
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Product items fill Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public MySqlDataReader fillitemsunitid(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select UID,Itemprice From Items Where ID = " + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        return GD;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Product fill Data unit" + "','" + ex.Message.ToString() + "')";
                    DA = new DataAccess();
                    var CMO = DA.Crudopration(Qurcat);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public DataTable Unitfilldata()
        {
            try
            {
                var Qur = @"SELECT ID, 
                                   UnitType From unittypes ";
                DA = new DataAccess();
                var GD = DA.Getdatatable(Qur);
                if (GD != null)
                {
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Unit fill Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Catfilldata()
        {
            try
            {
                var Qur = @"SELECT CategoryNo, 
                                   CategoryName From Category";
                DA = new DataAccess();
                var GD = DA.Getdatatable(Qur);
                if (GD != null)
                {
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Unit fill Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public MySqlDataReader fillunitid(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select Unitid,UnitPrice From Product Where ProductNo = " + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        return GD;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Unit fill Data" + "','" + ex.Message.ToString() + "')";
                    DA = new DataAccess();
                    var CMO = DA.Crudopration(Qurcat);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public string GetStock(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select StocksOnHand From stock Where Proid = " + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        var SOH = GD["StocksOnHand"].ToString();
                        return SOH;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Stock" + "','" + ex.Message.ToString() + "')";
                    DA = new DataAccess();
                    var CMO = DA.Crudopration(Qurcat);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public string GetStockitems(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select Itemqty From items Where ID = " + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        var SOH = GD["StocksOnHand"].ToString();
                        return SOH;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Stock items Data" + "','" + ex.Message.ToString() + "')";
                    DA = new DataAccess();
                    var CMO = DA.Crudopration(Qurcat);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public DataTable Getthirdparties()
        {
            Thirdparty Third = new Thirdparty();
            var DT = Third.Getdatabycustomer();
            return DT;
        }
        public DataTable Getemp()
        {
            empcreator EC = new empcreator();
            var DT = EC.Getdata();
            return DT;
        }
        public DataTable GetSuppliers()
        {
            Thirdparty Third = new Thirdparty();
            var DT = Third.Getdatabysuppliers();
            return DT;
        }
        public void Savesales()
        {
            SH = new Sales();
            SH.Savesales();
        }
        public MySqlDataReader Getsaleslist(int Proid)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                SL = new List<Saleslist>();
                var Qur = @"SELECT
                            Sales.Invoiceno,
                            Sales.TDate,
                            Sales.TTime,
                            Sales.NonVatTotal,
                            Sales.Discount,
                            Sales.VatAmount,
                            Sales.TotalAmount,
                            Sales.Billtype,
                            thirdparty.Name,
                            thirdparty.Taxnumber,
                            thirdparty.Address,
                            thirdparty.MobileNumber,
                            salesdetailes.ProductNo,
                            salesdetailes.TDDesc,
                            salesdetailes.Unitid,
                            unittypes.UnitType,
                            salesdetailes.ItemPrice,
                            salesdetailes.Quantity,
                            salesdetailes.Subtotal,
                            salesdetailes.Discount As 'Discountdet',
                            salesdetailes.Totafterdiscount,
                            salesdetailes.Total,
                            payment.PaymentMethod,
                                                        payment.Bank + payment.Cash As 'Paid',
                            Sales.TotalAmount - (payment.Bank + payment.Cash) As 'Remining',
                            payment.Type,
                            contractors.Purchaseorder,
                            contractors.Refrancenumber,
                            contractors.Projectname,
                            Sales.Note,
                            tailor.Date,
                            tailor.Receiptdate
                            FROM Sales
                            Left Outer Join thirdparty
                            On Sales.ThirdPartyID = thirdparty.ID
                            Left Outer Join salesdetailes
                            On salesdetailes.InvoiceNo = Sales.Invoiceno
                            Left Outer Join unittypes
                            On unittypes.ID = salesdetailes.Unitid
							Left Outer Join Payment
                            On Payment.InvoiceNo = sales.InvoiceNo
                            Left Outer Join transactions
                            On transactions.InvoiceNo = sales.InvoiceNo
                            Left Outer Join Contractors
                            On Contractors.InvoiceNo = sales.InvoiceNo
                            Left Outer Join tailor
                            on tailor.invoiceid = sales.Invoiceno
                            Where Sales.InvoiceNo = " + Proid + " "+
                           @"group by
                            Sales.Invoiceno,
                            Sales.TDate,
                            Sales.TTime,
                            Sales.NonVatTotal,
                            Sales.Discount,
                            Sales.VatAmount,
                            Sales.TotalAmount,
                            Sales.Billtype,
                            thirdparty.Name,
                            thirdparty.Taxnumber,
                            thirdparty.Address,
                            thirdparty.MobileNumber,
                            salesdetailes.ProductNo,
                            salesdetailes.TDDesc,
                            salesdetailes.Unitid,
                            unittypes.UnitType,
                            salesdetailes.ItemPrice,
                            salesdetailes.Quantity,
                            salesdetailes.Subtotal,
                            salesdetailes.Discount,
                            salesdetailes.Totafterdiscount,
                            salesdetailes.Total,
                            payment.cash,
                            payment.Bank,
                            payment.PaymentMethod,
                            payment.Type,
                            contractors.Purchaseorder,
                            contractors.Refrancenumber,
                            contractors.Projectname,
                            Sales.Note,                        
                            tailor.Date,
                            tailor.Receiptdate";
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    var Monypayed = GD["Paid"].ToString();
                    var Monremining = GD["Remining"].ToString();
                    if (Monypayed == "")
                    {
                        Monypayed = "0";
                    }
                    if (Monremining == "")
                    {
                        Monremining = "0";
                    }
                    SL.Add(new Saleslist()
                    {
                        Invoiceno = int.Parse(GD["Invoiceno"].ToString()),
                        TDate = GD["TDate"].ToString(),
                        TTime = GD["TTime"].ToString(),
                        NonVatTotal = double.Parse(GD["NonVatTotal"].ToString()),
                        Discount = double.Parse(GD["Discount"].ToString()),
                        VatAmount = double.Parse(GD["VatAmount"].ToString()),
                        TotalAmount = double.Parse(GD["TotalAmount"].ToString()),
                        Name = GD["Name"].ToString(),
                        Billtype = GD["Billtype"].ToString(),
                        Taxnumber = GD["Taxnumber"].ToString(),
                        MobileNumber = GD["MobileNumber"].ToString(),
                        Address = GD["Address"].ToString(),
                        ProductNo = int.Parse(GD["ProductNo"].ToString()),
                        TDDesc = GD["TDDesc"].ToString(),
                        UnitType = GD["UnitType"].ToString(),
                        Unitid = int.Parse(GD["Unitid"].ToString()),
                        ItemPrice = double.Parse(GD["ItemPrice"].ToString()),
                        Quantity = double.Parse(GD["Quantity"].ToString()),
                        Subtotal = double.Parse(GD["Subtotal"].ToString()),
                        Discountdet = double.Parse(GD["Discountdet"].ToString()),
                        Totafterdiscount = double.Parse(GD["Totafterdiscount"].ToString()),
                        Total = double.Parse(GD["Total"].ToString()),
                        Paid = double.Parse(Monypayed),
                        PaymentMethod = GD["PaymentMethod"].ToString(),
                        Purchaseorder = GD["Purchaseorder"].ToString(),
                        Refrancenumber = GD["Refrancenumber"].ToString(),
                        Projectname = GD["Projectname"].ToString(),
                        Type = GD["Type"].ToString(),
                        Remaining = double.Parse(Monremining),
                        Note = GD["Note"].ToString(),
                        Tailordate = GD["Date"].ToString(),
                        TailorReceiptdate = GD["Receiptdate"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Sales List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
            return dr;
        }
        public MySqlDataReader Getpriceoffer(int Proid)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                PDL = new List<Pricelist>();
                var Qur = @"SELECT
                            Price.ID,
                            Price.Datefrom,
                            Price.Dateto,
                            Price.NonVatTotal,
                            Price.Discount,
                            Price.VatAmount,
                            Price.TotalAmount,
                            thirdparty.Name,
                            thirdparty.Taxnumber,
                            thirdparty.Address,
                            thirdparty.MobileNumber,
                            pricedetailes.Priceid,
                            pricedetailes.ProductNo,
                            pricedetailes.TDDesc,
                            pricedetailes.Unitid,
                            unittypes.UnitType,
                            pricedetailes.ItemPrice,
                            pricedetailes.Quantity,
                            pricedetailes.Subtotal,
                            pricedetailes.Discount As 'Discountdet',
                            pricedetailes.Totafterdiscount,
                            pricedetailes.Total
                            FROM Price
                            Left Outer Join thirdparty
                            On Price.ThirdPartyID = thirdparty.ID
                            Left Outer Join pricedetailes
                            On pricedetailes.Priceid = Price.ID
                            Left Outer Join unittypes
                            On unittypes.ID = pricedetailes.Unitid
                            Where Price.ID =" + Proid;
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    PDL.Add(new Pricelist()
                    {
                        ID = int.Parse(GD["ID"].ToString()),
                        Priceid = int.Parse(GD["Priceid"].ToString()),
                        Datefrom = GD["Datefrom"].ToString(),
                        Dateto = GD["Dateto"].ToString(),
                        NonVatTotal = double.Parse(GD["NonVatTotal"].ToString()),
                        Discount = double.Parse(GD["Discount"].ToString()),
                        VatAmount = double.Parse(GD["VatAmount"].ToString()),
                        TotalAmount = double.Parse(GD["TotalAmount"].ToString()),
                        Name = GD["Name"].ToString(),
                        Taxnumber = GD["Taxnumber"].ToString(),
                        MobileNumber = GD["MobileNumber"].ToString(),
                        Address = GD["Address"].ToString(),
                        ProductNo = int.Parse(GD["ProductNo"].ToString()),
                        TDDesc = GD["TDDesc"].ToString(),
                        UnitType = GD["UnitType"].ToString(),
                        Unitid = int.Parse(GD["Unitid"].ToString()),
                        ItemPrice = double.Parse(GD["ItemPrice"].ToString()),
                        Quantity = int.Parse(GD["Quantity"].ToString()),
                        Subtotal = double.Parse(GD["Subtotal"].ToString()),
                        Discountdet = double.Parse(GD["Discountdet"].ToString()),
                        Totafterdiscount = double.Parse(GD["Totafterdiscount"].ToString()),
                        Total = decimal.Parse(GD["Total"].ToString()),
                    });
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Price offer" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
            return dr;
        }
        public MySqlDataReader Getpurchaselist(int Proid)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                SL = new List<Saleslist>();
                var Qur = @"SELECT
                            Purchases.Invoiceno,
                            Purchases.TDate,
                            Purchases.TTime,
                            Purchases.NonVatTotal,
                            Purchases.Discount,
                            Purchases.VatAmount,
                            Purchases.TotalAmount,
                            Purchases.Billtype,
                            thirdparty.Name,
                            thirdparty.Taxnumber,
                            thirdparty.Address,
                            thirdparty.MobileNumber,
                            Purchasedetailes.ProductNo,
                            Purchasedetailes.TDDesc,
                            Purchasedetailes.Unitid,
                            unittypes.UnitType,
                            Purchasedetailes.ItemPrice,
                            Purchasedetailes.Quantity,
                            Purchasedetailes.Subtotal,
                            Purchasedetailes.Discount As 'Discountdet',
                            Purchasedetailes.Totafterdiscount,
                            Purchasedetailes.Total,
                            paymentout.paymentMethod,
                            paymentout.Paid,
                            paymentout.Type,
                            Purchases.Supplierbill
                            FROM Purchases
                            Left Outer Join thirdparty
                            On Purchases.ThirdPartyID = thirdparty.ID
                            Left Outer Join Purchasedetailes
                            On Purchasedetailes.InvoiceNo = Purchases.Invoiceno
                            Left Outer Join unittypes
                            On unittypes.ID = Purchasedetailes.Unitid
							Left Outer Join paymentout
                            On paymentout.InvoiceNo = Purchases.InvoiceNo
                            Where Purchases.InvoiceNo = " + Proid;
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    SL.Add(new Saleslist()
                    {
                        Invoiceno = int.Parse(GD["Invoiceno"].ToString()),
                        TDate = GD["TDate"].ToString(),
                        TTime = GD["TTime"].ToString(),
                        NonVatTotal = double.Parse(GD["NonVatTotal"].ToString()),
                        Discount = double.Parse(GD["Discount"].ToString()),
                        VatAmount = double.Parse(GD["VatAmount"].ToString()),
                        TotalAmount = double.Parse(GD["TotalAmount"].ToString()),
                        Name = GD["Name"].ToString(),
                        Billtype = GD["Billtype"].ToString(),
                        Taxnumber = GD["Taxnumber"].ToString(),
                        MobileNumber = GD["MobileNumber"].ToString(),
                        Address = GD["Address"].ToString(),
                        ProductNo = int.Parse(GD["ProductNo"].ToString()),
                        TDDesc = GD["TDDesc"].ToString(),
                        UnitType = GD["UnitType"].ToString(),
                        Unitid = int.Parse(GD["Unitid"].ToString()),
                        ItemPrice = double.Parse(GD["ItemPrice"].ToString()),
                        Quantity = int.Parse(GD["Quantity"].ToString()),
                        Subtotal = double.Parse(GD["Subtotal"].ToString()),
                        Discountdet = double.Parse(GD["Discountdet"].ToString()),
                        Totafterdiscount = double.Parse(GD["Totafterdiscount"].ToString()),
                        Total = double.Parse(GD["Total"].ToString()),
                        Paid = double.Parse(GD["Paid"].ToString()),
                        PaymentMethod = GD["PaymentMethod"].ToString(),
                        Type = GD["Type"].ToString(),
                        Supplierbill = GD["Supplierbill"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Purchases list" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
            return dr;
        }
        public bool Deletebyid(string Qur)
        {
            DA = new DataAccess();
            var CMO = DA.Crudopration(Qur);
            if (CMO)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Bitmap DQ(double tot, double tax, DateTime DT)
        {
            Comp = new CompanyInfo();
            Comp.GetCompany();
            String Seller = Comp.CName;
            String VatNo = Comp.Taxnumber;
            DateTime dTime = DT;
            double Total = tot;
            double Tax = tax;
            tlv = new TLV(Seller, VatNo, dTime, Total, Tax);
            var Qrimage = tlv.toQrCode();
            return Qrimage;
        }
        public byte[] CTB(System.Drawing.Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        public MySqlDataReader Getcatlist()
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                CL = new List<Category>();
                var Qur = @"Select * From Category";
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    CL.Add(new Category()
                    {
                        CategoryNo = int.Parse(GD["CategoryNo"].ToString()),
                        CategoryName = GD["CategoryName"].ToString(),
                        Color = int.Parse(GD["Color"].ToString()),
                    });
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Cat List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
            return dr;
        }
        public string GetArabicProductName(string Description)
        {
            string ArabicProduct = "";
            var SplitText = Description.Split(' ', '\t');
            var Line1 = "";
            var Line2 = "";
            var desc = "";
            foreach (var item in SplitText)
            {

                if (item.Length > 0 && item != "" && IsAllCharEnglish(item) == false)
                {
                    ArabicProduct += item + " ";

                }



            }
            return ArabicProduct.TrimEnd().TrimStart();
        }
        private bool IsAllCharEnglish(string Input)
        {
            foreach (var item in Input.ToCharArray())
            {
                if (!char.IsLower(item) && !char.IsUpper(item) && !char.IsDigit(item) && !char.IsWhiteSpace(item))
                {
                    return false;
                }
            }
            return true;
        }
        public DataTable Getproductbyid(int CateNumber)
        {
            try
            {
                var Qur = @"Select * From Product Where CategoryNo= " + CateNumber + " order by Product.order";
                DA = new DataAccess();
                var GD = DA.Getdatatable(Qur);
                if (GD != null)
                {
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Product By Id" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public MySqlDataReader Getdatabyid(int Proid)
        {
            var Qur = @"Select 
                        product.ProductNo,
                        product.ProductCode,
                        product.Description,
                        product.Unitid,
                        unittypes.UnitType,
                        product.UnitPrice,
                        product.StocksOnHand
                        From product
                        Left Outer Join unittypes
                        On product.Unitid = unittypes.ID
                        Where product.ProductNo = " + Proid;

            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                Pro = new Product();
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    Pro.ProductNo = int.Parse(GD["ProductNo"].ToString());
                    Pro.ProductCode = GD["ProductCode"].ToString();
                    Pro.Description = GD["Description"].ToString();
                    Pro.Unitid = int.Parse(GD["Unitid"].ToString());
                    Pro.UnitPrice = double.Parse(GD["UnitPrice"].ToString());
                    Untname = GD["UnitType"].ToString();
                    Pro.StocksOnHand = int.Parse(GD["StocksOnHand"].ToString());
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Data By ID" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);

                var Messagedata = ex.Message.ToString();
                return null;
            }
            return dr;
        }
        public MySqlDataReader Getdataitemsbyid(int Proid)
        {
            var Qur = @"Select 
                        items.ID,
                        items.Itemqty
                        From items 
                        Where items.ID = " + Proid;

            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                Pro = new Product();
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    Pro.ProductCode = GD["ID"].ToString();
                    Pro.StocksOnHand = int.Parse(GD["Itemqty"].ToString());
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Data Items By ID" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
            return dr;
        }
        public MySqlDataReader GetAccountantlist(int Thirdid,string DF, string DT)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                AC = new List<Accountanat>();
                //var Qur = @"Select 
                //            payment.paymentNo,
                //            payment.InvoiceNo,
                //            payment.Date,
                //            sales.TotalAmount,
                //            payment.Paid,
                //            payment.Remaining,
                //            payment.ThirdPartyID,
                //            payment.Type,
                //            thirdparty.Name,
                //            thirdparty.MobileNumber,
                //            thirdparty.Address,
                //            thirdparty.Taxnumber,
                //            sales.Billtype
                //            From payment
                //            Left outer Join Sales
                //            on payment.Invoiceno = Sales.Invoiceno
                //            Left Outer Join ThirdParty
                //            on payment.ThirdPartyID = ThirdParty.ID
                //            Where payment.ThirdPartyID = " + Thirdid + " " +
                //            "And payment.Date >= '" + DF +"' And payment.Date <= '" + DT +"' ";



                var Qur = @"Select 
                            thirdparty.Name,
                            thirdparty.MobileNumber,
                            thirdparty.Address,
                            thirdparty.Taxnumber,
                            transactions.Type,
							transactions.Paynum,
                            transactions.InvoiceNo,
                            transactions.TDate,
                            transactions.ThirdPartyID,
                            sales.TotalAmount,
                            transactions.Paid,
                            payment.Remaining
                            From transactions
                            Left Outer Join ThirdParty
                            on transactions.ThirdPartyID = ThirdParty.ID
                            Left Outer Join payment
                            on transactions.Paynum = payment.paymentNo
                            Left Outer Join Sales
                            on sales.Invoiceno = transactions.Invoiceno
                            Where transactions.ThirdPartyID = " + Thirdid + " " +
                           "And transactions.TDate >= '" + DF + "' And transactions.TDate <= '" + DT + "' ";
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    decimal Totamount = 0;
                    var Totam = GD["TotalAmount"].ToString();
                    if (Totam == "")
                    {
                        Totamount = 0;
                    }
                    else
                    {
                        Totamount = decimal.Parse(GD["TotalAmount"].ToString());
                    }
                    AC.Add(new Accountanat()
                    {
                        ID = int.Parse(GD["Paynum"].ToString()),
                        Invoiceno = int.Parse(GD["InvoiceNo"].ToString()),
                        Type = GD["Type"].ToString(),
                        TDate = GD["TDate"].ToString(),
                        ThirdPartyID = int.Parse(GD["ThirdPartyID"].ToString()),
                        Name = GD["Name"].ToString(),
                        MobileNumber = GD["MobileNumber"].ToString(),
                        Address = GD["Address"].ToString(),
                        Taxnumber = GD["Taxnumber"].ToString(),
                        TotalAmount = Totamount,
                        Paid = decimal.Parse(GD["Paid"].ToString()),
                        Remaining = decimal.Parse(GD["Remaining"].ToString()),
                        Billtype = GD["Type"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Account List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
            return dr;
        }
        public MySqlDataReader GetAccountantpayoutlist(int Thirdid, string DF, string DT)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                AC = new List<Accountanat>();
                var Qur = @"Select 
                            paymentout.paymentNo,
                            paymentout.InvoiceNo,
                            paymentout.Date,
                            purchases.TotalAmount,
                            paymentout.Paid,
                            paymentout.Remaining,
                            paymentout.ThirdPartyID,
                            paymentout.Type,
                            thirdparty.Name,
                            thirdparty.MobileNumber,
                            thirdparty.Address,
                            thirdparty.Taxnumber,
                            purchases.Billtype
                            From paymentout
                            Left outer Join purchases
                            on paymentout.Invoiceno = purchases.Invoiceno
                            Left Outer Join ThirdParty
                            on paymentout.ThirdPartyID = ThirdParty.ID
                            Where paymentout.ThirdPartyID = " + Thirdid + " " +
                            "And paymentout.Date >= '" + DF + "' And paymentout.Date <= '" + DT + "' ";
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    AC.Add(new Accountanat()
                    {
                        ID = int.Parse(GD["paymentNo"].ToString()),
                        Invoiceno = int.Parse(GD["InvoiceNo"].ToString()),
                        Type = GD["Type"].ToString(),
                        TDate = GD["Date"].ToString(),
                        ThirdPartyID = int.Parse(GD["ThirdPartyID"].ToString()),
                        Name = GD["Name"].ToString(),
                        MobileNumber = GD["MobileNumber"].ToString(),
                        Address = GD["Address"].ToString(),
                        Taxnumber = GD["Taxnumber"].ToString(),
                        Paid = decimal.Parse(GD["Paid"].ToString()),
                        TotalAmount = decimal.Parse(GD["TotalAmount"].ToString()),
                        Billtype = GD["Billtype"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Account Payout List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
            return dr;
        }
        public decimal GetBalance(int Thirdid, string DF)
        {
            if (Thirdid != null)
            {
                try
                {
                    var Qur = @"Select 
                                Sum(payment.Remaining) -
                                Sum(payment.Paid) As 'Totalfinancial'
                                From payment
                                where payment.ThirdPartyID = "+ Thirdid +
                                " And payment.Date < '"+ DF +"' ";
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        if (Thirdid != 10)
                        {
                            Balance = decimal.Parse(GD["Totalfinancial"].ToString());
                        }
                        else
                        {
                            Balance = 0;
                        }
                        return Balance;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Centralized" + "','" + "Get Balance" + "','" + ex.Message.ToString() + "')";
                    DA = new DataAccess();
                    var CMO = DA.Crudopration(Qurcat);
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public void Onetax(string DTF, string DTT)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            //OTI : One Tax Invoice As Table Name
            DataTable DT = new DataTable("OTI");
            DT.Columns.Add("Total");
            DT.Columns.Add("Tax");
            DT.Columns.Add("Desc");
            var Salesqur = @"SELECT 
                             Sum(NonVatTotal) - Sum(Discount) As Total,
                             Sum(VatAmount) As Vat
                             FROM Sales
                             Where VatAmount is not null 
                             And VatAmount != '0.00'
                             And VatAmount != '0' ";
            if (!string.IsNullOrEmpty(DTF) && !string.IsNullOrEmpty(DTT))
            {
                Salesqur += "And Sales.TDate >='" + DTF + "' And Sales.TDate <='" + DTT + "' ";
            }
            Con.Open();
            Cmd = new MySqlCommand(Salesqur, Con);
            dr = Cmd.ExecuteReader();
            var GD = dr;
            if (GD.Read())
            {
                DataRow DTRa = DT.NewRow();
                DTRa["Total"] = GD["Total"].ToString();
                DTRa["Tax"] = GD["Vat"].ToString();
                if (string.IsNullOrEmpty(DTF) && string.IsNullOrEmpty(DTT))
                {
                    DTRa["Desc"] = "أجمالي المبيعات في الفترة";
                }
                else
                {
                    DTRa["Desc"] = "أجمالي المبيعات في الفترة" + "\n" + "من " + DTF.ToString() + "\n" + "الى " + DTT.ToString();
                }
                DT.Rows.Add(DTRa);
            }
            Con.Close();


            var Expensesqur = @"Select
                                Sum(Amount) As Total,
                                Sum(vat) As vat
                                From Expenses
                                Where Vat is not null 
                                And Vat != '0.00'
                                And Vat != '0' ";
            if (!string.IsNullOrEmpty(DTF) && !string.IsNullOrEmpty(DTT))
            {
                Expensesqur += "And Expenses.CDate >='" + DTF + "' And Expenses.CDate <='" + DTT + "' ";
            }
            Con.Open();
            Cmd = new MySqlCommand(Expensesqur, Con);
            dr = Cmd.ExecuteReader();
            var GDa = dr;
            if (GDa.Read())
            {
                DataRow DTRb = DT.NewRow();
                DTRb["Total"] = GDa["Total"].ToString();
                DTRb["Tax"] = GDa["vat"].ToString();
                if (string.IsNullOrEmpty(DTF) && string.IsNullOrEmpty(DTT))
                {
                    DTRb["Desc"] = "أجمالي المصروفات في الفترة";
                }
                else
                {
                    DTRb["Desc"] = "أجمالي المصروفات في الفترة" + "\n" + "من " + DTF.ToString() + "\n" + "الى " + DTT.ToString();
                }
                DT.Rows.Add(DTRb);
            }
            Con.Close();


            var Purchasesqur = @"Select
                                Sum(NonVatTotal) As Total,
                                Sum(VatAmount) As vat
                                From purchases
                                Where VatAmount is not null 
                                And VatAmount != '0.00'
                                And VatAmount != '0' ";
            if (!string.IsNullOrEmpty(DTF) && !string.IsNullOrEmpty(DTT))
            {
                Purchasesqur += "And Cast(TDate As date) >='" + DTF + "' And Cast(TDate As date) <='" + DTT + "' ";
            }
            Con.Open();
            Cmd = new MySqlCommand(Purchasesqur, Con);
            dr = Cmd.ExecuteReader();
            var GDb = dr;
            if (GDb.Read())
            {
                DataRow DTRc = DT.NewRow();
                DTRc["Total"] = GDb["Total"].ToString();
                DTRc["Tax"] = GDb["Vat"].ToString();
                if (string.IsNullOrEmpty(DTF) && string.IsNullOrEmpty(DTT))
                {
                    DTRc["Desc"] = "أجمالي المشتريات في الفترة";
                }
                else
                {
                    DTRc["Desc"] = "أجمالي المشتريات في الفترة" + "\n" + "من " + DTF.ToString() + "\n" + "الى " + DTT.ToString();
                }
                DT.Rows.Add(DTRc);
            }
            Con.Close();
            SOT = new List<Salesonetax>();
            double Restot = 0, Restax = 0;
            foreach (DataRow item in DT.Rows)
            {
                string Tot = item["Total"].ToString();
                string Tax = item["Tax"].ToString();
                if (string.IsNullOrEmpty(Tot))
                {
                    Tot = Convert.ToString(0);

                }
                if (string.IsNullOrEmpty(Tax))
                {
                    Tax = Convert.ToString(0);

                }
                string Desc = item["Desc"].ToString();
                if (Restot == 0)
                {
                    Restot = double.Parse(Tot);
                }
                else
                {
                    Restot -= double.Parse(Tot);
                }
                if (Restax == 0)
                {
                    Restax = double.Parse(Tax);
                }
                else
                {
                    Restax -= double.Parse(Tax);
                }
                if (Tot == "")
                {
                    Tot = "0";
                }
                if (Tax == "")
                {
                    Tax = "0";
                }
                SOT.Add(new Salesonetax
                {
                    Tot = Tot,Tax = Tax,Desc = Desc, Totaltax = Restax.ToString(), Totaltot= Restot.ToString()
                });
            }
        }
    }
}
