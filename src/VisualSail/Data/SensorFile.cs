using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmphibianSoftware.VisualSail.Data
{
    public class SensorFile
    {
        private int _id;
        private string _type;
        private string _name;
        private DateTime _loaded;

        private bool _new;
        private bool _changed;

        public SensorFile()
        {
            _new = true;
            _changed = false;
        }
        private SensorFile(SkipperDataSet.SensorFileRow row)
        {
            _id = row.id;
            _type = row.type;
            _name = row.name;
            _loaded = row.loaded;
            
            _new = false;
            _changed = false;
        }
        public SensorFile(string type,string name,DateTime loaded)
        {
            _type = type;
            _name = name;
            _loaded = loaded;

            _new = true;
            _changed = true;
        }

        public void Save()
        {
            if (_new && _changed)
            {
                Insert();
                _new = false;
                _changed = false;
            }
            else if (!_new && _changed)
            {
                Update();
                _changed = false;
            }
        }
        private void Insert()
        {
            Persistance.Data.SensorFile.AddSensorFileRow(_type, _name, _loaded);
            _id = ((SkipperDataSet.SensorFileRow)Persistance.Data.SensorFile.Rows[Persistance.Data.SensorFile.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.type = _type;
                Row.name = _name;
                Row.loaded = _loaded;
                Row.EndEdit();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Row.CancelEdit();
            }
        }
        public void Delete()
        {
            Row.Delete();
        }
        private static SkipperDataSet.SensorFileRow FindRowById(int id)
        {
            return (from r in Persistance.Data.SensorFile.AsEnumerable()
                    where (int)r["id"] == id
                    select r).First();
        }
        public static SensorFile FindById(int id)
        {
            return new SensorFile(FindRowById(id));
        }
        public static List<SensorFile> FindAll()
        {
            var query = from r in Persistance.Data.SensorFile.AsEnumerable()
                        select r;

            List<SensorFile> files = new List<SensorFile>();
            foreach (SkipperDataSet.SensorFileRow rr in query)
            {
                files.Add(new SensorFile(rr));
            }
            return files;
        }
        public SkipperDataSet.SensorReadingsDataTable SensorReadings
        {
            get
            {
                var query = from r in Persistance.Data.SensorReadings.AsEnumerable()
                            where r.sensorfile_id == _id
                            orderby r.datetime ascending
                            select r;

                SkipperDataSet.SensorReadingsDataTable dt = new SkipperDataSet.SensorReadingsDataTable();
                foreach (SkipperDataSet.SensorReadingsRow rr in query)
                {
                    dt.ImportRow(rr);
                }
                return dt;
            }
        }
        public void AddReading(DateTime time, double latitude, double longitude, double altitude, double speed, double heading, double depth, double temperature, double waterSpeed, double apparantWindDirection, double apparantWindSpeed, double trueWindDirection, double trueWindSpeed)
        {
            Persistance.Data.SensorReadings.AddSensorReadingsRow(this.Row, time, latitude, longitude, altitude, speed, heading, depth, temperature, waterSpeed, apparantWindDirection, apparantWindSpeed, trueWindDirection, trueWindSpeed);
        }
        public void AddReading(SkipperDataSet.SensorReadingsRow row)
        {
            row.SensorFileRow = this.Row;
            Persistance.Data.SensorReadings.AddSensorReadingsRow(row);
        }
        public SkipperDataSet.SensorFileRow Row
        {
            get
            {
                return FindRowById(_id);
            }
        }
        public int Id
        {
            get
            {
                return _id;
            }
        }
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                _changed = true;
            }
        }
        public DateTime Loaded
        {
            get
            {
                return _loaded;
            }
            set
            {
                _loaded = value;
            }
        }
        public override string ToString()
        {
            return _name; //+ _loaded.ToShortDateString() + " " + _loaded.ToShortTimeString();
        }
    }
}