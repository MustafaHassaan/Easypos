using Centeralized;
using Dataaccesslaeyr;
using Easypos.Masters;
using Easypos.Masters.Subforms;
using Easypos.Salesforms;
using Easypos.Salesforms.Cashier;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Querylaeyr.Modelmaster;
using Querylaeyr.Tailor;
using Reporting;
using Reporting.Tailor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Application = System.Windows.Forms.Application;
using Image = System.Drawing.Image;

namespace Easypos.Tailoring
{
    public partial class Frmtailoring : Form
    {
        public int STId { get; set; }
        public int Id { get; set; }
        string HTF;
        string BH;
        string Seel;
        string NPD;
        string ND;
        string Showentriangle;
        string Showenbox;
        string Hiddentriangle;
        string Hiddenbox;
        string Chinacloses;
        string Zigzagcloses;
        string HiddenZigzag;
        Centergetingfunctions CF;
        DataAccess DA;
        CompanyInfo Comp;
        byte[] Logo;
        public Frmtailoring()
        {
            InitializeComponent();
            CF = new Centergetingfunctions();
            DA = new DataAccess();
            Getcustomers();
            GAT();
            Comp = new CompanyInfo();
            Comp.GetCompany();
            //GCS();
        }
        public void Clearfildes()
        {
            dgw.Rows.Clear();
            Statustype.SelectedIndex = 0;
            Getcustomers();
            Btnsave.Text = "حفظ";
            Id = 0;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            textBox15.Clear();
            textBox29.Clear();
            textBox26.Clear();
            textBox27.Clear();
            textBox25.Clear();
            textBox24.Clear();
            textBox23.Clear();
            //dgw.Rows.Clear();
            //Getcustomers();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            //textBox15.Clear();
            textBox16.Clear();
            textBox17.Clear();
            textBox18.Clear();
            textBox19.Clear();
            textBox20.Clear();
            textBox21.Clear();
            textBox22.Clear();
            textBox28.Clear();
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
            checkBox11.Checked = false;
            checkBox12.Checked = false;
            checkBox13.Checked = false;
            checkBox14.Checked = false;
            checkBox15.Checked = false;
            checkBox16.Checked = false;
            checkBox17.Checked = false;
            checkBox18.Checked = false;
            checkBox19.Checked = false;
            checkBox23.Checked = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            checkBox20.Checked = false;
            checkBox21.Checked = false;
            checkBox22.Checked = false;
            checkBox24.Checked = false;
            checkBox25.Checked = false;
            checkBox26.Checked = false;
            checkBox27.Checked = false;
            checkBox28.Checked = false;
            checkBox29.Checked = false;
            checkBox30.Checked = false;
            checkBox31.Checked = false;
            checkBox32.Checked = false;
            checkBox33.Checked = false;
            checkBox34.Checked = false;
            checkBox35.Checked = false;
            checkBox36.Checked = false;
            checkBox37.Checked = false;
            checkBox38.Checked = false;
            checkBox39.Checked = false;
            GAT();
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة التفصيل" + "','" + "Getthirdparties" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Neckdetailes()
        {
            if (checkBox19.Checked)
            {
                ND = "طق زرار";
            }
            if (checkBox18.Checked)
            {
                ND = "طق مخفي";
            }
            if (checkBox17.Checked)
            {
                ND = "زرار";
            }
            if (checkBox4.Checked)
            {
                ND = "تركيبة";
            }
        }
        public void NPDT()
        {
            if (checkBox16.Checked)
            {
                NPD = "طق زرار";
            }
            if (checkBox15.Checked)
            {
                NPD = "طق مخفي";
            }
            if (checkBox14.Checked)
            {
                NPD = "زرار";
            }
            if (checkBox13.Checked)
            {
                NPD = "تركيبة";
            }
            if (checkBox25.Checked)
            {
                Chinacloses = checkBox25.Text;
            }
        }
        public void Seelve()
        {
            if (checkBox12.Checked)
            {
                Seel = "كم مدور";
            }
            if (checkBox11.Checked)
            {
                Seel = "كم مربع";
            }
            if (checkBox10.Checked)
            {
                Seel = "كم مقرن";
            }
            if (checkBox9.Checked)
            {
                Seel = "كم قماش حشوة";
            }
        }
        public void buttonhole()
        {
            if (checkBox8.Checked)
            {
                BH = "عروة سادة";
            }
            else
            {
                BH = "";
            }
            if (checkBox7.Checked)
            {
                BH = "عروة منقوش";
            }
            else
            {
                BH = "";
            }
            if (checkBox6.Checked)
            {
                HTF = "عروة سادة بزر";
            }
            else
            {
                BH = "";
            }
            if (checkBox5.Checked)
            {
                HTF = "عروة منقوش بزر";
            }
            else
            {
                BH = "";
            }
        }
        public void Handdetailesflag()
        {
            if (checkBox1.Checked)
            {
                HTF = "يد ساده";
            }
            if (checkBox2.Checked)
            {
                HTF = "يد ساده مفتوح";
            }
            if (checkBox3.Checked)
            {
                HTF = "يد ساده كبك";
            }
        }
        // الجبزور
        public void Gypsumdetailesflag()
        {
            if (checkBox22.Checked)
            {
                Showentriangle = checkBox22.Text;
            }
            else
            {
                Showentriangle = "";
            }
            if (checkBox21.Checked)
            {
                Showenbox = checkBox21.Text;
            }
            else
            {
                Showenbox = "";
            }
            if (checkBox20.Checked)
            {
                Hiddentriangle = checkBox20.Text;
            }
            else
            {
                Hiddentriangle = "";
            }
            if (checkBox24.Checked)
            {
                Hiddenbox = checkBox24.Text;
            }
            else
            {
                Hiddenbox = "";
            }
            if (checkBox26.Checked)
            {
                Zigzagcloses = checkBox26.Text;
            }
            else
            {
                Zigzagcloses = "";
            }
            if (checkBox27.Checked)
            {
                HiddenZigzag = checkBox27.Text;
            }
            else
            {
                HiddenZigzag = "";
            }
        }
        public void GAT()
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
                        tailor.KG,
                        tailor.Chinacloses,
                        tailor.Zigzagcloses,
                        tailor.HiddenZigzagcloses,
                        tailor.H1,
                        tailor.H2,
                        tailor.H3,
                        tailor.H4,
                        tailor.H5,
                        tailor.H6,
                        tailor.H7,
                        tailor.J1,
                        tailor.J2,
                        tailor.J3,
                        tailor.J4,
                        tailor.J5,
                        tailor.J6,
                        tailor.J7,
                        tailor.J8,
                        tailor.J9,
                        tailor.J10,
                        tailor.N1,
                        tailor.N2,
                        tailor.N3,
                        tailor.N4,
                        tailor.N5,
                        tailor.N6,
                        tailor.N7,
                        tailor.N8,
                        tailor.B1,
                        tailor.B2,
                        tailor.B3,
                        tailor.B4,
                        tailor.B5,
                        tailor.B6,
                        tailor.B7,
                        tailor.B8,
                        tailor.B9,
                        tailor.B10,
                        tailor.B11
                        FROM tailor Left Outer Join thirdparty
                        On thirdparty.Id = tailor.Custid";
                DA.Con = new MySqlConnection(DA.CS);
                DA.Con.Open();
                DA.Cmd = new MySqlCommand(Qur, DA.Con);
                DA.dr = DA.Cmd.ExecuteReader();
                while (DA.dr.Read())
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(dgw);
                    newRow.Cells[0].Value = DA.dr["ID"];
                    newRow.Cells[1].Value = DA.dr["Date"];
                    newRow.Cells[2].Value = DA.dr["Name"];
                    // الطول الامامي
                    newRow.Cells[3].Value = DA.dr["Forentlength"];
                    // الكتف
                    newRow.Cells[4].Value = DA.dr["Shouledr"];
                    // طول اليد
                    newRow.Cells[5].Value = DA.dr["Handlength"];
                    // الرقبة
                    newRow.Cells[6].Value = DA.dr["Neck"];
                    // الصدر
                    newRow.Cells[7].Value = DA.dr["Chest"];
                    // الوسع
                    newRow.Cells[8].Value = DA.dr["Width"];
                    // وسع اليد
                    newRow.Cells[9].Value = DA.dr["Handwidthup"];
                    // وسع اسفل
                    newRow.Cells[10].Value = DA.dr["Dowenwidth"];
                    newRow.Cells[11].Value = DA.dr["Clothestotal"];
                    newRow.Cells[12].Value = DA.dr["Handdetailes"];
                    newRow.Cells[13].Value = DA.dr["buttonhole"];
                    newRow.Cells[14].Value = DA.dr["Seelve"];
                    newRow.Cells[15].Value = DA.dr["Seelvea"];
                    newRow.Cells[16].Value = DA.dr["Seelveb"];
                    newRow.Cells[17].Value = DA.dr["NeckPuerdetailes"];
                    newRow.Cells[18].Value = DA.dr["NPDtot"];
                    newRow.Cells[19].Value = DA.dr["NPDA"];
                    newRow.Cells[20].Value = DA.dr["NPDB"];
                    newRow.Cells[21].Value = DA.dr["Neckdetailes"];
                    newRow.Cells[22].Value = DA.dr["NDtot"];
                    newRow.Cells[23].Value = DA.dr["NDA"];
                    newRow.Cells[24].Value = DA.dr["NDB"];
                    newRow.Cells[25].Value = DA.dr["NDO"];
                    newRow.Cells[26].Value = DA.dr["NDC"];
                    newRow.Cells[27].Value = DA.dr["Rent"];
                    newRow.Cells[28].Value = DA.dr["Fabricname"];
                    newRow.Cells[29].Value = DA.dr["Tottal"];
                    newRow.Cells[30].Value = DA.dr["Payed"];
                    newRow.Cells[31].Value = DA.dr["Creditoer"];
                    newRow.Cells[32].Value = DA.dr["Receiptdate"];
                    newRow.Cells[33].Value = DA.dr["Status"];
                    newRow.Cells[34].Value = DA.dr["Note"];
                    newRow.Cells[35].Value = DA.dr["Backlength"];
                    newRow.Cells[36].Value = DA.dr["Handwidthdowen"];
                    newRow.Cells[37].Value = DA.dr["Palmhand"];
                    newRow.Cells[38].Value = DA.dr["Showentriangle"];
                    newRow.Cells[39].Value = DA.dr["Showenbox"];
                    newRow.Cells[40].Value = DA.dr["Hiddentriangle"];
                    newRow.Cells[41].Value = DA.dr["Hiddenbox"];
                    newRow.Cells[42].Value = DA.dr["Statustype"];
                    newRow.Cells[43].Value = DA.dr["KG"];
                    newRow.Cells[44].Value = DA.dr["Chinacloses"];
                    newRow.Cells[45].Value = DA.dr["Zigzagcloses"];
                    newRow.Cells[46].Value = DA.dr["HiddenZigzagcloses"];

                    newRow.Cells[49].Value = DA.dr["H1"];
                    newRow.Cells[50].Value = DA.dr["H2"];
                    newRow.Cells[51].Value = DA.dr["H3"];
                    newRow.Cells[52].Value = DA.dr["H4"];
                    newRow.Cells[53].Value = DA.dr["H5"];
                    newRow.Cells[54].Value = DA.dr["H6"];
                    newRow.Cells[55].Value = DA.dr["H7"];
                    newRow.Cells[56].Value = DA.dr["J1"];
                    newRow.Cells[57].Value = DA.dr["J2"];
                    newRow.Cells[58].Value = DA.dr["J3"];
                    newRow.Cells[59].Value = DA.dr["J4"];
                    newRow.Cells[60].Value = DA.dr["J5"];
                    newRow.Cells[61].Value = DA.dr["J6"];
                    newRow.Cells[62].Value = DA.dr["J7"];
                    newRow.Cells[63].Value = DA.dr["J8"];
                    newRow.Cells[64].Value = DA.dr["J9"];
                    newRow.Cells[65].Value = DA.dr["J10"];
                    newRow.Cells[66].Value = DA.dr["N1"];
                    newRow.Cells[67].Value = DA.dr["N2"];
                    newRow.Cells[68].Value = DA.dr["N3"];
                    newRow.Cells[69].Value = DA.dr["N4"];
                    newRow.Cells[70].Value = DA.dr["N5"];
                    newRow.Cells[71].Value = DA.dr["N6"];
                    newRow.Cells[72].Value = DA.dr["N7"];
                    newRow.Cells[73].Value = DA.dr["N8"];
                    newRow.Cells[74].Value = DA.dr["B1"];
                    newRow.Cells[75].Value = DA.dr["B2"];
                    newRow.Cells[76].Value = DA.dr["B3"];
                    newRow.Cells[77].Value = DA.dr["B4"];
                    newRow.Cells[78].Value = DA.dr["B5"];
                    newRow.Cells[79].Value = DA.dr["B6"];
                    newRow.Cells[80].Value = DA.dr["B7"];
                    newRow.Cells[81].Value = DA.dr["B8"];
                    newRow.Cells[82].Value = DA.dr["B9"];
                    newRow.Cells[83].Value = DA.dr["B10"];
                    newRow.Cells[84].Value = DA.dr["B11"];
                    dgw.Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة التفصيل" + "','" + "GAT" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void GCS()
        {
            try
            {
                dgw.Rows.Clear();
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
                        tailor.KG,
                        tailor.Datewm,
                        tailor.Chinacloses,
                        tailor.Zigzagcloses,
                        tailor.HiddenZigzagcloses
                        FROM tailor Left Outer Join thirdparty
                        On thirdparty.Id = tailor.Custid
                        where thirdparty.Id = '" + clientID.SelectedValue + "'";
                DA.Con = new MySqlConnection(DA.CS);
                DA.Con.Open();
                DA.Cmd = new MySqlCommand(Qur, DA.Con);
                DA.dr = DA.Cmd.ExecuteReader();
                while (DA.dr.Read())
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(dgw);
                    newRow.Cells[0].Value = DA.dr["ID"];
                    newRow.Cells[1].Value = DA.dr["Date"];
                    newRow.Cells[2].Value = DA.dr["Name"];
                    // الطول الامامي
                    newRow.Cells[3].Value = DA.dr["Forentlength"];
                    // الكتف
                    newRow.Cells[4].Value = DA.dr["Shouledr"];
                    // طول اليد
                    newRow.Cells[5].Value = DA.dr["Handlength"];
                    // الرقبة
                    newRow.Cells[6].Value = DA.dr["Neck"];
                    // الصدر
                    newRow.Cells[7].Value = DA.dr["Chest"];
                    // الوسع
                    newRow.Cells[8].Value = DA.dr["Width"];
                    // وسع اليد
                    newRow.Cells[9].Value = DA.dr["Handwidthup"];
                    // وسع اسفل
                    newRow.Cells[10].Value = DA.dr["Dowenwidth"];
                    newRow.Cells[11].Value = DA.dr["Clothestotal"];
                    newRow.Cells[12].Value = DA.dr["Handdetailes"];
                    newRow.Cells[13].Value = DA.dr["buttonhole"];
                    newRow.Cells[14].Value = DA.dr["Seelve"];
                    newRow.Cells[15].Value = DA.dr["Seelvea"];
                    newRow.Cells[16].Value = DA.dr["Seelveb"];
                    newRow.Cells[17].Value = DA.dr["NeckPuerdetailes"];
                    newRow.Cells[18].Value = DA.dr["NPDtot"];
                    newRow.Cells[19].Value = DA.dr["NPDA"];
                    newRow.Cells[20].Value = DA.dr["NPDB"];
                    newRow.Cells[21].Value = DA.dr["Neckdetailes"];
                    newRow.Cells[22].Value = DA.dr["NDtot"];
                    newRow.Cells[23].Value = DA.dr["NDA"];
                    newRow.Cells[24].Value = DA.dr["NDB"];
                    newRow.Cells[25].Value = DA.dr["NDO"];
                    newRow.Cells[26].Value = DA.dr["NDC"];
                    newRow.Cells[27].Value = DA.dr["Rent"];
                    newRow.Cells[28].Value = DA.dr["Fabricname"];
                    newRow.Cells[29].Value = DA.dr["Tottal"];
                    newRow.Cells[30].Value = DA.dr["Payed"];
                    newRow.Cells[31].Value = DA.dr["Creditoer"];
                    newRow.Cells[32].Value = DA.dr["Receiptdate"];
                    newRow.Cells[33].Value = DA.dr["Status"];
                    newRow.Cells[34].Value = DA.dr["Note"];
                    newRow.Cells[35].Value = DA.dr["Backlength"];
                    newRow.Cells[36].Value = DA.dr["Handwidthdowen"];
                    newRow.Cells[37].Value = DA.dr["Palmhand"];
                    newRow.Cells[38].Value = DA.dr["Showentriangle"];
                    newRow.Cells[39].Value = DA.dr["Showenbox"];
                    newRow.Cells[40].Value = DA.dr["Hiddentriangle"];
                    newRow.Cells[41].Value = DA.dr["Hiddenbox"];
                    newRow.Cells[42].Value = DA.dr["Statustype"];
                    newRow.Cells[43].Value = DA.dr["KG"];
                    newRow.Cells[44].Value = DA.dr["Datewm"];
                    newRow.Cells[45].Value = DA.dr["Chinacloses"];
                    newRow.Cells[46].Value = DA.dr["Zigzagcloses"];
                    newRow.Cells[47].Value = DA.dr["HiddenZigzagcloses"];
                    dgw.Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة التفصيل" + "','" + "GCS" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        private void Tailoring_Load(object sender, EventArgs e)
        {
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dgw_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Btnsave.Text = "تعديل";
            if (dgw.Rows.Count > 0)
            {
                Id = int.Parse(dgw.CurrentRow.Cells[0].Value.ToString());

                //DateTime DTA = DateTime.ParseExact(dgw.CurrentRow.Cells[1].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DTA = dgw.CurrentRow.Cells[1].Value.ToString();
                dateTimePicker1.Text = DTA.ToString();
                //dateTimePicker1.Text = dgw.CurrentRow.Cells[1].Value.ToString();
                clientID.Text = dgw.CurrentRow.Cells[2].Value.ToString();
                textBox1.Text = dgw.CurrentRow.Cells[3].Value.ToString();
                textBox26.Text = dgw.CurrentRow.Cells[35].Value.ToString();
                textBox3.Text = dgw.CurrentRow.Cells[6].Value.ToString();
                textBox7.Text = dgw.CurrentRow.Cells[9].Value.ToString();
                textBox27.Text = dgw.CurrentRow.Cells[36].Value.ToString();
                textBox2.Text = dgw.CurrentRow.Cells[4].Value.ToString();
                textBox4.Text = dgw.CurrentRow.Cells[7].Value.ToString();
                textBox8.Text = dgw.CurrentRow.Cells[10].Value.ToString();
                textBox6.Text = dgw.CurrentRow.Cells[5].Value.ToString();
                textBox5.Text = dgw.CurrentRow.Cells[8].Value.ToString();
                textBox22.Text = dgw.CurrentRow.Cells[11].Value.ToString();
                textBox20.Text = dgw.CurrentRow.Cells[15].Value.ToString();
                textBox19.Text = dgw.CurrentRow.Cells[16].Value.ToString();
                textBox9.Text = dgw.CurrentRow.Cells[18].Value.ToString();
                textBox10.Text = dgw.CurrentRow.Cells[19].Value.ToString();
                textBox11.Text = dgw.CurrentRow.Cells[20].Value.ToString();
                textBox14.Text = dgw.CurrentRow.Cells[22].Value.ToString();
                textBox13.Text = dgw.CurrentRow.Cells[23].Value.ToString();
                textBox12.Text = dgw.CurrentRow.Cells[24].Value.ToString();
                var Ret = dgw.CurrentRow.Cells[27].Value.ToString();
                if (Ret == "1")
                {
                    checkBox23.Checked = true;
                }
                textBox18.Text = dgw.CurrentRow.Cells[28].Value.ToString();
                textBox17.Text = dgw.CurrentRow.Cells[29].Value.ToString();
                textBox16.Text = dgw.CurrentRow.Cells[30].Value.ToString();
                textBox21.Text = dgw.CurrentRow.Cells[31].Value.ToString();
                //DateTime DTB = DateTime.ParseExact(dgw.CurrentRow.Cells[32].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //dateTimePicker2.Value = DTB;
                dateTimePicker2.Text = dgw.CurrentRow.Cells[32].Value.ToString();
                textBox24.Text = dgw.CurrentRow.Cells[33].Value.ToString();
                textBox23.Text = dgw.CurrentRow.Cells[34].Value.ToString();
                textBox28.Text = dgw.CurrentRow.Cells[37].Value.ToString();
                Statustype.Text = dgw.CurrentRow.Cells[42].Value.ToString();
                textBox29.Text = dgw.CurrentRow.Cells[43].Value.ToString();

                // اليد
                //كبك مقلوب
                checkBox1.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[49].Value);
                //كبك مربع
                checkBox2.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[50].Value);
                // كبك مقرن
                checkBox10.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[51].Value);
                // كبك مدور
                checkBox12.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[52].Value);
                //يد كبك ساده
                checkBox3.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[53].Value);
                // يد ساده
                checkBox11.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[54].Value);
                // يد مثلث
                checkBox9.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[55].Value);

                // الجبزور
                checkBox20.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[56].Value);
               checkBox24.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[57].Value);
               checkBox8.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[58].Value);
                checkBox27.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[59].Value);
                checkBox22.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[60].Value);
                checkBox21.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[61].Value);
                checkBox26.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[62].Value);
               checkBox7.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[63].Value);
               checkBox6.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[64].Value);
                checkBox5.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[65].Value);

                // الرقبه
                checkBox16.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[66].Value);
               checkBox14.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[67].Value);
               checkBox19.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[68].Value);
              checkBox28.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[69].Value);
              checkBox15.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[70].Value);
              checkBox25.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[71].Value);
              checkBox18.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[72].Value);
               checkBox17.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[73].Value);

                // الجيب
                checkBox36.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[74].Value); 
                checkBox32.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[75].Value);
                checkBox35.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[76].Value);
                checkBox30.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[77].Value);
                checkBox34.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[78].Value);
                checkBox31.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[79].Value);
                checkBox33.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[80].Value);
                checkBox29.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[81].Value);
                checkBox37.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[82].Value);
                checkBox38.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[83].Value);
                checkBox39.Checked = Convert.ToBoolean(dgw.CurrentRow.Cells[84].Value);
            }
            else
            {
                Btnsave.Text = "حفظ";
                return;
            }
        }
        private void Btnclear_Click(object sender, EventArgs e)
        {
            Clearfildes();
        }
        private void dgw_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgw.Columns[e.ColumnIndex].Name == "Delete")
            {
                Id = Convert.ToInt32(dgw.CurrentRow.Cells[0].Value.ToString());
                if (Id == 0)
                {
                    MessageBox.Show("برجاء اختيار الامر", "خطأ");
                    return;
                }
                else
                {
                    var Res = MessageBox.Show("هل تريد حذف هذا الامر", "حذف الامر", MessageBoxButtons.YesNo);
                    if (Res == DialogResult.Yes)
                    {
                        var Qur = "Delete From tailor WHERE ID=@ID ";
                        DA.Con = new MySqlConnection(DA.CS);
                        DA.Con.Open();
                        DA.Cmd = new MySqlCommand(Qur, DA.Con);
                        DA.Cmd.Parameters.AddWithValue("@ID", Id);
                        DA.Cmd.ExecuteNonQuery();
                        Clearfildes();
                        GCS();
                        MessageBox.Show("تم الحذف بنجاح", "حذف امر تفصيل");
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        private void Btnsave_Click(object sender, EventArgs e)
        {
            //جلب اليد
            Handdetailesflag();

            //جلب الجبزور
            Gypsumdetailesflag();
            NPDT();
            Seelve();
            buttonhole();
            Neckdetailes();



            //تفاصيل الرأس
            var Date = dateTimePicker1.Value.ToString("dd-MM-yyyy");
            var Custnumber = textBox15.Text;
            var Custname = clientID.Text;
            // اليد
            //كبك مقلوب
            bool UDH = checkBox1.Checked;
            //كبك مربع
            bool SH = checkBox2.Checked;
            // كبك مقرن
            bool CH = checkBox10.Checked;
            // كبك مدور
            bool RH = checkBox12.Checked;
            //يد كبك ساده
            bool PH = checkBox3.Checked;
            // يد ساده
            bool RGH = checkBox11.Checked;
            // يد مثلث
            bool TH = checkBox9.Checked;


            // الجبزور
            bool FJ = checkBox20.Checked;
            bool SJ = checkBox24.Checked;
            bool TJ = checkBox8.Checked;
            bool FOJ = checkBox27.Checked;
            bool FIJ = checkBox22.Checked;
            bool SIJ = checkBox21.Checked;
            bool SVJ = checkBox26.Checked;
            bool EJ = checkBox7.Checked;
            bool NJ = checkBox6.Checked;
            bool TIJ = checkBox5.Checked;
            // الرقبه
            bool PN = checkBox16.Checked;
            bool PHN = checkBox14.Checked;
            bool PFN = checkBox19.Checked;
            bool CN = checkBox28.Checked;
            bool RT = checkBox15.Checked;
            bool OF = checkBox25.Checked;
            bool CF = checkBox18.Checked;
            bool FFF = checkBox17.Checked;

            // الجيب
            bool Ba = checkBox36.Checked;
            bool Bb = checkBox32.Checked;
            bool Bc = checkBox35.Checked;
            bool Bd = checkBox30.Checked;
            bool Be = checkBox34.Checked;
            bool Bf = checkBox31.Checked;
            bool Bg = checkBox33.Checked;
            bool Bh = checkBox29.Checked;
            bool Bi = checkBox37.Checked;
            bool Bj = checkBox38.Checked;
            bool Bk = checkBox39.Checked;

            if (clientID.Text == "--اختر--" || clientID.Text == "")
            {
                MessageBox.Show("برجاء اختيار العميل", "خطأ");
                return;
            }
            else
            {
                if (Btnsave.Text == "حفظ")
                {
                    var Quradd = "INSERT INTO tailor(Custid,Forentlength,Backlength,Neck,Handwidthup,Handwidthdowen,Shouledr,Chest,Dowenwidth,Handlength,Width,Clothestotal,Handdetailes,buttonhole,Seelve,Seelvea,Seelveb,NeckPuerdetailes,NPDtot,NPDA,NPDB,Neckdetailes,NDtot,NDA,NDB,NDO,NDC,Rent,Fabricname,Tottal,Payed,Creditoer,Receiptdate,Date,Status,Note," +
                                  "Palmhand,Showentriangle,Showenbox,Hiddentriangle,Hiddenbox,Statustype,KG,Chinacloses,Zigzagcloses,HiddenZigzagcloses,H1,H2,H3,H4,H5,H6,H7,J1,J2,J3,J4,J5,J6,J7,J8,J9,J10,N1,N2,N3,N4,N5,N6,N7,N8,B1,B2,B3,B4,B5,B6,B7,B8,B9,B10,B11) " +
                                  "Value(@Custid,@Forentlength,@Backlength,@Neck,@Handwidthup,@Handwidthdowen,@Shouledr,@Chest,@Dowenwidth,@Handlength,@Width,@Clothestotal,@Handdetailes,@buttonhole,@Seelve,@Seelvea,@Seelveb,@NeckPuerdetailes,@NPDtot,@NPDA,@NPDB,@Neckdetailes,@NDtot,@NDA,@NDB,@NDO,@NDC,@Rent,@Fabricname,@Tottal,@Payed,@Creditoer,@Receiptdate,@Date,@Status,@Note," +
                                  "@Palmhand,@Showentriangle,@Showenbox,@Hiddentriangle,@Hiddenbox,@Statustype,@KG,@Chinacloses,@Zigzagcloses,@HiddenZigzagcloses,@H1,@H2,@H3,@H4,@H5,@H6,@H7,@J1,@J2,@J3,@J4,@J5,@J6,@J7,@J8,@J9,@J10,@N1,@N2,@N3,@N4,@N5,@N6,@N7,@N8,@B1,@B2,@B3,@B4,@B5,@B6,@B7,@B8,@B9,@B10,@B11)";
                    DA.Con = new MySqlConnection(DA.CS);
                    DA.Con.Open();
                    DA.Cmd = new MySqlCommand(Quradd,DA.Con);
                    DA.Cmd.Parameters.AddWithValue("@Custid", clientID.SelectedValue);
                    //Forentlength
                    DA.Cmd.Parameters.AddWithValue("@Forentlength", textBox1.Text);
                    //Backlength
                    DA.Cmd.Parameters.AddWithValue("@Backlength", textBox26.Text);
                    DA.Cmd.Parameters.AddWithValue("@Neck", textBox3.Text);
                    //Handwidthup
                    DA.Cmd.Parameters.AddWithValue("@Handwidthup", textBox7.Text);
                    //Handwidthdowen
                    DA.Cmd.Parameters.AddWithValue("@Handwidthdowen", textBox27.Text);
                    DA.Cmd.Parameters.AddWithValue("@Shouledr", textBox2.Text);
                    DA.Cmd.Parameters.AddWithValue("@Chest", textBox4.Text);
                    DA.Cmd.Parameters.AddWithValue("@Dowenwidth", textBox8.Text);
                    DA.Cmd.Parameters.AddWithValue("@Handlength", textBox6.Text);
                    DA.Cmd.Parameters.AddWithValue("@Width", textBox5.Text);
                    DA.Cmd.Parameters.AddWithValue("@Clothestotal", textBox22.Text);
                    DA.Cmd.Parameters.AddWithValue("@Handdetailes", HTF);
                    DA.Cmd.Parameters.AddWithValue("@buttonhole", BH);
                    DA.Cmd.Parameters.AddWithValue("@Seelve", Seel);
                    DA.Cmd.Parameters.AddWithValue("@Seelvea", textBox20.Text);
                    DA.Cmd.Parameters.AddWithValue("@Seelveb", textBox19.Text);
                    DA.Cmd.Parameters.AddWithValue("@NeckPuerdetailes", NPD);
                    DA.Cmd.Parameters.AddWithValue("@NPDtot", textBox9.Text);
                    DA.Cmd.Parameters.AddWithValue("@NPDA", textBox10.Text);
                    DA.Cmd.Parameters.AddWithValue("@NPDB", textBox11.Text);
                    DA.Cmd.Parameters.AddWithValue("@Neckdetailes", ND);
                    DA.Cmd.Parameters.AddWithValue("@NDtot", textBox14.Text);
                    DA.Cmd.Parameters.AddWithValue("@NDA", textBox13.Text);
                    DA.Cmd.Parameters.AddWithValue("@NDB", textBox12.Text);
                    DA.Cmd.Parameters.AddWithValue("@NDO", radioButton2.Checked);
                    DA.Cmd.Parameters.AddWithValue("@NDC", radioButton1.Checked);
                    DA.Cmd.Parameters.AddWithValue("@Rent", checkBox23.Checked);
                    DA.Cmd.Parameters.AddWithValue("@Fabricname", textBox18.Text);
                    DA.Cmd.Parameters.AddWithValue("@Tottal", textBox17.Text);
                    DA.Cmd.Parameters.AddWithValue("@Payed", textBox16.Text);
                    DA.Cmd.Parameters.AddWithValue("@Creditoer", textBox21.Text);
                    DA.Cmd.Parameters.AddWithValue("@Receiptdate", dateTimePicker2.Value.ToString("dd-MM-yyyy"));
                    DA.Cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value.ToString("dd-MM-yyyy"));
                    DA.Cmd.Parameters.AddWithValue("@Status", textBox24.Text);
                    DA.Cmd.Parameters.AddWithValue("@Note", textBox23.Text);

                    DA.Cmd.Parameters.AddWithValue("@Palmhand", textBox28.Text);
                    DA.Cmd.Parameters.AddWithValue("@Showentriangle", Showentriangle);
                    DA.Cmd.Parameters.AddWithValue("@Showenbox", Showenbox);
                    DA.Cmd.Parameters.AddWithValue("@Hiddentriangle", Hiddentriangle);
                    DA.Cmd.Parameters.AddWithValue("@Hiddenbox", Hiddenbox);
                    DA.Cmd.Parameters.AddWithValue("@Statustype", Statustype.Text);
                    DA.Cmd.Parameters.AddWithValue("@KG", textBox29.Text);
                    DA.Cmd.Parameters.AddWithValue("@Chinacloses", Chinacloses);
                    DA.Cmd.Parameters.AddWithValue("@Zigzagcloses", Zigzagcloses);
                    DA.Cmd.Parameters.AddWithValue("@HiddenZigzagcloses", HiddenZigzag);

                    DA.Cmd.Parameters.AddWithValue("@H1", UDH);
                    DA.Cmd.Parameters.AddWithValue("@H2", SH);
                    DA.Cmd.Parameters.AddWithValue("@H3", CH);
                    DA.Cmd.Parameters.AddWithValue("@H4", RH);
                    DA.Cmd.Parameters.AddWithValue("@H5", PH);
                    DA.Cmd.Parameters.AddWithValue("@H6", RGH);
                    DA.Cmd.Parameters.AddWithValue("@H7", TH);

                    DA.Cmd.Parameters.AddWithValue("@J1", FJ);
                    DA.Cmd.Parameters.AddWithValue("@J2", SJ);
                    DA.Cmd.Parameters.AddWithValue("@J3", TJ);
                    DA.Cmd.Parameters.AddWithValue("@J4", FOJ);
                    DA.Cmd.Parameters.AddWithValue("@J5", FIJ);
                    DA.Cmd.Parameters.AddWithValue("@J6", SIJ);
                    DA.Cmd.Parameters.AddWithValue("@J7", SVJ);
                    DA.Cmd.Parameters.AddWithValue("@J8", EJ);
                    DA.Cmd.Parameters.AddWithValue("@J9", NJ);
                    DA.Cmd.Parameters.AddWithValue("@J10", TIJ);
 
                    DA.Cmd.Parameters.AddWithValue("@N1", PN);
                    DA.Cmd.Parameters.AddWithValue("@N2", PHN);
                    DA.Cmd.Parameters.AddWithValue("@N3", PFN);
                    DA.Cmd.Parameters.AddWithValue("@N4", CN);
                    DA.Cmd.Parameters.AddWithValue("@N5", RT);
                    DA.Cmd.Parameters.AddWithValue("@N6", OF);
                    DA.Cmd.Parameters.AddWithValue("@N7", CF);
                    DA.Cmd.Parameters.AddWithValue("@N8", FFF);
                    
                    DA.Cmd.Parameters.AddWithValue("@B1", Ba);
                    DA.Cmd.Parameters.AddWithValue("@B2", Bb);
                    DA.Cmd.Parameters.AddWithValue("@B3", Bc);
                    DA.Cmd.Parameters.AddWithValue("@B4", Bd);
                    DA.Cmd.Parameters.AddWithValue("@B5", Be);
                    DA.Cmd.Parameters.AddWithValue("@B6", Bf);
                    DA.Cmd.Parameters.AddWithValue("@B7", Bg);
                    DA.Cmd.Parameters.AddWithValue("@B8", Bh);
                    DA.Cmd.Parameters.AddWithValue("@B9", Bi);
                    DA.Cmd.Parameters.AddWithValue("@B10", Bj);
                    DA.Cmd.Parameters.AddWithValue("@B11", Bk);

                    DA.Cmd.ExecuteNonQuery();
                    DA.Con.Close();
                    MessageBox.Show("تم الحفظ بنجاح", "اضافة امر تفصيل");
                }
                else
                {
                    if (Id == 0)
                    {
                        MessageBox.Show("برجاء اختيار الامر", "خطأ");
                        return;
                    }
                    else
                    {
                        var Qureid = "Update tailor Set " +
                                      "Custid = @Custid," +
                                      "Forentlength = @Forentlength," +
                                      "Backlength = @Backlength," +
                                      "Neck = @Neck," +
                                      "Handwidthup = @Handwidthup," +
                                      "Handwidthdowen = @Handwidthdowen," +
                                      "Shouledr = @Shouledr," +
                                      "Chest = @Chest," +
                                      "Dowenwidth = @Dowenwidth," +
                                      "Handlength = @Handlength," +
                                      "Width = @Width," +
                                      "Clothestotal = @Clothestotal," +
                                      "Handdetailes = @Handdetailes," +
                                      "buttonhole = @buttonhole," +
                                      "Seelve = @Seelve," +
                                      "Seelvea = @Seelvea," +
                                      "Seelveb = @Seelveb," +
                                      "NeckPuerdetailes = @NeckPuerdetailes," +
                                      "NPDtot = @NPDtot," +
                                      "NPDA = @NPDA," +
                                      "NPDB = @NPDB," +
                                      "Neckdetailes = @Neckdetailes," +
                                      "NDtot = @NDtot," +
                                      "NDA = @NDA," +
                                      "NDB = @NDB," +
                                      "NDO = @NDO," +
                                      "NDC = @NDC," +
                                      "Rent = @Rent," +
                                      "Fabricname = @Fabricname," +
                                      "Tottal = @Tottal," +
                                      "Payed = @Payed," +
                                      "Creditoer = @Creditoer," +
                                      "Receiptdate = @Receiptdate, " +
                                      "Date = @Date, " +
                                      "Status = @Status, " +
                                      "Note = @Note," +
                                      "Palmhand = @Palmhand," +
                                      "Showentriangle = @Showentriangle," +
                                      "Showenbox = @Showenbox," +
                                      "Hiddentriangle = @Hiddentriangle," +
                                      "Hiddenbox = @Hiddenbox, " +
                                      "Statustype = @Statustype, " +
                                      "KG = @KG, " +
                                      "Chinacloses = @Chinacloses, " +
                                      "Zigzagcloses = @Zigzagcloses, " +
                                      "HiddenZigzagcloses = @HiddenZigzagcloses, " +
                                      "H1 = @H1, "+
                                      "H2 = @H2, "+
                                      "H3 = @H3, "+
                                      "H4 = @H4, "+
                                      "H5 = @H5, "+
                                      "H6 = @H6, "+
                                      "H7 = @H7, "+
                                      "J1 = @J1, "+
                                      "J2 = @J2, "+
                                      "J3 = @J3," +
                                      "J4 = @J4,"+
                                      "J5 = @J5,"+
                                      "J6 = @J6,"+
                                      "J7 = @J7,"+
                                      "J8 = @J8,"+
                                      "J9 = @J9,"+
                                      "J10 = @J10," +
                                      "N1 = @N1," +
                                      "N2 = @N2,"+
                                      "N3 = @N3,"+
                                      "N4 = @N4,"+
                                      "N5 = @N5,"+
                                      "N6 = @N6,"+
                                      "N7 = @N7,"+
                                      "N8 = @N8,"+
                                      "B1 = @B1,"+
                                      "B3 = @B3," +
                                      "B4 = @B4,"+
                                      "B5 = @B5,"+
                                      "B6 = @B6,"+
                                      "B7 = @B7,"+
                                      "B8 = @B8,"+
                                      "B9 = @B9,"+
                                      "B10 = @B10,"+
                                      "B11 = @B11 " +
                                      "Where Id = '" + Id + "'";
                        DA.Con = new MySqlConnection(DA.CS);
                        DA.Con.Open();
                        DA.Cmd = new MySqlCommand(Qureid, DA.Con);
                        DA.Cmd.Parameters.AddWithValue("@Custid", clientID.SelectedValue);
                        //Forentlength
                        DA.Cmd.Parameters.AddWithValue("@Forentlength", textBox1.Text);
                        //Backlength
                        DA.Cmd.Parameters.AddWithValue("@Backlength", textBox26.Text);
                        DA.Cmd.Parameters.AddWithValue("@Neck", textBox3.Text);
                        //Handwidthup
                        DA.Cmd.Parameters.AddWithValue("@Handwidthup", textBox7.Text);
                        //Handwidthdowen
                        DA.Cmd.Parameters.AddWithValue("@Handwidthdowen", textBox27.Text);
                        DA.Cmd.Parameters.AddWithValue("@Shouledr", textBox2.Text);
                        DA.Cmd.Parameters.AddWithValue("@Chest", textBox4.Text);
                        DA.Cmd.Parameters.AddWithValue("@Dowenwidth", textBox8.Text);
                        DA.Cmd.Parameters.AddWithValue("@Handlength", textBox6.Text);
                        DA.Cmd.Parameters.AddWithValue("@Width", textBox5.Text);
                        DA.Cmd.Parameters.AddWithValue("@Clothestotal", textBox22.Text);
                        DA.Cmd.Parameters.AddWithValue("@Handdetailes", HTF);
                        DA.Cmd.Parameters.AddWithValue("@buttonhole", BH);
                        DA.Cmd.Parameters.AddWithValue("@Seelve", Seel);
                        DA.Cmd.Parameters.AddWithValue("@Seelvea", textBox20.Text);
                        DA.Cmd.Parameters.AddWithValue("@Seelveb", textBox19.Text);
                        DA.Cmd.Parameters.AddWithValue("@NeckPuerdetailes", NPD);
                        DA.Cmd.Parameters.AddWithValue("@NPDtot", textBox9.Text);
                        DA.Cmd.Parameters.AddWithValue("@NPDA", textBox10.Text);
                        DA.Cmd.Parameters.AddWithValue("@NPDB", textBox11.Text);
                        DA.Cmd.Parameters.AddWithValue("@Neckdetailes", ND);
                        DA.Cmd.Parameters.AddWithValue("@NDtot", textBox14.Text);
                        DA.Cmd.Parameters.AddWithValue("@NDA", textBox13.Text);
                        DA.Cmd.Parameters.AddWithValue("@NDB", textBox12.Text);
                        DA.Cmd.Parameters.AddWithValue("@NDO", radioButton2.Checked);
                        DA.Cmd.Parameters.AddWithValue("@NDC", radioButton1.Checked);
                        DA.Cmd.Parameters.AddWithValue("@Rent", checkBox23.Checked);
                        DA.Cmd.Parameters.AddWithValue("@Fabricname", textBox18.Text);
                        DA.Cmd.Parameters.AddWithValue("@Tottal", textBox17.Text);
                        DA.Cmd.Parameters.AddWithValue("@Payed", textBox16.Text);
                        DA.Cmd.Parameters.AddWithValue("@Creditoer", textBox21.Text);
                        DA.Cmd.Parameters.AddWithValue("@Receiptdate", dateTimePicker2.Value.ToString("dd-MM-yyyy"));
                        DA.Cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value.ToString("dd-MM-yyyy"));
                        DA.Cmd.Parameters.AddWithValue("@Status", textBox24.Text);
                        DA.Cmd.Parameters.AddWithValue("@Note", textBox23.Text);

                        DA.Cmd.Parameters.AddWithValue("@Palmhand", textBox28.Text);
                        DA.Cmd.Parameters.AddWithValue("@Showentriangle", Showentriangle);
                        DA.Cmd.Parameters.AddWithValue("@Showenbox", Showenbox);
                        DA.Cmd.Parameters.AddWithValue("@Hiddentriangle", Hiddentriangle);
                        DA.Cmd.Parameters.AddWithValue("@Hiddenbox", Hiddenbox);
                        DA.Cmd.Parameters.AddWithValue("@Statustype", Statustype.Text);
                        DA.Cmd.Parameters.AddWithValue("@KG", textBox29.Text);
                        DA.Cmd.Parameters.AddWithValue("@Chinacloses", Chinacloses);
                        DA.Cmd.Parameters.AddWithValue("@Zigzagcloses", Zigzagcloses);
                        DA.Cmd.Parameters.AddWithValue("@HiddenZigzagcloses", HiddenZigzag);

                        DA.Cmd.Parameters.AddWithValue("@H1", UDH);
                        DA.Cmd.Parameters.AddWithValue("@H2", SH);
                        DA.Cmd.Parameters.AddWithValue("@H3", CH);
                        DA.Cmd.Parameters.AddWithValue("@H4", RH);
                        DA.Cmd.Parameters.AddWithValue("@H5", PH);
                        DA.Cmd.Parameters.AddWithValue("@H6", RGH);
                        DA.Cmd.Parameters.AddWithValue("@H7", TH);

                        DA.Cmd.Parameters.AddWithValue("@J1", FJ);
                        DA.Cmd.Parameters.AddWithValue("@J2", SJ);
                        DA.Cmd.Parameters.AddWithValue("@J3", TJ);
                        DA.Cmd.Parameters.AddWithValue("@J4", FOJ);
                        DA.Cmd.Parameters.AddWithValue("@J5", FIJ);
                        DA.Cmd.Parameters.AddWithValue("@J6", SIJ);
                        DA.Cmd.Parameters.AddWithValue("@J7", SVJ);
                        DA.Cmd.Parameters.AddWithValue("@J8", EJ);
                        DA.Cmd.Parameters.AddWithValue("@J9", NJ);
                        DA.Cmd.Parameters.AddWithValue("@J10", TIJ);

                        DA.Cmd.Parameters.AddWithValue("@N1", PN);
                        DA.Cmd.Parameters.AddWithValue("@N2", PHN);
                        DA.Cmd.Parameters.AddWithValue("@N3", PFN);
                        DA.Cmd.Parameters.AddWithValue("@N4", CN);
                        DA.Cmd.Parameters.AddWithValue("@N5", RT);
                        DA.Cmd.Parameters.AddWithValue("@N6", OF);
                        DA.Cmd.Parameters.AddWithValue("@N7", CF);
                        DA.Cmd.Parameters.AddWithValue("@N8", FFF);

                        DA.Cmd.Parameters.AddWithValue("@B1", Ba);
                        DA.Cmd.Parameters.AddWithValue("@B2", Bb);
                        DA.Cmd.Parameters.AddWithValue("@B3", Bc);
                        DA.Cmd.Parameters.AddWithValue("@B4", Bd);
                        DA.Cmd.Parameters.AddWithValue("@B5", Be);
                        DA.Cmd.Parameters.AddWithValue("@B6", Bf);
                        DA.Cmd.Parameters.AddWithValue("@B7", Bg);
                        DA.Cmd.Parameters.AddWithValue("@B8", Bh);
                        DA.Cmd.Parameters.AddWithValue("@B9", Bi);
                        DA.Cmd.Parameters.AddWithValue("@B10", Bj);
                        DA.Cmd.Parameters.AddWithValue("@B11", Bk);
                        DA.Cmd.ExecuteNonQuery();
                        DA.Con.Close();
                        MessageBox.Show("تم الحفظ بنجاح", "اضافة امر تفصيل");
                    }
                }
                Clearfildes();
            }
        }
        private void Btnbull_Click(object sender, EventArgs e)
        {
            frmPOS FPos = new frmPOS();
            FPos.STId = this.STId;
            FPos.Show();
            FPos.clients.Text = clientID.Text;
            FPos.textBox3.Text = Id.ToString();
            FPos.dateTimePicker2.Value = dateTimePicker1.Value;
        }
        public void Getcustdetailes()
        {
            try
            {
                dgw.Rows.Clear();
                //Clearfildes();
                DA.Con = new MySqlConnection(DA.CS);
                DA.Con.Open();
                var Qur = @"SELECT MobileNumber FROM thirdparty where Type=2 And ID != 10 And ID = " + clientID.SelectedValue;
                DA.Cmd = new MySqlCommand(Qur, DA.Con);
                DA.dr = DA.Cmd.ExecuteReader();
                if (DA.dr.Read())
                {
                    textBox15.Text = DA.dr["MobileNumber"].ToString();
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة التفصيل" + "','" + "Getcustdetailes" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        private void clientID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Getcustdetailes();
            GCS();
        }
        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            //DA.Con.Close();
            //if (!string.IsNullOrEmpty(textBox15.Text))
            //{
            //    var Qur = "SELECT ID,Name FROM thirdparty where Type=2 And PhoneNumber or MobileNumber Like '%" + textBox15.Text + "%' ";
            //    DA.Cmd = new MySqlCommand(Qur, DA.Con);
            //    DA.Con.Open();
            //    DA.dr = DA.Cmd.ExecuteReader();
            //    if (DA.dr.Read())
            //    {
            //        clientID.SelectedValue = int.Parse(DA.dr["ID"].ToString());
            //    }
            //    else
            //    {
            //        clientID.SelectedValue = 0;
            //        Clearfildes();
            //        dgw.Rows.Clear();
            //    }
            //    GCS();
            //}
            //else
            //{
            //    Getcustomers();
            //    GAT();
            //}
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
        }
        private void Btnreport_Click(object sender, EventArgs e)
        {
            TFR Frmtrf = new TFR();
            Frmtrf.ShowDialog();
        }
        private void Btncustomers_Click(object sender, EventArgs e)
        {
            frmListThirdParty FTP = new frmListThirdParty();
            FTP.radioClient.Checked = true;
            FTP.Show();
        }
        private void Btnsearch_Click(object sender, EventArgs e)
        {
            Frmclient open = Application.OpenForms["Frmclient"] as Frmclient;
            if (open == null)
            {
                Frmclient FC = new Frmclient(this);
                FC.Show();
            }
            else
            {
                open.Activate();
                if (open.WindowState == FormWindowState.Minimized)
                {
                    open.WindowState = FormWindowState.Normal;
                }
            }

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            DA.Con.Close();
            if (!string.IsNullOrEmpty(textBox15.Text))
            {
                var Qur = "SELECT ID,Name FROM thirdparty where Type=2 And PhoneNumber or MobileNumber Like '%" + textBox15.Text + "%' ";
                DA.Cmd = new MySqlCommand(Qur, DA.Con);
                DA.Con.Open();
                DA.dr = DA.Cmd.ExecuteReader();
                if (DA.dr.Read())
                {
                    clientID.SelectedValue = int.Parse(DA.dr["ID"].ToString());
                }
                else
                {
                    clientID.SelectedValue = 0;
                    Clearfildes();
                    dgw.Rows.Clear();
                }
                Getcustdetailes();
                GCS();
            }
            else
            {
                Getcustomers();
                GAT();
            }
        }
        private byte[] ConvertImageToByteArray(Image image)
        {
            if (image == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Dataset ds = new Dataset();

            //تفاصيل الرأس
            var Date = dateTimePicker1.Value.ToString("dd-MM-yyyy");
            var Custnumber = textBox15.Text;
            var Custname = clientID.Text;
            // اليد
            //كبك مقلوب
            byte[] UDH = new byte[0];
            if (checkBox1.Checked)
            {
                UDH = ConvertImageToByteArray(pictureBox1.Image);
                ds.Hand.Rows.Add(UDH);
            }
            //كبك مربع
            byte[] SH = new byte[0];
            if (checkBox2.Checked)
            {
                SH = ConvertImageToByteArray(pictureBox3.Image);
                ds.Hand.Rows.Add(SH);
            }
            // كبك مقرن
            byte[] CH = new byte[0];
            if (checkBox10.Checked)
            {
                CH = ConvertImageToByteArray(pictureBox5.Image);
                ds.Hand.Rows.Add(CH);
            }
            // كبك مدور
            byte[] RH = new byte[0];
            if (checkBox12.Checked)
            {
                RH = ConvertImageToByteArray(pictureBox6.Image);
                ds.Hand.Rows.Add(RH);
            }
            //يد كبك ساده
            byte[] PH = new byte[0];
            if (checkBox3.Checked)
            {
                PH = ConvertImageToByteArray(pictureBox4.Image);
                ds.Hand.Rows.Add(PH);
            }
            // يد ساده
            byte[] RGH = new byte[0];
            if (checkBox11.Checked)
            {
                RGH = ConvertImageToByteArray(pictureBox7.Image);
                ds.Hand.Rows.Add(RGH);
            }
            // يد مثلث
            byte[] TH = new byte[0];
            if (checkBox9.Checked)
            {
                TH = ConvertImageToByteArray(pictureBox8.Image);
                ds.Hand.Rows.Add(TH);
            }


            // الجبزور
            byte[] FJ = new byte[0];
            byte[] SJ = new byte[0];
            byte[] TJ = new byte[0];
            byte[] FOJ = new byte[0];
            byte[] FIJ = new byte[0];
            byte[] SIJ = new byte[0];
            byte[] SVJ = new byte[0];
            byte[] EJ = new byte[0];
            byte[] NJ = new byte[0];
            byte[] TIJ = new byte[0];
            if (checkBox20.Checked)
            {
                FJ = ConvertImageToByteArray(pictureBox9.Image);
                ds.Jabzor.Rows.Add(FJ);
            }
            if (checkBox24.Checked)
            {
                SJ = ConvertImageToByteArray(pictureBox10.Image);
                ds.Jabzor.Rows.Add(SJ);
            }
            if (checkBox8.Checked)
            {
                TJ = ConvertImageToByteArray(pictureBox11.Image);
                ds.Jabzor.Rows.Add(TJ);
            }
            if (checkBox27.Checked)
            {
                FOJ = ConvertImageToByteArray(pictureBox12.Image);
                ds.Jabzor.Rows.Add(FOJ);
            }
            if (checkBox22.Checked)
            {
                FIJ = ConvertImageToByteArray(pictureBox13.Image);
                ds.Jabzor.Rows.Add(FIJ);
            }
            if (checkBox21.Checked)
            {
                SIJ = ConvertImageToByteArray(pictureBox14.Image);
                ds.Jabzor.Rows.Add(SIJ);
            }
            if (checkBox26.Checked)
            {
                SVJ = ConvertImageToByteArray(pictureBox15.Image);
                ds.Jabzor.Rows.Add(SVJ);
            }
            if (checkBox7.Checked)
            {
                EJ = ConvertImageToByteArray(pictureBox16.Image);
                ds.Jabzor.Rows.Add(EJ);
            }
            if (checkBox6.Checked)
            {
                NJ = ConvertImageToByteArray(pictureBox17.Image);
                ds.Jabzor.Rows.Add(NJ);
            }
            if (checkBox5.Checked)
            {
                TIJ = ConvertImageToByteArray(pictureBox18.Image);
                ds.Jabzor.Rows.Add(TIJ);
            }

            // الرقبه
            byte[] PN = new byte[0];
            byte[] PHN = new byte[0];
            byte[] PFN = new byte[0];
            byte[] CN = new byte[0];
            byte[] RT = new byte[0];
            byte[] OF = new byte[0];
            byte[] CF = new byte[0];
            byte[] FFF = new byte[0];
            if (checkBox16.Checked)
            {
                PN = ConvertImageToByteArray(pictureBox19.Image);
                ds.Neck.Rows.Add(PN);
            }
            if (checkBox14.Checked)
            {
                PHN = ConvertImageToByteArray(pictureBox23.Image);
                ds.Neck.Rows.Add(PHN);
            }
            if (checkBox19.Checked)
            {
                PFN = ConvertImageToByteArray(pictureBox20.Image);
                ds.Neck.Rows.Add(PFN);
            }
            if (checkBox28.Checked)
            {
                CN = ConvertImageToByteArray(pictureBox25.Image);
                ds.Neck.Rows.Add(CN);
            }
            if (checkBox15.Checked)
            {
                RT = ConvertImageToByteArray(pictureBox21.Image);
                ds.Neck.Rows.Add(RT);
            }
            if (checkBox25.Checked)
            {
                OF = ConvertImageToByteArray(pictureBox24.Image);
                ds.Neck.Rows.Add(OF);
            }
            if (checkBox18.Checked)
            {
                CF = ConvertImageToByteArray(pictureBox22.Image);
                ds.Neck.Rows.Add(CF);
            }
            if (checkBox17.Checked)
            {
                FFF = ConvertImageToByteArray(pictureBox26.Image);
                ds.Neck.Rows.Add(FFF);
            }

            // الجيب
            byte[] Ba = new byte[0];
            byte[] Bb = new byte[0];
            byte[] Bc = new byte[0];
            byte[] Bd = new byte[0];
            byte[] Be = new byte[0];
            byte[] Bf = new byte[0];
            byte[] Bg = new byte[0];
            byte[] Bh = new byte[0];
            byte[] Bi = new byte[0];
            byte[] Bj = new byte[0];
            byte[] Bk = new byte[0];
            if (checkBox36.Checked)
            {
                Ba = ConvertImageToByteArray(pictureBox34.Image);
                ds.Boket.Rows.Add(Ba);
            }
            if (checkBox32.Checked)
            {
                Bb = ConvertImageToByteArray(pictureBox30.Image);
                ds.Boket.Rows.Add(Bb);
            }
            if (checkBox35.Checked)
            {
                Bc = ConvertImageToByteArray(pictureBox33.Image);
                ds.Boket.Rows.Add(Be);
            }
            if (checkBox30.Checked)
            {
                Bd = ConvertImageToByteArray(pictureBox28.Image);
                ds.Boket.Rows.Add(Bd);
            }
            if (checkBox34.Checked)
            {
                Be = ConvertImageToByteArray(pictureBox32.Image);
                ds.Boket.Rows.Add(Be);
            }
            if (checkBox31.Checked)
            {
                Bf = ConvertImageToByteArray(pictureBox29.Image);
                ds.Boket.Rows.Add(Bf);
            }
            if (checkBox33.Checked)
            {
                Bg = ConvertImageToByteArray(pictureBox31.Image);
                ds.Boket.Rows.Add(Bg);
            }
            if (checkBox29.Checked)
            {
                Bh = ConvertImageToByteArray(pictureBox27.Image);
                ds.Boket.Rows.Add(Bh);
            }
            if (checkBox37.Checked)
            {
                Bi = ConvertImageToByteArray(pictureBox35.Image);
                ds.Boket.Rows.Add(Bi);
            }
            if (checkBox38.Checked)
            {
                Bj = ConvertImageToByteArray(pictureBox36.Image);
                ds.Boket.Rows.Add(Bj);
            }
            if (checkBox39.Checked)
            {
                Bk = ConvertImageToByteArray(pictureBox37.Image);
                ds.Boket.Rows.Add(Bk);
            }

            Tialorder CRA = new Tialorder();
            Frmreporting FCCR = new Frmreporting();
            ds.Custordertile.Rows.Add(new object[] { Date, Custnumber, Custname});
            ds.Detaileordertile.Rows.Add(new object[] { 
                textBox1.Text, textBox26.Text,textBox2.Text,
                textBox6.Text,textBox3.Text, textBox5.Text,
                textBox4.Text, textBox7.Text,textBox27.Text,
                textBox8.Text, textBox28.Text,textBox29.Text,
            });
            //ds.Hand.Rows.Add(new object[] { UDH, SH, CH, RH, PH, RGH, TH, textBox20.Text, textBox19.Text });
            //ds.Hand.Rows.Add(DTH);
            //ds.Jabzor.Rows.Add(new object[] { FJ, SJ, TJ, FOJ, FIJ, SVJ, EJ, NJ, TIJ, SIJ });
            //ds.Neck.Rows.Add(new object[] { PN, PHN, PFN, CN, RT, OF, CF, FFF });
            //ds.Boket.Rows.Add(new object[] { Ba, Bb, Bc, Bd, Be, Bf, Bg, Bh, Bi, Bj, Bk });
            CRA.SetDataSource(ds);

            CRA.SetParameterValue("CompanyName", Comp.CName);
            CRA.SetParameterValue("Taxnum", Comp.Taxnumber);
            CRA.SetParameterValue("Proname", Comp.CRN);
            CRA.SetParameterValue("English_Shop_name", Comp.CENName);
            FCCR.CRV.ReportSource = CRA;
            FCCR.CRV.Refresh();
            FCCR.Show();
            //checkBox5.Checked = false;
            //checkBox6.Checked = false;
            //checkBox7.Checked = false;
            //checkBox8.Checked = false;
            //checkBox13.Checked = false;
            //checkBox14.Checked = false;
            //checkBox15.Checked = false;
            //checkBox16.Checked = false;
            //checkBox17.Checked = false;
            //checkBox18.Checked = false;
            //checkBox19.Checked = false;
            //checkBox23.Checked = false;
            //radioButton1.Checked = false;
            //radioButton2.Checked = false;
            //checkBox20.Checked = false;
            //checkBox21.Checked = false;
            //checkBox22.Checked = false;
            //checkBox24.Checked = false;
            //checkBox25.Checked = false;
            //checkBox26.Checked = false;
            //checkBox27.Checked = false;
            //checkBox28.Checked = false;
        }
    }
}
