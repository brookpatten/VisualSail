namespace AmphibianSoftware.VisualSail.UI
{
    partial class StatisticsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsForm));
            this.statisticsMenu = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byBoatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byCourseLegToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metricToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toHtmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedCellsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveExportFD = new System.Windows.Forms.SaveFileDialog();
            this.statsTGV = new AmphibianSoftware.VisualSail.UI.Controls.TreeGridView.TreeGridView();
            this.statisticsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statsTGV)).BeginInit();
            this.SuspendLayout();
            // 
            // statisticsMenu
            // 
            this.statisticsMenu.AllowMerge = false;
            this.statisticsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.statsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.graphToolStripMenuItem});
            this.statisticsMenu.Location = new System.Drawing.Point(0, 0);
            this.statisticsMenu.Name = "statisticsMenu";
            this.statisticsMenu.Size = new System.Drawing.Size(699, 24);
            this.statisticsMenu.TabIndex = 0;
            this.statisticsMenu.Text = "statisticsMenu";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byBoatToolStripMenuItem,
            this.byCourseLegToolStripMenuItem,
            this.unitsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // byBoatToolStripMenuItem
            // 
            this.byBoatToolStripMenuItem.Name = "byBoatToolStripMenuItem";
            this.byBoatToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.byBoatToolStripMenuItem.Text = "By Boat";
            this.byBoatToolStripMenuItem.Visible = false;
            // 
            // byCourseLegToolStripMenuItem
            // 
            this.byCourseLegToolStripMenuItem.Name = "byCourseLegToolStripMenuItem";
            this.byCourseLegToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.byCourseLegToolStripMenuItem.Text = "By Course Leg";
            this.byCourseLegToolStripMenuItem.Visible = false;
            // 
            // unitsToolStripMenuItem
            // 
            this.unitsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.standardToolStripMenuItem,
            this.metricToolStripMenuItem});
            this.unitsToolStripMenuItem.Name = "unitsToolStripMenuItem";
            this.unitsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.unitsToolStripMenuItem.Text = "Units";
            // 
            // standardToolStripMenuItem
            // 
            this.standardToolStripMenuItem.Checked = true;
            this.standardToolStripMenuItem.CheckOnClick = true;
            this.standardToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.standardToolStripMenuItem.Name = "standardToolStripMenuItem";
            this.standardToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.standardToolStripMenuItem.Text = "Standard";
            this.standardToolStripMenuItem.CheckedChanged += new System.EventHandler(this.standardToolStripMenuItem_CheckedChanged);
            // 
            // metricToolStripMenuItem
            // 
            this.metricToolStripMenuItem.CheckOnClick = true;
            this.metricToolStripMenuItem.Name = "metricToolStripMenuItem";
            this.metricToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.metricToolStripMenuItem.Text = "Metric";
            this.metricToolStripMenuItem.CheckedChanged += new System.EventHandler(this.metricToolStripMenuItem_CheckedChanged);
            // 
            // statsToolStripMenuItem
            // 
            this.statsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.statsToolStripMenuItem.Name = "statsToolStripMenuItem";
            this.statsToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.statsToolStripMenuItem.Text = "Statistics";
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.selectNoneToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toHtmlToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // toHtmlToolStripMenuItem
            // 
            this.toHtmlToolStripMenuItem.Name = "toHtmlToolStripMenuItem";
            this.toHtmlToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.toHtmlToolStripMenuItem.Text = "To Html";
            this.toHtmlToolStripMenuItem.Click += new System.EventHandler(this.toHtmlToolStripMenuItem_Click);
            // 
            // graphToolStripMenuItem
            // 
            this.graphToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedColumnsToolStripMenuItem,
            this.selectedCellsToolStripMenuItem});
            this.graphToolStripMenuItem.Name = "graphToolStripMenuItem";
            this.graphToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.graphToolStripMenuItem.Text = "Graph";
            // 
            // selectedColumnsToolStripMenuItem
            // 
            this.selectedColumnsToolStripMenuItem.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.chart_curve;
            this.selectedColumnsToolStripMenuItem.Name = "selectedColumnsToolStripMenuItem";
            this.selectedColumnsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.selectedColumnsToolStripMenuItem.Text = "Selected Columns";
            this.selectedColumnsToolStripMenuItem.Click += new System.EventHandler(this.selectedColumnsToolStripMenuItem_Click);
            // 
            // selectedCellsToolStripMenuItem
            // 
            this.selectedCellsToolStripMenuItem.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.chart_curve;
            this.selectedCellsToolStripMenuItem.Name = "selectedCellsToolStripMenuItem";
            this.selectedCellsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.selectedCellsToolStripMenuItem.Text = "Selected Cells";
            this.selectedCellsToolStripMenuItem.Click += new System.EventHandler(this.selectedCellsToolStripMenuItem_Click);
            // 
            // saveExportFD
            // 
            this.saveExportFD.Filter = "Html Files|*.html|All Files|*.*";
            // 
            // statsTGV
            // 
            this.statsTGV.AllowUserToAddRows = false;
            this.statsTGV.AllowUserToDeleteRows = false;
            this.statsTGV.AllowUserToOrderColumns = true;
            this.statsTGV.AllowUserToResizeRows = false;
            this.statsTGV.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.statsTGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsTGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.statsTGV.ImageList = null;
            this.statsTGV.Location = new System.Drawing.Point(0, 24);
            this.statsTGV.Name = "statsTGV";
            this.statsTGV.ReadOnly = true;
            this.statsTGV.RowHeadersVisible = false;
            this.statsTGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.statsTGV.Size = new System.Drawing.Size(699, 277);
            this.statsTGV.TabIndex = 2;
            // 
            // StatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 301);
            this.Controls.Add(this.statsTGV);
            this.Controls.Add(this.statisticsMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.statisticsMenu;
            this.Name = "StatisticsForm";
            this.ShowInTaskbar = false;
            this.TabText = "Statistics";
            this.Text = "Statistics";
            this.VisibleChanged += new System.EventHandler(this.StatisticsForm_VisibleChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatisticsForm_FormClosing_1);
            this.statisticsMenu.ResumeLayout(false);
            this.statisticsMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statsTGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip statisticsMenu;
        private System.Windows.Forms.ToolStripMenuItem statsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byBoatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byCourseLegToolStripMenuItem;
        private AmphibianSoftware.VisualSail.UI.Controls.TreeGridView.TreeGridView statsTGV;
        private System.Windows.Forms.ToolStripMenuItem unitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem standardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem metricToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toHtmlToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveExportFD;
        private System.Windows.Forms.ToolStripMenuItem graphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedCellsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedColumnsToolStripMenuItem;
    }
}