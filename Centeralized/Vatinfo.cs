using Dataaccesslaeyr;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Centeralized
{
    public class Vatinfo
    {
        public int VATID { get; set; }
        public double VatPercent { get; set; }
        public bool PricesWithVAT { get; set; }
        public bool ISVAT { get; set; }
        public bool ISDiscforpro { get; set; }
        public bool ISDiscafterpro { get; set; }

        DataAccess DA;
        public void GetVat()
        {
            try
            {
                var Qur = @"SELECT VATID,VatPercent,PricesWithVAT, ISVAT,ISDiscforpro,ISDiscafterpro FROM Vatsetting";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD!= null)
                {
                    VATID = int.Parse(GD[0].ToString());
                    VatPercent = double.Parse(GD[1].ToString());
                    var PWV = GD[2].ToString();
                    if (PWV == "1")
                    {
                        PricesWithVAT = true;
                    }
                    else
                    {
                        PricesWithVAT = false;
                    }
                    var IV = GD[3].ToString();
                    if (IV == "1")
                    {
                        ISVAT = true;
                    }
                    else
                    {
                        ISVAT = false;
                    }

                    var DFP = GD[4].ToString();
                    if (DFP == "1")
                    {
                        ISDiscforpro = true;
                    }
                    else
                    {
                        ISDiscforpro = false;
                    }

                    var DFb = GD[5].ToString();
                    if (DFb == "1")
                    {
                        ISDiscafterpro = true;
                    }
                    else
                    {
                        ISDiscafterpro = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المبيعات جلب الضريبة" + "','" + "Sales Get VAT INFO" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Savedata()
        {
            try
            {
                var Qur = @"Update Vatsetting Set 
                            VatPercent = " + VatPercent + "," +
                           "PricesWithVAT = " + PricesWithVAT + "," +
                           "ISDiscforpro = " + ISDiscforpro + ", " +
                           "ISDiscafterpro = " + ISDiscafterpro + ", " +
                           "ISVAT = " + ISVAT + " " +
                           "Where VATID = " + VATID;
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
                if (CMO)
                {
                    MessageBox.Show("تم ادخال معلومات الضريبة بنجاح", "معلومات الضريبة");
                }
                else
                {
                    MessageBox.Show("خطأ لم يتم ادخال المعلومات", "معلومات الشركة");
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة المبيعات جلب الضريبة" + "','" + "Save Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
