using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Modelmaster
{
    public class addon_item
    {
        public int ID { get; set; }
        public string AddonCode { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public Double UnitPrice { get; set; }
        public string Barcode { get; set; }
        public bool ShowInPOS { get; set; }
    }
}
