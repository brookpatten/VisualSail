using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using AmphibianSoftware.VisualSail.UI;
using AmphibianSoftware.VisualSail.Library.Nmea;

namespace AmphibianSoftware.VisualSail.Data.Import
{
    public class NmeaImporter : FileImporter
    {
        private DateTime? _startDate;
        private System.Globalization.CultureInfo _numberCulture;
        public NmeaImporter()
        {
            _numberCulture = System.Globalization.CultureInfo.GetCultureInfo("en-us");
        }
        private DateTime StartDate
        {
            get
            {
                if (_startDate == null)
                {
                    AmphibianSoftware.VisualSail.UI.Import.NMEA.NmeaOptions no = new AmphibianSoftware.VisualSail.UI.Import.NMEA.NmeaOptions();
                    no.ShowDialog();
                    _startDate = no.SelectedDate;
                    _startDate = ZeroTime(_startDate.Value);
                }
                return _startDate.Value;
            }
            set
            {
                _startDate = value;
            }
        }
        private DateTime ZeroTime(DateTime dt)
        {
            return dt - new TimeSpan(0, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }
        public override SensorFile ImportFile(string path, Boat boat)
        {
            int line = 0;
            StringBuilder log = new StringBuilder();
            try
            {
                FileInfo fi = new FileInfo(path);
                SensorFile file = new SensorFile("nmea", fi.Name, DateTime.Now);
                file.Save();

                StreamReader reader = new StreamReader(path);
                Dictionary<string, Dictionary<string, string>> first=null;
                if (!reader.EndOfStream)
                {
                    while (first == null && !reader.EndOfStream)
                    {
                        try
                        {
                            string lineText = reader.ReadLine();
                            first = NmeaParser.Parse(lineText);
                        }
                        catch (NMEAIgnoredSentenceException) { }
                        catch (NMEAUnkownSentenceException) { }
                        catch (NMEAEmptySentenceException) { }
                        catch (Exception e)
                        {
                            log.Append("Line " + line + ": " + e.Message);
                            log.Append(Environment.NewLine);
                        }
                        line++;
                    }
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            Dictionary<string, Dictionary<string, string>> current = first;
                            bool newGroup = true;
                            double latitude = 0;
                            double longitude = 0;
                            double speed = 0;
                            double waterTemperature = 0;
                            double depth = 0;
                            double relativeWindSpeed = 0;
                            double relativeWindAngle = 0;
                            double heading = 0;
                            TimeSpan? time = null;
                            Exception groupException = null;
                            while (newGroup || (current.Keys.ElementAt(0) != first.Keys.ElementAt(0) && !reader.EndOfStream))
                            {
                                try
                                {


                                    //process current
                                    if (current.Keys.ElementAt(0) == "Global Positioning System Fix Data")
                                    {
                                        //time
                                        string timestring = current[current.Keys.ElementAt(0)]["Time"];
                                        char[] splitter = {'.'};
                                        string[] parts = timestring.Split(splitter);
                                        if (parts[0].Length == 6)
                                        {
                                            int hour = int.Parse(parts[0].Substring(0, 2));
                                            int minute = int.Parse(parts[0].Substring(2, 2));
                                            int second = int.Parse(parts[0].Substring(4, 2));
                                            int millisecond = 0;
                                            if (parts.Length > 1)
                                            {
                                                millisecond =
                                                    (int)
                                                        (double.Parse("0." + parts[1], _numberCulture.NumberFormat)*
                                                         1000.0);
                                            }
                                            time = new TimeSpan(0, hour, minute, second, millisecond);
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Time Format");
                                        }

                                        //latitude
                                        string latitudeString = current[current.Keys.ElementAt(0)]["Latitude"];
                                        string latitudeDirectionString =
                                            current[current.Keys.ElementAt(0)]["Latitude Direction"];
                                        if (latitudeString.Length > 2)
                                        {
                                            int latdegrees = int.Parse(latitudeString.Substring(0, 2));
                                            double latminute = double.Parse(latitudeString.Substring(2),
                                                _numberCulture.NumberFormat);
                                            latitude = Coordinate.CoordinateToDouble(latdegrees, latminute, 0);
                                            if (latitudeDirectionString.ToLower() == "s")
                                            {
                                                latitude = -latitude;
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Latitude format");
                                        }

                                        //longitude
                                        string longitudeString = current[current.Keys.ElementAt(0)]["Longitude"];
                                        string longitudeDirectionString =
                                            current[current.Keys.ElementAt(0)]["Longitude Direction"];
                                        if (longitudeString.Length > 2)
                                        {
                                            int longdegrees = int.Parse(longitudeString.Substring(0, 3));
                                            double longminute = double.Parse(longitudeString.Substring(3),
                                                _numberCulture.NumberFormat);
                                            longitude = Coordinate.CoordinateToDouble(longdegrees, longminute, 0);
                                            if (longitudeDirectionString.ToLower() == "w")
                                            {
                                                longitude = -longitude;
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Longitude format");
                                        }
                                    }
                                    else if (current.Keys.ElementAt(0) == "Recommended Minimum Specific GPS/TRANSIT Data")
                                    {
                                        //time
                                        string timestring = current[current.Keys.ElementAt(0)]["Time of Fix"];
                                        char[] splitter = {'.'};
                                        string[] parts = timestring.Split(splitter);
                                        if (parts[0].Length == 6)
                                        {
                                            int hour = int.Parse(parts[0].Substring(0, 2));
                                            int minute = int.Parse(parts[0].Substring(2, 2));
                                            int second = int.Parse(parts[0].Substring(4, 2));
                                            time = new TimeSpan(0, hour, minute, second, 0);
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Time Format");
                                        }

                                        //latitude
                                        string latitudeString = current[current.Keys.ElementAt(0)]["Latitude"];
                                        string latitudeDirectionString =
                                            current[current.Keys.ElementAt(0)]["Latitude Direction"];
                                        if (latitudeString.Length > 2)
                                        {
                                            int latdegrees = int.Parse(latitudeString.Substring(0, 2));
                                            double latminute = double.Parse(latitudeString.Substring(2),
                                                _numberCulture.NumberFormat);
                                            latitude = Coordinate.CoordinateToDouble(latdegrees, latminute, 0);
                                            if (latitudeDirectionString.ToLower() == "s")
                                            {
                                                latitude = -latitude;
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Latitude format");
                                        }

                                        //longitude
                                        string longitudeString = current[current.Keys.ElementAt(0)]["Longitude"];
                                        string longitudeDirectionString =
                                            current[current.Keys.ElementAt(0)]["Longitude Direction"];
                                        if (longitudeString.Length > 2)
                                        {
                                            int longdegrees = int.Parse(longitudeString.Substring(0, 3));
                                            double longminute = double.Parse(longitudeString.Substring(3),
                                                _numberCulture.NumberFormat);
                                            longitude = Coordinate.CoordinateToDouble(longdegrees, longminute, 0);
                                            if (longitudeDirectionString.ToLower() == "w")
                                            {
                                                longitude = -longitude;
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Longitude format");
                                        }

                                        string speedString = current[current.Keys.ElementAt(0)]["Speed over ground"];
                                        double.TryParse(speedString, out speed);
                                        //speed = double.Parse(speedString, _numberCulture.NumberFormat);

                                        string datestring = current[current.Keys.ElementAt(0)]["Date of Fix"];
                                        if (datestring.Length == 6)
                                        {
                                            int day = int.Parse(datestring.Substring(0, 2));
                                            int month = int.Parse(datestring.Substring(2, 2));
                                            int year = 2000 + int.Parse(datestring.Substring(4, 2));
                                            DateTime dt = new DateTime(year, month, day);
                                            dt = ZeroTime(dt);
                                            StartDate = dt;
                                            time = new TimeSpan(0, time.Value.Hours, time.Value.Minutes,
                                                time.Value.Seconds, time.Value.Milliseconds);
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Time Format");
                                        }
                                    }
                                    else if (current.Keys.ElementAt(0) == "Data and Time")
                                    {
                                        //time
                                        string timestring = current[current.Keys.ElementAt(0)]["Time"];
                                        char[] splitter = {'.'};
                                        string[] parts = timestring.Split(splitter);
                                        if (parts[0].Length == 6)
                                        {
                                            int hour = int.Parse(parts[0].Substring(0, 2));
                                            int minute = int.Parse(parts[0].Substring(2, 2));
                                            int second = int.Parse(parts[0].Substring(4, 2));
                                            int millisecond =
                                                (int)
                                                    (double.Parse("0." + parts[1], _numberCulture.NumberFormat)*
                                                     1000.0);
                                            int day = int.Parse(current[current.Keys.ElementAt(0)]["Day"]);
                                            int month = int.Parse(current[current.Keys.ElementAt(0)]["Month"]);
                                            int year = int.Parse(current[current.Keys.ElementAt(0)]["Year"]);
                                            DateTime dt = new DateTime(year, month, day);
                                            dt = ZeroTime(dt);
                                            StartDate = dt;
                                            time = new TimeSpan(0, hour, minute, second, millisecond);
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Time Format");
                                        }
                                    }
                                    else if (current.Keys.ElementAt(0) == "Water Temperature")
                                    {
                                        string tempString = current[current.Keys.ElementAt(0)]["Water Temperature"];
                                        string tempUnitString =
                                            current[current.Keys.ElementAt(0)]["Water Temperature Unit"];
                                        waterTemperature = double.Parse(tempString, _numberCulture.NumberFormat);
                                        //convert fahrenheit to celsius
                                        if (tempUnitString.ToLower() == "f")
                                        {
                                            waterTemperature = (5.0/9.0)*(waterTemperature - 32.0);
                                        }
                                    }
                                    else if (current.Keys.ElementAt(0) == "Depth")
                                    {
                                        string depthString = current[current.Keys.ElementAt(0)]["Depth"];
                                        depth = double.Parse(depthString, _numberCulture.NumberFormat);
                                    }
                                    else if (current.Keys.ElementAt(0) == "Wind Speed and Angle")
                                    {
                                        string windAngleString = current[current.Keys.ElementAt(0)]["Wind Angle"];
                                        string windspeedString = current[current.Keys.ElementAt(0)]["Wind Speed"];
                                        string windspeedUnitString =
                                            current[current.Keys.ElementAt(0)]["Wind Speed Unit"];
                                        relativeWindAngle = double.Parse(windAngleString,
                                            _numberCulture.NumberFormat);
                                        relativeWindSpeed =
                                            ConvertValueWithUnit(windspeedString + " " + windspeedUnitString);
                                    }
                                    else if (current.Keys.ElementAt(0) == "NovaSail")
                                    {
                                        string latitudeString = current[current.Keys.ElementAt(0)]["Latitude"];
                                        string longitudeString = current[current.Keys.ElementAt(0)]["Longitude"];
                                        string headingString = current[current.Keys.ElementAt(0)]["Heading"];
                                        string speedString = current[current.Keys.ElementAt(0)]["Speed"];
                                        string rollString = current[current.Keys.ElementAt(0)]["Roll"];
                                        string dayString = current[current.Keys.ElementAt(0)]["Day"];
                                        string monthString = current[current.Keys.ElementAt(0)]["Month"];
                                        string yearString = current[current.Keys.ElementAt(0)]["Year"];
                                        string hourString = current[current.Keys.ElementAt(0)]["Hour"];
                                        string minuteString = current[current.Keys.ElementAt(0)]["Minute"];
                                        string secondString = current[current.Keys.ElementAt(0)]["Second"];

                                        latitude = double.Parse(latitudeString, _numberCulture.NumberFormat);
                                        longitude = double.Parse(longitudeString, _numberCulture.NumberFormat);

                                        speed = double.Parse(speedString, _numberCulture.NumberFormat);
                                        heading = double.Parse(headingString, _numberCulture.NumberFormat);
                                        //roll
                                        DateTime dt = new DateTime(int.Parse(yearString), int.Parse(monthString),
                                            int.Parse(dayString));
                                        dt = ZeroTime(dt);
                                        StartDate = dt;
                                        time = new TimeSpan(0, int.Parse(hourString), int.Parse(minuteString),
                                            int.Parse(secondString), 0);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    groupException = ex;
                                }
                                line++;
                                string lineText = reader.ReadLine();
                                current = NmeaParser.Parse(lineText);
                                newGroup = false;
                            }
                            if (time != null && groupException==null)
                            {
                                file.AddReading(StartDate + time.Value, latitude, longitude, 0, speed, heading, depth, waterTemperature, 0, relativeWindAngle, relativeWindSpeed, 0, 0);
                            }
                            else
                            {
                                //log something?
                                log.Append("The date and time could not be determined at line " + line);
                                log.Append(Environment.NewLine);
                            }
                            first = current;
                        }
                        catch (NMEAIgnoredSentenceException) {}
                        catch (NMEAUnkownSentenceException) {}
                        catch (NMEAEmptySentenceException) {}
                        catch (Exception e)
                        {
                            log.Append("Line " + line + ": " + e.Message);
                            log.Append(Environment.NewLine);
                        }
                        
                    }
                   
                    boat.AddFile(file);
                    if (log.Length > 0)
                    {
                        if (log.ToString().Length > 1000)
                        {
                            MessageBox.Show(log.ToString().Substring(0, 1000));
                        }
                        else
                        {
                            MessageBox.Show(log.ToString());
                        }
                    }
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
