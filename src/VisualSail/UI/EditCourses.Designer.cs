namespace AmphibianSoftware.VisualSail.UI
{
    partial class EditCourses
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCourses));
            this.editCourseGB = new System.Windows.Forms.GroupBox();
            this.dateDP = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.nameTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.routeLB = new System.Windows.Forms.ListBox();
            this.marksLB = new System.Windows.Forms.ListBox();
            this.lakePNL = new AmphibianSoftware.VisualSail.UI.Controls.UserDrawnPanel();
            this.markTypeCB = new System.Windows.Forms.ComboBox();
            this.markNameTB = new System.Windows.Forms.TextBox();
            this.mouseCoordsLBL = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.marksTC = new System.Windows.Forms.TabControl();
            this.marksTP = new System.Windows.Forms.TabPage();
            this.editMarkGB = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.routeTP = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.routeMarkLB = new System.Windows.Forms.ListBox();
            this.windTP = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.windToMarkCB = new System.Windows.Forms.ComboBox();
            this.windFromMarkCB = new System.Windows.Forms.ComboBox();
            this.windSensorRB = new System.Windows.Forms.RadioButton();
            this.windManualRB = new System.Windows.Forms.RadioButton();
            this.windCourseRB = new System.Windows.Forms.RadioButton();
            this.manualDS = new AmphibianSoftware.VisualSail.UI.Controls.DirectionSelector();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.images = new System.Windows.Forms.ImageList(this.components);
            this.zoomInCB = new System.Windows.Forms.CheckBox();
            this.zoomOutBTN = new System.Windows.Forms.Button();
            this.showBoatsCB = new System.Windows.Forms.CheckBox();
            this.addBTN = new System.Windows.Forms.Button();
            this.removeBTN = new System.Windows.Forms.Button();
            this.removeOrderBTN = new System.Windows.Forms.Button();
            this.addOrderBTN = new System.Windows.Forms.Button();
            this.okBTN = new System.Windows.Forms.Button();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.editCourseGB.SuspendLayout();
            this.panel1.SuspendLayout();
            this.marksTC.SuspendLayout();
            this.marksTP.SuspendLayout();
            this.editMarkGB.SuspendLayout();
            this.routeTP.SuspendLayout();
            this.windTP.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // editCourseGB
            // 
            this.editCourseGB.Controls.Add(this.dateDP);
            this.editCourseGB.Controls.Add(this.label3);
            this.editCourseGB.Controls.Add(this.nameTB);
            this.editCourseGB.Controls.Add(this.label2);
            this.editCourseGB.Dock = System.Windows.Forms.DockStyle.Top;
            this.editCourseGB.Location = new System.Drawing.Point(0, 0);
            this.editCourseGB.Name = "editCourseGB";
            this.editCourseGB.Size = new System.Drawing.Size(632, 48);
            this.editCourseGB.TabIndex = 1;
            this.editCourseGB.TabStop = false;
            this.editCourseGB.Text = "Edit Course";
            // 
            // dateDP
            // 
            this.dateDP.CustomFormat = "M/d/yyyy";
            this.dateDP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateDP.Location = new System.Drawing.Point(405, 13);
            this.dateDP.Name = "dateDP";
            this.dateDP.Size = new System.Drawing.Size(171, 20);
            this.dateDP.TabIndex = 3;
            this.dateDP.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(369, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date";
            this.label3.Visible = false;
            // 
            // nameTB
            // 
            this.nameTB.Location = new System.Drawing.Point(47, 14);
            this.nameTB.Name = "nameTB";
            this.nameTB.Size = new System.Drawing.Size(316, 20);
            this.nameTB.TabIndex = 1;
            this.nameTB.TextChanged += new System.EventHandler(this.nameTB_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Name";
            // 
            // routeLB
            // 
            this.routeLB.FormattingEnabled = true;
            this.routeLB.Location = new System.Drawing.Point(149, 19);
            this.routeLB.Name = "routeLB";
            this.routeLB.Size = new System.Drawing.Size(100, 329);
            this.routeLB.TabIndex = 14;
            this.routeLB.SelectedIndexChanged += new System.EventHandler(this.routeLB_SelectedIndexChanged);
            // 
            // marksLB
            // 
            this.marksLB.FormattingEnabled = true;
            this.marksLB.Location = new System.Drawing.Point(5, 19);
            this.marksLB.Name = "marksLB";
            this.marksLB.Size = new System.Drawing.Size(206, 186);
            this.marksLB.TabIndex = 4;
            this.marksLB.SelectedIndexChanged += new System.EventHandler(this.marksLB_SelectedIndexChanged);
            // 
            // lakePNL
            // 
            this.lakePNL.BackColor = System.Drawing.SystemColors.Control;
            this.lakePNL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lakePNL.Location = new System.Drawing.Point(0, 0);
            this.lakePNL.Name = "lakePNL";
            this.lakePNL.Size = new System.Drawing.Size(370, 354);
            this.lakePNL.TabIndex = 10;
            this.lakePNL.Paint += new System.Windows.Forms.PaintEventHandler(this.lakePNL_Paint);
            this.lakePNL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lakePNL_MouseMove);
            this.lakePNL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lakePNL_MouseDown);
            this.lakePNL.Resize += new System.EventHandler(this.lakePNL_Resize);
            this.lakePNL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lakePNL_MouseUp);
            // 
            // markTypeCB
            // 
            this.markTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.markTypeCB.FormattingEnabled = true;
            this.markTypeCB.Items.AddRange(new object[] {
            "Mark",
            "Gate"});
            this.markTypeCB.Location = new System.Drawing.Point(47, 36);
            this.markTypeCB.Name = "markTypeCB";
            this.markTypeCB.Size = new System.Drawing.Size(150, 21);
            this.markTypeCB.TabIndex = 11;
            this.markTypeCB.SelectedIndexChanged += new System.EventHandler(this.markTypeCB_SelectedIndexChanged);
            // 
            // markNameTB
            // 
            this.markNameTB.Location = new System.Drawing.Point(47, 13);
            this.markNameTB.Name = "markNameTB";
            this.markNameTB.Size = new System.Drawing.Size(150, 20);
            this.markNameTB.TabIndex = 12;
            this.markNameTB.TextChanged += new System.EventHandler(this.markNameTB_TextChanged);
            // 
            // mouseCoordsLBL
            // 
            this.mouseCoordsLBL.AutoSize = true;
            this.mouseCoordsLBL.Location = new System.Drawing.Point(56, 60);
            this.mouseCoordsLBL.Name = "mouseCoordsLBL";
            this.mouseCoordsLBL.Size = new System.Drawing.Size(14, 13);
            this.mouseCoordsLBL.TabIndex = 13;
            this.mouseCoordsLBL.Text = "#";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.okBTN);
            this.panel1.Controls.Add(this.cancelBTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 424);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 22);
            this.panel1.TabIndex = 8;
            // 
            // marksTC
            // 
            this.marksTC.Controls.Add(this.marksTP);
            this.marksTC.Controls.Add(this.routeTP);
            this.marksTC.Controls.Add(this.windTP);
            this.marksTC.Dock = System.Windows.Forms.DockStyle.Left;
            this.marksTC.Location = new System.Drawing.Point(0, 48);
            this.marksTC.Name = "marksTC";
            this.marksTC.SelectedIndex = 0;
            this.marksTC.Size = new System.Drawing.Size(262, 376);
            this.marksTC.TabIndex = 23;
            // 
            // marksTP
            // 
            this.marksTP.Controls.Add(this.editMarkGB);
            this.marksTP.Controls.Add(this.label1);
            this.marksTP.Controls.Add(this.marksLB);
            this.marksTP.Controls.Add(this.addBTN);
            this.marksTP.Controls.Add(this.removeBTN);
            this.marksTP.Location = new System.Drawing.Point(4, 22);
            this.marksTP.Name = "marksTP";
            this.marksTP.Padding = new System.Windows.Forms.Padding(3);
            this.marksTP.Size = new System.Drawing.Size(254, 350);
            this.marksTP.TabIndex = 0;
            this.marksTP.Text = "Marks";
            this.marksTP.UseVisualStyleBackColor = true;
            // 
            // editMarkGB
            // 
            this.editMarkGB.Controls.Add(this.label4);
            this.editMarkGB.Controls.Add(this.label6);
            this.editMarkGB.Controls.Add(this.mouseCoordsLBL);
            this.editMarkGB.Controls.Add(this.markTypeCB);
            this.editMarkGB.Controls.Add(this.markNameTB);
            this.editMarkGB.Controls.Add(this.label5);
            this.editMarkGB.Enabled = false;
            this.editMarkGB.Location = new System.Drawing.Point(8, 240);
            this.editMarkGB.Name = "editMarkGB";
            this.editMarkGB.Size = new System.Drawing.Size(203, 102);
            this.editMarkGB.TabIndex = 21;
            this.editMarkGB.TabStop = false;
            this.editMarkGB.Text = "Edit Mark";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Position";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Select Mark";
            // 
            // routeTP
            // 
            this.routeTP.Controls.Add(this.label8);
            this.routeTP.Controls.Add(this.label7);
            this.routeTP.Controls.Add(this.routeMarkLB);
            this.routeTP.Controls.Add(this.removeOrderBTN);
            this.routeTP.Controls.Add(this.addOrderBTN);
            this.routeTP.Controls.Add(this.routeLB);
            this.routeTP.Location = new System.Drawing.Point(4, 22);
            this.routeTP.Name = "routeTP";
            this.routeTP.Padding = new System.Windows.Forms.Padding(3);
            this.routeTP.Size = new System.Drawing.Size(254, 350);
            this.routeTP.TabIndex = 1;
            this.routeTP.Text = "Route";
            this.routeTP.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(163, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Route";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Select Mark";
            // 
            // routeMarkLB
            // 
            this.routeMarkLB.FormattingEnabled = true;
            this.routeMarkLB.Location = new System.Drawing.Point(5, 19);
            this.routeMarkLB.Name = "routeMarkLB";
            this.routeMarkLB.Size = new System.Drawing.Size(100, 329);
            this.routeMarkLB.TabIndex = 5;
            this.routeMarkLB.SelectedIndexChanged += new System.EventHandler(this.routeMarkLB_SelectedIndexChanged);
            // 
            // windTP
            // 
            this.windTP.Controls.Add(this.label10);
            this.windTP.Controls.Add(this.label9);
            this.windTP.Controls.Add(this.windToMarkCB);
            this.windTP.Controls.Add(this.windFromMarkCB);
            this.windTP.Controls.Add(this.windSensorRB);
            this.windTP.Controls.Add(this.windManualRB);
            this.windTP.Controls.Add(this.windCourseRB);
            this.windTP.Controls.Add(this.manualDS);
            this.windTP.Location = new System.Drawing.Point(4, 22);
            this.windTP.Name = "windTP";
            this.windTP.Size = new System.Drawing.Size(254, 350);
            this.windTP.TabIndex = 2;
            this.windTP.Text = "Wind";
            this.windTP.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 181);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "To";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 153);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "From";
            // 
            // windToMarkCB
            // 
            this.windToMarkCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.windToMarkCB.Enabled = false;
            this.windToMarkCB.FormattingEnabled = true;
            this.windToMarkCB.Location = new System.Drawing.Point(62, 178);
            this.windToMarkCB.Name = "windToMarkCB";
            this.windToMarkCB.Size = new System.Drawing.Size(121, 21);
            this.windToMarkCB.TabIndex = 5;
            this.windToMarkCB.SelectedIndexChanged += new System.EventHandler(this.windToMarkCB_SelectedIndexChanged);
            // 
            // windFromMarkCB
            // 
            this.windFromMarkCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.windFromMarkCB.Enabled = false;
            this.windFromMarkCB.FormattingEnabled = true;
            this.windFromMarkCB.Location = new System.Drawing.Point(62, 150);
            this.windFromMarkCB.Name = "windFromMarkCB";
            this.windFromMarkCB.Size = new System.Drawing.Size(121, 21);
            this.windFromMarkCB.TabIndex = 4;
            this.windFromMarkCB.SelectedIndexChanged += new System.EventHandler(this.windFromMarkCB_SelectedIndexChanged);
            // 
            // windSensorRB
            // 
            this.windSensorRB.AutoSize = true;
            this.windSensorRB.Enabled = false;
            this.windSensorRB.Location = new System.Drawing.Point(8, 205);
            this.windSensorRB.Name = "windSensorRB";
            this.windSensorRB.Size = new System.Drawing.Size(157, 17);
            this.windSensorRB.TabIndex = 2;
            this.windSensorRB.Text = "Dynamic, From Sensor Data";
            this.windSensorRB.UseVisualStyleBackColor = true;
            // 
            // windManualRB
            // 
            this.windManualRB.AutoSize = true;
            this.windManualRB.Checked = true;
            this.windManualRB.Location = new System.Drawing.Point(5, 9);
            this.windManualRB.Name = "windManualRB";
            this.windManualRB.Size = new System.Drawing.Size(108, 17);
            this.windManualRB.TabIndex = 1;
            this.windManualRB.TabStop = true;
            this.windManualRB.Text = "Constant, Manual";
            this.windManualRB.UseVisualStyleBackColor = true;
            this.windManualRB.CheckedChanged += new System.EventHandler(this.windManualRB_CheckedChanged);
            // 
            // windCourseRB
            // 
            this.windCourseRB.AutoSize = true;
            this.windCourseRB.Location = new System.Drawing.Point(8, 127);
            this.windCourseRB.Name = "windCourseRB";
            this.windCourseRB.Size = new System.Drawing.Size(121, 17);
            this.windCourseRB.TabIndex = 0;
            this.windCourseRB.Text = "Constant, By Course";
            this.windCourseRB.UseVisualStyleBackColor = true;
            this.windCourseRB.CheckedChanged += new System.EventHandler(this.windCourseRB_CheckedChanged);
            // 
            // manualDS
            // 
            this.manualDS.Location = new System.Drawing.Point(27, 32);
            this.manualDS.Name = "manualDS";
            this.manualDS.Size = new System.Drawing.Size(93, 89);
            this.manualDS.TabIndex = 3;
            this.manualDS.Value = 0;
            this.manualDS.ValueChanged += new System.EventHandler(this.manualDS_ValueChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lakePNL);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(262, 48);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(370, 376);
            this.panel2.TabIndex = 24;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.zoomInCB);
            this.panel3.Controls.Add(this.zoomOutBTN);
            this.panel3.Controls.Add(this.showBoatsCB);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 354);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(370, 22);
            this.panel3.TabIndex = 0;
            // 
            // images
            // 
            this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
            this.images.TransparentColor = System.Drawing.Color.Transparent;
            this.images.Images.SetKeyName(0, "Magnify.jpg");
            this.images.Images.SetKeyName(1, "ZoomIn.jpg");
            this.images.Images.SetKeyName(2, "ZoomOut.jpg");
            this.images.Images.SetKeyName(3, "Boat.jpg");
            // 
            // zoomInCB
            // 
            this.zoomInCB.Appearance = System.Windows.Forms.Appearance.Button;
            this.zoomInCB.AutoSize = true;
            this.zoomInCB.Dock = System.Windows.Forms.DockStyle.Right;
            this.zoomInCB.ImageIndex = 1;
            this.zoomInCB.ImageList = this.images;
            this.zoomInCB.Location = new System.Drawing.Point(301, 0);
            this.zoomInCB.Name = "zoomInCB";
            this.zoomInCB.Size = new System.Drawing.Size(23, 22);
            this.zoomInCB.TabIndex = 3;
            this.zoomInCB.UseVisualStyleBackColor = true;
            this.zoomInCB.CheckedChanged += new System.EventHandler(this.zoomInCB_CheckedChanged);
            // 
            // zoomOutBTN
            // 
            this.zoomOutBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.zoomOutBTN.ImageIndex = 2;
            this.zoomOutBTN.ImageList = this.images;
            this.zoomOutBTN.Location = new System.Drawing.Point(324, 0);
            this.zoomOutBTN.Name = "zoomOutBTN";
            this.zoomOutBTN.Size = new System.Drawing.Size(23, 22);
            this.zoomOutBTN.TabIndex = 2;
            this.zoomOutBTN.UseVisualStyleBackColor = true;
            this.zoomOutBTN.Click += new System.EventHandler(this.zoomOutBTN_Click);
            // 
            // showBoatsCB
            // 
            this.showBoatsCB.Appearance = System.Windows.Forms.Appearance.Button;
            this.showBoatsCB.AutoSize = true;
            this.showBoatsCB.Checked = true;
            this.showBoatsCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showBoatsCB.Dock = System.Windows.Forms.DockStyle.Right;
            this.showBoatsCB.ImageIndex = 3;
            this.showBoatsCB.ImageList = this.images;
            this.showBoatsCB.Location = new System.Drawing.Point(347, 0);
            this.showBoatsCB.Name = "showBoatsCB";
            this.showBoatsCB.Size = new System.Drawing.Size(23, 22);
            this.showBoatsCB.TabIndex = 0;
            this.showBoatsCB.UseVisualStyleBackColor = true;
            this.showBoatsCB.CheckedChanged += new System.EventHandler(this.showBoatsCB_CheckedChanged);
            // 
            // addBTN
            // 
            this.addBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.add;
            this.addBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addBTN.Location = new System.Drawing.Point(6, 211);
            this.addBTN.Name = "addBTN";
            this.addBTN.Size = new System.Drawing.Size(100, 23);
            this.addBTN.TabIndex = 7;
            this.addBTN.Text = "Add";
            this.addBTN.UseVisualStyleBackColor = true;
            this.addBTN.Click += new System.EventHandler(this.addBTN_Click);
            // 
            // removeBTN
            // 
            this.removeBTN.Enabled = false;
            this.removeBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.delete;
            this.removeBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeBTN.Location = new System.Drawing.Point(112, 211);
            this.removeBTN.Name = "removeBTN";
            this.removeBTN.Size = new System.Drawing.Size(99, 23);
            this.removeBTN.TabIndex = 6;
            this.removeBTN.Text = "Remove";
            this.removeBTN.UseVisualStyleBackColor = true;
            this.removeBTN.Click += new System.EventHandler(this.removeBTN_Click);
            // 
            // removeOrderBTN
            // 
            this.removeOrderBTN.Enabled = false;
            this.removeOrderBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.arrow_left;
            this.removeOrderBTN.Location = new System.Drawing.Point(111, 143);
            this.removeOrderBTN.Name = "removeOrderBTN";
            this.removeOrderBTN.Size = new System.Drawing.Size(32, 23);
            this.removeOrderBTN.TabIndex = 22;
            this.removeOrderBTN.UseVisualStyleBackColor = true;
            this.removeOrderBTN.Click += new System.EventHandler(this.removeOrderBTN_Click);
            // 
            // addOrderBTN
            // 
            this.addOrderBTN.Enabled = false;
            this.addOrderBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.arrow_right;
            this.addOrderBTN.Location = new System.Drawing.Point(111, 114);
            this.addOrderBTN.Name = "addOrderBTN";
            this.addOrderBTN.Size = new System.Drawing.Size(32, 23);
            this.addOrderBTN.TabIndex = 15;
            this.addOrderBTN.UseVisualStyleBackColor = true;
            this.addOrderBTN.Click += new System.EventHandler(this.addOrderBTN_Click);
            // 
            // okBTN
            // 
            this.okBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.okBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.accept;
            this.okBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okBTN.Location = new System.Drawing.Point(432, 0);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(100, 22);
            this.okBTN.TabIndex = 7;
            this.okBTN.Text = "Ok";
            this.okBTN.UseVisualStyleBackColor = true;
            this.okBTN.Click += new System.EventHandler(this.okBTN_Click);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.cancelBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.cancel;
            this.cancelBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelBTN.Location = new System.Drawing.Point(532, 0);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(100, 22);
            this.cancelBTN.TabIndex = 8;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // EditCourses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 446);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.marksTC);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.editCourseGB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditCourses";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Course";
            this.editCourseGB.ResumeLayout(false);
            this.editCourseGB.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.marksTC.ResumeLayout(false);
            this.marksTP.ResumeLayout(false);
            this.marksTP.PerformLayout();
            this.editMarkGB.ResumeLayout(false);
            this.editMarkGB.PerformLayout();
            this.routeTP.ResumeLayout(false);
            this.routeTP.PerformLayout();
            this.windTP.ResumeLayout(false);
            this.windTP.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox editCourseGB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateDP;
        private AmphibianSoftware.VisualSail.UI.Controls.UserDrawnPanel lakePNL;
        private System.Windows.Forms.Button addBTN;
        private System.Windows.Forms.Button removeBTN;
        private System.Windows.Forms.ListBox marksLB;
        private System.Windows.Forms.TextBox markNameTB;
        private System.Windows.Forms.ComboBox markTypeCB;
        private System.Windows.Forms.Label mouseCoordsLBL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button addOrderBTN;
        private System.Windows.Forms.ListBox routeLB;
        private System.Windows.Forms.Button removeOrderBTN;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.TabControl marksTC;
        private System.Windows.Forms.TabPage marksTP;
        private System.Windows.Forms.TabPage routeTP;
        private System.Windows.Forms.ListBox routeMarkLB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox editMarkGB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button zoomOutBTN;
        private System.Windows.Forms.CheckBox showBoatsCB;
        private System.Windows.Forms.CheckBox zoomInCB;
        private System.Windows.Forms.TabPage windTP;
        private System.Windows.Forms.RadioButton windCourseRB;
        private System.Windows.Forms.RadioButton windSensorRB;
        private System.Windows.Forms.RadioButton windManualRB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox windToMarkCB;
        private System.Windows.Forms.ComboBox windFromMarkCB;
        private AmphibianSoftware.VisualSail.UI.Controls.DirectionSelector manualDS;
        private System.Windows.Forms.ImageList images;

    }
}