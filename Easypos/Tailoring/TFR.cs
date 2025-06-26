using Centeralized;
using Dataaccesslaeyr;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Reporting;
using Reporting.Tailor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Easypos.Tailoring
{
    public partial class TFR : Form
    {
        Centergetingfunctions CF;
        CompanyInfo Comp;
        DataAccess DA;
        byte[] Logo;
        public TFR()
        {
            InitializeComponent();
            DA = new DataAccess();
            CF = new Centergetingfunctions();
            Comp = new CompanyInfo();
            Getcustomers();
            Comp.GetCompany();
            Logo = Convert.FromBase64String(Comp.Logo);
        }
        public void Getcustomers()
        {
            try
            {
                var GD = CF.Getthirdparties();
                DataRow dr = GD.NewRow();
                dr["ID"] = 0;
                dr["Name"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                clientID.DataSource = GD;
                clientID.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة تقرير الثياب" + "','" + "Getcustomers" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        private void TFR_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label9.Enabled = true;
                dateTimePicker1.Enabled = true;
                label1.Enabled = true;
                dateTimePicker2.Enabled = true;
            }
            else
            {
                label9.Enabled = false;
                dateTimePicker1.Enabled = false;
                label1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            CRAll CRA = new CRAll();
            Frmreporting FCCR = new Frmreporting();
            Dataset ds = new Dataset();
            var Qur = @"SELECT 
                        ntailor.Id,
                        ntailor.Invoicedate,
                        ntailor.Receiptdate,
                        orders.count,
                        orders.Receivedcount,
                        orders.Remainingcount,
                        Sales.TotalAmount,
                        payment.Paid,
                        payment.Remaining,
                        thirdparty.Name,
                        thirdparty.MobileNumber
                        From ntailor 
                        Left Outer Join thirdparty
                        On thirdparty.ID = ntailor.Custid
						Left Outer Join orders
                        On orders.ntid = ntailor.Id
                        Left Outer Join Sales
                        On Sales.Invoiceno = ntailor.Invoiceid
                        Left Outer Join payment
                        On payment.InvoiceNo = Sales.Invoiceno";
            var flag = false;
            if (checkBox1.Checked)
            {
                flag = true;
                if (flag) {
                    Qur += " Where ";
                }
                Qur += "ntailor.Invoicedate >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' And ntailor.Invoicedate <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' ";
            }
            if (checkBox2.Checked)
            {
                if (flag)
                {
                    Qur += " And ";
                }
                else
                {
                    flag = true;
                    Qur += " Where ";
                }
                Qur += "orders.Remainingcount > 0  ";
            }
            if (clientID.Text != "--اختر--")
            {
                if (flag)
                {
                    Qur += " And ";
                }
                else
                {
                    flag = true;
                    Qur += " Where ";
                }
                Qur += "ntailor.Custid = '" + clientID.SelectedValue + "' ";
            }
            DA.Con = new MySqlConnection(DA.CS);
            DA.Con.Open();
            DA.Cmd = new MySqlCommand(Qur, DA.Con);
            DA.dr = DA.Cmd.ExecuteReader();
            while (DA.dr.Read())
            {
                var Id = DA.dr["Id"].ToString();
                var Date = DA.dr["Invoicedate"].ToString();
                var Receiptdate = DA.dr["Receiptdate"].ToString();
                var Clothestotal = DA.dr["count"].ToString();
                var Status = DA.dr["Receivedcount"].ToString();
                var Residual = DA.dr["Remainingcount"].ToString();
                var Custname = DA.dr["Name"].ToString();
                var Custphone = DA.dr["MobileNumber"].ToString();
                var Total = DA.dr["TotalAmount"].ToString();
                if (Total == "")
                {
                    Total = "0";
                }
                var Paied = DA.dr["Paid"].ToString();
                if (Paied == "")
                {
                    Paied = "0";
                }
                var Credit = DA.dr["Remaining"].ToString();
                if (Credit == "")
                {
                    Credit = "0";
                }
                ds.Closes.Rows.Add(new object[] { Id, Date, Receiptdate, Clothestotal, Status, Residual, Custname, Custphone, Total, Paied, Credit, Logo });
            }
            if (!DA.dr.Read())
            {
                ds.Closes.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null, null, null, Logo });
            }
            DA.Con.Close();
            CRA.SetDataSource(ds);

            CRA.SetParameterValue("CompanyName", Comp.CName);
            CRA.SetParameterValue("Address", Comp.Address);
            CRA.SetParameterValue("PhoneNo", Comp.PhoneNo);
            CRA.SetParameterValue("MobileNo", Comp.PhoneNo);
            CRA.SetParameterValue("Taxnum", Comp.Taxnumber);
            CRA.SetParameterValue("Proname", Comp.CRN);
            CRA.SetParameterValue("English_Shop_name", Comp.CENName);
            FCCR.CRV.ReportSource = CRA;
            FCCR.CRV.Refresh();
            FCCR.Show();
        }
    }
}
