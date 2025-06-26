using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Accountant
{
    public class Accountanat
    {
        public int ID { get; set; }
        public int Invoiceno { get; set; }
        public string Type { get; set; }
        public string TDate { get; set; }
        public int ThirdPartyID { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Taxnumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Paid { get; set; }
        public decimal Remaining { get; set; }
        public string Billtype { get; set; }
    }
}
