using Centeralized;
using Easypos.Purchases;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Xml.Linq;
using ZatcaIntegrationSDK;
using ZatcaIntegrationSDK.APIHelper;
using ZatcaIntegrationSDK.HelperContracts;

namespace Easypos.Masters
{
    public partial class frmSystemSetting : Form
    {
        CompanyInfo CI;
        Cashierinfo CSI;
        Vatinfo VI;
        Centergetingfunctions CGF;
        private Mode mode { get; set; }
        public int Csid { get; set; }
        public frmSystemSetting()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            Loadcashiers();
            CGF = new Centergetingfunctions();
            VI = new Vatinfo();
            VI.GetVat();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                CMDLang.SelectedIndex = 1;
            }
            else
            {
                CMDLang.SelectedIndex = 0;
            }
        }
        public void Loadcashiers()
        {
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                Cashierdgv.Columns[0].HeaderText = "Id";
                Cashierdgv.Columns[1].HeaderText = "System Name";
                Cashierdgv.Columns[2].HeaderText = "System Serial";
                Cashierdgv.Columns[3].HeaderText = "Material Name";
                Cashierdgv.Columns[4].HeaderText = "CSR";
                Cashierdgv.Columns[5].HeaderText = "Private Key";
                Cashierdgv.Columns[6].HeaderText = "Public Key";
                Cashierdgv.Columns[7].HeaderText = "Secret";
                Cashierdgv.Columns[8].HeaderText = "Comid";
            }
            else
            {
                Cashierdgv.Columns[0].HeaderText = "المسلسل";
                Cashierdgv.Columns[1].HeaderText = "اسم السيستم";
                Cashierdgv.Columns[2].HeaderText = "رقم السيستم";
                Cashierdgv.Columns[3].HeaderText = "اسم البرنامج";
                Cashierdgv.Columns[4].HeaderText = "CSR";
                Cashierdgv.Columns[5].HeaderText = "Private Key";
                Cashierdgv.Columns[6].HeaderText = "Public Key";
                Cashierdgv.Columns[7].HeaderText = "Secret";
                Cashierdgv.Columns[8].HeaderText = "رقم الشركه";
            }
            CSI = new Cashierinfo();
            var CD = CSI.Getdata();
            Cashierdgv.DataSource = CD;
        }
        public void Changelang()
        {
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                SetCulture("en");
            }
            else
            {
                SetCulture("ar");
            }
        }
        public void SetCulture(string cultureName)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture =
               System.Globalization.CultureInfo.GetCultureInfo(cultureName);
            var resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            GetChildren(this).ToList().ForEach(c => {
                resources.ApplyResources(c, c.Name);
            });
            this.Controls.Clear();
        }
        public IEnumerable<Control> GetChildren(Control control)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetChildren(ctrl)).Concat(controls);
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void Loadpriters()
        {
            foreach (string printname in PrinterSettings.InstalledPrinters)
            {
                CBPrinters.Items.Add(printname);
            }
        }
        public void Loadcompany()
        {
            txtName.Text = CI.CName;
            txtENName.Text = CI.CENName;
            txtAddress.Text = CI.Address;
            txtPhoneNo.Text = CI.PhoneNo;
            txtTaxnumber.Text = CI.Taxnumber;
            txtRN.Text = CI.CRN;
            txtCompanyname.Text = CI.Logoname;
            txtENCompanyname.Text = CI.LogoENName;
            var img = CI.Logo;
            if (img != null)
            {
                byte[] bytes = Convert.FromBase64String(img);
                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }
                txtLogoPath.Text = "الصوره في المعرض";
                Logo.Image = image;
            }
            CBBarcode.Checked = CI.ISUSEBarcode;
            PrintCheak.Checked = CI.ISUSEPrinter;
            if (CI.ISUSEPrinter)
            {
                CBPrinters.Text = CI.Printername;
            }
            if (CI.ISUSEResturant)
            {
                Restlist.Checked = CI.ISUSEResturant;
            }
            if (CI.ISUSElineproduction)
            {
                Lineproduction.Checked = CI.ISUSElineproduction;
            }
            var PT = CI.Sysnametype;
            switch (PT)
            {
                case "طباعة نظام مبيعات كبير":
                    CBBigPrint.Checked = true;
                    break;
                case "طباعة نظام مبيعات صغير":
                    CBSmalPrint.Checked = true;
                    break;
                case "طباعة نظام مبيعات صغير خياطه":
                    CBTialorPrint.Checked = true;
                    break;
                default:
                    CBBigPrint.Checked = true;
                    break;
            }
            cbzatca.Checked = CI.ISUsePhase2;
            if (cbzatca.Checked)
            {
                txt_seller_street.Text = CI.street;
                txt_seller_buildingnumber.Text = CI.buildingnumber;
                txt_seller_cityname.Text = CI.cityname;
                txt_seller_citysubdiv.Text = CI.citysubdiv;
                txt_seller_postalzone.Text = CI.postalzone;
                txtName.Text = CI.organizationName;
                txtcountryName.Text = CI.countryName;
                txt_location.Text = CI.location;
            }
        }
        public void Showhidprinter()
        {
            if (PrintCheak.Checked)
            {
                Printgroup.Visible = true;
            }
            else
            {
                Printgroup.Visible = false;
            }
        }
        public void Getvatdata()
        {
            if (VI.ISVAT)
            {
                ChkVAT.Checked = VI.ISVAT;
                ChkPWVAT.Checked = VI.PricesWithVAT;
                Chkfpro.Checked = VI.ISDiscforpro;
                Chkfbill.Checked = VI.ISDiscafterpro;
                txtPercent.Text = VI.VatPercent.ToString();
            }
        }
        private void frmSystemSetting_Load(object sender, EventArgs e)
        {
            Loadpriters();
            Loadcompany();
            Showhidprinter();
            Getvatdata();
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtLogoPath.Text = openFileDialog1.FileName;
                Logo.Image = new Bitmap(openFileDialog1.FileName);
            }
        }
        private void txtPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            CGF.Usenumber(e,sender);
        }
        private void PrintCheak_CheckedChanged(object sender, EventArgs e)
        {
            Showhidprinter();
        }
        private void Btncomsave_Click(object sender, EventArgs e)
        {
            if (CI.CompanyID != null)
            {
                CI.CName = txtName.Text ;
                CI.CENName = txtENName.Text ;
                CI.Address = txtAddress.Text ;
                CI.PhoneNo = txtPhoneNo.Text ;
                CI.Taxnumber = txtTaxnumber.Text ;
                CI.CRN = txtRN.Text ;
                CI.Logoname = txtCompanyname.Text ;
                CI.LogoENName = txtENCompanyname.Text;
                if (txtLogoPath.Text != "الصوره في المعرض" && !string.IsNullOrEmpty(txtLogoPath.Text))
                {
                    var Path = txtLogoPath.Text;
                    byte[] imageArray = File.ReadAllBytes(Path);
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                    CI.Logo = base64ImageRepresentation;
                }
                if (cbzatca.Checked)
                {
                    CI.ISUsePhase2 = cbzatca.Checked;
                    CI.street = txt_seller_street.Text;
                    CI.buildingnumber = txt_seller_buildingnumber.Text;
                    CI.cityname = txt_seller_cityname.Text;
                    CI.citysubdiv = txt_seller_citysubdiv.Text;
                    CI.postalzone = txt_seller_postalzone.Text;
                    CI.organizationUnitName = txtName.Text;
                    CI.organizationName = txtName.Text;
                    CI.countryName = txtcountryName.Text;
                    CI.location = txt_location.Text;
                    CI.industry = txt_industry.Text;
                }
                else
                {
                    CI.ISUsePhase2 = cbzatca.Checked;
                    CI.street = "";
                    CI.buildingnumber = "";
                    CI.cityname = "";
                    CI.citysubdiv = "";
                    CI.postalzone = "";
                    CI.organizationUnitName = "";
                    CI.organizationName = "";
                    CI.countryName = "";
                    CI.location = "";
                    CI.industry = "";
                }
                CI.Savedata();
            }
        }
        private void Btnpercentsave_Click(object sender, EventArgs e)
        {
            if (VI.ISVAT != null)
            {
                VI.VatPercent = double.Parse(txtPercent.Text);
                VI.PricesWithVAT = ChkPWVAT.Checked;
                VI.ISDiscforpro = Chkfpro.Checked;
                VI.ISDiscafterpro = Chkfbill.Checked;
                VI.ISVAT = ChkVAT.Checked;
                VI.Savedata();
            }
        }
        public void Affterchangelang()
        {
            Changelang();
            InitializeComponent();
            CI = new CompanyInfo();
            CI.GetCompany();
            Loadpriters();
            Loadcompany();
            Showhidprinter();
            Getvatdata();
            CGF = new Centergetingfunctions();
            VI = new Vatinfo();
            VI.GetVat();
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                CMDLang.SelectedIndex = 1;
            }
            else
            {
                CMDLang.SelectedIndex = 0;
            }
        }
        private void Btnrest_Click(object sender, EventArgs e)
        {
            Frmindex FI = (Application.OpenForms["Frmindex"] as Frmindex);
            if (CI.CompanyID != null)
            {
                CI.ISUSEBarcode = CBBarcode.Checked;
                CI.ISUSEResturant = Restlist.Checked;
                CI.ISUSElineproduction = Lineproduction.Checked;
                if (CI.ISUSEResturant)
                {
                    FI.Resturantmenu.Visible = true;
                }
                else
                {
                    FI.Resturantmenu.Visible = false;
                }
                CI.Systemlang = CMDLang.Text;
                CI.ISUSEPrinter = PrintCheak.Checked;
                if (CBBigPrint.Checked)
                {
                    CI.Sysnametype = "طباعة نظام مبيعات كبير";
                }
                else if (CBSmalPrint.Checked)
                {
                    CI.Sysnametype = "طباعة نظام مبيعات صغير";
                }
                else
                {
                    CI.Sysnametype = "طباعة نظام مبيعات صغير خياطه";
                }
                CI.Printername = CBPrinters.Text;
                CI.Savedata();
                Affterchangelang();
                if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
                {
                    MessageBox.Show("Please restart the program to work properly.", "Operation message");
                }
                else
                {
                    MessageBox.Show("برجاء اعادة تشغيل البرنامج ليعمل بشكل صحيح", "رسالة تشغيل");
                }
            }
        }
        private void cbzatca_CheckedChanged(object sender, EventArgs e)
        {
            if (cbzatca.Checked)
            {
                comdata.Visible = true;
                zatacadata.Visible = true;
            }
            else
            {
                comdata.Visible = false;
                zatacadata.Visible = false;
            }
        }
        private void ChkVAT_CheckedChanged(object sender, EventArgs e)
        {
            if (!ChkVAT.Checked)
            {
                cbzatca.Checked = false;
                comdata.Visible = false;
                zatacadata.Visible = false;
            }
        }
        private void Btnsave_Click(object sender, EventArgs e)
        {
            CSI.SystemName = txt_commonName.Text;
            CSI.Materialname = txt_industry.Text;
            CSI.Systemserial = txt_serialNumber.Text;
            CSI.CSR = txt_csr.Text;
            CSI.PrivateKey = txt_privatekey.Text;
            CSI.PublicKey = txt_publickey.Text;
            CSI.Secret = txt_secret.Text;
            CSI.Comid = 1;
            CSI.Savedata();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            CSI.Id = Csid;
            CSI.SystemName = txt_commonName.Text;
            CSI.Materialname = txt_industry.Text;
            CSI.Systemserial = txt_serialNumber.Text;
            CSI.CSR = txt_csr.Text;
            CSI.PrivateKey = txt_privatekey.Text;
            CSI.PublicKey = txt_publickey.Text;
            CSI.Secret = txt_secret.Text;
            CSI.Comid = 1;
            CSI.Editdata();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CSI.Id = Csid;
            CSI.Deletedata();
        }
        private CertificateRequest GetCSRRequest()
        {
            CertificateRequest certrequest = new CertificateRequest();
            certrequest.OTP = "123456";
            certrequest.CommonName = CI.organizationUnitName;
            certrequest.OrganizationName = CI.organizationUnitName;
            certrequest.OrganizationUnitName = CI.organizationUnitName;
            certrequest.CountryName = "SA";
            certrequest.SerialNumber = txt_serialNumber.Text;
            certrequest.OrganizationIdentifier = CI.Taxnumber;
            certrequest.Location = txt_location.Text;
            certrequest.BusinessCategory = txt_industry.Text;
            certrequest.InvoiceType = "1100";
            return certrequest;
        }
        private void btn_csid_Click(object sender, EventArgs e)
        {
            Invoice inv = new Invoice();
            CertificateRequest certrequest = GetCSRRequest();
            mode = Mode.developer;
            CSIDGenerator generator = new CSIDGenerator(mode);
            CertificateResponse response = generator.GenerateCSID(certrequest, inv, Directory.GetCurrentDirectory());
            if (response.IsSuccess)
            {
                //save to db zatcaCSID table
                // get all certificate data
                txt_csr.Text = response.CSR;
                txt_privatekey.Text = response.PrivateKey;
                txt_publickey.Text = response.CSID;
                txt_secret.Text = response.SecretKey;
                btn_info.Visible = true;
            }
            else
            {
                MessageBox.Show(response.ErrorMessage);
            }
        }
        private string GetCertificateInfo(System.Security.Cryptography.X509Certificates.X509Certificate2 cert)
        {
            string info = "";
            DateTime dt = cert.NotAfter;
            DateTime dt1 = cert.NotBefore;
            info = "Valid From :" + cert.GetEffectiveDateString() + "\n";
            info += "Valid To :" + cert.GetExpirationDateString() + "\n";
            return info;
        }
        private void btn_info_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_publickey.Text))
            {
                sbyte[] certificateBytes = (from x in Encoding.UTF8.GetBytes(txt_publickey.Text)
                                            select (sbyte)x).ToArray();
                System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2((byte[])(object)certificateBytes);
                MessageBox.Show(GetCertificateInfo(cert));
            }
        }
        private void RefreshSerialNumber()
        {
            string input = txt_serialNumber.Text.Trim();
            int index = input.LastIndexOf("|3-");
            if (index >= 0)
            {
                input = input.Substring(0, index + 3);
                txt_serialNumber.Text = input + Guid.NewGuid().ToString();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            RefreshSerialNumber();
        }
        private void Cashierdgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Cashierdgv.Rows.Count > 0)
            {
                Csid = int.Parse(Cashierdgv.CurrentRow.Cells[0].Value.ToString());
                txt_commonName.Text = Cashierdgv.CurrentRow.Cells[1].Value.ToString();
                txt_serialNumber.Text = Cashierdgv.CurrentRow.Cells[2].Value.ToString();
                txt_industry.Text = Cashierdgv.CurrentRow.Cells[3].Value.ToString();
                txt_secret.Text = Cashierdgv.CurrentRow.Cells[8].Value.ToString();
                txt_publickey.Text = Cashierdgv.CurrentRow.Cells[7].Value.ToString();
                txt_privatekey.Text = Cashierdgv.CurrentRow.Cells[6].Value.ToString();
                txt_csr.Text = Cashierdgv.CurrentRow.Cells[5].Value.ToString();
            }
        }
    }
}
