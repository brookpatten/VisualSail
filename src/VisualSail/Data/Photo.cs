using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmphibianSoftware.VisualSail.Data
{
    public class Photo
    {
        private int _id;
        private string _name;
        private string _caption;
        private DateTime _time;
        private byte[] _jpg;
        private List<Boat> _boats;

        private bool _new;
        private bool _changed;

        private Photo(SkipperDataSet.PhotoRow row)
        {
            _id=row.id;
            _name=row.name;
            _caption=row.caption;
            _time=row.time;
            _jpg=row.jpg;

            _new = false;
            _changed = false;
        }
        public Photo(string name, string caption, DateTime time, byte[] jpg)
        {
            _name=name;
            _caption=caption;
            _time=time;
            _jpg=jpg;

            _new = true;
            _changed = true;
        }
        public Photo()
        {
            _new = true;
            _changed = false;
        }
        public void Save()
        {
            if (_new && _changed)
            {
                Insert();
                SaveBoats();
                _new = false;
                _changed = false;
            }
            else if (!_new && _changed)
            {
                Update();
                SaveBoats();
                _changed = false;
            }
        }
        private void Insert()
        {
            Persistance.Data.Photo.AddPhotoRow(_name, _caption, _time, _jpg);
            _id = ((SkipperDataSet.PhotoRow)Persistance.Data.Photo.Rows[Persistance.Data.Photo.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.name = _name;
                Row.caption = _caption;
                Row.time = _time;
                Row.jpg = _jpg;
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
            if (!_new)
            {
                Boats = new List<Boat>();
                Save();
                Row.Delete();
            }
        }
        private static SkipperDataSet.PhotoRow FindRowById(int id)
        {
            return (from r in Persistance.Data.Photo.AsEnumerable()
                    where (int)r["id"] == id
                    select r).First();
        }
        public static Photo FindById(int id)
        {
            return new Photo(FindRowById(id));
        }
        public static Photo FromRow(SkipperDataSet.PhotoRow row)
        {
            return new Photo(row);
        }
        public SkipperDataSet.PhotoRow Row
        {
            get
            {
                return FindRowById(_id);
            }
        }
        public override string ToString()
        {
            if (_caption != null && _caption != string.Empty)
            {
                return _caption;
            }
            else
            {
                return _name;
            }
        }
        public List<Boat> Boats
        {
            get
            {
                if (_boats == null)
                {
                    _boats = new List<Boat>();
                    var q = from bp in Persistance.Data.BoatPhoto.AsEnumerable()
                            where bp.photo_id==_id
                            join b in Persistance.Data.Boat.AsEnumerable() on bp.boat_id equals b.id
                            select b;

                    foreach (SkipperDataSet.BoatRow b in q)
                    {
                        _boats.Add(Boat.FromRow(b));
                    }
                }
                return _boats;
            }
            set
            {
                _boats = value;
                _changed = true;
            }
        }
        public static List<Photo> FindAll()
        {
            var query = from r in Persistance.Data.Photo.AsEnumerable()
                        orderby r.time ascending
                        select r;
                        

            List<Photo> photos = new List<Photo>();
            foreach (SkipperDataSet.PhotoRow rr in query)
            {
                photos.Add(new Photo(rr));
            }
            return photos;
        }
        public static Dictionary<PhotoIndex, Photo> FindInDateRange(DateTime start, DateTime end)
        {
            var query = from r in Persistance.Data.Photo.AsEnumerable()
                        where r.time > start && r.time < end
                        orderby r.time ascending
                        select r;
            Dictionary<PhotoIndex, Photo> photos = new Dictionary<PhotoIndex, Photo>();
            foreach (SkipperDataSet.PhotoRow rr in query)
            {
                Photo p = new Photo(rr);
                photos.Add(new PhotoIndex(p), p);
            }
            return photos;
        }
        private void SaveBoats()
        {
            var q = from bp in Persistance.Data.BoatPhoto.AsEnumerable()
                    where bp.photo_id == _id
                    select bp;

            for (int i = 0; i < q.Count(); i++)
            {
                q.ElementAt(i).Delete();
            }

            foreach (Boat b in this.Boats)
            {
                Persistance.Data.BoatPhoto.AddBoatPhotoRow(b.Row, this.Row);
            }
        }
        public int ID
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
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                _changed = true;
            }
        }
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                _changed = true;
            }
        }
        public byte[] Jpg
        {
            get
            {
                return _jpg;
            }
            set
            {
                _jpg = value;
                _changed = true;
            }
        }
    }
    public class PhotoIndex
    {
        public DateTime Time;
        public List<AmphibianSoftware.VisualSail.Data.Boat> Boats;
        public PhotoIndex(Photo p)
        {
            Time = p.Time;
            Boats = p.Boats;
        }
        public bool ContainsAnyIds(List<int> ids)
        {
            return Boats.FindAll(p => ids.Contains(p.Id)).Count() > 0;
        }
    }
}
