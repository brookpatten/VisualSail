namespace AmphibianSoftware.VisualSail.UI
{
    partial class SelectRace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectRace));
            this.raceLV = new System.Windows.Forms.ListView();
            this.nameCH = new System.Windows.Forms.ColumnHeader();
            this.startCH = new System.Windows.Forms.ColumnHeader();
            this.endCH = new System.Windows.Forms.ColumnHeader();
            this.boatsCH = new System.Windows.Forms.ColumnHeader();
            this.newBTN = new System.Windows.Forms.Button();
            this.openBTN = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.photoBTN = new System.Windows.Forms.Button();
            this.playBTN = new System.Windows.Forms.Button();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // raceLV
            // 
            this.raceLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameCH,
            this.startCH,
            this.endCH,
            this.boatsCH});
            this.raceLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.raceLV.FullRowSelect = true;
            this.raceLV.HideSelection = false;
            this.raceLV.Location = new System.Drawing.Point(0, 0);
            this.raceLV.MultiSelect = false;
            this.raceLV.Name = "raceLV";
            this.raceLV.Size = new System.Drawing.Size(500, 182);
            this.raceLV.TabIndex = 0;
            this.raceLV.UseCompatibleStateImageBehavior = false;
            this.raceLV.View = System.Windows.Forms.View.Details;
            this.raceLV.SelectedIndexChanged += new System.EventHandler(this.raceLV_SelectedIndexChanged);
            // 
            // nameCH
            // 
            this.nameCH.Text = "Race";
            this.nameCH.Width = 124;
            // 
            // startCH
            // 
            this.startCH.Text = "Start";
            this.startCH.Width = 116;
            // 
            // endCH
            // 
            this.endCH.Text = "End";
            this.endCH.Width = 116;
            // 
            // boatsCH
            // 
            this.boatsCH.Text = "Boats";
            this.boatsCH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.boatsCH.Width = 41;
            // 
            // newBTN
            // 
            this.newBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.newBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.add;
            this.newBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newBTN.Location = new System.Drawing.Point(0, 0);
            this.newBTN.Name = "newBTN";
            this.newBTN.Size = new System.Drawing.Size(100, 23);
            this.newBTN.TabIndex = 2;
            this.newBTN.Text = "New";
            this.newBTN.UseVisualStyleBackColor = true;
            this.newBTN.Click += new System.EventHandler(this.newBTN_Click);
            // 
            // openBTN
            // 
            this.openBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.openBTN.Enabled = false;
            this.openBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.pencil;
            this.openBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openBTN.Location = new System.Drawing.Point(100, 0);
            this.openBTN.Name = "openBTN";
            this.openBTN.Size = new System.Drawing.Size(100, 23);
            this.openBTN.TabIndex = 3;
            this.openBTN.Text = "Edit";
            this.openBTN.UseVisualStyleBackColor = true;
            this.openBTN.Click += new System.EventHandler(this.openBTN_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.newBTN);
            this.panel1.Controls.Add(this.openBTN);
            this.panel1.Controls.Add(this.photoBTN);
            this.panel1.Controls.Add(this.playBTN);
            this.panel1.Controls.Add(this.cancelBTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 182);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 23);
            this.panel1.TabIndex = 4;
            // 
            // photoBTN
            // 
            this.photoBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.photoBTN.Enabled = false;
            this.photoBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.photos;
            this.photoBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.photoBTN.Location = new System.Drawing.Point(200, 0);
            this.photoBTN.Name = "photoBTN";
            this.photoBTN.Size = new System.Drawing.Size(100, 23);
            this.photoBTN.TabIndex = 5;
            this.photoBTN.Text = "Photos";
            this.photoBTN.UseVisualStyleBackColor = true;
            this.photoBTN.Click += new System.EventHandler(this.photoBTN_Click);
            // 
            // playBTN
            // 
            this.playBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.playBTN.Enabled = false;
            this.playBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.resultset_next;
            this.playBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.playBTN.Location = new System.Drawing.Point(300, 0);
            this.playBTN.Name = "playBTN";
            this.playBTN.Size = new System.Drawing.Size(100, 23);
            this.playBTN.TabIndex = 4;
            this.playBTN.Text = "Play";
            this.playBTN.UseVisualStyleBackColor = true;
            this.playBTN.Click += new System.EventHandler(this.playBTN_Click);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.cancelBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.cancel;
            this.cancelBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelBTN.Location = new System.Drawing.Point(400, 0);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(100, 23);
            this.cancelBTN.TabIndex = 0;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // SelectRace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 205);
            this.Controls.Add(this.raceLV);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(516, 34);
            this.Name = "SelectRace";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Race";
            this.Load += new System.EventHandler(this.SelectRace_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView raceLV;
        private System.Windows.Forms.ColumnHeader nameCH;
        private System.Windows.Forms.ColumnHeader startCH;
        private System.Windows.Forms.ColumnHeader endCH;
        private System.Windows.Forms.Button newBTN;
        private System.Windows.Forms.Button openBTN;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.Button playBTN;
        private System.Windows.Forms.ColumnHeader boatsCH;
        private System.Windows.Forms.Button photoBTN;
    }
}