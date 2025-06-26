using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr.Tailor
{
    public class Tailor
    {
        public int Id { get; set; }
        public string Invoicedate { get; set; }
        public string Receiptdate { get; set; }
        public int Custid { get; set; }
        public int Invoiceid { get; set; }
        public int Userid { get; set; }
        public string note { get; set; }
    }
    public class Orders
    {
        public int Id { get; set; }
        public int Tailortype { get; set; }
        public int clothnational { get; set; }
        public int fabricid { get; set; }
        public int fabrictypeid { get; set; }
        public decimal fabriccount { get; set; }
        public int count { get; set; }
        public int empid { get; set; }
        public int ntid { get; set; }
        public int ntoid { get; set; }
        public int Receivedcount { get; set; }
        public int Remainingcount { get; set; }
        public string Status { get; set; }

    }
    public class Tailordetailes
    {
        public string Frontlength { get; set; }
        public string Backlength { get; set; }
        public string Shoulder { get; set; }
        public string Plainarmlength { get; set; }
        public string Cuffarmlength { get; set; }
        public string Plainneck { get; set; }
        public string Turnneck { get; set; }
        public string Width { get; set; }
        public string Handwidth { get; set; }
        public string Middlehand { get; set; }
        public string Headhand { get; set; }
        public string Plaincuff { get; set; }
        public string Cufflength { get; set; }
        public string Cuffwidth { get; set; }
        public string collar { get; set; }
        public string Bottomwidth { get; set; }
        public string Hide { get; set; }
        public string Mobilehider { get; set; }
        public string Middlechest { get; set; }
        public string Middleside { get; set; }
        public string Weight { get; set; }
    }
    public class Tailoroprations
    {
        public int NTId { get; set; }
        public int NTIOId { get; set; }
        public string ntailorInvoiceid { get; set; }
        public string salesInvoiceno { get; set; }
        public string salesTotalAmount { get; set; }
        public List<ListOfOrders> List;

        DataAccess DA;
        public bool AddTialor(Tailor Tail)
        {
            DA = new DataAccess();
            DA.Con = new MySqlConnection(DA.CS);
            string Qur = @"Insert into ntailor(Invoicedate,Receiptdate,Custid,Invoiceid,Userid,note)
                    value('" + Tail.Invoicedate + "','" + Tail.Receiptdate + "'," + Tail.Custid + "," + Tail.Invoiceid + "," + Tail.Userid + ",'" + Tail.note + "');";
            MySqlCommand Cmd = new MySqlCommand(Qur, DA.Con);
            DA.Con.Open();
            //var data = Cmd.ExecuteScalar();
            Cmd.ExecuteNonQuery();
            //now change the sql statment to take identity 
            Cmd.CommandText = "SELECT @@IDENTITY";
            int id = Convert.ToInt32(Cmd.ExecuteScalar());
            if (id.ToString() != "")
            {
                Tail.Id = id;
                NTId = Tail.Id;
                DA.Con.Close();
                return true;
            }
            else
            {
                DA.Con.Close();
                return false;
            }

        }
        public bool AddTialdetail(Tailordetailes Td)
        {
            DA = new DataAccess();
            DA.Con = new MySqlConnection(DA.CS);
            string Qur = @"Insert into ntailordetailes(Frontlength,Backlength,Shoulder,Plainarmlength,
                                               Cuffarmlength,Plainneck,Turnneck,Width,Handwidth,
                                               Middlehand,Headhand,Plaincuff,Cufflength,Cuffwidth,
                                               collar,Bottomwidth,Hide,Mobilehider,Middlechest,Middleside,Weight)
                           value('" + Td.Frontlength + "','"+ Td.Backlength + "','"+ Td.Shoulder +"','" + Td.Plainarmlength +"', "+
                                "'"+Td.Cuffarmlength +"','"+ Td.Plainneck +"','"+ Td.Turnneck +"','"+ Td.Width +"','"+ Td.Handwidth + "',"+
                                "'"+ Td.Middlehand+"','"+ Td.Headhand+"','"+ Td.Plaincuff+"','"+ Td.Cufflength+"','"+ Td.Cuffwidth +"',"+
                                "'"+ Td.collar+"','"+ Td.Bottomwidth+"','"+ Td.Hide+"','" + Td.Mobilehider+"','"+ Td.Middlechest + "','" + Td.Middleside + "','" + Td.Weight + "');";
            MySqlCommand Cmd = new MySqlCommand(Qur, DA.Con);
            DA.Con.Open();
            //var data = Cmd.ExecuteScalar();
            Cmd.ExecuteNonQuery();
            //now change the sql statment to take identity 
            Cmd.CommandText = "SELECT @@IDENTITY";
            int id = Convert.ToInt32(Cmd.ExecuteScalar());
            if (id.ToString() != "")
            {
                NTIOId = id;
                DA.Con.Close();
                return true;
            }
            else
            {
                DA.Con.Close();
                return false;
            }

        }
        public bool AddOrders(Orders Ord)
        {
            DA = new DataAccess();
            string Qur = @"Insert into orders(Tailortype,clothnational,fabricid,fabrictypeid,fabriccount,count,empid,ntid,ntoid,Receivedcount,Remainingcount,status) 
                    value(" + Ord.Tailortype+ ","+Ord.clothnational + ","+Ord.fabricid+ ","+Ord.fabrictypeid+ ","+Ord.fabriccount + ","+Ord.count+","+Ord.empid+","+ NTId + ","+NTIOId+","+Ord.Receivedcount+","+Ord.Remainingcount+",'"+Ord.Status+"')";
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
        public bool EditOrders(Orders Ord)
        {
            DA = new DataAccess();
            string Qur = @"Update orders Set 
                           Tailortype = " + Ord.Tailortype + ", " +
                          "clothnational = " + Ord.clothnational + ", " +
                          "fabricid = " + Ord.fabricid + "," +
                          "fabrictypeid = " + Ord.fabrictypeid + "," +
                          "fabriccount = " + Ord.fabriccount + "," +
                          "count = " + Ord.count + "," +
                          "empid = " + Ord.empid + "," +
                          "ntid = " + NTId + "," +
                          "ntoid = " + NTIOId + " ," +
                          "Receivedcount = " + Ord.Receivedcount + "," +
                          "Remainingcount = " + Ord.Remainingcount + "," +
                          "status =  '" + Ord.Status + "' Where Id = " + Ord.Id ;
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
        public bool DeleteOrders(string Qur) {
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
        public void Selectdata(int id)
        {
            DA = new DataAccess();
            DA.Con = new MySqlConnection(DA.CS);
            DA.Con.Open();
            var Qur = @"SELECT * FROM orders
                        Left outer Join ntailor
                        on orders.ntid = ntailor.Id
                        Left outer Join ntailordetailes
                        on orders.ntoid = ntailordetailes.Id
                        Where orders.Id = " + id;
            MySqlCommand Cmd = new MySqlCommand(Qur, DA.Con);
            var data = Cmd.ExecuteReader();
            List = new List<ListOfOrders>();
            while (data.Read()) {
                var Id = data[0].ToString();
                var Tailortype = data[1].ToString();
                var clothnational = data[2].ToString();
                var fabricid = data[3].ToString();
                var fabrictypeid = data[4].ToString();
                var fabriccount = data[5].ToString();
                var count = data[6].ToString();
                var empid = data[7].ToString();
                var ntid = data[8].ToString();
                var ntoid = data[9].ToString();
                var Invoicedate = data[14].ToString();
                var Receiptdate = data[15].ToString();
                var Custid = data[16].ToString();
                var Invoiceid = data[17].ToString();
                var Userid = data[18].ToString();
                var note = data[19].ToString();
                var Frontlength = data[21].ToString();
                var Backlength = data[22].ToString();
                var Shoulder = data[23].ToString();
                var Plainarmlength = data[24].ToString();
                var Cuffarmlength = data[25].ToString();
                var Plainneck = data[26].ToString();
                var Turnneck = data[27].ToString();
                var Width = data[28].ToString();
                var Handwidth = data[29].ToString();
                var Middlehand = data[30].ToString();
                var Headhand = data[31].ToString();
                var Plaincuff = data[32].ToString();
                var Cufflength = data[33].ToString();
                var Cuffwidth = data[34].ToString();
                var collar = data[35].ToString();
                var Bottomwidth = data[36].ToString();
                var Hide = data[37].ToString();
                var Mobilehider = data[38].ToString();
                var Middlechest = data[39].ToString();
                var Middleside = data[40].ToString();
                var Weight = data[41].ToString();
                var Receivedcount = data[10].ToString();
                var Remainingcount = data[11].ToString();
                var Status = data[12].ToString();
                List.Add(new ListOfOrders
                {
                    Id = Id,
                    Tailortype = Tailortype,
                    clothnational = clothnational,
                    fabricid = fabricid,
                    fabrictypeid = fabrictypeid,
                    fabriccount = fabriccount,
                    count = count,
                    empid = empid,
                    ntid = ntid,
                    ntoid = ntoid,
                    Invoicedate = Invoicedate,
                    Receiptdate = Receiptdate,
                    Custid = Custid,
                    Invoiceid = Invoiceid,
                    Userid = Userid,
                    note = note,
                    Frontlength = Frontlength,
                    Backlength = Backlength,
                    Shoulder = Shoulder,
                    Plainarmlength = Plainarmlength,
                    Cuffarmlength = Cuffarmlength,
                    Plainneck = Plainneck,
                    Turnneck = Turnneck,
                    Width = Width,
                    Handwidth = Handwidth,
                    Middlehand  = Middlehand,
                    Headhand = Headhand,
                    Plaincuff = Plaincuff,
                    Cufflength = Cufflength,
                    Cuffwidth = Cuffwidth,
                    collar = collar,
                    Bottomwidth = Bottomwidth,
                    Hide = Hide,
                    Mobilehider = Mobilehider,
                    Middlechest = Middlechest,
                    Middleside = Middleside,
                    Receivedcount = Receivedcount,
                    Remainingcount = Remainingcount,
                    Status = Status,
                    Weight = Weight
                });
            }
            DA.Con.Close();
        }
        public void Getdata()
        {
            DA = new DataAccess();
            DA.Con = new MySqlConnection(DA.CS);
            DA.Con.Open();
            var Qur = @"SELECT 
                        ntailor.Invoiceid,
                        orders.Id,
                        thirdparty.Name,
                        orders.count,
                        ntailor.Invoicedate,
                        ntailor.Receiptdate
                        FROM orders
                        Left outer Join ntailor
                        on orders.ntid = ntailor.Id
                        Left outer Join ntailordetailes
                        on orders.ntoid = ntailordetailes.Id
                        Left Outer Join thirdparty
                        on ntailor.Custid = thirdparty.ID";
            MySqlCommand Cmd = new MySqlCommand(Qur, DA.Con);
            var data = Cmd.ExecuteReader();
            List = new List<ListOfOrders>();
            while (data.Read())
            {
                var Invoiceid = data[0].ToString();
                var ordernum = data[1].ToString();
                var Custname = data[2].ToString();
                var count = data[3].ToString();
                var Invoicedate = data[4].ToString();
                var Receiptdate = data[5].ToString();
                List.Add(new ListOfOrders
                {
                    Invoiceid = Invoiceid,
                    ntoid = ordernum,
                    Custid = Custname,
                    count = count,
                    Invoicedate = Invoicedate,
                    Receiptdate = Receiptdate,
                });
            }
            DA.Con.Close();
        }
        public bool Editdata(string Qur)
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
        public void Selectdatabysearch(string invid,string orderid,string DTF, string DTT, string Date, string Cust)
        {
            DA = new DataAccess();
            DA.Con = new MySqlConnection(DA.CS);
            DA.Con.Open();
            var Qur = @"SELECT 
                        ntailor.Invoiceid,
                        orders.Id,
                        thirdparty.Name,
                        orders.count,
                        ntailor.Invoicedate,
                        ntailor.Receiptdate
                        FROM orders
                        Left outer Join ntailor
                        on orders.ntid = ntailor.Id
                        Left outer Join ntailordetailes
                        on orders.ntoid = ntailordetailes.Id
                        Left Outer Join thirdparty
                        on ntailor.Custid = thirdparty.ID ";
            var Flag = false;
            if (invid != "" || orderid != "" || Date != "") {
                Qur += "Where ";
            }
            if (invid != "")
            {
                Qur += @"ntailor.Invoiceid = " + int.Parse(invid);
                Flag = true;
            }
            if (orderid != "")
            {
                if (Flag)
                {
                    Qur += "And ";
                    Flag = true;
                }
                Qur += @"orders.Id = " + int.Parse(orderid);
            }
            if (Cust != "")
            {
                if (Flag)
                {
                    Qur += "And ";
                    Flag = true;
                }
                Qur += @"thirdparty.ID = " + int.Parse(Cust);
            }
            if (Date == "Tailor")
            {
                if (Flag)
                {
                    Qur += "And ";
                    Flag = true;
                }
                Qur += @"ntailor.Receiptdate >= " + DTF + "And ntailor.Receiptdate <= " + DTT;
            }
            if (Date == "Invoice")
            {
                if (Flag)
                {
                    Qur += "And ";
                    Flag = true;
                }
                Qur += @"Invoicedate.Receiptdate >= " + DTF + "And Invoicedate.Receiptdate <= " + DTT;
            }
            MySqlCommand Cmd = new MySqlCommand(Qur, DA.Con);
            var data = Cmd.ExecuteReader();
            List = new List<ListOfOrders>();
            while (data.Read())
            {
                var Invoiceid = data[0].ToString();
                var ordernum = data[1].ToString();
                var Custname = data[2].ToString();
                var count = data[3].ToString();
                var Invoicedate = data[4].ToString();
                var Receiptdate = data[5].ToString();
                List.Add(new ListOfOrders
                {
                    Invoiceid = Invoiceid,
                    ntoid = ordernum,
                    Custid = Custname,
                    count = count,
                    Invoicedate = Invoicedate,
                    Receiptdate = Receiptdate,
                });
            }
            DA.Con.Close();
        }
        public DataTable GetorderCombodata(int id)
        {
            try
            {
                var Qur = @"SELECT 
                        orders.Id
                        FROM orders
                        Left outer Join ntailor
                        on orders.ntid = ntailor.Id
                        Left outer Join ntailordetailes
                        on orders.ntoid = ntailordetailes.Id
                        Left Outer Join thirdparty
                        on ntailor.Custid = thirdparty.ID Where thirdparty.ID = " + id;
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
        public void Getinvoicedata(int id)
        {
            try
            {
                DA = new DataAccess();
                DA.Con = new MySqlConnection(DA.CS);
                DA.Con.Open();
                var Qur = @"SELECT 
                            ntailor.Invoiceid,
                            sales.Invoiceno,
                            sales.TotalAmount
                            FROM ntailor
                            Left Outer Join Sales
                            on ntailor.Invoiceid = sales.Invoiceno
                            Left outer Join Orders
                            on Orders.ntid = ntailor.Id
                            Where orders.Id = " + id;
                MySqlCommand Cmd = new MySqlCommand(Qur, DA.Con);
                var data = Cmd.ExecuteReader();
                if (data.Read())
                {
                    ntailorInvoiceid = data[0].ToString();
                    salesInvoiceno = data[1].ToString();
                    salesTotalAmount = data[2].ToString();
                }
                DA.Con.Close();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب الاصناف" + "','" + "Category Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }

    }
}
