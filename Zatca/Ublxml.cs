using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dataaccesslaeyr;

namespace Zatca
{
    public class Ublxml
    {
        public int Id { get; set; }
        public int Salid { get; set; }
        public string Invhash { get; set; }


        DataAccess DA;
        public void Getdatabyid(int id)
        {
            try
            {
                var Qur = @"SELECT * FROM Ublxml Where Id = " + id;
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    Id = int.Parse(GD[0].ToString());
                    Invhash = GD[1].ToString();
                    Salid = int.Parse(GD[2].ToString());
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
                var Qur = @"Insert Into Ublxml (Invhash,Salid) value('" + Invhash + "'," + Salid + ")";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qur);
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
