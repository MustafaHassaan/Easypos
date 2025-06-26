using Dataaccesslaeyr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Tailor
{
    public class Fabrictype
    {
        public int Id { get; set; }
        public string Typename { get; set; }
        DataAccess DA;
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT Id,Typename FROM fabrictype";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب الاقمشة" + "','" + "Get Data" + "','" + ex.Message.ToString() + "')";
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

        public DataTable GetCombodata()
        {
            try
            {
                var Qur = @"SELECT Id,Typename FROM fabrictype";
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
