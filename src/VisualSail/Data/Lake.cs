using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AmphibianSoftware.VisualSail.Data
{
    public class Lake
    {
        private int _id;
        private string _name;
        private double _north;
        private double _south;
        private double _east;
        private double _west;
        private double _altitude;
        private string _heightMap;
        private TimeZoneInfo _timezone;

        private bool _new;
        private bool _changed;

        public Lake()
        {
            _new = true;
            _changed = false;
            _timezone = TimeZoneInfo.Local;
        }
        private Lake(SkipperDataSet.LakeRow row)
        {
            _id = row.id;
            _name = row.name;
            _north = row.north;
            _south = row.south;
            _east = row.east;
            _west = row.west;
            _altitude = row.altitude;
            _heightMap = row.heightmap;
            try
            {
                _timezone = FindTimeZoneInfoByNameString(row.timezone);
            }
            catch (TimeZoneNotFoundException)
            {
                _timezone = TimeZoneInfo.Local;
            }
            _new = false;
            _changed = false;
        }
        public Lake(string name, double north,double south,double east,double west, double altitude,string heightMap,TimeZoneInfo tzi)
        {
            _name = name;
            _north = north;
            _south = south;
            _east = east;
            _west = west;
            _altitude = altitude;
            _heightMap = heightMap;
            _timezone = tzi;
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
        private TimeZoneInfo FindTimeZoneInfoByNameString(string name)
        {
            foreach (TimeZoneInfo tzi in TimeZoneInfo.GetSystemTimeZones())
            {
                if (tzi.DisplayName == name || tzi.Id==name)
                {
                    return tzi;
                }
            }
            throw new TimeZoneNotFoundException();
        }
        private void Insert()
        {
            Persistance.Data.Lake.AddLakeRow(_name,_north,_south,_east,_west,_altitude,_heightMap,_timezone.Id);
            _id = ((SkipperDataSet.LakeRow)Persistance.Data.Lake.Rows[Persistance.Data.Lake.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.name = _name;
                Row.north = _north;
                Row.south = _south;
                Row.east = _east;
                Row.west = _west;
                Row.altitude = _altitude;
                Row.heightmap = _heightMap;
                Row.timezone = _timezone.Id;
                
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
        private static SkipperDataSet.LakeRow FindRowById(int id)
        {
            return (from r in Persistance.Data.Lake.AsEnumerable()
                    where (int)r["id"] == id
                    select r).First();
        }
        public static Lake FindById(int id)
        {
            return new Lake(FindRowById(id));
        }
        public static List<Lake> FindByBoundingBox(double minLat, double maxLat, double minLon, double maxLon)
        {
            var query = from r in Persistance.Data.Lake.AsEnumerable()
                        where ((SkipperDataSet.LakeRow)r).west <= minLon && ((SkipperDataSet.LakeRow)r).east >= maxLon && ((SkipperDataSet.LakeRow)r).north >= maxLat && ((SkipperDataSet.LakeRow)r).south<=minLat
                        select r;

            List<Lake> lakes = new List<Lake>();
            foreach (SkipperDataSet.LakeRow rr in query)
            {
                lakes.Add(new Lake(rr));
            }
            return lakes;
        }
        public static List<Lake> FindAll()
        {
            var query = from r in Persistance.Data.Lake.AsEnumerable()
                        select r;

            List<Lake> lakes = new List<Lake>();
            foreach (SkipperDataSet.LakeRow rr in query)
            {
                lakes.Add(new Lake(rr));
            }
            return lakes;
        }
        public SkipperDataSet.LakeRow Row
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
        public double North
        {
            get
            {
                return _north;
            }
            set
            {
                _north = value;
                _changed = true;
            }
        }
        public double South
        {
            get
            {
                return _south;
            }
            set
            {
                _south = value;
                _changed = true;
            }
        }
        public double East
        {
            get
            {
                return _east;
            }
            set
            {
                _east = value;
                _changed = true;
            }
        }
        public double West
        {
            get
            {
                return _west;
            }
            set
            {
                _west = value;
                _changed = true;
            }
        }
        public double Altitude
        {
            get
            {
                return _altitude;
            }
            set
            {
                _altitude = value;
                _changed = true;
            }
        }
        public string HeightMap
        {
            get
            {
                return _heightMap;
            }
            set
            {
                _heightMap = value;
                _changed = true;
            }
        }
        public TimeZoneInfo TimeZone
        {
            get
            {
                return _timezone;
            }
            set
            {
                _timezone = value;
                _changed = true;
            }
        }
        public ProjectedPoint TopLeftInMeters
        {
            get
            {
                CoordinatePoint a = new CoordinatePoint(new Coordinate(_north), new Coordinate(_west), 0);
                return a.Project();
            }
        }
        public ProjectedPoint TopRightInMeters
        {
            get
            {
                CoordinatePoint a = new CoordinatePoint(new Coordinate(_north), new Coordinate(_east), 0);
                return a.Project();
            }
        }
        public ProjectedPoint BottomRightInMeters
        {
            get
            {
                CoordinatePoint a = new CoordinatePoint(new Coordinate(_south), new Coordinate(_east), 0);
                return a.Project();
            }
        }
        public ProjectedPoint BottomLeftInMeters
        {
            get
            {
                CoordinatePoint a = new CoordinatePoint(new Coordinate(_south), new Coordinate(_west), 0);
                return a.Project();
            }
        }
        public double WidthInMeters
        {
            get
            {
                CoordinatePoint a = new CoordinatePoint(new Coordinate(_north), new Coordinate(_west), 0);
                ProjectedPoint pa = a.Project();
                CoordinatePoint b = new CoordinatePoint(new Coordinate(_north), new Coordinate(_east), 0);
                ProjectedPoint pb = b.Project();
                return CoordinatePoint.TwoDimensionalDistance(pa.Easting, pa.Northing, pb.Easting, pb.Northing);
            }
        }
        public double HeightInMeters
        {
            get
            {
                CoordinatePoint a = new CoordinatePoint(new Coordinate(_north), new Coordinate(_west), 0);
                ProjectedPoint pa = a.Project();
                CoordinatePoint b = new CoordinatePoint(new Coordinate(_south), new Coordinate(_west), 0);
                ProjectedPoint pb = b.Project();
                return CoordinatePoint.TwoDimensionalDistance(pa.Easting, pa.Northing, pb.Easting, pb.Northing);
            }
        }
        public override string ToString()
        {
            return _name;
        }
        public static string DefaultName
        {
            get
            {
                return "New Area";
            }
        }
    }
}
