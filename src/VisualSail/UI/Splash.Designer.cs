namespace AmphibianSoftware.VisualSail.UI
{
    partial class Splash
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
            this.versionLBL = new System.Windows.Forms.Label();
            this.loadPB = new System.Windows.Forms.ProgressBar();
            this.licenseLBL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // versionLBL
            // 
            this.versionLBL.AutoSize = true;
            this.versionLBL.BackColor = System.Drawing.Color.Transparent;
            this.versionLBL.Location = new System.Drawing.Point(325, 114);
            this.versionLBL.Name = "versionLBL";
            this.versionLBL.Size = new System.Drawing.Size(22, 13);
            this.versionLBL.TabIndex = 0;
            this.versionLBL.Text = "1.0";
            this.versionLBL.UseWaitCursor = true;
            // 
            // loadPB
            // 
            this.loadPB.BackColor = System.Drawing.Color.Black;
            this.loadPB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.loadPB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.loadPB.Location = new System.Drawing.Point(0, 475);
            this.loadPB.Name = "loadPB";
            this.loadPB.Size = new System.Drawing.Size(634, 10);
            this.loadPB.TabIndex = 1;
            this.loadPB.UseWaitCursor = true;
            this.loadPB.Value = 50;
            // 
            // licenseLBL
            // 
            this.licenseLBL.AutoSize = true;
            this.licenseLBL.BackColor = System.Drawing.Color.Transparent;
            this.licenseLBL.Location = new System.Drawing.Point(375, 114);
            this.licenseLBL.Name = "licenseLBL";
            this.licenseLBL.Size = new System.Drawing.Size(44, 13);
            this.licenseLBL.TabIndex = 2;
            this.licenseLBL.Text = "License";
            this.licenseLBL.UseWaitCursor = true;
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AmphibianSoftware.VisualSail.Properties.Resources.splash;
            this.ClientSize = new System.Drawing.Size(634, 485);
            this.ControlBox = false;
            this.Controls.Add(this.licenseLBL);
            this.Controls.Add(this.loadPB);
            this.Controls.Add(this.versionLBL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.Splash_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label versionLBL;
        private System.Windows.Forms.ProgressBar loadPB;
        private System.Windows.Forms.Label licenseLBL;
    }
}