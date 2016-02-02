using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmphibianSoftware.VisualSail.Data
{
    public class Bookmark
    {
        private int _id;
        private string _name;
        private DateTime _time;
        
        private bool _new;
        private bool _changed;

        private Bookmark(SkipperDataSet.BookmarkRow row)
        {
            _id=row.id;
            _name=row.name;
            _time=row.time;
            
            _new = false;
            _changed = false;
        }
        public Bookmark(string name,DateTime time)
        {
            _name=name;
            _time=time;
            
            _new = true;
            _changed = true;
        }
        public Bookmark()
        {
            _new = true;
            _changed = false;
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
            Persistance.Data.Bookmark.AddBookmarkRow(_time,_name);
            _id = ((SkipperDataSet.BookmarkRow)Persistance.Data.Bookmark.Rows[Persistance.Data.Bookmark.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.name = _name;
                Row.time = _time;
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
                Row.Delete();
            }
        }
        private static SkipperDataSet.BookmarkRow FindRowById(int id)
        {
            return (from r in Persistance.Data.Bookmark.AsEnumerable()
                    where (int)r["id"] == id
                    select r).First();
        }
        public static Bookmark FindById(int id)
        {
            return new Bookmark(FindRowById(id));
        }
        public static Bookmark FromRow(SkipperDataSet.BookmarkRow row)
        {
            return new Bookmark(row);
        }
        public SkipperDataSet.BookmarkRow Row
        {
            get
            {
                return FindRowById(_id);
            }
        }
        public override string ToString()
        {
            if (_name != null && _name != string.Empty)
            {
                return _name;
            }
            else
            {
                return _time.ToLongTimeString();
            }
        }
        public static List<Bookmark> FindAll()
        {
            var query = from r in Persistance.Data.Bookmark.AsEnumerable()
                        orderby r.time ascending
                        select r;


            List<Bookmark> bookmarks = new List<Bookmark>();
            foreach (SkipperDataSet.BookmarkRow rr in query)
            {
                bookmarks.Add(new Bookmark(rr));
            }
            return bookmarks;
        }
        public static List<Bookmark> FindInDateRange(DateTime start, DateTime end)
        {
            var query = from r in Persistance.Data.Bookmark.AsEnumerable()
                        where r.time >= start && r.time <= end
                        orderby r.time ascending
                        select r;
            List<Bookmark> bookmarks = new List<Bookmark>();
            foreach (SkipperDataSet.BookmarkRow b in query)
            {
                bookmarks.Add(new Bookmark(b));
            }
            return bookmarks;
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
    }
}