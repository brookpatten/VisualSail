using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AmphibianSoftware.VisualSail.Data
{
    public class Boat
    {
        private int _id;
        private string _name;
        private string _number;
        private int _color;
        private int _boatTypeId;

        private bool _new;
        private bool _changed;

        private DateTime? _gpsDataStart;
        private DateTime? _gpsDataEnd;
        private double? _gpsDataMinLat;
        private double? _gpsDataMaxLat;
        private double? _gpsDataMinLon;
        private double? _gpsDataMaxLon;

        public Boat()
        {
            _new = true;
            _changed = false;
        }
        private Boat(SkipperDataSet.BoatRow row)
        {
            LoadFromRow(row);   
        }
        protected void LoadFromRow(SkipperDataSet.BoatRow row)
        {
            _id = row.id;
            _name = row.name;
            _number = row.number;
            _color = row.color;
            _boatTypeId = row.boattype_id;

            _new = false;
            _changed = false;
        }
        public Boat(string name, string number,int color,BoatType bt)
        {
            _name = name;
            _number = number;
            _color = color;
            _boatTypeId = bt.Id;

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
            Persistance.Data.Boat.AddBoatRow(_name,_number,_color,BoatType.Row);
            _id = ((SkipperDataSet.BoatRow)Persistance.Data.Boat.Rows[Persistance.Data.Boat.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.name = _name;
                Row.number = _number;
                Row.color = _color;
                Row.boattype_id = _boatTypeId;
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
        protected static SkipperDataSet.BoatRow FindRowById(int id)
        {
            return (from r in Persistance.Data.Boat.AsEnumerable()
                    where (int)r["id"] == id
                    select r).First();
        }
        public static Boat FindById(int id)
        {
            return new Boat(FindRowById(id));
        }
        public static List<Boat> FindAll()
        {
            var query = from r in Persistance.Data.Boat.AsEnumerable()
                        select r;

            List<Boat> boats = new List<Boat>();
            foreach (SkipperDataSet.BoatRow rr in query)
            {
                boats.Add(new Boat(rr));
            }
            return boats;
        }
        public List<SensorFile> SensorFiles
        {
            get
            {
                List<SensorFile> files = new List<SensorFile>();
                var query = from r in Persistance.Data.BoatFile.AsEnumerable()
                            where (int)r["boat_id"] == _id
                            select r;
                foreach (SkipperDataSet.BoatFileRow r in query)
                {
                    files.Add(SensorFile.FindById(r.sensorfile_id));
                }
                return files;
            }
        }
        public void AddFile(SensorFile f)
        {
            Persistance.Data.BoatFile.AddBoatFileRow(this.Row, f.Row);
        }
        public SkipperDataSet.SensorReadingsDataTable GetSensorReadings()
        {
            return GetSensorReadings(null, null);
        }
        public SkipperDataSet.SensorReadingsDataTable GetSensorReadings(DateTime? start,DateTime? end)
        {
            SkipperDataSet.SensorReadingsDataTable dt = new SkipperDataSet.SensorReadingsDataTable();
            List<SensorFile> files = new List<SensorFile>();
            var query = from r in Persistance.Data.BoatFile.AsEnumerable()
                        join f in Persistance.Data.SensorReadings.AsEnumerable() on r.sensorfile_id equals f.sensorfile_id
                        where 
                            r.boat_id == _id 
                            && (start==null || f.datetime >=start)
                            && (end == null || f.datetime <= end)
                        orderby f.datetime ascending
                        select f;
            foreach (SkipperDataSet.SensorReadingsRow r in query)
            {
                dt.ImportRow(r);
            }
            return dt;
        }
        public static Boat FromRow(SkipperDataSet.BoatRow row)
        {
            return new Boat(row);
        }
        public SkipperDataSet.BoatRow Row
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
        public string Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                _changed = true;
            }
        }
        public int Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                _changed = true;
            }
        }
        public BoatType BoatType
        {
            get
            {
                return BoatType.FindById(_boatTypeId);
            }
            set
            {
                _boatTypeId = value.Id;
                _changed = true;
            }
        }
        public override string ToString()
        {
            return _number + " "+_name;
        }

        public void RefreshGpsBounds()
        {
            _gpsDataStart = null;
            _gpsDataEnd = null;
            _gpsDataMinLat = null;
            _gpsDataMaxLat = null;
            _gpsDataMinLon = null;
            _gpsDataMaxLon = null;

            foreach (SensorFile sf in SensorFiles)
            {
                foreach (SkipperDataSet.SensorReadingsRow r in sf.SensorReadings)
                {
                    if (_gpsDataStart==null || r.datetime < _gpsDataStart.Value)
                    {
                        _gpsDataStart = r.datetime;
                    }
                    if (_gpsDataEnd == null || r.datetime > _gpsDataEnd.Value)
                    {
                        _gpsDataEnd = r.datetime;
                    }
                    if (_gpsDataMinLat == null || r.latitude < _gpsDataMinLat.Value)
                    {
                        _gpsDataMinLat = r.latitude;
                    }
                    if (_gpsDataMaxLat == null || r.latitude > _gpsDataMaxLat.Value)
                    {
                        _gpsDataMaxLat = r.latitude;
                    }
                    if (_gpsDataMinLon == null || r.longitude < _gpsDataMinLon.Value)
                    {
                        _gpsDataMinLon = r.longitude;
                    }
                    if (_gpsDataMaxLon == null || r.longitude > _gpsDataMaxLon.Value)
                    {
                        _gpsDataMaxLon = r.longitude;
                    }
                }
            }
        }

        public DateTime? GpsDataStart
        {
            get
            {
                return _gpsDataStart;
            }
        }

        public DateTime? GpsDataEnd
        {
            get
            {
                return _gpsDataEnd;
            }
        }

        public double? GpsMinimumLatitude
        {
            get
            {
                return _gpsDataMinLat;
            }
        }

        public double? GpsMaximumLatitude
        {
            get
            {
                return _gpsDataMaxLat;
            }
        }

        public double? GpsMinimumLongitude
        {
            get
            {
                return _gpsDataMinLon;
            }
        }

        public double? GpsMaximumLongitude
        {
            get
            {
                return _gpsDataMaxLon;
            }
        }
    }
}
