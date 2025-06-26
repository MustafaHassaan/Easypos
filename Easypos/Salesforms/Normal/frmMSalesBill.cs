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

using MySql.Data.MySqlClient;
using System.Windows.Controls.Primitives;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using Centeralized;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Controls;
using Querylaeyr.Modelmaster;
using Easypos.Payment;
using Querylaeyr.Modelsales;
using System.IO;
using Image = System.Drawing.Image;
using System.Security.Policy;
using Reporting;
using Reporting.Sales.Normal.Big;
using System.Security.Cryptography;
using Querylaeyr.Paymodel;
using MySqlX.XDevAPI;
using System.Windows.Forms.VisualStyles;
using System.Windows.Documents;
using Querylaeyr.ResturantModles;
using Easypos.Masters;
using CrystalDecisions.CrystalReports.Engine;
using Reporting.Sales.Normal.Small;
using Reporting.Tailor;
using Dataaccesslaeyr;
using java.security.cert;
using java.security;
using System.Runtime.ConstrainedExecution;
using System.Xml;
using ZatcaIntegrationSDK.BLL;
using ZatcaIntegrationSDK.HelperContracts;
using ZatcaIntegrationSDK;
using ZXing.Common;
using ZXing;
using ZatcaIntegrationSDK.APIHelper;
using Zatca;

