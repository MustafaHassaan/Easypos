using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using Querylaeyr.Modelmaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.ResturantModles
{
    public class Itemsales
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }
        public int Itemid { get; set; }
        public int invoiceno { get; set; }
        public int Proquantity { get; set; }
        public List<Itemsales> ISL;
        DataAccess DA;
        public bool Saveitemsales()
        {
            Calculatitems();
            var Qur = @"Insert Into itemsales (Date,Quantity,Itemid,invoiceno) Values( '" + Date + "',"+ Quantity + ","+ ID + ","+ invoiceno + ")";
            DA = new DataAccess();
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
        public MySqlDataReader Getitemid(int proid)
        {
            ISL = new List<Itemsales>();
            var Qur = @"Select 
                        items.ID,
                        items.itemqty,
                        productitems.Quantity
                        From items
                        Left Outer Join productitems
                        on Items.ID = productitems.itemid
                        Where productitems.Proid = " + proid;
            DA = new DataAccess();
            var GD = DA.Getdatatable(Qur);
            if (GD == null)
            {
                return null;
            }
            else
            {
                foreach (DataRow row in GD.Rows)
                {
                    var ID = int.Parse(row["ID"].ToString());
                    var itemqty = int.Parse(row["itemqty"].ToString());
                    var Proquantity = int.Parse(row["Quantity"].ToString());
                    ISL.Add(new Itemsales()
                    {
                        ID = ID,
                        Quantity = itemqty,
                        Proquantity = Proquantity
                    });
                }
                return null;
            }
            
        }
        public void Calculatitems()
        {
            foreach (var item in ISL.ToList())
            {
                var ID = item.ID;
                var Quantity = item.Quantity;
                var Proquantity = item.Proquantity;
                this.Itemid = ID;
                this.Quantity = Proquantity;
                var Totresult = Quantity - Proquantity;
                Updateitemsales(Totresult);
            }
        }

        public void Updateitemsales(int Qty)
        {
            var Qur = @"Update items Set Itemqty = " + Qty + " Where ID = " + this.Itemid;
            DA = new DataAccess();
            var CMO = DA.Crudopration(Qur);
        }
    }
}
