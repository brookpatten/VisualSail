namespace AmphibianSoftware.VisualSail.UI
{
    partial class VideoUploadDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoUploadDialog));
            this.uploadBTN = new System.Windows.Forms.Button();
            this.uploadPB = new System.Windows.Forms.ProgressBar();
            this.signinGB = new System.Windows.Forms.GroupBox();
            this.passwordTB = new System.Windows.Forms.TextBox();
            this.userTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.detailsGB = new System.Windows.Forms.GroupBox();
            this.locationTB = new System.Windows.Forms.TextBox();
            this.descriptionTB = new System.Windows.Forms.TextBox();
            this.keywordTB = new System.Windows.Forms.TextBox();
            this.titleTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.privateCB = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusLBL = new System.Windows.Forms.Label();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.signinGB.SuspendLayout();
            this.detailsGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // uploadBTN
            // 
            this.uploadBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.add;
            this.uploadBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uploadBTN.Location = new System.Drawing.Point(93, 295);
            this.uploadBTN.Name = "uploadBTN";
            this.uploadBTN.Size = new System.Drawing.Size(100, 23);
            this.uploadBTN.TabIndex = 0;
            this.uploadBTN.Text = "Upload";
            this.uploadBTN.UseVisualStyleBackColor = true;
            this.uploadBTN.Click += new System.EventHandler(this.uploadBTN_Click);
            // 
            // uploadPB
            // 
            this.uploadPB.Location = new System.Drawing.Point(73, 266);
            this.uploadPB.Name = "uploadPB";
            this.uploadPB.Size = new System.Drawing.Size(226, 23);
            this.uploadPB.TabIndex = 1;
            // 
            // signinGB
            // 
            this.signinGB.Controls.Add(this.passwordTB);
            this.signinGB.Controls.Add(this.userTB);
            this.signinGB.Controls.Add(this.label2);
            this.signinGB.Controls.Add(this.label1);
            this.signinGB.Location = new System.Drawing.Point(3, 3);
            this.signinGB.Name = "signinGB";
            this.signinGB.Size = new System.Drawing.Size(296, 66);
            this.signinGB.TabIndex = 2;
            this.signinGB.TabStop = false;
            this.signinGB.Text = "YouTube Sign In";
            // 
            // passwordTB
            // 
            this.passwordTB.Location = new System.Drawing.Point(70, 36);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.Size = new System.Drawing.Size(220, 20);
            this.passwordTB.TabIndex = 3;
            this.passwordTB.UseSystemPasswordChar = true;
            // 
            // userTB
            // 
            this.userTB.Location = new System.Drawing.Point(70, 13);
            this.userTB.Name = "userTB";
            this.userTB.Size = new System.Drawing.Size(220, 20);
            this.userTB.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // detailsGB
            // 
            this.detailsGB.Controls.Add(this.locationTB);
            this.detailsGB.Controls.Add(this.descriptionTB);
            this.detailsGB.Controls.Add(this.keywordTB);
            this.detailsGB.Controls.Add(this.titleTB);
            this.detailsGB.Controls.Add(this.label6);
            this.detailsGB.Controls.Add(this.privateCB);
            this.detailsGB.Controls.Add(this.label5);
            this.detailsGB.Controls.Add(this.label4);
            this.detailsGB.Controls.Add(this.label3);
            this.detailsGB.Location = new System.Drawing.Point(3, 75);
            this.detailsGB.Name = "detailsGB";
            this.detailsGB.Size = new System.Drawing.Size(296, 191);
            this.detailsGB.TabIndex = 3;
            this.detailsGB.TabStop = false;
            this.detailsGB.Text = "Video Details";
            // 
            // locationTB
            // 
            this.locationTB.Location = new System.Drawing.Point(70, 165);
            this.locationTB.Name = "locationTB";
            this.locationTB.Size = new System.Drawing.Size(151, 20);
            this.locationTB.TabIndex = 8;
            // 
            // descriptionTB
            // 
            this.descriptionTB.Location = new System.Drawing.Point(70, 61);
            this.descriptionTB.Multiline = true;
            this.descriptionTB.Name = "descriptionTB";
            this.descriptionTB.Size = new System.Drawing.Size(220, 101);
            this.descriptionTB.TabIndex = 7;
            // 
            // keywordTB
            // 
            this.keywordTB.Location = new System.Drawing.Point(70, 38);
            this.keywordTB.Name = "keywordTB";
            this.keywordTB.Size = new System.Drawing.Size(220, 20);
            this.keywordTB.TabIndex = 6;
            // 
            // titleTB
            // 
            this.titleTB.Location = new System.Drawing.Point(70, 17);
            this.titleTB.Name = "titleTB";
            this.titleTB.Size = new System.Drawing.Size(220, 20);
            this.titleTB.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Location:";
            // 
            // privateCB
            // 
            this.privateCB.AutoSize = true;
            this.privateCB.Location = new System.Drawing.Point(231, 168);
            this.privateCB.Name = "privateCB";
            this.privateCB.Size = new System.Drawing.Size(59, 17);
            this.privateCB.TabIndex = 3;
            this.privateCB.Text = "Private";
            this.privateCB.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Keywords:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Description:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Title:";
            // 
            // statusLBL
            // 
            this.statusLBL.AutoSize = true;
            this.statusLBL.Location = new System.Drawing.Point(13, 271);
            this.statusLBL.Name = "statusLBL";
            this.statusLBL.Size = new System.Drawing.Size(38, 13);
            this.statusLBL.TabIndex = 4;
            this.statusLBL.Text = "Ready";
            // 
            // cancelBTN
            // 
            this.cancelBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.cancel;
            this.cancelBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelBTN.Location = new System.Drawing.Point(199, 295);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(100, 23);
            this.cancelBTN.TabIndex = 5;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // VideoUploadDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 320);
            this.Controls.Add(this.cancelBTN);
            this.Controls.Add(this.statusLBL);
            this.Controls.Add(this.detailsGB);
            this.Controls.Add(this.signinGB);
            this.Controls.Add(this.uploadPB);
            this.Controls.Add(this.uploadBTN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VideoUploadDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "YouTube Video Upload";
            this.signinGB.ResumeLayout(false);
            this.signinGB.PerformLayout();
            this.detailsGB.ResumeLayout(false);
            this.detailsGB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uploadBTN;
        private System.Windows.Forms.ProgressBar uploadPB;
        private System.Windows.Forms.GroupBox signinGB;
        private System.Windows.Forms.TextBox passwordTB;
        private System.Windows.Forms.TextBox userTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox detailsGB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox privateCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox locationTB;
        private System.Windows.Forms.TextBox descriptionTB;
        private System.Windows.Forms.TextBox keywordTB;
        private System.Windows.Forms.TextBox titleTB;
        private System.Windows.Forms.Label statusLBL;
        private System.Windows.Forms.Button cancelBTN;
    }
}