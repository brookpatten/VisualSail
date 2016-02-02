
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.Data.Statistics;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.PostBuild;

namespace AmphibianSoftware.VisualSail.Library
{
    public class ReplayBoat:Boat
    {
        [DoNotObfuscate()]
        private enum InstrumentDrawing { Line, InwardArrow, OutwardArrow };
        [DoNotObfuscate()]
        public enum RacingStatus { Starting, Racing, Finished, NotRacing };
        [DoNotObfuscate()]
        public enum BoatDirection {Forward,Backwards};
        [DoNotObfuscate()]
        public enum CourseDirection { Upwind, Downwind };
        [DoNotObfuscate()]
        public enum PointOfSail { Irons, CloseHauled, BeamReach, BroadReach, Running };
        //private BasicEffect sailEffect;
        //private BasicEffect hudEffect;
        //private Sail sail;
        //private Sail jib;
        private ProjectedPoint _projectedPoint;
        private float arrowY = 0f;
        private float arrowAngle;
        private float windAngle = 0f;
        private float currentHeel = 0f;
        private float currentBoomAngle = 0f;
        private float currentSailCurve = 0f;
        private float waterLine = 1f;
        private float speed;
        //private Model boat;
        //private Notify _updateStatistics;
        //private Vector3 _color;
        private float _markRoundDistance = 20f;
        //private AmphibianSoftware.VisualSail.Data.Boat _boatData;
        private SkipperDataSet.SensorReadingsDataTable _boatDataRows;
        private DateTime _lastUpdate;
        private int _currentBoatDataRow = 0;
        private BoatDirection direction;
        private TimeLineStatistic<int> _currentMarkIndex;
        private TimeLineStatistic<int> _currentTackIndex;
        private Mark _currentMark;
        private Mark _previousMark;
        //private VertexPositionColor[] _pathCurve;
        //private SortedList<int, int> _boatDataRowsCurveMap;
        //private List<float> _boatDataRowsDistances;
        //private List<Vector3> _pathControlPoints;
        private TimeLineStatisticCollection statistics;
        private List<TimeLineStatisticCollection> legStatistics;
        private List<TimeLineStatisticCollection> tackStatistics;
        private List<Tack> _tacks;
        private bool _positionInitialized = false;

        private DateTime? _lowWaterMark;
        private DateTime? _highWaterMark;

        private ReplayBoat(SkipperDataSet.BoatRow row)
        {
            LoadFromRow(row);
            _currentMarkIndex = new TimeLineStatistic<int>(new Raw<int>("Current Mark", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Int32.Calculator(), 0, StatisticType.other, StatisticUnit.other, StatisticUnit.other, "", false));
            _currentTackIndex = new TimeLineStatistic<int>(new Raw<int>("Current Tack", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Int32.Calculator(), 1, StatisticType.other, StatisticUnit.other, StatisticUnit.other, "", false));
            _tacks = new List<Tack>();
            _boatDataRows = GetSensorReadings();
            InitializeStatistics();
        }
        public static ReplayBoat FromBoat(ref Boat boat/*,DateTime start, DateTime end*//*,Notify updateStatistics*/)
        {
            ReplayBoat newBoat = ReplayBoat.FindById(boat.Id);
            boat = newBoat;
            return newBoat;
        }
        
