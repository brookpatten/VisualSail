using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using AmphibianSoftware.VisualSail.Library;

namespace AmphibianSoftware.VisualSail.UI
{
    public delegate void Increment(int value);
    public partial class Splash : Form
    {
        string _version;
        string _aboutLicense;
        Thread runner;
        public Splash(string version,string aboutLicense)
        {
            _aboutLicense = aboutLicense;
            _version = version;
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            versionLBL.Text = _version;
            licenseLBL.Text = _aboutLicense;
            runner = new Thread(new ThreadStart(this.run));
            runner.Start();
        }

        private void run()
        {
            Increment inc = new Increment(this.incrementer);
            for (int i = 0; i < 100; i++)
            {
                lock (loadPB)
                {
                    loadPB.Invoke(inc, i);
                }
//#if !DEBUG
                Thread.Sleep(30);
//#endif
            }
            this.Invoke(new Notify(this.closer), null);
        }

        private void closer()
        {
            this.Close();
        }

        private void incrementer(int value)
        {
            loadPB.Value = value;
        }
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }
    }
}
