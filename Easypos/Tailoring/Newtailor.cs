using Centeralized;
using Dataaccesslaeyr;
using Easypos.Masters;
using Easypos.Masters.Subforms;
using Easypos.Salesforms.Cashier;
using FontAwesome.Sharp;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Querylaeyr.Modelmaster;
using Querylaeyr.Tailor;
using Reporting.Tailor;
using Reporting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Easypos.Tailoring
{
    public partial class Newtailor : Form
    {
        Centergetingfunctions CF;
        DataAccess DA;
        public int staffid { get; set; }
        public int TOId { get; set; }
        public string Tailid { get; set; }
        CompanyInfo Comp;
        byte[] Logo;
        public Newtailor()
        {
            InitializeComponent();
            CF = new Centergetingfunctions();
            DA = new DataAccess();
            Comp = new CompanyInfo();
            Comp.GetCompany();
            Logo = Convert.FromBase64String(Comp.Logo);
            Getcustomers();
            Gettailor();
            Getcatlist();
            Getfabric();
            Getfabrictype();
            Gettailrotype();
            Statustype.SelectedIndex = 0;
        }
        public void Getorders(int id)
        {
            Tailoroprations Ord = new Tailoroprations();
            var GD = Ord.GetorderCombodata(id);
            DataRow dr = GD.NewRow();
            dr["Id"] = 0;
            //dr["Id"] = "--اختر--";
            GD.Rows.InsertAt(dr, 0);
            comboBox1.DataSource = GD;
            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "Id";

        }
        public void Getcatlist()
        {
            Category Cat = new Category();
            var GD = Cat.GetCombodata();
            DataRow dr = GD.NewRow();
            dr["CategoryNo"] = 0;
            dr["Description"] = "--اختر--";
            GD.Rows.InsertAt(dr, 0);
            comboBox3.DataSource = GD;
            comboBox3.ValueMember = "CategoryNo";
            comboBox3.DisplayMember = "Description";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة التفصيل اسم العميل" + "','" + "Getthirdparties" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Gettailor()
        {
            try
            {
                empcreator EC = new empcreator();
                var GD = EC.Getdatabytailor();
                DataRow dr = GD.NewRow();
                dr["Id"] = 0;
                dr["Empname"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                comboBox5.DataSource = GD;
                comboBox5.ValueMember = "Id";
                comboBox5.DisplayMember = "Empname";
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة التفصيل اسم الخياط" + "','" + "Getthirdparties" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Getfabric()
        {
            try
            {
                Fabric EC = new Fabric();
                var GD = EC.GetCombodata();
                DataRow dr = GD.NewRow();
                dr["Id"] = 0;
                dr["Fabricname"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                comboBox7.DataSource = GD;
                comboBox7.ValueMember = "Id";
                comboBox7.DisplayMember = "Fabricname";
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة التفصيل اسم الخياط" + "','" + "Getthirdparties" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Getfabrictype()
        {
            try
            {
                Fabrictype EC = new Fabrictype();
                var GD = EC.GetCombodata();
                DataRow dr = GD.NewRow();
                dr["Id"] = 0;
                dr["Typename"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                comboBox8.DataSource = GD;
                comboBox8.ValueMember = "Id";
                comboBox8.DisplayMember = "Typename";
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة التفصيل اسم الخياط" + "','" + "Getthirdparties" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        public void Gettailrotype()
        {
            try
            {
                tailortype EC = new tailortype();
                var GD = EC.GetCombodata();
                DataRow dr = GD.NewRow();
                dr["Id"] = 0;
                dr["Type"] = "--اختر--";
                GD.Rows.InsertAt(dr, 0);
                comboBox4.DataSource = GD;
                comboBox4.ValueMember = "Id";
                comboBox4.DisplayMember = "Type";
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة التفصيل اسم الخياط" + "','" + "Getthirdparties" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
            }
        }
        private void iconButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox15.Text))
            {
                DA.Con = new MySqlConnection(DA.CS);
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
                }
            }
            else
            {
                Getcustomers();
            }
        }
        private void Btnsearch_Click(object sender, EventArgs e)
        {
            Frmclient FC = new Frmclient(this);
            FC.Show();
        }
        private void Btncustomers_Click(object sender, EventArgs e)
        {
            frmListThirdParty FTP = new frmListThirdParty();
            FTP.radioClient.Checked = true;
            FTP.Show();
        }
        private void iconButton3_Click(object sender, EventArgs e)
        {
            frmPOS FO = new frmPOS();
            FO.textBox3.Text = Tailid;
            FO.clients.Text = clientID.Text;
            FO.txtNote.Text = textBox23.Text;
            FO.ShowDialog();
        }
        private void iconButton6_Click(object sender, EventArgs e)
        {
            Tailoroprations TO = new Tailoroprations();
            Tailordetailes TD = new Tailordetailes();
            Tailor Tail = new Tailor();
            Orders Ord;
            var Custid = int.Parse(clientID.SelectedValue.ToString());
            if (Custid > 1)
            {
                if (iconButton6.Text == "حفظ")
                {
                    Tail.Invoicedate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                    Tail.Receiptdate = dateTimePicker3.Value.ToString("yyyy-MM-dd");
                    Tail.Custid = int.Parse(clientID.SelectedValue.ToString());
                    var invid = comboBox2.SelectedValue;
                    if (invid == null)
                    {
                        Tail.Invoiceid = 0;
                    }
                    else
                    {
                        Tail.Invoiceid = int.Parse(comboBox2.SelectedValue.ToString());
                    }
                    Tail.Userid = staffid;
                    Tail.note = textBox23.Text;
                    TO.AddTialor(Tail);


                    TD.Frontlength = textBox14.Text;
                    TD.Backlength = textBox13.Text;
                    TD.Shoulder = textBox12.Text;
                    TD.Plainarmlength = textBox8.Text;
                    TD.Cuffarmlength = textBox19.Text;
                    TD.Plainneck = textBox18.Text;
                    TD.Turnneck = textBox17.Text;
                    TD.Width = textBox16.Text;
                    TD.Handwidth = textBox21.Text;
                    TD.Middlehand = textBox20.Text;
                    TD.Headhand = textBox32.Text;
                    TD.Plaincuff = textBox31.Text;
                    TD.Cufflength = textBox30.Text;
                    TD.Cuffwidth = textBox29.Text;
                    TD.collar = textBox28.Text;
                    TD.Bottomwidth = textBox27.Text;
                    TD.Hide = textBox26.Text;
                    TD.Mobilehider = textBox25.Text;
                    TD.Middlechest = textBox24.Text;
                    TD.Middleside = textBox22.Text;
                    TD.Weight = textBox3.Text;
                    TO.AddTialdetail(TD);
                    if (DGV.Rows.Count > 0)
                    {
                        for (int i = 0; i < DGV.Rows.Count; i++)
                        {
                            Ord = new Orders();
                            Ord.Tailortype = int.Parse(DGV.Rows[i].Cells[3].Value.ToString());
                            Ord.clothnational = int.Parse(DGV.Rows[i].Cells[4].Value.ToString());
                            Ord.fabricid = int.Parse(DGV.Rows[i].Cells[5].Value.ToString());
                            Ord.fabrictypeid = int.Parse(DGV.Rows[i].Cells[6].Value.ToString());
                            Ord.fabriccount = decimal.Parse(DGV.Rows[i].Cells[2].Value.ToString());
                            Ord.count = int.Parse(DGV.Rows[i].Cells[7].Value.ToString());
                            Ord.empid = int.Parse(DGV.Rows[i].Cells[8].Value.ToString());
                            Ord.Receivedcount = int.Parse(DGV.Rows[i].Cells[9].Value.ToString());
                            Ord.Remainingcount = int.Parse(DGV.Rows[i].Cells[10].Value.ToString());
                            Ord.Status = DGV.Rows[i].Cells[11].Value.ToString();
                            TO.AddOrders(Ord);
                        }
                    }
                    else
                    {
                        Ord = new Orders();
                        Ord.Tailortype = 0;
                        Ord.clothnational = 0;
                        Ord.fabricid = 0;
                        Ord.fabrictypeid = 0;
                        Ord.fabriccount = 0;
                        Ord.count = 0;
                        Ord.empid = 0;
                        Ord.Receivedcount = 0;
                        Ord.Remainingcount = 0;
                        Ord.Status = "";
                        TO.AddOrders(Ord);
                    }

                    TOId = TO.NTIOId;
                    Tailid = TO.NTId.ToString();
                    iconButton3.Enabled = true;
                }
                else
                {
                    var Qurtail = @"Update ntailor Set Invoicedate = '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'," +
                                                      "Receiptdate = '" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "'," +
                                                      "Custid = " + int.Parse(clientID.SelectedValue.ToString()) + "," +
                                                      "Invoiceid = " + int.Parse(comboBox2.SelectedValue.ToString()) + "," +
                                                      "Userid = " + staffid + "," +
                                                      "note = '" + textBox23.Text + "' Where Id =" + int.Parse(Tailid);
                    TO.Editdata(Qurtail);
                    var Qurtaildet = @"Update ntailordetailes Set Frontlength = '" + textBox14.Text + "'," +
                                                                 "Backlength = '" + textBox13.Text + "'," +
                                                                 "Shoulder = '" + textBox12.Text + "'," +
                                                                 "Plainarmlength = '" + textBox8.Text + "'," +
                                                                 "Cuffarmlength = '" + textBox19.Text + "'," +
                                                                 "Plainneck = '" + textBox18.Text + "'," +
                                                                 "Turnneck = '" + textBox17.Text + "'," +
                                                                 "Width = '" + textBox16.Text + "'," +
                                                                 "Handwidth = '" + textBox21.Text + "'," +
                                                                 "Middlehand = '" + textBox20.Text + "'," +
                                                                 "Headhand = '" + textBox32.Text + "'," +
                                                                 "Plaincuff = '" + textBox31.Text + "'," +
                                                                 "Cufflength = '" + textBox30.Text + "'," +
                                                                 "Cuffwidth = '" + textBox29.Text + "'," +
                                                                 "collar = '" + textBox28.Text + "'," +
                                                                 "Bottomwidth = '" + textBox27.Text + "'," +
                                                                 "Hide = '" + textBox26.Text + "'," +
                                                                 "Mobilehider = '" + textBox25.Text + "'," +
                                                                 "Middlechest = '" + textBox24.Text + "'," +
                                                                 "Middleside = '" + textBox22.Text + "', " +
                                                                 "Weight = '" + textBox3.Text + "' " +
                                                                 "Where Id = " + TOId;
                    ;
                    TO.Editdata(Qurtaildet);
                    if (DGV.Rows.Count > 0)
                    {
                        for (int i = 0; i < DGV.Rows.Count; i++)
                        {
                            Ord = new Orders();
                            Ord.Tailortype = int.Parse(DGV.Rows[i].Cells[3].Value.ToString());
                            Ord.clothnational = int.Parse(DGV.Rows[i].Cells[4].Value.ToString());
                            Ord.fabricid = int.Parse(DGV.Rows[i].Cells[5].Value.ToString());
                            Ord.fabrictypeid = int.Parse(DGV.Rows[i].Cells[6].Value.ToString());
                            Ord.fabriccount = decimal.Parse(DGV.Rows[i].Cells[2].Value.ToString());
                            Ord.count = int.Parse(DGV.Rows[i].Cells[7].Value.ToString());
                            Ord.empid = int.Parse(DGV.Rows[i].Cells[8].Value.ToString());
                            Ord.Receivedcount = int.Parse(DGV.Rows[i].Cells[9].Value.ToString());
                            Ord.Remainingcount = int.Parse(DGV.Rows[i].Cells[10].Value.ToString());
                            Ord.Status = DGV.Rows[i].Cells[11].Value.ToString();
                            Ord.Id = int.Parse(DGV.Rows[i].Cells[12].Value.ToString());
                            Ord.ntid = int.Parse(Tailid);
                            Ord.ntoid = TOId;
                            TO.NTId = Ord.ntid;
                            TO.NTIOId = Ord.ntoid;
                            TO.EditOrders(Ord);
                        }
                    }
                    else
                    {
                        Ord = new Orders();
                        Ord.Tailortype = 0;
                        Ord.clothnational = 0;
                        Ord.fabricid = 0;
                        Ord.fabrictypeid = 0;
                        Ord.fabriccount = 0;
                        Ord.count = 0;
                        Ord.empid = 0;
                        Ord.Receivedcount = 0;
                        Ord.Remainingcount = 0;
                        Ord.Status = "";
                        TO.AddOrders(Ord);
                    }
                }
                Clearfilds();
                MessageBox.Show("تم حفظ الطلب بنجاح","تم");
            }
            else
            {
                MessageBox.Show("برجاء اختيار العميل","خطأ");
                return;
            }
        }
        public void cleardata()
        {
            comboBox3.SelectedValue = 0;
            comboBox4.SelectedValue = 0;
            comboBox5.SelectedValue = 0;
            comboBox7.SelectedValue = 0;
            comboBox8.SelectedValue = 0;
            Statustype.SelectedIndex = 0;
            textBox35.Text = "0.0";
            textBox37.Text = "0";
            textBox4.Text = "0";
            textBox5.Text = "0";
        }
        private void iconButton13_Click(object sender, EventArgs e)
        {
            var Id = 0;
            var CB1 = comboBox1.Text;
            if (CB1 != "" || CB1 != "0")
            {
                Id = int.Parse(comboBox1.SelectedValue.ToString());
            }
            DGV.Rows.Add(comboBox7.Text, comboBox8.Text, textBox35.Text,
                         comboBox3.SelectedValue.ToString(),
                         comboBox4.SelectedValue.ToString(),
                         comboBox7.SelectedValue.ToString(),
                         comboBox8.SelectedValue.ToString(),
                         textBox37.Text,
                         comboBox5.SelectedValue.ToString(),
                         textBox5.Text, 
                         textBox4.Text,
                         Statustype.Text,Id);
            cleardata();
        }
        public void Clearfilds()
        {
            iconButton3.Enabled = false;
            comboBox2.DataSource = null;
            textBox2.Clear();
            textBox15.Text = "";
            comboBox1.DataSource = null;
            Statustype.SelectedIndex = 0;
            textBox5.Text = "0";
            textBox4.Text = "0";
            iconButton6.Text = "حفظ";
            textBox3.Text = "0.0";
            textBox23.Clear();
            clientID.SelectedValue = 1;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now;
            textBox14.Text = "0.0";
            textBox13.Text = "0.0";
            textBox12.Text = "0.0";
            textBox8.Text = "0.0";
            textBox19.Text = "0.0";
            textBox18.Text = "0.0";
            textBox17.Text = "0.0";
            textBox16.Text = "0.0";
            textBox21.Text = "0.0";
            textBox20.Text = "0.0";
            textBox32.Text = "0.0";
            textBox31.Text = "0.0";
            textBox30.Text = "0.0";
            textBox29.Text = "0.0";
            textBox28.Text = "0.0";
            textBox27.Text = "0.0";
            textBox26.Text = "0.0";
            textBox25.Text = "0.0";
            textBox24.Text = "0.0";
            textBox22.Text = "0.0";
            comboBox3.SelectedValue = 0;
            comboBox4.SelectedValue = 0;
            comboBox5.SelectedValue = 0;
            comboBox7.SelectedValue = 0;
            comboBox8.SelectedValue = 0;
            textBox35.Text = "0.0";
            textBox37.Text = "0";
            textBox2.Clear();
            DGV.Rows.Clear();
        }
        private void iconButton10_Click(object sender, EventArgs e)
        {
            var id = int.Parse(Tailid);
            Selectdata(id);
        }
        public void Selectdata(int id)
        {
            Tailoroprations TO = new Tailoroprations();
            TO.Selectdata(id);
            var data = TO.List;
            foreach (var item in data)
            {
                var Id = item.Id;
                Tailid = item.ntid;
                TOId = int.Parse(item.ntoid);
                textBox23.Text = item.note.ToString();
                clientID.SelectedValue = int.Parse(item.Custid.ToString());
                dateTimePicker2.Text = item.Invoicedate.ToString();
                dateTimePicker3.Text = item.Receiptdate.ToString();
                textBox14.Text = item.Frontlength.ToString();
                textBox13.Text = item.Backlength.ToString();
                textBox12.Text = item.Shoulder.ToString();
                textBox8.Text = item.Plainarmlength.ToString();
                textBox19.Text = item.Cuffarmlength.ToString();
                textBox18.Text = item.Plainneck.ToString();
                textBox17.Text = item.Turnneck.ToString();
                textBox16.Text = item.Width.ToString();
                textBox21.Text = item.Handwidth.ToString();
                textBox20.Text = item.Middlehand.ToString();
                textBox32.Text = item.Headhand.ToString();
                textBox31.Text = item.Plaincuff.ToString();
                textBox30.Text = item.Cufflength.ToString();
                textBox29.Text = item.Cuffwidth.ToString();
                textBox28.Text = item.collar.ToString();
                textBox27.Text = item.Bottomwidth.ToString();
                textBox26.Text = item.Hide.ToString();
                textBox25.Text = item.Mobilehider.ToString();
                textBox24.Text = item.Middlechest.ToString();
                textBox22.Text = item.Middleside.ToString();
                textBox3.Text = item.Weight.ToString();
                comboBox3.SelectedValue = int.Parse(item.Tailortype.ToString());
                comboBox4.SelectedValue = int.Parse(item.clothnational.ToString());
                comboBox5.SelectedValue = int.Parse(item.empid.ToString());
                comboBox7.SelectedValue = int.Parse(item.fabricid.ToString());
                comboBox8.SelectedValue = int.Parse(item.fabrictypeid.ToString());
                textBox35.Text = item.fabriccount.ToString();
                textBox37.Text = item.count.ToString();
                Statustype.Text = item.Status.ToString();
                textBox5.Text = item.Receivedcount.ToString();
                textBox4.Text = item.Remainingcount.ToString();
                DGV.Rows.Add(comboBox7.Text,
                             comboBox8.Text,
                             textBox35.Text,
                             comboBox3.SelectedValue.ToString(),
                             comboBox4.SelectedValue.ToString(),
                             comboBox7.SelectedValue.ToString(),
                             comboBox8.SelectedValue.ToString(),
                             textBox37.Text,
                             comboBox5.SelectedValue.ToString(),
                             textBox5.Text,
                             textBox4.Text,
                             Statustype.Text,Id);
                cleardata();
            }
            iconButton6.Text = "تعديل";
        }
        private void iconButton2_Click(object sender, EventArgs e)
        {
            var data = Tailid;
            if (data != null)
            {
                Tailoroprations TO = new Tailoroprations();
                var Qurorders = "Delete From orders Where ntid = " + int.Parse(Tailid);
                var Qurtiailor = "Delete From ntailor Where Id = " + int.Parse(Tailid);
                var Qurtiailordetailes = "Delete From ntailordetailes Where Id = " + TOId;
                TO.DeleteOrders(Qurorders);
                TO.DeleteOrders(Qurtiailor);
                TO.DeleteOrders(Qurtiailordetailes);
                MessageBox.Show("تم حذف الامر بنجاح", "تم");
            }
            else
            {
                MessageBox.Show("برجاء اختيار الامر", "خطأ");
                return;
            }
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            Clearfilds();
        }
        private void iconButton5_Click(object sender, EventArgs e)
        {
            Frmtailorlist FMS = new Frmtailorlist();
            Frmtailorlist open = Application.OpenForms["Frmtailorlist"] as Frmtailorlist;
            if (open == null)
            {
                FMS.Show();
            }
            else
            {
                open.Activate();
            }
        }
        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Columns[e.ColumnIndex].Name == "Delete")
            {
                int rowIndex = DGV.CurrentCell.RowIndex;
                DGV.Rows.RemoveAt(rowIndex);
            }
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Rows.Count > 0)
            {
                comboBox3.SelectedValue = int.Parse(DGV.CurrentRow.Cells[3].Value.ToString());
                comboBox4.SelectedValue = int.Parse(DGV.CurrentRow.Cells[4].Value.ToString());
                comboBox5.SelectedValue = int.Parse(DGV.CurrentRow.Cells[8].Value.ToString());
                comboBox7.SelectedValue = int.Parse(DGV.CurrentRow.Cells[5].Value.ToString());
                comboBox8.SelectedValue = int.Parse(DGV.CurrentRow.Cells[6].Value.ToString());
                Statustype.Text = DGV.CurrentRow.Cells[11].Value.ToString();
                textBox35.Text = DGV.CurrentRow.Cells[2].Value.ToString();
                textBox37.Text = DGV.CurrentRow.Cells[7].Value.ToString();
                textBox4.Text = DGV.CurrentRow.Cells[10].Value.ToString();
                textBox5.Text = DGV.CurrentRow.Cells[9].Value.ToString();
            }
        }
        public void clearforsearch()
        {
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now;
            textBox3.Text = "0.0";
            textBox5.Text = "0";
            textBox4.Text = "0";
            textBox14.Text = "0.0";
            textBox13.Text = "0.0";
            textBox12.Text = "0.0";
            textBox8.Text = "0.0";
            textBox19.Text = "0.0";
            textBox18.Text = "0.0";
            textBox17.Text = "0.0";
            textBox16.Text = "0.0";
            textBox21.Text = "0.0";
            textBox20.Text = "0.0";
            textBox32.Text = "0.0";
            textBox31.Text = "0.0";
            textBox30.Text = "0.0";
            textBox29.Text = "0.0";
            textBox28.Text = "0.0";
            textBox27.Text = "0.0";
            textBox26.Text = "0.0";
            textBox25.Text = "0.0";
            textBox24.Text = "0.0";
            textBox22.Text = "0.0";
            textBox35.Text = "0.0";
            textBox37.Text = "0";

            comboBox2.DataSource = null;
            textBox2.Clear();

            comboBox3.SelectedValue = 0;
            comboBox4.SelectedValue = 0;
            comboBox5.SelectedValue = 0;
            comboBox7.SelectedValue = 0;
            comboBox8.SelectedValue = 0;
            Statustype.SelectedIndex = 0;
            DGV.Rows.Clear();
            iconButton6.Text = "حفظ";
            iconButton3.Enabled = false;
        }
        private void iconButton4_Click(object sender, EventArgs e)
        {
            var orderid = int.Parse(comboBox1.SelectedValue.ToString());
            if (orderid > 0)
            {
                clearforsearch();
                Selectdata(orderid);
                Getallsalesforcustomer();
                Cheackinvoice(orderid);
            }
            else
            {
                Clearfilds();
                return;
            }
        }
        private void clientID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var id = int.Parse(clientID.SelectedValue.ToString());
            if (id>0)
            {
                Getorders(id);
                Getcustdata(id);
            }
        }
        public void Getcustdata(int id)
        {
            Thirdparty Thr = new Thirdparty();
            Thr.Getdatabyid(id);
            textBox15.Text = Thr.MobileNumber;
        }
        public void Cheackinvoice(int id)
        {
            Tailoroprations TO = new Tailoroprations();
            TO.Getinvoicedata(id);
            comboBox2.Text = TO.salesInvoiceno;
            textBox2.Text = TO.salesTotalAmount;
            if (comboBox2.Text != "" && textBox2.Text != "") {
                iconButton3.Enabled = false;
            }
            else
            {
                iconButton3.Enabled=true;
            }
        }
        public DataTable Getalesdata()
        {
            try
            {
                var Qur = @"SELECT
                            Sales.Invoiceno
                            FROM Sales
                            Where ThirdPartyID = " + int.Parse(clientID.SelectedValue.ToString()) +" ORDER BY Sales.Invoiceno DESC";
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
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المبيعات" + "','" + "Sales Data" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
        public void Getallsalesforcustomer()
        {
            var GD = Getalesdata();
            DataRow dr = GD.NewRow();
            dr["Invoiceno"] = 0;
            GD.Rows.InsertAt(dr, 0);
            comboBox2.DataSource = GD;
            comboBox2.ValueMember = "Invoiceno";
            comboBox2.DisplayMember = "Invoiceno";
        }
        private void iconButton9_Click(object sender, EventArgs e)
        {
            TFR Frmtrf = new TFR();
            Frmtrf.ShowDialog();
        }
        public void Counter()
        {
            int Req = Convert.ToInt32(textBox37.Text);
            int Res = Convert.ToInt32(textBox5.Text);
            int Out = Req - Res;
            textBox4.Text = Out.ToString();
            int Pros = Convert.ToInt32(textBox4.Text);
            if (!string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox37.Text))
            {
                textBox4.Text = Convert.ToString(Convert.ToInt32(textBox37.Text) - Convert.ToInt32(textBox5.Text));
            }
            else
            {
                textBox37.Text = "0";
                textBox5.Text = "0";
                textBox4.Text = "0";
            }

        }
        private void textBox37_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e,sender);
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
        }
        private void textBox35_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
        }
        private void textBox32_KeyPress(object sender, KeyPressEventArgs e)
        {
            CF.Usenumber(e, sender);
        }
        private void textBox37_TextChanged(object sender, EventArgs e)
        {
            Counter();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Counter();
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0) {
                iconButton3.Enabled = true;
            }
            else
            {
                iconButton3.Enabled = false;
                try
                {
                    var Qur = @"Select TotalAmount From sales Where Invoiceno = " + int.Parse(comboBox2.SelectedValue.ToString());
                    DA = new DataAccess();
                    var GD = DA.Getdata(Qur);
                    if (GD != null)
                    {
                        textBox2.Text = GD["TotalAmount"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "شاشة جلب المبيعات" + "','" + "Getsalesbyid" + "','" + ex.Message.ToString() + "')";
                    DA = new DataAccess();
                    var CMO = DA.Crudopration(Qurcat);
                }
            }       
        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            var Size = comboBox1.SelectedValue;
            if (Size == null || Size.ToString()  == "" || int.Parse(Size.ToString()) == 0)
            {
                MessageBox.Show("برجاء اختيار امر التفصيل","خطأ");
                return;
            }
            else
            {
                Thoberinv CRA = new Thoberinv();
                Frmreporting FCCR = new Frmreporting();
                Dataset ds = new Dataset();
                var Qurorders = @"SELECT
                             orders.Id,
                             ntailordetailes.Frontlength,
                             ntailordetailes.Backlength,
                             ntailordetailes.Shoulder,
                             ntailordetailes.Plainarmlength,
                             ntailordetailes.Cuffarmlength,
                             ntailordetailes.Plainneck,
                             ntailordetailes.Turnneck,
                             ntailordetailes.Width,
                             ntailordetailes.Handwidth,
                             ntailordetailes.Middlehand,
                             ntailordetailes.Headhand,
                             ntailordetailes.Plaincuff,
                             ntailordetailes.Cufflength,
                             ntailordetailes.Cuffwidth,
                             ntailordetailes.Collar,
                             ntailordetailes.Bottomwidth,
                             ntailordetailes.Hide,
                             ntailordetailes.Mobilehider,
                             ntailordetailes.Middlechest,
                             ntailordetailes.Middleside,
                             ntailordetailes.Weight,
                             ntailor.Invoiceid,
                             sales.TDate,
                             salesdetailes.TDDesc,
                             salesdetailes.ItemPrice,
                             salesdetailes.Quantity,
                             salesdetailes.Total,
                             sales.NonVatTotal,
                             sales.Discount,
                             sales.VatAmount,
                             sales.TotalAmount,
                             payment.Paid,
                             payment.Remaining,
                             thirdparty.Name
                             FROM orders
                             Left Outer Join ntailor
                             on orders.ntid = ntailor.Id
                             Left Outer Join ntailordetailes
                             on orders.ntoid = ntailordetailes.Id
                             Left outer Join Sales
                             on ntailor.Invoiceid = sales.Invoiceno
                             Left Outer Join salesdetailes
                             on salesdetailes.InvoiceNo = sales.Invoiceno
                             Left Outer Join payment
                             on sales.Invoiceno = payment.InvoiceNo
                             Left Outer Join thirdparty
                             on ntailor.Custid = thirdparty.ID
                             Where orders.Id = " + Size;

                DA.Con = new MySqlConnection(DA.CS);
                DA.Con.Open();
                DA.Cmd = new MySqlCommand(Qurorders, DA.Con);
                DA.dr = DA.Cmd.ExecuteReader();
                while (DA.dr.Read())
                {
                    var Id = DA.dr["Id"].ToString();
                    var Invoiceid = DA.dr["Invoiceid"].ToString();
                    var TDate = DA.dr["TDate"].ToString();
                    var TDDesc = DA.dr["TDDesc"].ToString();
                    var ItemPrice = DA.dr["ItemPrice"].ToString();
                    var Quantity = DA.dr["Quantity"].ToString();
                    var Total = DA.dr["Total"].ToString();
                    var NonVatTotal = DA.dr["NonVatTotal"].ToString();
                    var Discount = DA.dr["Discount"].ToString();
                    var VatAmount = DA.dr["VatAmount"].ToString();
                    var TotalAmount = DA.dr["TotalAmount"].ToString();
                    var Paid = DA.dr["Paid"].ToString();
                    var Remaining = DA.dr["Remaining"].ToString();
                    var Frontlength = DA.dr["Frontlength"].ToString();
                    var Backlength = DA.dr["Backlength"].ToString();
                    var Shoulder = DA.dr["Shoulder"].ToString();
                    var Plainarmlength = DA.dr["Plainarmlength"].ToString();
                    var Cuffarmlength = DA.dr["Cuffarmlength"].ToString();
                    var Plainneck = DA.dr["Plainneck"].ToString();
                    var Turnneck = DA.dr["Turnneck"].ToString();
                    var Width = DA.dr["Width"].ToString();
                    var Handwidth = DA.dr["Handwidth"].ToString();
                    var Middlehand = DA.dr["Middlehand"].ToString();
                    var Headhand = DA.dr["Headhand"].ToString();
                    var Plaincuff = DA.dr["Plaincuff"].ToString();
                    var Cufflength = DA.dr["Cufflength"].ToString();
                    var Cuffwidth = DA.dr["Cuffwidth"].ToString();
                    var Collar = DA.dr["Collar"].ToString();
                    var Bottomwidth = DA.dr["Bottomwidth"].ToString();
                    var Hide = DA.dr["Hide"].ToString();
                    var Mobilehider = DA.dr["Mobilehider"].ToString();
                    var Middlechest = DA.dr["Middlechest"].ToString();
                    var Middleside = DA.dr["Middleside"].ToString();
                    var Weight = DA.dr["Weight"].ToString();
                    var Name = DA.dr["Name"].ToString();
                    ds.Qurorders.Rows.Add(new object[] { Id,
                                                         Frontlength,Backlength,Shoulder,Plainarmlength,Cuffarmlength,
                                                         Plainneck,Turnneck,Width,Handwidth,Middlehand,
                                                         Headhand,Plaincuff,Cufflength,Cuffwidth,Collar,
                                                         Bottomwidth,Hide,Mobilehider,Middlechest,Middleside,Weight,
                                                         TDDesc, TDate,Quantity, ItemPrice, Total,
                                                         Invoiceid, NonVatTotal, Discount, VatAmount, TotalAmount, 
                                                         Logo, Paid, Remaining,Name });
                }
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
                DA.Con.Close();
            }
        }
    }
}
