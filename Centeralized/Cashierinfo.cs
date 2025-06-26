using Dataaccesslaeyr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Centeralized
{
    public class Cashierinfo
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string Systemserial { get; set; }
        public string Materialname { get; set; }
        public string CSR { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string Secret { get; set; }
        public int Comid { get; set; }


        DataAccess DA;
        public void GetCashiers()
        {
            try
            {
                var Qur = @"SELECT Id,SystemName,Systemserial,Materialname,CSR,PrivateKey,PublicKey,Secret FROM Cashier";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    Id = int.Parse(GD[0].ToString());
                    SystemName = GD[1].ToString();
                    Systemserial = GD[2].ToString();
                    Materialname = GD[3].ToString();
                    CSR = GD[4].ToString();
                    PrivateKey = GD[5].ToString();
                    PublicKey = GD[6].ToString();
                    Secret = GD[7].ToString();
                }
            }
            catch (Exception ex)
            {
                var Qur = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة ادخال معلومات الشركة" + "','" + "Company Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
            }
        }
        public void Savedata()
        {
            try
            {
                var Qur = @"Insert Into Cashier (SystemName,Systemserial,Materialname,CSR,PrivateKey,PublicKey,Secret,Comid) value('" + SystemName+"','"+Systemserial+"','"+Materialname+"','"+CSR+"','"+PrivateKey+"','"+PublicKey+"','"+Secret+"',"+ Comid + ")";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
                if (CMO)
                {
                    MessageBox.Show("تم ادخال معلومات الشركة بنجاح", "معلومات الشركة");
                }
                else
                {
                    MessageBox.Show("خطأ لم يتم ادخال المعلومات", "معلومات الشركة");
                }
            }
            catch (Exception ex)
            {
                var Qur = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة تعديل معلومات الشركة" + "','" + "Company Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
            }
        }
        public void Editdata()
        {
            try
            {
                var Qur = @"Update Cashier Set 
                            SystemName = '" + SystemName + "'," +
                           "Systemserial = '" + Systemserial + "'," +
                           "Materialname = '" + Materialname + "'," +
                           "CSR = '" + CSR + "'," +
                           "PrivateKey = '" + PrivateKey + "'," +
                           "PublicKey = '" + PublicKey + "'," +
                           "Secret = '" + Secret + "' " +
                           "Where Id = " + Id;
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
                if (CMO)
                {
                    MessageBox.Show("تم ادخال معلومات الشركة بنجاح", "معلومات الشركة");
                }
                else
                {
                    MessageBox.Show("خطأ لم يتم ادخال المعلومات", "معلومات الشركة");
                }
            }
            catch (Exception ex)
            {
                var Qur = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة تعديل معلومات الشركة" + "','" + "Company Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
            }
        }
        public void Deletedata()
        {
            try
            {
                var Qur = @"Delete From Cashier Where Id = " + Id;
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
                if (CMO)
                {
                    MessageBox.Show("تم ادخال معلومات الشركة بنجاح", "معلومات الشركة");
                }
                else
                {
                    MessageBox.Show("خطأ لم يتم ادخال المعلومات", "معلومات الشركة");
                }
            }
            catch (Exception ex)
            {
                var Qur = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة تعديل معلومات الشركة" + "','" + "Company Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
            }
        }
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT * FROM Cashier";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب الاصناف" + "','" + "Category Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
    }
}
