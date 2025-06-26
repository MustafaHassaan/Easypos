using Dataaccesslaeyr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Modelmaster
{
    public class Expense
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string CDate { get; set; }
        public double Vat { get; set; }
        public int? Typeid { get; set; }

        DataAccess DA;
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT
                            expenses.ID,
                            expenses.Description,
                            expenses.Amount,
                            expenses.CDate,
                            expenses.Vat,
                            expencestype.Expencestypename
                            FROM expenses 
                            Left Outer Join expencestype
                            on expenses.Typeid = expencestype.Id";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المصروفات" + "','" + "Expences Data" + "','" + ex.Message.ToString() + "')";
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
                var Qur = @"SELECT
                            expenses.ID,
                            expenses.Description,
                            expenses.Amount,
                            expenses.CDate,
                            expenses.Vat,
                            expencestype.Expencestypename
                            FROM expenses 
                            Left Outer Join expencestype
                            on expenses.Typeid = expencestype.Id Where Description LIKE '" + Qury + "%' ";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المصروفات" + "','" + "Expences Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
    }
}
