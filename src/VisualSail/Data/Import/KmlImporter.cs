using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;


namespace AmphibianSoftware.VisualSail.Data.Import
{
    public class KmlImporter : FileImporter
    {
        private System.Globalization.CultureInfo _numberCulture;
        public KmlImporter()
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
                SensorFile file = new SensorFile("kml", fi.Name, DateTime.Now);
                file.Save();

                Dictionary<DateTime, CoordinatePoint> points = new Dictionary<DateTime, CoordinatePoint>();
                ExtractPoints(ref points, doc);

                foreach (DateTime dt in points.Keys)
                {
                    file.AddReading(dt, points[dt].Latitude.Value, points[dt].Longitude.Value, points[dt].HeightAboveGeoID, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                }

                boat.AddFile(file);
                return file;
            }
            catch //(Exception e)
            {
                return null;
            }
        }
        public void ExtractPoints(ref Dictionary<DateTime, CoordinatePoint> points,XmlNode node)
        {
            if (node.Name.ToLower() == "placemark")
            {
                DateTime? when=null;
                CoordinatePoint point=null;
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name.ToLower() == "timestamp")
                    {
                        foreach (XmlNode whenNode in child.ChildNodes)
                        {
                            if (whenNode.Name.ToLower() == "when")
                            {
                                string timeString = whenNode.InnerText;
                                timeString = timeString.Replace('T', ' ').Replace('Z', ' ');
                                when = DateTime.Parse(timeString);
                            }
                        }
                    }
                    else if (child.Name.ToLower() == "point")
                    {
                        foreach (XmlNode coordinateNode in child.ChildNodes)
                        {
                            if (coordinateNode.Name.ToLower() == "coordinates")
                            {
                                string coordinateString = coordinateNode.InnerText;

                                char[] splitters = { ',' };
                                string[] coordinateParts = coordinateString.Split(splitters);

                                double longitude = double.Parse(coordinateParts[0],_numberCulture.NumberFormat);
                                double latitude = double.Parse(coordinateParts[1], _numberCulture.NumberFormat);
                                double altitude = double.Parse(coordinateParts[2], _numberCulture.NumberFormat);
                                point = new CoordinatePoint(new Coordinate(latitude), new Coordinate(longitude), altitude);
                            }
                        }
                    }
                }
                if (when.HasValue && point != null)
                {
                    if (!points.ContainsKey(when.Value))
                    {
                        points.Add(when.Value, point);
                    }
                }
            }
            else
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    ExtractPoints(ref points, child);
                }
            }
        }
        public static string KmlXsdUrl
        {
            get
            {
                return "http://code.google.com/apis/kml/schema/kml21.xsd";
            }
        }
        public static string KmlXsdPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"//kml21.xsd";
            }
        }
    }
}
