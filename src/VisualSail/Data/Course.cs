using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AmphibianSoftware.VisualSail.PostBuild;

namespace AmphibianSoftware.VisualSail.Data
{
    public class Course
    {
        private int _id;
        private string _name;
        private int _lakeId;
        private DateTime _date;
        [DoNotObfuscate()]
        public enum WindDirectionType {ConstantCourse,ConstantManual,DynamicSensor};
        private WindDirectionType _directionType;
        private double _manualWindDirection;
        private int _windDirectionFromMarkId;
        private int _windDirectionToMarkId;
        private List<Mark> _routeCache;

        private bool _new;
        private bool _changed;

        public Course()
        {
            _new = true;
            _changed = false;
        }
        private Course(SkipperDataSet.CourseRow row)
        {
            _id = row.id;
            _name = row.name;
            _lakeId = row.lake_id;
            _date = row.date;
            try
            {
                if (row.wind_direction_type == "ConstantCourse")
                {
                    _directionType = WindDirectionType.ConstantCourse;
                    _windDirectionFromMarkId = row.wind_direction_from_mark_id;
                    _windDirectionToMarkId = row.wind_direction_to_mark_id;
                }
                else if (row.wind_direction_type == "ConstantManual")
                {
                    _directionType = WindDirectionType.ConstantManual;
                    _manualWindDirection = row.manual_wind_direction;
                }
                else if (row.wind_direction_type == "DynamicSensor")
                {
                    _directionType = WindDirectionType.DynamicSensor;
                    throw new Exception("Dynamic Wind Sensors are not supported... yet.");
                }
                else if (row.wind_direction_type == null || row.wind_direction_type == "")
                {
                    _directionType = WindDirectionType.ConstantManual;
                    _manualWindDirection = 0.0;
                }
                else
                {
                    throw new Exception("Unknown Wind Direction Type");
                }
            }
            catch//(Exception e)
            {
                _directionType = WindDirectionType.ConstantManual;
                _manualWindDirection = 0.0;
            }
            _new = false;
            _changed = false;
        }
        public Course(string name,DateTime date, Lake lake)
        {
            _name = name;
            _date = date;
            _lakeId = lake.Id;
            _directionType = WindDirectionType.ConstantManual;
            _manualWindDirection = 0.0;
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
        public WindDirectionType DirectionType
        {
            get
            {
                return _directionType;
            }
        }
        private void Insert()
        {
            Persistance.Data.Course.AddCourseRow(_name, _date, this.Lake.Row,_directionType.ToString(),_manualWindDirection,_windDirectionFromMarkId,_windDirectionToMarkId);
            _id = ((SkipperDataSet.CourseRow)Persistance.Data.Course.Rows[Persistance.Data.Course.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.name = _name;
                Row.lake_id = _lakeId;
                Row.date = _date;
                Row.wind_direction_type = _directionType.ToString();
                Row.manual_wind_direction = _manualWindDirection;
                Row.wind_direction_from_mark_id = _windDirectionFromMarkId;
                Row.wind_direction_to_mark_id = _windDirectionToMarkId;
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
        private static SkipperDataSet.CourseRow FindRowById(int id)
        {
            return (from r in Persistance.Data.Course.AsEnumerable()
                    where (int)r["id"] == id
                    select r).First();
        }
        public static List<Course> FindAllByLake(Lake lake)
        {
            var query = from r in Persistance.Data.Course.AsEnumerable()
                        where (int)r["lake_id"] == lake.Id
                        select r;

            List<Course> courses = new List<Course>();
            foreach (SkipperDataSet.CourseRow rr in query)
            {
                courses.Add(new Course(rr));
            }
            return courses;
        }
        public static Course FindById(int id)
        {
            return new Course(FindRowById(id));
        }
        public SkipperDataSet.CourseRow Row
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
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                _changed = true;
            }
        }
        public List<Mark> Route
        {
            get
            {
                if (_routeCache == null)
                {
                    var query = from r in Persistance.Data.CourseMarkOrder.AsEnumerable()
                                where r.course_id == _id
                                orderby r.order ascending
                                select r;

                    List<Mark> marks = new List<Mark>();
                    foreach (SkipperDataSet.CourseMarkOrderRow cmo in query)
                    {
                        marks.Add(Mark.FindById(cmo.mark_id));
                    }
                    _routeCache = marks;
                }
                return _routeCache;
            }
            set
            {
                _routeCache = value;
                List<SkipperDataSet.CourseMarkOrderRow> rows = new List<SkipperDataSet.CourseMarkOrderRow>();
                var query = from r in Persistance.Data.CourseMarkOrder.AsEnumerable()
                            where r.course_id == _id
                            orderby r.order ascending
                            select r;
                foreach (SkipperDataSet.CourseMarkOrderRow cmo in query)
                {
                    rows.Add(cmo);
                }
                foreach (SkipperDataSet.CourseMarkOrderRow cmo in rows)
                {
                    cmo.Delete();
                }

                int order = 1;
                foreach (Mark m in value)
                {
                    Persistance.Data.CourseMarkOrder.AddCourseMarkOrderRow(this.Row, m.Row, order);
                    order++;
                }
            }
        }
        public Lake Lake
        {
            get
            {
                if (_lakeId != 0)
                {
                    return Lake.FindById(_lakeId);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _lakeId = value.Id;
                _changed = true;
            }
        }
        public double ManualWindAngle
        {
            get
            {
                return _manualWindDirection;
            }
            set
            {
                _directionType = WindDirectionType.ConstantManual;
                _manualWindDirection = value;
                _changed = true;
                _windDirectionFromMarkId = 0;
                _windDirectionToMarkId = 0;
            }
        }
        public Mark WindFromMark
        {
            get
            {
                if (_windDirectionFromMarkId != 0)
                {
                    return Mark.FindById(_windDirectionFromMarkId);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _directionType = WindDirectionType.ConstantCourse;
                _windDirectionFromMarkId = value.Id;
                _changed = true;
                _manualWindDirection = 0;
            }
        }
        public Mark WindToMark
        {
            get
            {
                if (_windDirectionToMarkId != 0)
                {
                    return Mark.FindById(_windDirectionToMarkId);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _directionType = WindDirectionType.ConstantCourse;
                _windDirectionToMarkId = value.Id;
                _changed = true;
                _manualWindDirection = 0;
            }
        }
        public override string ToString()
        {
            return _name;
        }
        public List<Mark> Marks
        {
            get
            {
                return Mark.FindAllByCourse(this);
            }
        }
        public bool IsValidRaceCourse
        {
            get
            {
                //make sure we have at least 2 marks, and at least 2 route points
                return Marks.Count >= 2 && Route.Count >= 2;
            }
        }
    }
}
