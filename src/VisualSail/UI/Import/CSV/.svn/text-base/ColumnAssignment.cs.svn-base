using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AmphibianSoftware.VisualSail.Data.Import;
using System.IO;

namespace AmphibianSoftware.VisualSail.UI.Import.CSV
{
    public partial class ColumnAssignment : Form
    {
        List<string> _mappings;
        List<bool> _filters;
        public ColumnAssignment(List<string> initialMapping,string path)
        {
            _mappings = initialMapping;
            InitializeComponent();
            FileInfo fi = new FileInfo(path);
            this.Text = this.Text + " - " + fi.Name;
            PopulateHeaderRow();
            PopulateDataRows(path);
        }
        private void PopulateHeaderRow()
        {
            List<string> possibleMappings = FileImporter.AllowedColumns;
            DataTable dt = new DataTable();
            for (int i = 0; i < _mappings.Count;i++ )
            {
                DataGridViewComboBoxColumn cc = new DataGridViewComboBoxColumn();
                cc.DisplayMember = "Column "+(i+1);
                cc.ValueMember = "column"+i;
                cc.DataPropertyName = "column"+i;
                cc.HeaderText = "Map to";
                cc.Items.Add("");
                cc.Width = 150;
                foreach (string s in possibleMappings)
                {
                    cc.Items.Add(s);
                }
                headerGV.Columns.Add(cc);

                DataGridViewCheckBoxColumn filter = new DataGridViewCheckBoxColumn();
                filter.DataPropertyName = "filter" + i;
                filter.HeaderText = "Filter";
                filter.Width = 50;
                headerGV.Columns.Add(filter);

                dt.Columns.Add("column" + i);
                dt.Columns.Add("filter" + i,typeof(bool));
            }
            DataRow dr = dt.NewRow();
            for (int i = 0; i < _mappings.Count; i++)
            {
                dr[i*2] = _mappings[i];
                dr[(i*2)+1] = false;
            }
            dt.Rows.Add(dr);
            headerGV.DataSource = dt;
        }
        private void PopulateDataRows(string path)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < _mappings.Count; i++)
            {
                fileGV.Columns.Add("column" + i, "Column " + (i + 1));
                fileGV.Columns[fileGV.Columns.Count - 1].DataPropertyName = "column" + i;
                fileGV.Columns[fileGV.Columns.Count - 1].Width = 200;
                DataColumn dc = new DataColumn("column" + i);
                dt.Columns.Add(dc);
            }
            StreamReader reader = new StreamReader(path);
            int s = 0;
            char[] splitter = new char[] { ',' };
            while (!reader.EndOfStream && s < 10)
            {
                string line = reader.ReadLine();
                string[] parts = line.Split(splitter);
                DataRow dr = dt.NewRow();
                for (int c = 0; c < _mappings.Count;c++ )
                {
                    if (c < parts.Length)
                    {
                        dr[c] = parts[c];
                    }
                    else
                    {
                        dr[c] = "";
                    }
                }
                dt.Rows.Add(dr);
                s++;
            }
            reader.Close();
            fileGV.DataSource = dt;
        }
        public List<string> ColumnMappings
        {
            get
            {
                return _mappings;
            }
        }
        public List<bool> Filters
        {
            get
            {
                return _filters;
            }
        }
        public bool SkipFirstRow
        {
            get
            {
                return skipFirstRowCB.Checked;
            }
        }

        private void fileGV_Scroll(object sender, ScrollEventArgs e)
        {
            headerGV.HorizontalScrollingOffset = fileGV.HorizontalScrollingOffset;
            
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            List<string> newMappings = new List<string>();
            List<bool> newFilters = new List<bool>();
            DataRow dr = ((DataTable)headerGV.DataSource).Rows[0];
            for (int i = 0; i < dr.Table.Columns.Count; i=i+2)
            {
                newMappings.Add((string)dr[i]);
                newFilters.Add((bool)dr[i + 1]);
            }
            _mappings = newMappings;
            _filters = newFilters;
            this.Close();
        }
    }
}
