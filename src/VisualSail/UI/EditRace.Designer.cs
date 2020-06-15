namespace AmphibianSoftware.VisualSail.UI
{
    partial class EditRace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditRace));
            this.startDP = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.endDP = new System.Windows.Forms.DateTimePicker();
            this.nameLBL = new System.Windows.Forms.Label();
            this.raceNameTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.courseCB = new System.Windows.Forms.ComboBox();
            this.editTC = new System.Windows.Forms.TabControl();
            this.boatsTP = new System.Windows.Forms.TabPage();
            this.boatsLV = new System.Windows.Forms.ListView();
            this.boatNameCH = new System.Windows.Forms.ColumnHeader();
            this.boatNumberCH = new System.Windows.Forms.ColumnHeader();
            this.boatColorCH = new System.Windows.Forms.ColumnHeader();
            this.gpsStartCH = new System.Windows.Forms.ColumnHeader();
            this.gpsEndCH = new System.Windows.Forms.ColumnHeader();
            this.panel2 = new System.Windows.Forms.Panel();
            this.newBoatBTN = new System.Windows.Forms.Button();
            this.gpsDataBTN = new System.Windows.Forms.Button();
            this.boatEditBTN = new System.Windows.Forms.Button();
            this.raceTP = new System.Windows.Forms.TabPage();
            this.startSequenceDTP = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.timezoneCB = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.gpsDataLBL = new System.Windows.Forms.Label();
            this.lakeTP = new System.Windows.Forms.TabPage();
            this.editGB = new System.Windows.Forms.GroupBox();
            this.lakeResizer = new AmphibianSoftware.VisualSail.UI.Controls.LakeResizeControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lakeAltNUD = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lakeNameTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.existingBTN = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.editCourseBTN = new System.Windows.Forms.Button();
            this.newCourseBTN = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.playBTN = new System.Windows.Forms.Button();
            this.okBTN = new System.Windows.Forms.Button();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.editTC.SuspendLayout();
            this.boatsTP.SuspendLayout();
            this.panel2.SuspendLayout();
            this.raceTP.SuspendLayout();
            this.lakeTP.SuspendLayout();
            this.editGB.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lakeAltNUD)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startDP
            // 
            this.startDP.CustomFormat = "M/d/yyyy h:mm:ss tt";
            this.startDP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDP.Location = new System.Drawing.Point(100, 82);
            this.startDP.Name = "startDP";
            this.startDP.Size = new System.Drawing.Size(188, 20);
            this.startDP.TabIndex = 8;
            this.startDP.ValueChanged += new System.EventHandler(this.startDP_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Start";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(65, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "End";
            // 
            // endDP
            // 
            this.endDP.CustomFormat = "M/d/yyyy h:mm:ss tt";
            this.endDP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDP.Location = new System.Drawing.Point(100, 109);
            this.endDP.Name = "endDP";
            this.endDP.Size = new System.Drawing.Size(188, 20);
            this.endDP.TabIndex = 11;
            this.endDP.ValueChanged += new System.EventHandler(this.endDP_ValueChanged);
            // 
            // nameLBL
            // 
            this.nameLBL.AutoSize = true;
            this.nameLBL.Location = new System.Drawing.Point(27, 9);
            this.nameLBL.Name = "nameLBL";
            this.nameLBL.Size = new System.Drawing.Size(64, 13);
            this.nameLBL.TabIndex = 14;
            this.nameLBL.Text = "Race Name";
            // 
            // raceNameTB
            // 
            this.raceNameTB.Location = new System.Drawing.Point(100, 6);
            this.raceNameTB.Name = "raceNameTB";
            this.raceNameTB.Size = new System.Drawing.Size(188, 20);
            this.raceNameTB.TabIndex = 15;
            this.raceNameTB.TextChanged += new System.EventHandler(this.raceNameTB_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(3, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Course:";
            // 
            // courseCB
            // 
            this.courseCB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.courseCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.courseCB.FormattingEnabled = true;
            this.courseCB.Location = new System.Drawing.Point(46, 16);
            this.courseCB.Name = "courseCB";
            this.courseCB.Size = new System.Drawing.Size(369, 21);
            this.courseCB.TabIndex = 23;
            this.courseCB.SelectedIndexChanged += new System.EventHandler(this.courseCB_SelectedIndexChanged);
            // 
            // editTC
            // 
            this.editTC.Controls.Add(this.boatsTP);
            this.editTC.Controls.Add(this.raceTP);
            this.editTC.Controls.Add(this.lakeTP);
            this.editTC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editTC.Location = new System.Drawing.Point(0, 0);
            this.editTC.Name = "editTC";
            this.editTC.SelectedIndex = 0;
            this.editTC.Size = new System.Drawing.Size(632, 449);
            this.editTC.TabIndex = 31;
            this.editTC.SelectedIndexChanged += new System.EventHandler(this.editTC_SelectedIndexChanged);
            // 
            // boatsTP
            // 
            this.boatsTP.Controls.Add(this.boatsLV);
            this.boatsTP.Controls.Add(this.panel2);
            this.boatsTP.Location = new System.Drawing.Point(4, 22);
            this.boatsTP.Name = "boatsTP";
            this.boatsTP.Size = new System.Drawing.Size(624, 423);
            this.boatsTP.TabIndex = 3;
            this.boatsTP.Text = "Boats";
            this.boatsTP.UseVisualStyleBackColor = true;
            // 
            // boatsLV
            // 
            this.boatsLV.CheckBoxes = true;
            this.boatsLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.boatNameCH,
            this.boatNumberCH,
            this.boatColorCH,
            this.gpsStartCH,
            this.gpsEndCH});
            this.boatsLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boatsLV.FullRowSelect = true;
            this.boatsLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.boatsLV.HideSelection = false;
            this.boatsLV.Location = new System.Drawing.Point(0, 0);
            this.boatsLV.MultiSelect = false;
            this.boatsLV.Name = "boatsLV";
            this.boatsLV.Size = new System.Drawing.Size(624, 401);
            this.boatsLV.TabIndex = 31;
            this.boatsLV.UseCompatibleStateImageBehavior = false;
            this.boatsLV.View = System.Windows.Forms.View.Details;
            this.boatsLV.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.boatsLV_ItemChecked);
            this.boatsLV.SelectedIndexChanged += new System.EventHandler(this.boatsLV_SelectedIndexChanged);
            // 
            // boatNameCH
            // 
            this.boatNameCH.Text = "Name";
            this.boatNameCH.Width = 130;
            // 
            // boatNumberCH
            // 
            this.boatNumberCH.Text = "Number";
            this.boatNumberCH.Width = 70;
            // 
            // boatColorCH
            // 
            this.boatColorCH.Text = "Color";
            // 
            // gpsStartCH
            // 
            this.gpsStartCH.Text = "Gps Start";
            this.gpsStartCH.Width = 80;
            // 
            // gpsEndCH
            // 
            this.gpsEndCH.Text = "Gps End";
            this.gpsEndCH.Width = 79;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.newBoatBTN);
            this.panel2.Controls.Add(this.gpsDataBTN);
            this.panel2.Controls.Add(this.boatEditBTN);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 401);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(624, 22);
            this.panel2.TabIndex = 32;
            // 
            // newBoatBTN
            // 
            this.newBoatBTN.Dock = System.Windows.Forms.DockStyle.Left;
			this.newBoatBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("add.png");
            this.newBoatBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newBoatBTN.Location = new System.Drawing.Point(0, 0);
            this.newBoatBTN.Name = "newBoatBTN";
            this.newBoatBTN.Size = new System.Drawing.Size(100, 22);
            this.newBoatBTN.TabIndex = 31;
            this.newBoatBTN.Text = "New Boat";
            this.newBoatBTN.UseVisualStyleBackColor = true;
            this.newBoatBTN.Click += new System.EventHandler(this.newBoatBTN_Click_1);
            // 
            // gpsDataBTN
            // 
            this.gpsDataBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.gpsDataBTN.Enabled = false;
			this.gpsDataBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("chart_line.png");
            this.gpsDataBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.gpsDataBTN.Location = new System.Drawing.Point(424, 0);
            this.gpsDataBTN.Name = "gpsDataBTN";
            this.gpsDataBTN.Size = new System.Drawing.Size(100, 22);
            this.gpsDataBTN.TabIndex = 30;
            this.gpsDataBTN.Text = "Gps Data";
            this.gpsDataBTN.UseVisualStyleBackColor = true;
            this.gpsDataBTN.Click += new System.EventHandler(this.gpsDataBTN_Click);
            // 
            // boatEditBTN
            // 
            this.boatEditBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.boatEditBTN.Enabled = false;
			this.boatEditBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("pencil.png");
            this.boatEditBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.boatEditBTN.Location = new System.Drawing.Point(524, 0);
            this.boatEditBTN.Name = "boatEditBTN";
            this.boatEditBTN.Size = new System.Drawing.Size(100, 22);
            this.boatEditBTN.TabIndex = 29;
            this.boatEditBTN.Text = "Edit Boat";
            this.boatEditBTN.UseVisualStyleBackColor = true;
            this.boatEditBTN.Click += new System.EventHandler(this.boatEditBTN_Click);
            // 
            // raceTP
            // 
            this.raceTP.Controls.Add(this.startSequenceDTP);
            this.raceTP.Controls.Add(this.label1);
            this.raceTP.Controls.Add(this.timezoneCB);
            this.raceTP.Controls.Add(this.label7);
            this.raceTP.Controls.Add(this.gpsDataLBL);
            this.raceTP.Controls.Add(this.raceNameTB);
            this.raceTP.Controls.Add(this.nameLBL);
            this.raceTP.Controls.Add(this.startDP);
            this.raceTP.Controls.Add(this.label4);
            this.raceTP.Controls.Add(this.label5);
            this.raceTP.Controls.Add(this.endDP);
            this.raceTP.Location = new System.Drawing.Point(4, 22);
            this.raceTP.Name = "raceTP";
            this.raceTP.Padding = new System.Windows.Forms.Padding(3);
            this.raceTP.Size = new System.Drawing.Size(624, 423);
            this.raceTP.TabIndex = 0;
            this.raceTP.Text = "Race";
            this.raceTP.UseVisualStyleBackColor = true;
            // 
            // startSequenceDTP
            // 
            this.startSequenceDTP.CustomFormat = "mm:ss";
            this.startSequenceDTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startSequenceDTP.Location = new System.Drawing.Point(100, 56);
            this.startSequenceDTP.Name = "startSequenceDTP";
            this.startSequenceDTP.ShowUpDown = true;
            this.startSequenceDTP.Size = new System.Drawing.Size(188, 20);
            this.startSequenceDTP.TabIndex = 33;
            this.startSequenceDTP.Value = new System.DateTime(2009, 3, 3, 12, 0, 0, 0);
            this.startSequenceDTP.ValueChanged += new System.EventHandler(this.startSequenceDTP_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Start Sequence";
            // 
            // timezoneCB
            // 
            this.timezoneCB.DropDownHeight = 200;
            this.timezoneCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timezoneCB.DropDownWidth = 300;
            this.timezoneCB.FormattingEnabled = true;
            this.timezoneCB.IntegralHeight = false;
            this.timezoneCB.Location = new System.Drawing.Point(100, 29);
            this.timezoneCB.Name = "timezoneCB";
            this.timezoneCB.Size = new System.Drawing.Size(188, 21);
            this.timezoneCB.TabIndex = 31;
            this.timezoneCB.SelectedIndexChanged += new System.EventHandler(this.timezoneCB_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Time Zone";
            // 
            // gpsDataLBL
            // 
            this.gpsDataLBL.AutoSize = true;
            this.gpsDataLBL.ForeColor = System.Drawing.Color.Maroon;
            this.gpsDataLBL.Location = new System.Drawing.Point(294, 88);
            this.gpsDataLBL.Name = "gpsDataLBL";
            this.gpsDataLBL.Size = new System.Drawing.Size(312, 13);
            this.gpsDataLBL.TabIndex = 16;
            this.gpsDataLBL.Text = "The start time you have specified occurs before the first gps data";
            // 
            // lakeTP
            // 
            this.lakeTP.Controls.Add(this.editGB);
            this.lakeTP.Controls.Add(this.groupBox1);
            this.lakeTP.Location = new System.Drawing.Point(4, 22);
            this.lakeTP.Name = "lakeTP";
            this.lakeTP.Padding = new System.Windows.Forms.Padding(3);
            this.lakeTP.Size = new System.Drawing.Size(624, 423);
            this.lakeTP.TabIndex = 1;
            this.lakeTP.Text = "Area & Course";
            this.lakeTP.UseVisualStyleBackColor = true;
            // 
            // editGB
            // 
            this.editGB.Controls.Add(this.lakeResizer);
            this.editGB.Controls.Add(this.panel3);
            this.editGB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editGB.Location = new System.Drawing.Point(3, 3);
            this.editGB.Name = "editGB";
            this.editGB.Size = new System.Drawing.Size(618, 376);
            this.editGB.TabIndex = 30;
            this.editGB.TabStop = false;
            this.editGB.Text = "Area";
            // 
            // lakeResizer
            // 
            this.lakeResizer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lakeResizer.Lake = null;
            this.lakeResizer.Location = new System.Drawing.Point(3, 39);
            this.lakeResizer.Name = "lakeResizer";
            this.lakeResizer.Size = new System.Drawing.Size(612, 334);
            this.lakeResizer.TabIndex = 31;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lakeAltNUD);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.lakeNameTB);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.existingBTN);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(612, 23);
            this.panel3.TabIndex = 30;
            // 
            // lakeAltNUD
            // 
            this.lakeAltNUD.DecimalPlaces = 2;
            this.lakeAltNUD.Dock = System.Windows.Forms.DockStyle.Left;
            this.lakeAltNUD.Location = new System.Drawing.Point(256, 0);
            this.lakeAltNUD.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.lakeAltNUD.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.lakeAltNUD.Name = "lakeAltNUD";
            this.lakeAltNUD.Size = new System.Drawing.Size(53, 20);
            this.lakeAltNUD.TabIndex = 29;
            this.lakeAltNUD.Visible = false;
            this.lakeAltNUD.ValueChanged += new System.EventHandler(this.lakeAltNUD_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(177, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Altitude(meters)";
            this.label3.Visible = false;
            // 
            // lakeNameTB
            // 
            this.lakeNameTB.Dock = System.Windows.Forms.DockStyle.Left;
            this.lakeNameTB.Location = new System.Drawing.Point(35, 0);
            this.lakeNameTB.Name = "lakeNameTB";
            this.lakeNameTB.Size = new System.Drawing.Size(142, 20);
            this.lakeNameTB.TabIndex = 29;
            this.lakeNameTB.TextChanged += new System.EventHandler(this.lakeNameTB_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Name";
            // 
            // existingBTN
            // 
            this.existingBTN.Dock = System.Windows.Forms.DockStyle.Right;
			this.existingBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("map.png");
            this.existingBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.existingBTN.Location = new System.Drawing.Point(512, 0);
            this.existingBTN.Name = "existingBTN";
            this.existingBTN.Size = new System.Drawing.Size(100, 23);
            this.existingBTN.TabIndex = 30;
            this.existingBTN.Text = "Browse...";
            this.existingBTN.UseVisualStyleBackColor = true;
            this.existingBTN.Click += new System.EventHandler(this.existingBTN_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.courseCB);
            this.groupBox1.Controls.Add(this.editCourseBTN);
            this.groupBox1.Controls.Add(this.newCourseBTN);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(3, 379);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(618, 41);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Course";
            // 
            // editCourseBTN
            // 
            this.editCourseBTN.Dock = System.Windows.Forms.DockStyle.Right;
			this.editCourseBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("map_edit.png");
            this.editCourseBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editCourseBTN.Location = new System.Drawing.Point(415, 16);
            this.editCourseBTN.Name = "editCourseBTN";
            this.editCourseBTN.Size = new System.Drawing.Size(100, 22);
            this.editCourseBTN.TabIndex = 0;
            this.editCourseBTN.Text = "Edit Course";
            this.editCourseBTN.UseVisualStyleBackColor = true;
            this.editCourseBTN.Click += new System.EventHandler(this.editCourseBTN_Click_2);
            // 
            // newCourseBTN
            // 
            this.newCourseBTN.Dock = System.Windows.Forms.DockStyle.Right;
			this.newCourseBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("map_add.png");
            this.newCourseBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newCourseBTN.Location = new System.Drawing.Point(515, 16);
            this.newCourseBTN.Name = "newCourseBTN";
            this.newCourseBTN.Size = new System.Drawing.Size(100, 22);
            this.newCourseBTN.TabIndex = 1;
            this.newCourseBTN.Text = "New Course";
            this.newCourseBTN.UseVisualStyleBackColor = true;
            this.newCourseBTN.Click += new System.EventHandler(this.newCourseBTN_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.playBTN);
            this.panel1.Controls.Add(this.okBTN);
            this.panel1.Controls.Add(this.cancelBTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 449);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 23);
            this.panel1.TabIndex = 32;
            // 
            // playBTN
            // 
            this.playBTN.Dock = System.Windows.Forms.DockStyle.Right;
			this.playBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("resultset_next.png");
            this.playBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.playBTN.Location = new System.Drawing.Point(332, 0);
            this.playBTN.Name = "playBTN";
            this.playBTN.Size = new System.Drawing.Size(100, 23);
            this.playBTN.TabIndex = 15;
            this.playBTN.Text = "Play";
            this.playBTN.UseVisualStyleBackColor = true;
            this.playBTN.Click += new System.EventHandler(this.okBTN_Click);
            // 
            // okBTN
            // 
            this.okBTN.Dock = System.Windows.Forms.DockStyle.Right;
			this.okBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("accept.png");
            this.okBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okBTN.Location = new System.Drawing.Point(432, 0);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(100, 23);
            this.okBTN.TabIndex = 13;
            this.okBTN.Text = "Ok";
            this.okBTN.UseVisualStyleBackColor = true;
            this.okBTN.Click += new System.EventHandler(this.okBTN_Click);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Dock = System.Windows.Forms.DockStyle.Right;
			this.cancelBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("cancel.png");
            this.cancelBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelBTN.Location = new System.Drawing.Point(532, 0);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(100, 23);
            this.cancelBTN.TabIndex = 14;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // EditRace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 472);
            this.ControlBox = false;
            this.Controls.Add(this.editTC);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "EditRace";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Race";
            this.Load += new System.EventHandler(this.EditRace_Load);
            this.editTC.ResumeLayout(false);
            this.boatsTP.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.raceTP.ResumeLayout(false);
            this.raceTP.PerformLayout();
            this.lakeTP.ResumeLayout(false);
            this.editGB.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lakeAltNUD)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker startDP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker endDP;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.Label nameLBL;
        private System.Windows.Forms.TextBox raceNameTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox courseCB;
        private System.Windows.Forms.TabControl editTC;
        private System.Windows.Forms.TabPage boatsTP;
        private System.Windows.Forms.TabPage raceTP;
        private System.Windows.Forms.TabPage lakeTP;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.ListView boatsLV;
        private System.Windows.Forms.ColumnHeader boatNameCH;
        private System.Windows.Forms.ColumnHeader boatNumberCH;
        private System.Windows.Forms.ColumnHeader gpsStartCH;
        private System.Windows.Forms.ColumnHeader gpsEndCH;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ColumnHeader boatColorCH;
        private System.Windows.Forms.Button boatEditBTN;
        private System.Windows.Forms.Button gpsDataBTN;
        private System.Windows.Forms.Button newBoatBTN;
        private System.Windows.Forms.NumericUpDown lakeAltNUD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox lakeNameTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox editGB;
        private System.Windows.Forms.Panel panel3;
        private AmphibianSoftware.VisualSail.UI.Controls.LakeResizeControl lakeResizer;
        private System.Windows.Forms.Button existingBTN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button editCourseBTN;
        private System.Windows.Forms.Button newCourseBTN;
        private System.Windows.Forms.Label gpsDataLBL;
        private System.Windows.Forms.Button playBTN;
        private System.Windows.Forms.ComboBox timezoneCB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker startSequenceDTP;
        private System.Windows.Forms.Label label1;
    }
}