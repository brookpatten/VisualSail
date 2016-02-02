using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AmphibianSoftware.VisualSail.UI.Import.CSV
{
    public partial class ColumnFilter : Form
    {
        Dictionary<int, List<string>> _possibleFilters;
        Dictionary<int, List<string>> _selectedFilters;
        Dictionary<int, string> _columnNames;
        public ColumnFilter(List<bool> filteredColumns,string path,bool skipFirstRow)
        {
            InitializeComponent();
            _possibleFilters = new Dictionary<int, List<string>>();
            _selectedFilters = new Dictionary<int, List<string>>();
            _columnNames = new Dictionary<int, string>();

            bool hasFilters = false;

            for (int i = 0; i < filteredColumns.Count; i++)
            {
                if (filteredColumns[i])
                {
                    _possibleFilters[i] = new List<string>();
                    _selectedFilters[i] = new List<string>();
                    hasFilters = true;
                }
            }

            //if there's no filters, there's no need to do this
            //so we'll skip it to speed things up
            if (hasFilters)
            {
                StreamReader reader = new StreamReader(path);
                bool isFirst = true;
                char[] splitter = new char[] { ',' };
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(splitter);
                        
                    if ((skipFirstRow && !isFirst) || !skipFirstRow)
                    {
                        if (!skipFirstRow && isFirst)
                        {
                            for (int i = 0; i < filteredColumns.Count; i++)
                            {
                                if (filteredColumns[i])
                                {
                                    _columnNames[i] = "Column " + i;
                                }
                            }
                        }

                        for (int i = 0; i < _possibleFilters.Keys.Count; i++)
                        {
                            if (!_possibleFilters[_possibleFilters.Keys.ElementAt(i)].Contains(parts[_possibleFilters.Keys.ElementAt(i)]))
                            {
                                _possibleFilters[_possibleFilters.Keys.ElementAt(i)].Add(parts[_possibleFilters.Keys.ElementAt(i)]);
                                _selectedFilters[_possibleFilters.Keys.ElementAt(i)].Add(parts[_possibleFilters.Keys.ElementAt(i)]);
                            }
                        }
                    }
                    else if(skipFirstRow && isFirst)
                    {
                        isFirst = false;
                        for (int i = 0; i < filteredColumns.Count; i++)
                        {
                            if (filteredColumns[i])
                            {
                                _columnNames[i] = parts[i];
                            }
                        }
                    }
                }
                PopulateColumnList();
                columnLB.SelectedIndex = 0;
                this.ShowDialog();
            }
        }

        private void PopulateColumnList()
        {
            int selectedColumn = columnLB.SelectedIndex;
            columnLB.Items.Clear();
            foreach (int i in _possibleFilters.Keys)
            {
                columnLB.Items.Add(_columnNames[i]);
            }
            if (selectedColumn >= 0)
            {
                columnLB.SelectedIndex = selectedColumn;
            }
        }
        private void PopulateFilterList()
        {
            if (columnLB.SelectedIndex >= 0)
            {
                valuesCLB.Items.Clear();
                foreach (string s in _possibleFilters[_possibleFilters.Keys.ElementAt(columnLB.SelectedIndex)])
                {
                    bool check = _selectedFilters[_possibleFilters.Keys.ElementAt(columnLB.SelectedIndex)].Contains(s);
                    valuesCLB.Items.Add(s, check);
                }
            }
        }

        private void columnLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateFilterList();
        }

        private void valuesCLB_SelectedValueChanged(object sender, EventArgs e)
        {
            if (columnLB.SelectedIndex >= 0)
            {
                _selectedFilters[_selectedFilters.Keys.ElementAt(columnLB.SelectedIndex)] = new List<string>();
                foreach (object o in valuesCLB.CheckedItems)
                {
                    _selectedFilters[_selectedFilters.Keys.ElementAt(columnLB.SelectedIndex)].Add((string)o);
                }
            }
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public Dictionary<int,List<string>> FilterValues
        {
            get
            {
                return _selectedFilters;
            }
        }
    }
}
