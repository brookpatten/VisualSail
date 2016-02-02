using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;


namespace AmphibianSoftware.VisualSail.Data.Import
{
    public class CsvImporter:FileImporter
    {
        private List<string> _mappings;
        private bool _skipFirstRow;
        private Dictionary<int, List<string>> _allowedValues;
        private System.Globalization.CultureInfo _numberCulture;
        private CsvImporter() 
        {
            throw new System.NotImplementedException();
        }
        public CsvImporter(List<string> columnMappings,Dictionary<int,List<string>> allowedValues,bool skipFirst)
        {
            _mappings = columnMappings;
            _skipFirstRow = skipFirst;
            _allowedValues = allowedValues;
            _numberCulture = System.Globalization.CultureInfo.GetCultureInfo("en-us");
        }

        public override SensorFile ImportFile(string path, Boat boat)
        {
            StringBuilder log = new StringBuilder();
            char[] splitter = new char[] { ',' };

            FileInfo fi = new FileInfo(path);

            SensorFile file = new SensorFile("csv", fi.Name, DateTime.Now);
            file.Save();

            
            

            int lineNumber = 1;
            StreamReader reader = new StreamReader(path);
            bool isFirst = true;
            while (!reader.EndOfStream)
            {
                try
                {
                    string line = reader.ReadLine();
                    if ((_skipFirstRow && !isFirst) || !_skipFirstRow)
                    {
                        DateTime date=DateTime.Now;
                        string latDirection="";
                        string longDirection="";
                        string heightUnit="";
                        string speedUnit="";

                        string[] parts = line.Split(splitter);

                        if (parts.Length != _mappings.Count)
                        {
                            log.Append("Line " + lineNumber + " has an unexpected number of columns");
                        }

                        bool filtered = false;
                        foreach (int i in _allowedValues.Keys)
                        {
                            if (!_allowedValues[i].Contains(parts[i]))
                            {
                                filtered = true;
                                break;
                            }
                        }

                        if (!filtered)
                        {
                            SkipperDataSet.SensorReadingsRow reading = (SkipperDataSet.SensorReadingsRow)Persistance.Data.SensorReadings.NewRow();
                            for (int i = 0; i < parts.Length; i++)
                            {
                                if (Persistance.Data.SensorReadings.Columns.Contains(_mappings[i]))
                                {
                                    if (Persistance.Data.SensorReadings.Columns[_mappings[i]].DataType == typeof(double))
                                    {
                                        if (_mappings[i] == "latitude")
                                        {
                                            if (latDirection != "")
                                            {
                                                reading[_mappings[i]] = ConvertValueWithUnit(parts[i] + " " + latDirection);
                                            }
                                            else
                                            {
                                                reading[_mappings[i]] = ConvertValueWithUnit(parts[i]);
                                            }
                                        }
                                        else if (_mappings[i] == "longitude")
                                        {
                                            if (longDirection != "")
                                            {
                                                reading[_mappings[i]] = ConvertValueWithUnit(parts[i] + " " + longDirection);
                                            }
                                            else
                                            {
                                                reading[_mappings[i]] = ConvertValueWithUnit(parts[i]);
                                            }
                                        }
                                        else if (_mappings[i] == "altitude")
                                        {
                                            if (heightUnit != "")
                                            {
                                                reading[_mappings[i]] = ConvertValueWithUnit(parts[i] + " " + heightUnit);
                                            }
                                            else
                                            {
                                                reading[_mappings[i]] = ConvertValueWithUnit(parts[i]);
                                            }
                                        }
                                        else if (_mappings[i] == "speed")
                                        {
                                            if (speedUnit != "")
                                            {
                                                reading[_mappings[i]] = ConvertValueWithUnit(parts[i] + " " + speedUnit);
                                            }
                                            else
                                            {
                                                reading[_mappings[i]] = ConvertValueWithUnit(parts[i]);
                                            }
                                        }
                                        else
                                        {
                                            reading[_mappings[i]] = ConvertValueWithUnit(parts[i]);
                                        }
                                    }
                                    else if (Persistance.Data.SensorReadings.Columns[_mappings[i]].DataType == typeof(string))
                                    {
                                        reading[_mappings[i]] = parts[i];
                                    }
                                    else if (Persistance.Data.SensorReadings.Columns[_mappings[i]].DataType == typeof(DateTime))
                                    {
                                        date = DateTime.Parse(parts[i]);
                                        reading[_mappings[i]] = date;
                                    }
                                    else
                                    {
                                        throw new Exception("Unknown type");
                                    }
                                }
                                else
                                {
                                    if (_mappings[i] == "date")
                                    {
                                        int hours = date.Hour;
                                        int minutes = date.Minute;
                                        int seconds = date.Second;
                                        int millis = date.Millisecond;
                                        date = DateTime.Parse(parts[i]);
                                        date = date.Add(new TimeSpan(0, hours, minutes, seconds, millis));
                                        reading["datetime"] = date;
                                    }
                                    else if (_mappings[i] == "time")
                                    {
                                        int hours = date.Hour;
                                        int minutes = date.Minute;
                                        int seconds = date.Second;
                                        int millis = date.Millisecond;
                                        date = date.Subtract(new TimeSpan(0, hours, minutes, seconds, millis));
                                        date = date.Add(TimeSpan.Parse(parts[i]));
                                        reading["datetime"] = date;
                                    }
                                    else if (_mappings[i] == "n/s")
                                    {
                                        latDirection = parts[i].ToLower();
                                        if (reading["latitude"] != System.DBNull.Value)
                                        {
                                            reading["latitude"] = ConvertValueWithUnit(reading["latitude"] + " " + latDirection);
                                        }
                                    }
                                    else if (_mappings[i] == "e/w")
                                    {
                                        longDirection = parts[i].ToLower();
                                        if (reading["longitude"] != System.DBNull.Value)
                                        {
                                            reading["longitude"] = ConvertValueWithUnit(reading["longitude"] + " " + longDirection);
                                        }
                                    }
                                    else if (_mappings[i] == "height unit")
                                    {
                                        heightUnit = parts[i].ToLower();
                                        if (reading["altitude"] != System.DBNull.Value)
                                        {
                                            reading["altitude"] = ConvertValueWithUnit(reading["altitude"] + " " + heightUnit);
                                        }
                                    }
                                    else if (_mappings[i] == "speed unit")
                                    {
                                        speedUnit = parts[i].ToLower();
                                        if (reading["speed"] != System.DBNull.Value)
                                        {
                                            reading["speed"] = ConvertValueWithUnit(reading["speed"] + " " + speedUnit);
                                        }
                                    }
                                    else if (_mappings[i] == "")
                                    {
                                    }
                                    else
                                    {
                                        throw new Exception("invalid column mapping");
                                    }
                                }
                            }

                            file.AddReading(reading);
                        }
                    }
                    else
                    {
                        isFirst = false;
                    }
                }
                catch (Exception e)
                {
                    log.Append("Error loading line " + lineNumber + ". " + e.Message+Environment.NewLine);
                }
                lineNumber++;
            }
            reader.Close();
            if (log.Length > 0)
            {
                if(log.ToString().Length>1000)
                    log.Remove(1000, log.ToString().Length - 1000);
                MessageBox.Show("Errors occured while loading the file" + Environment.NewLine + log.ToString());
            }
            boat.AddFile(file);


            return file;
        }
        public static List<string> AutoDetectColumnMappings(string[] columnNames)
        {
            List<string> possibleColumns = FileImporter.AllowedColumns;
            List<string> mappings = new List<string>();
            for (int i = 0; i < columnNames.Length; i++)
            {
                bool mapped = false;
                for (int x = 0; x < possibleColumns.Count; x++)
                {
                    if (columnNames[i].ToLower() == possibleColumns[x].ToLower())
                    {
                        mapped = true;
                        mappings.Add(possibleColumns[x]);
                        possibleColumns.RemoveAt(x);
                        break;
                    }
                }
                //begin aliases
                if (!mapped)
                {
                    if (columnNames[i].ToLower() == "height")
                    {
                        mappings.Add("altitude");
                        mapped = true;
                    }
                }
                //end aliases
                if (!mapped)
                {
                    mappings.Add("");
                }
            }
            return mappings;
        }
    }
}
