using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using Querylaeyr.Modelmaster;
using Querylaeyr.Modelsales;
using Querylaeyr.Paymodel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centeralized
{
    public class AllSalesreports
    {
        public CompanyInfo CI;
        MySqlConnection Con;
        MySqlCommand Cmd;
        MySqlDataReader dr;
        DataAccess DA;
        public List<Saleslist> SL;
        public List<Expense> Exp;
        public List<Paymentmodel> Payment;

        public AllSalesreports()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
        }
        public List<Saleslist> Getsaleslist(string DF, string DT)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                SL = new List<Saleslist>();
                var Qur = @"SELECT
                            Sales.Invoiceno,
                            thirdparty.Name,
                            Sales.TDate,
                            payment.PaymentMethod,
                            Sales.NonVatTotal,
                            Sales.Discount,
                            Sales.VatAmount,
                            Sales.TotalAmount,
                            Sum(payment.Paid) As 'Paid',
                            Sales.TotalAmount - Sum(payment.Paid) As 'Remaining'
                            FROM Sales
                            Left Outer Join thirdparty
                            On Sales.ThirdPartyID = thirdparty.ID
							Left Outer Join Payment
                            On Payment.InvoiceNo = sales.InvoiceNo
                            Where sales.Billtype = 'صدرت' ";
                if (!string.IsNullOrEmpty(DF) && !string.IsNullOrEmpty(DT))
                {
                    Qur += "And Sales.TDate >='"+ DF +"' And Sales.TDate <='"+ DT +"' ";
                }
                Qur += @"group by 
							Sales.Invoiceno,
                            thirdparty.Name,
                            Sales.TDate,
                            payment.PaymentMethod,
                            Sales.NonVatTotal,
                            Sales.Discount,
                            Sales.VatAmount,
                            Sales.TotalAmount,
                            payment.PaymentMethod";
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    var PM = GD["PaymentMethod"].ToString();
                    var Pid = GD["Paid"].ToString();
                    var Rem = GD["Remaining"].ToString();
                    if (PM == "")
                    {
                        PM = "غير مدفوع";
                    }
                    if (Pid == "")
                    {
                        Pid = "0";
                    }
                    if (Rem == "")
                    {
                        Rem = "0";
                    }
                    SL.Add(new Saleslist()
                    {
                        Invoiceno = int.Parse(GD["Invoiceno"].ToString()),
                        Name = GD["Name"].ToString(),
                        TDate = GD["TDate"].ToString(),
                        NonVatTotal = double.Parse(GD["NonVatTotal"].ToString()),
                        Discount = double.Parse(GD["Discount"].ToString()),
                        VatAmount = double.Parse(GD["VatAmount"].ToString()),
                        TotalAmount = double.Parse(GD["TotalAmount"].ToString()),
                        PaymentMethod = PM,
                        Paid = double.Parse(Pid),
                        Remaining = double.Parse(Rem),
                    });
                }
                return SL;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "AllSales" + "','" + "Get Sales List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
        }

        public List<Saleslist> Getsalesdetaileslist(string DF, string DT)
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
                            salesdetailes.ProductNo,
                            salesdetailes.TDDesc,
                            salesdetailes.Quantity,
                            salesdetailes.ItemPrice,
                             salesdetailes.Total,
                            Sales.TotalAmount  
                            FROM Sales 
                            Left outer Join salesdetailes 
                            on salesdetailes.InvoiceNo = sales.InvoiceNo 
                            Where sales.Billtype = 'صدرت'  And ProductNo IS NOT NULL ";
                if (!string.IsNullOrEmpty(DF) && !string.IsNullOrEmpty(DT))
                {
                    Qur += "And Sales.TDate >='" + DF + "' And Sales.TDate <='" + DT + "' ";
                }
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
                        ProductNo = int.Parse(GD["ProductNo"].ToString()),
                        TDDesc = GD["TDDesc"].ToString(),
                        Quantity = int.Parse(GD["Quantity"].ToString()),
                        ItemPrice = double.Parse(GD["ItemPrice"].ToString()),
                        Total = double.Parse(GD["Total"].ToString()),
                        TotalAmount = double.Parse(GD["TotalAmount"].ToString()),
                    });
                }
                return SL;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "AllSales" + "','" + "Get Sales Detailes List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
        }

        public List<Saleslist> Getsalesdetailesuser(string DF, string DT, int staffid)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                SL = new List<Saleslist>();
                var Qur = @"Select 
						Sales.Invoiceno,
                        Sales.TDate,
					    Sales.TTime,
                        Sales.NonVatTotal,
                        Sales.VatAmount,
                        salesdetailes.ItemPrice,
                        salesdetailes.Quantity,
                        salesdetailes.Total,
                        Sales.TotalAmount,
                        product.ProductNo,
                        product.Description,
                        staff.Username
                        From Sales
                        Left Outer Join salesdetailes
                        On Sales.Invoiceno = salesdetailes.Invoiceno
                        Left Outer Join product
                        On salesdetailes.ProductNo = product.ProductNo
                        Left Outer Join staff
                        On staff.StaffID = sales.StaffID
                        Where sales.Billtype = 'صدرت'  And product.ProductNo IS NOT NULL And Sales.StaffID = " + staffid;
                if (!string.IsNullOrEmpty(DF) && !string.IsNullOrEmpty(DT))
                {
                    Qur += " And Sales.TDate >='" + DF + "' And Sales.TDate <='" + DT + "' ";
                }
                Qur += @" Group By 
                        Sales.Invoiceno,
                        Sales.TDate,
					    Sales.TTime,
                        Sales.NonVatTotal,
                        Sales.VatAmount,
                        salesdetailes.ItemPrice,
                        salesdetailes.Quantity,
                        salesdetailes.Total,
                        Sales.TotalAmount,
                        product.ProductNo,
                        product.Description,
                        staff.Username";
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
                        ProductNo = int.Parse(GD["ProductNo"].ToString()),
                        TDDesc = GD["Description"].ToString(),
                        Quantity = int.Parse(GD["Quantity"].ToString()),
                        ItemPrice = double.Parse(GD["ItemPrice"].ToString()),
                        Total = double.Parse(GD["Total"].ToString()),
                        TotalAmount = double.Parse(GD["TotalAmount"].ToString()),
                    });
                }
                return SL;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "AllSales" + "','" + "Get Sales Detailes User" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);

                var Messagedata = ex.Message.ToString();
                return null;
            }
        }

        public List<Saleslist> Getpurchaselist(string DF, string DT)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                SL = new List<Saleslist>();
                var Qur = @"SELECT 
                            purchases.*,
                            thirdparty.Name 
                            FROM purchases 
                            inner join thirdparty 
                            on purchases.ThirdPartyID = thirdparty.ID ";
                if (!string.IsNullOrEmpty(DF) && !string.IsNullOrEmpty(DT))
                {
                    Qur += "And purchases.TDate >='" + DF + "' And purchases.TDate <='" + DT + "' ";
                }
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    SL.Add(new Saleslist()
                    {
                        Invoiceno = int.Parse(GD["Invoiceno"].ToString()),
                        Name = GD["Name"].ToString(),
                        TDate = GD["TDate"].ToString(),
                        NonVatTotal = double.Parse(GD["NonVatTotal"].ToString()),
                        Discount = double.Parse(GD["Discount"].ToString()),
                        VatAmount = double.Parse(GD["VatAmount"].ToString()),
                        TotalAmount = double.Parse(GD["TotalAmount"].ToString()),
                    });
                }
                return SL;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "AllSales" + "','" + "Get Purchases List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
        }


        public List<Expense> GetExpenselist(string DF, string DT,string typeexp)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                Exp = new List<Expense>();
                var Qur = @"SELECT 
                            expenses.ID, 
                            expenses.Description, 
                            expenses.Amount, 
                            expenses.CDate, 
                            expenses.Vat,
                            expencestype.Expencestypename
                            FROM expenses 
                            Left Outer Join expencestype
                            On expenses.Typeid = expencestype.Id ";
                if (!string.IsNullOrEmpty(DF) && !string.IsNullOrEmpty(DT))
                {
                    Qur += "Where ";
                    Qur += "expenses.CDate >='" + DF + "' And expenses.CDate <='" + DT + "' ";
                }
                if (!string.IsNullOrEmpty(typeexp))
                {
                    if (string.IsNullOrEmpty(DF) && string.IsNullOrEmpty(DT))
                    {
                        Qur += "Where ";
                    }
                    Qur += "expencestype.Expencestypename = '"+typeexp+"' ";
                }
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    Exp.Add(new Expense()
                    {
                        ID = int.Parse(GD["ID"].ToString()),
                        Description = GD["Description"].ToString(),
                        CDate = GD["CDate"].ToString(),
                        Amount = double.Parse(GD["Amount"].ToString()),
                        Vat = double.Parse(GD["Vat"].ToString()),
                    });
                }
                return Exp;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "AllSales" + "','" + "Get Expences List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
        }

        public List<Paymentmodel> GetPaymentlist(string DF, string DT)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                Payment = new List<Paymentmodel>();
                var Qur = @"SELECT 
                            payment.paymentNo,
                            payment.InvoiceNo,
                            payment.PaymentMethod,
                            payment.Date,
                            thirdparty.Name,
                            payment.Paid,
                            payment.Type
                            FROM payment 
                            Left Outer join thirdparty 
                            on payment.ThirdPartyID = thirdparty.ID 
                            Left Outer Join sales 
                            On sales.ThirdPartyID = thirdparty.ID ";
                if (!string.IsNullOrEmpty(DF) && !string.IsNullOrEmpty(DT))
                {
                    Qur += "And payment.Date >='" + DF + "' And payment.Date <='" + DT + "' ";
                }
                Qur += @"Group by 
                        payment.paymentNo,
                        payment.InvoiceNo,                                                           
                        payment.PaymentMethod,                                         
                        payment.Date,                                                     
                        thirdparty.Name,                                                           
                        payment.Paid,                                                           
                        payment.Type";
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    Payment.Add(new Paymentmodel()
                    {
                        paymentNo = int.Parse(GD["paymentNo"].ToString()),
                        InvoiceNo = int.Parse(GD["InvoiceNo"].ToString()),
                        PaymentMethod = GD["PaymentMethod"].ToString(),
                        Date = GD["Date"].ToString(),
                        Name = GD["Name"].ToString(),
                        Type = GD["Type"].ToString(),
                        Paid = double.Parse(GD["Paid"].ToString()),
                    });
                }
                return Payment;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "AllSales" + "','" + "Get Payment List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
        }

        public List<Paymentmodel> GetPayoutlist(string DF, string DT)
        {
            DA = new DataAccess();
            Con = new MySqlConnection(DA.CS);
            try
            {
                Payment = new List<Paymentmodel>();
                var Qur = @"SELECT 
                            paymentout.paymentNo,
                            paymentout.InvoiceNo,
                            paymentout.PaymentMethod,
                            paymentout.Date,
                            thirdparty.Name,
                            paymentout.Paid,
                            paymentout.Type
                            FROM paymentout 
                            Left Outer join thirdparty 
                            on paymentout.ThirdPartyID = thirdparty.ID ";
                if (!string.IsNullOrEmpty(DF) && !string.IsNullOrEmpty(DT))
                {
                    Qur += "And paymentout.Date >='" + DF + "' And paymentout.Date <='" + DT + "' ";
                }
                Qur += @"Group by 
                        paymentout.paymentNo,
                        paymentout.InvoiceNo,                                                           
                        paymentout.PaymentMethod,                                         
                        paymentout.Date,                                                     
                        thirdparty.Name,                                                           
                        paymentout.Paid,                                                           
                        paymentout.Type";
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    Payment.Add(new Paymentmodel()
                    {
                        paymentNo = int.Parse(GD["paymentNo"].ToString()),
                        InvoiceNo = int.Parse(GD["InvoiceNo"].ToString()),
                        PaymentMethod = GD["PaymentMethod"].ToString(),
                        Date = GD["Date"].ToString(),
                        Name = GD["Name"].ToString(),
                        Type = GD["Type"].ToString(),
                        Paid = double.Parse(GD["Paid"].ToString()),
                    });
                }
                return Payment;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "AllSales" + "','" + "Get Payout List" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                var Messagedata = ex.Message.ToString();
                return null;
            }
        }
    }
}
