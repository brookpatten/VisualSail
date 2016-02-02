using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;


namespace AmphibianSoftware.VisualSail.Data.Import
{
    public class GpxImporter:FileImporter
    {
        System.Globalization.CultureInfo _numberCulture;
        public GpxImporter()
        {
            _numberCulture = System.Globalization.CultureInfo.GetCultureInfo("en-us");
        }
        public override SensorFile ImportFile(string path, Boat boat)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                StringBuilder log = new StringBuilder();
                FileInfo fi = new FileInfo(path);
                SensorFile file = new SensorFile("gpx", fi.Name, DateTime.Now);
                file.Save();
                foreach (XmlNode gpx in doc.ChildNodes)
                {
                    if (gpx.Name == "gpx")
                    {
                        foreach (XmlNode wpt in gpx.ChildNodes)
                        {
                            /*if (wpt.Name == "wpt")
                            {
                                double lat=0;
                                double lon=0;
                                double elevation=0;
                                DateTime time=DateTime.MinValue;
                                foreach (XmlAttribute attribute in wpt.Attributes)
                                {
                                    if (attribute.Name == "lat")
                                    {
                                        lat = double.Parse(attribute.Value, _numberCulture.NumberFormat);
                                    }
                                    if (attribute.Name == "lon")
                                    {
                                        lon = double.Parse(attribute.Value, _numberCulture.NumberFormat);
                                    }
                                }
                                foreach (XmlElement attribute in wpt.ChildNodes)
                                {
                                    if (attribute.Name == "time")
                                    {
                                        string gpxTime = attribute.InnerText;
                                        gpxTime=gpxTime.Replace('T', ' ');
                                        gpxTime=gpxTime.Replace('Z', ' ');
                                        time = DateTime.Parse(gpxTime);
                                    }
                                    if (attribute.Name == "ele")
                                    {
                                        elevation = double.Parse(attribute.InnerText, _numberCulture.NumberFormat);
                                    }
                                }
                                file.AddReading(time, lat, lon, elevation, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                            }*/
                            if (wpt.Name == "trk")
                            {
                                foreach (XmlNode seg in wpt.ChildNodes)
                                {
                                    if (seg.Name == "trkseg")
                                    {
                                        foreach (XmlNode trkpt in seg.ChildNodes)
                                        {
                                            if (trkpt.Name == "trkpt")
                                            {
                                                double lat = 0;
                                                double lon = 0;
                                                double elevation = 0;
                                                DateTime time = DateTime.MinValue;
                                                foreach (XmlAttribute attribute in trkpt.Attributes)
                                                {
                                                    if (attribute.Name == "lat")
                                                    {
                                                        lat = double.Parse(attribute.Value, _numberCulture.NumberFormat);
                                                    }
                                                    if (attribute.Name == "lon")
                                                    {
                                                        lon = double.Parse(attribute.Value, _numberCulture.NumberFormat);
                                                    }
                                                }
                                                foreach (XmlElement attribute in trkpt.ChildNodes)
                                                {
                                                    if (attribute.Name == "time")
                                                    {
                                                        string gpxTime = attribute.InnerText;
                                                        gpxTime = gpxTime.Replace('T', ' ');
                                                        gpxTime = gpxTime.Replace('Z', ' ');
                                                        time = DateTime.Parse(gpxTime);
                                                    }
                                                    if (attribute.Name == "ele")
                                                    {
                                                        elevation = double.Parse(attribute.InnerText, _numberCulture.NumberFormat);
                                                    }
                                                }
                                                file.AddReading(time, lat, lon, elevation, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                boat.AddFile(file);
                return file;
            }
            catch//(System.Xml.Schema.XmlSchemaException e)
            {
                return null;
            }
        }
        public static string GpxXsdUrl
        {
            get
            {
                return "http://www.topografix.com/gpx/1/1/gpx.xsd";
            }
        }
        public static string GpxXsdPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory+@"gpx.xsd";
            }
        }
    }
}
