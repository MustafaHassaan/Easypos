using Dataaccesslaeyr;
using Querylaeyr.Modelmaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Modelsales
{
    public class Salesdetails
    {
        public int TDetailNo { get; set; }
        public int InvoiceNo { get; set; }
        public int ProductNo { get; set; }
        public string TDDesc { get; set; }
        public int Unitid { get; set; }
        public decimal ItemPrice { get; set; }
        public double Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Totafterdiscount { get; set; }
        public decimal Total { get; set; }

        DataAccess DA;
        public bool Savesalesdetailes()
        {
            DA = new DataAccess();
            var Qur = @"Insert Into Salesdetailes(InvoiceNo,
                                                    ProductNo,
                                                    TDDesc,
                                                    Unitid,
                                                    ItemPrice,
                                                    Quantity,
                                                    Subtotal,
                                                    Discount,
                                                    Totafterdiscount,
                                                    Total)
                        Value("+ InvoiceNo + ","+
                               ""+ ProductNo + ","+
                               "'"+ TDDesc + "',"+
                               ""+ Unitid + ","+
                               ""+ ItemPrice + ","+
                               ""+ Quantity + ","+
                               ""+ Subtotal + ","+
                               ""+ Discount + ","+
                               ""+ Totafterdiscount + ","+
                               ""+ Total + " "+
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
        public bool Savepurchasedetailes()
        {
            DA = new DataAccess();
            var Qur = @"Insert Into purchasedetailes(InvoiceNo,
                                                    ProductNo,
                                                    TDDesc,
                                                    Unitid,
                                                    ItemPrice,
                                                    Quantity,
                                                    Subtotal,
                                                    Discount,
                                                    Totafterdiscount,
                                                    Total)
                        Values(" + InvoiceNo + "," +
                               "" + ProductNo + "," +
                               "'" + TDDesc + "'," +
                               "" + Unitid + "," +
                               "" + ItemPrice + "," +
                               "" + Quantity + "," +
                               "" + Subtotal + "," +
                               "" + Discount + "," +
                               "" + Totafterdiscount + "," +
                               "" + Total + " " +
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
    }
}
