using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AmphibianSoftware.VisualSail.Library
{
    public static class ContentHelper
    {
        public static string StaticContentPath
        {
            get
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                FileInfo fi = new FileInfo(path);
                DirectoryInfo di = fi.Directory;
                return di.FullName + @"\Content\";
            }
        }

        public static string DynamicContentPath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Amphibian Software\VisualSail\Content";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path+@"\";
            }
        }
    }
}
