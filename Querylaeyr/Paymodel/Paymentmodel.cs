using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using Querylaeyr.Modelmaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Querylaeyr.Paymodel
{
    public class Paymentmodel
    {
        public int paymentNo { get; set; }
        public int InvoiceNo { get; set; }
        public int Paynum { get; set; }
        public double Cash { get; set; }
        public double Bank { get; set; }
        public double Paid { get; set; }
        public double Remaining { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int ThirdPartyID { get; set; }
        public string PaymentMethod { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public string Name { get; set; }

        DataAccess DA;
        public bool Savepayment()
        {
            DA = new DataAccess();
            var Qur = @"Insert Into payment(InvoiceNo,
                                            Cash,
                                            Bank,
                                            Paid,
                                            Remaining,
                                            Date,
                                            Time,
                                            ThirdPartyID,
                                            PaymentMethod,
                                            Type,
                                            Note
                                            )
                        Values(" + InvoiceNo + "," +
                               "" + Cash + "," +
                               "" + Bank + "," +
                               "" + Paid + "," +
                               "" + Remaining + "," +
                               "'" + Date + "'," +
                               "'" + Time + "'," +
                               "" + ThirdPartyID + "," +
                               "'" + PaymentMethod + "', " +
                               "'" + Type + "', " +
                               "'" + Note + "' " +
                        ")";
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
        public bool Savepaymentout()
        {
            DA = new DataAccess();
            var Qur = @"Insert Into paymentout(InvoiceNo,
                                            Cash,
                                            Bank,
                                            Paid,
                                            Remaining,
                                            Date,
                                            Time,
                                            ThirdPartyID,
                                            PaymentMethod,
                                            Type,
                                            Note
                                            )
                        Values(" + InvoiceNo + "," +
                               "" + Cash + "," +
                               "" + Bank + "," +
                               "" + Paid + "," +
                               "" + Remaining + "," +
                               "'" + Date + "'," +
                               "'" + Time + "'," +
                               "" + ThirdPartyID + "," +
                               "'" + PaymentMethod + "', " +
                               "'" + Type + "', " +
                               "'" + Note + "' " +
                        ")";
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
        public MySqlDataReader Getdata(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select * From Payment Where Invoiceno =" + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        paymentNo = int.Parse(GD["paymentNo"].ToString());
                        Cash = double.Parse(GD["Cash"].ToString());
                        Bank = double.Parse(GD["Bank"].ToString());
                        Paid = double.Parse(GD["Paid"].ToString());
                        Remaining = double.Parse(GD["Remaining"].ToString());
                        return GD;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المدفوعات" + "','" + "Getdata" + "','" + ex.Message.ToString() + "')";
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
        public MySqlDataReader Getpurdata(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select * From Paymentout Where Invoiceno =" + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        Cash = double.Parse(GD["Cash"].ToString());
                        Bank = double.Parse(GD["Bank"].ToString());
                        Paid = double.Parse(GD["Paid"].ToString());
                        Remaining = double.Parse(GD["Remaining"].ToString());
                        return GD;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public DataTable Getdata()
        {
            try
            {
                //Where payment.Type != 'فاتورة مبيعات'
                var Qur = @"SELECT 
                            transactions.ID,
                            transactions.Invoiceno,
                            transactions.Paynum,
                            transactions.TDate,
                            transactions.Type,
                            thirdparty.Name,
                            transactions.Paid,
                            transactions.Paytype,
                            transactions.Note
                            FROM transactions
                            Left Outer Join thirdparty
                            on transactions.ThirdPartyID = thirdparty.ID
                            Order By transactions.ID DESC";
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
                return null;
            }
        }
        public DataTable Getrecivdata()
        {
            try
            {
                var Qur = @"SELECT
                            paymentout.paymentNo,
                            paymentout.InvoiceNo,
                            paymentout.Date,
                            thirdparty.Name,
                            paymentout.Paid,
                            paymentout.PaymentMethod,
                            paymentout.Type,
                            paymentout.Note
                            FROM paymentout
                            Left Outer Join thirdparty
                            on paymentout.ThirdPartyID = thirdparty.ID
                            Where paymentout.Type != 'فاتورة مشتريات'
                            Order by paymentout.paymentNo DESC";
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
                return null;
            }
        }
        public List<Paymentmodel> invoices { get; set; }
        public MySqlDataReader GetPaybyid()
        {
            try
            {
                var Qur = @"Select Max(paymentNo) As 'paymentNo' From payment";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    paymentNo = int.Parse(GD["paymentNo"].ToString());
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public MySqlDataReader GetPayoutbyid()
        {
            try
            {
                var Qur = @"Select Max(paymentNo) As 'paymentNo' From Paymentout";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    paymentNo = int.Parse(GD["paymentNo"].ToString());
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Editpayment()
        {
            DA = new DataAccess();
            var Qur = @"Update payment Set 
                        InvoiceNo = " + InvoiceNo + "," +
                        "Cash = " + Cash + "," +
                        "Bank = " + Bank + "," +
                        "Paid = " + Paid + "," +
                        "Remaining = " + Remaining + "," +
                        "Date = '" + Date + "'," +
                        "Time = '" + Time + "'," +
                        "ThirdPartyID = " + ThirdPartyID + "," +
                        "PaymentMethod = '" + PaymentMethod + "'," +
                        "Type = '" + Type + "'," +
                        "Note = '" + Note + "' Where paymentNo = " + paymentNo;
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
        public bool Editpaymentout()
        {
            DA = new DataAccess();
            var Qur = @"Update paymentout Set 
                        InvoiceNo = " + InvoiceNo + "," +
                        "Cash = " + Cash + "," +
                        "Bank = " + Bank + "," +
                        "Paid = " + Paid + "," +
                        "Remaining = " + Remaining + "," +
                        "Date = '" + Date + "'," +
                        "Time = '" + Time + "'," +
                        "ThirdPartyID = " + ThirdPartyID + "," +
                        "PaymentMethod = '" + PaymentMethod + "'," +
                        "Type = '" + Type + "'," +
                        "Note = '" + Note + "' Where paymentNo = " + paymentNo;
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
        public MySqlDataReader Getvoucher(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select * From Payment Where paymentNo =" + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        Cash = double.Parse(GD["Cash"].ToString());
                        Bank = double.Parse(GD["Bank"].ToString());
                        Paid = double.Parse(GD["Paid"].ToString());
                        Remaining = double.Parse(GD["Remaining"].ToString());
                        return GD;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public MySqlDataReader GetReciver(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select * From Paymentout Where paymentNo =" + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        Cash = double.Parse(GD["Cash"].ToString());
                        Bank = double.Parse(GD["Bank"].ToString());
                        Paid = double.Parse(GD["Paid"].ToString());
                        Remaining = double.Parse(GD["Remaining"].ToString());
                        return GD;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public MySqlDataReader Printvoucher(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"SELECT 
                                transactions.ID,
                                transactions.Invoiceno,
                                transactions.TDate,
                                transactions.Type,
                                thirdparty.Name,
                                transactions.Paid,
                                transactions.Paytype,
                                transactions.Note
                                FROM
                                transactions
                                Left Outer Join thirdparty
                                on transactions.ThirdPartyID = thirdparty.ID
                                Where transactions.ID =" + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        paymentNo = int.Parse(GD["ID"].ToString());
                        InvoiceNo = int.Parse(GD["InvoiceNo"].ToString());
                        Paid = double.Parse(GD["Paid"].ToString());
                        Date = GD["TDate"].ToString();
                        PaymentMethod = GD["Paytype"].ToString();
                        Type = GD["Type"].ToString();
                        Note = GD["Note"].ToString();
                        Name = GD["Name"].ToString();
                        return GD;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public MySqlDataReader Printreciver(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"SELECT 
                            paymentout.*,
                            thirdparty.Name 
                            FROM paymentout
                            inner join thirdparty 
                            on paymentout.ThirdPartyID = thirdparty.ID 
                            WHERE paymentout.paymentNo =" + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        paymentNo = int.Parse(GD["paymentNo"].ToString());
                        InvoiceNo = int.Parse(GD["InvoiceNo"].ToString());
                        Cash = double.Parse(GD["Cash"].ToString());
                        Bank = double.Parse(GD["Bank"].ToString());
                        Paid = double.Parse(GD["Paid"].ToString());
                        Remaining = double.Parse(GD["Remaining"].ToString());
                        Date = GD["Date"].ToString();
                        Time = GD["Time"].ToString();
                        PaymentMethod = GD["PaymentMethod"].ToString();
                        Type = GD["Type"].ToString();
                        Note = GD["Note"].ToString();
                        Name = GD["Name"].ToString();
                        return GD;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public DataTable Getvoucherbythird(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select 
                                sales.InvoiceNo,
                                payment.Remaining
                                From Sales
                                Left Outer Join Payment
                                on Sales.InvoiceNo = Payment.Invoiceno
                                Where Sales.thirdpartyID = " + id +
                                " And sales.Billtype != 'مسوده' "+
                                " ORDER BY sales.InvoiceNo DESC ";
                    DA = new DataAccess();
                    var GD = DA.Getdatatable(Qur);
                    invoices = new List<Paymentmodel>();
                    if (GD != null)
                    {
                        foreach (DataRow row in GD.Rows)
                        {
                            var Rem = row["Remaining"].ToString();
                            if (Rem == "")
                            {
                                Rem = "0.00";
                            }
                            invoices.Add(new Paymentmodel
                            {
                                InvoiceNo = int.Parse(row["InvoiceNo"].ToString()),
                                Remaining = double.Parse(Rem)
                            });
                        }
                    }
                    return GD;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + Remaining);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public DataTable Getreciverbythird(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select 
                                purchases.InvoiceNo,
                                paymentout.Remaining
                                From purchases
                                Left Outer Join paymentout
                                on purchases.InvoiceNo = paymentout.Invoiceno
                                Where purchases.ThirdPartyID = " + id +
                                " And purchases.Billtype != 'مسوده' " +
                                " ORDER BY purchases.InvoiceNo DESC  ";
                    DA = new DataAccess();
                    var GD = DA.Getdatatable(Qur);
                    invoices = new List<Paymentmodel>();
                    if (GD != null)
                    {
                        foreach (DataRow row in GD.Rows)
                        {
                            var Rem = row["Remaining"].ToString();
                            if (Rem == "")
                            {
                                Rem = "0.00";
                            }
                            invoices.Add(new Paymentmodel
                            {
                                InvoiceNo = int.Parse(row["InvoiceNo"].ToString()),
                                Remaining = double.Parse(Rem)
                            });
                        }
                    }
                    return GD;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public MySqlDataReader Getvoucherinv(int id)
        {
            if (id != null)
            {
                try
                {
                    var Qur = @"Select * From Payment Where InvoiceNo =" + id;
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        Cash = double.Parse(GD["Cash"].ToString());
                        Bank = double.Parse(GD["Bank"].ToString());
                        Paid = double.Parse(GD["Paid"].ToString());
                        Remaining = double.Parse(GD["Remaining"].ToString());
                        return GD;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
