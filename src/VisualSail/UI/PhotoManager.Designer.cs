namespace AmphibianSoftware.VisualSail.UI
{
    partial class PhotoManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhotoManager));
            this.photosLV = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.addFolderBTN = new System.Windows.Forms.Button();
            this.addFilesBTN = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.drawPNL = new AmphibianSoftware.VisualSail.UI.Controls.UserDrawnPanel();
            this.editPhotoGB = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.boatsLV = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.panel2 = new System.Windows.Forms.Panel();
            this.captionTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.takenAtDTP = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.filenameLBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameLBL = new System.Windows.Forms.Label();
            this.deleteBTN = new System.Windows.Forms.Button();
            this.saveBTN = new System.Windows.Forms.Button();
            this.rotateRightBTN = new System.Windows.Forms.Button();
            this.rotateLeftBTN = new System.Windows.Forms.Button();
            this.folderBD = new System.Windows.Forms.FolderBrowserDialog();
            this.openFD = new System.Windows.Forms.OpenFileDialog();
            this.saveFD = new System.Windows.Forms.SaveFileDialog();
            this.panel4 = new System.Windows.Forms.Panel();
            this.sizeLBL = new System.Windows.Forms.Label();
            this.okBTN = new System.Windows.Forms.Button();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.editPhotoGB.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // photosLV
            // 
            this.photosLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.photosLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.photosLV.FullRowSelect = true;
            this.photosLV.HideSelection = false;
            this.photosLV.Location = new System.Drawing.Point(3, 38);
            this.photosLV.Name = "photosLV";
            this.photosLV.Size = new System.Drawing.Size(200, 381);
            this.photosLV.TabIndex = 0;
            this.photosLV.UseCompatibleStateImageBehavior = false;
            this.photosLV.View = System.Windows.Forms.View.Details;
            this.photosLV.SelectedIndexChanged += new System.EventHandler(this.photosLV_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Time";
            this.columnHeader2.Width = 74;
            // 
            // addFolderBTN
            // 
            this.addFolderBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this.addFolderBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.folderbrowse;
            this.addFolderBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addFolderBTN.Location = new System.Drawing.Point(0, 0);
            this.addFolderBTN.Name = "addFolderBTN";
            this.addFolderBTN.Size = new System.Drawing.Size(100, 22);
            this.addFolderBTN.TabIndex = 2;
            this.addFolderBTN.Text = "Add Folder";
            this.addFolderBTN.UseVisualStyleBackColor = true;
            this.addFolderBTN.Click += new System.EventHandler(this.addFolderBTN_Click);
            // 
            // addFilesBTN
            // 
            this.addFilesBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this.addFilesBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.filebrowse;
            this.addFilesBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addFilesBTN.Location = new System.Drawing.Point(100, 0);
            this.addFilesBTN.Name = "addFilesBTN";
            this.addFilesBTN.Size = new System.Drawing.Size(100, 22);
            this.addFilesBTN.TabIndex = 3;
            this.addFilesBTN.Text = "Add Files";
            this.addFilesBTN.UseVisualStyleBackColor = true;
            this.addFilesBTN.Click += new System.EventHandler(this.addFilesBTN_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.photosLV);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 422);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Photos";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.addFilesBTN);
            this.panel1.Controls.Add(this.addFolderBTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 22);
            this.panel1.TabIndex = 5;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(206, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.drawPNL);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.editPhotoGB);
            this.splitContainer1.Size = new System.Drawing.Size(418, 422);
            this.splitContainer1.SplitterDistance = 276;
            this.splitContainer1.TabIndex = 5;
            // 
            // drawPNL
            // 
            this.drawPNL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawPNL.Location = new System.Drawing.Point(0, 0);
            this.drawPNL.Name = "drawPNL";
            this.drawPNL.Size = new System.Drawing.Size(418, 276);
            this.drawPNL.TabIndex = 0;
            this.drawPNL.Paint += new System.Windows.Forms.PaintEventHandler(this.drawPNL_Paint);
            // 
            // editPhotoGB
            // 
            this.editPhotoGB.Controls.Add(this.groupBox3);
            this.editPhotoGB.Controls.Add(this.panel2);
            this.editPhotoGB.Controls.Add(this.panel3);
            this.editPhotoGB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editPhotoGB.Enabled = false;
            this.editPhotoGB.Location = new System.Drawing.Point(0, 0);
            this.editPhotoGB.Name = "editPhotoGB";
            this.editPhotoGB.Size = new System.Drawing.Size(418, 142);
            this.editPhotoGB.TabIndex = 0;
            this.editPhotoGB.TabStop = false;
            this.editPhotoGB.Text = "Edit Photo";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.boatsLV);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 60);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(412, 79);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Boats in this Photo";
            // 
            // boatsLV
            // 
            this.boatsLV.CheckBoxes = true;
            this.boatsLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.boatsLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boatsLV.Location = new System.Drawing.Point(3, 16);
            this.boatsLV.Name = "boatsLV";
            this.boatsLV.Size = new System.Drawing.Size(406, 60);
            this.boatsLV.TabIndex = 0;
            this.boatsLV.UseCompatibleStateImageBehavior = false;
            this.boatsLV.View = System.Windows.Forms.View.SmallIcon;
            this.boatsLV.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.boatsLV_ItemChecked);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Number";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.captionTB);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.takenAtDTP);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(412, 22);
            this.panel2.TabIndex = 2;
            // 
            // captionTB
            // 
            this.captionTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captionTB.Location = new System.Drawing.Point(233, 0);
            this.captionTB.Name = "captionTB";
            this.captionTB.Size = new System.Drawing.Size(179, 20);
            this.captionTB.TabIndex = 5;
            this.captionTB.TextChanged += new System.EventHandler(this.captionTB_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(190, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Caption";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // takenAtDTP
            // 
            this.takenAtDTP.CustomFormat = "M/d/yyyy h:mm:ss tt";
            this.takenAtDTP.Dock = System.Windows.Forms.DockStyle.Left;
            this.takenAtDTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.takenAtDTP.Location = new System.Drawing.Point(30, 0);
            this.takenAtDTP.Name = "takenAtDTP";
            this.takenAtDTP.Size = new System.Drawing.Size(160, 20);
            this.takenAtDTP.TabIndex = 3;
            this.takenAtDTP.ValueChanged += new System.EventHandler(this.takenAtDTP_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Time";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.filenameLBL);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.nameLBL);
            this.panel3.Controls.Add(this.deleteBTN);
            this.panel3.Controls.Add(this.saveBTN);
            this.panel3.Controls.Add(this.rotateRightBTN);
            this.panel3.Controls.Add(this.rotateLeftBTN);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(412, 22);
            this.panel3.TabIndex = 1;
            // 
            // filenameLBL
            // 
            this.filenameLBL.AutoSize = true;
            this.filenameLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filenameLBL.Location = new System.Drawing.Point(156, 0);
            this.filenameLBL.Name = "filenameLBL";
            this.filenameLBL.Size = new System.Drawing.Size(0, 13);
            this.filenameLBL.TabIndex = 4;
            this.filenameLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(118, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nameLBL
            // 
            this.nameLBL.AutoSize = true;
            this.nameLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameLBL.Location = new System.Drawing.Point(118, 0);
            this.nameLBL.Name = "nameLBL";
            this.nameLBL.Size = new System.Drawing.Size(33, 13);
            this.nameLBL.TabIndex = 1;
            this.nameLBL.Text = "name";
            // 
            // deleteBTN
            // 
            this.deleteBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this.deleteBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.cross;
            this.deleteBTN.Location = new System.Drawing.Point(88, 0);
            this.deleteBTN.Name = "deleteBTN";
            this.deleteBTN.Size = new System.Drawing.Size(30, 22);
            this.deleteBTN.TabIndex = 2;
            this.deleteBTN.UseVisualStyleBackColor = true;
            this.deleteBTN.Click += new System.EventHandler(this.deleteBTN_Click);
            // 
            // saveBTN
            // 
            this.saveBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this.saveBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.imagesave;
            this.saveBTN.Location = new System.Drawing.Point(60, 0);
            this.saveBTN.Name = "saveBTN";
            this.saveBTN.Size = new System.Drawing.Size(28, 22);
            this.saveBTN.TabIndex = 3;
            this.saveBTN.UseVisualStyleBackColor = true;
            this.saveBTN.Click += new System.EventHandler(this.saveBTN_Click);
            // 
            // rotateRightBTN
            // 
            this.rotateRightBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this.rotateRightBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.rotateright;
            this.rotateRightBTN.Location = new System.Drawing.Point(30, 0);
            this.rotateRightBTN.Name = "rotateRightBTN";
            this.rotateRightBTN.Size = new System.Drawing.Size(30, 22);
            this.rotateRightBTN.TabIndex = 1;
            this.rotateRightBTN.UseVisualStyleBackColor = true;
            this.rotateRightBTN.Click += new System.EventHandler(this.rotateRightBTN_Click);
            // 
            // rotateLeftBTN
            // 
            this.rotateLeftBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this.rotateLeftBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.rotateleft;
            this.rotateLeftBTN.Location = new System.Drawing.Point(0, 0);
            this.rotateLeftBTN.Name = "rotateLeftBTN";
            this.rotateLeftBTN.Size = new System.Drawing.Size(30, 22);
            this.rotateLeftBTN.TabIndex = 0;
            this.rotateLeftBTN.UseVisualStyleBackColor = true;
            this.rotateLeftBTN.Click += new System.EventHandler(this.rotateLeftBTN_Click);
            // 
            // openFD
            // 
            this.openFD.Filter = "Image Files|*.jpg;*.jpeg;*.tif;*.tiff;*.bmp;*.gif;*.png";
            this.openFD.Multiselect = true;
            // 
            // saveFD
            // 
            this.saveFD.Filter = "Jpg Images|*.jpg";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.sizeLBL);
            this.panel4.Controls.Add(this.okBTN);
            this.panel4.Controls.Add(this.cancelBTN);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 422);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(624, 22);
            this.panel4.TabIndex = 6;
            // 
            // sizeLBL
            // 
            this.sizeLBL.AutoSize = true;
            this.sizeLBL.Dock = System.Windows.Forms.DockStyle.Left;
            this.sizeLBL.Location = new System.Drawing.Point(0, 0);
            this.sizeLBL.Name = "sizeLBL";
            this.sizeLBL.Size = new System.Drawing.Size(13, 13);
            this.sizeLBL.TabIndex = 2;
            this.sizeLBL.Text = "0";
            // 
            // okBTN
            // 
            this.okBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.okBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.accept;
            this.okBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okBTN.Location = new System.Drawing.Point(424, 0);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(100, 22);
            this.okBTN.TabIndex = 1;
            this.okBTN.Text = "Ok";
            this.okBTN.UseVisualStyleBackColor = true;
            this.okBTN.Click += new System.EventHandler(this.okBTN_Click);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.cancelBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.cancel;
            this.cancelBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelBTN.Location = new System.Drawing.Point(524, 0);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(100, 22);
            this.cancelBTN.TabIndex = 0;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // PhotoManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 444);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(224, 225);
            this.Name = "PhotoManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Photo Manager";
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.editPhotoGB.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView photosLV;
        private System.Windows.Forms.Button addFolderBTN;
        private System.Windows.Forms.Button addFilesBTN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox editPhotoGB;
        private System.Windows.Forms.DateTimePicker takenAtDTP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label nameLBL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button rotateRightBTN;
        private System.Windows.Forms.Button rotateLeftBTN;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView boatsLV;
        private System.Windows.Forms.TextBox captionTB;
        private AmphibianSoftware.VisualSail.UI.Controls.UserDrawnPanel drawPNL;
        private System.Windows.Forms.Button deleteBTN;
        private System.Windows.Forms.Button saveBTN;
        private System.Windows.Forms.Label filenameLBL;
        private System.Windows.Forms.FolderBrowserDialog folderBD;
        private System.Windows.Forms.OpenFileDialog openFD;
        private System.Windows.Forms.SaveFileDialog saveFD;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label sizeLBL;
    }
}