        private void InitializeStatistics()
        {
            statistics = TimeLineStatisticCollection.CreateDefaultStatisticCollection();
            statistics.ReplayBoat = this;
            legStatistics = new List<TimeLineStatisticCollection>();
            tackStatistics = new List<TimeLineStatisticCollection>();
        }
        public RacingStatus CurrentRacingStatus
        {
            get
            {
                if (_previousMark == null && _currentMark == null)
                {
                    return RacingStatus.NotRacing;
                }
                else if (_previousMark != null && _currentMark == null)
                {
                    return RacingStatus.Finished;
                }
                else if (_previousMark == null && _currentMark != null)
                {
                    return RacingStatus.Starting;
                }
                else if (_previousMark != null && _currentMark != null)
                {
                    return RacingStatus.Racing;
                }
                else
                {
                    throw new Exception("Unknown race state");
                }
            }
        }
        private void UpdateStatistics(DateTime now,double distance)
        {
            if (CurrentRacingStatus == RacingStatus.NotRacing)
            {
                UpdateStatisticGroup(statistics, now,distance, true);
            }
            else
            {
                while (legStatistics.Count <= _currentMarkIndex.GetValue(now))
                {
                    TimeLineStatisticCollection ls = TimeLineStatisticCollection.CreateDefaultStatisticCollection();
                    ls.legIndex = _currentMarkIndex.GetValue(now);
                    ls.ReplayBoat = this;
                    legStatistics.Add(ls);
                }

                while (tackStatistics.Count <= _currentTackIndex.GetValue(now))
                {
                    TimeLineStatisticCollection ts = TimeLineStatisticCollection.CreateDefaultStatisticCollection();
                    ts.ReplayBoat = this;
                    ts.legIndex = _currentMarkIndex.GetValue(now);
                    ts.tackIndex = _currentTackIndex.GetValue(now);
                    tackStatistics.Add(ts);
                }

                if ((_previousMark==null || _previousMark.DistanceTo(this.Location.Project()) > _markRoundDistance) && (_currentMark==null || _currentMark.DistanceTo(this.Location.Project()) > _markRoundDistance))
                {
                    UpdateStatisticGroup(legStatistics[_currentMarkIndex.GetValue(now)], now, distance, false);
                }

                if (CurrentRacingStatus == RacingStatus.Racing)
                {
                    UpdateStatisticGroup(statistics, now,distance, true);

                    if (_previousMark.DistanceTo(this.Location.Project()) > _markRoundDistance && _currentMark.DistanceTo(this.Location.Project()) > _markRoundDistance)
                    {
                        UpdateStatisticGroup(tackStatistics[_currentTackIndex.GetValue(now)], now, distance, false);
                    }
                }
            }
        }
        private void UpdateStatisticGroup(TimeLineStatisticCollection stats, DateTime now,double distance, bool isCurrent)
        {
            stats.AddValue<int>("Current Leg",now, _currentMarkIndex.GetValue(now));
            stats.AddValue<int>("Current Tack",now, _currentTackIndex.GetValue(now));
            
            stats.AddValue<float>("Speed", now, speed);
            //stats.AddValue<float>("Speed (10 Second Average)",now, speed);
            stats.AddValue<float>("VMG to Wind",now, VelocityAgainstWind);
            stats.AddValue<float>("VMG to Mark",now, VelocityToMark);
            stats.AddValue<float>("VMG to Course", now, VelocityOnCourse);
            stats.AddValue<float>("Average VMG to Wind", now, VelocityAgainstWind);
            stats.AddValue<float>("Average VMG to Mark", now, VelocityToMark);
            stats.AddValue<float>("Average VMG to Course", now, VelocityOnCourse);
            stats.AddValue<float>("Distance to Mark",now, DistanceToMark);
            stats.AddValue<float>("Distance to Course",now, DistanceToStraightCourse);
            stats.AddValue<float>("Angle to Mark", now, MathHelper.ToDegrees((float)Math.Abs(RelativeAngleToMark)));
            stats.AddValue<float>("Angle to Wind",now, MathHelper.ToDegrees((float)Math.Abs(RelativeAngleToWind)));
            stats.AddValue<float>("Angle to Course", now, MathHelper.ToDegrees((float)Math.Abs(RelativeAngleToCourse)));
            
            stats.AddValue<float>("Average Speed",now, speed);
            stats.AddValue<float>("Minimum Speed",now, speed);
            stats.AddValue<float>("Maximum Speed",now, speed);
            stats.AddValue<float>("Start Distance to Mark",now, DistanceToMark);
            stats.AddValue<float>("End Distance to Mark",now, DistanceToMark);
            stats.AddValue<float>("Average Angle to Wind",now, MathHelper.ToDegrees((float)Math.Abs(RelativeAngleToWind)));
            stats.AddValue<float>("Average Angle to Course", now, MathHelper.ToDegrees((float)Math.Abs(RelativeAngleToCourse)));
            stats.AddValue<float>("Distance Covered", now, (float)distance);
            
            stats.AddValue<DateTime>("Start Time", now, now);
            stats.AddValue<DateTime>("End Time", now, now);

            DateTime start = stats.GetValue<DateTime>("Start Time", now);
            DateTime end = stats.GetValue<DateTime>("End Time", now);
            stats.AddValue<TimeSpan>("Elapsed", now, new TimeSpan(end.Ticks - start.Ticks));
        }
        public float GetSpeed(DateTime now)
        {
            return statistics.GetValue<float>("Speed",now);
        }
        public SkipperDataSet.SensorReadingsDataTable SensorReadings
        {
            get
            {
                return _boatDataRows;
            }
        }
        public int CurrentSensorReadingIndex
        {
            get
            {
                return _currentBoatDataRow;
            }
        }
        public TimeLineStatisticCollection TotalStatistics
        {
            get
            {
                return statistics;
            }
        }
        public List<TimeLineStatisticCollection> LegStatistics
        {
            get
            {
                return legStatistics;
            }
        }
        public List<TimeLineStatisticCollection> TackStatistics
        {
            get
            {
                return tackStatistics;
            }
        }
        public List<Tack> Tacks
        {
            get
            {
                return _tacks;
            }
        }
        public float Angle
        {
            get
            {
                return arrowAngle;
            }
        }
        public BoatDirection Direction
        {
            get
            {
                return direction;
            }
        }
        public float RelativeAngleToMark
        {
            get
            {
                float angle = AngleHelper.AngleDifference(Angle, AngleToMark);
                return angle;
            }
        }
        public float AngleToMark
        {
            get
            {
                if (_currentMark != null && ProjectedPoint != null)
                {
                    float angle = (float)AngleHelper.FindAngle(_currentMark.AveragedLocation.Project(), ProjectedPoint);
                    return angle;
                }
                else
                {
                    return 0;
                }
            }
        }
        public float RelativeAngleToWind
        {
            get
            {
                return AngleHelper.AngleDifference(AngleHelper.NormalizeAngle(windAngle + MathHelper.Pi), arrowAngle);
            }
        }
        public float RelativeAngleToCourse
        {
            get
            {
                if (_previousMark != null && _currentMark != null)
                {
                    float courseAngle = (float)AngleHelper.FindAngle(_currentMark.AveragedLocation.Project(), _previousMark.AveragedLocation.Project());
                    float boatAngle = Angle;
                    return AngleHelper.AngleDifference(courseAngle, boatAngle);
                }
                else
                {
                    return 0;
                }
            }
        }
        public Tack.TackDirection CurrentTackDirection
        {
            get
            {
                if (RelativeAngleToWind > 0f)
                {
                    return Tack.TackDirection.Port;
                }
                else
                {
                    return Tack.TackDirection.Starboard;
                }
            }
        }
        public CourseDirection CurrentCourseDirection
        {
            get
            {
                if (Math.Abs(RelativeAngleToWind) > MathHelper.PiOver2)
                {
                    return CourseDirection.Downwind;
                }
                else
                {
                    return CourseDirection.Upwind;
                }
            }
        }
        public PointOfSail CurrentPointOfSail
        {
            get
            {
                float angle = MathHelper.ToDegrees(Math.Abs(RelativeAngleToWind));
                if (angle < 22.5)
                {
                    return PointOfSail.Irons;
                }
                else if (angle >= 22.5f && angle < 67.5f)
                {
                    return PointOfSail.CloseHauled;
                }
                else if (angle >= 67.5f && angle < 112.5f)
                {
                    return PointOfSail.BeamReach;
                }
                else if (angle >= 112.5f && angle < 157.5f)
                {
                    return PointOfSail.BroadReach;
                }
                else if (angle >= 157.5f && angle <=180f)
                {
                    return PointOfSail.Running;
                }
                else
                {
                    throw new Exception("Invalid Wind Angle");
                }
            }
        }
        public int GetCurrentLeg(DateTime time)
        {
            return _currentMarkIndex.GetValue(time);
        }
        public int GetCurrentPosition(DateTime time)
        {
            return statistics.GetValue<int>("Position", time);
        }
        public DateTime GetLegEndTime(int legIndex,DateTime time)
        {
            if (legIndex == 0 && _currentMarkIndex.GetValue(time)>=1)
            {
                return legStatistics[legIndex+1].GetValue<DateTime>("Start Time", time);
            }
            else if (legIndex!=0 && legIndex <= _currentMarkIndex.GetValue(time))
            {
                return legStatistics[legIndex].GetValue<DateTime>("End Time", time);
            }
            else
            {
                return time;
            }
        }
        public int GetCurrentTack(DateTime time)
        {
            return (int)_currentTackIndex.GetValue(time);
        }
        public void SetPosition(int position, DateTime time, TimeSpan leaderLag, TimeSpan nextLag)
        {
            if (CurrentRacingStatus == RacingStatus.Racing)
            {
                if (statistics.GetValue<int>("Position", time) != position)
                {
                    statistics.AddValue<int>("Position Changes", time, 1);
                    if (legStatistics.Count > _currentMarkIndex.GetValue(time))
                    {
                        legStatistics[_currentMarkIndex.GetValue(time)].AddValue<int>("Position Changes", time, 1);
                        if (tackStatistics.Count > _currentTackIndex.GetValue(time))
                        {
                            tackStatistics[_currentTackIndex.GetValue(time)].AddValue<int>("Position Changes", time, 1);
                        }
                    }
                }
                
                statistics.AddValue<int>("Start Position", time, position);
                statistics.AddValue<int>("End Position", time, position);
                statistics.AddValue<TimeSpan>("Lag Behind Leader", time, leaderLag);
                statistics.AddValue<TimeSpan>("Lag Behind Next", time, nextLag);
                statistics.AddValue<int>("Position", time, position);
                if (legStatistics.Count > _currentMarkIndex.GetValue(time))
                {
                    legStatistics[_currentMarkIndex.GetValue(time)].AddValue<int>("Position", time, position);
                    legStatistics[_currentMarkIndex.GetValue(time)].AddValue<int>("Start Position", time, position);
                    legStatistics[_currentMarkIndex.GetValue(time)].AddValue<int>("End Position", time, position);
                    legStatistics[_currentMarkIndex.GetValue(time)].AddValue<TimeSpan>("Lag Behind Leader", time, leaderLag);
                    legStatistics[_currentMarkIndex.GetValue(time)].AddValue<TimeSpan>("Lag Behind Next", time, nextLag);
                    if (tackStatistics.Count > _currentTackIndex.GetValue(time))
                    {
                        tackStatistics[_currentTackIndex.GetValue(time)].AddValue<int>("Position", time, position);
                        tackStatistics[_currentTackIndex.GetValue(time)].AddValue<int>("Start Position", time, position);
                        tackStatistics[_currentTackIndex.GetValue(time)].AddValue<int>("End Position", time, position);
                        tackStatistics[_currentTackIndex.GetValue(time)].AddValue<TimeSpan>("Lag Behind Leader", time, leaderLag);
                        tackStatistics[_currentTackIndex.GetValue(time)].AddValue<TimeSpan>("Lag Behind Next", time, nextLag);
                    }
                }
            }
            else if (CurrentRacingStatus == RacingStatus.Finished)
            {
                statistics.AddValue<int>("Position", time, position);
                if (legStatistics.Count > _currentMarkIndex.GetValue(time))
                {
                    legStatistics[_currentMarkIndex.GetValue(time)].AddValue<int>("Position", time, position);
                    if (tackStatistics.Count > _currentTackIndex.GetValue(time))
                    {
                        tackStatistics[_currentTackIndex.GetValue(time)].AddValue<int>("Position", time, position);
                    }
                }
            }
        }
        public float Speed
        {
            get
            {
                return speed;
            }
        }
        public float VelocityAgainstWind
        {
            get
            {
                return (float)Math.Cos(Math.Abs(RelativeAngleToWind))*Speed;
            }
        }
        public float VelocityToMark
        {
            get
            {
                return (float)Math.Cos(Math.Abs(RelativeAngleToMark)) * Speed;
            }
        }
        public float VelocityOnCourse
        {
            get
            {
                return (float)Math.Cos(Math.Abs(RelativeAngleToCourse)) * Speed;
            }
        }
        public float DistanceToMark
        {
            get
            {
                if (_currentMark != null && ProjectedPoint != null)
                {
                    return (float)_currentMark.DistanceTo(ProjectedPoint);
                }
                else
                {
                    return 0;
                }
            }
        }
        public float DistanceToStraightCourse
        {
            get
            {
                if (_currentMark != null && _previousMark != null && ProjectedPoint != null)
                {
                    return (float)GeometryHelper.DistancePointToLineSegment(_currentMark.AveragedLocation.Project().Easting, _currentMark.AveragedLocation.Project().Northing, _previousMark.AveragedLocation.Project().Easting, _previousMark.AveragedLocation.Project().Northing, ProjectedPoint.Easting, ProjectedPoint.Northing);
                }
                else
                {
                    return 0;
                }
            }
        }
        
