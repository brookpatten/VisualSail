using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.UI.Controls;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class EditCourses : Form
    {
        private Point _zoomFirstPoint;
        private Point _zoomSecondPoint;
        private bool _zoomed = false;
        private bool _mouseDown = false;
        private Bouy _selectedBouy;
        private Course _course;
        private Lake _lake;
        private Lake _zoomedLake;
        private Image _image;
        private List<List<ColoredCoordinatePoint>> _paths;

        private Image _cachedPanel;
        private Image _cachedPanelWithBoats;
        
        public EditCourses(Race race)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            LoadPaths(race);
            _lake = race.Lake;
            try
            {
                _image = AmphibianSoftware.VisualSail.Library.SatelliteImageryHelper.GetImageForLake(_lake);
            }
            catch (Exception)
            {
                _image = null;
            }
            
            _course = race.Course;
            LoadCourse();
        }

        private void LoadPaths(Race race)
        {
            DateTime start = race.UtcStart;
            DateTime end = race.UtcEnd;
            _paths = new List<List<ColoredCoordinatePoint>>();
            foreach (Boat b in race.Boats)
            {
                List<ColoredCoordinatePoint> points = new List<ColoredCoordinatePoint>();
                Color c = Color.FromArgb(b.Color);
                SkipperDataSet.SensorReadingsDataTable dt = b.GetSensorReadings(start, end);
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    CoordinatePoint cp = new CoordinatePoint(new Coordinate(((SkipperDataSet.SensorReadingsRow)dt.Rows[i]).latitude), new Coordinate(((SkipperDataSet.SensorReadingsRow)dt.Rows[i]).longitude), ((SkipperDataSet.SensorReadingsRow)dt.Rows[i]).altitude);
                    points.Add(new ColoredCoordinatePoint(cp, c));
                }
                _paths.Add(points);
            }
        }
        
        private void lakePNL_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            if (zoomInCB.Checked)
            {
                _zoomSecondPoint = new Point(e.X, e.Y);
                CoordinatePoint a = ScreenToCoordinates(_zoomFirstPoint);
                CoordinatePoint b = ScreenToCoordinates(_zoomSecondPoint);

                double minLat;
                double maxLat;
                double minLon;
                double maxLon;

                if (a.Latitude.Value < b.Latitude.Value)
                {
                    minLat = a.Latitude.Value;
                    maxLat = b.Latitude.Value;
                }
                else
                {
                    maxLat = a.Latitude.Value;
                    minLat = b.Latitude.Value;
                }

                if (a.Longitude.Value < b.Longitude.Value)
                {
                    minLon = a.Longitude.Value;
                    maxLon = b.Longitude.Value;
                }
                else
                {
                    maxLon = a.Longitude.Value;
                    minLon = b.Longitude.Value;
                }

                _zoomedLake = new Lake(_lake.Name, maxLat, minLat, maxLon,minLon, _lake.Altitude, "", _lake.TimeZone);

                zoomInCB.Checked = false;
                _zoomed = true;
                lakePNL.Cursor = Cursors.Arrow;
            }
            ClearCache();
            lakePNL.Invalidate();
        }

        private void lakePNL_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            if (zoomInCB.Checked)
            {
                _zoomFirstPoint = new Point(e.X, e.Y);
                _zoomSecondPoint = _zoomFirstPoint;
            }
            else
            {
                FindClosestBouy(new Point(e.X, e.Y));
            }
            lakePNL.Invalidate();
        }

        private void lakePNL_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                if (zoomInCB.Checked)
                {
                    _zoomSecondPoint = new Point(e.X, e.Y);
                    lakePNL.Invalidate();
                }
                else if (SelectedMark != null && _selectedBouy != null)
                {
                    Point mp = new Point(e.X, e.Y);
                    CoordinatePoint cp = ScreenToCoordinates(mp);
                    _selectedBouy.Longitude = cp.Longitude;
                    _selectedBouy.Latitude = cp.Latitude;
                    _selectedBouy.Save();
                    lakePNL.Invalidate();
                    mouseCoordsLBL.Text = _selectedBouy.Latitude.ToString() + " " + _selectedBouy.Longitude.ToString();
                }
            }
            else if(!zoomInCB.Checked)
            {
                Bouy b=null;
                if (TryFindCloseBouy(new Point(e.X, e.Y), ref b))
                {
                    lakePNL.Cursor = Cursors.Hand;
                }
                else
                {
                    lakePNL.Cursor = Cursors.Default;
                }
            }
        }

        private bool TryFindCloseBouy(Point p,ref Bouy closest)
        {
            List<Bouy> bouys = new List<Bouy>();
            foreach (object o in marksLB.Items)
            {
                bouys.AddRange(((Mark)o).Bouys);
            }
            double maximumDistance = 10.0;
            double closestDistance = maximumDistance;
            Bouy closestBouy = null;
            foreach (Bouy b in bouys)
            {
                Point onScreen = CoordinatesToScreen(new CoordinatePoint(b.Latitude, b.Longitude, 0));
                double distance = TwoDimensionalDistance(p.X, p.Y, onScreen.X, onScreen.Y);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBouy = b;
                }
            }
            if (closestBouy != null)
            {
                closest = closestBouy;
                return true;
            }
            else
            {
                closest = null;
                return false;
            }
        }

        private void FindClosestBouy(Point p)
        {
            Bouy b=null;
            if(TryFindCloseBouy(p,ref b))
            {
                _selectedBouy = b;
                for (int i = 0; i < marksLB.Items.Count; i++)
                {
                    if (((Mark)marksLB.Items[i]).Id == b.Mark.Id)
                    {
                        marksLB.SelectedIndex = i;
                    }
                }
            }
        }

        private double TwoDimensionalDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }

        

        private void LoadCourse()
        {
            Course.WindDirectionType dirType = _course.DirectionType;
            int fromId=0;
            int toId=0;
            if (_course.WindFromMark != null)
            {
                fromId = _course.WindFromMark.Id;
            }
            if (_course.WindToMark != null)
            {
                toId = _course.WindToMark.Id;
            }
            double manualAngle = _course.ManualWindAngle;


            LoadCourseMarks();
            LoadCourseRoute();

            nameTB.Text = _course.Name;
            dateDP.Value = _course.Date;

            if (dirType == Course.WindDirectionType.ConstantCourse)
            {
                windCourseRB.Checked = true;
                if (_course.WindFromMark != null)
                {
                    for (int i = 0; i < windFromMarkCB.Items.Count; i++)
                    {
                        if (((Mark)windFromMarkCB.Items[i]).Id == fromId)
                        {
                            windFromMarkCB.SelectedIndex = i;
                            break;
                        }
                    }
                }
                
                if (_course.WindToMark != null)
                {
                    for (int i = 0; i < windToMarkCB.Items.Count; i++)
                    {
                        if (((Mark)windToMarkCB.Items[i]).Id == toId)
                        {
                            windToMarkCB.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else if (dirType == Course.WindDirectionType.ConstantManual)
            {
                windManualRB.Checked = true;
                manualDS.Value = manualAngle;
            }
        }
        private void LoadCourseMarks()
        {
            int selection = marksLB.SelectedIndex;
            int fromSelection = windFromMarkCB.SelectedIndex;
            int toSelection = windToMarkCB.SelectedIndex;

            marksLB.Items.Clear();
            routeMarkLB.Items.Clear();
            windFromMarkCB.Items.Clear();
            windToMarkCB.Items.Clear();
            foreach (Mark m in _course.Marks)
            {
                marksLB.Items.Add(m);
                routeMarkLB.Items.Add(m);
                windFromMarkCB.Items.Add(m);
                windToMarkCB.Items.Add(m);
            }
            if (selection >= 0 && selection < marksLB.Items.Count)
            {
                marksLB.SelectedIndex = selection;
            }
            if (fromSelection >= 0 && fromSelection < windFromMarkCB.Items.Count)
            {
                windFromMarkCB.SelectedIndex = fromSelection;
            }
            else if (windFromMarkCB.Items.Count > 0)
            {
                windFromMarkCB.SelectedIndex = 0;
            }
            if (toSelection >= 0 && toSelection < windToMarkCB.Items.Count)
            {
                windToMarkCB.SelectedIndex = toSelection;
            }
            else if (windToMarkCB.Items.Count > 0)
            {
                windToMarkCB.SelectedIndex = 0;
            }
        }
        private void LoadCourseRoute()
        {
            routeLB.Items.Clear();
            List<Mark> order = _course.Route;
            foreach (Mark m in order)
            {
                foreach (object o in marksLB.Items)
                {
                    if (((Mark)o).Id == m.Id)
                    {
                        routeLB.Items.Add(o);
                        break;
                    }
                }
            }
        }

        private void LoadSelectedMark()
        {
            if (SelectedMark != null)
            {
                editMarkGB.Enabled = true;
                removeBTN.Enabled = true;
                markNameTB.Text = SelectedMark.Name;
                markTypeCB.SelectedItem = SelectedMark.MarkType;
                mouseCoordsLBL.Text = SelectedMark.Bouys[0].Latitude.ToString() + " " + SelectedMark.Bouys[0].Longitude.ToString();
                _selectedBouy = SelectedMark.Bouys[0];
                lakePNL.Invalidate();
            }
            else
            {
                editMarkGB.Enabled = false;
                removeBTN.Enabled = false;
            }
        }

        private Mark SelectedMark
        {
            get
            {
                if (marksLB.SelectedIndex >= 0)
                {
                    return (Mark)marksLB.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
        }

        //private void dateDP_ValueChanged(object sender, EventArgs e)
        //{
        //    _course.Date = dateDP.Value;
        //}

        private void nameTB_TextChanged(object sender, EventArgs e)
        {
            _course.Name = nameTB.Text;
        }

        private void marksLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedMark();
        }

        private void addBTN_Click(object sender, EventArgs e)
        {
            Mark m = new Mark("New Mark", "Mark", _course);
            m.Save();

            double lakeWidth = _lake.West - _lake.East;
            double lakeHeight = _lake.North - _lake.South;

            Bouy b = NewBouy(m);
            marksLB.Items.Add(m);
            marksLB.SelectedIndex = marksLB.Items.Count - 1;
        }

        private Bouy NewBouy(Mark m)
        {
            CoordinatePoint bouyC = ScreenToCoordinates(new Point(lakePNL.Width / 2, lakePNL.Height / 2));
            Bouy b = new Bouy(m, bouyC.Latitude, bouyC.Longitude);
            b.Save();
            return b;
        }

        private void removeBTN_Click(object sender, EventArgs e)
        {
            if (SelectedMark != null)
            {
                SelectedMark.Delete();
                marksLB.Items.RemoveAt(marksLB.SelectedIndex);
                if (marksLB.Items.Count > 0)
                {
                    marksLB.SelectedIndex = 0;
                }
            }
        }

        
        private void markNameTB_TextChanged(object sender, EventArgs e)
        {
            SelectedMark.Name = markNameTB.Text;
            SelectedMark.Save();
            LoadCourseMarks();
            LoadCourseRoute();
        }

        private void markTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedMark.MarkType = (string)markTypeCB.SelectedItem;
            if (SelectedMark.MarkType == "Gate")
            {
                if (SelectedMark.Bouys.Count < 2)
                {
                    while (SelectedMark.Bouys.Count < 2)
                    {
                        Bouy b = NewBouy(SelectedMark);
                        b.Save();
                    }
                }
            }
            else if (SelectedMark.MarkType == "Mark")
            {
                if (SelectedMark.Bouys.Count > 1)
                {
                    while (SelectedMark.Bouys.Count > 1)
                    {
                        SelectedMark.Bouys[SelectedMark.Bouys.Count - 1].Delete();
                    }
                }
            }
            SelectedMark.Save();
            lakePNL.Invalidate();
        }

        private void ClearCache()
        {
            _cachedPanel = null;
            _cachedPanelWithBoats = null;
        }

        private void lakePNL_Paint(object sender, PaintEventArgs e)
        {
            CoordinatePoint a2 = new CoordinatePoint(new Coordinate(_lake.South), new Coordinate(_lake.East),0);
            CoordinatePoint b2 = new CoordinatePoint(new Coordinate(_lake.North), new Coordinate(_lake.West),0);
            Point br=this.CoordinatesToScreen(a2);
            Point tl = this.CoordinatesToScreen(b2);

            
            if (_cachedPanel == null)
            {
                _cachedPanel = new Bitmap(lakePNL.Width, lakePNL.Height);
                _cachedPanelWithBoats = new Bitmap(lakePNL.Width, lakePNL.Height);
                Graphics pg = Graphics.FromImage(_cachedPanel);
                Graphics pgwb = Graphics.FromImage(_cachedPanelWithBoats);

                pg.FillRectangle(Brushes.White, 0, 0, lakePNL.Width, lakePNL.Height);

                if (_image == null)
                {
                    pg.FillRectangle(Brushes.DarkBlue, tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);
                }
                else
                {
                    pg.DrawImage(_image, tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);
                }

                pgwb.DrawImage(_cachedPanel, 0, 0);

                foreach (List<ColoredCoordinatePoint> points in _paths)
                {
                    int skip = 1;
                    if (points.Count >= skip + 1)
                    {
                        Color c = points[0].Color;
                        for (int i = 0; i < points.Count - skip; i = i + skip)
                        {
                            pgwb.DrawLine(new Pen(c), CoordinatesToScreen(points[i].Point), CoordinatesToScreen(points[i + skip].Point));
                        }                        
                    }
                }
            }

            Bitmap temp = new Bitmap(lakePNL.Width, lakePNL.Height);
            Graphics gt = Graphics.FromImage(temp);

            if (showBoatsCB.Checked)
            {
                gt.DrawImage(_cachedPanelWithBoats,0,0);
            }
            else
            {
                gt.DrawImage(_cachedPanel, 0, 0);
            }

            if (_mouseDown && zoomInCB.Checked)
            {
                int lowX, highX, lowY, highY;
                if (_zoomFirstPoint.X < _zoomSecondPoint.X)
                {
                    lowX = _zoomFirstPoint.X;
                    highX = _zoomSecondPoint.X;
                }
                else
                {
                    highX = _zoomFirstPoint.X;
                    lowX = _zoomSecondPoint.X;
                }
                if (_zoomFirstPoint.Y < _zoomSecondPoint.Y)
                {
                    lowY = _zoomFirstPoint.Y;
                    highY = _zoomSecondPoint.Y;
                }
                else
                {
                    highY = _zoomFirstPoint.Y;
                    lowY = _zoomSecondPoint.Y;
                }

                Pen ZoomPen = new Pen(Color.Green, 2f);

                gt.DrawRectangle(ZoomPen, new Rectangle(lowX, lowY, highX - lowX, highY - lowY));
            }

            List<Bouy> bouys = new List<Bouy>();
            foreach (object o in marksLB.Items)
            {
                bouys.AddRange(((Mark)o).Bouys);
            }

            Point? previous = null;
            foreach (object o in routeLB.Items)
            {
                Mark currentMark = (Mark)o;
                int xSum = 0;
                int ySum = 0;
                List<Point> bouyPoints = new List<Point>();
                foreach (Bouy b in currentMark.Bouys)
                {
                    Point p = CoordinatesToScreen(new CoordinatePoint(b.Latitude, b.Longitude, 0));
                    bouyPoints.Add(p);
                    xSum = xSum + p.X;
                    ySum = ySum + p.Y;
                }
                Point current = new Point(xSum / currentMark.Bouys.Count, ySum / currentMark.Bouys.Count);

                if (bouyPoints.Count > 1)
                {
                    foreach (Point p in bouyPoints)
                    {
                        gt.DrawLine(Pens.Green, current, p);
                    }
                }

                if (previous != null)
                {
                    gt.DrawLine(Pens.Yellow, current, (Point)previous);
                }
                previous = current;
            }

            foreach (Bouy b in bouys)
            {
                Point p=CoordinatesToScreen(new CoordinatePoint(b.Latitude,b.Longitude,0));
                if (_selectedBouy != null && b.Id == _selectedBouy.Id)
                {
                    gt.FillEllipse(Brushes.Red, new Rectangle(p.X - 5, p.Y - 5, 10, 10));
                }
                else
                {
                    gt.FillEllipse(Brushes.Orange, new Rectangle(p.X - 4, p.Y - 4, 8, 8));
                }
            }

            e.Graphics.DrawImage(temp, 0, 0);
        }

        private void courseCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourse();
            lakePNL.Invalidate();
        }

        private Point CoordinatesToScreen(CoordinatePoint point)
        {
            Lake lake = _lake;
            if (_zoomed&&_zoomedLake!=null)
            {
                lake = _zoomedLake;
            }

            //find length and width of the scaled area
            double scaleWidth = Math.Abs(lake.West - lake.East);
            double scaleHeight = Math.Abs(lake.North - lake.South);

            double scale; //the ratio of panel width to scaled width
            int dimension = 0; //the smallest dimension of the panel

            //set the scale based on whichever dimension is the longest
            //(if they are equal it doesn't matter)

            if (scaleWidth > scaleHeight)
            {
                if ((lakePNL.Width / scaleWidth) * scaleHeight <= lakePNL.Height)
                {
                    dimension = lakePNL.Width;
                    scale = (double)dimension / scaleWidth;
                }
                else
                {
                    dimension = lakePNL.Height;
                    scale = (double)dimension / scaleHeight;
                }
            }
            else
            {
                if (((double)lakePNL.Height / scaleHeight) * scaleWidth <= lakePNL.Width)
                {
                    dimension = lakePNL.Height;
                    scale = (double)dimension / scaleHeight;
                }
                else
                {
                    dimension = lakePNL.Width;
                    scale = (double)dimension / scaleWidth;
                }
            }

            Point output = new Point();
            output.X = (int)((point.Longitude.Value - lake.West) * scale);
            output.Y = (int)(scaleHeight * scale) - (int)((point.Latitude.Value - lake.South) * scale);
            return output;
        }
        private CoordinatePoint ScreenToCoordinates(Point p)
        {
            Lake lake = _lake;
            if (_zoomed && _zoomedLake != null)
            {
                lake = _zoomedLake;
            }
            //find length and width of the scaled area
            double scaleWidth = Math.Abs(lake.West - lake.East);
            double scaleHeight = Math.Abs(lake.North - lake.South);

            double scale; //the ratio of panel width to scaled width
            int dimension; //the smallest dimension of the panel

            //set the scale based on whichever dimension is the longest
            //(if they are equal it doesn't matter)

            if (scaleWidth > scaleHeight)
            {
                if ((lakePNL.Width / scaleWidth) * scaleHeight <= lakePNL.Height)
                {
                    dimension = lakePNL.Width;
                    scale = scaleWidth / (double)dimension;
                }
                else
                {
                    dimension = lakePNL.Height;
                    scale = scaleHeight / (double)dimension;
                }
            }
            else
            {
                if (((double)lakePNL.Height / scaleHeight) * scaleWidth <= lakePNL.Width)
                {
                    dimension = lakePNL.Height;
                    scale = scaleHeight / (double)dimension;
                }
                else
                {
                    dimension = lakePNL.Width;
                    scale = scaleWidth / (double)dimension;
                }
            }

            Coordinate longitude;
            Coordinate latitude;
            //DoublePoint output = new DoublePoint();
            if (lake.West < 0)
            {
                //output.X = _right - (((scaleWidth * scale) - p.X) * scale);
                longitude = new Coordinate(lake.West - (((scaleWidth * scale) - p.X) * scale));
            }
            else
            {
                //output.X = _left + (p.X * scale);
                longitude = new Coordinate(lake.West + (p.X * scale));
            }
            if (lake.North < 0)
            {
                //output.Y = _top - (p.Y * scale);
                latitude = new Coordinate(lake.North - (p.Y * scale));
            }
            else
            {
                //output.Y = _bottom + (((scaleHeight / scale) - p.Y) * scale);
                latitude = new Coordinate(lake.South + (((scaleHeight / scale) - p.Y) * scale));
            }
            return new CoordinatePoint(latitude, longitude, 0);
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            _course.Save();
            this.Close();
        }

        private void addOrderBTN_Click(object sender, EventArgs e)
        {
            if (routeMarkLB.SelectedIndex>=0)
            {
                routeLB.Items.Add((Mark)routeMarkLB.SelectedItem);
                SaveRoute();
                lakePNL.Invalidate();
            }
        }

        private void downBTN_Click(object sender, EventArgs e)
        {
            if (routeLB.SelectedItem != null)
            {
                int index = routeLB.SelectedIndex;
                if (index < routeLB.Items.Count - 2)
                {

                    //routeLB.Items.Insert(index + 2, routeLB.SelectedItem);
                    //routeLB.Items.RemoveAt(index);
                }
                SaveRoute();
            }
        }

        private void upBTN_Click(object sender, EventArgs e)
        {
            if (routeLB.SelectedItem != null)
            {
                int index = routeLB.SelectedIndex;
                if (index > 0)
                {
                    routeLB.Items.Insert(index-1, routeLB.SelectedItem);
                    routeLB.Items.RemoveAt(index);
                }
                SaveRoute();
            }
        }

        private void removeOrderBTN_Click(object sender, EventArgs e)
        {
            if (routeLB.SelectedItem != null)
            {
                routeLB.Items.RemoveAt(routeLB.SelectedIndex);
                SaveRoute();
                lakePNL.Invalidate();
            }
        }
        private void SaveRoute()
        {
            List<Mark> marks = new List<Mark>();
            foreach (object o in routeLB.Items)
            {
                marks.Add((Mark)o);
            }
            _course.Route = marks;
        }

        private void showBoatsCB_CheckedChanged(object sender, EventArgs e)
        {
            lakePNL.Invalidate();
        }

        private void lakePNL_Resize(object sender, EventArgs e)
        {
            ClearCache();
            lakePNL.Invalidate();
        }

        private void zoomInCB_CheckedChanged(object sender, EventArgs e)
        {
            lakePNL.Cursor = Cursors.Cross;
        }

        private void zoomOutBTN_Click(object sender, EventArgs e)
        {
            _zoomed = false;
            _zoomedLake = null;
            ClearCache();
            zoomInCB.Checked = false;
            lakePNL.Invalidate();
            lakePNL.Cursor = Cursors.Arrow;
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void windCourseRB_CheckedChanged(object sender, EventArgs e)
        {
            SetWind();
        }

        private void windManualRB_CheckedChanged(object sender, EventArgs e)
        {
            SetWind();
        }

        private void SetWind()
        {
            windFromMarkCB.Enabled = windCourseRB.Checked;
            windToMarkCB.Enabled = windCourseRB.Checked;
            manualDS.Enabled = windManualRB.Checked;

            if (windCourseRB.Checked)
            {
                if (windToMarkCB.SelectedIndex >= 0)
                {
                    _course.WindToMark = (Mark)windToMarkCB.SelectedItem;
                }
                if (windFromMarkCB.SelectedIndex >= 0)
                {
                    _course.WindFromMark = (Mark)windFromMarkCB.SelectedItem;
                }
            }
            else if (windManualRB.Checked)
            {
                _course.ManualWindAngle = manualDS.Value;
            }
        }

        private void manualDS_ValueChanged(object sender, EventArgs e)
        {
            SetWind();
        }

        private void windFromMarkCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetWind();
        }

        private void windToMarkCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetWind();
        }

        private void routeMarkLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            addOrderBTN.Enabled = routeMarkLB.SelectedIndex >= 0;
        }

        private void routeLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeOrderBTN.Enabled = routeLB.SelectedIndex >= 0;
        }
    }
    public struct ColoredCoordinatePoint
    {
        public CoordinatePoint Point;
        public Color Color;
        public ColoredCoordinatePoint(CoordinatePoint point, Color color)
        {
            Point = point;
            Color = color;
        }
    }
}
