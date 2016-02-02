using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Reflection;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.Data.Import;
using AmphibianSoftware.VisualSail.UI;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Library.Nmea;
using AmphibianSoftware.VisualSail.Library.IO;
using AmphibianSoftware.VisualSail.Data.Statistics;

namespace AmphibianSoftware.VisualSail.Executable
{
    static class Program
    {
        private static string key;
        private static string version;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Start(args, version, "GNU GENERAL PUBLIC LICENSE Version 3");
        }

        private static void Start(string[] args,string version,string aboutLicense)
        {
            if (args.Length > 0)
            {
                Application.Run(new SkipperMDI(args[0],version,aboutLicense));
            }
            else
            {
                Application.Run(new SkipperMDI(version, aboutLicense));
            }
        }
        
    }
}