using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Modelmaster
{
    public class Items
    {
        public int ID { get; set; }
        public string Itemname { get; set; }
        public double Itemprice { get; set; }
        public double Itemqty { get; set; }
        public int UID { get; set; }
        public int OpeningBalance { get; set; }
        public double Remining { get; set; }

        DataAccess DA;
        public DataTable Getsalesinvdata()
        {
            try
            {
                var Qur = @"SELECT 
                            itemsales.Id,
                            items.Itemname,
                            itemsales.Date,
                            items.Itemqty,
                            itemsales.Quantity,
                            items.Remining,
                            itemsales.invoiceno
                            FROM itemsales
                            Left Outer Join Items
                            on itemsales.Itemid = Items.ID";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاصناف الاولية" + "','" + "Items Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getpurinvdata()
        {
            try
            {
                var Qur = @"SELECT 
                            purchasedetailes.TDetailNo,
                            purchasedetailes.TDDesc,
                            purchases.Invoiceno,
                            purchases.TDate,
                            purchasedetailes.Quantity
                            FROM purchases
                            Left Outer Join purchasedetailes
                            On purchases.Invoiceno = purchasedetailes.InvoiceNo;";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاصناف الاولية" + "','" + "Items Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT 
                            items.ID,
                            items.Itemname,
                            items.Itemprice,
                            items.Itemqty,
                            unittypes.UnitType,
                            items.OpeningBalance,
                            items.UID
                            FROM items
                            Left Outer Join unittypes
                            on unittypes.ID = items.UID;";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة الاصناف الاولية" + "','" + "Items Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public bool Crudopration(string Qur)
        {
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
        public DataTable Getdatabysearch(string Qury)
        {
            try
            {
                var Qur = @"SELECT 
                            items.ID,
                            items.Itemname,
                            items.Itemprice,
                            items.Itemqty,
                            unittypes.UnitType,
                            items.OpeningBalance,
                            items.UID
                            FROM items
                            Left Outer Join unittypes
                            on unittypes.ID = items.UID Where Itemname LIKE '" + Qury + "%' ";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "البحث عن الاصناف الاوليه" + "','" + "Getdatabysearch" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public MySqlDataReader Getdatabyquery(int Proid)
        {
            try
            {
                var Qur = @"SELECT 
                               Itemid,
                               Quantity
                               From Productitems Where Proid = " + Proid;
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة البحث عن الاصناف الاوليه" + "','" + "Getdatabyquery" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getitemsbydatatable(string Qur)
        {
            try
            {
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب قائمة الاصناف الاوليه" + "','" + "Getitemsbydatatable" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
    }
}