        public Mark CurrentMark
        {
            get
            {
                return _currentMark;
            }
        }
        public System.Drawing.Color Color
        {
            get
            {
                return System.Drawing.Color.FromArgb(base.Color);
            }
        }
        public void LoadResources(/*GraphicsDevice device,ContentManager content,*/DateTime start)
        {
            //boat = content.Load<Model>(ContentHelper.ContentPath + "e-scow_directxformat");
            //sail = new Sail(Sail.SailType.Main,10, 4, 2, 2);
            //jib = new Sail(Sail.SailType.Jib, 8, 3, 2,1);
            //sailEffect = new BasicEffect(device, null);
            //sailEffect.EnableDefaultLighting();
            //System.Drawing.Color c=System.Drawing.Color.FromArgb(_boatData.Color);
            //_color = new Vector3((float)c.R / 255.0f, (float)c.G / 255.0f, (float)c.B / 255.0f);
            //sailEffect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
            //sailEffect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
            //sailEffect.AmbientLightColor = _color;
            //sailEffect.VertexColorEnabled = true;
            CurrentMarkIndex.AddValue(start, 0);
            //hudEffect = new BasicEffect(device, null);
            //hudEffect.VertexColorEnabled = true;
            //BuildPathCurve();
        }
        //public void BuildPathCurve()
        //{
        //    _boatDataRowsCurveMap = new SortedList<int, int>();
        //    List<Vector3> controlPoints = new List<Vector3>();
        //    for (int i = 0; i < _boatDataRows.Count; i++)
        //    {
        //        CoordinatePoint cp = new CoordinatePoint(new Coordinate(_boatDataRows[i].latitude), new Coordinate(_boatDataRows[i].longitude), 0);
        //        controlPoints.Add(cp.Project().ToWorld());
        //    }
        //    List<Vector3> curvePoints = BezierHelper.CreateSmoothedLine(controlPoints, out _boatDataRowsCurveMap, out _pathControlPoints, out _boatDataRowsDistances);
        //    _pathCurve = new VertexPositionColor[(curvePoints.Count*2)-2];

