using Centeralized;
using CrystalDecisions.CrystalReports.Engine;
using Dataaccesslaeyr;
using MetroFramework.Forms;
using MySql.Data.MySqlClient;
using Reporting;
using Reporting.Resturantreports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resturantlayer
{
    public partial class Reportrestfiltrations : MetroForm
    {
        ReportDocument RD;
        Dataset Ds;
        DataAccess DA;
        Centergetingfunctions CF;
        CompanyInfo Comp;
        MySqlConnection Con;
        MySqlCommand Cmd;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        public Reportrestfiltrations()
        {
            InitializeComponent();
            CF = new Centergetingfunctions();
            DA = new DataAccess();
            Comp = new CompanyInfo();
            Comp.GetCompany();
            RBAllsales.Checked = true;
        }
        private void Btnshowreport_Click(object sender, EventArgs e)
        {
            Frmreporting FR = new Frmreporting();
            Ds = new Dataset();
            var ReportDateFrom = DTF.Value.ToString("yyyy-MM-dd");
            var ReportTimeFrom = TTF.Value.ToString("HH:mm:ss");
            var ReportDateTo = DTT.Value.ToString("yyyy-MM-dd");
            var ReportTimeTo = TTE.Value.ToString("HH:mm:ss");
            Con = new MySqlConnection(DA.CS);
            if (RBAllsales.Checked)
            {
                string Qur = "";
                RD = new Summarysales();
                Qur += @"Select 
                         salesdetailes.ProductNo,
                         salesdetailes.TDDesc,
                         salesdetailes.ItemPrice As 'Price',
                         Sum(salesdetailes.Quantity) As 'Quantity',
                         Sum(salesdetailes.Subtotal) As 'Subtotal',
                         Sum(salesdetailes.Discount) As 'Discount',
                         Sum(salesdetailes.Totafterdiscount) As 'Totafterdiscount'
                         From salesdetailes
                         Left Outer Join sales
                         on salesdetailes.InvoiceNo = sales.Invoiceno
                         Where CONCAT(sales.TDate,' ',sales.TTime) >= CONCAT('" + ReportDateFrom + "',' ','" + ReportTimeFrom + "') " +
                        "And  CONCAT(sales.TDate,' ',sales.TTime) <= CONCAT('" + ReportDateTo + "',' ','" + ReportTimeTo + "') " +
                        " group by salesdetailes.ProductNo,salesdetailes.TDDesc,salesdetailes.ItemPrice " +
                        " order by ProductNo";
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                da = new MySqlDataAdapter(Cmd);
                da.Fill(Ds, "dtbasic");
                Con.Close();


                var Qurpay = @"SELECT 
                               Sum(payment.Remaining) As 'PChange',
                               Sum(payment.Cash) As 'Paid'
                               FROM payment
                               Left Outer Join sales
                               on payment.InvoiceNo = sales.Invoiceno " +
                             @"Where CONCAT(sales.TDate,' ',sales.TTime) >= CONCAT('" + ReportDateFrom + "',' ','" + ReportTimeFrom + "') " +
                              "And  CONCAT(sales.TDate,' ',sales.TTime) <= CONCAT('" + ReportDateTo + "',' ','" + ReportTimeTo + "') ";
                Con.Open();
                Cmd = new MySqlCommand(Qurpay, Con);
                da = new MySqlDataAdapter(Cmd);
                da.Fill(Ds, "Dtpay");
                Con.Close();

                var Qurcredit = @"SELECT 
                                   Sum(payment.Bank) As 'Amount'
                                   FROM payment
                                   Where CONCAT(payment.Date,' ',payment.Time) >= CONCAT('" + ReportDateFrom + "',' ','" + ReportTimeFrom + "') " +
                                  "And  CONCAT(payment.Date,' ',payment.Time) <= CONCAT('" + ReportDateTo + "',' ','" + ReportTimeTo + "') ";
                Con.Open();
                Cmd = new MySqlCommand(Qurcredit, Con);
                da = new MySqlDataAdapter(Cmd);
                da.Fill(Ds, "Banktrans");
                Con.Close();

                var Qurinvdet = @"Select 
                                    sales.Discount,
                                    salesdetailes.ProductNo as 'productnumber',
                                    salesdetailes.TDDesc as 'description',
                                    salesdetailes.ItemPrice As 'price',
                                    salesdetailes.Quantity As 'quantity',
                                    salesdetailes.Total As 'total',
                                    sales.TotalAmount As totaldet,
                                    thirdparty.Name as 'customername',
                                    salesdetailes.InvoiceNo As 'invoiceid',
                                    (Select Sum(Paid) From payment Where InvoiceNo = sales.InvoiceNo) AS 'ReceivedAmount'
                                    From salesdetailes
                                    Left Outer Join sales
                                    on salesdetailes.InvoiceNo = sales.Invoiceno
                                    Left Outer Join payment
                                    on payment.InvoiceNo = sales.Invoiceno
                                    Left Outer Join thirdparty
                                    on sales.ThirdPartyID = thirdparty.ID
                                    Where sales.Billtype = 'صدرت' And Paid >= 0.00 And Remaining != 0.00 " +
                                @"And CONCAT(sales.TDate,' ',sales.TTime) >= CONCAT('" + ReportDateFrom + "',' ','" + ReportTimeFrom + "') " +
                                 "And CONCAT(sales.TDate,' ',sales.TTime) <= CONCAT('" + ReportDateTo + "',' ','" + ReportTimeTo + "') " +
                                 @"  Group by sales.Discount,
								    salesdetailes.ProductNo,
                                    salesdetailes.TDDesc,
                                    salesdetailes.ItemPrice,
                                    salesdetailes.Quantity,
                                    salesdetailes.Total,
                                    thirdparty.Name,
                                    salesdetailes.InvoiceNo ";
                Con.Open();
                Cmd = new MySqlCommand(Qurinvdet, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    var RA = GD["ReceivedAmount"].ToString();
                    if (RA == "")
                    {
                        RA = "0";
                    }
                    var Paid = Convert.ToDecimal(RA);
                    var Currentamount = Convert.ToDecimal(GD["totaldet"].ToString());
                    var Discount = GD["Discount"].ToString();
                    var productnumber = GD["productnumber"].ToString();
                    var description = GD["description"].ToString();
                    var price = GD["price"].ToString();
                    var quantity = GD["quantity"].ToString();
                    var total = GD["total"].ToString();
                    var invoiceid = GD["invoiceid"].ToString();
                    var totaldet = GD["totaldet"].ToString();
                    var customername = GD["customername"].ToString();
                    var ReceivedAmount = GD["ReceivedAmount"].ToString();
                    if (Paid < Currentamount)
                    {
                        Ds.invoicedetailes.Rows.Add(new object[]
                        {
                        Discount,productnumber,description,price,quantity,total,invoiceid,totaldet,customername,ReceivedAmount
                        });
                    }

                }
            }
            else if (RBItemsales.Checked)
            {
                RD = new Summaryitems();
                var Qur = @"Select 
                     items.ID,
                     Items.Itemname,
                     UnitTypes.UnitType,
                     (productitems.Quantity + items.itemqty) As 'QBD',
                     items.itemqty,
                     Sum(productitems.Quantity) As 'Quantity',
                     Items.Itemprice,
                     sum(productitems.Quantity * Items.Itemprice) As 'Total'
                     From sales
                     Left Outer Join salesdetailes
                     on salesdetailes.InvoiceNo = sales.InvoiceNo
                     Left Outer Join productitems
                     on productitems.Proid = salesdetailes.ProductNo
                     Left Outer Join Items
                     on Items.ID = productitems.itemid
                     Left Outer Join unittypes
                     on Items.UID = unittypes.ID
                     Where items.Itemname Is Not Null And 
                     CONCAT(sales.TDate,' ',sales.TTime) >= CONCAT('" + ReportDateFrom + "',' ','" + ReportTimeFrom + "') " +
                    " And  CONCAT(sales.TDate,' ',sales.TTime) <= CONCAT('" + ReportDateTo + "',' ','" + ReportTimeTo + "') " +
                    " Group By items.ID,items.Itemname,Items.Itemprice,QBD " +
                    " Order by items.ID ";
                Con = new MySqlConnection(DA.CS);
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                var GD = dr;
                while (GD.Read())
                {
                    var ID = GD[0].ToString();
                    var Itemname = GD[1].ToString();
                    var UnitType = GD[2].ToString();
                    var itemqty = GD[3].ToString();
                    var QBD = GD[4].ToString();
                    var Quantity = GD[5].ToString();
                    var Itemprice = GD[6].ToString();
                    var Total = GD[7].ToString();
                    Ds.Productitems.Rows.Add(new object[] {
                        Itemname,
                        UnitType,
                        itemqty,
                        Quantity,
                        Itemprice,
                        Total,
                        null,
                        QBD
                    });
                }
            }
            else
            {
                MessageBox.Show("برجاء اختيار نوع التقرير", "خطأ");
                return;
            }
            RD.SetDataSource(Ds);
            RD.SetParameterValue("SalesDate", $"من {ReportDateFrom.ToString()} " + " " + $" إلي {ReportDateTo.ToString()}");
            RD.SetParameterValue("English_Shop_name", Comp.CENName);
            RD.SetParameterValue("CompanyName", Comp.CName);
            FR.CRV.ReportSource = RD;
            FR.Show();
        }
        private void Reportrestfiltrations_Load(object sender, EventArgs e)
        {
            var DS = DTF.Value.ToString("yyyy-MM-dd");
            var De = DTT.Value.AddDays(1).ToString("yyyy-MM-dd");
            DTF.Text = DS;
            DTT.Text = De;
            var TS = TTF.Value.ToString("12:00:00");
            TTF.Text = TS;
            var TE = TTF.Value.AddHours(14);
            TTE.Text = TE.ToString();
        }
    }
}
