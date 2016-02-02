using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AmphibianSoftware.VisualSail.Data
{
    public class Bouy
    {
        private int _id;
        private int _markId;
        private Coordinate _latitude;
        private Coordinate _longitude;

        private bool _new;
        private bool _changed;

        public Bouy()
        {
            _new = true;
            _changed = false;
        }
        private Bouy(SkipperDataSet.BouyRow row)
        {
            _id = row.id;
            _markId = row.mark_id;
            _latitude = new Coordinate(row.latitude);
            _longitude = new Coordinate(row.longitude);

            _new = false;
            _changed = false;
        }
        public Bouy(Mark mark,Coordinate latitude,Coordinate longitude)
        {
            _markId = mark.Id;
            _latitude = latitude;
            _longitude = longitude;

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
            Persistance.Data.Bouy.AddBouyRow(this.Mark.Row,_latitude.Value,_longitude.Value);
            _id = ((SkipperDataSet.BouyRow)Persistance.Data.Bouy.Rows[Persistance.Data.Bouy.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.mark_id = _markId;
                Row.latitude = _latitude.Value;
                Row.longitude = _longitude.Value;
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
        private static SkipperDataSet.BouyRow FindRowById(int id)
        {
            return (from r in Persistance.Data.Bouy.AsEnumerable()
                    where (int)r["id"] == id
                    select r).First();
        }
        public static Bouy FindById(int id)
        {
            return new Bouy(FindRowById(id));
        }
        public static List<Bouy> FindAllByMark(Mark mark)
        {
            var query = from r in Persistance.Data.Bouy.AsEnumerable()
                        where (int)r["mark_id"] == mark.Id
                        select r;

            List<Bouy> bouys = new List<Bouy>();
            foreach (SkipperDataSet.BouyRow rr in query)
            {
                bouys.Add(new Bouy(rr));
            }
            return bouys;
        }
        public SkipperDataSet.BouyRow Row
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
        public Coordinate Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
                _changed = true;
            }
        }
        public Coordinate Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
                _changed = true;
            }
        }
        public CoordinatePoint CoordinatePoint
        {
            get
            {
                return new CoordinatePoint(Latitude, Longitude, 0);
            }
        }
        public Mark Mark
        {
            get
            {
                if (_markId != 0)
                {
                    return Mark.FindById(_markId);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _markId = value.Id;
                _changed = true;
            }
        }
        public override string ToString()
        {
            return Mark.Name + " Bouy";
        }
    }
}
