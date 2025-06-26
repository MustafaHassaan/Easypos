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
    public class Category
    {
        public int CategoryNo { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Color { get; set; }

        DataAccess DA;
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT * FROM CATEGORY";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب الاصناف" + "','" + "Category Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public bool Crudopration(string Qur)
        {
            var data = Qur;
            DA = new DataAccess();
            var CMO = DA.Crudopration(data);
            if (CMO)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DataTable Getdatabysearch(string Qury)
        {
            try
            {
                var Qur = @"SELECT * FROM CATEGORY Where CategoryName or Description LIKE '" + Qury + "%' ";
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
                var Qur = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة البحث عن الاصناف" + "','" + "Category Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
                return null;
            }
        }
        public DataTable GetCombodata()
        {
            try
            {
                var Qur = @"SELECT CategoryNo,Description FROM CATEGORY";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب الاصناف" + "','" + "Category Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }

    }
}
