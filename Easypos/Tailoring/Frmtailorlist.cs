using Centeralized;
using Dataaccesslaeyr;
using Easypos.Salesforms.Normal;
using Mysqlx.Crud;
using Querylaeyr.Modelmaster;
using Querylaeyr.Modelsales;
using Querylaeyr.Tailor;
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
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Control = System.Windows.Forms.Control;


namespace Easypos.Tailoring
{
    public partial class Frmtailorlist : Form
    {
        Centergetingfunctions CF;
        Tailoroprations TO;
        CompanyInfo Com;
        CompanyInfo CI;
        public Frmtailorlist()
        {
            CI = new CompanyInfo();
            CI.GetCompany();
            Changelang();
            InitializeComponent();
            CF = new Centergetingfunctions();
            Com = new CompanyInfo();
            TO = new Tailoroprations();
            Com.GetCompany();
            Getall();
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
        private void Getall()
        {
            IN.Clear();
            DGV.Rows.Clear();
            Getcustomers();
            Getlistdata();
        }
        public void Getcustomers()
        {
            var GD = CF.Getthirdparties();
            DataRow dr = GD.NewRow();
            dr["ID"] = 0;
            if (CI.Systemlang == "الانجليزية" || CI.Systemlang == "English")
            {
                dr["Name"] = "--Choose--";
            }
            else
            {
                dr["Name"] = "--اختر--";
            }
            GD.Rows.InsertAt(dr, 0);
            clientID.DataSource = GD;
        }
        public void Getlistdata()
        {
            TO.Getdata();
            var data = TO.List;
            foreach (var item in data) {
                DGV.Rows.Add(null,item.Invoiceid,item.ntoid,
                             item.Custid,item.count,item.Invoicedate,
                             item.Receiptdate);
            }
        }
        private void Frmbilllist_Load(object sender, EventArgs e)
        {
        }
        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
           this.Close();
        }
        private void Btnsearch_Click(object sender, EventArgs e)
        {
            DGV.Rows.Clear();
            var invid = IN.Text;
            var orderid = txtorder.Text;
            var Cust = "";
            if (clientID.SelectedValue.ToString() != "0")
            {
                Cust = clientID.SelectedValue.ToString();
            }
            var datefrom = "";
            var dateto = "";
            var Dtf = "";
            if (RBA.Checked)
            {
                Dtf = "Invoice";
                datefrom = DTF.Value.ToString("yyyy-MM-dd");
                dateto = DTT.Value.ToString("yyyy-MM-dd");
            }
            if (RBB.Checked)
            {
                Dtf = "Tailor";
                datefrom = DTF.Value.ToString("yyyy-MM-dd");
                dateto = DTT.Value.ToString("yyyy-MM-dd");
            }
            TO.Selectdatabysearch(invid, orderid, datefrom, dateto, Dtf, Cust);
            var data = TO.List;
            foreach (var item in data)
            {
                DGV.Rows.Add(null, item.Invoiceid, item.ntoid,
                             item.Custid, item.count, item.Invoicedate,
                             item.Receiptdate);
            }
        }
        private void Btnall_Click(object sender, EventArgs e)
        {
            Getall();
        }
        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Columns[e.ColumnIndex].Name == "Show")
            {
                var orderid = int.Parse(DGV.CurrentRow.Cells[2].Value.ToString());
                Newtailor FMS = new Newtailor();
                Newtailor open = Application.OpenForms["Newtailor"] as Newtailor;
                if (open == null)
                {
                    //FMS.Tailid = orderid.ToString();
                    FMS.Selectdata(orderid);
                    FMS.Show();
                    this.Close();
                }
                else
                {
                    open.Activate();
                    if (open.WindowState == FormWindowState.Maximized)
                    {
                        //open.Tailid = orderid.ToString();
                        open.Selectdata(orderid);
                        this.Close();
                    }
                }
            }
        }
    }
}
