using Dataaccesslaeyr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Tailor
{
    public class empcreator
    {
        public int Id { get; set; }
        public string Empname { get; set; }
        public string Empphone { get; set; }

        DataAccess DA;
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT Id,Empname,Empphone FROM empcreator";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب الوحدات" + "','" + "Get Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdatabytailor()
        {
            try
            {
                var Qur = @"SELECT Id,Empname FROM empcreator";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب الخياطين" + "','" + "Third Data" + "','" + ex.Message.ToString() + "')";
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
    }
}