namespace Easypos.Salesforms.Normal
{
    public partial class frmMSalesBill : Form
    {
        Centergetingfunctions CF;
        Vatinfo VI;
        public List<Saleslist> SL;
        private Mode mode = Mode.developer;
        public string Filename { get; set; }
        Ublxml UXML;
        Invoice inv;
        public int Invnum { get; set; }
        public int Stid { get; set; }
        public string St { get; set; }
        public string Paymethode { get; set; }
        public string Tailor { get; set; }
        public string BT { get; set; }
        public DateTime Time { get; set; }
        CompanyInfo CI;
        Cashierinfo CSI;
        Contractors Cntr;
        Thirdparty Cust;
        Salespayments SPay;
        Itemsales IS;
        DataAccess Da;
        List<Itemsales> ISL;
        bool Diskflag = true;
        bool Totflag = true;
        List<InvoiceItems> invlines;
        public frmMSalesBill()
        {
            InitializeComponent();
            UXML = new Ublxml();
            Da = new DataAccess();
            CF = new Centergetingfunctions();
            CF.Comp = new CompanyInfo();
            CF.Comp.GetCompany();
            VI = new Vatinfo();
            SL = new List<Saleslist>();
            VI.GetVat();
            CSI = new Cashierinfo();
            CSI.GetCashiers();
            if (VI.ISDiscafterpro)
            {
                txtDiscount.Enabled = true;
                txtTotal.Enabled = true;
                DGV.Columns["Discount"].Visible = false;
            }
            if (VI.ISDiscforpro)
            {
                txtDiscount.Enabled = false;
                txtTotal.Enabled = false;
                DGV.Columns["Discount"].Visible = true;
            }
            CI = new CompanyInfo();
            //Billtype.SelectedIndex = 1;
            IS = new Itemsales();
            ISL = new List<Itemsales>();
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
            Decimal.TryParse(txtDiscount.Text, out invoicediscount);
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
                if (CF.Comp.Sysnametype == "طباعة نظام مبيعات صغير خياطه")
                {
                    ReportDocument Rep = new ReportDocument();
                    Frmreporting FR = new Frmreporting();
                    if (CF.Comp.Sysnametype == "طباعة نظام مبيعات صغير خياطه")
                    {
                        Rep = new SmallInv();
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
                    if (Invnum != 0)
                    {
                        CF.Getsaleslist(Invnum);
                        SL = CF.SL;
                        Paymentmodel Pm = new Paymentmodel();
                        Pm.Getdata(Invnum);
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
                        SL = CF.SL;
                        int Countid = 0;
                        var invtyper = "";
                        var Wordofnumber = "";
                        Cntr = new Contractors();
                        Cust = new Thirdparty();
                        string DD = "";
                        string RD = ""; 
                        foreach (var item in SL)
                        {
                            Paymethode = item.PaymentMethod;
                            date = item.TDate;
                            Countid++;
                            DD = item.Tailordate; 
                            RD = item.TailorReceiptdate;
                            if (string.IsNullOrEmpty(DD))
                            {
                                DD = date;
                            }
                            if (string.IsNullOrEmpty(RD))
                            {
                                RD = date;
                            }
                            Totalafterdiscount = Convert.ToString(item.NonVatTotal - item.Discount);
                            Totaldiscount = item.Discount.ToString();
                            Cntr.Purchaseorder = item.Purchaseorder;
                            Cntr.Refrancenumber = item.Refrancenumber;
                            Cntr.Projectname = item.Projectname;
                            invtyper = item.Billtype;
                            Pm.Paid = item.Paid;
                            Pm.Remaining = item.Remaining;
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
                            date = item.TDate.ToString();
                            DateTime dt = Convert.ToDateTime(item.TDate.ToString() + " " + item.TTime.ToString());
                            Time = dt;
                            clientID.Text = item.Name;
                            Cust.Taxnumber = item.Taxnumber;
                            Cust.Address = item.Address;
                            Cust.MobileNumber = item.MobileNumber;
                            txtTBV.Text = item.NonVatTotal.ToString();
                            txtDiscount.Text = item.Discount.ToString();
                            txtTax.Text = item.VatAmount.ToString();
                            txtTotal.Text = item.TotalAmount.ToString();
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
                        Rep.SetParameterValue("Date", date);
                        Rep.SetParameterValue("DD", DD);
                        Rep.SetParameterValue("RD", RD);
                        Rep.SetParameterValue("Discount", txtDiscount.Text);
                        Rep.SetParameterValue("Tax", txtTax.Text);
                        Rep.SetParameterValue("Paied", Pm.Paid);
                        Rep.SetParameterValue("Dept", Pm.Remaining);
                        Rep.PrintOptions.PrinterName = CF.Comp.Printername;
                        Rep.PrintToPrinter(1, true, 1, 1);
                    }
                    else
                    {
                        MessageBox.Show("لا يوجد فاتورة لطباعتها", "خطأ");
                    }
                    //FR.ShowDialog();
                }
                else
                {
                    Frmreporting FR = new Frmreporting();
                    Bigsalesbill Rep = new Bigsalesbill();
                    Dataset Ds = new Dataset();
                    byte[] QRCode = null;
                    string Totaldiscount = "";
                    string Totalafterdiscount = "";
                    string date = "";
                    CF.Getsaleslist(Invnum);
                    SL = CF.SL;
                    if (Invnum != 0)
                    {

                        if (VI.ISVAT && CF.Comp.Taxnumber != "")
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
                            DTP.Text = item.TDate.ToString();
                            DateTime dt = Convert.ToDateTime(item.TDate.ToString() + " " + item.TTime.ToString());
                            Time = dt;
                            clientID.Text = item.Name;
                            Cust.Taxnumber = item.Taxnumber;
                            Cust.Address = item.Address;
                            Cust.MobileNumber = item.MobileNumber;
                            txtTBV.Text = item.NonVatTotal.ToString();
                            txtDiscount.Text = item.Discount.ToString();
                            txtTax.Text = item.VatAmount.ToString();
                            txtTotal.Text = item.TotalAmount.ToString();
                            txtNote.Text = item.Note.ToString();
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
                        item.TotalAmount,
                        Logo,
                        item.MobileNumber,
                        item.Note,
                    });
                        }

                        Rep.SetDataSource(Ds);


                        FR.CRV.ReportSource = Rep;
                        FR.CRV.Refresh();

                        Rep.SetParameterValue("Date", date);
                        Rep.SetParameterValue("CompanyName", CF.Comp.CName);
                        Rep.SetParameterValue("Totalafterdiscount", Totalafterdiscount);
                        Rep.SetParameterValue("Totaldiscount", Totaldiscount);
                        Rep.SetParameterValue("CompanyName", CF.Comp.CName);
                        Rep.SetParameterValue("Cashirname", St);
                        Rep.SetParameterValue("Address", CF.Comp.Address);
                        Rep.SetParameterValue("Taxnum", CF.Comp.Taxnumber);
                        Rep.SetParameterValue("Proname", CF.Comp.CRN);
                        Rep.SetParameterValue("Projectname", Cntr.Projectname);
                        Rep.SetParameterValue("Custaddress", Cust.Address);
                        Rep.SetParameterValue("Custtax", Cust.Taxnumber);
                        Rep.SetParameterValue("Paymethode", Paymethode);
                        Rep.SetParameterValue("Refransenumber", Cntr.Refrancenumber);
                        Rep.SetParameterValue("English_Shop_name", CF.Comp.CENName);
                        Rep.SetParameterValue("Wordofnumber", Wordofnumber);
                    }
                    else
                    {
                        MessageBox.Show("لا يوجد فاتورة لطباعتها", "خطأ");
                    }
                    FR.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "طباعة الفاتورة" + "','" + "Print Sales Data" + "','" + ex.Message.ToString() + "')";
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
                clientID.DataSource = GD;
                clientID.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "جلب معلومات العملاء" + "','" + "Getcustomers" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getproducts()
        {
            try
            {
                var GD = CF.Productfilldata();
                DataRow dr = GD.NewRow();
                dr["ProductNo"] = 0;
                dr["Description"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                cmbproducts.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "جلب معلومات المنتجات" + "','" + "Getproducts" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getunits()
        {
            try
            {
                var GD = CF.Unitfilldata();
                DataRow dr = GD.NewRow();
                dr["ID"] = 0;
                dr["UnitType"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                unitTypes.DataSource = GD;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "جلب معلومات الوحدات" + "','" + "Getunits" + "','" + ex.Message.ToString() + "')";
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
                txtTBV.Text = sumSubtotal.ToString();
                txtDiscount.Text = sumDiscount.ToString();
                var TotAfterDiscount = sumSubtotal - sumDiscount;
                if (VI.ISVAT)
                {
                    var tax = (VI.VatPercent / 100) * TotAfterDiscount;
                    txtTax.Text = tax.ToString();
                    sum = TotAfterDiscount + tax;
                }
                else
                {
                    sum = TotAfterDiscount;
                }
                txtTotal.Text = sum.ToString();
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "جمع معلومات الجدول" + "','" + "DGSum" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Changetot()
        {
            try
            {
                txtDiscount.Text = "0";
                txtTax.Text = "0";

                var Price = Convert.ToDouble(txtTotal.Text);
                var TBV = Price / 1.15;
                var GTBV = Math.Round(Convert.ToDecimal(TBV), 2).ToString();
                if (VI.ISVAT)
                {
                    var Tax = Convert.ToDouble(GTBV) * (VI.VatPercent / 100);
                    var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
                    //var GT = Math.Round(Convert.ToDecimal(GTax), MidpointRounding.ToEven);

                    txtTax.Text = GTax.ToString();

                    var Disc = Convert.ToDouble(txtTBV.Text) - TBV;
                    var GDisc = Math.Round(Convert.ToDouble(Disc), 2).ToString();
                    txtDiscount.Text = Convert.ToString(GDisc);
                }
                else
                {
                    var TB = Convert.ToDouble(txtTBV.Text);
                    var Tot = Convert.ToDouble(txtTotal.Text);
                    var Res = TB - Tot;
                    txtDiscount.Text = Res.ToString();
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
                    var TAD = Convert.ToDouble(txtTBV.Text) - Convert.ToDouble(txtDiscount.Text);
                    var Tax = TAD * (VI.VatPercent / 100);
                    var GTax = Math.Round(Convert.ToDecimal(Tax), 2).ToString();
                    //                var GT = Math.Round(Convert.ToDecimal(GTax), MidpointRounding.ToEven);

                    txtTax.Text = Convert.ToString(GTax);

                    //المبلغ بعد الضريبة
                    var TAV = Convert.ToDouble(txtTax.Text) + TAD;
                    txtTotal.Text = Convert.ToString(TAV);
                }
                else
                {
                    var TB = Convert.ToDouble(txtTBV.Text);
                    var Tot = Convert.ToDouble(txtDiscount.Text);
                    var Res = TB - Tot;
                    txtTotal.Text = Res.ToString();
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "تغيير الخصم" + "','" + "Changedisc" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void AddRondomdgv()
        {
            try
            {
                var Totbd = Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtPrice.Text);
                DGV.Rows.Add(cmbproducts.SelectedValue,
                 cmbproducts.Text,
                 unitTypes.Text,
                 unitTypes.SelectedValue,
                 txtQuantity.Text,
                 txtPrice.Text, 0,
                 Totbd);
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "اضافة المعلومات للجدول" + "','" + "AddRondomdgv" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Chachdgv()
        {
            try
            {
                Boolean Flag = false;
                int DI;
                for (DI = 0; DI < DGV.Rows.Count; DI++)
                {
                    var PC = DGV.Rows[DI].Cells[0].Value.ToString();
                    var Qty = Convert.ToDecimal(DGV.Rows[DI].Cells[4].Value);
                    if (PC == cmbproducts.SelectedValue.ToString())
                    {
                        Flag = true;
                        break;
                    }
                }
                if (Flag)
                {
                    DGV.Rows[DI].Cells[4].Value = Convert.ToDecimal(DGV.Rows[DI].Cells[4].Value.ToString()) + Convert.ToDecimal(txtQuantity.Text);
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "التحقق من الجدول" + "','" + "Chachdgv" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void AddProducttodgv()
        {
            if (unitTypes.Text == "" || unitTypes.SelectedIndex == 0)
            {
                MessageBox.Show("برجاء ادخال وحدات لكي يمكن اضافة المنتجات", "خطأ");
                return;
            }
            if (txtQuantity.Text == "" || txtQuantity.Text == "0" || txtQuantity.Text == ".")
            {
                MessageBox.Show("ادخل الكمية", "خطأ");
                return;
            }
            if (DGV.Rows.Count > 0)
            {
                Chachdgv();
            }
            else
            {
                AddRondomdgv();
            }
            DGSum();
        }
        public void Cleardata()
        {
            if (BT == "مسوده")
            {
                Billtype.SelectedIndex = 0;
            }
            else
            {
                Billtype.SelectedIndex = 1;
            }
            txtPrice.Clear();
            txtQuantity.Clear();
            QRPic.Image = null;
            txtRefranse.Clear();
            txtProjectname.Clear();
            Salesorder.Clear();
        }
        public void Selectfromunit()
        {
            try
            {
                if (cmbproducts.SelectedValue != null)
                {
                    Cleardata();
                    if (cmbproducts.SelectedIndex != 0)
                    {
                        var data = CF.fillunitid(int.Parse(cmbproducts.SelectedValue.ToString()));
                        if (data != null)
                        {
                            unitTypes.SelectedValue = data["Unitid"].ToString();
                            txtPrice.Text = data["UnitPrice"].ToString();
                        }
                        var GS = CF.GetStock(int.Parse(cmbproducts.SelectedValue.ToString()));
                        if (GS != null)
                        {
                            lblStockOnHand.Text = GS.ToString();
                        }
                    }
                    else
                    {
                        Getunits();
                        Cleardata();
                    }
                }
                else
                {
                    Getunits();
                    Cleardata();
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "تحديد الوحدات" + "','" + "Selectfromunit" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void SaveAndEdit()
        {
            try
            {
                for (int i = 0; i < DGV.Rows.Count; i++)
                {
                    SPay.sald.InvoiceNo = SPay.sal.Invoiceno;
                    SPay.sald.ProductNo = int.Parse(DGV.Rows[i].Cells[0].Value.ToString());
                    SPay.sald.TDDesc = DGV.Rows[i].Cells[1].Value.ToString();
                    SPay.sald.Unitid = int.Parse(DGV.Rows[i].Cells[3].Value.ToString());
                    SPay.sald.ItemPrice = decimal.Parse(DGV.Rows[i].Cells[5].Value.ToString());
                    SPay.sald.Quantity = int.Parse(DGV.Rows[i].Cells[4].Value.ToString());
                    SPay.sald.Subtotal = decimal.Parse(SPay.sald.ItemPrice.ToString()) * decimal.Parse(SPay.sald.Quantity.ToString());
                    SPay.sald.Discount = int.Parse(DGV.Rows[i].Cells[6].Value.ToString());
                    SPay.sald.Totafterdiscount = decimal.Parse(SPay.sald.Subtotal.ToString()) - decimal.Parse(SPay.sald.Discount.ToString());
                    SPay.sald.Total = decimal.Parse(DGV.Rows[i].Cells[7].Value.ToString());
                    SPay.sald.Savesalesdetailes();
                    if (Billtype.Text != "مسوده")
                    {
                        if (CF.Comp.ISUSElineproduction)
                        {
                            IS.Getitemid(SPay.sald.ProductNo);
                            IS.invoiceno = SPay.sal.Invoiceno;
                            IS.Date = DTP.Value.ToString("yyyy-MM-dd");
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
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "حفظ المبيعات" + "','" + "SaveAndEdit" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Salesoprations()
        {
            SPay.sal.Savesales();
            SPay.sal.Getlastbyid();
            SaveAndEdit();
        }
        public void Chkfildesandsave(string Printing)
        {
            try
            {
                SL = CF.SL;
                string Date = "";
                string time = "";
                if (clientID.SelectedIndex == 0)
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
                    SPay.Typeprinting = Printing;
                    SPay.clients.DataSource = clientID.DataSource;
                    SPay.clients.SelectedValue = clientID.SelectedValue;
                    SPay.txtTotal.Text = txtTotal.Text;
                    SPay.Cntr.Projectname = txtProjectname.Text;
                    SPay.Cntr.Refrancenumber = txtRefranse.Text;
                    SPay.Cntr.Purchaseorder = Salesorder.Text;
                    SPay.sal.NonVatTotal = double.Parse(txtTBV.Text);
                    SPay.sal.Discount = double.Parse(txtDiscount.Text);
                    SPay.sal.VatAmount = double.Parse(txtTax.Text);
                    SPay.sal.TotalAmount = double.Parse(txtTotal.Text);
                    SPay.sal.ThirdPartyID = int.Parse(SPay.clients.SelectedValue.ToString());
                    SPay.sal.Billtype = Billtype.Text;
                    SPay.sal.StaffID = Stid;
                    if (Btnsave.Text == "حفظ")
                    {
                        //SPay.sal.TDate = DateTime.Now.ToString("yyyy-MM-dd");
                        SPay.sal.TDate = DTP.Value.ToString("yyyy-MM-dd");
                        SPay.sal.TTime = DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                    {
                        foreach (var item in SL)
                        {
                            Date = item.TDate;
                            time = item.TTime;
                        }
                        SPay.sal.TDate = Date;
                        SPay.sal.TTime = time;
                        if (Invnum != 0)
                        {
                            SPay.Invnum = Invnum;
                            SPay.Pm.Getdata(SPay.Invnum);
                            SPay.txtCash.Text = SPay.Pm.Cash.ToString();
                            SPay.txtBank.Text = SPay.Pm.Bank.ToString();
                        }
                    }
                    SPay.sal.Note = txtNote.Text;
                    SPay.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "التحقق من الحقول للمبيعات" + "','" + "Chkfildesandsave" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public void Getinvoice()
        {
            try
            {
                if (Invnum != 0)
                {
                    DGV.Rows.Clear();
                    CF.Getsaleslist(Invnum);
                    SL = CF.SL;
                    foreach (var item in SL)
                    {
                        DTP.Text = item.TDate.ToString();
                        DateTime dt = Convert.ToDateTime(item.TDate.ToString() + " " + item.TTime.ToString());
                        Time = dt;
                        clientID.Text = item.Name;
                        txtTBV.Text = item.NonVatTotal.ToString();
                        txtDiscount.Text = item.Discount.ToString();
                        txtTax.Text = item.VatAmount.ToString();
                        txtTotal.Text = item.TotalAmount.ToString();
                        Salesorder.Text = item.Purchaseorder;
                        txtProjectname.Text = item.Projectname;
                        txtRefranse.Text = item.Refrancenumber;
                        Paymethode = item.PaymentMethod;
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
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "جلب الفاتورة" + "','" + "Getinvoice" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        public double Taxinvlist { get; set; }
        public double Totinvlist { get; set; }
        public DateTime Timelist { get; set; }
        public void Generatqrcode()
        {
            try
            {
                var Tot = double.Parse(txtTotal.Text);
                var Tax = double.Parse(txtTax.Text);
                if (Tot == 0 && Tax == 0)
                {
                    Tot = Totinvlist;
                    Tax = Taxinvlist;
                    Time = Timelist;
                }
                var GDtlv = CF.DQ(Tot, Tax, Time);
                QRPic.Image = GDtlv;
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "انشاء QR Code" + "','" + "Generatqrcode" + "','" + ex.Message.ToString() + "')";
                var CMO = Da.Crudopration(Qurcat);
            }
        }
        private void frmSalesBill_Load(object sender, EventArgs e) 
        {
            Getproducts();
            Getunits();
            Getcustomers();
            Getinvoice();
        }
        private void products_SelectedIndexChanged(object sender, EventArgs e) {
            Selectfromunit();
        }
        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e) {
            CF.Usenumber(e,sender);
        }
        private void Btnbilllist_Click(object sender, EventArgs e) {
            Frmbilllist FBL = new Frmbilllist();
            Frmbilllist open = Application.OpenForms["Frmbilllist"] as Frmbilllist;
            if (open == null)
            {
                FBL.Cashier = St;
                FBL.Show();
            }
            else
            {
                open.Activate();
                if (open.WindowState == FormWindowState.Minimized)
                {
                    open.Cashier = St;
                    open.WindowState = FormWindowState.Normal;
                }
            }
        }
        private void txtBarcode_TextChanged(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(txtBarcode.Text))
            {
                cmbproducts.SelectedValue = txtBarcode.Text;
            }
            else
            {
                Getproducts();
                Getunits();
            }
        }
        private void products_SelectionChangeCommitted(object sender, EventArgs e) {
            Selectfromunit();
        }
        private void Btnadd_Click(object sender, EventArgs e)
        {
            AddProducttodgv();
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
            if (e.KeyChar == (char)13)
            {
                AddProducttodgv();
            }
        }
        private void txtTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
        }
        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Columns[e.ColumnIndex].Name == "Delete")
            {
                int rowIndex = DGV.CurrentCell.RowIndex;
                DGV.Rows.RemoveAt(rowIndex);
                DGSum();
            }
        }
        private void Btnsave_Click(object sender, EventArgs e)
        {
            Chkfildesandsave(Btnsave.Text);
        }
        private void Btnprint_Click(object sender, EventArgs e)
        {
            Printinv();
        }
        private void Btnsaveandprint_Click(object sender, EventArgs e)
        {
            Chkfildesandsave(Btnsaveandprint.Text);   
        }

        private void Btncustomers_Click(object sender, EventArgs e)
        {
            frmListThirdParty FTP = new frmListThirdParty();
            FTP.radioClient.Checked = true;
            FTP.Show();
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            Totflag = true;
            var Price = Convert.ToDouble(txtTotal.Text);
            var TBV = Price / 1.15;
            var GTBV = Math.Round(Convert.ToDecimal(TBV), 2).ToString();
            if (VI.ISVAT)
            {
                var Tax = Convert.ToDouble(GTBV) * (VI.VatPercent / 100);
                var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
                //var GT = Math.Round(Convert.ToDecimal(GTax), MidpointRounding.ToEven);

                txtTax.Text = GTax.ToString();

                var Disc = Convert.ToDouble(txtTBV.Text) - TBV;
                var GDisc = Math.Round(Convert.ToDouble(Disc), 2).ToString();
                txtDiscount.Text = Convert.ToString(GDisc);
            }
            else
            {
                var TB = Convert.ToDouble(txtTBV.Text);
                var Tot = Convert.ToDouble(txtTotal.Text);
                var Res = TB - Tot;
                txtDiscount.Text = Res.ToString();
            }
            Totflag = false;
            //if (Diskflag)
            //{
            //    Totflag = false;
            //    if (!string.IsNullOrEmpty(txtTotal.Text) && txtTotal.Text != ".")
            //    {
            //        Changetot();
            //    }
            //}
            //Totflag = true;
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if (!Totflag)
            {
                var ST = Convert.ToDouble(txtTBV.Text) - Convert.ToDouble(txtDiscount.Text);
                if (VI.ISVAT)
                {
                    var Tax = Convert.ToDouble(ST) * (VI.VatPercent / 100);
                    var GTax = Math.Round(Convert.ToDouble(Tax), 2).ToString();
                    txtTax.Text = GTax.ToString();
                    var Price = ST + Tax;
                    var GPrice = Math.Round(Convert.ToDouble(Price), 2).ToString();
                    txtTotal.Text = GPrice.ToString();
                }
                else
                {
                    txtTotal.Text = ST.ToString();
                }
            }
            //if (Totflag)
            //{
            //    Diskflag = false;
            //    if (!string.IsNullOrEmpty(txtDiscount.Text) && txtDiscount.Text != ".")
            //    {
            //        Changedisc();
            //    }
            //}
            //Diskflag = true;
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.CurrentCell.ColumnIndex == 4 || DGV.CurrentCell.ColumnIndex == 5)
            {
                var C1 = Convert.ToDouble(DGV.CurrentRow.Cells[4].Value.ToString());
                var C2 = Convert.ToDouble(DGV.CurrentRow.Cells[5].Value);
                var Res = C1 * C2;
                DGV.CurrentRow.Cells[7].Value = Res;
                DGSum();
            }
            if (DGV.CurrentCell.ColumnIndex == 7)
            {
                var C1 = Convert.ToDouble(DGV.CurrentRow.Cells[4].Value.ToString());
                var C2 = Convert.ToDouble(DGV.CurrentRow.Cells[5].Value);
                var Res = C1 * C2;
                DGV.CurrentRow.Cells[7].Value = Res;
                DGSum();
            }
        }
    }
}
