#if !NOLICENSE
namespace AmphibianSoftware.VisualSail.UI
{
    partial class LicenseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));
            this.onlineRB = new System.Windows.Forms.RadioButton();
            this.fileRB = new System.Windows.Forms.RadioButton();
            this.demoRB = new System.Windows.Forms.RadioButton();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.okBTN = new System.Windows.Forms.Button();
            this.demoInfoLBL = new System.Windows.Forms.Label();
            this.fileInfoLBL = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // onlineRB
            // 
            this.onlineRB.AutoSize = true;
            this.onlineRB.Location = new System.Drawing.Point(15, 69);
            this.onlineRB.Name = "onlineRB";
            this.onlineRB.Size = new System.Drawing.Size(118, 17);
            this.onlineRB.TabIndex = 1;
            this.onlineRB.TabStop = true;
            this.onlineRB.Text = "Activate online now";
            this.onlineRB.UseVisualStyleBackColor = true;
            // 
            // fileRB
            // 
            this.fileRB.AutoSize = true;
            this.fileRB.Location = new System.Drawing.Point(15, 92);
            this.fileRB.Name = "fileRB";
            this.fileRB.Size = new System.Drawing.Size(181, 17);
            this.fileRB.TabIndex = 2;
            this.fileRB.TabStop = true;
            this.fileRB.Text = "Activate via email and license file";
            this.fileRB.UseVisualStyleBackColor = true;
            // 
            // demoRB
            // 
            this.demoRB.AutoSize = true;
            this.demoRB.Enabled = false;
            this.demoRB.Location = new System.Drawing.Point(15, 46);
            this.demoRB.Name = "demoRB";
            this.demoRB.Size = new System.Drawing.Size(126, 17);
            this.demoRB.TabIndex = 3;
            this.demoRB.TabStop = true;
            this.demoRB.Text = "Continue using Demo";
            this.demoRB.UseVisualStyleBackColor = true;
            // 
            // cancelBTN
            // 
            this.cancelBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.cancel;
            this.cancelBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelBTN.Location = new System.Drawing.Point(242, 115);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(100, 22);
            this.cancelBTN.TabIndex = 4;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // okBTN
            // 
            this.okBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.accept;
            this.okBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okBTN.Location = new System.Drawing.Point(136, 115);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(100, 22);
            this.okBTN.TabIndex = 5;
            this.okBTN.Text = "Ok";
            this.okBTN.UseVisualStyleBackColor = true;
            this.okBTN.Click += new System.EventHandler(this.okBTN_Click);
            // 
            // demoInfoLBL
            // 
            this.demoInfoLBL.AutoSize = true;
            this.demoInfoLBL.Location = new System.Drawing.Point(147, 48);
            this.demoInfoLBL.Name = "demoInfoLBL";
            this.demoInfoLBL.Size = new System.Drawing.Size(70, 13);
            this.demoInfoLBL.TabIndex = 6;
            this.demoInfoLBL.Text = "demoInfoLBL";
            // 
            // fileInfoLBL
            // 
            this.fileInfoLBL.AutoSize = true;
            this.fileInfoLBL.Location = new System.Drawing.Point(194, 94);
            this.fileInfoLBL.Name = "fileInfoLBL";
            this.fileInfoLBL.Size = new System.Drawing.Size(52, 13);
            this.fileInfoLBL.TabIndex = 7;
            this.fileInfoLBL.TabStop = true;
            this.fileInfoLBL.Text = "More Info";
            this.fileInfoLBL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.fileInfoLBL_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(319, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "To Purchase and Activate this software please visit VisualSail.com";
            // 
            // LicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 144);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fileInfoLBL);
            this.Controls.Add(this.demoInfoLBL);
            this.Controls.Add(this.okBTN);
            this.Controls.Add(this.cancelBTN);
            this.Controls.Add(this.demoRB);
            this.Controls.Add(this.fileRB);
            this.Controls.Add(this.onlineRB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VisualSail Activation";
            this.Load += new System.EventHandler(this.LicenseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton onlineRB;
        private System.Windows.Forms.RadioButton fileRB;
        private System.Windows.Forms.RadioButton demoRB;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.Label demoInfoLBL;
        private System.Windows.Forms.LinkLabel fileInfoLBL;
        private System.Windows.Forms.Label label3;
    }
}
#endif