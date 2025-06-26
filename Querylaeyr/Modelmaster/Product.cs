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
    public class Product
    {
        public int ProductNo { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public double UnitPrice { get; set; }
        public double Purchaseprice { get; set; }
        public int StocksOnHand { get; set; }
        public int CategoryNo { get; set; }
        public int Unitid { get; set; }

        // Stock Product options
        public bool ShowInPOS { get; set; }
        public bool AllowInventory { get; set; }
        public bool Checkedit { get; set; }
        public bool Discountcheck { get; set; }
        public int ReorderLevel { get; set; }
        public int Order { get; set; }

        DataAccess DA;
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"Select 
                            product.ProductNo,
                            product.ProductCode,
                            product.Description,
                            product.Barcode,
                            product.UnitPrice,
                            product.StocksOnHand,
                            category.CategoryName,
                            unittypes.UnitType,
                            product.Unitid,
                            product.CategoryNo,
                            product.ShowInPOS,
                            product.AllowInventory,
                            product.Checkedit,
                            product.ReorderLevel
                            From product
                            Left Outer Join unittypes
                            on product.Unitid = unittypes.ID
                            Left Outer Join Category
							on product.CategoryNo = Category.CategoryNo";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المنتجات" + "','" + "Getdata" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Getdatabysearch(string search)
        {
            try
            {
                var Qur = @"Select 
                            product.ProductNo,
                            product.ProductCode,
                            product.Description,
                            product.Barcode,
                            product.UnitPrice,
                            product.StocksOnHand,
                            category.CategoryName,
                            unittypes.UnitType,
                            product.Unitid,
                            product.CategoryNo,
                            product.ShowInPOS,
                            product.AllowInventory,
                            product.Checkedit,
                            product.ReorderLevel
                            From product
                            Left Outer Join unittypes
                            on product.Unitid = unittypes.ID
                            Left Outer Join Category
							on product.Unitid = unittypes.ID
                            Where product.Description Like '" + search + "%' ";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة البحث عن الاصناف" + "','" + "Getdatabysearch" + "','" + ex.Message.ToString() + "')";
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
        public MySqlDataReader Getdatabyid()
        {
            try
            {
                var Qur = @"Select Max(ProductNo) As 'ProductNo' From Product";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    ProductNo = int.Parse(GD["ProductNo"].ToString());
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة البحث عن الاصناف" + "','" + "Getdatabyid" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public DataTable Filldata()
        {
            try
            {
                var Qur = @"SELECT ProductNo, 
                                   Description,
                                   Product.order FROM product 
                            Where CategoryNo= " + CategoryNo + " " +
                            "Order by  product.order";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة ملئ الاصناف" + "','" + "Filldata" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
    }
}
