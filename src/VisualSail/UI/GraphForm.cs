using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using AmphibianSoftware.VisualSail.Data.Statistics;
using AmphibianSoftware.VisualSail.PostBuild;

using ZedGraph;
using WeifenLuo.WinFormsUI.Docking;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class GraphForm : DockContent
    {
        //private bool _needsRepaint = false;
        private int _updateTimeout = 500;
        private bool _run=true;
        private bool _configuring = false;
        private static int _defaultAutoScrollSize = 120;
        private int? _autoScrollSize = _defaultAutoScrollSize;
        private Thread _painter;
        [DoNotObfuscate()]
        private enum StatisticGroupType { Boat, Leg, Tack };
        private Replay _replay;
        private string _statisticName;
        private List<SelectedStatisticCell> _statistics;
        private List<CurveItem> _curves;
        private StatisticUnitType _unitType;
        private StatisticUnit _unit;
        private StatisticGroupType _type;

        private XDate _minXValue=double.MaxValue;
        private XDate _maxXValue=double.MinValue;
        private double _minYValue = double.MaxValue;
        private double _maxYValue = double.MinValue;

        private bool _enableOffset = false;
        private DateTime? _offset;
        ToolStripMenuItem _offsetOption;

        private bool _showKey = false;
        private ToolStripMenuItem _showKeyOption;

        //private LineObj _nowLine;

        public GraphForm()
        {
            InitializeComponent();
        }
        public GraphForm(Replay replay, string statName, List<SelectedStatisticCell> selection, StatisticUnitType unitType)
        {
            if (selection.Count > 0)
            {
                InitializeComponent();
                _replay = replay;
                _statisticName = statName;
                _statistics = selection;
                _unitType = unitType;
                if (selection[0].TackIndex.HasValue)
                {
                    _type = StatisticGroupType.Tack;
                }
                else if (selection[0].LegIndex.HasValue)
                {
                    _type = StatisticGroupType.Leg;
                }
                else
                {
                    _type = StatisticGroupType.Boat;
                }
                WireEvents();
                ConfigureGraph();
                _painter = new Thread(new ThreadStart(this.Run));
                _painter.Start();
            }
            else
            {
                throw new Exception("You must specify at least one statistic");
            }
        }
        private void ValidateSelections()
        {
            for (int i = 0; i < _statistics.Count;i++ )
            {
                if (_statistics[i].BoatIndex < _replay.Boats.Count)
                {
                    if (!_statistics[i].LegIndex.HasValue || _statistics[i].LegIndex < _replay.Race.Course.Route.Count)
                    {
                        if (!_statistics[i].TackIndex.HasValue || _statistics[i].TackIndex.Value < _replay.Boats[_statistics[i].BoatIndex.Value].Tacks.Count)
                        {
                            //everything checks out
                        }
                        else
                        {
                            //the tack is gone
                            _statistics.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        //the leg is gone, remove this selection
                        _statistics.RemoveAt(i);
                        i--;
                    }
                }
                else
                {
                    //the boat is gone, remove this selection
                    _statistics.RemoveAt(i);
                    i--;
                }
            }
        }
        private void WireEvents()
        {
            zg.PointValueEvent += new ZedGraphControl.PointValueHandler(zg_PointValueEvent);
            zg.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(zg_ContextMenuBuilder);
            zg.ZoomEvent += new ZedGraphControl.ZoomEventHandler(zg_ZoomEvent);
            zg.ScrollEvent += new ScrollEventHandler(zg_ScrollEvent);
            WireDataEvents();
        }

        void zg_ScrollEvent(object sender, ScrollEventArgs e)
        {
            //_autoScrollSize = null;
        }

        void zg_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            _autoScrollSize = null;
        }

        public void Reset()
        {
            //signal the thread to shut down
            //_run = false;
            ////wait for it to shut down
            //while (_painter.ThreadState == ThreadState.Running)
            //{
            //    Thread.Sleep(1);
            //}
            //it's possible the indices that were selected are now out of bounds
            //so we validate our selections
            ValidateSelections();
            //rewire data events
            UnwireDataEvents();
            WireEvents();
            ConfigureGraph();
            
            //_run = true;
            //_painter = new Thread(new ThreadStart(this.Run));
            //_painter.Start();
        }

        public void Shutdown()
        {
            UnwireDataEvents();
            _run = false;
            try
            {
                _painter.Abort();
            }
            catch { }
        }

        private void WireDataEvents()
        {
            foreach (SelectedStatisticCell ssc in _statistics)
            {
                if (_type == StatisticGroupType.Tack)
                {
                    _replay.Boats[ssc.BoatIndex.Value].TackStatistics[ssc.TackIndex.Value].AddStatisticListener(new TimeLineStatisticNewValueAdded(this.AddValueEvent), _statisticName);
                }
                else if (_type == StatisticGroupType.Leg)
                {
                    _replay.Boats[ssc.BoatIndex.Value].LegStatistics[ssc.LegIndex.Value].AddStatisticListener(new TimeLineStatisticNewValueAdded(this.AddValueEvent), _statisticName);
                }
                else
                {
                    _replay.Boats[ssc.BoatIndex.Value].TotalStatistics.AddStatisticListener(new TimeLineStatisticNewValueAdded(this.AddValueEvent), _statisticName);
                }
            }
        }

        private void UnwireDataEvents()
        {
            foreach (SelectedStatisticCell ssc in _statistics)
            {
                if (_type == StatisticGroupType.Tack)
                {
                    _replay.Boats[ssc.BoatIndex.Value].TackStatistics[ssc.TackIndex.Value].RemoveStatisticListener(new TimeLineStatisticNewValueAdded(this.AddValueEvent), _statisticName);
                }
                else if (_type == StatisticGroupType.Leg)
                {
                    _replay.Boats[ssc.BoatIndex.Value].LegStatistics[ssc.LegIndex.Value].RemoveStatisticListener(new TimeLineStatisticNewValueAdded(this.AddValueEvent), _statisticName);
                }
                else
                {
                    if (_replay.Boats != null)
                    {
                        _replay.Boats[ssc.BoatIndex.Value].TotalStatistics.RemoveStatisticListener(new TimeLineStatisticNewValueAdded(this.AddValueEvent), _statisticName);
                    }
                }
            }
        }

        private void Run()
        {
            while (_run)
            {
                if (_autoScrollSize.HasValue)
                {
                    //0 is a magic value indicating that we auto scale
                    //also, we do not autoscale if offest is enabled
                    if (_enableOffset||_autoScrollSize.Value==0)
                    {
                        zg.RestoreScale(zg.GraphPane);
                        //_nowLine.IsVisible = false;
                    }
                    else if (_autoScrollSize.Value > 0 && !_enableOffset)
                    {
                        DateTime simTime = _replay.SimulationTime;
                        //make sure we have some values
                        if (_minXValue != double.MaxValue && _maxXValue != double.MinValue)
                        {
                            //make sure the current time is on the scale
                            //if it's not we just stay where we are
                            if (simTime >= _minXValue && simTime <= _maxXValue.DateTime + new TimeSpan(0, 0, 5))
                            {
                                XDate min = new XDate(simTime - new TimeSpan(0, 0, _autoScrollSize.Value / 2));
                                XDate max = new XDate(simTime + new TimeSpan(0, 0, _autoScrollSize.Value / 2));
                                zg.GraphPane.XAxis.Scale.Min = min;
                                zg.GraphPane.XAxis.Scale.Max = max;
                                zg.GraphPane.YAxis.Scale.Min = _minYValue;
                                zg.GraphPane.YAxis.Scale.Max = _maxYValue;
                                //_nowLine.Location = new Location(new XDate(simTime), double.MaxValue, CoordType.XChartFractionYScale);
                                //_nowLine.IsVisible = true;
                            }
                            else
                            {
                                //_nowLine.IsVisible = false;
                            }
                        }
                        else
                        {
                            //_nowLine.Location = new Location(new XDate(simTime), double.MaxValue, CoordType.XChartFractionYScale);
                            //_nowLine.IsVisible = true;
                        }
                    }
                }
                zg.AxisChange();
                zg.Invalidate();
                Thread.Sleep(_updateTimeout);
            }
        }

        void zg_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            for (int i = 0; i < menuStrip.Items.Count; i++)
            {
                if ((string)menuStrip.Items[i].Tag == "unzoom" || (string)menuStrip.Items[i].Tag == "undo_all" || (string)menuStrip.Items[i].Tag == "show_val")
                {
                    menuStrip.Items.RemoveAt(i);
                    i--;
                }
                else if ((string)menuStrip.Items[i].Tag == "default")
                {
                    menuStrip.Items[i].Text = "Zoom Out";
                }
            }

            _offsetOption = new ToolStripMenuItem();
            _offsetOption.Text = "Toggle Comparison Mode";
            _offsetOption.Name = "offsetOption";
            _offsetOption.Tag = "offset";
            _offsetOption.Click += new EventHandler(offsetOption_Click);
            menuStrip.Items.Add(_offsetOption);

            _showKeyOption = new ToolStripMenuItem();
            _showKeyOption.Text = "Show/Hide Legend";
            _showKeyOption.Name = "showKey";
            _showKeyOption.Tag = "showKey";
            _showKeyOption.Click += new EventHandler(_showKeyOption_Click);
            menuStrip.Items.Add(_showKeyOption);

            ToolStripMenuItem autoScrollMenu = new ToolStripMenuItem();
            autoScrollMenu.Text = "Auto Scroll";
            autoScrollMenu.Name = "autoScroll";
            autoScrollMenu.Tag = "autoScroll";

            ToolStripMenuItem autoScroll30 = new ToolStripMenuItem();
            autoScroll30.Text = "30s";
            autoScroll30.Name = "autoScroll30";
            autoScroll30.Tag = "autoScroll30";
            autoScroll30.Click += new EventHandler(autoScroll_Click);
            autoScrollMenu.DropDownItems.Add(autoScroll30);

            ToolStripMenuItem autoScroll60 = new ToolStripMenuItem();
            autoScroll60.Text = "1m";
            autoScroll60.Name = "autoScroll60";
            autoScroll60.Tag = "autoScroll60";
            autoScroll60.Click += new EventHandler(autoScroll_Click);
            autoScrollMenu.DropDownItems.Add(autoScroll60);

            ToolStripMenuItem autoScroll120 = new ToolStripMenuItem();
            autoScroll120.Text = "2m";
            autoScroll120.Name = "autoScroll120";
            autoScroll120.Tag = "autoScroll120";
            autoScroll120.Click += new EventHandler(autoScroll_Click);
            autoScrollMenu.DropDownItems.Add(autoScroll120);

            ToolStripMenuItem autoScroll300 = new ToolStripMenuItem();
            autoScroll300.Text = "5m";
            autoScroll300.Name = "autoScroll300";
            autoScroll300.Tag = "autoScroll300";
            autoScroll300.Click += new EventHandler(autoScroll_Click);
            autoScrollMenu.DropDownItems.Add(autoScroll300);

            ToolStripMenuItem autoScroll0 = new ToolStripMenuItem();
            autoScroll0.Text = "All";
            autoScroll0.Name = "autoScroll0";
            autoScroll0.Tag = "autoScroll0";
            autoScroll0.Click += new EventHandler(autoScroll_Click);
            autoScrollMenu.DropDownItems.Add(autoScroll0);

            menuStrip.Items.Add(autoScrollMenu);
        }

        void autoScroll_Click(object sender, EventArgs e)
        {
            string tag = (string)((ToolStripMenuItem)sender).Tag;
            _autoScrollSize = int.Parse(tag.Replace("autoScroll", ""));
        }

        void _showKeyOption_Click(object sender, EventArgs e)
        {
            _showKey = !_showKey;
            _showKeyOption.Checked = _showKey;
            zg.GraphPane.Legend.IsVisible = _showKey;
            zg.Invalidate();
        }

        void offsetOption_Click(object sender, EventArgs e)
        {
            _enableOffset = !_enableOffset;
            _offsetOption.Checked = _enableOffset;
            //if we're disabling the offset default to 1m auto scroll
            if (!_enableOffset && !_autoScrollSize.HasValue)
            {
                _autoScrollSize = _defaultAutoScrollSize;
            }
            ConfigureGraph();
        }

        string zg_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            if (_configuring)
            {
                return "";
            }
            else
            {
                string time = "";
                if (!_enableOffset)
                {
                    time = (new XDate(curve[iPt].X)).DateTime.ToShortTimeString();
                }
                string val = string.Format("{0:0.##} " + _unit.ToString(), curve[iPt].Y);
                int index = _curves.IndexOf(curve);
                string name = GetSelectionLabel(_statistics[index]);

                return name + " " + time + " " + val;
            }
        }
        private void AddValueEvent(TimeLineStatisticCollection collection, DateTime dt)
        {
            if (!_configuring)
            {
                //find the indices
                int boatIndex = _replay.Boats.IndexOf(collection.ReplayBoat);
                int? legIndex = collection.legIndex;
                int? tackIndex = collection.tackIndex;

                //now find the index in our selections list
                int selectionIndex = 0;
                for (int i = 0; i < _statistics.Count; i++)
                {
                    if (_statistics[i].BoatIndex == boatIndex && _statistics[i].LegIndex == legIndex && _statistics[i].TackIndex == tackIndex)
                    {
                        selectionIndex = i;
                        break;
                    }
                }
                lock (_curves)
                {
                    AddPoint(_curves[selectionIndex], dt, collection.GetValue(_statisticName,_unitType,dt));
                }
                //_needsRepaint = true;
            }
        }
        private string GetSelectionLabel(SelectedStatisticCell ssc)
        {
            string boatName = _replay.Boats[ssc.BoatIndex.Value].Name;
            string curveName = "";
            if (_type == StatisticGroupType.Tack)
            {
                string tackName = (_replay.Boats[ssc.BoatIndex.Value].Tacks[ssc.TackIndex.Value].IndexOnLeg + 1).ToString() + " (" + _replay.Boats[ssc.BoatIndex.Value].Tacks[ssc.TackIndex.Value].Direction.ToString() + ")";
                string legName = ssc.LegIndex.ToString() + " (" + _replay.Race.Course.Route[ssc.LegIndex.Value].Name + ")";
                curveName = boatName + ", " + legName + ", " + tackName;
            }
            else if (_type == StatisticGroupType.Leg)
            {
                string legName = ssc.LegIndex.ToString() + " (" + _replay.Race.Course.Route[ssc.LegIndex.Value].Name + ")";
                curveName = boatName + ", " + legName;
            }
            else
            {
                curveName = boatName;
            }
            return curveName;
        }
        private void ConfigureGraph()
        {
            //make sure we only run one configure at a time.
            if (!_configuring)
            {
                _configuring = true;
                _curves = new List<CurveItem>();

                zg.GraphPane.Title.Text = _statisticName + " by " + _type.ToString();
                this.Text = zg.GraphPane.Title.Text;
                this.TabText = zg.GraphPane.Title.Text;
                zg.GraphPane.XAxis.Title.Text = "Time";
                _unit = _replay.Boats[0].TotalStatistics.GetStatisticUnit(_statisticName, _unitType);
                zg.GraphPane.YAxis.Title.Text = _statisticName + " (" + _unit.ToString() + ")";
                zg.GraphPane.CurveList.Clear();

                foreach (SelectedStatisticCell ssc in _statistics)
                {
                    SortedList<DateTime, double> data = new SortedList<DateTime, double>();
                    string curveName = GetSelectionLabel(ssc);
                    if (_type == StatisticGroupType.Tack)
                    {
                        data = _replay.Boats[ssc.BoatIndex.Value].TackStatistics[ssc.TackIndex.Value].GetGraphableTimeline(_statisticName, _unitType);
                    }
                    else if (_type == StatisticGroupType.Leg)
                    {
                        data = _replay.Boats[ssc.BoatIndex.Value].LegStatistics[ssc.LegIndex.Value].GetGraphableTimeline(_statisticName, _unitType);
                    }
                    else
                    {
                        data = _replay.Boats[ssc.BoatIndex.Value].TotalStatistics.GetGraphableTimeline(_statisticName, _unitType);
                    }

                    PointPairList graphData = new PointPairList();
                    LineItem curve = zg.GraphPane.AddCurve(curveName, graphData, _replay.Boats[ssc.BoatIndex.Value].Color, SymbolType.Diamond);
                    foreach (DateTime dt in data.Keys)
                    {
                        AddPoint(curve, dt, data[dt]);
                    }
                    curve.Symbol.Fill = new Fill(Color.White);
                    curve.Symbol.IsVisible = true;
                    curve.Symbol.Size = 3f;
                    curve.Line.IsOptimizedDraw = true;
                    _curves.Add(curve);
                }

                if (_enableOffset)
                {
                    List<int> usedColors = new List<int>();


                    _offset = DateTime.MaxValue;
                    //find the smallest date
                    //also alter the colors so that they are different
                    foreach (CurveItem ci in zg.GraphPane.CurveList)
                    {
                        if (ci.Points.Count > 0)
                        {
                            if (new XDate(ci[0].X).DateTime < _offset)
                            {
                                _offset = new XDate(ci[0].X).DateTime;
                            }
                        }


                    }

                    //now go through and apply the offset to all points
                    foreach (CurveItem ci in zg.GraphPane.CurveList)
                    {
                        if (ci.Points.Count > 0)
                        {
                            //no point offsetting if we're on the minimum
                            if (new XDate(ci[0].X).DateTime != _offset)
                            {
                                //determine the difference between this curve and the minimum curve
                                TimeSpan difference = new XDate(ci[0].X).DateTime - _offset.Value;
                                //now go through and offset each point
                                for (int i = 0; i < ci.Points.Count; i++)
                                {
                                    ci[i].X = new XDate(new XDate(ci[i].X).DateTime - difference);
                                }
                            }
                        }
                    }
                }
                else
                {
                    _offset = null;
                }

                DateTime simTime = _replay.SimulationTime;
                //_nowLine = new LineObj(Color.Blue,new XDate(simTime), _minYValue, new XDate(simTime), _maxYValue);
                //_nowLine.IsClippedToChartRect = true;
                //_nowLine.ZOrder = ZOrder.A_InFront;
                //_nowLine.IsVisible = true;
                //zg.GraphPane.GraphObjList.Add(_nowLine);

                zg.GraphPane.Legend.IsVisible = false;

                zg.GraphPane.XAxis.MajorGrid.IsVisible = true;
                zg.GraphPane.XAxis.Type = AxisType.Date;

                zg.IsShowHScrollBar = true;
                zg.IsShowVScrollBar = true;
                zg.IsAutoScrollRange = true;
                zg.IsShowPointValues = true;
                zg.AxisChange();
                zg.RestoreScale(zg.GraphPane);
                zg.Invalidate();
                _autoScrollSize = _defaultAutoScrollSize;
                _configuring = false;
            }
        }

        private void AddPoint(CurveItem curve,DateTime x, double y)
        {
            AddPoint(curve,(double)new XDate(x), y);
        }
        private void AddPoint(CurveItem curve,double x, double y)
        {
            if (x < _minXValue)
            {
                _minXValue = x;
            }
            if (x > _maxXValue)
            {
                _maxXValue = x;
            }
            if (y < _minYValue)
            {
                _minYValue = y;
            }
            if (y > _maxYValue)
            {
                _maxYValue = y;
            }
            curve.AddPoint(x, y);
        }

        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _run = false;
            UnwireDataEvents();
        }
    }
}
