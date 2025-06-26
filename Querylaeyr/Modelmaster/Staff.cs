using Dataaccesslaeyr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Modelmaster
{
    public class Staff
    {
        public int StaffID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fullname { get; set; }
        public string Phonenumber { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string UPassword { get; set; }
        public string Role { get; set; }

        DataAccess DA;
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT * FROM Staff";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب بيانات المستخدمين" + "','" + "Getdata" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getusers()
        {
            try
            {
                var Qur = @"SELECT StaffID,Username FROM Staff";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب معلومات المستخدم" + "','" + "Getusers" + "','" + ex.Message.ToString() + "')";
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
                var Qur = @"SELECT * FROM Staff Where Phonenumber LIKE '" + Qury + "%' ";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب بيانات المستخدم" + "','" + "Getdatabynum" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdatabyname(string Qury)
        {
            try
            {
                var Qur = @"SELECT * FROM Staff Where Fullname LIKE '" + Qury + "%' ";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب بيانات المستخدم" + "','" + "Getdatabyname" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
    }
}
