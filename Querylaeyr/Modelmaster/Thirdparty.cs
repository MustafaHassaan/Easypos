using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Modelmaster
{
    public class Thirdparty
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Comments { get; set; }
        public int Type { get; set; }
        public double OpeningBalance { get; set; }
        public string Taxnumber { get; set; }
        public string Billnumber { get; set; }
        public string Pagenumber { get; set; }


        DataAccess DA;
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT * FROM Thirdparty";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب العملاء والمورديين" + "','" + "Third Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public bool Crudopration(string Qur)
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
        public DataTable Getdatabynum(string Qury)
        {
            try
            {
                var Qur = @"SELECT * FROM Thirdparty Where PhoneNumber or MobileNumber LIKE '" + Qury + "%' ";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة البحث عن العملاء والمورديين" + "','" + "Third Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdatabyname(string Qury)
        {
            try
            {
                var Qur = @"SELECT * FROM Thirdparty Where Name LIKE '" + Qury + "%' ";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة البحث عن العملاء والمورديين" + "','" + "Third Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public MySqlDataReader Getdatabyid(int id)
        {
            try
            {
                var Qur = @"SELECT * FROM Thirdparty Where ID =" + id ;
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    Type = int.Parse(GD["Type"].ToString());
                    MobileNumber = GD["MobileNumber"].ToString();
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة البحث عن العملاء والمورديين" + "','" + "Third Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdatabysuppliers()
        {
            try
            {
                var Qur = @"SELECT * FROM Thirdparty Where Type = 1 ";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المورديين" + "','" + "Third Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdatabycustomer()
        {
            try
            {
                var Qur = @"SELECT * FROM Thirdparty Where Type = 2 ";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب العملاء" + "','" + "Third Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdatabycustomersupplier(string Qury, int Thirdnum)
        {
            var Qur = @"SELECT * FROM Thirdparty Where Name = '" + Qury + "' And Type = "+ Thirdnum +" ";
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
    }
}
