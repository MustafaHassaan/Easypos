namespace Easypos.Salesforms.Cashier
{
    partial class frmPOS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOS));
            this.Panel1 = new System.Windows.Forms.Panel();
            this.Panel4 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.dgwInvoice = new System.Windows.Forms.DataGridView();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProdName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UNITID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSettlepayment = new System.Windows.Forms.Button();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.QRPic = new System.Windows.Forms.PictureBox();
            this.btnRePrint = new System.Windows.Forms.Button();
            this.btnNewTran = new System.Windows.Forms.Button();
            this.Panel9 = new System.Windows.Forms.Panel();
            this.Panel7 = new System.Windows.Forms.Panel();
            this.lblTotalAmount = new System.Windows.Forms.TextBox();
            this.Billtype = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.Datecheacked = new System.Windows.Forms.CheckBox();
            this.OldDate = new System.Windows.Forms.DateTimePicker();
            this.lblDate = new System.Windows.Forms.Label();
            this.Disc = new System.Windows.Forms.TextBox();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblTAmtBVat = new System.Windows.Forms.Label();
            this.LblVat = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.LblTTotalAmt = new System.Windows.Forms.Label();
            this.Panel12 = new System.Windows.Forms.Panel();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Btncustomers = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.clients = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtcustphone = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ctpanel = new System.Windows.Forms.Panel();
            this.addonsCates = new System.Windows.Forms.Panel();
            this.PnlJ1 = new System.Windows.Forms.Panel();
            this.lblCate = new System.Windows.Forms.Label();
            this.panelList = new System.Windows.Forms.Panel();
            this.addonsPanel = new System.Windows.Forms.Panel();
            this.Invoiceno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Reprint = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Panel1.SuspendLayout();
            this.Panel4.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwInvoice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QRPic)).BeginInit();
            this.Panel9.SuspendLayout();
            this.Panel7.SuspendLayout();
            this.Panel12.SuspendLayout();
            this.panel2.SuspendLayout();
            this.ctpanel.SuspendLayout();
            this.PnlJ1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.White;
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel1.Controls.Add(this.Panel4);
            this.Panel1.Controls.Add(this.Panel9);
            this.Panel1.Controls.Add(this.panel2);
            resources.ApplyResources(this.Panel1, "Panel1");
            this.Panel1.Name = "Panel1";
            // 
            // Panel4
            // 
            this.Panel4.BackColor = System.Drawing.Color.Gainsboro;
            this.Panel4.Controls.Add(this.panel8);
            this.Panel4.Controls.Add(this.panel5);
            resources.ApplyResources(this.Panel4, "Panel4");
            this.Panel4.Name = "Panel4";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.dgwInvoice);
            this.panel8.Controls.Add(this.DGV);
            resources.ApplyResources(this.panel8, "panel8");
            this.panel8.Name = "panel8";
            // 
            // dgwInvoice
            // 
            this.dgwInvoice.AllowUserToAddRows = false;
            this.dgwInvoice.AllowUserToDeleteRows = false;
            this.dgwInvoice.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgwInvoice.BackgroundColor = System.Drawing.Color.White;
            this.dgwInvoice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwInvoice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Invoiceno,
            this.TDate,
            this.BT,
            this.Delete,
            this.Reprint});
            this.dgwInvoice.GridColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dgwInvoice, "dgwInvoice");
            this.dgwInvoice.Name = "dgwInvoice";
            this.dgwInvoice.ReadOnly = true;
            this.dgwInvoice.RowHeadersVisible = false;
            this.dgwInvoice.RowTemplate.Height = 40;
            this.dgwInvoice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwInvoice.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwInvoice_CellContentClick);
            this.dgwInvoice.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwInvoice_CellDoubleClick);
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToResizeColumns = false;
            this.DGV.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FloralWhite;
            this.DGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV.BackgroundColor = System.Drawing.Color.White;
            this.DGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.DGV, "DGV");
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductCode,
            this.ProdName,
            this.UnitType,
            this.UNITID,
            this.Quantity,
            this.UnitPrice,
            this.Discount,
            this.LineTotal});
            this.DGV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DGV.EnableHeadersVisualStyles = false;
            this.DGV.GridColor = System.Drawing.Color.White;
            this.DGV.MultiSelect = false;
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.NavajoWhite;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.DGV.RowHeadersVisible = false;
            this.DGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.SandyBrown;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            this.DGV.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.DGV.RowTemplate.Height = 40;
            this.DGV.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // ProductCode
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ProductCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.ProductCode.FillWeight = 45F;
            resources.ApplyResources(this.ProductCode, "ProductCode");
            this.ProductCode.Name = "ProductCode";
            // 
            // ProdName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ProdName.DefaultCellStyle = dataGridViewCellStyle4;
            this.ProdName.FillWeight = 150F;
            resources.ApplyResources(this.ProdName, "ProdName");
            this.ProdName.Name = "ProdName";
            // 
            // UnitType
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.UnitType.DefaultCellStyle = dataGridViewCellStyle5;
            this.UnitType.FillWeight = 57.44501F;
            resources.ApplyResources(this.UnitType, "UnitType");
            this.UnitType.Name = "UnitType";
            // 
            // UNITID
            // 
            resources.ApplyResources(this.UNITID, "UNITID");
            this.UNITID.Name = "UNITID";
            // 
            // Quantity
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle6;
            this.Quantity.FillWeight = 57.44501F;
            resources.ApplyResources(this.Quantity, "Quantity");
            this.Quantity.Name = "Quantity";
            // 
            // UnitPrice
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.UnitPrice.DefaultCellStyle = dataGridViewCellStyle7;
            this.UnitPrice.FillWeight = 57.44501F;
            resources.ApplyResources(this.UnitPrice, "UnitPrice");
            this.UnitPrice.Name = "UnitPrice";
            // 
            // Discount
            // 
            resources.ApplyResources(this.Discount, "Discount");
            this.Discount.Name = "Discount";
            // 
            // LineTotal
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.LineTotal.DefaultCellStyle = dataGridViewCellStyle8;
            this.LineTotal.FillWeight = 57.44501F;
            resources.ApplyResources(this.LineTotal, "LineTotal");
            this.LineTotal.Name = "LineTotal";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnClose);
            this.panel5.Controls.Add(this.btnSettlepayment);
            this.panel5.Controls.Add(this.btnRemoveItem);
            this.panel5.Controls.Add(this.QRPic);
            this.panel5.Controls.Add(this.btnRePrint);
            this.panel5.Controls.Add(this.btnNewTran);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.PowderBlue;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.SystemColors.Highlight;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSettlepayment
            // 
            this.btnSettlepayment.BackColor = System.Drawing.Color.PowderBlue;
            resources.ApplyResources(this.btnSettlepayment, "btnSettlepayment");
            this.btnSettlepayment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettlepayment.FlatAppearance.BorderColor = System.Drawing.SystemColors.Highlight;
            this.btnSettlepayment.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btnSettlepayment.Name = "btnSettlepayment";
            this.btnSettlepayment.UseVisualStyleBackColor = false;
            this.btnSettlepayment.Click += new System.EventHandler(this.btnSettlepayment_Click);
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.BackColor = System.Drawing.Color.PowderBlue;
            resources.ApplyResources(this.btnRemoveItem, "btnRemoveItem");
            this.btnRemoveItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveItem.FlatAppearance.BorderColor = System.Drawing.SystemColors.Highlight;
            this.btnRemoveItem.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.UseVisualStyleBackColor = false;
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // QRPic
            // 
            resources.ApplyResources(this.QRPic, "QRPic");
            this.QRPic.Name = "QRPic";
            this.QRPic.TabStop = false;
            // 
            // btnRePrint
            // 
            this.btnRePrint.BackColor = System.Drawing.Color.PowderBlue;
            resources.ApplyResources(this.btnRePrint, "btnRePrint");
            this.btnRePrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRePrint.FlatAppearance.BorderColor = System.Drawing.SystemColors.Highlight;
            this.btnRePrint.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btnRePrint.Name = "btnRePrint";
            this.btnRePrint.UseVisualStyleBackColor = false;
            this.btnRePrint.Click += new System.EventHandler(this.btnRePrint_Click);
            // 
            // btnNewTran
            // 
            this.btnNewTran.BackColor = System.Drawing.Color.PowderBlue;
            resources.ApplyResources(this.btnNewTran, "btnNewTran");
            this.btnNewTran.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewTran.FlatAppearance.BorderColor = System.Drawing.SystemColors.Highlight;
            this.btnNewTran.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btnNewTran.Name = "btnNewTran";
            this.btnNewTran.UseVisualStyleBackColor = false;
            this.btnNewTran.Click += new System.EventHandler(this.btnNewTran_Click);
            // 
            // Panel9
            // 
            this.Panel9.BackColor = System.Drawing.Color.Gainsboro;
            this.Panel9.Controls.Add(this.Panel7);
            this.Panel9.Controls.Add(this.Panel12);
            resources.ApplyResources(this.Panel9, "Panel9");
            this.Panel9.Name = "Panel9";
            // 
            // Panel7
            // 
            this.Panel7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Panel7.Controls.Add(this.lblTotalAmount);
            this.Panel7.Controls.Add(this.Billtype);
            this.Panel7.Controls.Add(this.label18);
            this.Panel7.Controls.Add(this.Datecheacked);
            this.Panel7.Controls.Add(this.OldDate);
            this.Panel7.Controls.Add(this.lblDate);
            this.Panel7.Controls.Add(this.Disc);
            this.Panel7.Controls.Add(this.lblSubTotal);
            this.Panel7.Controls.Add(this.label3);
            this.Panel7.Controls.Add(this.LblTAmtBVat);
            this.Panel7.Controls.Add(this.LblVat);
            this.Panel7.Controls.Add(this.label56);
            this.Panel7.Controls.Add(this.LblTTotalAmt);
            resources.ApplyResources(this.Panel7, "Panel7");
            this.Panel7.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.Panel7.Name = "Panel7";
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalAmount.ForeColor = System.Drawing.Color.DarkSlateGray;
            resources.ApplyResources(this.lblTotalAmount, "lblTotalAmount");
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.TextChanged += new System.EventHandler(this.lblTotalAmount_TextChanged);
            this.lblTotalAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lblTotalAmount_KeyPress);
            // 
            // Billtype
            // 
            resources.ApplyResources(this.Billtype, "Billtype");
            this.Billtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Billtype.FormattingEnabled = true;
            this.Billtype.Items.AddRange(new object[] {
            resources.GetString("Billtype.Items"),
            resources.GetString("Billtype.Items1")});
            this.Billtype.Name = "Billtype";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // Datecheacked
            // 
            resources.ApplyResources(this.Datecheacked, "Datecheacked");
            this.Datecheacked.Name = "Datecheacked";
            this.Datecheacked.UseVisualStyleBackColor = true;
            this.Datecheacked.CheckedChanged += new System.EventHandler(this.Datecheacked_CheckedChanged);
            // 
            // OldDate
            // 
            resources.ApplyResources(this.OldDate, "OldDate");
            this.OldDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.OldDate.Name = "OldDate";
            // 
            // lblDate
            // 
            resources.ApplyResources(this.lblDate, "lblDate");
            this.lblDate.Name = "lblDate";
            // 
            // Disc
            // 
            this.Disc.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Disc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Disc.ForeColor = System.Drawing.Color.DarkSlateGray;
            resources.ApplyResources(this.Disc, "Disc");
            this.Disc.Name = "Disc";
            this.Disc.TextChanged += new System.EventHandler(this.Disc_TextChanged);
            this.Disc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Disc_KeyPress);
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblSubTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lblSubTotal, "lblSubTotal");
            this.lblSubTotal.Name = "lblSubTotal";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // LblTAmtBVat
            // 
            resources.ApplyResources(this.LblTAmtBVat, "LblTAmtBVat");
            this.LblTAmtBVat.Name = "LblTAmtBVat";
            // 
            // LblVat
            // 
            this.LblVat.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.LblVat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.LblVat, "LblVat");
            this.LblVat.Name = "LblVat";
            // 
            // label56
            // 
            resources.ApplyResources(this.label56, "label56");
            this.label56.Name = "label56";
            // 
            // LblTTotalAmt
            // 
            resources.ApplyResources(this.LblTTotalAmt, "LblTTotalAmt");
            this.LblTTotalAmt.Name = "LblTTotalAmt";
            // 
            // Panel12
            // 
            this.Panel12.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            resources.ApplyResources(this.Panel12, "Panel12");
            this.Panel12.Controls.Add(this.txtNote);
            this.Panel12.Controls.Add(this.label1);
            this.Panel12.Controls.Add(this.Btncustomers);
            this.Panel12.Controls.Add(this.textBox3);
            this.Panel12.Controls.Add(this.label4);
            this.Panel12.Controls.Add(this.label22);
            this.Panel12.Controls.Add(this.dateTimePicker2);
            this.Panel12.Controls.Add(this.clients);
            this.Panel12.Controls.Add(this.label10);
            this.Panel12.Controls.Add(this.label7);
            this.Panel12.Controls.Add(this.txtcustphone);
            this.Panel12.Name = "Panel12";
            // 
            // txtNote
            // 
            this.txtNote.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtNote, "txtNote");
            this.txtNote.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.txtNote.Name = "txtNote";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Btncustomers
            // 
            resources.ApplyResources(this.Btncustomers, "Btncustomers");
            this.Btncustomers.Name = "Btncustomers";
            this.Btncustomers.UseVisualStyleBackColor = true;
            this.Btncustomers.Click += new System.EventHandler(this.Btncustomers_Click);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.textBox3.Name = "textBox3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // dateTimePicker2
            // 
            resources.ApplyResources(this.dateTimePicker2, "dateTimePicker2");
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Name = "dateTimePicker2";
            // 
            // clients
            // 
            this.clients.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.clients.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.clients.DisplayMember = "Name";
            resources.ApplyResources(this.clients, "clients");
            this.clients.Name = "clients";
            this.clients.ValueMember = "ID";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // txtcustphone
            // 
            this.txtcustphone.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtcustphone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtcustphone, "txtcustphone");
            this.txtcustphone.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.txtcustphone.Name = "txtcustphone";
            this.txtcustphone.TextChanged += new System.EventHandler(this.txtcustphone_TextChanged);
            this.txtcustphone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtcustphone_KeyPress);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.ctpanel);
            this.panel2.Controls.Add(this.PnlJ1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // ctpanel
            // 
            this.ctpanel.Controls.Add(this.addonsCates);
            resources.ApplyResources(this.ctpanel, "ctpanel");
            this.ctpanel.Name = "ctpanel";
            // 
            // addonsCates
            // 
            resources.ApplyResources(this.addonsCates, "addonsCates");
            this.addonsCates.Name = "addonsCates";
            // 
            // PnlJ1
            // 
            this.PnlJ1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PnlJ1.Controls.Add(this.lblCate);
            this.PnlJ1.Controls.Add(this.panelList);
            this.PnlJ1.Controls.Add(this.addonsPanel);
            this.PnlJ1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            resources.ApplyResources(this.PnlJ1, "PnlJ1");
            this.PnlJ1.Name = "PnlJ1";
            // 
            // lblCate
            // 
            this.lblCate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCate, "lblCate");
            this.lblCate.Name = "lblCate";
            // 
            // panelList
            // 
            resources.ApplyResources(this.panelList, "panelList");
            this.panelList.Name = "panelList";
            // 
            // addonsPanel
            // 
            resources.ApplyResources(this.addonsPanel, "addonsPanel");
            this.addonsPanel.Name = "addonsPanel";
            // 
            // Invoiceno
            // 
            this.Invoiceno.DataPropertyName = "Invoiceno";
            resources.ApplyResources(this.Invoiceno, "Invoiceno");
            this.Invoiceno.Name = "Invoiceno";
            this.Invoiceno.ReadOnly = true;
            // 
            // TDate
            // 
            this.TDate.DataPropertyName = "TDate";
            resources.ApplyResources(this.TDate, "TDate");
            this.TDate.Name = "TDate";
            this.TDate.ReadOnly = true;
            // 
            // BT
            // 
            this.BT.DataPropertyName = "Billtype";
            resources.ApplyResources(this.BT, "BT");
            this.BT.Name = "BT";
            this.BT.ReadOnly = true;
            // 
            // Delete
            // 
            resources.ApplyResources(this.Delete, "Delete");
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Delete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Delete.Text = "حذف";
            this.Delete.UseColumnTextForButtonValue = true;
            // 
            // Reprint
            // 
            resources.ApplyResources(this.Reprint, "Reprint");
            this.Reprint.Name = "Reprint";
            this.Reprint.ReadOnly = true;
            this.Reprint.Text = "طباعه";
            this.Reprint.UseColumnTextForButtonValue = true;
            // 
            // frmPOS
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.Controls.Add(this.Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmPOS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Panel1.ResumeLayout(false);
            this.Panel4.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwInvoice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.QRPic)).EndInit();
            this.Panel9.ResumeLayout(false);
            this.Panel7.ResumeLayout(false);
            this.Panel7.PerformLayout();
            this.Panel12.ResumeLayout(false);
            this.Panel12.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ctpanel.ResumeLayout(false);
            this.PnlJ1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Panel Panel9;
        internal System.Windows.Forms.Panel Panel12;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel PnlJ1;
        private System.Windows.Forms.Label lblCate;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.Panel addonsPanel;
        private System.Windows.Forms.Panel addonsCates;
        internal System.Windows.Forms.Panel Panel4;
        internal System.Windows.Forms.DataGridView dgwInvoice;
        private System.Windows.Forms.Button btnRePrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnNewTran;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Button btnSettlepayment;
        internal System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox QRPic;
        private System.Windows.Forms.TextBox txtcustphone;
        internal System.Windows.Forms.Label label7;
        public System.Windows.Forms.ComboBox clients;
        internal System.Windows.Forms.Panel Panel7;
        public System.Windows.Forms.TextBox Disc;
        public System.Windows.Forms.Label lblSubTotal;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label LblTAmtBVat;
        public System.Windows.Forms.Label LblVat;
        internal System.Windows.Forms.Label label56;
        internal System.Windows.Forms.Label LblTTotalAmt;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.Panel ctpanel;
        private System.Windows.Forms.ComboBox Billtype;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox Datecheacked;
        public System.Windows.Forms.DateTimePicker OldDate;
        private System.Windows.Forms.Label lblDate;
        public System.Windows.Forms.TextBox lblTotalAmount;
        private System.Windows.Forms.Label label22;
        public System.Windows.Forms.DateTimePicker dateTimePicker2;
        public System.Windows.Forms.TextBox textBox3;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Btncustomers;
        public System.Windows.Forms.TextBox txtNote;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProdName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitType;
        private System.Windows.Forms.DataGridViewTextBoxColumn UNITID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Invoiceno;
        private System.Windows.Forms.DataGridViewTextBoxColumn TDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BT;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.DataGridViewButtonColumn Reprint;
    }
}