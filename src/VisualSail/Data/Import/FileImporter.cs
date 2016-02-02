using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Web;
using System.Net;
using System.Data;

using AmphibianSoftware.VisualSail.UI;
using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.UI.Import.CSV;
using AmphibianSoftware.VisualSail.UI.Import.NMEA;

namespace AmphibianSoftware.VisualSail.Data.Import
{
    public abstract class FileImporter
    {
        public abstract SensorFile ImportFile(string path, Boat boat);
        public static FileImporter DetectFileType(string path)
        {
            if (path.ToLower().EndsWith(".sail"))
            {
                throw new Exception(@".sail files cannot be imported, use ""open"" instead.");
            }
            else if (path.ToLower().EndsWith(".gpx") /*&& ValidateXmlWithSchema(path, GpxImporter.GpxXsdUrl,GpxImporter.GpxXsdPath)*/)
            {
                GpxImporter gi = new GpxImporter();
                return gi;
            }
            else if (path.ToLower().EndsWith(".kml") /*&& ValidateXmlWithSchema(path, KmlImporter.KmlXsdUrl, KmlImporter.KmlXsdPath)*/)
            {
                KmlImporter ki = new KmlImporter();
                return ki;
            }
            else if (path.ToLower().EndsWith(".vcc"))
            {
                VccImporter vi = new VccImporter();
                return vi;
            }
            else
            {
                StreamReader reader = new StreamReader(path);
                string firstLine = reader.ReadLine();
                reader.Close();

                if (firstLine.Contains(","))
                {
                    //possibly a csv format
                    char[] splitter = new char[] { ',' };
                    string[] parts = firstLine.Split(splitter);

                    if (parts[0].StartsWith("$")||parts[0].StartsWith("!"))
                    {
                        NmeaImporter ni = new NmeaImporter();
                        return ni;
                    }
                    else
                    {
                        //csv format
                        List<string> mappings = CsvImporter.AutoDetectColumnMappings(parts);
                        ColumnAssignment ca = new ColumnAssignment(mappings, path);
                        ca.ShowDialog();
                        mappings = ca.ColumnMappings;
                        ColumnFilter cf = new ColumnFilter(ca.Filters, path, ca.SkipFirstRow);
                        CsvImporter ci = new CsvImporter(mappings, cf.FilterValues, ca.SkipFirstRow);
                        return ci;
                    }
                }
                else
                {
                    throw new Exception("Format Could not be determined");
                }
            }
        }
        public static bool ValidateXmlWithSchema(string path, string schemaUrl,string schemaPath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, schemaPath);

            try
            {
                XmlReader reader = XmlReader.Create(path, settings);
                while (reader.Read())
                {
                    //read through the doc and validate it
                }
                return true;
            }
            catch (System.Xml.Schema.XmlSchemaException)
            {
                return false;
            }
        }
        public static void DownloadFile(string url, string path)
        {
            //use this to download schemas...maybe..or not
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            StreamWriter writer = new StreamWriter(path);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                writer.WriteLine(line);
            }
            writer.Close();
            reader.Close();
            response.Close();
        }
        public static List<string> AllowedColumns
        {
            get
            {
                List<string> columns = new List<string>();
                foreach (DataColumn dc in Persistance.Data.SensorReadings.Columns)
                {
                    if (dc.ColumnName != "sensorfile_id" && dc.ColumnName != "id")
                    {
                        columns.Add(dc.ColumnName);
                    }
                }
                columns.Add("date");
                columns.Add("time");
                columns.Add("n/s");
                columns.Add("e/w");
                columns.Add("height unit");
                columns.Add("speed unit");

                return columns;
            }
        }
        public static double ConvertValueWithUnit(string value)
        {
            System.Globalization.CultureInfo numberCulture = System.Globalization.CultureInfo.GetCultureInfo("en-us");

            char[] splitter = new char[] { ' ' };
            string[] parts = value.Split(splitter);
            value = parts[0];
            string unit = string.Empty;
            if (parts.Length > 1)
            {
                unit = parts[1].ToLower();
            }

            if (unit == string.Empty)
            {
                return double.Parse(value, numberCulture.NumberFormat);
            }
            else
            {
                double parsed = double.Parse(value, numberCulture.NumberFormat);
                if (unit == "n")
                {
                    //north
                }
                else if (unit == "s")
                {
                    //make south values negative
                    parsed = -parsed;
                }
                else if (unit == "e")
                {
                    //east
                }
                else if (unit == "w")
                {
                    //make west values negative
                    parsed = -parsed;
                }
                else if (unit == "f")
                {
                    //convert to meters
                    parsed = parsed * 0.3048;
                }
                else if (unit == "f/s")
                {
                    //convert to km/h
                    parsed = parsed * 0.0003048;//kilometers per foot
                    parsed = parsed * 60 * 60;//seconds to hours
                }
                else if (unit == "m/h"||unit=="mph")
                {
                    //convert to km/h
                    parsed = parsed * 1.609344;
                }
                else if (unit == "m")
                {
                    //meters....might cause problem if someones uses m for miles
                }
                else if (unit == "km/h")
                {
                    //kilometers per hour
                }
                else
                {
                    //wtf?
                    Exception e = new Exception("Unknown Unit");
                    //e.InnerException = new Exception("Unknown Unit " + unit);
                    throw e;
                }
                return parsed;
            }
        }
    }
}
