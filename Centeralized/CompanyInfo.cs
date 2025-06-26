using Dataaccesslaeyr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Centeralized
{
    public class CompanyInfo
    {
        public string CompanyID { get; set; }
        public string CName { get; set; }
        public string CENName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Taxnumber { get; set; }
        public string CRN { get; set; }
        public string Logo { get; set; }
        public string Logoname { get; set; }
        public string LogoENName { get; set; }
        public string Printername { get; set; }
        public string Printertype { get; set; }
        public string Sysnametype { get; set; }
        public bool ISUSEBarcode { get; set; }
        public bool ISUSEPrinter { get; set; }
        public bool ISUSEResturant { get; set; }
        public bool ISUSElineproduction { get; set; }
        public string Systemlang { get; set; }
        public bool ISUsePhase2 { get; set; }
        public string street { get; set; }
        public string buildingnumber { get; set; }
        public string cityname { get; set; }
        public string citysubdiv { get; set; }
        public string postalzone { get; set; }
        public string organizationUnitName { get; set; }
        public string organizationName { get; set; }
        public string countryName { get; set; }
        public string location { get; set; }
        public string industry { get; set; }

        DataAccess DA;
        public void GetCompany()
        {
            try
            {
                var Qur = @"SELECT CompanyID,Name,ENName, Address, PhoneNo, Taxnumber,CRN,CompanyLogo,Logoname,LogoENName,Printername,Printertype,Sysnametype,ISUSEBarcode,ISUSEPrinter,ISUSEResturant,ISUSElineproduction,Systemlang,ISUsePhase2,street,buildingnumber,cityname,citysubdiv,postalzone,organizationUnitName,organizationName,countryName,location,industry FROM Company";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD!= null)
                {
                    CompanyID = GD[0].ToString();
                    CName = GD[1].ToString();
                    CENName = GD[2].ToString();
                    Address = GD[3].ToString();
                    PhoneNo = GD[4].ToString();
                    Taxnumber = GD[5].ToString();
                    CRN = GD[6].ToString();
                    Logo = GD[7].ToString();
                    Logoname = GD[8].ToString();
                    LogoENName = GD[9].ToString();
                    Printername = GD[10].ToString();
                    Printertype = GD[11].ToString();
                    Sysnametype = GD[12].ToString();
                    ISUSEBarcode = Convert.ToBoolean(GD[13]);
                    ISUSEPrinter = Convert.ToBoolean(GD[14]);
                    ISUSEResturant = Convert.ToBoolean(GD[15]);
                    ISUSElineproduction = Convert.ToBoolean(GD[16]);
                    Systemlang = GD[17].ToString();
                    ISUsePhase2 = Convert.ToBoolean(GD[18]);
                    street = GD[19].ToString();
                    buildingnumber = GD[20].ToString();
                    cityname = GD[21].ToString();
                    citysubdiv = GD[22].ToString();
                    postalzone = GD[23].ToString();
                    organizationUnitName = GD[24].ToString();
                    organizationName = GD[25].ToString();
                    countryName = GD[26].ToString();
                    location = GD[27].ToString();
                    industry = GD[28].ToString();
                }
            }
            catch (Exception ex)
            {
                var Qur = @"Insert Into Exception(Exceptionname,Form,Note) value('"+"شاشة ادخال معلومات الشركة"+"','"+ "Company Data"+"','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
            }
        }
        public void Savedata()
        {
            try
            {
                var Qur = @"Update Company Set 
                            Name = '"+ CName +"',"+
                           "ENName = '"+ CENName +"',"+
                           "Address = '"+ Address +"',"+  
                           "PhoneNo = '"+ PhoneNo +"',"+
                           "Taxnumber = '"+ Taxnumber +"',"+
                           "CRN = '"+ CRN +"',"+
                           "CompanyLogo = '"+ Logo + "',"+
                           "Logoname = '"+ Logoname + "',"+
                           "LogoENName = '"+ LogoENName + "',"+
                           "Printername = '"+ Printername + "',"+
                           "Printertype = '"+ Printertype + "',"+
                           "Sysnametype = '"+ Sysnametype + "',"+
                           "ISUSEBarcode = "+ ISUSEBarcode + ","+
                           "ISUSEPrinter = "+ ISUSEPrinter + ", "+
                           "ISUSEResturant = " + ISUSEResturant + ", "+
                           "ISUSElineproduction = " + ISUSElineproduction + ", "+
                           "Systemlang = '" + Systemlang + "', "+
                           "ISUsePhase2 = " + ISUsePhase2 + ", "+
                           "street = '" + street + "', "+
                           "buildingnumber = '" + buildingnumber + "', "+
                           "cityname = '" + cityname + "', "+
                           "citysubdiv = '" + citysubdiv + "', "+
                           "postalzone = '" + postalzone + "', "+
                           "organizationUnitName = '" + organizationUnitName + "', "+
                           "organizationName = '" + organizationName + "', "+
                           "countryName = '" + countryName + "', "+
                           "location = '" + location + "', "+
                           "industry = '" + industry + "' "+
                           "Where CompanyID = " + CompanyID ;
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
                if (CMO)
                {
                    MessageBox.Show("تم ادخال معلومات الشركة بنجاح","معلومات الشركة");
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

    }
}
