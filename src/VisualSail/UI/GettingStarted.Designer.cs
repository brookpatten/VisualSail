namespace AmphibianSoftware.VisualSail.UI
{
    partial class GettingStarted
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GettingStarted));
            this.panel1 = new System.Windows.Forms.Panel();
            this.openLBL = new System.Windows.Forms.Label();
            this.newLBL = new System.Windows.Forms.Label();
            this.openBTN = new System.Windows.Forms.Button();
            this.newBTN = new System.Windows.Forms.Button();
            this.gpsBTN = new System.Windows.Forms.Button();
            this.gpsLBL = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.gpsLBL);
            this.panel1.Controls.Add(this.gpsBTN);
            this.panel1.Controls.Add(this.openLBL);
            this.panel1.Controls.Add(this.newLBL);
            this.panel1.Controls.Add(this.openBTN);
            this.panel1.Controls.Add(this.newBTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 349);
            this.panel1.TabIndex = 0;
            // 
            // openLBL
            // 
            this.openLBL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.openLBL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.openLBL.Location = new System.Drawing.Point(84, 139);
            this.openLBL.Name = "openLBL";
            this.openLBL.Padding = new System.Windows.Forms.Padding(3);
            this.openLBL.Size = new System.Drawing.Size(191, 41);
            this.openLBL.TabIndex = 4;
            this.openLBL.Text = "Open an existing race series from a .Sail file.";
            this.openLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.openLBL.Click += new System.EventHandler(this.openLBL_Click);
            // 
            // newLBL
            // 
            this.newLBL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.newLBL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newLBL.Location = new System.Drawing.Point(84, 92);
            this.newLBL.Name = "newLBL";
            this.newLBL.Padding = new System.Windows.Forms.Padding(3);
            this.newLBL.Size = new System.Drawing.Size(191, 41);
            this.newLBL.TabIndex = 1;
            this.newLBL.Text = "Create a new race series where you can add boats one by one.";
            this.newLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.newLBL.Click += new System.EventHandler(this.newLBL_Click);
            // 
            // openBTN
            // 
            this.openBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.openBTN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.openBTN.ForeColor = System.Drawing.Color.White;
            this.openBTN.Location = new System.Drawing.Point(12, 139);
            this.openBTN.Name = "openBTN";
            this.openBTN.Size = new System.Drawing.Size(75, 41);
            this.openBTN.TabIndex = 2;
            this.openBTN.Text = "Open Series";
            this.openBTN.UseVisualStyleBackColor = false;
            this.openBTN.Click += new System.EventHandler(this.openBTN_Click);
            // 
            // newBTN
            // 
            this.newBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.newBTN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.newBTN.ForeColor = System.Drawing.Color.White;
            this.newBTN.Location = new System.Drawing.Point(12, 92);
            this.newBTN.Name = "newBTN";
            this.newBTN.Size = new System.Drawing.Size(75, 41);
            this.newBTN.TabIndex = 1;
            this.newBTN.Text = "New Series";
            this.newBTN.UseVisualStyleBackColor = false;
            this.newBTN.Click += new System.EventHandler(this.newBTN_Click);
            // 
            // gpsBTN
            // 
            this.gpsBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gpsBTN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gpsBTN.ForeColor = System.Drawing.Color.White;
            this.gpsBTN.Location = new System.Drawing.Point(12, 186);
            this.gpsBTN.Name = "gpsBTN";
            this.gpsBTN.Size = new System.Drawing.Size(75, 41);
            this.gpsBTN.TabIndex = 1;
            this.gpsBTN.Text = "Import Files";
            this.gpsBTN.UseVisualStyleBackColor = false;
            this.gpsBTN.Click += new System.EventHandler(this.gpsBTN_Click);
            // 
            // gpsLBL
            // 
            this.gpsLBL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gpsLBL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gpsLBL.Location = new System.Drawing.Point(84, 186);
            this.gpsLBL.Name = "gpsLBL";
            this.gpsLBL.Padding = new System.Windows.Forms.Padding(3);
            this.gpsLBL.Size = new System.Drawing.Size(191, 41);
            this.gpsLBL.TabIndex = 5;
            this.gpsLBL.Text = "Select a group of GPS files to create a new series.";
            this.gpsLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gpsLBL.Click += new System.EventHandler(this.gpsLBL_Click);
            // 
            // GettingStarted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
			this.BackgroundImage = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("backdrop.png");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(492, 349);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 375);
            this.Name = "GettingStarted";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TabText = "Getting Started";
            this.Text = "Getting Started";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button newBTN;
        private System.Windows.Forms.Button openBTN;
        private System.Windows.Forms.Label newLBL;
        private System.Windows.Forms.Label openLBL;
        private System.Windows.Forms.Button gpsBTN;
        private System.Windows.Forms.Label gpsLBL;



    }
}