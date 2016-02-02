using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Threading;

using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.Data.Statistics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AmphibianSoftware.VisualSail.UI
{
    public enum InstrumentDrawing { Line, InwardArrow, OutwardArrow };
    public delegate void ShutdownViewPort(IViewPort vp);
    public class Replay:IDisposable /*: Microsoft.Xna.Framework.Game*/
    {
        private Renderer _renderer;
        
        private Thread _mainLoopThread;
        private bool _run = true;//keeps the thread running
        private DateTime _simTime;//current sim time
        private DateTime _previousRenderTime;//real time of last draw/move
        private DateTime _previousMoveTime;
        private DateTime? _jumpToTime;//if not null engine will proceed as quickly as possible to this value
        private DateTime _highWaterMark;//latest point in time where the engine has run (and gathered statistics)
        private DateTime _lowWaterMark;//earlierst point in time where the engine has run (may or may not be beginning of race, may or may not include stats)
        private double _speed=1.0;//speed at which we're running relative to real time.
        private List<ReplayBoat> _boats;
        private Race _race;
        private float _windAngle = 0f;
        
        private Notify _updateTime;
        private Notify _updateStatistics;
        private DateTime _lastUpdateStatistics;
        private Random _random = new Random();
        
        public Replay(Race race,Renderer renderer,Notify updateStatistics, Notify updateTime)
        {
            _renderer = renderer;
            race.SetReplayTimes();
            _updateStatistics = updateStatistics;
            _updateTime = updateTime;
            _race = race;
            LoadBoats();
            _renderer.Initialize(this);
            Reset();
        }
        public void Reset()
        {
            //SetOffsets();
            
            if (_race.Course.DirectionType == Course.WindDirectionType.ConstantManual)
            {
                _windAngle = (float)_race.Course.ManualWindAngle+(3f*MathHelper.PiOver2);
            }
            else if (_race.Course.DirectionType == Course.WindDirectionType.ConstantCourse)
            {
                if (_race.Course.WindFromMark != null && _race.Course.WindToMark != null)
                {
                    CoordinatePoint from = _race.Course.WindFromMark.AveragedLocation;
                    CoordinatePoint to = _race.Course.WindToMark.AveragedLocation;
                    _windAngle = (float)AngleHelper.FindAngle(to.Project(), from.Project());
                }
            }
            _renderer.Reset();
        }
        private void UpdateStatistics()
        {
            if (_lastUpdateStatistics == null || DateTime.Now - _lastUpdateStatistics >= new TimeSpan(0, 0, 1))
            {
                _updateStatistics.BeginInvoke(null, null);
                _lastUpdateStatistics = DateTime.Now;
            }
        }
        private new void LoadBoats()
        {
            //LoadBouys();
            _boats = new List<ReplayBoat>();
            for(int i=0;i<_race.Boats.Count;i++)
            {
                Boat r = _race.Boats[i];
                ReplayBoat b = ReplayBoat.FromBoat(ref r/*, new Notify(this.UpdateStatistics)*/);
                _race.Boats[i] = r;
                //b.LoadResources(_device, _content,_race.UtcReplayStart);
                _boats.Add(b);
            }

            //foreach (AmphibianSoftware.VisualSail.Library.Bouy b in _bouys)
            //{
                //b.LoadResources(_device, _content);
            //}
            //_skyTexture = LoadAndScaleTexture(ContentHelper.ContentPath + "average_day.jpg", _device);
            //_mouseTexture = _content.Load<Texture2D>(ContentHelper.ContentPath + "mouse");
            //_mouseLeftTexture = _content.Load<Texture2D>(ContentHelper.ContentPath + "mouse-left");
            //_mouseRightTexture = _content.Load<Texture2D>(ContentHelper.ContentPath + "mouse-right");

            //_lakeTextureEffect = new BasicEffect(_device, null);
            //if (!File.Exists(ContentHelper.ContentPath + SatelliteImageryHelper.GetFileName(_race.Lake.North, _race.Lake.South, _race.Lake.East, _race.Lake.West)))
            //{
            //    try
            //    {
            //        string lakeFile = SatelliteImageryHelper.GetSatelliteImage(_race.Lake.North, _race.Lake.South, _race.Lake.East, _race.Lake.West, (int)_race.Lake.WidthInMeters / 10, (int)_race.Lake.HeightInMeters / 10);
            //        FileInfo fi = new FileInfo(lakeFile);
            //        fi.MoveTo(ContentHelper.ContentPath + SatelliteImageryHelper.GetFileName(_race.Lake.North, _race.Lake.South, _race.Lake.East, _race.Lake.West));
            //    }
            //    catch //(Exception e)
            //    {
            //    }
            //}
            //try
            //{
            //    _lakeTexture = LoadAndScaleTexture(ContentHelper.ContentPath + SatelliteImageryHelper.GetFileName(_race.Lake.North, _race.Lake.South, _race.Lake.East, _race.Lake.West), _device);
            //    //_lakeTexture = Texture2D.FromFile(device, ContentHelper.ContentPath + SatelliteImageryHelper.GetFileName(_race.Lake.North, _race.Lake.South, _race.Lake.East, _race.Lake.West));
            //    //_lakeTexture = content.Load<Texture2D>(ContentHelper.ContentPath + SatelliteImageryHelper.GetFileName(_race.Lake.North, _race.Lake.South, _race.Lake.East, _race.Lake.West));
            //    _lakeTextureEffect.Texture = _lakeTexture;
            //    _lakeTextureEffect.TextureEnabled = true;
            //    _lakeTextureAvailible = true;

            //    //lakeTextureEffect.FogEnabled = true;
            //    //lakeTextureEffect.FogColor = Color.White.ToVector3();
            //    //lakeTextureEffect.FogStart = Camera.FarClipDistance - 300;
            //    //lakeTextureEffect.FogEnd = Camera.FarClipDistance;
            //    //MessageBox.Show("Loaded lake texture:"+_lakeTexture.Width+"x"+_lakeTexture.Height);
            //}
            //catch(Exception /*e*/)
            //{
            //    //MessageBox.Show("Failed to load area texture." + e.Message + ":" + e.StackTrace);
            //    _lakeTextureAvailible = false;
            //}

            //_skyBoxEffect = new BasicEffect(_device, null);
            //_skyBoxEffect.Texture = _skyTexture;
            //_skyBoxEffect.TextureEnabled = true;
            //_skyBoxEffect.FogEnabled = true;
            //_skyBoxEffect.FogColor = Color.White.ToVector3();
            //_skyBoxEffect.FogStart = Camera.FarClipDistance - 300;
            //_skyBoxEffect.FogEnd = Camera.FarClipDistance;

            //water = new BasicEffect(device, null);
            //water.TextureEnabled = true;
            //water.Alpha = 0.5f;
            //water.Texture = waterTexture;

            //effect = new BasicEffect(device, null);
            //effect.EnableDefaultLighting();

            //_instruments = new BasicEffect(_device, null);
            //_instruments.VertexColorEnabled = true;

            //text = new BasicEffect(device, null);

            //_font = _content.Load<SpriteFont>(ContentHelper.ContentPath + "tahoma");
            //_batch = new SpriteBatch(_device);
            //_line = new PrimitiveLine(_device);

            //_photos = Photo.FindInDateRange(_race.LocalCountdownStart, _race.LocalEnd);
        }
        public DateTime SimulationTime
        {
            get
            {
                return _simTime;
            }
        }
        public DateTime? TargetTime
        {
            get
            {
                return _jumpToTime;
            }
            set
            {
                _jumpToTime = value;
                UpdateTime();
            }
        }
        public double Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }
        
        public DataSet GetFullStatistics(StatisticUnitType unittype)
        {
            //get
            //{
                DataSet ds = new DataSet("Statistics");
                lock (_boats)
                {
                    DataTable boats = new DataTable("Boats");
                    boats.Columns.Add(new DataColumn("id", typeof(int)));
                    boats.Columns.Add(new DataColumn("name", typeof(string)));
                    boats.Columns.Add(new DataColumn("number", typeof(string)));
                    boats.Columns.Add(new DataColumn("color", typeof(System.Drawing.Color)));

                    foreach (ReplayBoat b in _boats)
                    {
                        object[] row = new object[4];
                        row[0] = b.Id;
                        row[1] = b.Name;
                        row[2] = b.Number;
                        row[3] = b.Color;
                        boats.Rows.Add(row);
                    }
                    ds.Tables.Add(boats);

                    DataTable legs = new DataTable("Legs");
                    DataColumn dc = legs.Columns.Add("index", typeof(int));
                    legs.Columns.Add("description", typeof(string));

                    for (int i = 0; i <= _race.Course.Route.Count; i++)
                    {
                        string description = "";
                        if (i == 0)
                        {
                            description = "Pre-Start";
                        }
                        else if (i > 0 && i < _race.Course.Route.Count)
                        {
                            description = i + " " + _race.Course.Route[i - 1].Name + " to " + _race.Course.Route[i].Name;
                        }
                        else if (i == _race.Course.Route.Count)
                        {
                            description = "Post-Finish";
                        }
                        else
                        {
                            throw new Exception("Invalid Course Leg Reached");
                        }
                        object[] row = { i, description };
                        legs.Rows.Add(row);
                    }

                    object[] row2 = { DBNull.Value, "Overall" };
                    legs.Rows.Add(row2);

                    ds.Tables.Add(legs);

                    DataTable tacks = new DataTable("Tacks");
                    DataColumn boatId = tacks.Columns.Add("boat_id", typeof(int));
                    DataColumn legIndex = tacks.Columns.Add("leg_index", typeof(int));
                    DataColumn tackIndex = tacks.Columns.Add("tack_index", typeof(int));
                    DataColumn tackname = tacks.Columns.Add("name", typeof(string));



                    DataTable stats = new DataTable("Statistics");
                    stats.Columns.Add("boat_id", typeof(int));
                    DataColumn dc2 = stats.Columns.Add("leg_index", typeof(int));
                    dc2.AllowDBNull = true;
                    DataColumn dc3 = stats.Columns.Add("tack_index", typeof(int));
                    dc3.AllowDBNull = true;

                    DataTable statinfo = new DataTable("StatisticInfo");
                    statinfo.Columns.Add("name", typeof(string));
                    statinfo.Columns.Add("type", typeof(string));
                    statinfo.Columns.Add("unit", typeof(string));
                    statinfo.Columns.Add("description", typeof(string));

                    TimeLineStatisticCollection example = _boats[0].TotalStatistics;
                    foreach (Type statType in example.StatisticDirectory.Keys)
                    {
                        foreach (string name in example.StatisticDirectory[statType])
                        {
                            if (!stats.Columns.Contains(name))
                            {
                                stats.Columns.Add(name, statType/*example[name].GetValue(unittype, _simTime).GetType()*/);

                                object[] info = null;

                                if (unittype == StatisticUnitType.metric)
                                {
                                    object[] t = { name, statType.ToString(), example.GetStatisticMetricUnit(statType,name).ToString(), "" };
                                    info = t;
                                }
                                else if (unittype == StatisticUnitType.standard)
                                {
                                    object[] t = { name, statType.ToString(), example.GetStatisticStandardUnit(statType, name).ToString(), "" };
                                    info = t;
                                }

                                statinfo.Rows.Add(info);
                            }
                        }
                    }
                    ds.Tables.Add(statinfo);

                    foreach (ReplayBoat b in _boats)
                    {
                        DataRow dr = stats.NewRow();
                        dr["boat_id"] = b.Id;
                        dr["leg_index"] = System.DBNull.Value;
                        dr["tack_index"] = System.DBNull.Value;
                        TimeLineStatisticCollection fullstats = b.TotalStatistics;
                        foreach (Type t in fullstats.StatisticDirectory.Keys)
                        {
                            foreach (string s in fullstats.StatisticDirectory[t])
                            {
                                dr[s] = fullstats.GetValue(t, s, _simTime, unittype);
                            }
                        }
                        stats.Rows.Add(dr);

                        List<TimeLineStatisticCollection> legstats = b.LegStatistics;
                        for (int i = 0; i < legstats.Count && i<=b.GetCurrentLeg(_simTime); i++)
                        {
                            DataRow d = stats.NewRow();
                            d["boat_id"] = b.Id;
                            d["leg_index"] = i;
                            d["tack_index"] = DBNull.Value;
                            foreach (Type t in legstats[i].StatisticDirectory.Keys)
                            {
                                foreach (string s in legstats[i].StatisticDirectory[t])
                                {
                                    d[s] = legstats[i].GetValue(t, s, _simTime, unittype);
                                }
                            }
                            stats.Rows.Add(d);
                        }

                        lock (b.Tacks)
                        {
                            for(int i=0;i<b.Tacks.Count && i<=b.GetCurrentTack(_simTime);i++)
                            {
                                DataRow d = tacks.NewRow();
                                d["boat_id"] = b.Id;
                                d["leg_index"] = b.Tacks[i].LegIndex;
                                d["tack_index"] = b.Tacks[i].Index;
                                d["name"] = (b.Tacks[i].IndexOnLeg + 1) + " " + b.Tacks[i].Direction.ToString();
                                tacks.Rows.Add(d);

                                TimeLineStatisticCollection tackstats = b.TackStatistics[b.Tacks[i].Index];

                                DataRow td = stats.NewRow();
                                td["boat_id"] = b.Id;
                                td["leg_index"] = b.Tacks[i].LegIndex;
                                td["tack_index"] = b.Tacks[i].Index;
                                foreach (Type tt in tackstats.StatisticDirectory.Keys)
                                {
                                    foreach (string s in tackstats.StatisticDirectory[tt])
                                    {
                                        td[s] = tackstats.GetValue(tt, s, _simTime, unittype);
                                    }
                                }
                                stats.Rows.Add(td);
                            }
                        }
                    }
                    ds.Tables.Add(stats);
                    ds.Tables.Add(tacks);
                }
                return ds;
            //}
        }
        public Race Race
        {
            get
            {
                return _race;
            }
        }
        public void Start()
        {
            _previousRenderTime = DateTime.Now;
            _previousMoveTime = DateTime.Now;
            _simTime = _race.UtcCountdownStart;
            _highWaterMark = _simTime;
            _lowWaterMark = _simTime;
            _run = true;
            _mainLoopThread = new Thread(new ThreadStart(this.MainLoop));
            _mainLoopThread.Start();
        }
        public void Stop()
        {
            _run = false;
            Thread.Sleep(1000);
            if (_mainLoopThread.ThreadState == ThreadState.Running)
            {
                _mainLoopThread.Abort();
            }
            //if (_xnaAviWriter != null && _xnaAviWriter.Recording)
            //{
            //    _xnaAviWriter.Close();
            //}
        }
        public void Dispose()
        {
            _boats = null;
        }
        public void Play()
        {
            //_play = true;
            if (_speed <= 0)
            {
                _speed = 1f;
            }
        }
        public void Pause()
        {
            //_play = false;
            _speed = 0f;
        }
        
        private void UpdateTime()
        {
            if (_simTime < _race.UtcReplayStart)
            {
                _race.UtcReplayStart = _simTime;
            }
            if (_simTime > _race.UtcReplayEnd)
            {
                _race.UtcReplayEnd = _simTime;
            }
            if (_updateTime != null)
            {
                _updateTime();
            }
        }
        private void MainLoop()
        {
            TimeSpan inc = new TimeSpan(0, 0, 0, 1, 0);
            while (_run)
            {
                DateTime now = DateTime.Now;
                if (_jumpToTime != null)
                {
                    if (_simTime < _jumpToTime.Value)
                    {
                        while (_simTime < _jumpToTime.Value)
                        {
                            if (_jumpToTime.Value > _highWaterMark && _simTime + inc < _jumpToTime.Value)
                            {
                                _simTime = _simTime + inc;
                                UpdateStatistics();
                                _renderer.RenderAll();
                            }
                            else
                            {
                                _simTime = _jumpToTime.Value;
                            }

                            foreach (ReplayBoat b in _boats)
                            {
                                b.Move(_simTime, _race.Course, _windAngle, _race.UtcReplayStart);
                            }
                            SetPositions();

                            UpdateTime();
                        }
                    }
                    else if (_simTime > _jumpToTime.Value)
                    {
                        _simTime = _jumpToTime.Value;
                        foreach (ReplayBoat b in _boats)
                        {
                            b.Move(_simTime, _race.Course, _windAngle, _race.UtcReplayStart);
                        }
                    }
                    _jumpToTime=null;

                    
                    UpdateTime();
                    UpdateStatistics();
                    _renderer.RenderAll();
                }
                else if(_speed!=0)
                {
                    TimeSpan realSpan = now - _previousMoveTime;
                    TimeSpan simSpan = new TimeSpan((long)(realSpan.Ticks * (long)_speed));
                    DateTime to = _simTime + simSpan;

                    if (_simTime < to)
                    {
                        while (_simTime < to && _speed != 0)
                        {
                            if ((to > _highWaterMark || to < _lowWaterMark) && _simTime + inc < to)
                            {
                                _simTime = _simTime + inc;
                                UpdateStatistics();
                                _renderer.RenderAll();
                            }
                            else
                            {
                                _simTime = to;
                            }

                            foreach (ReplayBoat b in _boats)
                            {
                                b.Move(_simTime, _race.Course, _windAngle, _race.UtcReplayStart);
                            }
                            SetPositions();

                            UpdateTime();
                        }
                    }
                    else if (_simTime > to)
                    {
                        _simTime = to;
                        foreach (ReplayBoat b in _boats)
                        {
                            b.Move(_simTime, _race.Course, _windAngle, _race.UtcReplayStart);
                        }
                    }

                    UpdateTime();
                    UpdateStatistics();
                    _renderer.RenderAll();
                }
                else
                {
                    //RenderAll();
                    _renderer.RenderAll();
                }

                //if (_race.UtcReplayStart==_race.UtcCountdownStart && _lowWaterMark<_race.UtcReplayStart)
                //{
                //    this.Pause();
                //}
                //if (_race.UtcReplayEnd==_race.UtcEnd && _highWaterMark>_race.UtcReplayEnd)
                //{
                //    this.Pause();
                //}

                if (_simTime > _highWaterMark)
                {
                    _highWaterMark = _simTime;
                }
                if (_simTime < _lowWaterMark)
                {
                    _lowWaterMark = _simTime;
                }
                
                _previousMoveTime = now;
            }
        }
        private void SetPositions()
        {
            DataTable sorter = new DataTable();
            sorter.Columns.Add(new DataColumn("boat_id", typeof(int)));
            sorter.Columns.Add(new DataColumn("finished", typeof(bool)));
            sorter.Columns.Add(new DataColumn("finished_position",typeof(int)));
            sorter.Columns.Add(new DataColumn("mark_index", typeof(int)));
            sorter.Columns.Add(new DataColumn("distance_to_mark", typeof(float)));

            foreach (ReplayBoat b in _boats)
            {
                int pos=int.MaxValue;
                bool finished=false;
                if (b.CurrentRacingStatus == ReplayBoat.RacingStatus.Finished)
                {
                    pos=b.GetCurrentPosition(_simTime);
                    finished=true;
                }
                object[] row = { b.Id,finished,pos, b.GetCurrentLeg(_simTime), b.DistanceToMark };
                sorter.Rows.Add(row);
            }

            DataView dv = new DataView(sorter, "", "finished desc,finished_position asc,mark_index desc, distance_to_mark asc", DataViewRowState.CurrentRows);
            DataTable sorted = dv.ToTable();

            ReplayBoat leader = null;
            int leaderBoatId = (int)sorted.Rows[0]["boat_id"];
            foreach (ReplayBoat b in _boats)
            {
                if (b.Id == leaderBoatId)
                {
                    leader = b;
                }
            }
            if (leader == null)
            {
                throw new Exception("Failed to find race leader");
            }

            for (int i = 0; i < sorted.Rows.Count; i++)
            {
                foreach (ReplayBoat b in _boats)
                {
                    if ((int)sorted.Rows[i]["boat_id"] == b.Id)
                    {
                        TimeSpan leaderLag = new TimeSpan();
                        TimeSpan nextLag = new TimeSpan();

                        int leg = b.GetCurrentLeg(_simTime);

                        if (leg > 0)
                        {
                            if (b.Id != leader.Id)
                            {
                                DateTime previousLeaderEnd = leader.GetLegEndTime(leg - 1, _simTime);
                                DateTime previousCurrentEnd = b.GetLegEndTime(leg - 1, _simTime);
                                leaderLag = new TimeSpan(previousLeaderEnd.Ticks - previousCurrentEnd.Ticks);
                            }

                            if (i > 0)
                            {
                                ReplayBoat nextBoat = null;
                                int nextId = (int)sorted.Rows[i - 1]["boat_id"];
                                foreach (ReplayBoat b2 in _boats)
                                {
                                    if (b2.Id == nextId)
                                    {
                                        nextBoat = b2;
                                    }
                                }

                                DateTime previousNextEnd = nextBoat.GetLegEndTime(leg - 1, _simTime);
                                DateTime previousCurrentEnd = b.GetLegEndTime(leg - 1, _simTime);
                                nextLag = new TimeSpan(previousNextEnd.Ticks - previousCurrentEnd.Ticks);
                            }
                        }

                        b.SetPosition(i + 1, _simTime, leaderLag, nextLag);

                        break;
                    }
                }
            }
        }
        public List<ReplayBoat> Boats
        {
            get
            {
                return _boats;
            }
        }
        public float WindAngle
        {
            get
            {
                return _windAngle;
            }
        }
        
        private string ExtractBoatStatisticString(ReplayBoat b, IViewPort target, string name, DateTime time)
        {
            string speedString = name+": ";
            speedString = speedString + string.Format("{0:0.##}", b.TotalStatistics.GetValue<float>(name, time, target.StatisticUnitType));
            if (target.StatisticUnitType == StatisticUnitType.metric)
            {
                speedString = speedString + " " + b.TotalStatistics.GetStatisticMetricUnit(typeof(float), name);
            }
            else if (target.StatisticUnitType == StatisticUnitType.standard)
            {
                speedString = speedString + " " + b.TotalStatistics.GetStatisticStandardUnit(typeof(float), name);
            }
            return speedString;
        }

        public void RefreshViewports()
        {
        //    foreach (IViewPort vp in _viewports.Keys)
        //    {
        //        Dictionary<AmphibianSoftware.VisualSail.Library.Boat, int> offsets = vp.SetBoatList(_boats);
        //        _viewportOffsets[vp] = offsets;
        //    }
        }
        public void AddViewPort(IViewPort viewport)
        {
            _renderer.AddViewPort(viewport);
            //viewport.SetMaxSize(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            //CameraMan man = new CameraMan();
            //man.Camera = new Camera(_worldBounds);
            //man.HorizontalRotation = 0f;
            //man.VerticalRotation =  (float)(Math.PI) + (((float)Math.PI) / 20f) + (((float)Math.PI) / 20f);
            //man.Zoom = 10f;
            //man.SelectedBoat = 0;
            //man.DrawSatelliteImagery = _lakeTextureAvailible;
            //viewport.CameraMan = man;
            ////List<string> boatNames = new List<string>();
            ////List<string> boatNumbers = new List<string>();
            ////List<System.Drawing.Color> boatColors = new List<System.Drawing.Color>();
            ////foreach (AmphibianSoftware.VisualSail.Library.Boat b in boats)
            ////{
            ////    boatNames.Add(b.Name);
            ////    boatNumbers.Add(b.Number);
            ////    boatColors.Add(b.Color);
            ////}
            //Dictionary<AmphibianSoftware.VisualSail.Library.Boat, int> offsets = viewport.SetBoatList(_boats);
            //viewport.Shutdown = new ShutdownViewPort(this.RemoveViewPort);
            //lock (_viewports)
            //{
            //    _viewports.Add(viewport, man);
            //}
            //lock (_viewportOffsets)
            //{
            //    _viewportOffsets.Add(viewport, offsets);
            //}
        }
        public void RemoveViewPort(IViewPort viewport)
        {
            _renderer.RemoveViewPort(viewport);
            //lock (_viewports)
            //{
            //    if(_viewports.ContainsKey(viewport) && _viewports[viewport].CurrentPhotoTexture!=null)
            //    {
            //        _viewports[viewport].CurrentPhotoTexture.Dispose();
            //        _viewports[viewport].CurrentPhotoTexture = null;
            //    }
            //    _viewports.Remove(viewport);
            //}
        }
        public SortedList<DateTime,double> GetStatisticTimeline(int boatIndex,int? legIndex,int? tackIndex,string statistic,StatisticUnitType unit)
        {
            if (tackIndex.HasValue)
            {
                int realTackIndex = 0;
                while (_boats[boatIndex].Tacks[realTackIndex].LegIndex != legIndex.Value && _boats[boatIndex].Tacks[realTackIndex].IndexOnLeg != tackIndex.Value && realTackIndex < _boats[boatIndex].Tacks.Count)
                {
                    realTackIndex++;
                }
                if (_boats[boatIndex].Tacks[realTackIndex].LegIndex == legIndex.Value && _boats[boatIndex].Tacks[realTackIndex].IndexOnLeg == tackIndex.Value && realTackIndex < _boats[boatIndex].Tacks.Count)
                {
                    return _boats[boatIndex].TackStatistics[realTackIndex].GetGraphableTimeline(statistic, unit);
                }
                else
                {
                    throw new Exception("Could not find the specified statistic timeline");
                }
            }
            else if (legIndex.HasValue)
            {
                return _boats[boatIndex].LegStatistics[legIndex.Value].GetGraphableTimeline(statistic, unit);
            }
            else
            {
                return _boats[boatIndex].TotalStatistics.GetGraphableTimeline(statistic, unit);
            }
        }
    }
}