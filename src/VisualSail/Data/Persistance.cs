using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace AmphibianSoftware.VisualSail.Data
{
    public static class Persistance
    {
        private static SkipperDataSet _data;
        private static string _defaultPath="default.sail";

        private static string _rgbKeyString = "rgbKey12";
        private static string _rgbIVString = "rgbIV 12";
        private static byte[] _rgbKey;
        private static byte[] _rgbIV;

        static Persistance()
        {
            _data = null;
            _rgbKey = new System.Text.ASCIIEncoding().GetBytes(_rgbKeyString);
            _rgbIV = new System.Text.ASCIIEncoding().GetBytes(_rgbIVString);
        }

        public static void CreateNew()
        {
            //DateTime start = DateTime.Now;

            SkipperDataSet ds = new SkipperDataSet();
            
            ds.BoatType.AddBoatTypeRow("Unspecified");
            
            //ds.Boat.AddBoatRow("Brook's Car", "OH1234", Color.Red.ToArgb(), (SkipperDataSet.BoatTypeRow)ds.BoatType.Rows[0]);
            
            //ds.Lake.AddLakeRow("Indian Lake", Coordinate.CoordinateToDouble(40,31,24),Coordinate.CoordinateToDouble(40,27,60),-Coordinate.CoordinateToDouble(83,50,26),-Coordinate.CoordinateToDouble(83,55,26), 300, "",TimeZoneInfo.Local.ToSerializedString());
            
            //ds.Course.AddCourseRow("Work to Home", start, (SkipperDataSet.LakeRow)ds.Lake.Rows[0]);
            
            //ds.Mark.AddMarkRow("Home","Mark",(SkipperDataSet.CourseRow)ds.Course.Rows[0]);
            //ds.Bouy.AddBouyRow((SkipperDataSet.MarkRow)ds.Mark.Rows[0], Coordinate.CoordinateToDouble(39,17,53.50), -Coordinate.CoordinateToDouble(84,29,24.21));
            
            //ds.Mark.AddMarkRow("Work", "Mark", (SkipperDataSet.CourseRow)ds.Course.Rows[0]);
            //ds.Bouy.AddBouyRow((SkipperDataSet.MarkRow)ds.Mark.Rows[1], Coordinate.CoordinateToDouble(39,3,45.20), -Coordinate.CoordinateToDouble(84,32,17.40));
            
            //ds.SensorFile.AddSensorFileRow("nmea", "vanquisher boat", DateTime.Now);
            //ds.SensorReadings.AddSensorReadingsRow((SkipperDataSet.SensorFileRow)ds.SensorFile.Rows[0], start + new TimeSpan(60000), 60, 60);
            //ds.SensorReadings.AddSensorReadingsRow((SkipperDataSet.SensorFileRow)ds.SensorFile.Rows[0], start + new TimeSpan(120000), 70, 70);
            //ds.SensorReadings.AddSensorReadingsRow((SkipperDataSet.SensorFileRow)ds.SensorFile.Rows[0], start + new TimeSpan(180000), 80, 80);
            //ds.SensorReadings.AddSensorReadingsRow((SkipperDataSet.SensorFileRow)ds.SensorFile.Rows[0], start + new TimeSpan(240000), 90, 90);
            //ds.BoatFile.AddBoatFileRow((SkipperDataSet.BoatRow)ds.Boat.Rows[0], (SkipperDataSet.SensorFileRow)ds.SensorFile.Rows[0]);

            //ds.Race.AddRaceRow("Work to Home", (SkipperDataSet.LakeRow)ds.Lake.Rows[0], (SkipperDataSet.CourseRow)ds.Course.Rows[0], start, start + new TimeSpan(240000));

            //ds.RaceBoat.AddRaceBoatRow((SkipperDataSet.RaceRow)ds.Race.Rows[0], (SkipperDataSet.BoatRow)ds.Boat.Rows[0]);
            
            _data=ds;
        }
        public static void LoadFromFile()
        {
            LoadFromFile(_defaultPath);
        }
        public static void LoadFromFile(string path)
        {
            if (path.ToLower().EndsWith(".xml"))
            {
                SkipperDataSet sds = new SkipperDataSet();
                sds.ReadXml(path);
                _data = sds;
            }
            else if (path.ToLower().EndsWith(".sail"))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider(); 
                FileStream fs = new FileStream(path, FileMode.Open);
                CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Read);
                GZipStream gzs = new GZipStream(cs, CompressionMode.Decompress);
                SkipperDataSet sds = new SkipperDataSet();
                sds.ReadXml(gzs);
                gzs.Close();
                cs.Close();
                fs.Close();
                _data = sds;
            }
        }
        public static bool SaveToFile()
        {
            return SaveToFile(_defaultPath);
        }
        public static bool SaveNeeded
        {
            get
            {
                return _data.HasChanges();
            }
        }
        public static bool SaveToFile(string path)
        {
            try
            {
                if (path.ToLower().EndsWith(".xml"))
                {
                    //_data.WriteXmlSchema("SkipperDataSet.xsd");
                    _data.WriteXml(path/*, System.Data.XmlWriteMode.WriteSchema*/);
                    return true;
                }
                else
                {
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    FileStream fs = new FileStream(path, FileMode.Create);
                    CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
                    GZipStream gzs = new GZipStream(cs, CompressionMode.Compress);
                    _data.WriteXml(gzs);
                    gzs.Flush();
                    gzs.Close();
                    cs.Flush();
                    cs.Close();
                    fs.Flush();
                    fs.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static SkipperDataSet Data
        {
            get
            {
                return _data;
            }
        }
        public static void UnloadFile()
        {
            _data = null;
        }
    }
}
