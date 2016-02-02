namespace AmphibianSoftware.VisualSail.UI
{
    partial class Dashboard
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
            this.dashTV = new System.Windows.Forms.TreeView();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sensorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileLBL = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.recordBTN = new System.Windows.Forms.Button();
            this.saveFD = new System.Windows.Forms.SaveFileDialog();
            this.mainMenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dashTV
            // 
            this.dashTV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashTV.Location = new System.Drawing.Point(0, 24);
            this.dashTV.Name = "dashTV";
            this.dashTV.Size = new System.Drawing.Size(330, 366);
            this.dashTV.TabIndex = 0;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(330, 24);
            this.mainMenuStrip.TabIndex = 1;
            this.mainMenuStrip.Text = "mainMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outputToFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // outputToFileToolStripMenuItem
            // 
            this.outputToFileToolStripMenuItem.Name = "outputToFileToolStripMenuItem";
            this.outputToFileToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.outputToFileToolStripMenuItem.Text = "Output To File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sensorsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.sensorsToolStripMenuItem_Click);
            // 
            // sensorsToolStripMenuItem
            // 
            this.sensorsToolStripMenuItem.Name = "sensorsToolStripMenuItem";
            this.sensorsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.sensorsToolStripMenuItem.Text = "Sensors";
            // 
            // fileLBL
            // 
            this.fileLBL.AutoSize = true;
            this.fileLBL.Location = new System.Drawing.Point(78, 9);
            this.fileLBL.Name = "fileLBL";
            this.fileLBL.Size = new System.Drawing.Size(20, 13);
            this.fileLBL.TabIndex = 2;
            this.fileLBL.Text = "file";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.recordBTN);
            this.panel1.Controls.Add(this.fileLBL);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 359);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 31);
            this.panel1.TabIndex = 3;
            // 
            // recordBTN
            // 
            this.recordBTN.Location = new System.Drawing.Point(12, 4);
            this.recordBTN.Name = "recordBTN";
            this.recordBTN.Size = new System.Drawing.Size(60, 23);
            this.recordBTN.TabIndex = 3;
            this.recordBTN.Text = "Record";
            this.recordBTN.UseVisualStyleBackColor = true;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 390);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dashTV);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dashboard_FormClosing);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView dashTV;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sensorsToolStripMenuItem;
        private System.Windows.Forms.Label fileLBL;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button recordBTN;
        private System.Windows.Forms.SaveFileDialog saveFD;
    }
}