using Centeralized;
using CrystalDecisions.CrystalReports.Engine;
using MetroFramework.Forms;
using Querylaeyr.Modelmaster;
using Querylaeyr.Modelsales;
using Querylaeyr.Paymodel;
using Reporting.Others;
using Reporting.Purchases;
using Reporting.Sales.Salesreportes;
using Reporting.Vouchers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reporting.Sales
{
    public partial class Salesfiltrations : MetroForm
    {
        Centergetingfunctions CF;
        AllSalesreports ASR;
        CompanyInfo Com;
        List<Salesonetax> SOT;
        List<Paymentmodel> PMN;
        byte[] Logo;
        Vatinfo VI;
        Staff Stf;
        Expencestype Expt;
        public void Getstaff()
        {
            var GD = Stf.Getusers();
            Users.DisplayMember = "Username";
            Users.ValueMember = "StaffID";
            Users.DataSource = GD;
        }
        public void Getexpt()
        {
            var GD = Expt.Getdata();
            Explist.DisplayMember = "Expencestypename";
            Explist.ValueMember = "Id";
            Explist.DataSource = GD;
        }
        public Salesfiltrations()
        {
            InitializeComponent();
            PMN = new List<Paymentmodel>();
            SOT = new List<Salesonetax>();
            CF = new Centergetingfunctions();
            ASR = new AllSalesreports();
            Com = new CompanyInfo();
            Stf = new Staff();
            Expt = new Expencestype();
            Getstaff();
            Getexpt();
            Com.GetCompany();
            Logo = Convert.FromBase64String(Com.Logo);
            VI = new Vatinfo();
            VI.GetVat();
            if (VI.ISVAT)
            {
                Chksalestaxdetailes.Enabled = true;
                Chksalestax.Enabled = true;
                Chkonetax.Enabled = true;
            }
        }
        private void TPACheacked()
        {
            ChkPurchases.Checked = false;
            ChkExpenses.Checked = false;
            ChkPayment.Checked = false;
            ChkPayout.Checked = false;
        }
        private void TPBCheacked()
        {
            Chksales.Checked = false;
            Chksalesdetailes.Checked = false;
            Chksalestaxdetailes.Checked = false;
            Chksalestax.Checked = false;
            Chkonetax.Checked = false;
            Chkuser.Checked = false;
        }
        private void Btnshowreport_Click(object sender, EventArgs e)
        {
            Frmreporting FR = new Frmreporting();
            Dataset Dsx = new Dataset();
            ReportDocument Objrpt = new ReportDocument();
            var DF = DTF.Value.ToString("yyyy-MM-dd");
            var DT = DTT.Value.ToString("yyyy-MM-dd");
            if (Chksales.Checked)
            {
                Objrpt = new AllSalesReport();
                List<Saleslist> SL = new List<Saleslist>();

                if (Chkdate.Checked)
                {
                    SL = ASR.Getsaleslist(DF, DT);
                }
                else
                {
                    SL = ASR.Getsaleslist(null, null);
                }
                foreach (var item in SL)
                {
                    Dsx.Sales.Rows.Add(new object[] {
                        item.Invoiceno,
                        item.Name,
                        item.TDate,
                        item.PaymentMethod,
                        item.NonVatTotal,
                        item.Discount,
                        item.VatAmount,
                        item.TotalAmount,
                        Logo,
                        item.Paid,
                        item.Remaining,
                    });
                }
            }
            else if (Chksalesdetailes.Checked)
            {
                Objrpt = new CRSalesrpt();
                List<Saleslist> SL = new List<Saleslist>();
                if (Chkdate.Checked)
                {
                    SL = ASR.Getsalesdetaileslist(DF, DT);
                }
                else
                {
                    SL = ASR.Getsalesdetaileslist(null, null);
                }
                foreach (var item in SL)
                {
                    Dsx.DSI.Rows.Add(new object[] {
                        item.Invoiceno,
                        item.ProductNo,
                        item.ItemPrice,
                        item.Quantity,
                        item.Total,
                        item.TDate,
                        item.TTime,
                        item.Total,
                        null,
                        item.TotalAmount,
                        item.TDDesc,
                        null,
                        Logo,
                    });
                }
            }
            else if (Chksalestaxdetailes.Checked)
            {
                Objrpt = new STPRep();
                List<Saleslist> SL = new List<Saleslist>();
                if (Chkdate.Checked)
                {
                    SL = ASR.Getsaleslist(DF, DT);
                }
                else
                {
                    SL = ASR.Getsaleslist(null, null);
                }
                foreach (var item in SL)
                {
                    Dsx.Sales.Rows.Add(new object[] {
                        item.Invoiceno,
                        item.Name,
                        item.TDate,
                        item.PaymentMethod,
                        item.NonVatTotal - item.Discount,
                        item.Discount,
                        item.VatAmount,
                        item.TotalAmount,
                        Logo,
                    });
                }
            }
            else if (Chksalestax.Checked)
            {
                Objrpt = new CRDSByVAT();
                List<Saleslist> SL = new List<Saleslist>();
                if (Chkdate.Checked)
                {
                    SL = ASR.Getsaleslist(DF, DT);
                }
                else
                {
                    SL = ASR.Getsaleslist(null, null);
                }
                foreach (var item in SL)
                {
                    Dsx.Sales.Rows.Add(new object[] {
                        item.Invoiceno,
                        item.Name,
                        item.TDate,
                        item.PaymentMethod,
                        item.NonVatTotal - item.Discount,
                        item.Discount,
                        item.VatAmount,
                        item.TotalAmount, Logo,
                    });
                }
            }
            else if (Chkonetax.Checked)
            {
                Objrpt = new CRonetax();
                if (Chkdate.Checked)
                {
                    CF.Onetax(DF, DT);
                }
                else
                {
                    CF.Onetax(null, null);
                }
                SOT = CF.SOT;
                foreach (var item in SOT)
                {
                    Dsx.Onetax.Rows.Add(new object[] {
                        item.Tot,
                        item.Tax,
                        item.Desc,
                        Logo,
                        item.Totaltax,
                        item.Totaltot
                    });
                }
            }
            else if (Chkuser.Checked)
            {
                Objrpt = new CRDSByUser();
                List<Saleslist> SL = new List<Saleslist>();
                var Sid = int.Parse(Users.SelectedValue.ToString());
                if (Chkdate.Checked)
                {
                    SL = ASR.Getsalesdetailesuser(DF, DT, Sid);
                }
                else
                {
                    SL = ASR.Getsalesdetailesuser(null, null, Sid);
                }
                foreach (var item in SL)
                {
                    Dsx.DailySalesByStaff.Rows.Add(new object[] {
                        item.ProductNo,
                        item.TDDesc,
                        item.TDate,
                        null,
                        item.ItemPrice,
                        Users.Text,
                        item.TTime,
                        item.Quantity,
                        item.Total,
                        item.Invoiceno,
                        null,
                        Logo,
                    });
                }
            }
            else if (ChkPurchases.Checked)
            {
                Objrpt = new PurchasesReport();
                List<Saleslist> SL = new List<Saleslist>();
                if (Chkdate.Checked)
                {
                    SL = ASR.Getpurchaselist(DF, DT);
                }
                else
                {
                    SL = ASR.Getpurchaselist(null, null);
                }
                foreach (var item in SL)
                {
                    Dsx.SalesPurchases.Rows.Add(new object[] {
                        item.Invoiceno,
                        item.Name,
                        item.TDate,
                        item.NonVatTotal,
                        item.Discount,
                        item.VatAmount,
                        null,
                        item.TotalAmount,
                        Logo,
                    });
                }
            }
            else if (ChkExpenses.Checked)
            {
                Objrpt = new ExpensesReport();
                List<Expense> Exp = new List<Expense>();
                if (Chkdate.Checked)
                {
                    Exp = ASR.GetExpenselist(DF, DT,null);
                }
                else if (Typeexp.Checked)
                {
                    Exp = ASR.GetExpenselist(null,null,Explist.Text);
                }
                else if (Typeexp.Checked && Chkdate.Checked)
                {
                    Exp = ASR.GetExpenselist(DF, DT, Explist.Text);
                }
                else
                {
                    Exp = ASR.GetExpenselist(null, null,null);
                }
                foreach (var item in Exp)
                {
                    Dsx.ExpensesRevenues.Rows.Add(new object[] {
                        item.ID,
                        item.Amount,
                        item.CDate,
                        item.Vat,
                        Logo,
                        item.Description,
                    });
                }
            }
            else if (ChkPayment.Checked)
            {
                Objrpt = new VoucherReport();
                List<Paymentmodel> PM = new List<Paymentmodel>();
                if (Chkdate.Checked)
                {
                    PM = ASR.GetPaymentlist(DF, DT);
                }
                else
                {
                    PM = ASR.GetPaymentlist(null, null);
                }
                foreach (var item in PM)
                {
                    Dsx.Payvochuers.Rows.Add(new object[] {
                        item.Name,
                        item.Date,
                        item.paymentNo,
                        item.Type,
                        item.Paid,
                        item.InvoiceNo,
                        Logo,
                    });
                }
            }
            else if (ChkPayout.Checked)
            {
                Objrpt = new VoucherReport();
                List<Paymentmodel> PM = new List<Paymentmodel>();
                if (Chkdate.Checked)
                {
                    PM = ASR.GetPayoutlist(DF, DT);
                }
                else
                {
                    PM = ASR.GetPayoutlist(null, null);
                }
                foreach (var item in PM)
                {
                    Dsx.Payvochuers.Rows.Add(new object[] {
                        item.Name,
                        item.Date,
                        item.paymentNo,
                        item.Type,
                        item.Paid,
                        item.InvoiceNo,
                        Logo,
                    });
                }
            }
            else
            {
                MessageBox.Show("برجاء اختيار نوع التقرير", "خطأ");
                return;
            }
            Objrpt.SetDataSource(Dsx);
            Objrpt.SetParameterValue("CompanyName", ASR.CI.CName);
            Objrpt.SetParameterValue("Address", ASR.CI.Address);
            Objrpt.SetParameterValue("Taxnum", ASR.CI.Taxnumber);
            Objrpt.SetParameterValue("PhoneNo", ASR.CI.PhoneNo);
            Objrpt.SetParameterValue("Proname", ASR.CI.CRN);
            Objrpt.SetParameterValue("English_Shop_name", ASR.CI.CENName);
            if (ChkPayout.Checked || ChkPayment.Checked || ChkExpenses.Checked || Chksales.Checked || Chksalestax.Checked || Chksalestaxdetailes.Checked || Chksalesdetailes.Checked || Chkuser.Checked || ChkPurchases.Checked)
            {
                Objrpt.SetParameterValue("SalesDate", $"من {DF} " + " " + $" إلي {DT}");
            }
            if (Chkonetax.Checked)
            {
                Objrpt.SetParameterValue("Fromdate", DF);
                Objrpt.SetParameterValue("Todate", DT);
            }
            if (ChkPayment.Checked)
            {
                Objrpt.SetParameterValue("Vouchertype", "تقرير سندات القبض حسب تاريخ");
            }
            if (ChkPayout.Checked)
            {
                Objrpt.SetParameterValue("Vouchertype", "تقرير سندات الصرف حسب تاريخ");
            }
            FR.CRV.ReportSource = Objrpt;
            FR.Show();
        }
        private void Chkdate_CheckedChanged(object sender, EventArgs e)
        {
            if (Chkdate.Checked)
            {
                label1.Visible = true;
                label2.Visible = true;
                DTF.Visible = true;
                DTT.Visible = true;
            }
            else
            {
                label1.Visible = false;
                label2.Visible = false;
                DTF.Visible = false;
                DTT.Visible = false;
            }
        }
        private void Chkuser_CheckedChanged(object sender, EventArgs e)
        {
            if (Chkuser.Checked)
            {
                Users.Visible = true;
            }
            else
            {
                Users.Visible = false;
            }
        }

        private void ChkPurchases_Click(object sender, EventArgs e)
        {
            TPBCheacked();
        }

        private void ChkExpenses_Click(object sender, EventArgs e)
        {
            TPBCheacked();
        }

        private void ChkPayment_Click(object sender, EventArgs e)
        {
            TPBCheacked();
        }

        private void ChkPayout_Click(object sender, EventArgs e)
        {
            TPBCheacked();
        }

        private void Chksales_Click(object sender, EventArgs e)
        {
            TPACheacked();
        }

        private void Chksalesdetailes_Click(object sender, EventArgs e)
        {
            TPACheacked();
        }

        private void Chksalestaxdetailes_Click(object sender, EventArgs e)
        {
            TPACheacked();
        }

        private void Chksalestax_Click(object sender, EventArgs e)
        {
            TPACheacked();
        }

        private void Chkonetax_Click(object sender, EventArgs e)
        {
            TPACheacked();
        }

        private void Chkuser_Click(object sender, EventArgs e)
        {
            TPACheacked();
        }

        private void ChkExpenses_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkExpenses.Checked)
            {
                Explist.Visible = true;
                Typeexp.Visible = true;
            }
            else
            {
                Explist.Visible = false;
                Typeexp.Visible = false;

            }
        }
    }
}
