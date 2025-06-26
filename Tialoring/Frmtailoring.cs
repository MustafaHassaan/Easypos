using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Querylaeyr.Tailor;
using Centeralized;
using Dataaccesslaeyr;
using MySql.Data.MySqlClient;

namespace Tialoring
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
        public Frmtailoring()
        {
            InitializeComponent();
            CF = new Centergetingfunctions();
            DA = new DataAccess();
            Getcustomers();
            GAT();
            GCS();
        }
        public void Clearfildes()
        {
            Getcustomers();
            Btnsave.Text = "حفظ";
            Id = 0;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
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
            //GAT();
        }
        public void Getcustomers()
        {
            var GD = CF.Getthirdparties();
            DataRow dr = GD.NewRow();
            dr["ID"] = 0;
            dr["Name"] = "--اختر--";
            GD.Rows.InsertAt(dr, 0);
            clientID.DataSource = GD;
            clientID.SelectedIndex = 1;
        }
        public void Neckdetailes()
        {
            if (checkBox19.Checked)
            {
                NPD = "طق زرار";
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
            if (checkBox7.Checked)
            {
                BH = "عروة منقوش";
            }
            if (checkBox6.Checked)
            {
                HTF = "عروة سادة بزر";
            }
            if (checkBox5.Checked)
            {
                HTF = "عروة منقوش بزر";
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
        public void Gypsumdetailesflag()
        {
            if (checkBox22.Checked)
            {
                Showentriangle = checkBox22.Text;
            }
            if (checkBox22.Checked)
            {
                Showenbox = checkBox21.Text;
            }
            if (checkBox20.Checked)
            {
                Hiddentriangle = checkBox20.Text;
            }
            if (checkBox24.Checked)
            {
                Hiddenbox = checkBox24.Text;
            }
        }
        public void GAT()
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
                        tailor.HiddenZigzagcloses
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
                dgw.Rows.Add(newRow);
            }
        }
        public void GCS()
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
        private void Tailoring_Load(object sender, EventArgs e)
        {
        }
        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dgw_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Clearfildes();
            Btnsave.Text = "تعديل";
            if (dgw.Rows.Count > 0)
            {
                Id = int.Parse(dgw.CurrentRow.Cells[0].Value.ToString());

                //DateTime DTA = DateTime.ParseExact(dgw.CurrentRow.Cells[1].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //var DTA = dgw.CurrentRow.Cells[1].Value.ToString();
                //dateTimePicker1.Text = DTA.ToString();
                dateTimePicker1.Text = dgw.CurrentRow.Cells[1].Value.ToString();
                clientID.Text = dgw.CurrentRow.Cells[2].Value.ToString();
                textBox1.Text = dgw.CurrentRow.Cells[3].Value.ToString();
                textBox26.Text = dgw.CurrentRow.Cells[35].Value.ToString();
                textBox3.Text = dgw.CurrentRow.Cells[4].Value.ToString();
                textBox7.Text = dgw.CurrentRow.Cells[5].Value.ToString();
                textBox27.Text = dgw.CurrentRow.Cells[36].Value.ToString();
                textBox2.Text = dgw.CurrentRow.Cells[6].Value.ToString();
                textBox4.Text = dgw.CurrentRow.Cells[7].Value.ToString();
                textBox8.Text = dgw.CurrentRow.Cells[8].Value.ToString();
                textBox6.Text = dgw.CurrentRow.Cells[9].Value.ToString();
                textBox5.Text = dgw.CurrentRow.Cells[10].Value.ToString();
                textBox22.Text = dgw.CurrentRow.Cells[11].Value.ToString();
                var CBA = dgw.CurrentRow.Cells[12].Value.ToString();
                if (CBA == "يد ساده")
                {
                    checkBox1.Checked = true;
                }
                if (CBA == "يد ساده مفتوح")
                {
                    checkBox2.Checked = true;
                }
                if (CBA == "يد ساده كبك")
                {
                    checkBox3.Checked = true;
                }

                var CBB = dgw.CurrentRow.Cells[13].Value.ToString();
                if (CBB == "عروة سادة")
                {
                    checkBox8.Checked = true;
                }
                if (CBB == "عروة منقوش")
                {
                    checkBox7.Checked = true;
                }
                if (CBB == "عروة سادة بزر")
                {
                    checkBox6.Checked = true;
                }
                if (CBB == "عروة منقوش بزر")
                {
                    checkBox5.Checked = true;
                }

                var CBC = dgw.CurrentRow.Cells[14].Value.ToString();
                if (CBC == "كم مدور")
                {
                    checkBox12.Checked = true;
                }
                if (CBC == "كم مربع")
                {
                    checkBox11.Checked = true;
                }
                if (CBC == "كم مقرن")
                {
                    checkBox10.Checked = true;
                }
                if (CBC == "كم قماش حشوة")
                {
                    checkBox9.Checked = true;
                }
                textBox20.Text = dgw.CurrentRow.Cells[15].Value.ToString();
                textBox19.Text = dgw.CurrentRow.Cells[16].Value.ToString();

                var CBD = dgw.CurrentRow.Cells[17].Value.ToString();
                if (CBD == "طق زرار")
                {
                    checkBox19.Checked = true;
                }
                if (CBD == "طق مخفي")
                {
                    checkBox18.Checked = true;
                }
                if (CBD == "زرار")
                {
                    checkBox17.Checked = true;
                }
                if (CBD == "تركيبة")
                {
                    checkBox4.Checked = true;
                }
                textBox9.Text = dgw.CurrentRow.Cells[18].Value.ToString();
                textBox10.Text = dgw.CurrentRow.Cells[19].Value.ToString();
                textBox11.Text = dgw.CurrentRow.Cells[20].Value.ToString();

                var CBE = dgw.CurrentRow.Cells[21].Value.ToString();
                if (CBE == "طق زرار")
                {
                    checkBox16.Checked = true;
                }
                if (CBE == "طق مخفي")
                {
                    checkBox15.Checked = true;
                }
                if (CBE == "زرار")
                {
                    checkBox14.Checked = true;
                }
                if (CBE == "تركيبة")
                {
                    checkBox13.Checked = true;
                }
                textBox14.Text = dgw.CurrentRow.Cells[22].Value.ToString();
                textBox13.Text = dgw.CurrentRow.Cells[23].Value.ToString();
                textBox12.Text = dgw.CurrentRow.Cells[24].Value.ToString();
                var Open = dgw.CurrentRow.Cells[25].Value.ToString();
                if (Open == "0")
                {
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton2.Checked = true;
                }
                var close = dgw.CurrentRow.Cells[26].Value.ToString();
                if (close == "0")
                {
                    radioButton1.Checked = false;
                }
                else
                {
                    radioButton1.Checked = true;
                }

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


                Showentriangle = dgw.CurrentRow.Cells[38].Value.ToString();
                if (!string.IsNullOrEmpty(Showentriangle))
                {
                    checkBox22.Checked = true;
                }
                Showenbox = dgw.CurrentRow.Cells[39].Value.ToString();
                if (!string.IsNullOrEmpty(Showenbox))
                {
                    checkBox21.Checked = true;
                }
                Hiddentriangle = dgw.CurrentRow.Cells[40].Value.ToString();
                if (!string.IsNullOrEmpty(Hiddentriangle))
                {
                    checkBox20.Checked = true;
                }
                Hiddenbox = dgw.CurrentRow.Cells[41].Value.ToString();
                if (!string.IsNullOrEmpty(Hiddenbox))
                {
                    checkBox24.Checked = true;
                }
                Statustype.Text = dgw.CurrentRow.Cells[42].Value.ToString();
                textBox29.Text = dgw.CurrentRow.Cells[43].Value.ToString();
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
                    //var Res = MessageBox.Show("هل تريد حذف هذا الامر", "حذف الامر", MessageBoxButtons.YesNo);
                    //if (Res == DialogResult.Yes)
                    //{
                    //    var Qur = "Delete From tailor WHERE ID = "+ Id;
                    //    TM.Crudopration(Qur);
                    //    Clearfildes();
                    //    MessageBox.Show("تم الحذف بنجاح","حذف امر تفصيل");
                    //}
                    //else
                    //{
                    //    return;
                    //}
                }
            }
        }
        private void Btnsave_Click(object sender, EventArgs e)
        {
            Gypsumdetailesflag();
            Handdetailesflag();
            NPDT();
            Seelve();
            buttonhole();
            Neckdetailes();
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
                                  "Palmhand,Showentriangle,Showenbox,Hiddentriangle,Hiddenbox,Statustype,KG,Chinacloses,Zigzagcloses,HiddenZigzag) " +
                                  "Value(@Custid,@Forentlength,@Backlength,@Neck,@Handwidthup,@Handwidthdowen,@Shouledr,@Chest,@Dowenwidth,@Handlength,@Width,@Clothestotal,@Handdetailes,@buttonhole,@Seelve,@Seelvea,@Seelveb,@NeckPuerdetailes,@NPDtot,@NPDA,@NPDB,@Neckdetailes,@NDtot,@NDA,@NDB,@NDO,@NDC,@Rent,@Fabricname,@Tottal,@Payed,@Creditoer,@Receiptdate,@Date,@Status,@Note," +
                                  "@Palmhand,@Showentriangle,@Showenbox,@Hiddentriangle,@Hiddenbox,@Statustype,@KG,@Chinacloses,@Zigzagcloses,@HiddenZigzag)";
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
                    DA.Cmd.Parameters.AddWithValue("@HiddenZigzag", HiddenZigzag);

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
                                      "HiddenZigzag = @HiddenZigzag " +
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
                        DA.Cmd.Parameters.AddWithValue("@HiddenZigzag", HiddenZigzag);

                        DA.Cmd.ExecuteNonQuery();
                        DA.Con.Close();
                        MessageBox.Show("تم الحفظ بنجاح", "اضافة امر تفصيل");
                    }
                }
            }
        }
        private void dgw_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
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
                        MessageBox.Show("تم الحذف بنجاح","حذف امر تفصيل");
                        GCS();
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void Btnbull_Click(object sender, EventArgs e)
        {
            frmPOS FPos = new frmPOS(STId);
            FPos.Show();
            FPos.clients.Text = clientID.Text;
            FPos.textBox3.Text = Id.ToString();
            FPos.dateTimePicker1.Value = dateTimePicker1.Value;
        }
    }
}
