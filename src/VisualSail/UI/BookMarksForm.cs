using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.Library;

using WeifenLuo.WinFormsUI.Docking;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class BookMarksForm : DockContent
    {
        private Replay _replay;
        public BookMarksForm(Replay replay)
        {
            InitializeComponent();
            _replay = replay;
            LoadBookmarks();
        }

        public void LoadBookmarks()
        {
            int? selectedIndex = null;
            if (bookMarksLV.SelectedIndices.Count > 0)
            {
                selectedIndex = bookMarksLV.SelectedIndices[0];
            }


            bookMarksLV.Items.Clear();

            TimeZoneInfo localZone = _replay.Race.Lake.TimeZone;

            List<Bookmark> bookmarks = GetBookMarks();
            if (bookmarks.Count == 0)
            {
                Bookmark raceStart = new Bookmark("Race Start", _replay.Race.UtcStart);
                raceStart.Save();
                bookmarks.Add(raceStart);
                Bookmark raceEnd = new Bookmark("Race End", _replay.Race.UtcEnd);
                raceEnd.Save();
                bookmarks.Add(raceEnd);
            }
            foreach (Bookmark b in bookmarks)
            {
                string[] subitems = { TimeZoneInfo.ConvertTimeFromUtc(b.Time, localZone).ToLongTimeString(), b.Name };
                bookMarksLV.Items.Add(new ListViewItem(subitems));
            }

            if (selectedIndex.HasValue && selectedIndex.Value < bookMarksLV.Items.Count)
            {
                bookMarksLV.Items[selectedIndex.Value].Selected = true;
            }
            else if (selectedIndex.HasValue && bookMarksLV.Items.Count > 0)
            {
                bookMarksLV.Items[bookMarksLV.Items.Count - 1].Selected = true;
            }
            else
            {
                bookMarksLV_SelectedIndexChanged(null, null);
            }
            
        }

        private List<Bookmark> GetBookMarks()
        {
            return Bookmark.FindInDateRange(_replay.Race.UtcStart, _replay.Race.UtcEnd);
        }

        private Bookmark SelectedBookMark
        {
            get
            {
                if (bookMarksLV.SelectedIndices.Count > 0)
                {
                    return GetBookMarks()[bookMarksLV.SelectedIndices[0]];
                }
                else
                {
                    return null;
                }
            }
        }

        private void bookMarksLV_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GoToSelected();
        }

        private void gotoBTN_Click(object sender, EventArgs e)
        {
            GoToSelected();
        }

        private void GoToSelected()
        {
            if (SelectedBookMark != null)
            {
                _replay.Pause();
                _replay.TargetTime = SelectedBookMark.Time;
            }
        }

        private void addBTN_Click(object sender, EventArgs e)
        {
            //get the time first, that way it's not delayed by the amount of time
            //it takes for the user to enter a name and hit ok.
            DateTime time = _replay.SimulationTime;
            NewBookMarkDialog nb = new NewBookMarkDialog();
            if (nb.ShowDialog() == DialogResult.OK)
            {
                Bookmark b = new Bookmark(nb.BookmarkName, time);
                b.Save();
                LoadBookmarks();
            }
        }

        private void removeBTN_Click(object sender, EventArgs e)
        {
            SelectedBookMark.Delete();
            LoadBookmarks();
        }

        private void bookMarksLV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bookMarksLV.SelectedIndices.Count > 0)
            {
                removeBTN.Enabled = true;
                gotoBTN.Enabled = true;
            }
            else
            {
                removeBTN.Enabled = false;
                gotoBTN.Enabled = false;
            }
        }
    }
}
