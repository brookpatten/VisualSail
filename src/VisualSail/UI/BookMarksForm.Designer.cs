namespace AmphibianSoftware.VisualSail.UI
{
    partial class BookMarksForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookMarksForm));
            this.bookMarksLV = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gotoBTN = new System.Windows.Forms.Button();
            this.addBTN = new System.Windows.Forms.Button();
            this.removeBTN = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bookMarksLV
            // 
            this.bookMarksLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.bookMarksLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bookMarksLV.FullRowSelect = true;
            this.bookMarksLV.HideSelection = false;
            this.bookMarksLV.Location = new System.Drawing.Point(0, 0);
            this.bookMarksLV.Name = "bookMarksLV";
            this.bookMarksLV.Size = new System.Drawing.Size(200, 401);
            this.bookMarksLV.TabIndex = 0;
            this.bookMarksLV.UseCompatibleStateImageBehavior = false;
            this.bookMarksLV.View = System.Windows.Forms.View.Details;
            this.bookMarksLV.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.bookMarksLV_MouseDoubleClick);
            this.bookMarksLV.SelectedIndexChanged += new System.EventHandler(this.bookMarksLV_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Time";
            this.columnHeader1.Width = 91;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 113;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gotoBTN);
            this.panel1.Controls.Add(this.addBTN);
            this.panel1.Controls.Add(this.removeBTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 378);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 23);
            this.panel1.TabIndex = 1;
            // 
            // gotoBTN
            // 
            this.gotoBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this.gotoBTN.Enabled = false;
            this.gotoBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.resultset_next;
            this.gotoBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.gotoBTN.Location = new System.Drawing.Point(0, 0);
            this.gotoBTN.Name = "gotoBTN";
            this.gotoBTN.Size = new System.Drawing.Size(100, 23);
            this.gotoBTN.TabIndex = 2;
            this.gotoBTN.Text = "Go To";
            this.gotoBTN.UseVisualStyleBackColor = true;
            this.gotoBTN.Click += new System.EventHandler(this.gotoBTN_Click);
            // 
            // addBTN
            // 
            this.addBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.addBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.add;
            this.addBTN.Location = new System.Drawing.Point(154, 0);
            this.addBTN.Name = "addBTN";
            this.addBTN.Size = new System.Drawing.Size(23, 23);
            this.addBTN.TabIndex = 0;
            this.addBTN.UseVisualStyleBackColor = true;
            this.addBTN.Click += new System.EventHandler(this.addBTN_Click);
            // 
            // removeBTN
            // 
            this.removeBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.removeBTN.Enabled = false;
            this.removeBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.delete;
            this.removeBTN.Location = new System.Drawing.Point(177, 0);
            this.removeBTN.Name = "removeBTN";
            this.removeBTN.Size = new System.Drawing.Size(23, 23);
            this.removeBTN.TabIndex = 1;
            this.removeBTN.UseVisualStyleBackColor = true;
            this.removeBTN.Click += new System.EventHandler(this.removeBTN_Click);
            // 
            // BookMarksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 401);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bookMarksLV);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BookMarksForm";
            this.TabText = "Bookmarks";
            this.Text = "Bookmarks";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView bookMarksLV;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button removeBTN;
        private System.Windows.Forms.Button addBTN;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button gotoBTN;
    }
}