        //    float maxWidth = 0.2f;//meters/coord divisor
        //    float minWidth = 0.01f;//meters/coord divisor
        //    float maxSpeed = 20f;//kmh
        //    //float minSpeed = 0f;//kmh

        //    int vertexIndex = 0;

        //    for (int i = 0; i < _boatDataRows.Count-2; i++)
        //    {
        //        int curveStart = _boatDataRowsCurveMap[i];
        //        int curveEnd = _boatDataRowsCurveMap[i + 1];

        //        float startDistance = _boatDataRowsDistances[i];
        //        float endDistance = _boatDataRowsDistances[i + 1];

        //        float startSpeed = (startDistance / 1000) / (float)(_boatDataRows[i + 1].datetime - _boatDataRows[i].datetime).TotalHours;
        //        float endSpeed = (endDistance / 1000) / (float)(_boatDataRows[i + 2].datetime - _boatDataRows[i + 1].datetime).TotalHours;

        //        float speedDelta = endSpeed - startSpeed;
        //        int curvePointCount = curveEnd - curveStart;

        //        for (int c = curveStart; c < curveEnd; c++)
        //        {
        //            float percentThrough = ((float)c - (float)curveStart) / (float)curvePointCount;
        //            float speed = startSpeed + (percentThrough * speedDelta);

        //            float width = minWidth + ((speed / maxSpeed) * (maxWidth - minWidth));

        //            width = maxWidth - width;

        //            float angleToNext = -(float)Math.Atan2(curvePoints[c + 1].Z - curvePoints[c].Z, curvePoints[c + 1].X - curvePoints[c].X);
        //            float angleRight = angleToNext + MathHelper.PiOver2;
        //            float angleLeft = angleToNext - MathHelper.PiOver2;

        //            float rightX = curvePoints[c].X + (float)Math.Cos(angleRight) * (width / 2f);
        //            float rightZ = curvePoints[c].Z - (float)Math.Sin(angleRight) * (width / 2f);
        //            VertexPositionColor right = new VertexPositionColor();
        //            right.Position = new Vector3(rightX, 0, rightZ);
        //            _pathCurve[vertexIndex] = right;
        //            vertexIndex++;

        //            float leftX = curvePoints[c].X + (float)Math.Cos(angleLeft) * (width / 2f);
        //            float leftZ = curvePoints[c].Z - (float)Math.Sin(angleLeft) * (width / 2f);
        //            VertexPositionColor left = new VertexPositionColor();
        //            left.Position = new Vector3(leftX, 0, leftZ);
        //            _pathCurve[vertexIndex] = left;
        //            vertexIndex++;
        //        }
        //    }
        //}
        //public void Draw(GraphicsDevice device, CameraMan cameraMan, DateTime time)
        //{
        //    if (ProjectedPoint != null)
        //    {
        //        Camera camera = cameraMan.Camera;
        //        float angle = arrowAngle;
        //        if (direction == BoatDirection.Backwards)
        //        {
        //            angle = AngleHelper.NormalizeAngle(arrowAngle + MathHelper.Pi);
        //        }

        //        foreach (ModelMesh mesh in boat.Meshes)
        //        {
        //            foreach (BasicEffect mfx in mesh.Effects)
        //            {
        //                mfx.EnableDefaultLighting();
        //                mfx.AmbientLightColor = _color;
        //                mfx.World = Matrix.CreateScale(0.02f) * Matrix.CreateTranslation(new Vector3(0f, 0f, -0.25f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(currentHeel)) * Matrix.CreateRotationY(MathHelper.ToRadians(90.0f)) * Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(ProjectedPoint.ToWorld());
        //                camera.ConfigureBasicEffect(mfx);
        //            }
        //            mesh.Draw();
        //        }

        //        camera.ConfigureBasicEffect(sailEffect);
        //        sailEffect.World = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(currentBoomAngle) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0.25f)) * Matrix.CreateTranslation(new Vector3(0, 0, -0.36f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-currentHeel)) * Matrix.CreateRotationY(MathHelper.ToRadians(270.0f) + angle) * Matrix.CreateTranslation(ProjectedPoint.ToWorld());
        //        sailEffect.Begin();
        //        foreach (EffectPass pass in sailEffect.CurrentTechnique.Passes)
        //        {
        //            pass.Begin();
        //            sail.Draw(device, currentSailCurve, 0);
        //            pass.End();
        //        }
        //        sailEffect.End();

        //        camera.ConfigureBasicEffect(sailEffect);
        //        sailEffect.World = Matrix.CreateScale(0.1f) /** Matrix.CreateRotationY(currentBoomAngle)*/ * Matrix.CreateTranslation(new Vector3(0f, 0f, 0.25f)) * Matrix.CreateTranslation(new Vector3(0, 0, -0.36f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-currentHeel)) * Matrix.CreateRotationY(MathHelper.ToRadians(270.0f) + angle) * Matrix.CreateTranslation(ProjectedPoint.ToWorld());
        //        sailEffect.Begin();
        //        foreach (EffectPass pass in sailEffect.CurrentTechnique.Passes)
        //        {
        //            pass.Begin();
        //            jib.Draw(device, currentSailCurve, (currentBoomAngle * 80f) * 0.02f);
        //            pass.End();
        //        }
        //        sailEffect.End();
        //    }
        //}
        //public void DrawHUD(GraphicsDevice device, CameraMan cameraMan, DateTime time, float coordinateDivisor)
        //{
        //    device.RenderState.DepthBufferEnable = false;
        //    Camera camera = cameraMan.Camera;
        //    float angle = arrowAngle;
        //    if (direction == BoatDirection.Backwards)
        //    {
        //        angle = AngleHelper.NormalizeAngle(arrowAngle + MathHelper.Pi);
        //    }



        //    camera.ConfigureBasicEffect(hudEffect);
        //    hudEffect.World = Matrix.Identity;
        //    hudEffect.Begin();
            
