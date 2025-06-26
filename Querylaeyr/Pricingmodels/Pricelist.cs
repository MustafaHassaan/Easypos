using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Pricingmodels
{
    public class Pricelist
    {
        public int ID { get; set; }
        public string Datefrom { get; set; }
        public string Dateto { get; set; }
        public double NonVatTotal { get; set; }
        public double Discount { get; set; }
        public double VatAmount { get; set; }
        public double TotalAmount { get; set; }
        public int ThirdPartyID { get; set; }
        public string Name { get; set; }
        public string Taxnumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public int Priceid { get; set; }
        public int ProductNo { get; set; }
        public string TDDesc { get; set; }
        public string UnitType { get; set; }
        public int Unitid { get; set; }
        public double ItemPrice { get; set; }
        public int Quantity { get; set; }
        public double Subtotal { get; set; }
        public double Discountdet { get; set; }
        public double Totafterdiscount { get; set; }
        public decimal Total { get; set; }
    }
}
