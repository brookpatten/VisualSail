using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;


namespace AmphibianSoftware.VisualSail.Data.Import
{
    public class VccImporter : FileImporter
    {
        private System.Globalization.CultureInfo _numberCulture;
        public VccImporter()
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
                SensorFile file = new SensorFile("vcc",fi.Name, DateTime.Now);
                file.Save();
                int rowCount = 0;
                foreach (XmlNode vcc in doc.ChildNodes)
                {
                    foreach (XmlNode capturedTrack in vcc.ChildNodes)
                    {
                        if (capturedTrack.Name == "CapturedTrack")
                        {
                            foreach (XmlNode trackpoints in capturedTrack.ChildNodes)
                            {
                                if (trackpoints.Name == "Trackpoints")
                                {
                                    foreach (XmlNode trackpoint in trackpoints.ChildNodes)
                                    {
                                        double lat = 0;
                                        double lon = 0;
                                        DateTime time = DateTime.MinValue;
                                        double heading = 0;
                                        double speed = 0;

                                        foreach (XmlAttribute attribute in trackpoint.Attributes)
                                        {
                                            if (attribute.Name == "dateTime")
                                            {
                                                time = DateTime.Parse(attribute.Value).ToUniversalTime();
                                            }
                                            if (attribute.Name == "heading")
                                            {
                                                heading = double.Parse(attribute.Value, _numberCulture.NumberFormat);
                                            }
                                            if (attribute.Name == "speed")
                                            {
                                                speed = double.Parse(attribute.Value, _numberCulture.NumberFormat);
                                            }
                                            if (attribute.Name == "latitude")
                                            {
                                                lat = double.Parse(attribute.Value, _numberCulture.NumberFormat);
                                            }
                                            if (attribute.Name == "longitude")
                                            {
                                                lon = double.Parse(attribute.Value, _numberCulture.NumberFormat);
                                            }
                                        }
                                        file.AddReading(time, lat, lon, 0, speed, heading, 0, 0, 0, 0, 0, 0, 0);
                                        rowCount++;
                                    }
                                }
                            }
                        }
                    }
                }
                if (rowCount > 0)
                {
                    boat.AddFile(file);
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch// (System.Xml.Schema.XmlSchemaException e)
            {
                return null;
            }
        }
    }
}
