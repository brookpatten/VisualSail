using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class GettingStarted : DockContent
    {
        private EventHandler OpenEventHandler;
        private EventHandler NewEventHandler;
        private EventHandler GpsEventHandler;

        public GettingStarted()
        {
            InitializeComponent();
        }

        private void newBTN_Click(object sender, EventArgs e)
        {
            NewEventHandler(sender, e);
        }

        private void openBTN_Click(object sender, EventArgs e)
        {
            OpenEventHandler(sender, e);
        }

        private void newTB_Click(object sender, EventArgs e)
        {
            NewEventHandler(sender, e);
        }

        private void openTB_Click(object sender, EventArgs e)
        {
            OpenEventHandler(sender, e);
        }

        public event EventHandler OpenClick
        {
            add
            {
                OpenEventHandler += value;
            }
            remove
            {
                OpenEventHandler -= value;
            }
        }

        public event EventHandler NewClick
        {
            add
            {
                NewEventHandler += value;
            }
            remove
            {
                NewEventHandler -= value;
            }
        }

        public event EventHandler GpsClick
        {
            add
            {
                GpsEventHandler += value;
            }
            remove
            {
                GpsEventHandler -= value;
            }
        }
        
        
        private void newLBL_Click(object sender, EventArgs e)
        {
            NewEventHandler(sender, e);
        }

        private void openLBL_Click(object sender, EventArgs e)
        {
            OpenEventHandler(sender, e);
        }

        private void gpsBTN_Click(object sender, EventArgs e)
        {
            GpsEventHandler(sender, e);
        }

        private void gpsLBL_Click(object sender, EventArgs e)
        {
            GpsEventHandler(sender, e);
        }
    }
}
