using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class BusyDialog : Form
    {
        public BusyDialog()
        {
            InitializeComponent();
        }
        public string DetailText
        {
            get
            {
                return detailLBL.Text;
            }
            set
            {
                detailLBL.Text = value;
            }
        }
        public static string CleanPath(string path)
        {
            char[] splitter={'\\','/'};
            return path.Substring(path.LastIndexOfAny(splitter) + 1);
        }
    }
}
