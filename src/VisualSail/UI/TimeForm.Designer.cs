namespace AmphibianSoftware.VisualSail.UI
{
    partial class TimeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeForm));
            this.reverseBTN = new System.Windows.Forms.Button();
            this.icons = new System.Windows.Forms.ImageList(this.components);
            this.forwardBTN = new System.Windows.Forms.Button();
            this.statusLBL = new System.Windows.Forms.Label();
            this.speedTB = new System.Windows.Forms.TrackBar();
            this.pauseBtn = new System.Windows.Forms.Button();
            this.positionTB = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timeUDP = new AmphibianSoftware.VisualSail.UI.Controls.UserDrawnPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.speedTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionTB)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // reverseBTN
            // 
            this.reverseBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this.reverseBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.resultset_previous;
            this.reverseBTN.Location = new System.Drawing.Point(0, 0);
            this.reverseBTN.Name = "reverseBTN";
            this.reverseBTN.Size = new System.Drawing.Size(54, 22);
            this.reverseBTN.TabIndex = 8;
            this.reverseBTN.UseVisualStyleBackColor = true;
            this.reverseBTN.Click += new System.EventHandler(this.reverseBTN_Click);
            // 
            // icons
            // 
            this.icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("icons.ImageStream")));
            this.icons.TransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(210)))));
            this.icons.Images.SetKeyName(0, "down.bmp");
            this.icons.Images.SetKeyName(1, "left.bmp");
            this.icons.Images.SetKeyName(2, "Magnify.jpg");
            this.icons.Images.SetKeyName(3, "right.bmp");
            this.icons.Images.SetKeyName(4, "up.bmp");
            this.icons.Images.SetKeyName(5, "ZoomIn.jpg");
            this.icons.Images.SetKeyName(6, "ZoomOut.jpg");
            // 
            // forwardBTN
            // 
            this.forwardBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.forwardBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.resultset_next;
            this.forwardBTN.Location = new System.Drawing.Point(154, 0);
            this.forwardBTN.Name = "forwardBTN";
            this.forwardBTN.Size = new System.Drawing.Size(48, 22);
            this.forwardBTN.TabIndex = 7;
            this.forwardBTN.UseVisualStyleBackColor = true;
            this.forwardBTN.Click += new System.EventHandler(this.forwardBTN_Click);
            // 
            // statusLBL
            // 
            this.statusLBL.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusLBL.Location = new System.Drawing.Point(0, 116);
            this.statusLBL.MinimumSize = new System.Drawing.Size(100, 0);
            this.statusLBL.Name = "statusLBL";
            this.statusLBL.Size = new System.Drawing.Size(202, 13);
            this.statusLBL.TabIndex = 6;
            this.statusLBL.Text = ">";
            this.statusLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // speedTB
            // 
            this.speedTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.speedTB.Location = new System.Drawing.Point(0, 0);
            this.speedTB.Maximum = 16;
            this.speedTB.Name = "speedTB";
            this.speedTB.Size = new System.Drawing.Size(148, 22);
            this.speedTB.TabIndex = 5;
            this.speedTB.Value = 9;
            this.speedTB.Scroll += new System.EventHandler(this.speedTB_Scroll);
            // 
            // pauseBtn
            // 
            this.pauseBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pauseBtn.Location = new System.Drawing.Point(54, 0);
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(100, 22);
            this.pauseBtn.TabIndex = 0;
            this.pauseBtn.Text = "| |";
            this.pauseBtn.UseVisualStyleBackColor = true;
            this.pauseBtn.Click += new System.EventHandler(this.pauseBtn_Click);
            // 
            // positionTB
            // 
            this.positionTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positionTB.Location = new System.Drawing.Point(0, 0);
            this.positionTB.Maximum = 1;
            this.positionTB.Name = "positionTB";
            this.positionTB.Size = new System.Drawing.Size(148, 22);
            this.positionTB.TabIndex = 9;
            this.positionTB.TickFrequency = 60;
            this.positionTB.Scroll += new System.EventHandler(this.positionTB_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Speed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Position";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.timeUDP);
            this.panel1.Controls.Add(this.statusLBL);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(202, 129);
            this.panel1.TabIndex = 12;
            // 
            // timeUDP
            // 
            this.timeUDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeUDP.Location = new System.Drawing.Point(0, 0);
            this.timeUDP.Name = "timeUDP";
            this.timeUDP.Size = new System.Drawing.Size(202, 116);
            this.timeUDP.TabIndex = 7;
            this.timeUDP.Paint += new System.Windows.Forms.PaintEventHandler(this.timeUDP_Paint);
            this.timeUDP.MouseClick += new System.Windows.Forms.MouseEventHandler(this.timeUDP_MouseClick);
            this.timeUDP.Resize += new System.EventHandler(this.timeUDP_Resize);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 129);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(202, 67);
            this.panel2.TabIndex = 13;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 44);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(202, 22);
            this.panel5.TabIndex = 14;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.positionTB);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(54, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(148, 22);
            this.panel7.TabIndex = 13;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(54, 22);
            this.panel6.TabIndex = 12;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel9);
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 22);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(202, 22);
            this.panel4.TabIndex = 13;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.speedTB);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(54, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(148, 22);
            this.panel9.TabIndex = 12;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(54, 22);
            this.panel8.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pauseBtn);
            this.panel3.Controls.Add(this.reverseBTN);
            this.panel3.Controls.Add(this.forwardBTN);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(202, 22);
            this.panel3.TabIndex = 12;
            // 
            // TimeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 196);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(182, 24);
            this.Name = "TimeForm";
            this.ShowInTaskbar = false;
            this.TabText = "Playback Control";
            this.Text = "Playback Control";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TimeForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.speedTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionTB)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button reverseBTN;
        private System.Windows.Forms.Button forwardBTN;
        private System.Windows.Forms.Label statusLBL;
        private System.Windows.Forms.TrackBar speedTB;
        private System.Windows.Forms.Button pauseBtn;
        private System.Windows.Forms.TrackBar positionTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList icons;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private AmphibianSoftware.VisualSail.UI.Controls.UserDrawnPanel timeUDP;
    }
}