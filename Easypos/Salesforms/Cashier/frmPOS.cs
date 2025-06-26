using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using MySqlX.XDevAPI;
using Centeralized;
using Querylaeyr.Modelmaster;
using Querylaeyr.Modelsales;
using Easypos.Payment;
using Easypos.Salesforms.Normal;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Windows.Documents;
using Querylaeyr.Paymodel;
using Reporting.Sales.Normal.Big;
using Reporting;
using Reporting.Sales.Normal.Small;
using Querylaeyr.ResturantModles;
using Reporting.Tailor;
using CrystalDecisions.CrystalReports.Engine;
using Easypos.Masters;
using Dataaccesslaeyr;
using Easypos.Tailoring;
using ZXing.Common;
using ZXing;
using System.IO;
using System.Xml;
using Zatca;
using ZatcaIntegrationSDK.BLL;
using ZatcaIntegrationSDK.HelperContracts;
using ZatcaIntegrationSDK;
using ZatcaIntegrationSDK.APIHelper;
using FontAwesome.Sharp;
using System.Runtime.ConstrainedExecution;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using ZatcaIntegrationSDK.GeneralLogic;
using File = System.IO.File;

namespace Easypos.Salesforms.Cashier
{
    public partial class frmPOS : Form
    {
        private Mode mode = Mode.developer;
        public string Filename { get; set; }
        Ublxml UXML;
        Cashierinfo CSI;
        Centergetingfunctions CF;
        List<Category> CL;
        Vatinfo VI;
        Thirdparty thir;
        Sales sal;
        List<Saleslist> SL;
        Salespayments SPay;
        Contractors Cntr;
        Thirdparty Cust;
        Paymentmodel Pm;
        Itemsales IS;
        List<Itemsales> ISL;
        DataAccess Da;
        bool Diskflag = true;
        bool Totflag = true;
        public int STId { get; set; }
        public int DgvinvId { get; set; }
        public string invType { get; set; }
        public string Tailor { get; set; }
        Invoice inv;
        public frmPOS()
        {
            InitializeComponent();
            Da = new DataAccess();
            UXML = new Ublxml();
            CF = new Centergetingfunctions();
            CF.Comp = new CompanyInfo();
            CF.Comp.GetCompany();
            CL = new List<Category>();
            sal = new Sales();
            thir = new Thirdparty();
            SL = new List<Saleslist>();
            ISL = new List<Itemsales>();
            Getcustomers();
            Setcategories();
            CSI = new Cashierinfo();
            CSI.GetCashiers();
            VI = new Vatinfo();
            VI.GetVat();
            if (VI.ISDiscafterpro)
            {
                Disc.Enabled = true;
                lblTotalAmount.Enabled = true;
                DGV.Columns["Discount"].Visible = false;
            }
            if (VI.ISDiscforpro)
            {
                Disc.Enabled = false;
                lblTotalAmount.Enabled = false;
                DGV.Columns["Discount"].Visible = true;
            }
            Billtype.SelectedIndex = 1;
            clients.SelectedIndex = 1;
            IS = new Itemsales();
            if (CF.Comp.Sysnametype == "طباعة نظام مبيعات صغير خياطه")
            {
                label22.Visible = true;
                label4.Visible = true;
                dateTimePicker2.Visible = true;
                textBox3.Visible = true;
            }
            else
            {
                label22.Visible = false;
                label4.Visible = false;
                dateTimePicker2.Visible = false;
                textBox3.Visible = false;
            }
        }
        int top = 0;
        public DateTime Time { get; set; }
        public string Paid { get; set; }
        public string Dept { get; set; }
        public string Date { get; set; }
        public int Invnum { get; set; }
        public string St { get; set; }
        public string Paymethode { get; set; }
        public void Changetot()
        {
            try
            {
                Disc.Text = "0";
                LblVat.Text = "0";

                var Price = Convert.ToDouble(lblTotalAmount.Text);
                var TBV = Price / 1.15;
                var GTBV = Math.Round(Convert.ToDecimal(TBV), 2).ToString();
                if (VI.ISVAT)
                {
                    var Tax = Convert.ToDouble(GTBV) * (VI.VatPercent / 100);
                    var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
                    //var GT = Math.Round(Convert.ToDecimal(GTax), MidpointRounding.ToEven);

                    LblVat.Text = GTax.ToString();

                    var Disco = Convert.ToDouble(lblSubTotal.Text) - TBV;
                    var GDisc = Math.Round(Convert.ToDouble(Disco), 2).ToString();
                    Disc.Text = Convert.ToString(GDisc);
                }
                else
                {
                    var TB = Convert.ToDouble(lblSubTotal.Text);
                    var Tot = Convert.ToDouble(lblTotalAmount.Text);
                    var Res = TB - Tot;
                    Disc.Text = Res.ToString();
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "تغيير الاجمالي" + "','" + "Changetot" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Changedisc()
        {
            try
            {
                if (VI.ISVAT)
                {
                    //المبلغ الضريبي
                    var TAD = Convert.ToDouble(lblSubTotal.Text) - Convert.ToDouble(Disc.Text);
                    var Tax = TAD * (VI.VatPercent / 100);
                    var GTax = Math.Round(Convert.ToDecimal(Tax), 2).ToString();
                    //                var GT = Math.Round(Convert.ToDecimal(GTax), MidpointRounding.ToEven);

                    LblVat.Text = Convert.ToString(GTax);

                    //المبلغ بعد الضريبة
                    var TAV = Convert.ToDouble(LblVat.Text) + TAD;
                    lblTotalAmount.Text = Convert.ToString(TAV);
                }
                else
                {
                    var TB = Convert.ToDouble(lblSubTotal.Text);
                    var Tot = Convert.ToDouble(Disc.Text);
                    var Res = TB - Tot;
                    lblTotalAmount.Text = Res.ToString();
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "تغيير الخصم" + "','" + "Changedisc" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Generatqrcode()
        {
            try
            {
                var Tot = double.Parse(lblTotalAmount.Text);
                var Tax = double.Parse(LblVat.Text);
                Time = DateTime.Now;
                var GDtlv = CF.DQ(Tot, Tax, Time);
                QRPic.Image = GDtlv;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "انشاء QRCode" + "','" + "Generatqrcode" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public Bitmap QrCodeImage(string Qrcode, int width = 250, int height = 250)
        {

            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height
                }
            };
            Bitmap QrCode = barcodeWriter.Write(Qrcode);

            return QrCode;
        }
        private void Makeinvoice()
        {
            UBLXML ubl = new UBLXML();
            inv = new Invoice();
            ZatcaIntegrationSDK.Result res = new ZatcaIntegrationSDK.Result();
            inv.ID = Invnum.ToString();
            inv.IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
            inv.IssueTime = DateTime.Now.ToString("HH:mm:ss");
            inv.invoiceTypeCode.id = 388;
            if (CF.Comp.Sysnametype == "طباعة نظام مبيعات كبير")
            {
                inv.invoiceTypeCode.Name = "0100000";
            }
            else
            {
                inv.invoiceTypeCode.Name = "0200000";
            }
            inv.DocumentCurrencyCode = "SAR";
            inv.TaxCurrencyCode = "SAR";
            inv.CurrencyRate = 1m;
            if (inv.invoiceTypeCode.id == 383 || inv.invoiceTypeCode.id == 381)
            {
                InvoiceDocumentReference invoiceDocumentReference = new InvoiceDocumentReference();
                invoiceDocumentReference.ID = "from invoice number 500 to invoice number 505";
                inv.billingReference.invoiceDocumentReferences.Add(invoiceDocumentReference);
            }
            UXML.Getdatabyid(Invnum - 1);
            string LastInvoiceHash = "";
            if (UXML.Id == 0)
            {
                LastInvoiceHash = "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";
            }
            else
            {
                LastInvoiceHash = UXML.Invhash;
            }
            long invoicecounter = Invnum;
            inv.AdditionalDocumentReferencePIH.EmbeddedDocumentBinaryObject = LastInvoiceHash;
            inv.AdditionalDocumentReferenceICV.UUID = invoicecounter;
            if (inv.invoiceTypeCode.Name.Substring(0, 2) == "01")
            {
                inv.delivery.ActualDeliveryDate = DateTime.Now.ToString("yyyy-MM-dd");
                inv.delivery.LatestDeliveryDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string paymentcode = "1";
            if (!string.IsNullOrEmpty(paymentcode))
            {
                PaymentMeans paymentMeans = new PaymentMeans();
                paymentMeans.PaymentMeansCode = paymentcode;
                if (inv.invoiceTypeCode.id == 383 || inv.invoiceTypeCode.id == 381)
                {
                    paymentMeans.InstructionNote = "dameged items";
                }
                paymentMeans.payeefinancialaccount.ID = "";
                paymentMeans.payeefinancialaccount.paymentnote = "";
                inv.paymentmeans.Add(paymentMeans);
            }
            inv.SupplierParty.partyIdentification.ID = CF.Comp.Taxnumber;
            inv.SupplierParty.partyIdentification.schemeID = "";
            inv.SupplierParty.postalAddress.StreetName = CF.Comp.street;
            inv.SupplierParty.postalAddress.BuildingNumber = CF.Comp.buildingnumber;
            inv.SupplierParty.postalAddress.CityName = CF.Comp.cityname;
            inv.SupplierParty.postalAddress.PostalZone = CF.Comp.postalzone;
            inv.SupplierParty.postalAddress.CitySubdivisionName = CF.Comp.citysubdiv;
            inv.SupplierParty.postalAddress.country.IdentificationCode = "SA";
            inv.SupplierParty.partyLegalEntity.RegistrationName = CF.Comp.CName;
            inv.SupplierParty.partyTaxScheme.CompanyID = CF.Comp.Taxnumber;
            if (inv.invoiceTypeCode.Name.Substring(0, 2) == "01")
            {
                // بيانات المشترى
                inv.CustomerParty.partyIdentification.ID = "";
                inv.CustomerParty.partyIdentification.schemeID = "";
                inv.CustomerParty.postalAddress.StreetName = "";
                inv.CustomerParty.postalAddress.BuildingNumber = "";
                inv.CustomerParty.postalAddress.CityName = "Jeddah";
                inv.CustomerParty.postalAddress.PostalZone = "";
                inv.CustomerParty.postalAddress.CitySubdivisionName = "";
                inv.CustomerParty.postalAddress.country.IdentificationCode = "SA";
                inv.CustomerParty.partyLegalEntity.RegistrationName = "";
                inv.CustomerParty.partyTaxScheme.CompanyID = "";
            }
            decimal invoicediscount = 0;
            Decimal.TryParse(Disc.Text, out invoicediscount);
            if (invoicediscount > 0)
            {
                AllowanceCharge allowance = new AllowanceCharge();
                allowance.ChargeIndicator = false;
                allowance.MultiplierFactorNumeric = 0;
                allowance.BaseAmount = 0;
                allowance.Amount = invoicediscount;
                allowance.taxCategory.ID = "S";
                allowance.taxCategory.Percent = 15;
                inv.allowanceCharges.Add(allowance);
            }
            decimal payableamount = 0;
            Decimal.TryParse("", out payableamount);
            inv.legalMonetaryTotal.PayableAmount = payableamount;
            foreach (var item in SL)
            {
                InvoiceLine invline = new InvoiceLine();
                //Product Quantity
                invline.InvoiceQuantity = decimal.Parse(item.Quantity.ToString());
                //Product Name
                invline.item.Name = item.TDDesc;
                var VP = 15;
                if (VP == 0)
                {
                    invline.item.classifiedTaxCategory.ID = "O";
                    invline.taxTotal.TaxSubtotal.taxCategory.ID = "O";
                    invline.taxTotal.TaxSubtotal.taxCategory.TaxExemptionReasonCode = "VATEX-SA-OOS";
                    invline.taxTotal.TaxSubtotal.taxCategory.TaxExemptionReason = "Medicines and medical equipment";

                }
                else
                {
                    invline.item.classifiedTaxCategory.ID = "S";
                    invline.taxTotal.TaxSubtotal.taxCategory.ID = "S";
                }
                invline.item.classifiedTaxCategory.Percent = 15;
                var TP = Math.Round((item.Subtotal * 0.15), 2);
                invline.taxTotal.TaxSubtotal.taxCategory.Percent = decimal.Parse(TP.ToString());
                invline.price.EncludingVat = false;
                invline.price.PriceAmount = decimal.Parse(item.ItemPrice.ToString());
                if (item.Discountdet > 0)
                {
                    AllowanceCharge allowanceCharge = new AllowanceCharge();
                    allowanceCharge.ChargeIndicator = false;
                    allowanceCharge.AllowanceChargeReason = "discount";
                    allowanceCharge.Amount = decimal.Parse(item.Discountdet.ToString());
                    allowanceCharge.MultiplierFactorNumeric = 0;
                    allowanceCharge.BaseAmount = 0;
                    invline.allowanceCharges.Add(allowanceCharge);
                }
                inv.InvoiceLines.Add(invline);
                inv.cSIDInfo.CertPem = CSI.PublicKey;
                inv.cSIDInfo.PrivateKey = CSI.PrivateKey;
                string secretkey = CSI.Secret;
                try
                {
                    bool savexmlfile = true;
                    res = ubl.GenerateInvoiceXML(inv, Directory.GetCurrentDirectory(), savexmlfile);
                    var ddd = res.SingedXMLFileNameFullPath;
                    Filename = Path.GetFileName(ddd).Split('.')[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "\n\n" + ex.InnerException.ToString());
                }
                if (res.IsValid)
                {
                    string warningmessage = res.WarningMessage;
                    decimal allowanceTotalAmount = 0;
                    decimal chargeTotalAmount = 0;
                    decimal lineExtensionAmount = 0;
                    decimal payableAmount = 0;
                    decimal prepaidAmount = 0;
                    decimal taxExclusiveAmount = 0;
                    decimal taxInclusiveAmount = 0;
                    decimal taxAmount = 0;
                    decimal.TryParse(res.AllowanceTotalAmount, out allowanceTotalAmount);
                    decimal.TryParse(res.ChargeTotalAmount, out chargeTotalAmount);
                    decimal.TryParse(res.LineExtensionAmount, out lineExtensionAmount);
                    decimal.TryParse(res.PayableAmount, out payableAmount);
                    decimal.TryParse(res.PrepaidAmount, out prepaidAmount);
                    decimal.TryParse(res.TaxExclusiveAmount, out taxExclusiveAmount);
                    decimal.TryParse(res.TaxInclusiveAmount, out taxInclusiveAmount);
                    decimal.TryParse(res.TaxAmount, out taxAmount);
                }
                else
                {
                    MessageBox.Show(res.ErrorMessage);
                    return;
                }
                mode = Mode.developer;
                ApiRequestLogic apireqlogic = new ApiRequestLogic(mode, Directory.GetCurrentDirectory(), true);

                InvoiceReportingRequest invrequestbody = new InvoiceReportingRequest();
                invrequestbody.invoice = res.EncodedInvoice;
                invrequestbody.invoiceHash = res.InvoiceHash;
                invrequestbody.uuid = res.UUID;
                int status_code = 0;
                string sending_status = "";
                bool is_sent = false;
                string warningmessage1 = "";
                string errormessage = "";
                string clearedinvoice = "";
                string qrcode = "";
                if (mode.ToString() == "developer")
                {
                    ComplianceCsrResponse tokenresponse = new ComplianceCsrResponse();
                    string csr = CSI.CSR;
                    tokenresponse = apireqlogic.GetComplianceCSIDAPI("123456", csr);
                    if (String.IsNullOrEmpty(tokenresponse.ErrorMessage))
                    {
                        InvoiceReportingResponse responsemodel = apireqlogic.CallComplianceInvoiceAPI(tokenresponse.BinarySecurityToken, tokenresponse.Secret, invrequestbody);
                        if (responsemodel.IsSuccess)
                        {
                            if (responsemodel.StatusCode == 200)
                            {
                                var Invhash = res.InvoiceHash.ToString();
                                UXML.Invhash = Invhash;
                                UXML.Salid = Invnum;
                                UXML.Savedata();
                                //save warning message in database to solve for next invoices
                                //responsemodel.WarningMessage
                            }
                            MessageBox.Show(responsemodel.ReportingStatus + responsemodel.ClearanceStatus); //REPORTED                                                     
                            //string qr = "ATHYudio2K/Yp9mE2YXYrdiz2YYg2LnZhNmKINmG2KfYrNmKINin2YTYtNix2KfYsdmKAg8zMDA0NTQ4MDI1MDAwMDMDEzIwMjQtMTEtMTVUMDA6MDA6MDAEBDQuMDAFBDAuNTIGLFExbnNsSFJSV3F5clZmQ3NjZ1lYb2tQMWw1U3hXUnMwSGVnaG5icmk3ZEE9B2BNRVVDSVFDdUJ2SVgyMDcvanFES01yNG0wcURxRmZ6SEI4WVc2VmlSVXJFRUppTExod0lnYmRBMisvREZRNjd5bHo1VS9YR3VBTUZXdlVFUWhsWHQ0ajkyVncrczd2TT0IWDBWMBAGByqGSM49AgEGBSuBBAAKA0IABCheNDlJ8BXkbLAoa2+qR6Y9AkcjmODyUmwObmslXiKLDpZ9Pm9VMLc+AfB+dfPo6RulPygL593FmG+R7zvKR74JRzBFAiEAlGE9DNv5cORJCCqv4wEcMjX8ieglkTD8/4lft9MBm2sCICy0C3UCcI7/aRimkTJBRrwHf3n7fXVCB2khmNtlf/+g";
                            QRPic.Image = QrCodeImage(res.QRCode, 200, 200);
                        }
                        else
                        {
                            MessageBox.Show(responsemodel.ErrorMessage);
                        }
                    }
                    else
                    {
                        MessageBox.Show(tokenresponse.ErrorMessage);
                    }
                }
                else
                {
                    //this code is for simulation and production mode

                    if (inv.invoiceTypeCode.Name.Substring(0, 2) == "01")
                    {
                        // to send standard invoices for clearing
                        //this this the calling of api 
                        //CLearedInvoice is a new xml after clearing it from zatca api with base64 format
                        InvoiceClearanceResponse responsemodel = apireqlogic.CallClearanceAPI(Utility.ToBase64Encode(inv.cSIDInfo.CertPem), secretkey, invrequestbody);
                        //if responsemodel.IsSuccess = true this means that your xml is successfully sent to zatca 
                        status_code = responsemodel.StatusCode;
                        sending_status = responsemodel.ClearanceStatus;
                        is_sent = responsemodel.IsSuccess;
                        if (responsemodel.IsSuccess)
                        {
                            ///////////

                            //if status code =202 it means that xml accepted but with warning 
                            //no need to sent xml again but you must solve that warning messages for the next invoices
                            if (responsemodel.StatusCode == 202)
                            {
                                //save warning message in database to solve for next invoices
                                warningmessage1 = responsemodel.WarningMessage;
                            }

                            // Cleared
                            //MessageBox.Show(responsemodel.QRCode);
                            clearedinvoice = responsemodel.ClearedInvoice;
                            qrcode = responsemodel.QRCode;
                            QRPic.Image = QrCodeImage(responsemodel.QRCode);
                            MessageBox.Show(responsemodel.ClearanceStatus);
                        }
                        else
                        {

                            // if status_code=400 must take new invoice counter and take pih of last created xml 
                            errormessage = responsemodel.ErrorMessage;
                            MessageBox.Show(responsemodel.ErrorMessage);
                        }
                    }
                    else
                    {
                        //to send simplified invoices for reporting
                        //this this the calling of api 
                        InvoiceReportingResponse responsemodel = apireqlogic.CallReportingAPI(Utility.ToBase64Encode(inv.cSIDInfo.CertPem), secretkey, invrequestbody);
                        status_code = responsemodel.StatusCode;
                        sending_status = responsemodel.ReportingStatus; //REPORTED    NOT_REPORTED
                        is_sent = responsemodel.IsSuccess;
                        if (responsemodel.IsSuccess)
                        {
                            //if status code =202 it means that xml accespted but with warning 
                            //no need to sent xml again but you must solve that warning messages for the next invoices
                            if (responsemodel.StatusCode == 202)
                            {
                                //save warning message in database to solve for next invoices
                                warningmessage1 = responsemodel.WarningMessage;
                            }

                            QRPic.Image = QrCodeImage(res.QRCode);
                            MessageBox.Show(responsemodel.ReportingStatus);// Reported
                        }
                        else
                        {
                            // if status_code=400 must take new invoice counter and take pih of last created xml 
                            errormessage = responsemodel.ErrorMessage;
                            MessageBox.Show(responsemodel.ErrorMessage);
                        }

                    }


                }
            }
        }
        public static string GetNodeInnerText(XmlDocument doc, string xPath)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsmgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            XmlNode titleNode = doc.SelectSingleNode(xPath, nsmgr);
            if (titleNode != null)
            {
                return titleNode.InnerText;
            }
            return "";
        }
        public void Printinv()
        {
            try
            {
                ReportDocument Rep = new ReportDocument();
                Frmreporting FR = new Frmreporting();
                if (CF.Comp.Sysnametype == "طباعة نظام مبيعات صغير خياطه")
                {
                    if (VI.ISVAT)
                    {
                        Rep = new SmallInv();
                    }
                    else
                    {
                        Rep = new Smot();
                    }
                }
                else
                {
                    Rep = new Smallsalesbill();
                }
                Dataset Ds = new Dataset();
                byte[] QRCode = null;
                string Totaldiscount = "";
                string Totalafterdiscount = "";
                string date = "";
                Invnum = sal.Invoiceno;
                if (Invnum != 0)
                {
                    Pm = new Paymentmodel();
                    Pm.Getdata(Invnum);
                    CF.Getsaleslist(Invnum);
                    SL = CF.SL;
                    if (VI.ISVAT)
                    {
                        if (!CF.Comp.ISUsePhase2)
                        {
                            Generatqrcode();
                            QRCode = CF.CTB(QRPic.Image);
                        }
                        else
                        {
                            Makeinvoice();
                            QRCode = CF.CTB(QRPic.Image);
                        }
                    }
                    int Countid = 0;
                    var invtyper = "";
                    var Wordofnumber = "";
                    Cntr = new Contractors();
                    Cust = new Thirdparty();
                    foreach (var item in SL)
                    {
                        Paymethode = item.PaymentMethod;
                        date = item.TDate;
                        Countid++;
                        Totalafterdiscount = Convert.ToString(item.NonVatTotal - item.Discount);
                        Totaldiscount = item.Discount.ToString();
                        Cntr.Purchaseorder = item.Purchaseorder;
                        Cntr.Refrancenumber = item.Refrancenumber;
                        Cntr.Projectname = item.Projectname;
                        invtyper = item.Billtype;
                        if (invtyper == "صدرت")
                        {
                            invtyper = item.Invoiceno.ToString();
                        }
                        else
                        {
                            invtyper = item.Billtype;
                        }
                        var Tot = Convert.ToDecimal(item.TotalAmount);
                        decimal GTot = Convert.ToDecimal(string.Format("{0:0.00}", Tot));
                        ConvertNumbersToArabicAlphabet a = new ConvertNumbersToArabicAlphabet(GTot.ToString());
                        Wordofnumber = a.GetNumberAr();
                        Date = item.TDate.ToString();
                        DateTime dt = Convert.ToDateTime(item.TDate.ToString() + " " + item.TTime.ToString());
                        Time = dt;
                        clients.Text = item.Name;
                        Cust.Taxnumber = item.Taxnumber;
                        Cust.Address = item.Address;
                        Cust.MobileNumber = item.MobileNumber;
                        lblSubTotal.Text = item.NonVatTotal.ToString();
                        Disc.Text = item.Discount.ToString();
                        LblVat.Text = item.VatAmount.ToString();
                        lblTotalAmount.Text = item.TotalAmount.ToString();
                        byte[] Logo = Convert.FromBase64String(CF.Comp.Logo);
                        Ds.Bill.Rows.Add(new object[] {
                        invtyper,
                        item.ItemPrice,
                        item.Quantity,
                        item.Discountdet,
                        item.Total,
                        item.Name,
                        QRCode,
                        Countid,
                        item.TDDesc,
                        item.UnitType,
                        item.VatAmount,
                        Pm.Paid.ToString(),
                        Logo
                    });
                    }

                    Rep.SetDataSource(Ds);


                    FR.CRV.ReportSource = Rep;
                    FR.CRV.Refresh();

                    Rep.SetParameterValue("CompanyName", CF.Comp.CName);
                    Rep.SetParameterValue("Shopname", CF.Comp.CName);
                    Rep.SetParameterValue("PhoneNo", CF.Comp.PhoneNo);
                    Rep.SetParameterValue("Address", CF.Comp.Address);
                    Rep.SetParameterValue("Taxnum", CF.Comp.Taxnumber);
                    Rep.SetParameterValue("Date", Date);
                    if (CF.Comp.Sysnametype == "طباعة نظام مبيعات صغير خياطه")
                    {
                        Rep.SetParameterValue("DD", Date);
                        Rep.SetParameterValue("RD", dateTimePicker2.Value.ToString("yyyy-MM-dd"));
                        Rep.SetParameterValue("Discount", Disc.Text);
                        Rep.SetParameterValue("Tax", LblVat.Text);
                        Rep.SetParameterValue("Paied", Paid);
                        Rep.SetParameterValue("Dept", Dept);
                    }
                    else
                    {
                        Rep.SetParameterValue("Paid", Pm.Paid.ToString());
                        Rep.SetParameterValue("Remaining", Pm.Remaining.ToString());
                    }

                    if (CF.Comp.ISUsePhase2)
                    {
                        var invname = Filename + ".pdf";
                        var Pathdata = Directory.GetCurrentDirectory() + @"\PDF\";
                        if (!Directory.Exists(Pathdata))
                        {
                            Directory.CreateDirectory(Pathdata);
                        }
                        Pathdata += invname;
                        Rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Pathdata);
                        PDFA3Result pdfresult = new PDFA3Result();
                        XMLPDF xMLPDF = new XMLPDF();
                        var GXUL = Directory.GetCurrentDirectory() + @"\Invoices\Signed\Simplified\Invoices\" + Filename + ".xml";
                        pdfresult = xMLPDF.ConvertXMLToPDFA3ByteArray(GXUL, Pathdata);
                        //pdfresult = xMLPDF.ConvertEncodedXMLToPDFA3ByteArray(@"DQo8SW52b2ljZSB4bWxucz0idXJuOm9hc2lzOm5hbWVzOnNwZWNpZmljYXRpb246dWJsOnNjaGVtYTp4c2Q6SW52b2ljZS0yIiB4bWxuczpjYWM9InVybjpvYXNpczpuYW1lczpzcGVjaWZpY2F0aW9uOnVibDpzY2hlbWE6eHNkOkNvbW1vbkFnZ3JlZ2F0ZUNvbXBvbmVudHMtMiIgeG1sbnM6Y2JjPSJ1cm46b2FzaXM6bmFtZXM6c3BlY2lmaWNhdGlvbjp1Ymw6c2NoZW1hOnhzZDpDb21tb25CYXNpY0NvbXBvbmVudHMtMiIgeG1sbnM6ZXh0PSJ1cm46b2FzaXM6bmFtZXM6c3BlY2lmaWNhdGlvbjp1Ymw6c2NoZW1hOnhzZDpDb21tb25FeHRlbnNpb25Db21wb25lbnRzLTIiPjxleHQ6VUJMRXh0ZW5zaW9ucz4NCiAgICA8ZXh0OlVCTEV4dGVuc2lvbj4NCiAgICAgICAgPGV4dDpFeHRlbnNpb25VUkk+dXJuOm9hc2lzOm5hbWVzOnNwZWNpZmljYXRpb246dWJsOmRzaWc6ZW52ZWxvcGVkOnhhZGVzPC9leHQ6RXh0ZW5zaW9uVVJJPg0KICAgICAgICA8ZXh0OkV4dGVuc2lvbkNvbnRlbnQ+DQogICAgICAgICAgICA8IS0tIFBsZWFzZSBub3RlIHRoYXQgdGhlIHNpZ25hdHVyZSB2YWx1ZXMgYXJlIHNhbXBsZSB2YWx1ZXMgb25seSAtLT4NCiAgICAgICAgICAgIDxzaWc6VUJMRG9jdW1lbnRTaWduYXR1cmVzIHhtbG5zOnNpZz0idXJuOm9hc2lzOm5hbWVzOnNwZWNpZmljYXRpb246dWJsOnNjaGVtYTp4c2Q6Q29tbW9uU2lnbmF0dXJlQ29tcG9uZW50cy0yIiB4bWxuczpzYWM9InVybjpvYXNpczpuYW1lczpzcGVjaWZpY2F0aW9uOnVibDpzY2hlbWE6eHNkOlNpZ25hdHVyZUFnZ3JlZ2F0ZUNvbXBvbmVudHMtMiIgeG1sbnM6c2JjPSJ1cm46b2FzaXM6bmFtZXM6c3BlY2lmaWNhdGlvbjp1Ymw6c2NoZW1hOnhzZDpTaWduYXR1cmVCYXNpY0NvbXBvbmVudHMtMiI+DQogICAgICAgICAgICAgICAgPHNhYzpTaWduYXR1cmVJbmZvcm1hdGlvbj4NCiAgICAgICAgICAgICAgICAgICAgPGNiYzpJRD51cm46b2FzaXM6bmFtZXM6c3BlY2lmaWNhdGlvbjp1Ymw6c2lnbmF0dXJlOjE8L2NiYzpJRD4NCiAgICAgICAgICAgICAgICAgICAgPHNiYzpSZWZlcmVuY2VkU2lnbmF0dXJlSUQ+dXJuOm9hc2lzOm5hbWVzOnNwZWNpZmljYXRpb246dWJsOnNpZ25hdHVyZTpJbnZvaWNlPC9zYmM6UmVmZXJlbmNlZFNpZ25hdHVyZUlEPg0KICAgICAgICAgICAgICAgICAgICA8ZHM6U2lnbmF0dXJlIElkPSJzaWduYXR1cmUiIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIj4NCiAgICAgICAgICAgICAgICAgICAgICAgIDxkczpTaWduZWRJbmZvPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkczpDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwNi8xMi94bWwtYzE0bjExIiAvPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkczpTaWduYXR1cmVNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNlY2RzYS1zaGEyNTYiIC8+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRzOlJlZmVyZW5jZSBJZD0iaW52b2ljZVNpZ25lZERhdGEiIFVSST0iIj4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRzOlRyYW5zZm9ybXM+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZHM6VHJhbnNmb3JtIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvVFIvMTk5OS9SRUMteHBhdGgtMTk5OTExMTYiPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkczpYUGF0aD5ub3QoLy9hbmNlc3Rvci1vci1zZWxmOjpleHQ6VUJMRXh0ZW5zaW9ucyk8L2RzOlhQYXRoPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kczpUcmFuc2Zvcm0+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZHM6VHJhbnNmb3JtIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvVFIvMTk5OS9SRUMteHBhdGgtMTk5OTExMTYiPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkczpYUGF0aD5ub3QoLy9hbmNlc3Rvci1vci1zZWxmOjpjYWM6U2lnbmF0dXJlKTwvZHM6WFBhdGg+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2RzOlRyYW5zZm9ybT4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy9UUi8xOTk5L1JFQy14cGF0aC0xOTk5MTExNiI+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRzOlhQYXRoPm5vdCgvL2FuY2VzdG9yLW9yLXNlbGY6OmNhYzpBZGRpdGlvbmFsRG9jdW1lbnRSZWZlcmVuY2VbY2JjOklEPSdRUiddKTwvZHM6WFBhdGg+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2RzOlRyYW5zZm9ybT4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDA2LzEyL3htbC1jMTRuMTEiIC8+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZHM6VHJhbnNmb3Jtcz4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRzOkRpZ2VzdE1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvMDQveG1sZW5jI3NoYTI1NiIgLz4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRzOkRpZ2VzdFZhbHVlPkZpUmU4SUd4V1RZVWRZMkJENjBHSWdZQ2F6TDJuMkh5MVJhTllEMEw5NkU9PC9kczpEaWdlc3RWYWx1ZT4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2RzOlJlZmVyZW5jZT4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZHM6UmVmZXJlbmNlIFR5cGU9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNTaWduYXR1cmVQcm9wZXJ0aWVzIiBVUkk9IiN4YWRlc1NpZ25lZFByb3BlcnRpZXMiPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZHM6RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxlbmMjc2hhMjU2IiAvPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZHM6RGlnZXN0VmFsdWU+TVdWbVpXUm1NamxtT0dZNVpqSTFNRFZoTkRJek0yUm1OREV3WldZME5EWmxabVZpTmpkbVl6RmlPVGt4T1RZeU5EazRaakJoTkRrNE5HWTFZemxrTVE9PTwvZHM6RGlnZXN0VmFsdWU+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kczpSZWZlcmVuY2U+DQogICAgICAgICAgICAgICAgICAgICAgICA8L2RzOlNpZ25lZEluZm8+DQogICAgICAgICAgICAgICAgICAgICAgICA8ZHM6U2lnbmF0dXJlVmFsdWU+TUVZQ0lRQ3QxbmVNK2hadzMxbW9XYjBHU3JoV3k2cktKVkJvQ1JyaHBwTlNvRldrZVFJaEFLbmtlMUxUL0k5cnllTWc5NmZ1eDk3T0tVVW0vdkF1UUszc1pNWDB6R3JNPC9kczpTaWduYXR1cmVWYWx1ZT4NCiAgICAgICAgICAgICAgICAgICAgICAgIDxkczpLZXlJbmZvPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkczpYNTA5RGF0YT4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRzOlg1MDlDZXJ0aWZpY2F0ZT5NSUlEOWpDQ0E1dWdBd0lCQWdJVGJ3QUFlQ3k5YUtjTEE5OUhyQUFCQUFCNExEQUtCZ2dxaGtqT1BRUURBakJqTVJVd0V3WUtDWkltaVpQeUxHUUJHUllGYkc5allXd3hFekFSQmdvSmtpYUprL0lzWkFFWkZnTm5iM1l4RnpBVkJnb0praWFKay9Jc1pBRVpGZ2RsZUhSbllYcDBNUnd3R2dZRFZRUURFeE5VVTFwRlNVNVdUMGxEUlMxVGRXSkRRUzB4TUI0WERUSXlNRFF4T1RJd05Ea3dPVm9YRFRJME1EUXhPREl3TkRrd09Wb3dXVEVMTUFrR0ExVUVCaE1DVTBFeEV6QVJCZ05WQkFvVENqTXhNak0wTlRZM09Ea3hEREFLQmdOVkJBc1RBMVJUVkRFbk1DVUdBMVVFQXhNZVZGTlVMUzA1TnpBMU5qQXdOREF0TXpFeU16UTFOamM0T1RBd01EQXpNRll3RUFZSEtvWkl6ajBDQVFZRks0RUVBQW9EUWdBRVlZTU1vT2FGWUFoTU8vc3Rlb3RmWnlhdnI2cDExU1Nsd3NLOWF6bXNMWTdiMWIrRkxocU1BcmhCMmRxSEtib3hxS05mdmtLRGVQaHBxanVpNWhjbjBhT0NBamt3Z2dJMU1JR2FCZ05WSFJFRWdaSXdnWStrZ1l3d2dZa3hPekE1QmdOVkJBUU1NakV0VkZOVWZESXRWRk5VZkRNdE5EZG1NVFpqTWpZdE9EQTJZaTAwWlRFMUxXSXlOamt0TjJFNE1ETTRPRFJpWlRsak1SOHdIUVlLQ1pJbWlaUHlMR1FCQVF3UE16RXlNelExTmpjNE9UQXdNREF6TVEwd0N3WURWUVFNREFReE1UQXdNUXd3Q2dZRFZRUWFEQU5VVTFReEREQUtCZ05WQkE4TUExUlRWREFkQmdOVkhRNEVGZ1FVTzVaaVU3TmFrVTNlZWpWYTNJMlMxQjJzRHdrd0h3WURWUjBqQkJnd0ZvQVVkbUNNK3dhZ3JHZFhOWjNQbXF5bks1azF0Uzh3VGdZRFZSMGZCRWN3UlRCRG9FR2dQNFk5YUhSMGNEb3ZMM1J6ZEdOeWJDNTZZWFJqWVM1bmIzWXVjMkV2UTJWeWRFVnVjbTlzYkM5VVUxcEZTVTVXVDBsRFJTMVRkV0pEUVMweExtTnliRENCclFZSUt3WUJCUVVIQVFFRWdhQXdnWjB3YmdZSUt3WUJCUVVITUFHR1ltaDBkSEE2THk5MGMzUmpjbXd1ZW1GMFkyRXVaMjkyTG5OaEwwTmxjblJGYm5KdmJHd3ZWRk5hUldsdWRtOXBZMlZUUTBFeExtVjRkR2RoZW5RdVoyOTJMbXh2WTJGc1gxUlRXa1ZKVGxaUFNVTkZMVk4xWWtOQkxURW9NU2t1WTNKME1Dc0dDQ3NHQVFVRkJ6QUJoaDlvZEhSd09pOHZkSE4wWTNKc0xucGhkR05oTG1kdmRpNXpZUzl2WTNOd01BNEdBMVVkRHdFQi93UUVBd0lIZ0RBZEJnTlZIU1VFRmpBVUJnZ3JCZ0VGQlFjREFnWUlLd1lCQlFVSEF3TXdKd1lKS3dZQkJBR0NOeFVLQkJvd0dEQUtCZ2dyQmdFRkJRY0RBakFLQmdnckJnRUZCUWNEQXpBS0JnZ3Foa2pPUFFRREFnTkpBREJHQWlFQTdtSFQ2eWc4NWp0UUdXcDNNN3RQVDdKazIrenN2VkhHczNiVTVaN1lFNjhDSVFENjBlYlFhbVlqWXZkZWJuRmpOZng0WDRkb3A3THNFQkZDTlNzTFkwSUZhUT09PC9kczpYNTA5Q2VydGlmaWNhdGU+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kczpYNTA5RGF0YT4NCiAgICAgICAgICAgICAgICAgICAgICAgIDwvZHM6S2V5SW5mbz4NCiAgICAgICAgICAgICAgICAgICAgICAgIDxkczpPYmplY3Q+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgPHhhZGVzOlF1YWxpZnlpbmdQcm9wZXJ0aWVzIFRhcmdldD0ic2lnbmF0dXJlIiB4bWxuczp4YWRlcz0iaHR0cDovL3VyaS5ldHNpLm9yZy8wMTkwMy92MS4zLjIjIj4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHhhZGVzOlNpZ25lZFByb3BlcnRpZXMgSWQ9InhhZGVzU2lnbmVkUHJvcGVydGllcyI+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8eGFkZXM6U2lnbmVkU2lnbmF0dXJlUHJvcGVydGllcz4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8eGFkZXM6U2lnbmluZ1RpbWU+MjAyNC0wNS0wN1QxNzoxNjozOTwveGFkZXM6U2lnbmluZ1RpbWU+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHhhZGVzOlNpZ25pbmdDZXJ0aWZpY2F0ZT4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHhhZGVzOkNlcnQ+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8eGFkZXM6Q2VydERpZ2VzdD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZHM6RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxlbmMjc2hhMjU2IiAvPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkczpEaWdlc3RWYWx1ZT5OamxoT1RWbVl6SXpOMkkwTWpjeE5HUmpORFExTjJFek0ySTVOR05qTkRVeVptUTVaakV4TURVd05HTTJPRE5qTkRBeE1UUTBaRGsxTkRRNE9UUm1ZZz09PC9kczpEaWdlc3RWYWx1ZT4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwveGFkZXM6Q2VydERpZ2VzdD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx4YWRlczpJc3N1ZXJTZXJpYWw+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRzOlg1MDlJc3N1ZXJOYW1lPkNOPVRTWkVJTlZPSUNFLVN1YkNBLTEsIERDPWV4dGdhenQsIERDPWdvdiwgREM9bG9jYWw8L2RzOlg1MDlJc3N1ZXJOYW1lPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkczpYNTA5U2VyaWFsTnVtYmVyPjI0NzUzODI4NzY3NzY1NjEzOTE1MTcyMDY2NTE2NDU2NjAyNzk0NjI3MjE1ODA8L2RzOlg1MDlTZXJpYWxOdW1iZXI+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3hhZGVzOklzc3VlclNlcmlhbD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC94YWRlczpDZXJ0Pg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwveGFkZXM6U2lnbmluZ0NlcnRpZmljYXRlPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC94YWRlczpTaWduZWRTaWduYXR1cmVQcm9wZXJ0aWVzPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3hhZGVzOlNpZ25lZFByb3BlcnRpZXM+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgPC94YWRlczpRdWFsaWZ5aW5nUHJvcGVydGllcz4NCiAgICAgICAgICAgICAgICAgICAgICAgIDwvZHM6T2JqZWN0Pg0KICAgICAgICAgICAgICAgICAgICA8L2RzOlNpZ25hdHVyZT4NCiAgICAgICAgICAgICAgICA8L3NhYzpTaWduYXR1cmVJbmZvcm1hdGlvbj4NCiAgICAgICAgICAgIDwvc2lnOlVCTERvY3VtZW50U2lnbmF0dXJlcz4NCiAgICAgICAgPC9leHQ6RXh0ZW5zaW9uQ29udGVudD4NCiAgICA8L2V4dDpVQkxFeHRlbnNpb24+DQo8L2V4dDpVQkxFeHRlbnNpb25zPg0KICA8Y2JjOlByb2ZpbGVJRD5yZXBvcnRpbmc6MS4wPC9jYmM6UHJvZmlsZUlEPg0KICA8Y2JjOklEPkludi0yMzQ8L2NiYzpJRD4NCiAgPGNiYzpVVUlEPmYzMGI5NTIyLWJkZDQtNGM1ZC1iZjViLTEwY2JlZDAyZWY4MDwvY2JjOlVVSUQ+DQogIDxjYmM6SXNzdWVEYXRlPjIwMjQtMDUtMDc8L2NiYzpJc3N1ZURhdGU+DQogIDxjYmM6SXNzdWVUaW1lPjE3OjE2OjIyPC9jYmM6SXNzdWVUaW1lPg0KICA8Y2JjOkludm9pY2VUeXBlQ29kZSBuYW1lPSIwMjAwMDAwIj4zODg8L2NiYzpJbnZvaWNlVHlwZUNvZGU+DQogIDxjYmM6RG9jdW1lbnRDdXJyZW5jeUNvZGU+U0FSPC9jYmM6RG9jdW1lbnRDdXJyZW5jeUNvZGU+DQogIDxjYmM6VGF4Q3VycmVuY3lDb2RlPlNBUjwvY2JjOlRheEN1cnJlbmN5Q29kZT4NCiAgPGNiYzpMaW5lQ291bnROdW1lcmljPjE8L2NiYzpMaW5lQ291bnROdW1lcmljPg0KICA8Y2FjOkFkZGl0aW9uYWxEb2N1bWVudFJlZmVyZW5jZT4NCiAgICA8Y2JjOklEPklDVjwvY2JjOklEPg0KICAgIDxjYmM6VVVJRD4yMzQ8L2NiYzpVVUlEPg0KICA8L2NhYzpBZGRpdGlvbmFsRG9jdW1lbnRSZWZlcmVuY2U+DQogIDxjYWM6QWRkaXRpb25hbERvY3VtZW50UmVmZXJlbmNlPg0KICAgIDxjYmM6SUQ+UElIPC9jYmM6SUQ+DQogICAgPGNhYzpBdHRhY2htZW50Pg0KICAgICAgPGNiYzpFbWJlZGRlZERvY3VtZW50QmluYXJ5T2JqZWN0IG1pbWVDb2RlPSJ0ZXh0L3BsYWluIj5OV1psWTJWaU5qWm1abU00Tm1Zek9HUTVOVEkzT0Raak5tUTJPVFpqTnpsak1tUmlZekl6T1dSa05HVTVNV0kwTmpjeU9XUTNNMkV5TjJaaU5UZGxPUT09PC9jYmM6RW1iZWRkZWREb2N1bWVudEJpbmFyeU9iamVjdD4NCiAgICA8L2NhYzpBdHRhY2htZW50Pg0KICA8L2NhYzpBZGRpdGlvbmFsRG9jdW1lbnRSZWZlcmVuY2U+DQogIDxjYWM6QWRkaXRpb25hbERvY3VtZW50UmVmZXJlbmNlPg0KICAgICAgICA8Y2JjOklEPlFSPC9jYmM6SUQ+DQogICAgICAgIDxjYWM6QXR0YWNobWVudD4NCiAgICAgICAgICAgIDxjYmM6RW1iZWRkZWREb2N1bWVudEJpbmFyeU9iamVjdCBtaW1lQ29kZT0idGV4dC9wbGFpbiI+QVF4MFpYTjBJR052YlhCaGJua0NEek13TURBMU5UazRORGN3TURBd013TVRNakF5TkMwd05TMHdOMVF4TnpveE5qb3lNZ1FHTVRFMUxqQXdCUVV4TlM0d01BWXNSbWxTWlRoSlIzaFhWRmxWWkZreVFrUTJNRWRKWjFsRFlYcE1NbTR5U0hreFVtRk9XVVF3VERrMlJUMEhZRTFGV1VOSlVVTjBNVzVsVFN0b1duY3pNVzF2VjJJd1IxTnlhRmQ1Tm5KTFNsWkNiME5TY21od2NFNVRiMFpYYTJWUlNXaEJTMjVyWlRGTVZDOUpPWEo1WlUxbk9UWm1kWGc1TjA5TFZWVnRMM1pCZFZGTE0zTmFUVmd3ZWtkeVRRaFlNRll3RUFZSEtvWkl6ajBDQVFZRks0RUVBQW9EUWdBRVlZTU1vT2FGWUFoTU8vc3Rlb3RmWnlhdnI2cDExU1Nsd3NLOWF6bXNMWTdiMWIrRkxocU1BcmhCMmRxSEtib3hxS05mdmtLRGVQaHBxanVpNWhjbjBRbElNRVlDSVFEdVlkUHJLRHptTzFBWmFuY3p1MDlQc21UYjdPeTlVY2F6ZHRUbG50Z1Ryd0loQVByUjV0QnFaaU5pOTE1dWNXTTEvSGhmaDJpbnN1d1FFVUkxS3d0alFnVnA8L2NiYzpFbWJlZGRlZERvY3VtZW50QmluYXJ5T2JqZWN0Pg0KICAgICAgICA8L2NhYzpBdHRhY2htZW50Pg0KPC9jYWM6QWRkaXRpb25hbERvY3VtZW50UmVmZXJlbmNlPjxjYWM6U2lnbmF0dXJlPgogICAgICA8Y2JjOklEPnVybjpvYXNpczpuYW1lczpzcGVjaWZpY2F0aW9uOnVibDpzaWduYXR1cmU6SW52b2ljZTwvY2JjOklEPgogICAgICA8Y2JjOlNpZ25hdHVyZU1ldGhvZD51cm46b2FzaXM6bmFtZXM6c3BlY2lmaWNhdGlvbjp1Ymw6ZHNpZzplbnZlbG9wZWQ6eGFkZXM8L2NiYzpTaWduYXR1cmVNZXRob2Q+CjwvY2FjOlNpZ25hdHVyZT48Y2FjOkFjY291bnRpbmdTdXBwbGllclBhcnR5Pg0KICAgIDxjYWM6UGFydHk+DQogICAgICA8Y2FjOlBhcnR5SWRlbnRpZmljYXRpb24+DQogICAgICAgIDxjYmM6SUQgc2NoZW1lSUQ9IkNSTiI+MTIzNDU2PC9jYmM6SUQ+DQogICAgICA8L2NhYzpQYXJ0eUlkZW50aWZpY2F0aW9uPg0KICAgICAgPGNhYzpQb3N0YWxBZGRyZXNzPg0KICAgICAgICA8Y2JjOlN0cmVldE5hbWU+c3RyZWV0IG5hbWU8L2NiYzpTdHJlZXROYW1lPg0KICAgICAgICA8Y2JjOkJ1aWxkaW5nTnVtYmVyPjM3MjQ8L2NiYzpCdWlsZGluZ051bWJlcj4NCiAgICAgICAgPGNiYzpDaXR5U3ViZGl2aXNpb25OYW1lPkFsZmFsYWg8L2NiYzpDaXR5U3ViZGl2aXNpb25OYW1lPg0KICAgICAgICA8Y2JjOkNpdHlOYW1lPnRhaWY8L2NiYzpDaXR5TmFtZT4NCiAgICAgICAgPGNiYzpQb3N0YWxab25lPjE1Mzg1PC9jYmM6UG9zdGFsWm9uZT4NCiAgICAgICAgPGNhYzpDb3VudHJ5Pg0KICAgICAgICAgIDxjYmM6SWRlbnRpZmljYXRpb25Db2RlPlNBPC9jYmM6SWRlbnRpZmljYXRpb25Db2RlPg0KICAgICAgICA8L2NhYzpDb3VudHJ5Pg0KICAgICAgPC9jYWM6UG9zdGFsQWRkcmVzcz4NCiAgICAgIDxjYWM6UGFydHlUYXhTY2hlbWU+DQogICAgICAgIDxjYmM6Q29tcGFueUlEPjMwMDA1NTk4NDcwMDAwMzwvY2JjOkNvbXBhbnlJRD4NCiAgICAgICAgPGNhYzpUYXhTY2hlbWU+DQogICAgICAgICAgPGNiYzpJRD5WQVQ8L2NiYzpJRD4NCiAgICAgICAgPC9jYWM6VGF4U2NoZW1lPg0KICAgICAgPC9jYWM6UGFydHlUYXhTY2hlbWU+DQogICAgICA8Y2FjOlBhcnR5TGVnYWxFbnRpdHk+DQogICAgICAgIDxjYmM6UmVnaXN0cmF0aW9uTmFtZT50ZXN0IGNvbXBhbnk8L2NiYzpSZWdpc3RyYXRpb25OYW1lPg0KICAgICAgPC9jYWM6UGFydHlMZWdhbEVudGl0eT4NCiAgICA8L2NhYzpQYXJ0eT4NCiAgPC9jYWM6QWNjb3VudGluZ1N1cHBsaWVyUGFydHk+DQogIDxjYWM6QWNjb3VudGluZ0N1c3RvbWVyUGFydHk+DQogICAgPGNhYzpQYXJ0eT4NCiAgICAgIDxjYWM6UGFydHlJZGVudGlmaWNhdGlvbj4NCiAgICAgICAgPGNiYzpJRCBzY2hlbWVJRD0iQ1JOIj4xMjM0NTY8L2NiYzpJRD4NCiAgICAgIDwvY2FjOlBhcnR5SWRlbnRpZmljYXRpb24+DQogICAgICA8Y2FjOlBvc3RhbEFkZHJlc3M+DQogICAgICAgIDxjYmM6U3RyZWV0TmFtZT5zdHJlZXQgbmFtZTwvY2JjOlN0cmVldE5hbWU+DQogICAgICAgIDxjYmM6QnVpbGRpbmdOdW1iZXI+MzcyNDwvY2JjOkJ1aWxkaW5nTnVtYmVyPg0KICAgICAgICA8Y2JjOkNpdHlTdWJkaXZpc2lvbk5hbWU+QWxmYWxhaDwvY2JjOkNpdHlTdWJkaXZpc2lvbk5hbWU+DQogICAgICAgIDxjYmM6Q2l0eU5hbWU+SmVkZGFoPC9jYmM6Q2l0eU5hbWU+DQogICAgICAgIDxjYmM6UG9zdGFsWm9uZT4xNTM4NTwvY2JjOlBvc3RhbFpvbmU+DQogICAgICAgIDxjYWM6Q291bnRyeT4NCiAgICAgICAgICA8Y2JjOklkZW50aWZpY2F0aW9uQ29kZT5TQTwvY2JjOklkZW50aWZpY2F0aW9uQ29kZT4NCiAgICAgICAgPC9jYWM6Q291bnRyeT4NCiAgICAgIDwvY2FjOlBvc3RhbEFkZHJlc3M+DQogICAgICA8Y2FjOlBhcnR5VGF4U2NoZW1lPg0KICAgICAgICA8Y2JjOkNvbXBhbnlJRD4zMTA0MjQ0MTUwMDAwMDM8L2NiYzpDb21wYW55SUQ+DQogICAgICAgIDxjYWM6VGF4U2NoZW1lPg0KICAgICAgICAgIDxjYmM6SUQ+VkFUPC9jYmM6SUQ+DQogICAgICAgIDwvY2FjOlRheFNjaGVtZT4NCiAgICAgIDwvY2FjOlBhcnR5VGF4U2NoZW1lPg0KICAgICAgPGNhYzpQYXJ0eUxlZ2FsRW50aXR5Pg0KICAgICAgICA8Y2JjOlJlZ2lzdHJhdGlvbk5hbWU+2YXYpNiz2LPYqSDYp9mE2YXYtNiq2LHZiTwvY2JjOlJlZ2lzdHJhdGlvbk5hbWU+DQogICAgICA8L2NhYzpQYXJ0eUxlZ2FsRW50aXR5Pg0KICAgIDwvY2FjOlBhcnR5Pg0KICA8L2NhYzpBY2NvdW50aW5nQ3VzdG9tZXJQYXJ0eT4NCiAgPGNhYzpQYXltZW50TWVhbnM+DQogICAgPGNiYzpQYXltZW50TWVhbnNDb2RlPjEwPC9jYmM6UGF5bWVudE1lYW5zQ29kZT4NCiAgPC9jYWM6UGF5bWVudE1lYW5zPg0KICA8Y2FjOlRheFRvdGFsPg0KICAgIDxjYmM6VGF4QW1vdW50IGN1cnJlbmN5SUQ9IlNBUiI+MTUuMDA8L2NiYzpUYXhBbW91bnQ+DQogICAgPGNhYzpUYXhTdWJ0b3RhbD4NCiAgICAgIDxjYmM6VGF4YWJsZUFtb3VudCBjdXJyZW5jeUlEPSJTQVIiPjEwMC4wMDwvY2JjOlRheGFibGVBbW91bnQ+DQogICAgICA8Y2JjOlRheEFtb3VudCBjdXJyZW5jeUlEPSJTQVIiPjE1LjAwPC9jYmM6VGF4QW1vdW50Pg0KICAgICAgPGNhYzpUYXhDYXRlZ29yeT4NCiAgICAgICAgPGNiYzpJRD5TPC9jYmM6SUQ+DQogICAgICAgIDxjYmM6UGVyY2VudD4xNS4wMDwvY2JjOlBlcmNlbnQ+DQogICAgICAgIDxjYWM6VGF4U2NoZW1lPg0KICAgICAgICAgIDxjYmM6SUQ+VkFUPC9jYmM6SUQ+DQogICAgICAgIDwvY2FjOlRheFNjaGVtZT4NCiAgICAgIDwvY2FjOlRheENhdGVnb3J5Pg0KICAgIDwvY2FjOlRheFN1YnRvdGFsPg0KICA8L2NhYzpUYXhUb3RhbD4NCiAgPGNhYzpUYXhUb3RhbD4NCiAgICA8Y2JjOlRheEFtb3VudCBjdXJyZW5jeUlEPSJTQVIiPjE1LjAwPC9jYmM6VGF4QW1vdW50Pg0KICA8L2NhYzpUYXhUb3RhbD4NCiAgPGNhYzpMZWdhbE1vbmV0YXJ5VG90YWw+DQogICAgPGNiYzpMaW5lRXh0ZW5zaW9uQW1vdW50IGN1cnJlbmN5SUQ9IlNBUiI+MTAwLjAwPC9jYmM6TGluZUV4dGVuc2lvbkFtb3VudD4NCiAgICA8Y2JjOlRheEV4Y2x1c2l2ZUFtb3VudCBjdXJyZW5jeUlEPSJTQVIiPjEwMC4wMDwvY2JjOlRheEV4Y2x1c2l2ZUFtb3VudD4NCiAgICA8Y2JjOlRheEluY2x1c2l2ZUFtb3VudCBjdXJyZW5jeUlEPSJTQVIiPjExNS4wMDwvY2JjOlRheEluY2x1c2l2ZUFtb3VudD4NCiAgICA8Y2JjOkFsbG93YW5jZVRvdGFsQW1vdW50IGN1cnJlbmN5SUQ9IlNBUiI+MC4wMDwvY2JjOkFsbG93YW5jZVRvdGFsQW1vdW50Pg0KICAgIDxjYmM6Q2hhcmdlVG90YWxBbW91bnQgY3VycmVuY3lJRD0iU0FSIj4wLjAwPC9jYmM6Q2hhcmdlVG90YWxBbW91bnQ+DQogICAgPGNiYzpQYXlhYmxlQW1vdW50IGN1cnJlbmN5SUQ9IlNBUiI+MTE1LjAwPC9jYmM6UGF5YWJsZUFtb3VudD4NCiAgPC9jYWM6TGVnYWxNb25ldGFyeVRvdGFsPg0KICA8Y2FjOkludm9pY2VMaW5lPg0KICAgIDxjYmM6SUQ+MTwvY2JjOklEPg0KICAgIDxjYmM6SW52b2ljZWRRdWFudGl0eT4xMDwvY2JjOkludm9pY2VkUXVhbnRpdHk+DQogICAgPGNiYzpMaW5lRXh0ZW5zaW9uQW1vdW50IGN1cnJlbmN5SUQ9IlNBUiI+MTAwLjAwPC9jYmM6TGluZUV4dGVuc2lvbkFtb3VudD4NCiAgICA8Y2FjOlRheFRvdGFsPg0KICAgICAgPGNiYzpUYXhBbW91bnQgY3VycmVuY3lJRD0iU0FSIj4xNS4wMDwvY2JjOlRheEFtb3VudD4NCiAgICAgIDxjYmM6Um91bmRpbmdBbW91bnQgY3VycmVuY3lJRD0iU0FSIj4xMTUuMDA8L2NiYzpSb3VuZGluZ0Ftb3VudD4NCiAgICA8L2NhYzpUYXhUb3RhbD4NCiAgICA8Y2FjOkl0ZW0+DQogICAgICA8Y2JjOk5hbWU+a2RrZGs8L2NiYzpOYW1lPg0KICAgICAgPGNhYzpDbGFzc2lmaWVkVGF4Q2F0ZWdvcnk+DQogICAgICAgIDxjYmM6SUQ+UzwvY2JjOklEPg0KICAgICAgICA8Y2JjOlBlcmNlbnQ+MTUuMDA8L2NiYzpQZXJjZW50Pg0KICAgICAgICA8Y2FjOlRheFNjaGVtZT4NCiAgICAgICAgICA8Y2JjOklEPlZBVDwvY2JjOklEPg0KICAgICAgICA8L2NhYzpUYXhTY2hlbWU+DQogICAgICA8L2NhYzpDbGFzc2lmaWVkVGF4Q2F0ZWdvcnk+DQogICAgPC9jYWM6SXRlbT4NCiAgICA8Y2FjOlByaWNlPg0KICAgICAgPGNiYzpQcmljZUFtb3VudCBjdXJyZW5jeUlEPSJTQVIiPjEwPC9jYmM6UHJpY2VBbW91bnQ+DQogICAgICA8Y2JjOkJhc2VRdWFudGl0eT4xLjAwPC9jYmM6QmFzZVF1YW50aXR5Pg0KICAgIDwvY2FjOlByaWNlPg0KICA8L2NhYzpJbnZvaWNlTGluZT4NCjwvSW52b2ljZT4=", @"C:\Users\Administrator\Downloads\AFAQ\new_pdf\good_InvoicePDF-RGBinv_A4-1110000015-1-5-2024.pdf", @"C:\Users\Administrator\Downloads\AFAQ\new_pdf");
                        if (pdfresult.IsValid)
                        {
                            // MessageBox.Show("File Saved Successfuly at ");
                            if (pdfresult.PDFA3ContentFile != null && pdfresult.PDFA3ContentFile.Length > 0)
                            {
                                try
                                {
                                    Pathdata = Directory.GetCurrentDirectory() + @"\Invoices\PDF\";
                                    if (!Directory.Exists(Pathdata))
                                    {
                                        Directory.CreateDirectory(Pathdata);
                                    }
                                    File.WriteAllBytes(Pathdata + Filename + ".Pdf", pdfresult.PDFA3ContentFile);
                                    //Rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Pathdata + Filename + ".Pdf");
                                    //saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf";
                                    //saveFileDialog1.FileName = "InvoicePDFA3.pdf";
                                    //DialogResult resultdiag = saveFileDialog1.ShowDialog();

                                    //if (resultdiag == DialogResult.OK)
                                    //{
                                    //    File.WriteAllBytes(saveFileDialog1.FileName, pdfresult.PDFA3ContentFile);
                                    //    MessageBox.Show("File Saved Successfuly at " + saveFileDialog1.FileName);
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.InnerException.ToString());
                                }

                            }
                            else
                            {
                                MessageBox.Show("PDF-A3 Not Generated !");
                            }

                        }
                        else
                        {
                            MessageBox.Show(pdfresult.ErrorMessage);
                        }
                    }
                    else
                    {
                        //FR.ShowDialog();
                        Rep.PrintOptions.PrinterName = CF.Comp.Printername;
                        Rep.PrintToPrinter(1, true, 1, 1);
                    }
                }
                else
                {
                    MessageBox.Show("لا يوجد فاتورة لطباعتها", "خطأ");
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "'طباعة الفاتورة'" + "','" + "Printinv" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void RemoveItem()
        {
            try
            {
                int rowIndex = DGV.CurrentCell.RowIndex;
                var CR = Convert.ToInt32(DGV[4, rowIndex].Value.ToString());
                if (CR > 1)
                {
                    var Price = Convert.ToDouble(DGV[5, rowIndex].Value.ToString());
                    DGV[4, rowIndex].Value = Convert.ToInt32(DGV[4, rowIndex].Value.ToString()) - 1;
                    var Q = DGV[4, rowIndex].Value.ToString();
                    var Totac = Convert.ToDouble(Price) * Convert.ToInt32(Q);
                    DGV[7, rowIndex].Value = Totac;
                }
                else
                {
                    DGV.Rows.RemoveAt(rowIndex);
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "حذف صنف" + "','" + "RemoveItem" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void Setcategories()
        {
            try
            {
                CF.Getcatlist();
                CL = CF.CL;
                foreach (Category c in CL)
                {
                    addCateButton(c.CategoryName, c.CategoryNo.ToString(), c.Color.ToString());
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "اضافة الفئات" + "','" + "Setcategories" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
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
                clients.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "جلب بيانات العملاء" + "','" + "Getcustomers" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void AddRondomdgv()
        {
            try
            {
                var TBD = CF.Pro.UnitPrice * 1;
                DGV.Rows.Add(CF.Pro.ProductNo,
                             CF.Pro.Description,
                             CF.Untname,
                             CF.Pro.Unitid,
                             1, CF.Pro.UnitPrice, 0, TBD);
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "اضافة البيانات للجدول" + "','" + "AddRondomdgv" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Chachdgv(int proid)
        {
            try
            {
                Boolean Flag = false;
                int DI;
                for (DI = 0; DI < DGV.Rows.Count; DI++)
                {
                    var PC = DGV.Rows[DI].Cells[0].Value.ToString();
                    var Qty = Convert.ToDecimal(DGV.Rows[DI].Cells[4].Value);
                    if (PC == proid.ToString())
                    {
                        Flag = true;
                        break;
                    }
                }
                if (Flag)
                {
                    DGV.Rows[DI].Cells[4].Value = Convert.ToDecimal(DGV.Rows[DI].Cells[4].Value.ToString()) + 1;
                    var Qty = DGV.Rows[DI].Cells[4].Value;
                    var Price = DGV.Rows[DI].Cells[5].Value;
                    var Totbd = ((Convert.ToDecimal(Qty) * Convert.ToDecimal(Price)) - Convert.ToDecimal(DGV.Rows[DI].Cells[6].Value));
                    DGV.Rows[DI].Cells[7].Value = Totbd;
                }
                else
                {
                    AddRondomdgv();
                }
                Flag = false;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "التحقق من بيانات الجدول" + "','" + "Chachdgv" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void DGSum()
        {
            try
            {
                double sumSubtotal = 0;
                double sumDiscount = 0;
                double sumTax = 0;
                double sum = 0;
                for (int i = 0; i < DGV.Rows.Count; i++)
                {
                    sumDiscount += Convert.ToDouble(DGV.Rows[i].Cells[6].Value);
                    sumSubtotal += Convert.ToDouble(DGV.Rows[i].Cells[7].Value);
                }
                lblSubTotal.Text = sumSubtotal.ToString();
                Disc.Text = sumDiscount.ToString();
                var TotAfterDiscount = sumSubtotal - sumDiscount;
                if (VI.ISVAT)
                {
                    var tax = (VI.VatPercent / 100) * TotAfterDiscount;
                    LblVat.Text = tax.ToString();
                    sum = TotAfterDiscount + tax;
                }
                else
                {
                    sum = TotAfterDiscount;
                }
                lblTotalAmount.Text = sum.ToString();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "جمع اعداد الجدول" + "','" + "DGSum" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void SaveAndEdit()
        {
            try
            {
                for (int i = 0; i < DGV.Rows.Count; i++)
                {
                    SPay.sald.InvoiceNo = sal.Invoiceno;
                    SPay.sald.ProductNo = int.Parse(DGV.Rows[i].Cells[0].Value.ToString());
                    SPay.sald.TDDesc = DGV.Rows[i].Cells[1].Value.ToString();
                    SPay.sald.Unitid = int.Parse(DGV.Rows[i].Cells[3].Value.ToString());
                    SPay.sald.ItemPrice = decimal.Parse(DGV.Rows[i].Cells[5].Value.ToString());
                    SPay.sald.Quantity = int.Parse(DGV.Rows[i].Cells[4].Value.ToString());
                    SPay.sald.Subtotal = decimal.Parse(SPay.sald.ItemPrice.ToString()) * decimal.Parse(SPay.sald.Quantity.ToString());
                    SPay.sald.Discount = decimal.Parse(DGV.Rows[i].Cells[6].Value.ToString());
                    SPay.sald.Totafterdiscount = SPay.sald.Subtotal - SPay.sald.Discount;
                    SPay.sald.Total = decimal.Parse(DGV.Rows[i].Cells[7].Value.ToString());
                    SPay.sald.Savesalesdetailes();
                    if (Billtype.Text != "مسوده")
                    {
                        if (CF.Comp.ISUSElineproduction)
                        {
                            IS.Getitemid(SPay.sald.ProductNo);
                            IS.invoiceno = SPay.sal.Invoiceno;
                            IS.Date = DateTime.Now.ToString("yyyy-MM-dd");
                            ISL = IS.ISL;
                            foreach (var item in ISL)
                            {
                                IS.Quantity = item.Quantity;
                                IS.ID = item.ID;
                                IS.Saveitemsales();
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    var Qurcat = @"Update ntailor Set invoiceid = " + sal.Invoiceno + " Where ID = " + int.Parse(textBox3.Text);
                    var CMO = Da.Crudopration(Qurcat);
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "حفظ بيانات المبيعات" + "','" + "SaveAndEdit" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Clearfieldes()
        {
            btnSettlepayment.Text = "حفظ الفاتورة";
            Billtype.SelectedIndex = 1;
            clients.SelectedIndex = 1;
            DGV.Rows.Clear();
            dgwInvoice.Visible = false;
            DGSum();
            Datecheacked.Checked = false;
            QRPic.Image = null;
            txtcustphone.Clear();
            //panelList.Controls.Clear();
            //Setcategories();
            lblCate.Text = string.Empty;
            btnRemoveItem.Visible = true;
            btnSettlepayment.Visible = true;
            txtNote.Clear();
            Newtailor open = Application.OpenForms["Newtailor"] as Newtailor;
            if (open != null)
            {
                this.Close();
            }
        }
        private void LoadInvoices()
        {
            try
            {
                dgwInvoice.Visible = true;
                var GDP = sal.Getdatapos();
                dgwInvoice.DataSource = GDP;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "جلب معلومات الفاتورة" + "','" + "LoadInvoices" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void AddProducttodgv(string Procode)
        {
            CF.Getdatabyid(int.Parse(Procode));
            if (DGV.Rows.Count > 0)
            {
                Chachdgv(int.Parse(Procode));
            }
            else
            {
                AddRondomdgv();
            }
            DGSum();
        }
        private void addCateButton(string CateName, string CateNumber, string backcolor)
        {
            Button button = new Button();
            button.Top = top;
            button.Left = 0;
            button.Height = 80;
            button.Width = 100;
            ctpanel.AutoScroll = true;
            button.BackColor = Color.FromArgb(int.Parse(backcolor)); //Color.DarkOrange;
            button.Text = CateName;
            button.Margin = new Padding(0, 5, 0, 5);
            button.FlatStyle = FlatStyle.Flat;
            button.Tag = "ctTlp_" + CateNumber;
            button.Click += new EventHandler(category_Click);
            ctpanel.Controls.Add(button);
            top += button.Height + 5;
            addCatePanelBtn(CateNumber, backcolor);
        }
        private void addCatePanelBtn(string CateNumber, string Backcolor)
        {
            var productsDt = CF.Getproductbyid(int.Parse(CateNumber));
            var rowCount = productsDt.Rows.Count;
            var columnCount = 3;
            var tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel1.ColumnCount = columnCount;
            //tableLayoutPanel1.RowCount = rowCount;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.Height = panelList.Height;
            tableLayoutPanel1.Width = panelList.Width;
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.ColumnCount = 4;
            //tableLayoutPanel1.RowCount = rowCount;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25));

            for (int i = 0; i < rowCount; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70));
            }
            tableLayoutPanel1.Tag = "ctTlp_" + CateNumber;
            var pageCount = (double)rowCount / 10;
            int PageCount = (int)Math.Ceiling(pageCount);
            int cl = 0, rw = 0;
            for (int i = 0; i < rowCount; i++)
            {
                Button button = new Button();
                button.Tag = productsDt.Rows[i]["ProductNo"].ToString();
                button.Text = CF.GetArabicProductName(productsDt.Rows[i]["Description"].ToString()) + "\r\n" + productsDt.Rows[i]["UnitPrice"].ToString() + "ريال ";
                button.Dock = DockStyle.Top;
                button.BackColor = Color.FromArgb(int.Parse(Backcolor)); //Color.DarkGreen;
                button.ForeColor = Color.White;
                //button.Font = new Font(FontFamily.Families.FirstOrDefault(s => s.Name == "Traditional Arabic"), 12, FontStyle.Regular);
                button.TabIndex = 25;
                button.FlatStyle = FlatStyle.Flat;
                button.Width = 100;
                button.Height = 85;
                button.Click += new EventHandler(productBtn_Click);
                tableLayoutPanel1.Controls.Add(button, cl, rw);
                cl++;
                if (cl == 4)
                {
                    cl = 0;
                    rw++;
                }
            }
            panelList.Controls.Add(tableLayoutPanel1);
        }
        private void category_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            lblCate.Text = bt.Text;
            string btnTag = bt.Tag.ToString();
            var otherPanels = panelList.Controls.OfType<TableLayoutPanel>().Where(c => c.Tag != null && c.Tag.ToString().Contains("ctTlp_") && c.Tag.ToString() != btnTag).ToList();
            foreach (var cbtn in otherPanels)
            {
                cbtn.Visible = false;
            }
            var showBtns = panelList.Controls.OfType<TableLayoutPanel>().Where(c => c.Tag.ToString() == btnTag).ToList();
            foreach (var cbtn in showBtns)
            {
                cbtn.Visible = true;
            }
        }
        private void productBtn_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            string code = bt.Tag.ToString();
            double Qty = 1;
            AddProducttodgv(code);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (DGV.Rows.Count <= 0)
            {
                return;
            }
            RemoveItem();
            DGSum();
        }
        private void btnRePrint_Click(object sender, EventArgs e)
        {
            LoadInvoices();
        }
        private void btnNewTran_Click(object sender, EventArgs e)
        {
            Clearfieldes();
        }
        private void Datecheacked_CheckedChanged(object sender, EventArgs e)
        {
            if (Datecheacked.Checked)
            {
                OldDate.Visible = true;
                lblDate.Visible = true;
            }
            else
            {
                OldDate.Visible = false;
                lblDate.Visible = false;
            }
        }
        public void Cashieroprations()
        {
            SPay.sal.Savesales();
            sal.Invoiceno = SPay.sal.Getlastbyid();
            SaveAndEdit();
            SPay.Invnum = sal.Invoiceno;
            SPay.Pm.Getdata(SPay.Invnum);
            //SPay.txtCash.Text = SPay.Pm.Cash.ToString();
            //SPay.txtBank.Text = SPay.Pm.Bank.ToString();
        }
        private void btnSettlepayment_Click(object sender, EventArgs e)
        {
            if (btnSettlepayment.Text == "حفظ الفاتورة")
            {
                if (clients.SelectedIndex == 0)
                {
                    MessageBox.Show("برجاء ادخال العميل", "خطأ");
                    return;
                }
                else if (DGV.Rows.Count == 0)
                {
                    MessageBox.Show("لا يوجد طلبات", "خطأ");
                    return;
                }
                else
                {
                    SPay = new Salespayments();
                    if (btnSettlepayment.Text == "حفظ الفاتورة")
                    {
                        SPay.Typeprinting = "حفظ كاشير";
                    }
                    else
                    {
                        SPay.Typeprinting = "تعديل كاشير";
                    }
                    SPay.clients.DataSource = clients.DataSource;
                    SPay.clients.SelectedValue = clients.SelectedValue;
                    SPay.txtTotal.Text = lblTotalAmount.Text;
                    SPay.sal.NonVatTotal = double.Parse(lblSubTotal.Text);
                    SPay.sal.Discount = double.Parse(Disc.Text);
                    SPay.sal.VatAmount = double.Parse(LblVat.Text);
                    SPay.sal.TotalAmount = double.Parse(lblTotalAmount.Text);
                    SPay.sal.ThirdPartyID = int.Parse(SPay.clients.SelectedValue.ToString());
                    SPay.sal.Billtype = Billtype.Text;
                    SPay.sal.StaffID = STId;
                    SPay.sal.Note = txtNote.Text;
                    if (btnSettlepayment.Text == "حفظ الفاتورة")
                    {
                        if (!Datecheacked.Checked)
                        {
                            SPay.sal.TDate = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            SPay.sal.TDate = OldDate.Value.ToString("yyyy-MM-dd");
                        }
                        SPay.sal.TTime = DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                    {
                        SPay.sal.Invoiceno = sal.Invoiceno;
                        SPay.sal.TDate = sal.TDate;
                        SPay.sal.TTime = sal.TTime;
                    }
                    SPay.ShowDialog();
                }
            }
            else
            {
                sal.Invoiceno = DgvinvId;
                frmMSalesBill FMS = new frmMSalesBill();
                FMS.Invnum = sal.Invoiceno;
                //FMS.St = Cashier;
                //FMS.Taxinvlist = double.Parse(DGV.CurrentRow.Cells[7].Value.ToString());
                //FMS.Totinvlist = double.Parse(DGV.CurrentRow.Cells[8].Value.ToString());
                //DateTime dt = Convert.ToDateTime(DGV.CurrentRow.Cells[3].Value.ToString() + " " + DGV.CurrentRow.Cells[4].Value.ToString());
                //FMS.Timelist = dt;
                FMS.Printinv();
                Clearfieldes();
            }
        }
        private void txtcustphone_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
        }
        private void txtcustphone_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcustphone.Text))
            {
                var GDT = thir.Getdatabynum(txtcustphone.Text);
                clients.DataSource = GDT;
            }
            else
            {
                Getcustomers();
            }
        }
        private void dgwInvoice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Clearfieldes();
            sal.Invoiceno = int.Parse(dgwInvoice.CurrentRow.Cells[2].Value.ToString());
            string BS = dgwInvoice.CurrentRow.Cells[2].Value.ToString();
            DgvinvId = int.Parse(BS);
            string BT = dgwInvoice.CurrentRow.Cells[4].Value.ToString();
            if (BT == "مسوده")
            {
                btnSettlepayment.Text = "تعديل الفاتورة";
            }
            else
            {
                btnSettlepayment.Text = "طباعه";
            }

            CF.Getsaleslist(sal.Invoiceno);
            SL = CF.SL;
            foreach (var item in SL)
            {
                sal.TDate = item.TDate;
                sal.TTime = item.TTime;
                clients.Text = item.Name;
                lblSubTotal.Text = item.NonVatTotal.ToString();
                Disc.Text = item.Discount.ToString();
                LblVat.Text = item.VatAmount.ToString();
                lblTotalAmount.Text = item.TotalAmount.ToString();
                Billtype.Text = item.Billtype;
                DGV.Rows.Add(
                    item.ProductNo,
                    item.TDDesc,
                    item.UnitType,
                    item.Unitid,
                    item.Quantity,
                    item.ItemPrice,
                    item.Discountdet,
                    item.Total
                    );
            }
            //if (CF.Comp.Taxnumber == "")
            //{

            //}
            //else
            //{
            //    MessageBox.Show("لا يمكن التعديل على فتورة صدرت", "خطأ");
            //    return;
            //}
        }
        private void Disc_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
            //if (!string.IsNullOrEmpty(Disc.Text) && Disc.Text != ".")
            //{
            //    Changedisc();
            //}
        }
        private void lblTotalAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
            //if (!string.IsNullOrEmpty(lblTotalAmount.Text) && lblTotalAmount.Text != ".")
            //{
            //    Changetot();
            //}
        }

        private void Btncustomers_Click(object sender, EventArgs e)
        {
            frmListThirdParty FTP = new frmListThirdParty();
            FTP.radioClient.Checked = true;
            FTP.Show();
        }

        private void dgwInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string BS = dgwInvoice.CurrentRow.Cells[2].Value.ToString();
            DgvinvId = int.Parse(BS);
            string BT = dgwInvoice.CurrentRow.Cells[3].Value.ToString();
            invType = BT;
            if (BT == "مسوده")
            {
                DGV.Rows.Clear();
                CF.Getsaleslist(int.Parse(BS));
                SL = CF.SL;
                foreach (var item in SL)
                {
                    sal.TDate = item.TDate;
                    sal.TTime = item.TTime;
                    clients.Text = item.Name;
                    lblSubTotal.Text = item.NonVatTotal.ToString();
                    Disc.Text = item.Discount.ToString();
                    LblVat.Text = item.VatAmount.ToString();
                    lblTotalAmount.Text = item.TotalAmount.ToString();
                    Billtype.Text = item.PaymentMethod;
                    DGV.Rows.Add(
                        item.ProductNo,
                        item.TDDesc,
                        item.UnitType,
                        item.Unitid,
                        item.Quantity,
                        item.ItemPrice,
                        item.Discountdet,
                        item.Total
                        );
                }
                if (BT == "مسوده")
                {
                    btnSettlepayment.Text = "تعديل الفاتورة";
                }
                else
                {
                    btnRemoveItem.Visible = false;
                    //btnSettlepayment.Visible = false;
                }
                //btnSettlepayment.Text = "طباعه";
                dgwInvoice.Visible = false;
            }
            //if (CF.Comp.Taxnumber == "" || BT == "مسوده")
            //{
            //    btnSettlepayment.Text = "تعديل الفاتورة";
            //    CF.Getsaleslist(int.Parse(BS));
            //    SL = CF.SL;
            //    foreach (var item in SL)
            //    {
            //        sal.TDate = item.TDate;
            //        sal.TTime = item.TTime;
            //        clients.Text = item.Name;
            //        lblSubTotal.Text = item.NonVatTotal.ToString();
            //        Disc.Text = item.Discount.ToString();
            //        LblVat.Text = item.VatAmount.ToString();
            //        lblTotalAmount.Text = item.TotalAmount.ToString();
            //        Billtype.Text = item.PaymentMethod;
            //        DGV.Rows.Add(
            //            item.ProductNo,
            //            item.TDDesc,
            //            item.UnitType,
            //            item.Unitid,
            //            item.Quantity,
            //            item.ItemPrice,
            //            item.Discountdet,
            //            item.Total
            //            );
            //    }
            //}
            if (dgwInvoice.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (CF.Comp.Taxnumber == "" || BT == "مسوده")
                {
                    var Qur = @"SELECT Invoiceno, 
                                   Paynum FROM transactions Where Invoiceno = " + int.Parse(BS);
                    DataAccess DA = new DataAccess();
                    var Paynum = 0;
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        Paynum = int.Parse(GD["Paynum"].ToString());
                    }
                    var Qst = @"Delete From Transactions Where Invoiceno = " + int.Parse(BS);
                    var Qsp = @"Delete From payment Where paymentNo = " + Paynum;
                    var Qsd = @"Delete From salesdetailes Where InvoiceNo = " + int.Parse(BS);
                    var Qss = @"Delete From Sales Where Invoiceno = " + int.Parse(BS);
                    CF.Deletebyid(Qst);
                    CF.Deletebyid(Qsp);
                    CF.Deletebyid(Qsd);
                    CF.Deletebyid(Qss);
                    MessageBox.Show("تم حذف الفاتورة بنجاح", "تم");
                    Clearfieldes();
                    return;
                }
                else
                {
                    MessageBox.Show("لا يمكن حذف على فاتورة صدرت", "خطأ");
                    return;
                }
            }
            if (dgwInvoice.Columns[e.ColumnIndex].Name == "Reprint")
            {
                sal.Invoiceno = int.Parse(BS);
                frmMSalesBill FMS = new frmMSalesBill();
                FMS.Invnum = sal.Invoiceno;
                //FMS.St = Cashier;
                //FMS.Taxinvlist = double.Parse(DGV.CurrentRow.Cells[7].Value.ToString());
                //FMS.Totinvlist = double.Parse(DGV.CurrentRow.Cells[8].Value.ToString());
                //DateTime dt = Convert.ToDateTime(DGV.CurrentRow.Cells[3].Value.ToString() + " " + DGV.CurrentRow.Cells[4].Value.ToString());
                //FMS.Timelist = dt;
                FMS.Printinv();
            }
        }

        private void lblTotalAmount_TextChanged(object sender, EventArgs e)
        {
            Totflag = true;
            var Price = Convert.ToDouble(lblTotalAmount.Text);
            var TBV = Price / 1.15;
            var GTBV = Math.Round(Convert.ToDecimal(TBV), 2).ToString();
            if (VI.ISVAT)
            {
                var Tax = Convert.ToDouble(GTBV) * (VI.VatPercent / 100);
                var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
                //var GT = Math.Round(Convert.ToDecimal(GTax), MidpointRounding.ToEven);

                LblVat.Text = GTax.ToString();

                var TDisc = Convert.ToDouble(lblSubTotal.Text) - TBV;
                var GDisc = Math.Round(Convert.ToDouble(TDisc), 2).ToString();
                if (double.Parse(GDisc) >= 0)
                {
                    Disc.Text = Convert.ToString(GDisc);
                }
                else
                {
                    DGSum();
                    MessageBox.Show("لا يمكن ان يكون الخصم اصغر من صفر","خطأ");
                    return;
                }
            }
            else
            {
                var TB = Convert.ToDouble(lblSubTotal.Text);
                var Tot = Convert.ToDouble(lblTotalAmount.Text);
                var Res = TB - Tot;
                if (Res >= 0)
                {
                    Disc.Text = Res.ToString();
                }
                else
                {
                    DGSum();
                    MessageBox.Show("لا يمكن ان يكون الخصم اصغر من صفر", "خطأ");
                    return;
                }
            }
            Totflag = false;
            //if (Diskflag)
            //{
            //    Totflag = false;
            //    if (!string.IsNullOrEmpty(lblTotalAmount.Text) && lblTotalAmount.Text != ".")
            //    {
            //        Changetot();
            //    }
            //}
            //Totflag = true;
        }

        private void Disc_TextChanged(object sender, EventArgs e)
        {
            if (!Totflag)
            {
                double subTotal = string.IsNullOrEmpty(lblSubTotal.Text) ? 0 : Convert.ToDouble(lblSubTotal.Text);
                double discount = string.IsNullOrEmpty(Disc.Text) ? 0 : Convert.ToDouble(Disc.Text);
                double ST = subTotal - discount;
                if (ST < 0)
                {
                    DGSum();
                    MessageBox.Show("لا يمكن ان يكون الخصم اصغر من صفر", "خطأ");
                    return;
                }
                else
                {
                    if (VI.ISVAT)
                    {
                        var Tax = Convert.ToDouble(ST) * (VI.VatPercent / 100);
                        var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
                        LblVat.Text = GTax.ToString();
                        var Price = ST + Tax;
                        var GPrice = Math.Round(Convert.ToDouble(Price), 2).ToString();
                        lblTotalAmount.Text = GPrice.ToString();
                    }
                    else
                    {
                        lblTotalAmount.Text = ST.ToString();
                    }
                }
            }
            //if (Totflag)
            //{
            //    Diskflag = false;
            //    if (!string.IsNullOrEmpty(Disc.Text) && Disc.Text != ".")
            //    {
            //        Changedisc();
            //    }
            //}
            //Diskflag = true;
        }
    }
}
