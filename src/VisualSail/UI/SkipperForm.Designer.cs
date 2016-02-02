namespace AmphibianSoftware.Skipper.UI
{
    partial class SkipperForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkipperForm));
            this.upperContainerPanel = new System.Windows.Forms.Panel();
            this.viewPanel = new AmphibianSoftware.Skipper.UI.RenderPanel();
            this.toolPanel = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.angleCB = new System.Windows.Forms.CheckBox();
            this.gridCB = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.zoomOutBTN = new System.Windows.Forms.Button();
            this.zoomInBTN = new System.Windows.Forms.Button();
            this.moveDownBTN = new System.Windows.Forms.Button();
            this.icons = new System.Windows.Forms.ImageList(this.components);
            this.moveRightBTN = new System.Windows.Forms.Button();
            this.moveLeftBTN = new System.Windows.Forms.Button();
            this.moveUpBTN = new System.Windows.Forms.Button();
            this.cameraFollowRB = new System.Windows.Forms.RadioButton();
            this.freeRB = new System.Windows.Forms.RadioButton();
            this.boatsLB = new System.Windows.Forms.ListBox();
            this.instrumentPanel = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.statsLV = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.reverseBTN = new System.Windows.Forms.Button();
            this.forwardBTN = new System.Windows.Forms.Button();
            this.statusLBL = new System.Windows.Forms.Label();
            this.speedTB = new System.Windows.Forms.TrackBar();
            this.timeLBL = new System.Windows.Forms.Label();
            this.pauseBtn = new System.Windows.Forms.Button();
            this.statisticsLV = new System.Windows.Forms.ListView();
            this.followRB = new System.Windows.Forms.RadioButton();
            this.freeLookRB = new System.Windows.Forms.RadioButton();
            this.currentTimeLBL = new System.Windows.Forms.Label();
            this.upperContainerPanel.SuspendLayout();
            this.toolPanel.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.instrumentPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTB)).BeginInit();
            this.SuspendLayout();
            // 
            // upperContainerPanel
            // 
            this.upperContainerPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.upperContainerPanel.Controls.Add(this.viewPanel);
            this.upperContainerPanel.Controls.Add(this.toolPanel);
            this.upperContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upperContainerPanel.Location = new System.Drawing.Point(0, 0);
            this.upperContainerPanel.Name = "upperContainerPanel";
            this.upperContainerPanel.Size = new System.Drawing.Size(807, 580);
            this.upperContainerPanel.TabIndex = 0;
            // 
            // viewPanel
            // 
            this.viewPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.viewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewPanel.Location = new System.Drawing.Point(125, 0);
            this.viewPanel.Name = "viewPanel";
            this.viewPanel.Play = false;
            this.viewPanel.RenderTime = new System.DateTime(((long)(0)));
            this.viewPanel.Size = new System.Drawing.Size(682, 580);
            this.viewPanel.Speed = 1;
            this.viewPanel.TabIndex = 1;
            // 
            // toolPanel
            // 
            this.toolPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.toolPanel.BackColor = System.Drawing.SystemColors.Control;
            this.toolPanel.Controls.Add(this.groupBox5);
            this.toolPanel.Controls.Add(this.groupBox4);
            this.toolPanel.Controls.Add(this.groupBox3);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolPanel.Location = new System.Drawing.Point(0, 0);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Size = new System.Drawing.Size(125, 580);
            this.toolPanel.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.angleCB);
            this.groupBox5.Controls.Add(this.gridCB);
            this.groupBox5.Location = new System.Drawing.Point(3, 311);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(119, 94);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "View";
            // 
            // angleCB
            // 
            this.angleCB.AutoSize = true;
            this.angleCB.Checked = true;
            this.angleCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.angleCB.Location = new System.Drawing.Point(6, 43);
            this.angleCB.Name = "angleCB";
            this.angleCB.Size = new System.Drawing.Size(96, 17);
            this.angleCB.TabIndex = 1;
            this.angleCB.Text = "Angle To Mark";
            this.angleCB.UseVisualStyleBackColor = true;
            this.angleCB.CheckedChanged += new System.EventHandler(this.angleCB_CheckedChanged);
            // 
            // gridCB
            // 
            this.gridCB.AutoSize = true;
            this.gridCB.Checked = true;
            this.gridCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gridCB.Location = new System.Drawing.Point(6, 20);
            this.gridCB.Name = "gridCB";
            this.gridCB.Size = new System.Drawing.Size(45, 17);
            this.gridCB.TabIndex = 0;
            this.gridCB.Text = "Grid";
            this.gridCB.UseVisualStyleBackColor = true;
            this.gridCB.CheckedChanged += new System.EventHandler(this.gridCB_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Location = new System.Drawing.Point(3, 254);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(119, 51);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sensors";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Sensor Dump";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.cameraFollowRB);
            this.groupBox3.Controls.Add(this.freeRB);
            this.groupBox3.Controls.Add(this.boatsLB);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(119, 245);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Camera";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.zoomOutBTN);
            this.groupBox6.Controls.Add(this.zoomInBTN);
            this.groupBox6.Controls.Add(this.moveDownBTN);
            this.groupBox6.Controls.Add(this.moveRightBTN);
            this.groupBox6.Controls.Add(this.moveLeftBTN);
            this.groupBox6.Controls.Add(this.moveUpBTN);
            this.groupBox6.Location = new System.Drawing.Point(6, 140);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(107, 100);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Control";
            // 
            // zoomOutBTN
            // 
            this.zoomOutBTN.Location = new System.Drawing.Point(74, 69);
            this.zoomOutBTN.Name = "zoomOutBTN";
            this.zoomOutBTN.Size = new System.Drawing.Size(26, 23);
            this.zoomOutBTN.TabIndex = 5;
            this.zoomOutBTN.Text = "-";
            this.zoomOutBTN.UseVisualStyleBackColor = true;
            this.zoomOutBTN.Click += new System.EventHandler(this.zoomOutBTN_Click);
            // 
            // zoomInBTN
            // 
            this.zoomInBTN.Location = new System.Drawing.Point(74, 10);
            this.zoomInBTN.Name = "zoomInBTN";
            this.zoomInBTN.Size = new System.Drawing.Size(27, 23);
            this.zoomInBTN.TabIndex = 4;
            this.zoomInBTN.Text = "+";
            this.zoomInBTN.UseVisualStyleBackColor = true;
            this.zoomInBTN.Click += new System.EventHandler(this.zoomInBTN_Click);
            // 
            // moveDownBTN
            // 
            this.moveDownBTN.ImageIndex = 1;
            this.moveDownBTN.ImageList = this.icons;
            this.moveDownBTN.Location = new System.Drawing.Point(42, 68);
            this.moveDownBTN.Name = "moveDownBTN";
            this.moveDownBTN.Size = new System.Drawing.Size(26, 23);
            this.moveDownBTN.TabIndex = 3;
            this.moveDownBTN.UseVisualStyleBackColor = true;
            this.moveDownBTN.Click += new System.EventHandler(this.moveDownBTN_Click);
            // 
            // icons
            // 
            this.icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("icons.ImageStream")));
            this.icons.TransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(210)))));
            this.icons.Images.SetKeyName(0, "up");
            this.icons.Images.SetKeyName(1, "down");
            this.icons.Images.SetKeyName(2, "left");
            this.icons.Images.SetKeyName(3, "right");
            // 
            // moveRightBTN
            // 
            this.moveRightBTN.ImageIndex = 3;
            this.moveRightBTN.ImageList = this.icons;
            this.moveRightBTN.Location = new System.Drawing.Point(74, 39);
            this.moveRightBTN.Name = "moveRightBTN";
            this.moveRightBTN.Size = new System.Drawing.Size(26, 23);
            this.moveRightBTN.TabIndex = 2;
            this.moveRightBTN.UseVisualStyleBackColor = true;
            this.moveRightBTN.Click += new System.EventHandler(this.moveRightBTN_Click);
            // 
            // moveLeftBTN
            // 
            this.moveLeftBTN.ImageIndex = 2;
            this.moveLeftBTN.ImageList = this.icons;
            this.moveLeftBTN.Location = new System.Drawing.Point(10, 39);
            this.moveLeftBTN.Name = "moveLeftBTN";
            this.moveLeftBTN.Size = new System.Drawing.Size(26, 23);
            this.moveLeftBTN.TabIndex = 1;
            this.moveLeftBTN.UseVisualStyleBackColor = true;
            this.moveLeftBTN.Click += new System.EventHandler(this.moveLeftBTN_Click);
            // 
            // moveUpBTN
            // 
            this.moveUpBTN.ImageIndex = 0;
            this.moveUpBTN.ImageList = this.icons;
            this.moveUpBTN.Location = new System.Drawing.Point(42, 10);
            this.moveUpBTN.Name = "moveUpBTN";
            this.moveUpBTN.Size = new System.Drawing.Size(26, 23);
            this.moveUpBTN.TabIndex = 0;
            this.moveUpBTN.UseVisualStyleBackColor = true;
            this.moveUpBTN.Click += new System.EventHandler(this.moveUpBTN_Click);
            // 
            // cameraFollowRB
            // 
            this.cameraFollowRB.AutoSize = true;
            this.cameraFollowRB.Checked = true;
            this.cameraFollowRB.Location = new System.Drawing.Point(9, 42);
            this.cameraFollowRB.Name = "cameraFollowRB";
            this.cameraFollowRB.Size = new System.Drawing.Size(80, 17);
            this.cameraFollowRB.TabIndex = 7;
            this.cameraFollowRB.TabStop = true;
            this.cameraFollowRB.Text = "Follow Boat";
            this.cameraFollowRB.UseVisualStyleBackColor = true;
            // 
            // freeRB
            // 
            this.freeRB.AutoSize = true;
            this.freeRB.Location = new System.Drawing.Point(9, 19);
            this.freeRB.Name = "freeRB";
            this.freeRB.Size = new System.Drawing.Size(46, 17);
            this.freeRB.TabIndex = 7;
            this.freeRB.Text = "Free";
            this.freeRB.UseVisualStyleBackColor = true;
            // 
            // boatsLB
            // 
            this.boatsLB.FormattingEnabled = true;
            this.boatsLB.Location = new System.Drawing.Point(22, 65);
            this.boatsLB.Name = "boatsLB";
            this.boatsLB.Size = new System.Drawing.Size(91, 69);
            this.boatsLB.TabIndex = 0;
            this.boatsLB.SelectedIndexChanged += new System.EventHandler(this.boatsLB_SelectedIndexChanged);
            // 
            // instrumentPanel
            // 
            this.instrumentPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.instrumentPanel.BackColor = System.Drawing.SystemColors.Control;
            this.instrumentPanel.Controls.Add(this.groupBox2);
            this.instrumentPanel.Controls.Add(this.groupBox1);
            this.instrumentPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.instrumentPanel.Location = new System.Drawing.Point(0, 424);
            this.instrumentPanel.Name = "instrumentPanel";
            this.instrumentPanel.Size = new System.Drawing.Size(807, 156);
            this.instrumentPanel.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.statsLV);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(630, 156);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statistics";
            // 
            // statsLV
            // 
            this.statsLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsLV.Location = new System.Drawing.Point(3, 16);
            this.statsLV.Name = "statsLV";
            this.statsLV.Size = new System.Drawing.Size(624, 137);
            this.statsLV.TabIndex = 5;
            this.statsLV.UseCompatibleStateImageBehavior = false;
            this.statsLV.View = System.Windows.Forms.View.Details;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.reverseBTN);
            this.groupBox1.Controls.Add(this.forwardBTN);
            this.groupBox1.Controls.Add(this.statusLBL);
            this.groupBox1.Controls.Add(this.speedTB);
            this.groupBox1.Controls.Add(this.timeLBL);
            this.groupBox1.Controls.Add(this.pauseBtn);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(630, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 156);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Playback";
            // 
            // reverseBTN
            // 
            this.reverseBTN.Location = new System.Drawing.Point(6, 54);
            this.reverseBTN.Name = "reverseBTN";
            this.reverseBTN.Size = new System.Drawing.Size(54, 23);
            this.reverseBTN.TabIndex = 8;
            this.reverseBTN.Text = "<";
            this.reverseBTN.UseVisualStyleBackColor = true;
            this.reverseBTN.Click += new System.EventHandler(this.reverseBTN_Click);
            // 
            // forwardBTN
            // 
            this.forwardBTN.Location = new System.Drawing.Point(119, 54);
            this.forwardBTN.Name = "forwardBTN";
            this.forwardBTN.Size = new System.Drawing.Size(48, 23);
            this.forwardBTN.TabIndex = 7;
            this.forwardBTN.Text = ">";
            this.forwardBTN.UseVisualStyleBackColor = true;
            this.forwardBTN.Click += new System.EventHandler(this.forwardBTN_Click);
            // 
            // statusLBL
            // 
            this.statusLBL.AutoSize = true;
            this.statusLBL.Location = new System.Drawing.Point(38, 131);
            this.statusLBL.MinimumSize = new System.Drawing.Size(100, 0);
            this.statusLBL.Name = "statusLBL";
            this.statusLBL.Size = new System.Drawing.Size(100, 13);
            this.statusLBL.TabIndex = 6;
            this.statusLBL.Text = ">";
            this.statusLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // speedTB
            // 
            this.speedTB.Location = new System.Drawing.Point(6, 83);
            this.speedTB.Maximum = 16;
            this.speedTB.Name = "speedTB";
            this.speedTB.Size = new System.Drawing.Size(163, 45);
            this.speedTB.TabIndex = 5;
            this.speedTB.Value = 9;
            this.speedTB.Scroll += new System.EventHandler(this.speedTB_Scroll);
            // 
            // timeLBL
            // 
            this.timeLBL.AutoSize = true;
            this.timeLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLBL.Location = new System.Drawing.Point(45, 25);
            this.timeLBL.Name = "timeLBL";
            this.timeLBL.Size = new System.Drawing.Size(93, 26);
            this.timeLBL.TabIndex = 4;
            this.timeLBL.Text = "timeLBL";
            // 
            // pauseBtn
            // 
            this.pauseBtn.Location = new System.Drawing.Point(66, 54);
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(47, 23);
            this.pauseBtn.TabIndex = 0;
            this.pauseBtn.Text = "Pause";
            this.pauseBtn.UseVisualStyleBackColor = true;
            this.pauseBtn.Click += new System.EventHandler(this.pauseBtn_Click);
            // 
            // statisticsLV
            // 
            this.statisticsLV.Location = new System.Drawing.Point(6, 14);
            this.statisticsLV.Name = "statisticsLV";
            this.statisticsLV.Size = new System.Drawing.Size(429, 126);
            this.statisticsLV.TabIndex = 5;
            this.statisticsLV.UseCompatibleStateImageBehavior = false;
            this.statisticsLV.View = System.Windows.Forms.View.Details;
            // 
            // followRB
            // 
            this.followRB.AutoSize = true;
            this.followRB.Location = new System.Drawing.Point(9, 42);
            this.followRB.Name = "followRB";
            this.followRB.Size = new System.Drawing.Size(80, 17);
            this.followRB.TabIndex = 7;
            this.followRB.TabStop = true;
            this.followRB.Text = "Follow Boat";
            this.followRB.UseVisualStyleBackColor = true;
            // 
            // freeLookRB
            // 
            this.freeLookRB.AutoSize = true;
            this.freeLookRB.Location = new System.Drawing.Point(9, 19);
            this.freeLookRB.Name = "freeLookRB";
            this.freeLookRB.Size = new System.Drawing.Size(46, 17);
            this.freeLookRB.TabIndex = 7;
            this.freeLookRB.TabStop = true;
            this.freeLookRB.Text = "Free";
            this.freeLookRB.UseVisualStyleBackColor = true;
            // 
            // currentTimeLBL
            // 
            this.currentTimeLBL.AutoSize = true;
            this.currentTimeLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentTimeLBL.Location = new System.Drawing.Point(45, 25);
            this.currentTimeLBL.Name = "currentTimeLBL";
            this.currentTimeLBL.Size = new System.Drawing.Size(93, 26);
            this.currentTimeLBL.TabIndex = 4;
            // 
            // SkipperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 580);
            this.Controls.Add(this.instrumentPanel);
            this.Controls.Add(this.upperContainerPanel);
            this.Name = "SkipperForm";
            this.Text = "Skipper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SkipperForm_FormClosing);
            this.upperContainerPanel.ResumeLayout(false);
            this.toolPanel.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.instrumentPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel upperContainerPanel;
        private System.Windows.Forms.Panel toolPanel;
        private System.Windows.Forms.Panel instrumentPanel;
        private RenderPanel viewPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox boatsLB;
        private System.Windows.Forms.Button pauseBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton cameraFollowRB;
        private System.Windows.Forms.RadioButton freeRB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView statsLV;
        private System.Windows.Forms.ListView statisticsLV;
        private System.Windows.Forms.RadioButton followRB;
        private System.Windows.Forms.RadioButton freeLookRB;
        private System.Windows.Forms.Label timeLBL;
        private System.Windows.Forms.Label currentTimeLBL;
        private System.Windows.Forms.TrackBar speedTB;
        private System.Windows.Forms.Label statusLBL;
        private System.Windows.Forms.Button reverseBTN;
        private System.Windows.Forms.Button forwardBTN;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button moveUpBTN;
        private System.Windows.Forms.ImageList icons;
        private System.Windows.Forms.Button moveDownBTN;
        private System.Windows.Forms.Button moveRightBTN;
        private System.Windows.Forms.Button moveLeftBTN;
        private System.Windows.Forms.Button zoomOutBTN;
        private System.Windows.Forms.Button zoomInBTN;
        private System.Windows.Forms.CheckBox gridCB;
        private System.Windows.Forms.CheckBox angleCB;
    }
}

