using Dataaccesslaeyr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Paymodel
{
    public class Transactions
    {
        public int ID { get; set; }
        public int Invoiceno { get; set; }
        public string Type { get; set; }

        DataAccess DA;
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
