using Dataaccesslaeyr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Pricingmodels
{
    public class pricedetailes
    {
        public int ID { get; set; }
        public int Priceid { get; set; }
        public int ProductNo { get; set; }
        public string TDDesc { get; set; }
        public int Unitid { get; set; }
        public double ItemPrice { get; set; }
        public int Quantity { get; set; }
        public double Subtotal { get; set; }
        public double Discount { get; set; }
        public double Totafterdiscount { get; set; }
        public decimal Total { get; set; }


        DataAccess DA;
        public bool Savepricedetailes()
        {
            DA = new DataAccess();
            var Qur = @"Insert Into pricedetailes(Priceid,
                                                    ProductNo,
                                                    TDDesc,
                                                    Unitid,
                                                    ItemPrice,
                                                    Quantity,
                                                    Subtotal,
                                                    Discount,
                                                    Totafterdiscount,
                                                    Total)
                        Values(" + Priceid + "," +
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
