using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using Querylaeyr.Modelsales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Querylaeyr.Pricingmodels
{
    public class Price
    {
        public int ID { get; set; }
        public string Datefrom { get; set; }
        public string Dateto { get; set; }
        public double NonVatTotal { get; set; }
        public double Discount { get; set; }
        public double VatAmount { get; set; }
        public double TotalAmount { get; set; }
        public int ThirdPartyID { get; set; }

        DataAccess DA;
        public bool Deletesalesdetailes(int id)

        {
            DA = new DataAccess();
            var Qur = @"Delete From pricedetailes Where Priceid = " + id;
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
        public bool Savesales()
        {
            DA = new DataAccess();
            var Qur = @"Insert Into Price(Datefrom,Dateto,NonVatTotal,Discount,VatAmount,TotalAmount,ThirdPartyID) 
                        Values( " +
                        "'" + Datefrom + "'," +
                        "'" + Dateto + "'," +
                        "" + NonVatTotal + "," +
                        "" + Discount + "," +
                        "" + VatAmount + "," +
                        "" + TotalAmount + "," +
                        "" + ThirdPartyID + " " +
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
        public bool Editsales(int id)
        {
            DA = new DataAccess();
            var Qur = @"Update Price set " +
                        "Datefrom = '" + Datefrom + "'," +
                        "Dateto = '" + Dateto + "'," +
                        "NonVatTotal = " + NonVatTotal + "," +
                        "Discount = " + Discount + "," +
                        "VatAmount = " + VatAmount + "," +
                        "TotalAmount = " + TotalAmount + "," +
                        "ThirdPartyID = " + ThirdPartyID + " " +
                        " Where ID = " + id ;
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
                var Qur = @"Select Max(ID) As 'ID' From price";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    ID = int.Parse(GD["ID"].ToString());
                    return ID;
                }
                else
                {
                    return ID;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المبيعات بواسطة الرقم" + "','" + "Getlastbyid" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return ID;
            }
        }

        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT
                            Price.ID,
                            Price.NonVatTotal,
                            Price.Discount,
                            Price.VatAmount,
                            Price.TotalAmount,
                            thirdparty.Name
                            FROM Price
                            Left Outer Join thirdparty
                            On Price.ThirdPartyID = thirdparty.ID
                            ORDER BY Price.ID DESC";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المبيعات" + "','" + "GetData" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }

        public DataTable Getdatabyfilter(int Invoiceno, int ThirdPartyID, string DF, string DT)
        {
            bool flag = false;
            try
            {
                var Qur = @"SELECT
                            Price.ID,
                            Price.NonVatTotal,
                            Price.Discount,
                            Price.VatAmount,
                            Price.TotalAmount,
                            thirdparty.Name
                            FROM Price
                            Left Outer Join thirdparty
                            On Sales.ThirdPartyID = thirdparty.ID ";
                if (Invoiceno != 0 ||
                    ThirdPartyID != 0 ||
                    !string.IsNullOrEmpty(DT) ||
                    !string.IsNullOrEmpty(DF))
                {
                    Qur += "Where ";
                    if (Invoiceno != 0)
                    {
                        Qur += @"Price.ID = " + Invoiceno;
                        flag = true;
                    }
                    if (ThirdPartyID != 0)
                    {
                        if (flag)
                        {
                            Qur += " And ";
                        }
                        Qur += @"Price.ThirdPartyID = " + ThirdPartyID;
                        flag = true;
                    }
                    if (!string.IsNullOrEmpty(DT) &&
                        !string.IsNullOrEmpty(DF))
                    {
                        if (flag)
                        {
                            Qur += " And ";
                        }
                        Qur += @"Price.Datefrom >= '" + DF + "' And Price.Dateto <= '" + DT + "'";
                        flag = true;
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة فلترة المنتجات" + "','" + "Getdatabyfilter" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }

    }
}
