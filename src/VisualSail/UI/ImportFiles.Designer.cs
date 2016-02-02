namespace AmphibianSoftware.VisualSail.UI
{
    partial class ImportFiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportFiles));
            this.panel1 = new System.Windows.Forms.Panel();
            this.boatLBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileLB = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.fileInfoLBL = new System.Windows.Forms.Label();
            this.removeFileBTN = new System.Windows.Forms.Button();
            this.importBTN = new System.Windows.Forms.Button();
            this.okBTN = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.openFD = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.boatLBL);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 36);
            this.panel1.TabIndex = 0;
            // 
            // boatLBL
            // 
            this.boatLBL.AutoSize = true;
            this.boatLBL.Location = new System.Drawing.Point(50, 9);
            this.boatLBL.Name = "boatLBL";
            this.boatLBL.Size = new System.Drawing.Size(35, 13);
            this.boatLBL.TabIndex = 1;
            this.boatLBL.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Boat:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fileLB);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 257);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Files";
            // 
            // fileLB
            // 
            this.fileLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileLB.FormattingEnabled = true;
            this.fileLB.Location = new System.Drawing.Point(3, 16);
            this.fileLB.Name = "fileLB";
            this.fileLB.Size = new System.Drawing.Size(222, 186);
            this.fileLB.TabIndex = 2;
            this.fileLB.SelectedIndexChanged += new System.EventHandler(this.fileLB_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.fileInfoLBL);
            this.panel2.Controls.Add(this.removeFileBTN);
            this.panel2.Controls.Add(this.importBTN);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 203);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(222, 51);
            this.panel2.TabIndex = 2;
            // 
            // fileInfoLBL
            // 
            this.fileInfoLBL.AutoSize = true;
            this.fileInfoLBL.Location = new System.Drawing.Point(9, 29);
            this.fileInfoLBL.Name = "fileInfoLBL";
            this.fileInfoLBL.Size = new System.Drawing.Size(106, 13);
            this.fileInfoLBL.TabIndex = 2;
            this.fileInfoLBL.Text = "this is the default text";
            // 
            // removeFileBTN
            // 
            this.removeFileBTN.Enabled = false;
            this.removeFileBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.chart_line_delete;
            this.removeFileBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeFileBTN.Location = new System.Drawing.Point(115, 3);
            this.removeFileBTN.Name = "removeFileBTN";
            this.removeFileBTN.Size = new System.Drawing.Size(100, 23);
            this.removeFileBTN.TabIndex = 2;
            this.removeFileBTN.Text = "Remove File";
            this.removeFileBTN.UseVisualStyleBackColor = true;
            this.removeFileBTN.Click += new System.EventHandler(this.removeFileBTN_Click);
            // 
            // importBTN
            // 
            this.importBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.chart_line_add;
            this.importBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.importBTN.Location = new System.Drawing.Point(9, 3);
            this.importBTN.Name = "importBTN";
            this.importBTN.Size = new System.Drawing.Size(100, 23);
            this.importBTN.TabIndex = 2;
            this.importBTN.Text = "Import File";
            this.importBTN.UseVisualStyleBackColor = true;
            this.importBTN.Click += new System.EventHandler(this.importBTN_Click);
            // 
            // okBTN
            // 
            this.okBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.okBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.accept;
            this.okBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okBTN.Location = new System.Drawing.Point(128, 0);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(100, 23);
            this.okBTN.TabIndex = 3;
            this.okBTN.Text = "Ok";
            this.okBTN.UseVisualStyleBackColor = true;
            this.okBTN.Click += new System.EventHandler(this.okBTN_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.okBTN);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 293);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(228, 23);
            this.panel3.TabIndex = 4;
            // 
            // openFD
            // 
            this.openFD.Filter = "All Gps Files|*.csv;*.gpx;*.kml;*.nmea;*.txt;*.vcc;*.log|CSV Files|*.csv|Google E" +
                "arth Files|*.kml|GPX Files|*.gpx|NMEA Files|*.nmea|Velocitek Files|*.vcc|Log Fil" +
                "es|*.log|All Files|*.*";
            // 
            // ImportFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 316);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import GPS Files";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label boatLBL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox fileLB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button removeFileBTN;
        private System.Windows.Forms.Button importBTN;
        private System.Windows.Forms.Label fileInfoLBL;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.OpenFileDialog openFD;
    }
}