        //    foreach (EffectPass pass in hudEffect.CurrentTechnique.Passes)
        //    {
        //        pass.Begin();
        //        if (cameraMan.DrawPastPath || cameraMan.DrawFuturePath)
        //        {
        //            DateTime startTime = _boatDataRows[_currentBoatDataRow].datetime;
        //            DateTime endTime = _boatDataRows[_currentBoatDataRow].datetime;
        //            int startIndex = _currentBoatDataRow;
        //            int endIndex = _currentBoatDataRow;
        //            if (cameraMan.DrawPastPath)
        //            {
        //                startTime = startTime - new TimeSpan(0, 0, cameraMan.DrawPathLength);
        //                while (startIndex > 0 && _boatDataRows[startIndex].datetime > startTime)
        //                {
        //                    startIndex--;
        //                }
        //            }
        //            if (cameraMan.DrawFuturePath)
        //            {
        //                endTime = endTime + new TimeSpan(0, 0, cameraMan.DrawPathLength);
        //                while (endIndex < _boatDataRows.Count && _boatDataRows[endIndex].datetime < endTime)
        //                {
        //                    endIndex++;
        //                }
        //            }

        //            if (_boatDataRowsCurveMap.ContainsKey(startIndex) && _boatDataRowsCurveMap.ContainsKey(endIndex))
        //            {

        //                int curveStart = _boatDataRowsCurveMap[startIndex] * 2;
        //                int curveEnd = _boatDataRowsCurveMap[endIndex] * 2;

        //                int curveLength = (curveEnd - curveStart) / 2;
        //                int curveMidPoint = _boatDataRowsCurveMap[_currentBoatDataRow] * 2;
        //                for (int i = curveStart; i < curveEnd; i++)
        //                {
        //                    float distance;
        //                    if (i < _currentBoatDataRow)
        //                    {
        //                        distance = (float)Math.Abs(i - curveMidPoint) / (float)Math.Abs(curveStart - curveMidPoint);
        //                    }
        //                    else if (i > _currentBoatDataRow)
        //                    {
        //                        distance = (float)Math.Abs(i - curveMidPoint) / (float)Math.Abs(curveEnd - curveMidPoint);
        //                    }
        //                    else
        //                    {
        //                        distance = 1f;
        //                    }

        //                    float alpha = 1 - distance;

        //                    Color c = new Color(new Vector4(_color, alpha));
        //                    _pathCurve[i].Color = c;
        //                }
        //                device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
        //                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, _pathCurve, curveStart, curveLength * 2);
        //            }
        //        }
        //        pass.End();
        //    }
            
        //    hudEffect.End();

        //    if (CurrentRacingStatus != RacingStatus.Finished && cameraMan.DrawAngleToMark)
        //    {
        //        //float angleToMark = AngleHelper.FindAngle(CurrentMarkLocation.ToWorld(), ProjectedPoint.ToWorld());
        //        DrawInstrument(device, InstrumentDrawing.OutwardArrow, Microsoft.Xna.Framework.Graphics.Color.Orange, ProjectedPoint.ToWorld(), AngleToMark, 0.5f, 0.5f);
        //    }

        //    if (cameraMan.DrawAngleToWind)
        //    {
        //        DrawInstrument(device, InstrumentDrawing.InwardArrow, Microsoft.Xna.Framework.Graphics.Color.Green, ProjectedPoint.ToWorld(), windAngle+MathHelper.Pi, 0.5f, 0.5f);
        //    }

        //    if (cameraMan.DrawAbsoluteAngleReference || cameraMan.DrawRelativeAngleReference)
        //    {
        //        int line = 0;
        //        for (float a = 0f; a < MathHelper.TwoPi; a = a + (MathHelper.PiOver4 / 2.0f))
        //        {
        //            float length = 0.1f;
        //            if (line == 0)
        //                length = 0.4f;
        //            else if (line % 4 == 0)
        //                length = 0.3f;
        //            else if (line % 2 == 0)
        //                length = 0.2f;

        //            if (cameraMan.DrawRelativeAngleReference)
        //            {
        //                InstrumentDrawing id = InstrumentDrawing.Line;
        //                if (line == 0)
        //                {
        //                    id = InstrumentDrawing.OutwardArrow;
        //                }
        //                DrawInstrument(device, id, Microsoft.Xna.Framework.Graphics.Color.Gray, ProjectedPoint.ToWorld(), a + this.Angle, 1.0f - length, length);
        //            }
        //            if (cameraMan.DrawAbsoluteAngleReference)
        //            {
        //                DrawInstrument(device, InstrumentDrawing.Line, Microsoft.Xna.Framework.Graphics.Color.LightGray, ProjectedPoint.ToWorld(), a, 1.0f, length);
        //            }
        //            line++;
        //        }
        //    }
        //    device.RenderState.DepthBufferEnable = true;
        //}
        //private void DrawInstrument(GraphicsDevice device,InstrumentDrawing drawingType, Color color, Vector3 location, float rotation, float startDistance, float length)
        //{
        //    float tipSize = 0.05f;
        //    hudEffect.World = Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(location);
        //    hudEffect.Begin();
        //    foreach (EffectPass pass in hudEffect.CurrentTechnique.Passes)
        //    {
        //        pass.Begin();

        //        VertexPositionColor[] vs;
        //        vs = new VertexPositionColor[6];
        //        vs[0].Position = new Vector3(startDistance, 0, 0);
        //        vs[0].Color = color;
        //        vs[1].Position = new Vector3(startDistance + length, 0, 0);
        //        vs[1].Color = color;

        //        if (drawingType == InstrumentDrawing.InwardArrow)
        //        {
        //            vs[2].Position = new Vector3(startDistance, 0, 0);
        //            vs[2].Color = color;
        //            vs[3].Position = new Vector3(startDistance + (tipSize * 2), 0, tipSize);
        //            vs[3].Color = color;

        //            vs[4].Position = new Vector3(startDistance, 0, 0);
        //            vs[4].Color = color;
        //            vs[5].Position = new Vector3(startDistance + (tipSize * 2), 0, -tipSize);
        //            vs[5].Color = color;
        //        }
        //        else if (drawingType == InstrumentDrawing.OutwardArrow)
        //        {
        //            vs[2].Position = new Vector3(startDistance + length, 0, 0);
        //            vs[2].Color = color;
        //            vs[3].Position = new Vector3(startDistance + length - (tipSize * 2), 0, tipSize);
        //            vs[3].Color = color;

        //            vs[4].Position = new Vector3(startDistance + length, 0, 0);
        //            vs[4].Color = color;
        //            vs[5].Position = new Vector3(startDistance + length - (tipSize * 2), 0, -tipSize);
        //            vs[5].Color = color;
        //        }

