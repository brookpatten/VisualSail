using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using AmphibianSoftware.VisualSail.Data;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class SelectRace : Form
    {
        List<Race> _races;
        int? _selectedRaceIndex=null;

        //Race _selectedRace;
        public SelectRace()
        {
            this.DialogResult = DialogResult.None;
            InitializeComponent();
        }

        private void SelectRace_Load(object sender, EventArgs e)
        {
            raceLV.Items.Clear();
            _races = Race.FindAll();
            bool hasExistingRaces = false;
            foreach (Race r in _races)
            {
                hasExistingRaces = true;
                string startString = r.LocalStart.ToShortDateString() + " " + r.LocalStart.ToShortTimeString();
                string endString = r.LocalEnd.ToShortDateString() + " " + r.LocalEnd.ToShortTimeString();
                string[] items = { r.Name, startString, endString,r.Boats.Count.ToString() };
                ListViewItem lvi = new ListViewItem(items);
                raceLV.Items.Add(lvi);
            }
            if (!hasExistingRaces)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                AllowPhotos();
                raceLV.Items[0].Selected = true;
                raceLV.Focus();
            }
        }

        private void newBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            _selectedRaceIndex = null;
            this.Close();
        }

        private void openBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public Race SelectedRace
        {
            get
            {
                if (_selectedRaceIndex.HasValue)
                {
                    return _races[_selectedRaceIndex.Value];
                }
                else
                {
                    Race r=new Race();
                    r.Name=Race.DefaultName;
                    return r;
                }
            }
        }

        private void raceLV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (raceLV.SelectedIndices.Count > 0 && raceLV.SelectedIndices[0] >= 0)
            {
                _selectedRaceIndex = raceLV.SelectedIndices[0];
                openBTN.Enabled = true;
                if (SelectedRace.Boats.Count > 0 && SelectedRace.Course != null && SelectedRace.Lake != null)
                {
                    playBTN.Enabled = true;
                }
                else
                {
                    playBTN.Enabled = false;
                }
            }
            else
            {
                openBTN.Enabled = false;
                playBTN.Enabled = false;
                _selectedRaceIndex = null;
            }
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void playBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void AllowPhotos()
        {
            photoBTN.Enabled = Boat.FindAll().Count > 0 && Lake.FindAll().Count>0;
        }

        private void photoBTN_Click(object sender, EventArgs e)
        {
            PhotoManager manager = new PhotoManager();
            manager.ShowDialog(this);
        }
    }
}
