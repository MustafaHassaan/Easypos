using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Querylaeyr.Tailor
{
    public class Tailormodel
    {
        public int ID { get; set; }
        public int Custid { get; set; }
        public string Forentlength { get; set; }
        public string Backlength { get; set; }
        public string Neck { get; set; }
        public string Handwidthup { get; set; }
        public string Handwidthdowen { get; set; }
        public string Shouledr { get; set; }
        public string Chest { get; set; }
        public string Dowenwidth { get; set; }
        public string Handlength { get; set; }
        public string Width { get; set; }
        public string Clothestotal { get; set; }
        public string Handdetailes { get; set; }
        public string buttonhole { get; set; }
        public string Seelve { get; set; }
        public string Seelvea { get; set; }
        public string Seelveb { get; set; }
        public string NeckPuerdetailes { get; set; }
        public string NPDtot { get; set; }
        public string NPDA { get; set; }
        public string NPDB { get; set; }
        public string Neckdetailes { get; set; }
        public string NDtot { get; set; }
        public string NDA { get; set; }
        public string NDB { get; set; }
        public string NDO { get; set; }
        public string NDC { get; set; }
        public bool Rent { get; set; }
        public string Fabricname { get; set; }
        public string Tottal { get; set; }
        public string Payed { get; set; }
        public string Creditoer { get; set; }
        public string Receiptdate { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Palmhand { get; set; }
        public string Showentriangle { get; set; }
        public string Showenbox { get; set; }
        public string Hiddentriangle { get; set; }
        public string Hiddenbox { get; set; }
        public string Statustype { get; set; }
        public string Datewm { get; set; }
        public bool WM { get; set; }
        public string KG { get; set; }
        
        DataAccess DA;
        public DataTable Getdata()
        {
            try
            {
                var Qur = @"SELECT 
                        tailor.ID,
                        tailor.Forentlength,
                        tailor.Backlength,
                        tailor.Neck,
                        tailor.Handwidthup,
                        tailor.Handwidthdowen,
                        tailor.Shouledr,
                        tailor.Chest,
                        tailor.Dowenwidth,
                        tailor.Handlength,
                        tailor.Width,
                        tailor.Clothestotal,
                        tailor.Handdetailes,
                        tailor.buttonhole,
                        tailor.Seelve,
                        tailor.Seelvea,
                        tailor.Seelveb,
                        tailor.NeckPuerdetailes,
                        tailor.NPDtot,
                        tailor.NPDA,
                        tailor.NPDB,
                        tailor.Neckdetailes,
                        tailor.NDtot,
                        tailor.NDA,
                        tailor.NDB,
                        tailor.NDO,
                        tailor.NDC,
                        tailor.Rent,
                        tailor.Fabricname,
                        tailor.Tottal,
                        tailor.Payed,
                        tailor.Creditoer,
                        tailor.Receiptdate,
                        tailor.Date,
                        thirdparty.Name,
                        tailor.Status,
                        tailor.Note,
                        tailor.Palmhand,
                        tailor.Showentriangle,
                        tailor.Showenbox,
                        tailor.Hiddentriangle,
                        tailor.Hiddenbox,
                        tailor.Statustype,
                        tailor.KG
                        FROM tailor Left Outer Join thirdparty
                        On thirdparty.Id = tailor.Custid";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب بيانات التفصيل" + "','" + "Getdata" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
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
