using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AmphibianSoftware.VisualSail.Data
{
    public class BoatType
    {
        private int _id;
        private string _name;
        
        private bool _new;
        private bool _changed;

        public BoatType()
        {
            _new = true;
            _changed = false;
        }
        private BoatType(SkipperDataSet.BoatTypeRow row)
        {
            _id = row.id;
            _name = row.name;
            
            _new = false;
            _changed = false;
        }
        public BoatType(string name)
        {
            _name = name;
            
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
            Persistance.Data.BoatType.AddBoatTypeRow(_name);
            _id = ((SkipperDataSet.BoatTypeRow)Persistance.Data.BoatType.Rows[Persistance.Data.BoatType.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.name = _name;
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
        private static SkipperDataSet.BoatTypeRow FindRowById(int id)
        {
            return (from r in Persistance.Data.BoatType.AsEnumerable()
                    where (int)r["id"] == id
                    select r).First();
        }
        public static BoatType FindById(int id)
        {
            return new BoatType(FindRowById(id));
        }
        public SkipperDataSet.BoatTypeRow Row
        {
            get
            {
                return FindRowById(_id);
            }
        }
        public static List<BoatType> FindAll()
        {
            var query = from r in Persistance.Data.BoatType.AsEnumerable()
                        select r;

            List<BoatType> types = new List<BoatType>();
            foreach (SkipperDataSet.BoatTypeRow rr in query)
            {
                types.Add(new BoatType(rr));
            }
            return types;
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
        public override string ToString()
        {
            return _name;
        }
    }
}
