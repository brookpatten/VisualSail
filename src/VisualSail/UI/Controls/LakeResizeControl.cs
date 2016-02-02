using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using AmphibianSoftware.VisualSail.Data;

namespace AmphibianSoftware.VisualSail.UI.Controls
{
    public partial class LakeResizeControl : UserControl
    {
        private enum SelectedCorner {TopLeft,TopRight,BottomLeft,BottomRight};
        private Lake _lake;
        private bool _mouseDown;
        private Point _mousePosition;
        private Image _satelite;
        private SelectedCorner? _selectedCorner=null;
        private CoordinatePoint _gpsBoundsNorthWest;
        private CoordinatePoint _gpsBoundsSouthEast;
        
        public LakeResizeControl()
        {
            InitializeComponent();
        }
        public LakeResizeControl(Lake lake)
        {
            InitializeComponent();
            _lake = lake;
        }
        public Lake Lake
        {
            get
            {
                return _lake;
            }
            set
            {
                _lake = value;
                if (_lake != null)
                {
                    UpdateImage();
                    this.Invalidate();
                }
            }
        }
        public void SetGpsBounds(CoordinatePoint northWest, CoordinatePoint southEast)
        {
            _gpsBoundsNorthWest = northWest;
            _gpsBoundsSouthEast = southEast;
        }
        private void LakeResizeControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height);
            if (_lake != null)
            {
                CoordinatePoint a2 = new CoordinatePoint(new Coordinate(_lake.South), new Coordinate(_lake.East), 0);
                CoordinatePoint b2 = new CoordinatePoint(new Coordinate(_lake.North), new Coordinate(_lake.West), 0);
                //Point br = this.CoordinatesToScreen(new CoordinatePoint(new Coordinate(_lake.South), new Coordinate(_lake.East), 0));
                Point tl = CoordinatesToScreen(b2);
                Point br = CoordinatesToScreen(a2);

                Point tr = new Point(br.X, tl.Y);
                Point bl = new Point(tl.X, br.Y);

                Rectangle bounds = new Rectangle(tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);

                e.Graphics.FillRectangle(Brushes.DarkBlue, bounds);
                if (_satelite != null)
                {
                    e.Graphics.DrawImage(_satelite, bounds);
                }
                
                if (_gpsBoundsNorthWest != null && _gpsBoundsSouthEast != null)
                {
                    Point gpsTl = CoordinatesToScreen(_gpsBoundsNorthWest);
                    Point gpsBr = CoordinatesToScreen(_gpsBoundsSouthEast);
                    e.Graphics.DrawRectangle(Pens.Red, gpsTl.X, gpsTl.Y, gpsBr.X - gpsTl.X, gpsBr.Y - gpsTl.Y);
                }

                if (_mouseDown)
                {
                    CoordinatePoint p = ScreenToCoordinates(_mousePosition);
                    Point test = CoordinatesToScreen(p);
                    e.Graphics.FillRectangle(Brushes.Red, test.X - 1, test.Y - 1, 2, 2);

                    switch (_selectedCorner)
                    {
                        case SelectedCorner.BottomLeft:
                            bl = _mousePosition;
                            tl.X = _mousePosition.X;
                            br.Y = _mousePosition.Y;
                            break;
                        case SelectedCorner.BottomRight:
                            br = _mousePosition;
                            tr.X = _mousePosition.X;
                            bl.Y = _mousePosition.Y;
                            break;
                        case SelectedCorner.TopLeft:
                            tl = _mousePosition;
                            bl.X = _mousePosition.X;
                            tr.Y = _mousePosition.Y;
                            break;
                        case SelectedCorner.TopRight:
                            tr = _mousePosition;
                            br.X = _mousePosition.X;
                            tl.Y = _mousePosition.Y;
                            break;
                    }
                    bounds = new Rectangle(tl.X, tl.Y, tr.X - tl.X, br.Y - tl.Y);
                }

                Pen outlinePen = new Pen(Brushes.Blue, 2);
                e.Graphics.DrawRectangle(outlinePen, bounds);
                e.Graphics.FillEllipse(Brushes.Orange, tl.X - 3, tl.Y - 3, 6, 6);
                e.Graphics.FillEllipse(Brushes.Orange, br.X - 3, br.Y - 3, 6, 6);
                e.Graphics.FillEllipse(Brushes.Orange, tr.X - 3, tr.Y - 3, 6, 6);
                e.Graphics.FillEllipse(Brushes.Orange, bl.X - 3, bl.Y - 3, 6, 6);
            }
        }
        private Point Offset
        {
            get
            {
                Point br = this.ScaleCoordinatesToScreen(new CoordinatePoint(new Coordinate(_lake.South), new Coordinate(_lake.East), 0));
                Point tl = new Point((this.Width - br.X) / 2, (this.Height - br.Y) / 2);
                return tl;
            }
        }
        private Point CoordinatesToScreen(CoordinatePoint point)
        {
            Point off = Offset;
            Point p = ScaleCoordinatesToScreen(point);
            p.X = p.X + off.X;
            p.Y = p.Y + off.Y;
            return p;
        }
        private Point ScaleCoordinatesToScreen(CoordinatePoint point)
        {
            //find length and width of the scaled area
            double scaleWidth = Math.Abs(_lake.West - _lake.East);
            double scaleHeight = Math.Abs(_lake.North - _lake.South);

            double scale; //the ratio of panel width to scaled width
            int dimension = 0; //the smallest dimension of the panel

            //set the scale based on whichever dimension is the longest
            //(if they are equal it doesn't matter)

            if (scaleWidth > scaleHeight)
            {
                if ((this.Width / scaleWidth) * scaleHeight <= this.Height)
                {
                    dimension = this.Width;
                    scale = (double)dimension / scaleWidth;
                }
                else
                {
                    dimension = this.Height;
                    scale = (double)dimension / scaleHeight;
                }
            }
            else
            {
                if (((double)this.Height / scaleHeight) * scaleWidth <= this.Width)
                {
                    dimension = this.Height;
                    scale = (double)dimension / scaleHeight;
                }
                else
                {
                    dimension = this.Width;
                    scale = (double)dimension / scaleWidth;
                }
            }

            scale = scale / 1.5;

            Point output = new Point();
            output.X = (int)((point.Longitude.Value - _lake.West) * scale);
            output.Y = (int)(scaleHeight * scale) - (int)((point.Latitude.Value - _lake.South) * scale);
            return output;
        }
        private CoordinatePoint ScreenToCoordinates(Point p)
        {
            Point off = Offset;
            p.X = p.X - off.X;
            p.Y = p.Y - off.Y;
            return ScaleScreenToCoordinates(p);
        }
        private CoordinatePoint ScaleScreenToCoordinates(Point p)
        {
            
            //find length and width of the scaled area
            double scaleWidth = Math.Abs(_lake.West - _lake.East);
            double scaleHeight = Math.Abs(_lake.North - _lake.South);

            double scale; //the ratio of panel width to scaled width
            int dimension; //the smallest dimension of the panel

            //set the scale based on whichever dimension is the longest
            //(if they are equal it doesn't matter)

            if (scaleWidth > scaleHeight)
            {
                if ((this.Width / scaleWidth) * scaleHeight <= this.Height)
                {
                    dimension = this.Width;
                    scale = scaleWidth / (double)dimension;
                }
                else
                {
                    dimension = this.Height;
                    scale = scaleHeight / (double)dimension;
                }
            }
            else
            {
                if (((double)this.Height / scaleHeight) * scaleWidth <= this.Width)
                {
                    dimension = this.Height;
                    scale = scaleHeight / (double)dimension;
                }
                else
                {
                    dimension = this.Width;
                    scale = scaleWidth / (double)dimension;
                }
            }
            scale = scale * 1.5;
            Coordinate longitude;
            Coordinate latitude;
            //DoublePoint output = new DoublePoint();
            if (_lake.West < 0)
            {
                //output.X = _right - (((scaleWidth * scale) - p.X) * scale);
                longitude = new Coordinate(_lake.West - (((scaleWidth * scale) - p.X) * scale));
            }
            else
            {
                //output.X = _left + (p.X * scale);
                longitude = new Coordinate(_lake.West + (p.X * scale));
            }
            if (_lake.North < 0)
            {
                //output.Y = _top - (p.Y * scale);
                latitude = new Coordinate(_lake.North - (p.Y * scale));
            }
            else
            {
                //output.Y = _bottom + (((scaleHeight / scale) - p.Y) * scale);
                latitude = new Coordinate(_lake.South + (((scaleHeight / scale) - p.Y) * scale));
            }
            return new CoordinatePoint(latitude, longitude, 0);
        }
        private void UpdateImage()
        {
            BusyDialogManager.Show("Loading Satellite Imagery");
            Thread.Sleep(100);
            try
            {
                _satelite = AmphibianSoftware.VisualSail.Library.SatelliteImageryHelper.GetImageForLake(_lake);
                BusyDialogManager.Hide();
            }
            catch (Exception e)
            {
                _satelite = null;
                BusyDialogManager.Hide();
                MessageBox.Show("Could not retrieve download imagery: " + e.Message.Replace("wms.jpl.nasa.gov",""));
            }
        }
        private void LakeResizeControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (_lake != null)
            {
                _mouseDown = true;
                Point mousePoint = new Point(e.X, e.Y);
                _mousePosition = mousePoint;

                //CoordinatePoint a2 = new CoordinatePoint(new Coordinate(_lake.South), new Coordinate(_lake.East), 0);
                //CoordinatePoint b2 = new CoordinatePoint(new Coordinate(_lake.North), new Coordinate(_lake.West), 0);
                //Point tl = CoordinatesToScreen(b2);
                //Point br = CoordinatesToScreen(a2);
                //Point tr = new Point(br.X, tl.Y);
                //Point bl = new Point(tl.X, br.Y);

                //double tlDistance = CoordinatePoint.TwoDimensionalDistance(mousePoint.X, mousePoint.Y, tl.X, tl.Y);
                //double brDistance = CoordinatePoint.TwoDimensionalDistance(mousePoint.X, mousePoint.Y, br.X, br.Y);
                //double trDistance = CoordinatePoint.TwoDimensionalDistance(mousePoint.X, mousePoint.Y, tr.X, tr.Y);
                //double blDistance = CoordinatePoint.TwoDimensionalDistance(mousePoint.X, mousePoint.Y, bl.X, bl.Y);

                //if (tlDistance <= brDistance && tlDistance <= trDistance && tlDistance <= blDistance)
                //{
                //    _selectedCorner = SelectedCorner.TopLeft;
                //}
                //else if (brDistance <= tlDistance && brDistance <= trDistance && brDistance <= blDistance)
                //{
                //    _selectedCorner = SelectedCorner.BottomRight;
                //}
                //else if (trDistance <= tlDistance && trDistance <= brDistance && trDistance <= blDistance)
                //{
                //    _selectedCorner = SelectedCorner.TopRight;
                //}
                //else if (blDistance <= tlDistance && blDistance <= brDistance && blDistance <= trDistance)
                //{
                //    _selectedCorner = SelectedCorner.BottomLeft;
                //}

                this.Invalidate();
            }
        }

        private void LakeResizeControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_lake != null)
            {
                _mouseDown = false;
                _mousePosition = new Point(e.X, e.Y);
                if (_selectedCorner.HasValue)
                {
                    switch (_selectedCorner.Value)
                    {
                        case SelectedCorner.BottomLeft:
                            CoordinatePoint bl = ScreenToCoordinates(_mousePosition);
                            if (_gpsBoundsNorthWest != null && _gpsBoundsSouthEast != null)
                            {
                                if (bl.Latitude.Value < _gpsBoundsSouthEast.Latitude.Value)
                                {
                                    _lake.South = bl.Latitude.Value;
                                }
                                else
                                {
                                    _lake.South = _gpsBoundsSouthEast.Latitude.Value;
                                }
                                if (bl.Longitude.Value < _gpsBoundsNorthWest.Longitude.Value)
                                {
                                    _lake.West = bl.Longitude.Value;
                                }
                                else
                                {
                                    _lake.West = _gpsBoundsNorthWest.Longitude.Value;
                                }
                            }
                            else
                            {
                                _lake.South = bl.Latitude.Value;
                                _lake.West = bl.Longitude.Value;
                            }
                            break;
                        case SelectedCorner.BottomRight:
                            CoordinatePoint br = ScreenToCoordinates(_mousePosition);
                            if (_gpsBoundsNorthWest != null && _gpsBoundsSouthEast != null)
                            {
                                if (br.Latitude.Value < _gpsBoundsSouthEast.Latitude.Value)
                                {
                                    _lake.South = br.Latitude.Value;
                                }
                                else
                                {
                                    _lake.South = _gpsBoundsSouthEast.Latitude.Value;
                                }
                                if (br.Longitude.Value > _gpsBoundsSouthEast.Longitude.Value)
                                {
                                    _lake.East = br.Longitude.Value;
                                }
                                else
                                {
                                    _lake.East = _gpsBoundsSouthEast.Longitude.Value;
                                }
                            }
                            else
                            {
                                _lake.South = br.Latitude.Value;
                                _lake.East = br.Longitude.Value;
                            }
                            break;
                        case SelectedCorner.TopLeft:
                            CoordinatePoint tl = ScreenToCoordinates(_mousePosition);
                            if (_gpsBoundsNorthWest != null && _gpsBoundsSouthEast != null)
                            {
                                if (tl.Latitude.Value > _gpsBoundsNorthWest.Latitude.Value)
                                {
                                    _lake.North = tl.Latitude.Value;
                                }
                                else
                                {
                                    _lake.North = _gpsBoundsNorthWest.Latitude.Value;
                                }
                                if (tl.Longitude.Value < _gpsBoundsNorthWest.Longitude.Value)
                                {
                                    _lake.West = tl.Longitude.Value;
                                }
                                else
                                {
                                    _lake.West = _gpsBoundsNorthWest.Longitude.Value;
                                }
                            }
                            else
                            {
                                _lake.North = tl.Latitude.Value;
                                _lake.West = tl.Longitude.Value;
                            }
                            break;
                        case SelectedCorner.TopRight:
                            CoordinatePoint tr = ScreenToCoordinates(_mousePosition);
                            if (_gpsBoundsNorthWest != null && _gpsBoundsSouthEast != null)
                            {
                                if (tr.Latitude.Value > _gpsBoundsNorthWest.Latitude.Value)
                                {
                                    _lake.North = tr.Latitude.Value;
                                }
                                else
                                {
                                    _lake.North = _gpsBoundsNorthWest.Latitude.Value;
                                }
                                if (tr.Longitude.Value > _gpsBoundsSouthEast.Longitude.Value)
                                {
                                    _lake.East = tr.Longitude.Value;
                                }
                                else
                                {
                                    _lake.East = _gpsBoundsSouthEast.Longitude.Value;
                                }
                            }
                            else
                            {
                                _lake.North = tr.Latitude.Value;
                                _lake.East = tr.Longitude.Value;
                            }
                            break;
                    }
                    _lake.Save();
                    UpdateImage();
                    this.Invalidate();
                }
            }
        }

        private void LakeResizeControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_lake != null)
            {
                _mousePosition = new Point(e.X, e.Y);
                if (_mouseDown && _selectedCorner.HasValue)
                {
                    this.Invalidate();
                }
                else
                {
                    double minDistance = 25.0;

                    CoordinatePoint a2 = new CoordinatePoint(new Coordinate(_lake.South), new Coordinate(_lake.East), 0);
                    CoordinatePoint b2 = new CoordinatePoint(new Coordinate(_lake.North), new Coordinate(_lake.West), 0);
                    Point tl = CoordinatesToScreen(b2);
                    Point br = CoordinatesToScreen(a2);
                    Point tr = new Point(br.X, tl.Y);
                    Point bl = new Point(tl.X, br.Y);

                    double tlDistance = CoordinatePoint.TwoDimensionalDistance(_mousePosition.X, _mousePosition.Y, tl.X, tl.Y);
                    double brDistance = CoordinatePoint.TwoDimensionalDistance(_mousePosition.X, _mousePosition.Y, br.X, br.Y);
                    double trDistance = CoordinatePoint.TwoDimensionalDistance(_mousePosition.X, _mousePosition.Y, tr.X, tr.Y);
                    double blDistance = CoordinatePoint.TwoDimensionalDistance(_mousePosition.X, _mousePosition.Y, bl.X, bl.Y);

                    if (tlDistance <= brDistance && tlDistance <= trDistance && tlDistance <= blDistance && tlDistance<minDistance)
                    {
                        _selectedCorner = SelectedCorner.TopLeft;
                        this.Cursor = Cursors.PanNW;
                    }
                    else if (brDistance <= tlDistance && brDistance <= trDistance && brDistance <= blDistance && brDistance<minDistance)
                    {
                        _selectedCorner = SelectedCorner.BottomRight;
                        this.Cursor = Cursors.PanSE;
                    }
                    else if (trDistance <= tlDistance && trDistance <= brDistance && trDistance <= blDistance && trDistance<minDistance)
                    {
                        _selectedCorner = SelectedCorner.TopRight;
                        this.Cursor = Cursors.PanNE;
                    }
                    else if (blDistance <= tlDistance && blDistance <= brDistance && blDistance <= trDistance && blDistance < minDistance)
                    {
                        _selectedCorner = SelectedCorner.BottomLeft;
                        this.Cursor = Cursors.PanSW;
                    }
                    else
                    {
                        _selectedCorner = null;
                        this.Cursor = Cursors.No;
                    }
                }
            }
        }
    }
}
