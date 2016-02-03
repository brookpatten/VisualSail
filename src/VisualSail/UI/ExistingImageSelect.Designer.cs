namespace AmphibianSoftware.VisualSail.UI
{
    partial class ExistingImageSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExistingImageSelect));
            this.panel1 = new System.Windows.Forms.Panel();
            this.okBTN = new System.Windows.Forms.Button();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.imageGV = new System.Windows.Forms.DataGridView();
            this.image = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageGV)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.okBTN);
            this.panel1.Controls.Add(this.cancelBTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 303);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 23);
            this.panel1.TabIndex = 1;
            // 
            // okBTN
            // 
            this.okBTN.Dock = System.Windows.Forms.DockStyle.Right;
			this.okBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("accept.png");
            this.okBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okBTN.Location = new System.Drawing.Point(120, 0);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(100, 23);
            this.okBTN.TabIndex = 0;
            this.okBTN.Text = "Ok";
            this.okBTN.UseVisualStyleBackColor = true;
            this.okBTN.Click += new System.EventHandler(this.okBTN_Click);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Dock = System.Windows.Forms.DockStyle.Right;
			this.cancelBTN.Image = AmphibianSoftware.VisualSail.Library.EmbeddedResourceHelper.LoadImage("cancel.png");
            this.cancelBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelBTN.Location = new System.Drawing.Point(220, 0);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(100, 23);
            this.cancelBTN.TabIndex = 1;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // imageGV
            // 
            this.imageGV.AllowUserToAddRows = false;
            this.imageGV.AllowUserToDeleteRows = false;
            this.imageGV.AllowUserToResizeColumns = false;
            this.imageGV.AllowUserToResizeRows = false;
            this.imageGV.BackgroundColor = System.Drawing.SystemColors.Control;
            this.imageGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.imageGV.ColumnHeadersVisible = false;
            this.imageGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.image});
            this.imageGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageGV.EnableHeadersVisualStyles = false;
            this.imageGV.Location = new System.Drawing.Point(0, 0);
            this.imageGV.MultiSelect = false;
            this.imageGV.Name = "imageGV";
            this.imageGV.ReadOnly = true;
            this.imageGV.RowHeadersVisible = false;
            this.imageGV.RowTemplate.Height = 300;
            this.imageGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.imageGV.Size = new System.Drawing.Size(320, 303);
            this.imageGV.TabIndex = 3;
            this.imageGV.SelectionChanged += new System.EventHandler(this.imageGV_SelectionChanged);
            // 
            // image
            // 
            this.image.HeaderText = "";
            this.image.Name = "image";
            this.image.ReadOnly = true;
            this.image.Width = 300;
            // 
            // ExistingImageSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 326);
            this.ControlBox = false;
            this.Controls.Add(this.imageGV);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExistingImageSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Browse Local Images";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.DataGridView imageGV;
        private System.Windows.Forms.DataGridViewImageColumn image;
    }
}