        //        device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
        //        device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vs, 0, 3);

        //        pass.End();
        //    }
        //    hudEffect.End();
        //}
        private TimeLineStatistic<int> CurrentMarkIndex
        {
            get
            {
                return _currentMarkIndex;
            }
        }
        private void PerformTack(DateTime time)
        {
            if (_currentTackIndex.GetValue(time) + 1 >= _tacks.Count)
            {
                int leg = _currentMarkIndex.GetValue(time);
                Tack t = new Tack();
                t.Direction = this.CurrentTackDirection;
                t.LegIndex = leg;
                t.Index = _tacks.Count;

                int legIndex = 0;
                for (int i = 0; i < _tacks.Count; i++)
                {
                    if (_tacks[i].LegIndex == leg)
                    {
                        legIndex++;
                    }
                }
                
                t.IndexOnLeg = legIndex;
                _tacks.Add(t);
                _currentTackIndex.AddValue(time, _tacks.Count-1);
                //UpdateStatistics(time);
            }
            else
            {
                _currentTackIndex.AddValue(time, _currentTackIndex.GetValue(time) + 1);
            }
        }
        public void Move(DateTime time,Course course,float windDirection,DateTime raceStart)
        {
            //cheat and jump to the time for the very first move.
            //kindof hacky, but it keeps the user from having to wait for the boats to get 
            //intialized when the race starts.  Also, we probably don't want stats for <race time
            if (!_positionInitialized)
            {
                while (_boatDataRows[_currentBoatDataRow].datetime < raceStart && _currentBoatDataRow < _boatDataRows.Count - 2)
                {
                    _currentBoatDataRow++;
                }
                //if (_currentBoatDataRow > 0)
                //{
                //    _currentBoatDataRow = _currentBoatDataRow - 1;
                //}
                _positionInitialized = true;
            }

            windAngle = windDirection;

            if (_lastUpdate == null)
            {
                _lastUpdate = time;
            }
            
            CoordinatePoint cp;
            ProjectedPoint current=null;

            CoordinatePoint np;
            ProjectedPoint next=null;

            long ticksBetween;
            long actualTicks;
            double percent=0.0;


            //bool changedIndex = false;
            bool doMove = false;
            if (_lastUpdate.Ticks < time.Ticks && _currentBoatDataRow < _boatDataRows.Count - 2)
            {
                doMove = true;
                direction = BoatDirection.Forward;
                while (_boatDataRows[_currentBoatDataRow].datetime < time && _currentBoatDataRow < _boatDataRows.Count - 2)
                {
                    _currentBoatDataRow++;
                    //changedIndex = true;

                    if ((_lowWaterMark == null || _highWaterMark == null) || (time < _lowWaterMark || time > _highWaterMark))
                    {

                        cp = new CoordinatePoint(new Coordinate(_boatDataRows[_currentBoatDataRow + 1].latitude), new Coordinate(_boatDataRows[_currentBoatDataRow + 1].longitude), 0);
                        current = cp.Project();

                        np = new CoordinatePoint(new Coordinate(_boatDataRows[_currentBoatDataRow].latitude), new Coordinate(_boatDataRows[_currentBoatDataRow].longitude), 0);
                        next = np.Project();

                        ProjectedPoint = current;

                        if (_currentMarkIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime) < course.Route.Count)
                        {
                            NavigateCourse(_boatDataRows[_currentBoatDataRow].datetime, course, cp, np);
                        }
                        double distance = CoordinatePoint.TwoDimensionalDistance(current.Easting, current.Northing, next.Easting, next.Northing);
                        TimeSpan ts = _boatDataRows[_currentBoatDataRow + 1].datetime - _boatDataRows[_currentBoatDataRow].datetime;
                        double kmh = ((distance / 1000.0) / ts.TotalHours);
                        speed = (float)kmh;
                        //float oldWindAngle = RelativeAngleToWind;
                        arrowAngle = (float)AngleHelper.FindAngle(current, next);
                        float newWindAngle = RelativeAngleToWind;
                        if (CurrentRacingStatus == RacingStatus.Racing)
                        {
                            float deadZone = MathHelper.PiOver4 / 2f;//use to be /4f;
                            Tack.TackDirection newTack = Tack.TackDirection.Undetermined;
                            if (newWindAngle < -deadZone && newWindAngle > -MathHelper.Pi + deadZone)
                            {
                                newTack = Tack.TackDirection.Starboard;
                            }
                            else if (newWindAngle > deadZone && newWindAngle < MathHelper.Pi - deadZone)
                            {
                                newTack = Tack.TackDirection.Port;
                            }

                            if (newTack != Tack.TackDirection.Undetermined && (_tacks.Count - 1 < _currentTackIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime) || _tacks[_currentTackIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime)].Direction != newTack))
                            {
                                PerformTack(_boatDataRows[_currentBoatDataRow].datetime);
                            }
                        }
                        //set the current and previous marks
                        if (_currentMarkIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime) < course.Route.Count)
                        {
                            _currentMark = course.Route[_currentMarkIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime)];
                        }
                        else
                        {
                            _currentMark = null;
                        }
                        if (_currentMarkIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime) > 0 && course.Route.Count > 0)
                        {
                            _previousMark = course.Route[_currentMarkIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime) - 1];
                        }
                        else
                        {
                            _previousMark = null;
                        }
                        UpdateStatistics(_boatDataRows[_currentBoatDataRow].datetime, distance / 1000.0);
                        Thread.Sleep(0);
                    }
                }
            }
            else if (_lastUpdate.Ticks > time.Ticks && _currentBoatDataRow > 0)
            {
                doMove = true;
                direction = BoatDirection.Backwards;
                while (_boatDataRows[_currentBoatDataRow].datetime > time && _currentBoatDataRow > 1)
                {
                    _currentBoatDataRow--;
                    //changedIndex = true;

                    if ((_lowWaterMark == null || _highWaterMark == null) || (time < _lowWaterMark || time > _highWaterMark))
                    {

                        cp = new CoordinatePoint(new Coordinate(_boatDataRows[_currentBoatDataRow - 1].latitude), new Coordinate(_boatDataRows[_currentBoatDataRow - 1].longitude), 0);
                        current = cp.Project();

                        np = new CoordinatePoint(new Coordinate(_boatDataRows[_currentBoatDataRow].latitude), new Coordinate(_boatDataRows[_currentBoatDataRow].longitude), 0);
                        next = np.Project();

                        //ProjectedPoint = cp.Project();

                        //double distance = CoordinatePoint.TwoDimensionalDistance(current.easting, current.northing, next.easting, next.northing);
                        //TimeSpan ts = _boatDataRows[_currentBoatDataRow].datetime - _boatDataRows[_currentBoatDataRow - 1].datetime;
                        //double kmh = ((distance / 1000.0) / ts.TotalHours);
                        //speed = (float)kmh;
                        arrowAngle = (float)AngleHelper.FindAngle(current, next);
                        //UpdateStatistics(_boatDataRows[_currentBoatDataRow].datetime, distance / 1000.0);
                        Thread.Sleep(0);
                    }
                }
            }

            //set the current and previous marks
            if (_currentMarkIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime) < course.Route.Count)
            {
                _currentMark = course.Route[_currentMarkIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime)];
            }
            else
            {
                _currentMark = null;
            }
            if (_currentMarkIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime) > 0 && course.Route.Count > 0)
            {
                _previousMark = course.Route[_currentMarkIndex.GetValue(_boatDataRows[_currentBoatDataRow].datetime) - 1];
            }
            else
            {
                _previousMark = null;
            }
            
            if (doMove && _currentBoatDataRow > 0 && _currentBoatDataRow < _boatDataRows.Count - 2)
            {
                int currentOffset;
                if (direction == BoatDirection.Forward)
                {
                    currentOffset = -1;
                }
                else
                {
                    currentOffset = 1;
                }

                cp = new CoordinatePoint(new Coordinate(_boatDataRows[_currentBoatDataRow + currentOffset].latitude), new Coordinate(_boatDataRows[_currentBoatDataRow + currentOffset].longitude), 0);
                current = cp.Project();

                np = new CoordinatePoint(new Coordinate(_boatDataRows[_currentBoatDataRow].latitude), new Coordinate(_boatDataRows[_currentBoatDataRow].longitude), 0);
                next = np.Project();

                CoordinatePoint pp = new CoordinatePoint(new Coordinate(_boatDataRows[_currentBoatDataRow - currentOffset].latitude), new Coordinate(_boatDataRows[_currentBoatDataRow - currentOffset].longitude), 0);
                ProjectedPoint previous = pp.Project();

                ticksBetween = _boatDataRows[_currentBoatDataRow].datetime.Ticks - _boatDataRows[_currentBoatDataRow + currentOffset].datetime.Ticks;
                actualTicks = time.Ticks - _boatDataRows[_currentBoatDataRow + currentOffset].datetime.Ticks;
                percent = (double)actualTicks / (double)ticksBetween;

                double northingDiff = next.Northing - current.Northing;
                double eastingDiff = next.Easting - current.Easting;

                double northing = current.Northing + (northingDiff * percent);
                double easting = current.Easting + (eastingDiff * percent);

                ProjectedPoint actual = new ProjectedPoint();
                actual.Easting = easting;
                actual.Northing = northing;

                ProjectedPoint = actual;

                double distance = CoordinatePoint.TwoDimensionalDistance(current.Easting, current.Northing, next.Easting, next.Northing);
                TimeSpan ts = new TimeSpan(ticksBetween);
                double kmh = ((distance / 1000.0) / ts.TotalHours);
                speed = (float)kmh;

                if (Math.Abs(kmh) > 2)
                {
                    float startingAngle = (float)AngleHelper.FindAngle(previous, current);
                    float desiredAngle = (float)AngleHelper.FindAngle(next, current);
                    desiredAngle = AngleHelper.NormalizeAngle(desiredAngle);
                    float deltaAngle = AngleHelper.AngleDifference(startingAngle, desiredAngle);
                    arrowAngle = startingAngle + (deltaAngle * (float)percent);
                    arrowAngle = AngleHelper.NormalizeAngle(arrowAngle);
                    
                }

                SetHeel(time,kmh);
                SetSailCurve(time);
                SetBoomAngle(time);
                
                //if (changedIndex)
                //{
                //    _updateStatistics();
                //}
                _lastUpdate = time;
            }
            else if(_currentBoatDataRow==0||_currentBoatDataRow==_boatDataRows.Count-1)
            {
                ProjectedPoint = new CoordinatePoint(new Coordinate(_boatDataRows[_currentBoatDataRow].latitude), new Coordinate(_boatDataRows[_currentBoatDataRow].longitude), 0).Project();
            }

            if (_lowWaterMark==null||time < _lowWaterMark)
            {
                _lowWaterMark = time;
            }
            if (_highWaterMark == null || time > _highWaterMark)
            {
                _highWaterMark = time;
            }

        }
        private void NavigateCourse(DateTime time, Course course,CoordinatePoint a,CoordinatePoint b)
        {
            //if (course.Route[CurrentMarkIndex.GetValue(time)].DistanceTo(location.Project()) < _markRoundDistance)
            //{
            //    CurrentMarkIndex.AddValue(time, CurrentMarkIndex.GetValue(time) + 1);
            //    #warning possible bad increment of tack count
            //    //not sure this should increment tack count, technically rounding a mark is not a tack.
            //    if (CurrentMarkIndex.GetValue(time) < course.Route.Count)
            //    {
            //        PerformTack(time);
            //    }
            //}

            Mark previous = null;
            Mark next = null;
            if (CurrentMarkIndex.GetValue(time) > 0)
            {
                previous = course.Route[CurrentMarkIndex.GetValue(time) - 1];
            }
            if (CurrentMarkIndex.GetValue(time) < course.Route.Count - 1)
            {
                next = course.Route[CurrentMarkIndex.GetValue(time) + 1];
            }

            if (course.Route[CurrentMarkIndex.GetValue(time)].IsRounding(a.Project(),b.Project(),previous,next))
            {
                CurrentMarkIndex.AddValue(time, CurrentMarkIndex.GetValue(time) + 1);
                if (CurrentMarkIndex.GetValue(time) < course.Route.Count)
                {
                    PerformTack(time);
                }
            }
        }
        public float WaterLevel
        {
            get
            {
                return arrowY;
            }
            set
            {
                arrowY = value+waterLine;
            }
        }
        public CoordinatePoint Location
        {
            get
            {
                return new CoordinatePoint(new Coordinate(_boatDataRows[_currentBoatDataRow].latitude), new Coordinate(_boatDataRows[_currentBoatDataRow].longitude), 0);
            }
        }
        private void SetHeel(DateTime now,double kmh)
        {
            float heelRate = 1f;

            float maxWind = 30f;
            float maxHeel = 45f;

            float currentWind = 15f;
            float desiredHeel;

            float angle = arrowAngle;
            if (direction == BoatDirection.Backwards)
            {
                angle = AngleHelper.NormalizeAngle(arrowAngle + MathHelper.Pi);
            }

            if (currentWind >= maxWind)
            {
                desiredHeel = maxHeel;
            }
            else
            {
                desiredHeel = (currentWind / maxWind) * maxHeel;
            }

            //make it positive or negative based on wind and direction
            if (CurrentRacingStatus == RacingStatus.Racing)
            {
                if (_tacks[_currentTackIndex.GetValue(now)].Direction == Tack.TackDirection.Port)
                {
                }
                else
                {
                    desiredHeel = -desiredHeel;
                }
            }
            else
            {
                float relativeWind = AngleHelper.NormalizeAngle((windAngle + MathHelper.Pi) - angle);
                if (relativeWind > MathHelper.Pi)
                {
                    desiredHeel = -desiredHeel;
                }
            }

            //if (kmh < 2)
            //{
            //    if (desiredHeel < 0)
            //    {
            //        desiredHeel = -80;
            //    }
            //    else
            //    {
            //        desiredHeel = 80;
            //    }
            //}

            if (desiredHeel < currentHeel)
            {
                //subtract
                if (MathHelper.Distance(desiredHeel, currentHeel) > heelRate)
                {
                    currentHeel = currentHeel - (MathHelper.Distance(desiredHeel, currentHeel) / 8);
                }
                else
                {
                    currentHeel = desiredHeel;
                }
            }
            else
            {
                if (MathHelper.Distance(desiredHeel, currentHeel) > heelRate)
                {
                    currentHeel = currentHeel + (MathHelper.Distance(desiredHeel, currentHeel) / 8);
                }
                else
                {
                    currentHeel = desiredHeel;
                }
            }
        }
        private void SetSailCurve(DateTime now)
        {
            float curveRate = 0.25f;

            float maxWind = 30f;
            float maxCurve = 4f;

            float currentWind = 15f;
            float desiredCurve;

            if (currentWind >= maxWind)
            {
                desiredCurve = maxCurve;
            }
            else
            {
                desiredCurve = (currentWind / maxWind) * maxCurve;
            }

            float angle = arrowAngle;
            if (direction == BoatDirection.Backwards)
            {
                angle = AngleHelper.NormalizeAngle(arrowAngle + MathHelper.Pi);
            }

            //make it positive or negative based on wind and direction
            if (CurrentRacingStatus == RacingStatus.Racing)
            {
                if (_tacks[_currentTackIndex.GetValue(now)].Direction == Tack.TackDirection.Port)
                {
                }
                else
                {
                    desiredCurve = -desiredCurve;
                }
            }
            else
            {
                float relativeWind = AngleHelper.NormalizeAngle((windAngle + MathHelper.Pi) - angle);
                if (relativeWind > MathHelper.Pi)
                {
                    desiredCurve = -desiredCurve;
                }
            }



            float distance = MathHelper.Distance(desiredCurve, currentSailCurve);
            if (desiredCurve < currentSailCurve)
            {
                //subtract
                if (distance > curveRate)
                {
                    currentSailCurve = currentSailCurve - (distance / 8);
                }
                else
                {
                    currentSailCurve = desiredCurve;
                }
            }
            else
            {
                if (distance > curveRate)
                {
                    currentSailCurve = currentSailCurve + (distance / 8);
                }
                else
                {
                    currentSailCurve = desiredCurve;
                }
            }
        }
        private void SetBoomAngle(DateTime now)
        {
            //this is kindof a mess
            float boomRate = MathHelper.Pi / 10f;

            float maxBoomAngle = MathHelper.ToRadians(80f);
            float desiredAngle;

            float relativeWindAngle = Math.Abs(RelativeAngleToWind);
            if (relativeWindAngle <= MathHelper.ToRadians(50f))
            {
                desiredAngle = 0;
            }
            else
            {
                float ratio = (relativeWindAngle-MathHelper.ToRadians(50f)) / (MathHelper.Pi-MathHelper.ToRadians(50f));
                desiredAngle = maxBoomAngle * ratio;
            }

            if (CurrentRacingStatus == RacingStatus.Racing)
            {
                if (_tacks[_currentTackIndex.GetValue(now)].Direction == Tack.TackDirection.Starboard)
                {
                    desiredAngle = -desiredAngle;
                }
            }
            else
            {
                if (CurrentTackDirection == Tack.TackDirection.Starboard)
                {
                    desiredAngle = -desiredAngle;
                }
            }

            //desiredAngle = AngleHelper.NormalizeAngle(desiredAngle);
            //desiredAngle = desiredAngle + MathHelper.Pi;


            float distance = MathHelper.Distance(desiredAngle, currentBoomAngle);
            if (desiredAngle < currentBoomAngle)
            {
                //subtract
                if (distance > boomRate)
                {
                    currentBoomAngle = currentBoomAngle - boomRate;// (MathHelper.Distance(desiredAngle, currentBoomAngle) / 10);
                }
                else
                {
                    currentBoomAngle = desiredAngle;
                }
            }
            else if (desiredAngle > currentBoomAngle)
            {
                if (distance > boomRate)
                {
                    currentBoomAngle = currentBoomAngle + boomRate;// (MathHelper.Distance(desiredAngle, currentBoomAngle) / 10);
                }
                else
                {
                    currentBoomAngle = desiredAngle;
                }
            }


        }
        public ProjectedPoint ProjectedPoint
        {
            get
            {
                return _projectedPoint;
            }
            set
            {
                _projectedPoint = value;
            }
        }
        public float BoomAngle
        {
            get
            {
                return currentBoomAngle;
            }
        }
        public float Heel
        {
            get
            {
                return currentHeel;
            }
        }
        public float SailCurve
        {
            get
            {
                return currentSailCurve;
            }
        }
        public static ReplayBoat FindById(int id)
        {
            return new ReplayBoat(FindRowById(id));
        }
    }
    public class Tack
    {
        [DoNotObfuscate()]
        public enum TackDirection { Undetermined, Port, Starboard };
        public TackDirection Direction;
        public int Index;
        public int IndexOnLeg;
        public int LegIndex;
    }
}
