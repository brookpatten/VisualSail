using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Microsoft.Xna.Framework;

using AmphibianSoftware.VisualSail.Library;

namespace AmphibianSoftware.VisualSail.Data
{
    public class Mark
    {
        private int _id;
        private string _name;
        private string _markType;
        private int _courseId;
        
        private bool _new;
        private bool _changed;

        public Mark()
        {
            _new = true;
            _changed = false;
        }
        private Mark(SkipperDataSet.MarkRow row)
        {
            _id = row.id;
            _name = row.name;
            _markType=row.marktype;
            _courseId=row.course_id;
            
            _new = false;
            _changed = false;
        }
        public Mark(string name, string markType, Course course)
        {
            _name = name;
            _markType=markType;
            _courseId = course.Id;
            
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
            Persistance.Data.Mark.AddMarkRow(_name, _markType,this.Course.Row);
            _id = ((SkipperDataSet.MarkRow)Persistance.Data.Mark.Rows[Persistance.Data.Mark.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.name = _name;
                Row.marktype = _markType;
                Row.course_id = _courseId;
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
        private static SkipperDataSet.MarkRow FindRowById(int id)
        {
            return (from r in Persistance.Data.Mark.AsEnumerable()
                    where (int)r["id"] == id
                    select r).First();
        }
        public static List<Mark> FindAllByCourse(Course course)
        {
            var query = from r in Persistance.Data.Mark.AsEnumerable()
                        where (int)r["course_id"] == course.Id
                        select r;

            List<Mark> marks = new List<Mark>();
            foreach (SkipperDataSet.MarkRow rr in query)
            {
                marks.Add(new Mark(rr));
            }
            return marks;
        }
        public static Mark FindById(int id)
        {
            return new Mark(FindRowById(id));
        }
        public SkipperDataSet.MarkRow Row
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
        public string MarkType
        {
            get
            {
                return _markType;
            }
            set
            {
                _markType = value;
                _changed = true;
            }
        }
        public Course Course
        {
            get
            {
                if (_courseId != 0)
                {
                    return Course.FindById(_courseId);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _courseId = value.Id;
                _changed = true;
            }
        }
        public List<Bouy> Bouys
        {
            get
            {
                if (_id != 0)
                {
                    return Bouy.FindAllByMark(this);
                }
                else
                {
                    return new List<Bouy>();
                }
            }
        }
        public CoordinatePoint AveragedLocation
        {
            get
            {
                double lat = Bouys[0].Latitude.Value;
                double lon = Bouys[0].Longitude.Value;
                for (int i = 1; i < Bouys.Count; i++)
                {
                    lat = ((lat * (double)i) + Bouys[i].Latitude.Value) / (double)(i + 1);
                    lon = ((lon * (double)i) + Bouys[i].Longitude.Value) / (double)(i + 1);
                }
                return new CoordinatePoint(new Coordinate(lat), new Coordinate(lon),0);
            }
        }
        public double DistanceTo(ProjectedPoint point)
        {
            if (Bouys.Count == 1)
            {
                CoordinatePoint bp = new CoordinatePoint(Bouys[0].Latitude, Bouys[0].Longitude, 0);
                return CoordinatePoint.TwoDimensionalDistance(point.Easting, point.Northing, bp.Project().Easting, bp.Project().Northing);
            }
            else if (Bouys.Count == 2)
            {
                CoordinatePoint a = new CoordinatePoint(Bouys[0].Latitude, Bouys[0].Longitude, 0);
                CoordinatePoint b = new CoordinatePoint(Bouys[1].Latitude, Bouys[1].Longitude, 0);
                return GeometryHelper.DistancePointToLineSegment(a.Project().Easting, a.Project().Northing, b.Project().Easting, b.Project().Northing, point.Easting, point.Northing);
            }
            else
            {
                throw new Exception("Not Implemented");
            }
        }
        public ProjectedPoint FindMarkRoundPoint(Mark previousMark, Mark nextMark)
        {
            ProjectedPoint myA = AveragedLocation.Project();

            float previousAngle = (float)AngleHelper.FindAngleWTF(previousMark.AveragedLocation.Project(), myA);
            previousAngle = AngleHelper.NormalizeAngle(-previousAngle);

            float nextAngle = (float)AngleHelper.FindAngleWTF(nextMark.AveragedLocation.Project(), myA);
            nextAngle = AngleHelper.NormalizeAngle(-nextAngle);
            
            float roundingAngle = AngleHelper.FindHalfwayCounterClockwiseAngle(previousAngle, nextAngle);

            ProjectedPoint pp=AngleHelper.PolarToRectangular(myA,(double)roundingAngle,1000.0);
            return pp;
        }
        public bool IsRounding(ProjectedPoint a, ProjectedPoint b, Mark previousMark,Mark nextMark)
        {
            if (Bouys.Count == 1)
            {
                
                if (previousMark != null && nextMark != null)
                {
                    //I know this is WAY more complicated than it needs to be, but it works
                    //I'll clean it later... maybe after I got back to college and take trig again.

                    Vector2 a2 = new Vector2((float)a.Easting, (float)a.Northing);
                    Vector2 b2 = new Vector2((float)b.Easting, (float)b.Northing);

                    ProjectedPoint myA = Bouys[0].CoordinatePoint.Project();
                    Vector2 myA2 = new Vector2((float)myA.Easting, (float)myA.Northing);

                    ProjectedPoint myB=FindMarkRoundPoint(previousMark, nextMark);
                    Vector2 myB2 = new Vector2((float)myB.Easting, (float)myB.Northing); ;

                    Vector2 intersect;
                    return GeometryHelper.TryFindIntersection(a2,b2,myA2,myB2, out intersect);
                }
                else
                {
                    float markRoundDistance = 20f;
                    return DistanceTo(a) <= markRoundDistance;
                }
            }
            else if (Bouys.Count == 2)
            {
                ProjectedPoint myA = Bouys[0].CoordinatePoint.Project();
                ProjectedPoint myB = Bouys[1].CoordinatePoint.Project();

                //Vector3 aV = a.ToWorld();
                //Vector3 bV = b.ToWorld();
                //Vector3 myAV = myA.ToWorld();
                //Vector3 myBV = myB.ToWorld();

                Vector2 intersect;
                return GeometryHelper.TryFindIntersection(new Vector2((float)a.Easting, (float)a.Northing), new Vector2((float)b.Easting, (float)b.Northing), new Vector2((float)myA.Easting, (float)myA.Northing), new Vector2((float)myB.Easting, (float)myB.Northing), out intersect);
            }
            else
            {
                throw new Exception("Not Implemented");
            }
        }
        public override string ToString()
        {
            return _name;
        }
    }
}
