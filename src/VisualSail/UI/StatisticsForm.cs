using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using AmphibianSoftware.VisualSail.UI.Controls.TreeGridView;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Data.Statistics;

using WeifenLuo.WinFormsUI.Docking;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class StatisticsForm : DockContent
    {
        bool _columnsConfigured = false;
        DataSet _cache;
        private Replay _replay;
        
        public StatisticsForm(Replay replay)
        {
            _replay = replay;
            InitializeComponent();
            
        }

        private Replay Replay
        {
            get
            {
                return _replay;
            }
            set
            {
                _replay = value;
            }
        }
        public void UpdateStatistics(bool force)
        {
            if (this.Visible||force)
            {
                if (_cache != null)
                {
                    lock (_cache)
                    {
                        _cache = this.Replay.GetFullStatistics(SelectedUnitType);
                    }
                }
                else
                {
                    _cache = this.Replay.GetFullStatistics(SelectedUnitType);
                }
                this.UpdateListView();
                //statsTGV.BeginInvoke(new AmphibianSoftware.VisualSail.Library.Notify(this.UpdateListView));
                //statsTGV.Invoke(new AmphibianSoftware.VisualSail.Library.Notify(this.UpdateListView));
                //UpdateListView();
            }
        }
        public StatisticUnitType SelectedUnitType
        {
            get
            {
                StatisticUnitType type = StatisticUnitType.metric;
                if (standardToolStripMenuItem.Checked)
                {
                    type = StatisticUnitType.standard;
                }
                return type;
            }
        }
        public void CreateDefaultGraphs()
        {
            List<SelectedStatisticCell> boats = new List<SelectedStatisticCell>();
            for(int i=0;i<_replay.Boats.Count;i++)
            {
                SelectedStatisticCell ssc = new SelectedStatisticCell();
                ssc.BoatIndex = i;
                boats.Add(ssc);
            }
            GraphForm gf = new GraphForm(_replay, "Speed", boats, SelectedUnitType);
            gf.Show(this.Pane, DockAlignment.Right, 0.5);
        }
        private void UpdateListView()
        {
            //try
            //{
                lock (_cache)
                {
                    lock (statsTGV)
                    {
                        DataView infoDV = new DataView(_cache.Tables["StatisticInfo"]);

                        if (!_columnsConfigured)
                        {
                            while (statsToolStripMenuItem.DropDownItems.Count > 2)
                            {
                                statsToolStripMenuItem.DropDownItems.RemoveAt(0);
                            }
                            statsTGV.Columns.Clear();
                            
                            foreach (DataColumn dc in _cache.Tables["Statistics"].Columns)
                            {
                                if (dc.ColumnName == "boat_id")
                                {
                                    TreeGridColumn tgc = new TreeGridColumn();
                                    tgc.HeaderText = "Name";
                                    tgc.Name = "Name";
                                    tgc.Frozen = true;
                                    tgc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                    statsTGV.Columns.Add(tgc);

                                    DataGridViewTextBoxColumn color = new DataGridViewTextBoxColumn();
                                    color.HeaderText = "Color";
                                    color.Name = "Color";
                                    color.Frozen = true;
                                    statsTGV.Columns.Add(color);
                                }
                                else if (dc.ColumnName == "leg_index" || dc.ColumnName=="tack_index")
                                {
                                }
                                else
                                {
                                    bool selectedByDefault = _replay.Boats[0].TotalStatistics.IsStatisticSelectedByDefault(dc.ColumnName);

                                    if (selectedByDefault)
                                    {
                                        DataGridViewTextBoxColumn dgvtbc = new DataGridViewTextBoxColumn();
                                        dgvtbc.Name = dc.ColumnName;

                                        infoDV.RowFilter = "name='" + dc.ColumnName + "'";
                                        string type = (string)infoDV[0]["type"];
                                        string unit = (string)infoDV[0]["unit"];
                                        string description = (string)infoDV[0]["description"];
                                        if (unit != "other")
                                        {
                                            dgvtbc.HeaderText = dc.ColumnName + " (" + unit + ")";
                                            dgvtbc.ToolTipText = "(" + unit + ") " + description;
                                        }

                                        statsTGV.Columns.Add(dgvtbc);
                                    }

                                    ToolStripMenuItem item = new ToolStripMenuItem();
                                    item.Text = dc.ColumnName;
                                    item.Checked = selectedByDefault;
                                    item.CheckOnClick = true;
                                    item.CheckedChanged += new EventHandler(selectStatisticToolStripMenuItem_Click);
                                    statsToolStripMenuItem.DropDownItems.Insert(statsToolStripMenuItem.DropDownItems.Count - 2, item);
                                }


                            }
                            statsTGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
                            _columnsConfigured = true;
                        }
                        else
                        {
                            for (int i = 0; i < statsToolStripMenuItem.DropDownItems.Count; i++)
                            {
                                if (((ToolStripMenuItem)statsToolStripMenuItem.DropDownItems[i]).Checked)
                                {
                                    if (statsToolStripMenuItem.DropDownItems[i].Text != "boat_id" && statsToolStripMenuItem.DropDownItems[i].Text != "leg_index" && !statsTGV.Columns.Contains(statsToolStripMenuItem.DropDownItems[i].Text))
                                    {
                                        DataGridViewTextBoxColumn dgvtbc = new DataGridViewTextBoxColumn();
                                        dgvtbc.Name = statsToolStripMenuItem.DropDownItems[i].Text;

                                        infoDV.RowFilter = "name='" + statsToolStripMenuItem.DropDownItems[i].Text + "'";
                                        string type = (string)infoDV[0]["type"];
                                        string unit = (string)infoDV[0]["unit"];
                                        string description = (string)infoDV[0]["description"];
                                        if (unit != "other")
                                        {
                                            dgvtbc.HeaderText = statsToolStripMenuItem.DropDownItems[i].Text + " (" + unit + ")";
                                            dgvtbc.ToolTipText = "(" + unit + ") " + description;
                                        }

                                        statsTGV.Columns.Add(dgvtbc);
                                    }
                                }
                                else
                                {
                                    if (statsTGV.Columns.Contains(statsToolStripMenuItem.DropDownItems[i].Text))
                                    {
                                        statsTGV.Columns.Remove(statsTGV.Columns[statsToolStripMenuItem.DropDownItems[i].Text]);
                                    }
                                }
                            }
                        }


                        //statsTGV.Nodes.Clear();
                        int boatIndex = 0;
                        TreeGridNode boat=null;
                        int legIndex = 0;
                        TreeGridNode leg=null;
                        int tackIndex = 0;
                        TreeGridNode tack=null;

                        foreach (DataRow dr in _cache.Tables["Boats"].Rows)
                        {
                            string boatName = dr["name"].ToString() + " " + dr["number"].ToString();
                            Color boatColor = (Color)dr["color"];
                            Color boatColorLight = ColorHelper.Darken(boatColor);//Color.FromArgb(boatColor.R / 2, boatColor.G / 2, boatColor.B / 2);
                            DataRow stats = _cache.Tables["Statistics"].Select("boat_id=" + dr["id"] + " and leg_index is null and tack_index is null")[0];

                            if (boatIndex < statsTGV.Nodes.Count)
                            {
                                for (int i = 0; i < statsTGV.Columns.Count; i++)
                                {
                                    if (statsTGV.Columns[i].Name == "Name")
                                    {
                                        statsTGV.Nodes[boatIndex].Cells[i].Value = boatName;
                                    }
                                    else if (statsTGV.Columns[i].Name == "Color")
                                    {
                                        statsTGV.Nodes[boatIndex].Cells[i].Style.BackColor = boatColor;
                                        statsTGV.Nodes[boatIndex].Cells[i].Style.SelectionBackColor = boatColorLight;
                                    }
                                    else
                                    {
                                        //we might need to add cells
                                        if (i < statsTGV.Nodes[boatIndex].Cells.Count)
                                        {
                                            statsTGV.Nodes[boatIndex].Cells[statsTGV.Columns[i].Name].Value = FormatValue(stats[statsTGV.Columns[i].Name]);
                                        }
                                        else
                                        {
                                            //TreeGridCell c = new TreeGridCell();
                                            DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                                            c.Value = FormatValue(stats[statsTGV.Columns[i].Name]);
                                            statsTGV.Nodes[boatIndex].Cells.Add(c);
                                        }
                                    }
                                }
                                //remove any excess nodes
                                while (boatIndex >= statsTGV.Nodes.Count)
                                {
                                    statsTGV.Nodes.RemoveAt(statsTGV.Nodes.Count - 1);
                                }
                                //remove any excess cells
                                while (statsTGV.Nodes[boatIndex].Cells.Count > statsTGV.Columns.Count)
                                {
                                    statsTGV.Nodes[boatIndex].Cells.RemoveAt(statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells.Count - 1);
                                }
                                boat = statsTGV.Nodes[boatIndex];
                            }
                            else
                            {
                                List<object> values = new List<object>();
                                for (int i = 0; i < statsTGV.Columns.Count; i++)
                                {
                                    if (statsTGV.Columns[i].Name == "Name")
                                    {
                                        values.Add(boatName);
                                    }
                                    else if (statsTGV.Columns[i].Name == "Color")
                                    {
                                        values.Add("");
                                    }
                                    else
                                    {
                                        values.Add(FormatValue(stats[statsTGV.Columns[i].Name]));
                                    }
                                }
                                boat = statsTGV.Nodes.Add(values.ToArray());
                            }
                            legIndex = 0;

                            foreach (DataRow ldr in _cache.Tables["Legs"].Rows)
                            {
                                string legName = ldr["description"].ToString();
                                if (ldr["index"].ToString() != "")
                                {
                                    DataRow[] drc = _cache.Tables["Statistics"].Select("boat_id=" + dr["id"] + " and leg_index=" + ldr["index"]+" and tack_index is null");
                                    if (drc.Length > 0)
                                    {
                                        DataRow legstats = drc[0];
                                        if (legIndex < boat.Nodes.Count)
                                        {
                                            for (int i = 0; i < statsTGV.Columns.Count; i++)
                                            {
                                                if (statsTGV.Columns[i].Name == "Name")
                                                {
                                                    statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells[i].Value = legName;
                                                }
                                                else if (statsTGV.Columns[i].Name == "Color")
                                                {
                                                    statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells[i].Style.BackColor = boatColor;
                                                    statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells[i].Style.SelectionBackColor = boatColorLight;
                                                }
                                                else
                                                {
                                                    //we might need to add cells
                                                    if (i<statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells.Count)
                                                    {
                                                        statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells[i].Value = FormatValue(legstats[statsTGV.Columns[i].Name]);
                                                    }
                                                    else
                                                    {

                                                        //TreeGridCell c=new TreeGridCell();
                                                        DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                                                        c.Value = FormatValue(legstats[statsTGV.Columns[i].Name]);
                                                        statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells.Add(c);
                                                    }
                                                }
                                            }

                                            //remove any excess nodes
                                            while (legIndex >= boat.Nodes.Count)
                                            {
                                                boat.Nodes.RemoveAt(boat.Nodes.Count - 1);
                                            }
                                            //remove any excess cells
                                            while (statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells.Count > statsTGV.Columns.Count)
                                            {
                                                statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells.RemoveAt(statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells.Count - 1);
                                            }
                                            leg = statsTGV.Nodes[boatIndex].Nodes[legIndex];
                                        }
                                        else
                                        {
                                            List<object> legvalues = new List<object>();
                                            for (int i = 0; i < statsTGV.Columns.Count; i++)
                                            {
                                                if (statsTGV.Columns[i].Name == "Name")
                                                {
                                                    legvalues.Add(legName);
                                                }
                                                else if (statsTGV.Columns[i].Name == "Color")
                                                {
                                                    legvalues.Add("");
                                                }
                                                else
                                                {
                                                    legvalues.Add(FormatValue(legstats[statsTGV.Columns[i].Name]));
                                                }
                                            }
                                            leg = boat.Nodes.Add(legvalues.ToArray());
                                        }
                                    }
                                    try
                                    {
                                        DataRow[] tackRows = _cache.Tables["Tacks"].Select(string.Format("boat_id={0} and leg_index={1}", dr["id"], ldr["index"]), "tack_index asc");
                                        tackIndex = 0;
                                        foreach (DataRow tdr in tackRows)
                                        {
                                            string tackName = tdr["Name"].ToString();
                                            DataRow[] trc = _cache.Tables["Statistics"].Select("boat_id=" + dr["id"] + " and leg_index=" + ldr["index"] + " and tack_index=" + tdr["tack_index"]);
                                            if (trc.Length > 0)
                                            {
                                                DataRow tackstats = trc[0];
                                                if (tackIndex < leg.Nodes.Count)
                                                {
                                                    for (int i = 0; i < statsTGV.Columns.Count; i++)
                                                    {
                                                        if (statsTGV.Columns[i].Name == "Name")
                                                        {
                                                            statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells[i].Value = tackName;
                                                        }
                                                        else if (statsTGV.Columns[i].Name == "Color")
                                                        {
                                                            statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells[i].Style.BackColor = boatColor;
                                                            statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells[i].Style.SelectionBackColor = boatColorLight;
                                                        }
                                                        else
                                                        {
                                                            //we might need to add cells
                                                            if (i < statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells.Count)
                                                            {
                                                                statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells[i].Value = FormatValue(tackstats[statsTGV.Columns[i].Name]);
                                                            }
                                                            else
                                                            {
                                                                //TreeGridCell c=new TreeGridCell();
                                                                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                                                                c.Value = FormatValue(tackstats[statsTGV.Columns[i].Name]);
                                                                statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells.Add(c);
                                                            }
                                                        }
                                                    }

                                                    //remove any excess nodes
                                                    while (tackIndex >= leg.Nodes.Count)
                                                    {
                                                        leg.Nodes.RemoveAt(leg.Nodes.Count - 1);
                                                    }
                                                    //remove any excess cells
                                                    while (statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells.Count > statsTGV.Columns.Count)
                                                    {
                                                        statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells.RemoveAt(statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells.Count - 1);
                                                    }
                                                    tack = statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex];
                                                }
                                                else
                                                {
                                                    List<object> tackvalues = new List<object>();
                                                    for (int i = 0; i < statsTGV.Columns.Count; i++)
                                                    {
                                                        if (statsTGV.Columns[i].Name == "Name")
                                                        {
                                                            tackvalues.Add(tackName);
                                                        }
                                                        else if (statsTGV.Columns[i].Name == "Color")
                                                        {
                                                            tackvalues.Add("");
                                                        }
                                                        else
                                                        {
                                                            tackvalues.Add(FormatValue(tackstats[statsTGV.Columns[i].Name]));
                                                        }
                                                    }
                                                    tack = leg.Nodes.Add(tackvalues.ToArray());
                                                }
                                            }
                                            tackIndex++;
                                        }
                                    }
                                    catch (Exception)
                                    {
#warning catch with no implementation.... why the fuck did i do this? it must be for a reason
                                    }
                                    legIndex++;
                                }
                            }
                            boatIndex++;
                        }
                    }
                }
            //}
            //catch (Exception e)
            //{
            //    #warning remove this try catch block for production
            //    //this is just here for a breakpoint, delete it later
            //    throw e;
            //}
            //statsTGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
        }

        public void Reset()
        {
            _cache = null;
            Clear();
        }

        private void Clear()
        {
            statsTGV.Nodes.Clear();
            statsTGV.Columns.Clear();
            _columnsConfigured = false;
        }

        private string FormatValue(object val)
        {
            if (val.GetType() == typeof(DateTime))
            {
                return ((DateTime)val).ToString("HH:mm:ss");
            }
            else if (val.GetType() == typeof(TimeSpan))
            {
                return ((TimeSpan)val).ToString();
            }
            else
            {
                return string.Format("{0:0.##}", val);
            }
        }

        private void StatisticsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
        }

        private void resizeColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statsTGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
        }

        private void selectStatisticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statsTGV.BeginInvoke(new AmphibianSoftware.VisualSail.Library.Notify(this.UpdateListView));
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllStatistics();
        }

        public void SelectAllStatistics()
        {
            for (int i = 0; i < statsToolStripMenuItem.DropDownItems.Count - 2; i++)
            {
                ((ToolStripMenuItem)statsToolStripMenuItem.DropDownItems[i]).Checked = true;
            }
            UpdateStatistics(true);
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < statsToolStripMenuItem.DropDownItems.Count - 2; i++)
            {
                ((ToolStripMenuItem)statsToolStripMenuItem.DropDownItems[i]).Checked = false;
            }
        }
        
        private void standardToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            metricToolStripMenuItem.Checked = !standardToolStripMenuItem.Checked;
            Clear();
            UpdateStatistics(true);
        }

        private void metricToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            standardToolStripMenuItem.Checked = !metricToolStripMenuItem.Checked;
            Clear();
            UpdateStatistics(true);
        }

        private void StatisticsForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void toHtmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveExportFD.ShowDialog() == DialogResult.OK)
            {
                string path = saveExportFD.FileName;
                ExportStatistics(path);
            }
        }

        public void ExportStatistics(string path)
        {
            StreamWriter write = new StreamWriter(path);
            write.Write(statsTGV.ExportToHtml());
            write.Flush();
            write.Close();
        }

        public string ExportStatistics()
        {
            return statsTGV.ExportToHtml();
        }

        private void StatisticsForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                UpdateStatistics(true);
            }
        }

        private void selectedCellsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool graphedAnything = false;
            //loop through each column (stat), starting with the 3rd column
            //so that we skip "name" and "color", since those can't really be graphed
            for (int columnIndex = 2; columnIndex < statsTGV.Columns.Count; columnIndex++)
            {
                List<SelectedStatisticCell> boats = new List<SelectedStatisticCell>();
                List<SelectedStatisticCell> legs = new List<SelectedStatisticCell>();
                List<SelectedStatisticCell> tacks = new List<SelectedStatisticCell>();
                string statName = statsTGV.Columns[columnIndex].Name;

                //loop through the top level nodes AKA "boat nodes"
                for (int boatIndex = 0; boatIndex < statsTGV.Nodes.Count; boatIndex++)
                {
                    //look for "boat level" cells that are selected
                    if (statsTGV.Nodes[boatIndex].Cells[columnIndex].Selected)
                    {
                        if (boatIndex < _replay.Boats.Count)
                        {
                            SelectedStatisticCell ssc = new SelectedStatisticCell();
                            ssc.BoatIndex = boatIndex;
                            ssc.Statistic = statName;
                            boats.Add(ssc);
                        }
                    }

                    //loop through the 2nd level nodes AKA "leg nodes"
                    for (int legIndex = 0; legIndex < statsTGV.Nodes[boatIndex].Nodes.Count; legIndex++)
                    {
                        if (statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells[columnIndex].Selected)
                        {
                            if (boatIndex < _replay.Boats.Count && legIndex<_replay.Race.Course.Route.Count)
                            {
                                SelectedStatisticCell ssc = new SelectedStatisticCell();
                                ssc.BoatIndex = boatIndex;
                                ssc.LegIndex = legIndex;
                                ssc.Statistic = statName;
                                legs.Add(ssc);
                            }
                        }

                        //loop through the 3rd level nodes AKA "tack nodes"
                        for (int tackIndex = 0; tackIndex < statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes.Count; tackIndex++)
                        {
                            if (statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells[columnIndex].Selected)
                            {
                                if (boatIndex < _replay.Boats.Count && legIndex < _replay.Race.Course.Route.Count)
                                {
                                    SelectedStatisticCell ssc = new SelectedStatisticCell();
                                    ssc.BoatIndex = boatIndex;
                                    ssc.LegIndex = legIndex;
                                    int realTackIndex = (from t in this.Replay.Boats[boatIndex].Tacks
                                                         where t.LegIndex == legIndex && t.IndexOnLeg == tackIndex
                                                         select t.Index).Single();
                                    ssc.TackIndex = realTackIndex;
                                    ssc.Statistic = statName;
                                    tacks.Add(ssc);
                                }
                            }
                        }
                    }
                }

                //open graphs for each group type
                if (boats.Count > 0)
                {
                    GraphForm gp = new GraphForm(Replay, statName, boats, SelectedUnitType);
                    gp.Show(this.DockPanel, UIHelper.FindCenteredPosition(this.DockPanel, this));
                    graphedAnything = true;
                }
                if (legs.Count > 0)
                {
                    GraphForm gp = new GraphForm(Replay, statName, legs, SelectedUnitType);
                    gp.Show(this.DockPanel, UIHelper.FindCenteredPosition(this.DockPanel, this));
                    graphedAnything = true;
                }
                if (tacks.Count > 0)
                {
                    GraphForm gp = new GraphForm(Replay, statName, tacks, SelectedUnitType);
                    gp.Show(this.DockPanel, UIHelper.FindCenteredPosition(this.DockPanel, this));
                    graphedAnything = true;
                }
            }
            if (!graphedAnything)
            {
                MessageBox.Show("Select one or more cells first.");
            }
        }

        private void selectedColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool graphedAnything = false;
            //loop through each column (stat), starting with the 3rd column
            //so that we skip "name" and "color", since those can't really be graphed
            for (int columnIndex = 2; columnIndex < statsTGV.Columns.Count; columnIndex++)
            {
                List<SelectedStatisticCell> boats = new List<SelectedStatisticCell>();
                List<SelectedStatisticCell> legs = new List<SelectedStatisticCell>();
                List<SelectedStatisticCell> tacks = new List<SelectedStatisticCell>();
                string statName = statsTGV.Columns[columnIndex].Name;

                //loop through the top level nodes AKA "boat nodes"
                for (int boatIndex = 0; boatIndex < statsTGV.Nodes.Count; boatIndex++)
                {
                    //look for "boat level" cells that are selected
                    if (statsTGV.Nodes[boatIndex].Cells[columnIndex].Selected)
                    {
                        if (boats.Count == 0)
                        {
                            for (int bi = 0; bi < _replay.Boats.Count; bi++)
                            {
                                SelectedStatisticCell ssc = new SelectedStatisticCell();
                                ssc.BoatIndex = bi;
                                ssc.Statistic = statName;
                                boats.Add(ssc);
                            }
                        }
                    }

                    //loop through the 2nd level nodes AKA "leg nodes"
                    for (int legIndex = 0; legIndex < statsTGV.Nodes[boatIndex].Nodes.Count; legIndex++)
                    {
                        if (statsTGV.Nodes[boatIndex].Nodes[legIndex].Cells[columnIndex].Selected)
                        {
                            if (boatIndex < _replay.Boats.Count && legIndex < _replay.Race.Course.Route.Count)
                            {
                                if (legs.Count == 0)
                                {
                                    for (int bi = 0; bi < _replay.Boats.Count; bi++)
                                    {
                                        if (legIndex < _replay.Boats[bi].LegStatistics.Count)
                                        {
                                            SelectedStatisticCell ssc = new SelectedStatisticCell();
                                            ssc.BoatIndex = bi;
                                            ssc.LegIndex = legIndex;
                                            ssc.Statistic = statName;
                                            legs.Add(ssc);
                                        }
                                    }
                                }
                            }
                        }

                        //loop through the 3rd level nodes AKA "tack nodes"
                        for (int tackIndex = 0; tackIndex < statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes.Count; tackIndex++)
                        {
                            if (statsTGV.Nodes[boatIndex].Nodes[legIndex].Nodes[tackIndex].Cells[columnIndex].Selected)
                            {
                                if (boatIndex < _replay.Boats.Count && legIndex < _replay.Race.Course.Route.Count)
                                {
                                    if (tacks.Count == 0)
                                    {
                                        for (int bi = 0; bi < _replay.Boats.Count; bi++)
                                        {
                                            if(tackIndex<_replay.Boats[bi].TackStatistics.Count)
                                            {
                                                SelectedStatisticCell ssc = new SelectedStatisticCell();
                                                ssc.BoatIndex = bi;
                                                ssc.LegIndex = _replay.Boats[bi].Tacks[tackIndex].LegIndex;
                                                ssc.TackIndex = tackIndex;
                                                ssc.Statistic = statName;
                                                tacks.Add(ssc);
                                            }
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                }

                //open graphs for each group type
                if (boats.Count > 0)
                {
                    GraphForm gp = new GraphForm(Replay, statName, boats, SelectedUnitType);
                    gp.Show(this.DockPanel, UIHelper.FindCenteredPosition(this.DockPanel, this));
                    graphedAnything = true;
                }
                if (legs.Count > 0)
                {
                    GraphForm gp = new GraphForm(Replay, statName, legs, SelectedUnitType);
                    gp.Show(this.DockPanel, UIHelper.FindCenteredPosition(this.DockPanel, this));
                    graphedAnything = true;
                }
                if (tacks.Count > 0)
                {
                    GraphForm gp = new GraphForm(Replay, statName, tacks, SelectedUnitType);
                    gp.Show(this.DockPanel, UIHelper.FindCenteredPosition(this.DockPanel, this));
                    graphedAnything = true;
                }
            }
            if (!graphedAnything)
            {
                MessageBox.Show("Select one or more cells first.");
            }
        }
    }

    public class SelectedStatisticCell
    {
        public int? BoatIndex=null;
        public int? LegIndex=null;
        public int? TackIndex=null;
        public string Statistic = null;
    }
}
