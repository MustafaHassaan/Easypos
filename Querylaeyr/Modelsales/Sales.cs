using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using Querylaeyr.Modelmaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Modelsales
{
    public class Sales
    {
        public int Invoiceno { get; set; }
        public string TDate { get; set; }
        public string TTime { get; set; }
        public double NonVatTotal { get; set; }
        public double Discount { get; set; }
        public double VatAmount { get; set; }
        public double TotalAmount { get; set; }
        public int ThirdPartyID { get; set; }
        public string Billtype { get; set; }
        public string Supplierbill { get; set; }
        public int StaffID { get; set; }
        public string Note { get; set; }
        DataAccess DA;
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT
                            Sales.Invoiceno,
                            Sales.TDate,
                            Sales.TTime,
                            Sales.NonVatTotal,
                            Sales.Discount,
                            Sales.VatAmount,
                            Sales.TotalAmount,
                            thirdparty.Name,
                            Payment.Cash,
                            Payment.Bank,
                            Sales.Billtype,
                            Sales.Note
                            FROM Sales
                            Left Outer Join thirdparty
                            On Sales.ThirdPartyID = thirdparty.ID
                            Left Outer Join Payment
                            On Payment.InvoiceNo = sales.InvoiceNo
                            ORDER BY Sales.Invoiceno DESC";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المبيعات" + "','" + "Sales Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getpurdata()
        {
            try
            {
                var Qur = @"SELECT
                            purchases.Invoiceno,
                            purchases.TDate,
                            purchases.TTime,
                            purchases.NonVatTotal,
                            purchases.Discount,
                            purchases.VatAmount,
                            purchases.TotalAmount,
                            thirdparty.Name,
                            purchases.Billtype
                            FROM purchases
                            Left Outer Join thirdparty
                            On purchases.ThirdPartyID = thirdparty.ID
                            ORDER BY purchases.Invoiceno DESC";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المشتريات" + "','" + "Getpurdata" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdatapos()
        {
            try
            {
                var Qur = @"SELECT
                            Sales.Invoiceno,
                            Sales.TDate,
                            Sales.Billtype
                            FROM Sales
                            ORDER BY Sales.Invoiceno DESC";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المبيعات" + "','" + "Getdatapos" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public bool Savesales()
        {
            DA = new DataAccess();
            var Qur = @"Insert Into Sales(TDate,TTime,NonVatTotal,Discount,VatAmount,TotalAmount,ThirdPartyID,Billtype,StaffID,Note) 
                        Values( " +
                        "'"+ TDate + "'," +
                        "'"+ TTime + "'," +
                        ""+ NonVatTotal + "," +
                        ""+ Discount + "," +
                        ""+ VatAmount + "," +
                        ""+ TotalAmount + "," +
                        ""+ ThirdPartyID + ", " +
                        "'"+ Billtype + "', " +
                        " "+ StaffID + ", " +
                        " '"+ Note + "' " +
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
        public int Getlastbyid()
        {
            try
            {
                var Qur = @"Select Max(Invoiceno) As 'Invoiceno' From sales";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    Invoiceno = int.Parse(GD["Invoiceno"].ToString());
                    return Invoiceno;
                }
                else
                {
                    return Invoiceno;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المبيعات" + "','" + "Getlastbyid" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return Invoiceno;
            }
        }
        public int Getsalesbyid(int id)
        {
            try
            {
                var Qur = @"Select Invoiceno As 'Invoiceno' From sales Where Invoiceno = " + id;
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    Invoiceno = int.Parse(GD["Invoiceno"].ToString());
                    return Invoiceno;
                }
                else
                {
                    return Invoiceno;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المبيعات" + "','" + "Getsalesbyid" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return Invoiceno;
            }
        }
        public int Getpurbyid(int id)
        {
            try
            {
                var Qur = @"Select Invoiceno As 'Invoiceno' From purchases Where Invoiceno = " + id;
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    Invoiceno = int.Parse(GD["Invoiceno"].ToString());
                    return Invoiceno;
                }
                else
                {
                    return Invoiceno;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المبيعات" + "','" + "Getpurbyid" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return Invoiceno;
            }
        }
        public bool Editsales()
        {
            DA = new DataAccess();
            var Qur = @"Update Sales Set 
                        TDate = '" + TDate + "', " +
                        "TTime = '" + TTime + "', " +
                        "NonVatTotal = " + NonVatTotal + ", " +
                        "Discount = " + Discount + ", " +
                        "VatAmount = " + VatAmount + ", " +
                        "TotalAmount = " + TotalAmount + ", " +
                        "ThirdPartyID = " + ThirdPartyID + ", " +
                        "Billtype = '" + Billtype + "', " +
                        "StaffID = " + StaffID + " " +
                        "Where Invoiceno = " + Invoiceno;
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
        public bool Editpur()
        {
            DA = new DataAccess();
            var Qur = @"Update purchases Set 
                        TDate = '" + TDate + "', " +
                        "TTime = '" + TTime + "', " +
                        "NonVatTotal = " + NonVatTotal + ", " +
                        "Discount = " + Discount + ", " +
                        "VatAmount = " + VatAmount + ", " +
                        "TotalAmount = " + TotalAmount + ", " +
                        "ThirdPartyID = " + ThirdPartyID + ", " +
                        "Billtype = '" + Billtype + "' " +
                        "Where Invoiceno = " + Invoiceno;
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
        public bool Savepurchase()
        {
            DA = new DataAccess();
            var Qur = @"Insert Into purchases(TDate,TTime,NonVatTotal,Discount,VatAmount,TotalAmount,ThirdPartyID,Billtype,Supplierbill) 
                        Values( " +
                        "'" + TDate + "'," +
                        "'" + TTime + "'," +
                        "" + NonVatTotal + "," +
                        "" + Discount + "," +
                        "" + VatAmount + "," +
                        "" + TotalAmount + "," +
                        "" + ThirdPartyID + ", " +
                        "'" + Billtype + "', " +
                        "'" + Supplierbill + "' " +
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
        public int GetPurlastbyid()
        {
            try
            {
                var Qur = @"Select Max(Invoiceno) As 'Invoiceno' From purchases";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    Invoiceno = int.Parse(GD["Invoiceno"].ToString());
                    return Invoiceno;
                }
                else
                {
                    return Invoiceno;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المشتريات" + "','" + "GetPurlastbyid" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return Invoiceno;
            }
        }
        public DataTable Getdatabyfilter(int Invoiceno, int ThirdPartyID, string DF, string DT, string TF, string TT, string Phone, string Note)
        {
            bool flag = false;
            try
            {
                var Qur = @"SELECT
                            Sales.Invoiceno,
                            Sales.TDate,
                            Sales.TTime,
                            Sales.NonVatTotal,
                            Sales.Discount,
                            Sales.VatAmount,
                            Sales.TotalAmount,
                            thirdparty.Name,
                            Payment.Cash,
                            Payment.Bank,
                            Sales.Billtype,
                            Sales.Note
                            FROM Sales
                            Left Outer Join thirdparty
                            On Sales.ThirdPartyID = thirdparty.ID 
                            Left Outer Join Payment
                            On Payment.InvoiceNo = sales.InvoiceNo ";
                if (Invoiceno != 0 || 
                    ThirdPartyID != 0 || 
                    !string.IsNullOrEmpty(DT) || 
                    !string.IsNullOrEmpty(DF) || 
                    !string.IsNullOrEmpty(TF) || 
                    !string.IsNullOrEmpty(TT) ||
                    !string.IsNullOrEmpty(Phone) ||
                    !string.IsNullOrEmpty(Note)
                    )
                {
                    Qur += "Where ";
                    if (Invoiceno != 0)
                    {
                        Qur += @"Sales.Invoiceno = " + Invoiceno;
                        flag = true;
                    }
                    if (ThirdPartyID != 0)
                    {
                        if (flag)
                        {
                            Qur += " And ";
                        }
                        Qur += @"Sales.ThirdPartyID = " + ThirdPartyID;
                        flag = true;
                    }
                    if (!string.IsNullOrEmpty(DT) &&
                        !string.IsNullOrEmpty(DF))
                    {
                        if (flag)
                        {
                            Qur += " And ";
                        }
                        Qur += @"Sales.TDate >= '" + DF + "' And Sales.TDate <= '" + DT + "'";
                        flag = true;
                    }
                    if (!string.IsNullOrEmpty(TT) &&
                        !string.IsNullOrEmpty(TF))
                    {
                        if (flag)
                        {
                            Qur += " And ";
                        }
                        Qur += @"Sales.TTime >= '" + TF + "' And Sales.TTime <= '" + TT + "' ";
                    }
                    if (!string.IsNullOrEmpty(Phone))
                    {
                        if (flag)
                        {
                            Qur += " And ";
                        }
                        Qur += @"thirdparty.MobileNumber = '" + Phone + "' ";
                    }
                    if (!string.IsNullOrEmpty(Note))
                    {
                        if (flag)
                        {
                            Qur += " And ";
                        }
                        Qur += @"Sales.Note = '" + Note + "' ";
                    }
                }
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة فلترة المبيعات" + "','" + "Getdatabyfilter" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdatapurbyfilter(int Invoiceno, int ThirdPartyID, string DF, string DT)
        {
            bool flag = false;
            try
            {
                var Qur = @"SELECT
                            purchases.Invoiceno,
                            purchases.TDate,
                            purchases.TTime,
                            purchases.NonVatTotal,
                            purchases.Discount,
                            purchases.VatAmount,
                            purchases.TotalAmount,
                            thirdparty.Name
                            FROM purchases
                            Left Outer Join thirdparty
                            On purchases.ThirdPartyID = thirdparty.ID ";
                if (Invoiceno != 0 ||
                    ThirdPartyID != 0 ||
                    !string.IsNullOrEmpty(DT) ||
                    !string.IsNullOrEmpty(DF))
                {
                    Qur += "Where ";
                    if (Invoiceno != 0)
                    {
                        Qur += @" purchases.Invoiceno = " + Invoiceno;
                        flag = true;
                    }
                    if (ThirdPartyID != 0)
                    {
                        if (flag)
                        {
                            Qur += " And ";
                        }
                        Qur += @" purchases.ThirdPartyID = " + ThirdPartyID;
                        flag = true;
                    }
                    if (!string.IsNullOrEmpty(DT) &&
                        !string.IsNullOrEmpty(DF))
                    {
                        if (flag)
                        {
                            Qur += " And ";
                        }
                        Qur += @" purchases.TDate >= '" + DF + "' And purchases.TDate <= '" + DT + "'";
                    }
                }
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة فلترة المشتريات" + "','" + "Getdatapurbyfilter" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
    }
}
