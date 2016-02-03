
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Threading;
using System.Drawing;

using Microsoft.Xna.Framework;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Data.Statistics;

namespace AmphibianSoftware.VisualSail.UI
{
    public class GdiRenderer : Renderer
    {
        //private fields
        private bool _lakeTextureAvailible = false;
        //private Notify _updateStatistics;
        private DateTime _previousRenderTime = DateTime.MinValue;
        private Bitmap _lakeTexture;

        private Dictionary<IViewPort, GdiCameraMan> _viewports;
        private Dictionary<IViewPort, Bitmap> _buffers;
        private Dictionary<IViewPort, Graphics> _graphics;
        private Dictionary<IViewPort, Graphics> _targetGraphics;

        private Dictionary<ReplayBoat, SortedList<DateTime, ProjectedPoint>> _boatTracks;
        
        //used to convert projected points into 3d space to eliminate loss of precision
        //private double _coordinateDivisor = 10f;//factor by which projected coordinates are divided to make minimize loss or precision with floats in xna
        private double _eastingOffset;//subtracted from the x value of projected coordinates to minimize loss of precision with floats
        private double _northingOffset;//subtracted from the z value of projected coordinates to minimize loss of precision with floats

        
        private CoordinatePoint _placeHolderCP; //does nothing other than keep a CP in memory so that the static fields don't clear
        //private ProjectedPoint _placeHolder; //does nothing other than keep a pp in memory so that the static fields don't clear

        public GdiRenderer()
        {
        }

        public override void Initialize(Replay replay)
        {
            base.Initialize(replay);
            SetOffsets();
            _viewports = new Dictionary<IViewPort, GdiCameraMan>();
            _buffers = new Dictionary<IViewPort, Bitmap>();
            _graphics = new Dictionary<IViewPort, Graphics>();
            _targetGraphics = new Dictionary<IViewPort, Graphics>();
            LoadBoatTracks();
            Resize();
        }
        private void LoadBoatTracks()
        {
            _boatTracks = new Dictionary<ReplayBoat, SortedList<DateTime, ProjectedPoint>>();

            foreach (ReplayBoat boat in Replay.Boats)
            {
                var points = from r in boat.SensorReadings
                             orderby r.datetime ascending
                             select r;
                SortedList<DateTime, ProjectedPoint> linePoints = new SortedList<DateTime, ProjectedPoint>();
                foreach (var point in points)
                {
                    CoordinatePoint cp = new CoordinatePoint(new Coordinate(point.latitude), new Coordinate(point.longitude), 0);
                    ProjectedPoint pp = cp.Project();
                    if (!linePoints.ContainsKey(point.datetime))
                    {
                        linePoints.Add(point.datetime, pp);
                    }
                }
                _boatTracks.Add(boat, linePoints);
            }
        }
        public override void Reset()
        {
            LoadContent();
            //SetUpGridLines();
            //SetupSkyBox();
        }
        public override void Resize()
        {
        }
        private void LoadContent()
        {
            if (!File.Exists(ContentHelper.DynamicContentPath + SatelliteImageryHelper.GetFileName(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West)))
            {
                try
                {
                    string lakeFile = SatelliteImageryHelper.GetSatelliteImage(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West, (int)this.Race.Lake.WidthInMeters / 10, (int)this.Race.Lake.HeightInMeters / 10);
                    //FileInfo fi = new FileInfo(lakeFile);
                    //fi.MoveTo(ContentHelper.ContentPath + SatelliteImageryHelper.GetFileName(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West));
                }
                catch //(Exception e)
                {
                }
            }
            try
            {
                _lakeTexture = (Bitmap)Bitmap.FromFile(ContentHelper.DynamicContentPath + SatelliteImageryHelper.GetFileName(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West));
                _lakeTextureAvailible = true;
            }
            catch (Exception /*e*/)
            {
                _lakeTextureAvailible = false;
            }

        }
        //private void SetUpGridLines()
        //{
        //    float maxX = (float)this.Race.Course.Lake.TopLeftInMeters.ToWorld().X;
        //    float minX = (float)this.Race.Course.Lake.BottomRightInMeters.ToWorld().X;
        //    float maxZ = (float)this.Race.Course.Lake.BottomRightInMeters.ToWorld().Z;
        //    float minZ = (float)this.Race.Course.Lake.TopLeftInMeters.ToWorld().Z;

        //    float horizontalCount = (Math.Abs((maxX - minX)) * _coordinateDivisor);
        //    float verticalCount = (Math.Abs((maxZ - minZ)) * _coordinateDivisor);

        //    _gridLines = new VertexPositionColor[(int)((horizontalCount * 2.0) + (verticalCount * 2.0))];

        //    int index = 0;
        //    float y = -0.01f;

        //    //horizontal lines
        //    for (float c = minX; c < maxX; c = c + (1f /*/ coordinateDivisor*/))
        //    {
        //        _gridLines[index].Position = new Vector3(c, y, minZ);
        //        _gridLines[index].Color = Color.Blue;
        //        _gridLines[index + 1].Position = new Vector3(c, y, maxZ);
        //        _gridLines[index + 1].Color = Color.Blue;
        //        index = index + 2;
        //    }

        //    //horizontal lines
        //    for (float c = minZ; c < maxZ && index < _gridLines.Length - 1; c = c + (1f /*/ coordinateDivisor*/))
        //    {
        //        _gridLines[index].Position = new Vector3(minX, y, c);
        //        _gridLines[index].Color = Color.Blue;
        //        _gridLines[index + 1].Position = new Vector3(maxX, y, c);
        //        _gridLines[index + 1].Color = Color.Blue;
        //        index = index + 2;
        //    }
        //}
        //private void SetupSkyBox()
        //{
            

        //    float padding = 2f;
        //    float minX;

        //    if (this.Race.Lake.BottomLeftInMeters.ToWorld().X > this.Race.Lake.BottomRightInMeters.ToWorld().X)
        //    {
        //        minX = this.Race.Lake.BottomLeftInMeters.ToWorld().X;
        //    }
        //    else
        //    {
        //        minX = this.Race.Lake.BottomRightInMeters.ToWorld().X;
        //    }

        //    float minZ;
        //    if (this.Race.Lake.BottomLeftInMeters.ToWorld().Z > this.Race.Lake.TopLeftInMeters.ToWorld().Z)
        //    {
        //        minZ = this.Race.Lake.BottomLeftInMeters.ToWorld().Z;
        //    }
        //    else
        //    {
        //        minZ = this.Race.Lake.TopLeftInMeters.ToWorld().Z;
        //    }
        //    minX = minX + padding;
        //    minZ = minZ + padding;


        //    float maxX;
        //    if (this.Race.Lake.TopLeftInMeters.ToWorld().X > this.Race.Lake.TopRightInMeters.ToWorld().X)
        //    {
        //        maxX = this.Race.Lake.TopRightInMeters.ToWorld().X;
        //    }
        //    else
        //    {
        //        maxX = this.Race.Lake.TopLeftInMeters.ToWorld().X;
        //    }

        //    float maxZ;
        //    if (this.Race.Lake.TopRightInMeters.ToWorld().Z > this.Race.Lake.BottomRightInMeters.ToWorld().Z)
        //    {
        //        maxZ = this.Race.Lake.BottomRightInMeters.ToWorld().Z;
        //    }
        //    else
        //    {
        //        maxZ = this.Race.Lake.TopRightInMeters.ToWorld().Z;
        //    }
        //    maxX = maxX - padding;
        //    maxZ = maxZ - padding;

        //    Vector3 min = new Vector3(minX, 0, minZ);
        //    Vector3 max = new Vector3(maxX, 0, maxZ);
        //    _worldBounds = new BoundingBox(min, max);
        //}
        public override void Shutdown()
        {
            if (_lakeTexture != null)
            {
                _lakeTexture.Dispose();
            }
        }
        private void RefreshViewports()
        {
            foreach (IViewPort vp in _viewports.Keys)
            {
                /*Dictionary<ReplayBoat, int> offsets = */
                vp.SetBoatList(this.Replay.Boats);
                //_viewportOffsets[vp] = offsets;
            }
        }
        public override void AddViewPort(IViewPort viewport)
        {
            GdiCameraMan man = new GdiCameraMan();
            man.SelectedBoat = 0;
            man.DrawSatelliteImagery = _lakeTextureAvailible;
            viewport.CameraMan = man;
            
            
            viewport.SetBoatList(this.Replay.Boats);
            viewport.Shutdown = new ShutdownViewPort(this.RemoveViewPort);
            lock (_viewports)
            {
                _viewports.Add(viewport, man);
            }
            lock (_buffers)
            {
                int bufferWidth = viewport.RenderTarget.Width;
                int bufferHeight = viewport.RenderTarget.Height;
                _buffers.Add(viewport, new Bitmap(bufferWidth, bufferHeight));
            }
            lock (_graphics)
            {
                _graphics.Add(viewport,Graphics.FromImage(_buffers[viewport]));
            }
            lock (_targetGraphics)
            {
                if (viewport.RenderTarget.IsHandleCreated)
                {
                    _targetGraphics.Add(viewport, Graphics.FromHwnd(viewport.RenderTarget.Handle));
                }
            }
        }
        public override void RemoveViewPort(IViewPort viewport)
        {
            lock (_graphics)
            {
                _graphics.Remove(viewport);
            }
            lock (_buffers)
            {
                _buffers.Remove(viewport);
            }
            lock (_viewports)
            {
                _viewports.Remove(viewport);
            }

        }
        //public IBoundingBox DrawPhoto(Photo photo, Rectangle location, IViewPort vp)
        //{
        //}
        public override void RenderAll()
        {
            //fps limiter
            DateTime n = DateTime.Now;
            if (n - _previousRenderTime >= new TimeSpan(0, 0, 0, 0, 30))
            {
                lock (_viewports)
                {
                    //IViewPort recordingViewport = null;
                    //foreach (IViewPort vp in _viewports.Keys)
                    //{
                    //    if (vp.Record == RecorderState.Recording || vp.Record == RecorderState.Paused)
                    //    {
                    //        recordingViewport = vp;
                    //        break;
                    //    }
                    //}

                    //if we found somebody recording
                    //if (recordingViewport != null)
                    //{
                    //    foreach (IViewPort vp in _viewports.Keys)
                    //    {
                    //        if (vp != recordingViewport)
                    //        {
                    //            vp.Record = RecorderState.Disabled;
                    //        }
                    //    }
                    //    //make sure the recorder is ready to go
                    //    if (_xnaAviWriter == null || !_xnaAviWriter.Recording)
                    //    {
                    //        try
                    //        {
                    //            _xnaAviWriter = new XnaAviWriter(_device);
                    //            _xnaAviWriter.VideoInitialize(recordingViewport.RecordingPath, 25, recordingViewport.RecordingSize.Width, recordingViewport.RecordingSize.Height);
                    //            RenderVideoTitle();
                    //        }
                    //        catch (Exception message)
                    //        {
                    //            MessageBox.Show("A problem occured while starting the recording." + Environment.NewLine + message.Message);
                    //            recordingViewport.Record = RecorderState.Ready;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (_xnaAviWriter != null && _xnaAviWriter.Recording)
                    //    {
                    //        _xnaAviWriter.Close();
                    //        _xnaAviWriter = null;
                    //    }
                    //    foreach (IViewPort vp in _viewports.Keys)
                    //    {
                    //        vp.RecordingPath = null;
                    //        vp.Record = RecorderState.Ready;
                    //    }
                    //}

                    foreach (IViewPort vp in _viewports.Keys)
                    {
                        Render(vp);
                        //if (vp.ScreenshotPath != null && vp.RecordingSize.Width > 0 && vp.RecordingSize.Height > 0)
                        //{
                        //    try
                        //    {
                        //        if (_xnaAviWriter == null)
                        //        {
                        //            _xnaAviWriter = new XnaAviWriter(_device);
                        //        }
                        //        _xnaAviWriter.ScreenShot(vp.ScreenshotPath, vp.RecordingSize.Width, vp.RecordingSize.Height);
                        //        _xnaAviWriter = null;
                        //    }
                        //    catch
                        //    {
                        //        MessageBox.Show("The image could not be captured to the specified file");
                        //    }
                        //    vp.ScreenshotPath = null;
                        //}
                    }
                }
                _previousRenderTime = n;
            }

        }
        private void Render(IViewPort target)
        {
            //get our cameraman
            GdiCameraMan cameraMan = _viewports[target];

            //check the buffer size, make sure it matches the window
            //if not rebuild it
            if ((target.RenderTarget.Height > 0 && target.RenderTarget.Width>0) && (target.RenderTarget.Width != _buffers[target].Width || target.RenderTarget.Height != _buffers[target].Height))
            {
                lock (_buffers)
                {
                    _buffers[target].Dispose();
                    _buffers.Remove(target);
                    _buffers.Add(target, new Bitmap(target.RenderTarget.Width, target.RenderTarget.Height));
                }

                lock (_graphics)
                {
                    _graphics[target].Dispose();
                    _graphics.Remove(target);
                    _graphics.Add(target, Graphics.FromImage(_buffers[target]));
                }

                if (target.RenderTarget.IsHandleCreated)
                {
                    lock (_targetGraphics)
                    {
                        if (_targetGraphics[target] != null)
                        {
                            _targetGraphics[target].Dispose();
                            _targetGraphics.Remove(target);
                        }
                        _targetGraphics.Add(target, Graphics.FromHwnd(target.RenderTarget.Handle));
                    }
                }
            }


            _graphics[target].Transform = new System.Drawing.Drawing2D.Matrix();

            //clear the buffer to black
			_graphics[target].Clear(System.Drawing.Color.Black);
            //_graphics[target].FillRectangle(Brushes.Black, 0, 0, _buffers[target].Width, _buffers[target].Height);

            int xOffset = target.RenderTarget.Width / 2;
            int yOffset = target.RenderTarget.Height / 2;
            _graphics[target].TranslateTransform(xOffset, yOffset, System.Drawing.Drawing2D.MatrixOrder.Prepend);
            _graphics[target].RotateTransform(cameraMan.Rotation, System.Drawing.Drawing2D.MatrixOrder.Prepend);
            


            ReplayBoat boat=this.Replay.Boats[cameraMan.SelectedBoat];
            if (boat.ProjectedPoint != null)
            {
                cameraMan.X = 0;
                cameraMan.Y = 0;
                cameraMan.FollowBoat(ProjectedPointToWorld(boat.ProjectedPoint, cameraMan));
            }

            float lakeX = ProjectedPointToWorld(Replay.Race.Lake.TopLeftInMeters,cameraMan).X;
            float lakeY = ProjectedPointToWorld(Replay.Race.Lake.TopLeftInMeters,cameraMan).Y;
            float lakeWidth = ProjectedPointToWorld(Replay.Race.Lake.BottomRightInMeters,cameraMan).X - lakeX;
            float lakeHeight = ProjectedPointToWorld(Replay.Race.Lake.BottomRightInMeters,cameraMan).Y - lakeY;

            

            _graphics[target].DrawImage(_lakeTexture, lakeX, lakeY, lakeWidth, lakeHeight);

            DateTime lineStart = this.Replay.SimulationTime;
            DateTime lineEnd = this.Replay.SimulationTime;

            if (cameraMan.DrawFuturePath)
            {
                lineEnd = this.Replay.SimulationTime + new TimeSpan(0, 0, cameraMan.DrawPathLength);
            }
            if (cameraMan.DrawPastPath)
            {
                lineStart = this.Replay.SimulationTime - new TimeSpan(0, 0, cameraMan.DrawPathLength);
            }

            foreach (ReplayBoat b in this.Replay.Boats)
            {
                if (b.ProjectedPoint != null)
                {
                    _graphics[target].FillRectangle(new SolidBrush(b.Color), ProjectedPointToWorld(b.ProjectedPoint,cameraMan).X-2, ProjectedPointToWorld(b.ProjectedPoint,cameraMan).Y-2, 4, 4);

                    if (cameraMan.DrawFuturePath || cameraMan.DrawPastPath)
                    {
                        var points = from pp in _boatTracks[b]
                                     where pp.Key >= lineStart
                                     && pp.Key <= lineEnd
                                     select pp.Value;

                        var projectedPoints = from pp in points
                                              select ProjectedPointToWorld(pp, cameraMan);

                        var drawPoints = (from pp in projectedPoints
                                         select new System.Drawing.Point((int)pp.X, (int)pp.Y)).ToArray();

                        _graphics[target].DrawLines(new Pen(new SolidBrush(b.Color)), drawPoints);
                    }
                }
            }

            foreach (Mark mark in this.Replay.Race.Course.Marks)
            {
                foreach (Bouy bouy in mark.Bouys)
                {
                    _graphics[target].FillRectangle(Brushes.Orange, ProjectedPointToWorld(bouy.CoordinatePoint.Project(),cameraMan).X - 2, ProjectedPointToWorld(bouy.CoordinatePoint.Project(),cameraMan).Y - 2, 4, 4);
                }
            }

            _graphics[target].Flush();
            _targetGraphics[target].DrawImage(_buffers[target], 0, 0);
            _targetGraphics[target].Flush();
            
            //Graphics targetGraphics = _targetGraphics[target];
            //buffer.Save("C:\\test.bmp");
            //targetGraphics.DrawImage(buffer, 0, 0);

            //    if (cameraMan.DrawGrid)
            //    {
            //        _instruments.World = Matrix.Identity;
            //        _instruments.Begin();
            //        foreach (EffectPass pass in _instruments.CurrentTechnique.Passes)
            //        {
            //            pass.Begin();
            //            _device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
            //            _device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, _gridLines, 0, _gridLines.Length / 2);
            //            pass.End();
            //        }
            //        _instruments.End();
            //    }
            //    Dictionary<ReplayBoat, Vector2> locations = new Dictionary<ReplayBoat, Vector2>();
            //    foreach (ReplayBoat b in this.Replay.Boats)
            //    {
            //        DrawHUD(b, _device, cameraMan, this.Replay.SimulationTime, _coordinateDivisor);
            //        if (cameraMan.ShowAnyIdentifiers || target.ClickedPoint != null || cameraMan.PhotoMode != CameraMan.PhotoDisplayMode.Disabled)
            //        {
            //            if (b.ProjectedPoint != null)
            //            {
            //                Vector3 twoD = viewport.Project(b.ProjectedPoint.ToWorld(), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
            //                if (twoD.Z < 1f)
            //                {
            //                    locations.Add(b, new Vector2(twoD.X, twoD.Y));
            //                }
            //            }
            //        }
            //    }
            //    foreach (ReplayBoat b in this.Replay.Boats)
            //    {
            //        DrawBoat(b, _device, cameraMan, this.Replay.SimulationTime);
            //    }

            //    Dictionary<Bouy, Vector2> bouyLocations = new Dictionary<Bouy, Vector2>();
            //    foreach (Mark m in this.Replay.Race.Course.Marks)
            //    {
            //        foreach (Bouy b in m.Bouys)
            //        {
            //            DrawBouy(b, _device, camera);
            //            if (cameraMan.ShowMarkNames)
            //            {
            //                Vector3 twoD = viewport.Project(b.CoordinatePoint.Project().ToWorld(), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
            //                if (twoD.Z < 1f)
            //                {
            //                    bouyLocations.Add(b, new Vector2(twoD.X, twoD.Y));
            //                }
            //            }
            //        }
            //    }

            //    ////course navigation debug lines
            //    //List<VertexPositionColor> markRoundLines = new List<VertexPositionColor>();
            //    //for (int i = 0; i < this.Race.Course.Route.Count; i++)
            //    //{
            //    //    if (i > 0 && i < this.Race.Course.Route.Count - 1)
            //    //    {
            //    //        Vector3 a = this.Race.Course.Route[i].AveragedLocation.Project().ToWorld();
            //    //        Vector2 b2 = this.Race.Course.Route[i].FindMarkRoundPoint(this.Race.Course.Route[i - 1], this.Race.Course.Route[i + 1]);
            //    //        Vector3 b = a;
            //    //        b.X = b2.X;
            //    //        b.Z = b2.Y;

            //    //        VertexPositionColor vpcA = new VertexPositionColor(a, Color.Red);
            //    //        VertexPositionColor vpcB = new VertexPositionColor(b, Color.Red);
            //    //        markRoundLines.Add(vpcA);
            //    //        markRoundLines.Add(vpcB);
            //    //    }
            //    //}

            //    //instruments.World = Matrix.Identity;
            //    //instruments.Begin();
            //    //foreach (EffectPass pass in instruments.CurrentTechnique.Passes)
            //    //{
            //    //    pass.Begin();
            //    //    device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
            //    //    device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, markRoundLines.ToArray(), 0, markRoundLines.Count/2);
            //    //    pass.End();
            //    //}
            //    //instruments.End();

            //    DrawMouseInstructions(DateTime.Now - target.CreatedAt, viewport.Width, viewport.Height);

            //    if (cameraMan.ShowAnyIdentifiers || cameraMan.ShowMarkNames || cameraMan.DrawPlaybackSpeed || cameraMan.PhotoMode != CameraMan.PhotoDisplayMode.Disabled)
            //    {
            //        _batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            //        List<BoundingBox> labelBoxBounds = new List<BoundingBox>();

            //        //if (cameraMan.PhotoMode != CameraMan.PhotoDisplayMode.Disabled)
            //        //{
            //        //    BoundingBox box = DrawPhoto(locations, target, cameraMan);
            //        //    if (box != default(BoundingBox))
            //        //    {
            //        //        labelBoxBounds.Add(box);
            //        //    }
            //        //}

            //        if (cameraMan.DrawPlaybackSpeed)
            //        {

            //            string playString = "";
            //            if (Replay.Speed == 0 /*|| _play==false*/)
            //            {
            //                playString = "||";
            //            }
            //            else if (Replay.Speed == 1)
            //            {
            //                playString = ">";
            //            }
            //            else if (Replay.Speed > 1)
            //            {
            //                playString = ">> " + Replay.Speed + "x";
            //            }
            //            else if (Replay.Speed == -1)
            //            {
            //                playString = "<";
            //            }
            //            else if (Replay.Speed < -1)
            //            {
            //                playString = "<< " + Replay.Speed + "x";
            //            }
            //            else
            //            {
            //                playString = "?";
            //            }
            //            _batch.DrawString(_font, playString, new Vector2(50, 50), new Color(new Vector4(1, 1, 1, .75f)));
            //        }

            //        if (cameraMan.ShowAnyIdentifiers)
            //        {
            //            foreach (ReplayBoat b in locations.Keys)
            //            {
            //                Vector2 p = locations[b];
            //                if (IsBoatOnScreen(p, target))
            //                {
            //                    List<string> text = new List<string>();
            //                    string topRow = "";

            //                    if (cameraMan.ShowName)
            //                    {
            //                        topRow = topRow + b.Name;
            //                    }

            //                    if (cameraMan.ShowNumber)
            //                    {
            //                        topRow = topRow + " " + b.Number;
            //                    }

            //                    if (cameraMan.ShowPosition)
            //                    {
            //                        int pos = b.GetCurrentPosition(this.Replay.SimulationTime);
            //                        string v = VerbageHelper.PositionString(pos);
            //                        if (v.Length > 0)
            //                        {
            //                            topRow = topRow + " (" + v + ")";
            //                        }
            //                    }

            //                    if (topRow.Length > 0)
            //                    {
            //                        text.Add(topRow);
            //                    }

            //                    if (cameraMan.ShowSpeed)
            //                    {

            //                        text.Add(ExtractBoatStatisticString(b, target, "Speed", this.Replay.SimulationTime));
            //                    }
            //                    if (cameraMan.ShowVMGToCourse)
            //                    {
            //                        text.Add(ExtractBoatStatisticString(b, target, "VMG to Course", this.Replay.SimulationTime));
            //                    }
            //                    if (cameraMan.ShowDistanceToMark)
            //                    {
            //                        text.Add(ExtractBoatStatisticString(b, target, "Distance to Mark", this.Replay.SimulationTime));
            //                    }
            //                    if (cameraMan.ShowDistanceToCourse)
            //                    {
            //                        text.Add(ExtractBoatStatisticString(b, target, "Distance to Course", this.Replay.SimulationTime));
            //                    }
            //                    if (cameraMan.ShowAngleToMark)
            //                    {
            //                        text.Add(ExtractBoatStatisticString(b, target, "Angle to Mark", this.Replay.SimulationTime));
            //                    }
            //                    if (cameraMan.ShowAngleToWind)
            //                    {
            //                        text.Add(ExtractBoatStatisticString(b, target, "Angle to Wind", this.Replay.SimulationTime));
            //                    }
            //                    if (cameraMan.ShowAngleToCourse)
            //                    {
            //                        text.Add(ExtractBoatStatisticString(b, target, "Angle to Course", this.Replay.SimulationTime));
            //                    }

            //                    DrawLabelBox(p, _batch, new Color(b.Color.R, b.Color.G, b.Color.B, b.Color.A), text, ref labelBoxBounds, viewport.Width, viewport.Height);
            //                }
            //            }
            //        }

            //        if (cameraMan.ShowMarkNames)
            //        {
            //            foreach (Bouy b in bouyLocations.Keys)
            //            {
            //                Vector2 p = bouyLocations[b];
            //                if (p.X > 0 && p.X < viewport.Width && p.Y > 0 && p.Y < viewport.Height)
            //                {
            //                    List<string> text = new List<string>();
            //                    text.Add(b.Mark.Name);
            //                    DrawLabelBox(p, _batch, new Color(new Vector3(1, 0.5f, 0)), text, ref labelBoxBounds, viewport.Width, viewport.Height);
            //                }
            //            }
            //        }

            //        _batch.End();
            //    }

            if (target.ClickedPoint != null)
            {
                Vector3 clicked = new Vector3((float)target.ClickedPoint.Value.X-xOffset, (float)target.ClickedPoint.Value.Y-yOffset,0f);
                float minDistance = float.MaxValue;
                int selectedIndex = -1;
                for (int i = 0; i < Replay.Boats.Count; i++)
                {
                    Vector3 boatLocation = ProjectedPointToWorld(Replay.Boats[i].ProjectedPoint, cameraMan);
                    float distance = Vector3.Distance(clicked, boatLocation);
                    if (distance < minDistance)
                    {
                        selectedIndex = i;
                        minDistance = distance;
                    }
                }
                if (minDistance < 25f)
                {
                    target.SetSelectedBoatIndex(selectedIndex);
                }
                target.ClickedPoint = null;
            }

            //    try
            //    {
            //        //make sure we have a handle before we try and draw
            //        if (target.HasHandle)
            //        {
            //            lock (target.RenderTarget)
            //            {
            //                _device.Present(new Rectangle(0, 0, _device.Viewport.Width, _device.Viewport.Height), new Rectangle(0, 0, _device.Viewport.Width, _device.Viewport.Height), target.RenderTarget.Handle);

            //            }
            //        }
            //        else
            //        {
            //            throw new InvalidOperationException("No Handle");
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        //if the windows are being docked/undocked or similiar, the handle might get destroyed mid-draw
            //        //it's ok if this happens, we'll just draw again on the next frame.
            //        //this is hacky, but i can't find a better way to prevent or catch this issue
            //    }

            //    if (target.Record == RecorderState.Recording && _xnaAviWriter != null && _xnaAviWriter.Recording)
            //    {
            //        try
            //        {
            //            _xnaAviWriter.AddFrame();
            //        }
            //        catch
            //        {
            //            MessageBox.Show("The video codec or compression you selected is not compatible with VisualSail");
            //            target.Record = RecorderState.Ready;
            //        }
            //    }
            //}
        }
        //public void DrawHUD(ReplayBoat boat, GraphicsDevice device, CameraMan cameraMan, DateTime time, float coordinateDivisor)
        //{
            //device.RenderState.DepthBufferEnable = false;
            //Camera camera = cameraMan.Camera;
            //float angle = boat.Angle;
            //if (boat.Direction == ReplayBoat.BoatDirection.Backwards)
            //{
            //    angle = AngleHelper.NormalizeAngle(boat.Angle + MathHelper.Pi);
            //}



            //camera.ConfigureBasicEffect(_hudEffect);
            //_hudEffect.World = Matrix.Identity;
            //_hudEffect.Begin();

            //foreach (EffectPass pass in _hudEffect.CurrentTechnique.Passes)
            //{
            //    pass.Begin();
            //    if (cameraMan.DrawPastPath || cameraMan.DrawFuturePath)
            //    {
            //        DateTime startTime = boat.SensorReadings[boat.CurrentSensorReadingIndex].datetime;
            //        DateTime endTime = boat.SensorReadings[boat.CurrentSensorReadingIndex].datetime;
            //        int startIndex = boat.CurrentSensorReadingIndex;
            //        int endIndex = boat.CurrentSensorReadingIndex;
            //        if (cameraMan.DrawPastPath)
            //        {
            //            startTime = startTime - new TimeSpan(0, 0, cameraMan.DrawPathLength);
            //            while (startIndex > 0 && boat.SensorReadings[startIndex].datetime > startTime)
            //            {
            //                startIndex--;
            //            }
            //        }
            //        if (cameraMan.DrawFuturePath)
            //        {
            //            endTime = endTime + new TimeSpan(0, 0, cameraMan.DrawPathLength);
            //            while (endIndex < boat.SensorReadings.Count && boat.SensorReadings[endIndex].datetime < endTime)
            //            {
            //                endIndex++;
            //            }
            //        }

            //        if (_boatDataRowsCurveMap[boat].ContainsKey(startIndex) && _boatDataRowsCurveMap[boat].ContainsKey(endIndex))
            //        {

            //            int curveStart = _boatDataRowsCurveMap[boat][startIndex] * 2;
            //            int curveEnd = _boatDataRowsCurveMap[boat][endIndex] * 2;

            //            int curveLength = (curveEnd - curveStart) / 2;
            //            int curveMidPoint = _boatDataRowsCurveMap[boat][boat.CurrentSensorReadingIndex] * 2;
            //            for (int i = curveStart; i < curveEnd; i++)
            //            {
            //                float distance;
            //                if (i < boat.CurrentSensorReadingIndex)
            //                {
            //                    distance = (float)Math.Abs(i - curveMidPoint) / (float)Math.Abs(curveStart - curveMidPoint);
            //                }
            //                else if (i > boat.CurrentSensorReadingIndex)
            //                {
            //                    distance = (float)Math.Abs(i - curveMidPoint) / (float)Math.Abs(curveEnd - curveMidPoint);
            //                }
            //                else
            //                {
            //                    distance = 1f;
            //                }

            //                float alpha = 1 - distance;

            //                Color c = new Color(new Vector4(boat.Color.R, boat.Color.G, boat.Color.B, alpha));
            //                _boatPathCurve[boat][i].Color = c;
            //            }
            //            device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
            //            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, _boatPathCurve[boat], curveStart, curveLength * 2);
            //        }
            //    }
            //    pass.End();
            //}

            //_hudEffect.End();

            //if (boat.CurrentRacingStatus != ReplayBoat.RacingStatus.Finished && cameraMan.DrawAngleToMark)
            //{
            //    //float angleToMark = AngleHelper.FindAngle(CurrentMarkLocation.ToWorld(), ProjectedPoint.ToWorld());
            //    DrawInstrument(device, InstrumentDrawing.OutwardArrow, Microsoft.Xna.Framework.Graphics.Color.Orange, boat.ProjectedPoint.ToWorld(), boat.Angle, 0.5f, 0.5f);
            //}

            //if (cameraMan.DrawAngleToWind)
            //{
            //    DrawInstrument(device, InstrumentDrawing.InwardArrow, Microsoft.Xna.Framework.Graphics.Color.Green, boat.ProjectedPoint.ToWorld(), Replay.WindAngle + MathHelper.Pi, 0.5f, 0.5f);
            //}

            //if (cameraMan.DrawAbsoluteAngleReference || cameraMan.DrawRelativeAngleReference)
            //{
            //    int line = 0;
            //    for (float a = 0f; a < MathHelper.TwoPi; a = a + (MathHelper.PiOver4 / 2.0f))
            //    {
            //        float length = 0.1f;
            //        if (line == 0)
            //            length = 0.4f;
            //        else if (line % 4 == 0)
            //            length = 0.3f;
            //        else if (line % 2 == 0)
            //            length = 0.2f;

            //        if (cameraMan.DrawRelativeAngleReference)
            //        {
            //            InstrumentDrawing id = InstrumentDrawing.Line;
            //            if (line == 0)
            //            {
            //                id = InstrumentDrawing.OutwardArrow;
            //            }
            //            DrawInstrument(device, id, Microsoft.Xna.Framework.Graphics.Color.Gray, boat.ProjectedPoint.ToWorld(), a + boat.Angle, 1.0f - length, length);
            //        }
            //        if (cameraMan.DrawAbsoluteAngleReference)
            //        {
            //            DrawInstrument(device, InstrumentDrawing.Line, Microsoft.Xna.Framework.Graphics.Color.LightGray, boat.ProjectedPoint.ToWorld(), a, 1.0f, length);
            //        }
            //        line++;
            //    }
            //}
            //device.RenderState.DepthBufferEnable = true;
        //}
        //private void DrawInstrument(GraphicsDevice device, InstrumentDrawing drawingType, System.Drawing.Color color, Vector3 location, float rotation, float startDistance, float length)
        //{
        //    //float tipSize = 0.05f;
        //    //_hudEffect.World = Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(location);
        //    //_hudEffect.Begin();
        //    //foreach (EffectPass pass in _hudEffect.CurrentTechnique.Passes)
        //    //{
        //    //    pass.Begin();

        //    //    VertexPositionColor[] vs;
        //    //    vs = new VertexPositionColor[6];
        //    //    vs[0].Position = new Vector3(startDistance, 0, 0);
        //    //    vs[0].Color = color;
        //    //    vs[1].Position = new Vector3(startDistance + length, 0, 0);
        //    //    vs[1].Color = color;

        //    //    if (drawingType == InstrumentDrawing.InwardArrow)
        //    //    {
        //    //        vs[2].Position = new Vector3(startDistance, 0, 0);
        //    //        vs[2].Color = color;
        //    //        vs[3].Position = new Vector3(startDistance + (tipSize * 2), 0, tipSize);
        //    //        vs[3].Color = color;

        //    //        vs[4].Position = new Vector3(startDistance, 0, 0);
        //    //        vs[4].Color = color;
        //    //        vs[5].Position = new Vector3(startDistance + (tipSize * 2), 0, -tipSize);
        //    //        vs[5].Color = color;
        //    //    }
        //    //    else if (drawingType == InstrumentDrawing.OutwardArrow)
        //    //    {
        //    //        vs[2].Position = new Vector3(startDistance + length, 0, 0);
        //    //        vs[2].Color = color;
        //    //        vs[3].Position = new Vector3(startDistance + length - (tipSize * 2), 0, tipSize);
        //    //        vs[3].Color = color;

        //    //        vs[4].Position = new Vector3(startDistance + length, 0, 0);
        //    //        vs[4].Color = color;
        //    //        vs[5].Position = new Vector3(startDistance + length - (tipSize * 2), 0, -tipSize);
        //    //        vs[5].Color = color;
        //    //    }

        //    //    device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
        //    //    device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vs, 0, 3);

        //    //    pass.End();
        //    //}
        //    //_hudEffect.End();
        //}
        public void BuildBoatPathCurve(ReplayBoat boat)
        {
            //SortedList<int, int> tempBoatDataRowsCurveMap = new SortedList<int, int>();
            //List<Vector3> tempControlPoints = new List<Vector3>();
            //List<float> tempDistances = new List<float>();

            //List<Vector3> controlPoints = new List<Vector3>();
            //for (int i = 0; i < boat.SensorReadings.Count; i++)
            //{
            //    CoordinatePoint cp = new CoordinatePoint(new Coordinate(boat.SensorReadings[i].latitude), new Coordinate(boat.SensorReadings[i].longitude), 0);
            //    controlPoints.Add(cp.Project().ToWorld());
            //}
            //List<Vector3> curvePoints = BezierHelper.CreateSmoothedLine(controlPoints, out tempBoatDataRowsCurveMap, out tempControlPoints, out tempDistances);
            //_boatDataRowsCurveMap[boat] = tempBoatDataRowsCurveMap;
            //_boatPathCurve[boat] = new VertexPositionColor[(curvePoints.Count * 2) - 2];
            //_boatPathControlPoints[boat] = tempControlPoints;
            //_boatDataRowsDistances[boat] = tempDistances;

            //float maxWidth = 0.2f;//meters/coord divisor
            //float minWidth = 0.01f;//meters/coord divisor
            //float maxSpeed = 20f;//kmh
            ////float minSpeed = 0f;//kmh

            //int vertexIndex = 0;

            //for (int i = 0; i < boat.SensorReadings.Count - 2; i++)
            //{
            //    int curveStart = _boatDataRowsCurveMap[boat][i];
            //    int curveEnd = _boatDataRowsCurveMap[boat][i + 1];

            //    float startDistance = _boatDataRowsDistances[boat][i];
            //    float endDistance = _boatDataRowsDistances[boat][i + 1];

            //    float startSpeed = (startDistance / 1000) / (float)(boat.SensorReadings[i + 1].datetime - boat.SensorReadings[i].datetime).TotalHours;
            //    float endSpeed = (endDistance / 1000) / (float)(boat.SensorReadings[i + 2].datetime - boat.SensorReadings[i + 1].datetime).TotalHours;

            //    float speedDelta = endSpeed - startSpeed;
            //    int curvePointCount = curveEnd - curveStart;

            //    for (int c = curveStart; c < curveEnd; c++)
            //    {
            //        float percentThrough = ((float)c - (float)curveStart) / (float)curvePointCount;
            //        float speed = startSpeed + (percentThrough * speedDelta);

            //        float width = minWidth + ((speed / maxSpeed) * (maxWidth - minWidth));

            //        width = maxWidth - width;

            //        float angleToNext = -(float)Math.Atan2(curvePoints[c + 1].Z - curvePoints[c].Z, curvePoints[c + 1].X - curvePoints[c].X);
            //        float angleRight = angleToNext + MathHelper.PiOver2;
            //        float angleLeft = angleToNext - MathHelper.PiOver2;

            //        float rightX = curvePoints[c].X + (float)Math.Cos(angleRight) * (width / 2f);
            //        float rightZ = curvePoints[c].Z - (float)Math.Sin(angleRight) * (width / 2f);
            //        VertexPositionColor right = new VertexPositionColor();
            //        right.Position = new Vector3(rightX, 0, rightZ);
            //        _boatPathCurve[boat][vertexIndex] = right;
            //        vertexIndex++;

            //        float leftX = curvePoints[c].X + (float)Math.Cos(angleLeft) * (width / 2f);
            //        float leftZ = curvePoints[c].Z - (float)Math.Sin(angleLeft) * (width / 2f);
            //        VertexPositionColor left = new VertexPositionColor();
            //        left.Position = new Vector3(leftX, 0, leftZ);
            //        _boatPathCurve[boat][vertexIndex] = left;
            //        vertexIndex++;
            //    }
            //}
        }
        //public void DrawBoat(ReplayBoat boat, GraphicsDevice device, CameraMan cameraMan, DateTime time)
        //{
        //    //if (boat.ProjectedPoint != null)
        //    //{
        //    //    Camera camera = cameraMan.Camera;
        //    //    float angle = boat.Angle;
        //    //    if (boat.Direction == ReplayBoat.BoatDirection.Backwards)
        //    //    {
        //    //        angle = AngleHelper.NormalizeAngle(boat.Angle + MathHelper.Pi);
        //    //    }

        //    //    foreach (ModelMesh mesh in _boatModel.Meshes)
        //    //    {
        //    //        foreach (BasicEffect mfx in mesh.Effects)
        //    //        {
        //    //            mfx.EnableDefaultLighting();
        //    //            mfx.AmbientLightColor = DrawingToXnaColor(boat.Color).ToVector3();
        //    //            mfx.World = Matrix.CreateScale(0.02f) * Matrix.CreateTranslation(new Vector3(0f, 0f, -0.25f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(boat.Heel)) * Matrix.CreateRotationY(MathHelper.ToRadians(90.0f)) * Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(boat.ProjectedPoint.ToWorld());
        //    //            camera.ConfigureBasicEffect(mfx);
        //    //        }
        //    //        mesh.Draw();
        //    //    }

        //    //    camera.ConfigureBasicEffect(_sailEffect);
        //    //    _sailEffect.World = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(boat.BoomAngle) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0.25f)) * Matrix.CreateTranslation(new Vector3(0, 0, -0.36f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-boat.Heel)) * Matrix.CreateRotationY(MathHelper.ToRadians(270.0f) + angle) * Matrix.CreateTranslation(boat.ProjectedPoint.ToWorld());
        //    //    _sailEffect.Begin();
        //    //    foreach (EffectPass pass in _sailEffect.CurrentTechnique.Passes)
        //    //    {
        //    //        pass.Begin();
        //    //        _boatMains[boat].Draw(device, boat.SailCurve, 0);
        //    //        pass.End();
        //    //    }
        //    //    _sailEffect.End();

        //    //    camera.ConfigureBasicEffect(_sailEffect);
        //    //    _sailEffect.World = Matrix.CreateScale(0.1f) /** Matrix.CreateRotationY(currentBoomAngle)*/ * Matrix.CreateTranslation(new Vector3(0f, 0f, 0.25f)) * Matrix.CreateTranslation(new Vector3(0, 0, -0.36f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-boat.Heel)) * Matrix.CreateRotationY(MathHelper.ToRadians(270.0f) + angle) * Matrix.CreateTranslation(boat.ProjectedPoint.ToWorld());
        //    //    _sailEffect.Begin();
        //    //    foreach (EffectPass pass in _sailEffect.CurrentTechnique.Passes)
        //    //    {
        //    //        pass.Begin();
        //    //        _boatJibs[boat].Draw(device, boat.SailCurve, (boat.BoomAngle * 80f) * 0.02f);
        //    //        pass.End();
        //    //    }
        //    //    _sailEffect.End();
        //    //}
        //}
        //public void DrawBouy(Bouy bouy, GraphicsDevice device, Camera camera)
        //{
        //    //foreach (ModelMesh mesh in _bouyModel.Meshes)
        //    //{
        //    //    foreach (BasicEffect mfx in mesh.Effects)
        //    //    {
        //    //        //mfx.DiffuseColor = new Vector3(255, 128, 64);
        //    //        mfx.EnableDefaultLighting();
        //    //        mfx.AmbientLightColor = new Vector3(1, 0.5f, 0);
        //    //        mfx.DiffuseColor = new Vector3(1, 0.5f, 0);
        //    //        mfx.SpecularColor = new Vector3(1, 0.5f, 0);
        //    //        mfx.World = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(bouy.CoordinatePoint.Project().ToWorld());
        //    //        camera.ConfigureBasicEffect(mfx);
        //    //    }
        //    //    mesh.Draw();
        //    //}
        //}
        //private Texture2D LoadAndScaleTexture(string texturePath, GraphicsDevice device)
        //{
            //TextureInformation ti = Texture2D.GetTextureInformation(texturePath);
            //int bigTexture = ti.Width >= ti.Height ? ti.Width : ti.Height;
            //int smallDevice = device.GraphicsDeviceCapabilities.MaxTextureWidth <= device.GraphicsDeviceCapabilities.MaxTextureHeight ? device.GraphicsDeviceCapabilities.MaxTextureWidth : device.GraphicsDeviceCapabilities.MaxTextureHeight;

            //int desiredSize = bigTexture >= smallDevice ? smallDevice : bigTexture;


            //int desiredSizePow2 = desiredSize;
            //desiredSizePow2--;
            //desiredSizePow2 |= desiredSizePow2 >> 1;
            //desiredSizePow2 |= desiredSizePow2 >> 2;
            //desiredSizePow2 |= desiredSizePow2 >> 4;
            //desiredSizePow2 |= desiredSizePow2 >> 8;
            //desiredSizePow2 |= desiredSizePow2 >> 16;
            //desiredSizePow2++;

            //Texture2D ret = Texture2D.FromFile(device, texturePath, desiredSizePow2, desiredSizePow2);
            //return ret;
        //}
        private float ScaleFontToFitWidth(string s, float desiredWidth)
        {
            //Vector2 size = _font.MeasureString(s);
            //float multiplier = desiredWidth / size.X;
            //if (multiplier > 0.75f)
            //{
            //    multiplier = 0.75f;
            //}
            //return multiplier;
            throw new NotImplementedException();
        }
        //private void DrawMouseInstructions(TimeSpan time, float viewportWidth, float viewportHeight)
        //{
        //    //if (time < new TimeSpan(0, 0, 12) && viewportWidth >= 320 && viewportHeight >= 240)
        //    //{
        //    //    _batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

        //    //    string instructions;
        //    //    Texture2D texture;

        //    //    float maxBackgroundAlpha = 0.3f;
        //    //    float backgroundAlpha = maxBackgroundAlpha;
        //    //    float messageAlpha = 1f;

        //    //    if (time < new TimeSpan(0, 0, 4))
        //    //    {
        //    //        if (time < new TimeSpan(0, 0, 1))
        //    //        {
        //    //            backgroundAlpha = ((float)time.Milliseconds / 1000) * maxBackgroundAlpha;
        //    //            messageAlpha = ((float)time.Milliseconds / 1000);
        //    //        }
        //    //        else if (time > new TimeSpan(0, 0, 3))
        //    //        {
        //    //            messageAlpha = ((float)(1000 - time.Milliseconds) / 1000);
        //    //        }

        //    //        //left click
        //    //        instructions = "Left click and drag to rotate the camera.";
        //    //        texture = _mouseLeftTexture;
        //    //    }
        //    //    else if (time < new TimeSpan(0, 0, 8))
        //    //    {
        //    //        if (time < new TimeSpan(0, 0, 5))
        //    //        {
        //    //            messageAlpha = ((float)time.Milliseconds / 1000);
        //    //        }
        //    //        else if (time > new TimeSpan(0, 0, 7))
        //    //        {
        //    //            messageAlpha = ((float)(1000 - time.Milliseconds) / 1000);
        //    //        }

        //    //        //right click
        //    //        instructions = "Right click and drag to zoom in and out.";
        //    //        texture = _mouseRightTexture;
        //    //    }
        //    //    else if (time < new TimeSpan(0, 0, 12))
        //    //    {
        //    //        if (time < new TimeSpan(0, 0, 9))
        //    //        {
        //    //            messageAlpha = ((float)time.Milliseconds / 1000);
        //    //        }
        //    //        else if (time > new TimeSpan(0, 0, 11))
        //    //        {
        //    //            backgroundAlpha = ((float)(1000 - time.Milliseconds) / 1000) * maxBackgroundAlpha;
        //    //            messageAlpha = ((float)(1000 - time.Milliseconds) / 1000);
        //    //        }

        //    //        //double click
        //    //        instructions = "Double click on a boat to follow it.";
        //    //        if (time.Seconds % 2 == 0)
        //    //        {
        //    //            texture = _mouseLeftTexture;
        //    //        }
        //    //        else
        //    //        {
        //    //            texture = _mouseTexture;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        throw new Exception("Unknown instruction state");
        //    //    }

        //    //    //find the dimensions of the text and image
        //    //    float imageWidth = (float)texture.Width;
        //    //    float imageHeight = (float)texture.Height;
        //    //    float fontScale = ScaleFontToFitWidth(instructions, viewportWidth - imageWidth - 50);
        //    //    float instructionsWidth = _font.MeasureString(instructions).X * fontScale;
        //    //    float instructionsHeight = _font.MeasureString(instructions).Y * fontScale;
        //    //    //find the height and width of our message box
        //    //    float height = imageHeight * 1.5f;
        //    //    float width = viewportWidth;

        //    //    //draw the gray background
        //    //    _line.ClearVectors();
        //    //    _line.Colour = new Color(new Vector4(1f, 1f, 1f, backgroundAlpha));
        //    //    _line.Thickness = height;
        //    //    _line.AddVector(new Vector2(0, viewportHeight - height));
        //    //    _line.AddVector(new Vector2(viewportWidth, viewportHeight - height));
        //    //    _line.Render(_batch);
        //    //    //draw thew mouse texture
        //    //    _batch.Draw(texture, new Vector2(25, (viewportHeight - height) + (height - imageHeight) / 2), new Color(1f, 1f, 1f, messageAlpha));
        //    //    //draw the instructions text
        //    //    _batch.DrawString(_font, instructions, new Vector2(25 + imageWidth + 25, (viewportHeight - height) + (height - instructionsHeight) / 2), new Color(1f, 1f, 1f, messageAlpha), 0.0f, new Vector2(0, 0), fontScale, SpriteEffects.None, 0);
        //    //    _batch.End();
        //    //}
        //}
        //private void DrawLabelBox(Vector2 boatLocation, SpriteBatch batch, System.Drawing.Color color, List<string> text, ref List<BoundingBox> otherBoxes, float viewportWidth, float viewportHieght)
        //{
        //    //float hpad = 5;
        //    //float vpad = 2.5f;
        //    //float width = 0;
        //    //float height = 0;
        //    //float lineHeight = 0;
        //    //Color normalTextColor = Color.Black;

        //    //float fontScale = 0.5f;

        //    //foreach (string s in text)
        //    //{
        //    //    Vector2 size = _font.MeasureString(s);
        //    //    size = size * fontScale;
        //    //    if (size.X + (2 * hpad) > width)
        //    //    {
        //    //        width = size.X + (2 * hpad);
        //    //    }
        //    //    if (size.Y + (2 * vpad) > lineHeight)
        //    //    {
        //    //        lineHeight = size.Y + (2 * vpad);
        //    //    }
        //    //}
        //    //height = lineHeight * text.Count;

        //    ////find an appropriate offset so that the label is towards the outer edge of the viewport
        //    //Vector2 labelLocation = new Vector2(0f, 0f);
        //    //Vector2 labelOffset = boatLocation - new Vector2(viewportWidth / 2f, viewportHieght / 2f);
        //    //labelOffset.X = (labelOffset.X / (viewportWidth / 2f)) * 200f;
        //    //labelOffset.Y = (labelOffset.Y / (viewportHieght / 2f)) * 200f;
        //    ////use the offset to figure out where we would LIKE to put the label
        //    //labelLocation.X = boatLocation.X + labelOffset.X - (width / 2f);
        //    //labelLocation.Y = boatLocation.Y + labelOffset.Y - (height / 2f) - 50;

        //    ////check and make sure we're not drawing the label off the screen
        //    ////if we are, fix it
        //    //if (labelLocation.X < 0)
        //    //    labelLocation.X = 0;
        //    //if (labelLocation.X + width > viewportWidth)
        //    //    labelLocation.X = viewportWidth - width;
        //    //if (labelLocation.Y > viewportHieght)
        //    //    labelLocation.Y = viewportHieght;
        //    //if (labelLocation.Y - height < 0)
        //    //    labelLocation.Y = height;


        //    //////see if it's a valid place to put it
        //    //bool locationSet = CheckBounds(otherBoxes, labelLocation, width, height, viewportWidth, viewportWidth);
        //    //float offset = 5;


        //    ////we intentionally exhause all possibilities looking "up" before we look "down"
        //    ////to avoid the labels jumping around when it gets stuck on another label
        //    ////if it's not valid, look "up" for a clear spot
        //    //if (!locationSet)
        //    //{
        //    //    while (offset < viewportHieght)
        //    //    {
        //    //        if (CheckBounds(otherBoxes, labelLocation + new Vector2(0, -offset), width, height, viewportWidth, viewportWidth))
        //    //        {
        //    //            labelLocation = labelLocation + new Vector2(0, -offset);
        //    //            locationSet = true;
        //    //            break;
        //    //        }
        //    //        offset = offset + 5;
        //    //    }
        //    //}
        //    ////if it's still not valid, look "down" for a clear spot
        //    //if (!locationSet)
        //    //{
        //    //    offset = 5;
        //    //    while (offset < viewportHieght)
        //    //    {
        //    //        //up
        //    //        if (CheckBounds(otherBoxes, labelLocation + new Vector2(0, offset), width, height, viewportWidth, viewportWidth))
        //    //        {
        //    //            labelLocation = labelLocation + new Vector2(0, offset);
        //    //            locationSet = true;
        //    //            break;
        //    //        }
        //    //        offset = offset + 5;
        //    //    }
        //    //}

        //    ////if we found a spot draw the label
        //    //if (locationSet)
        //    //{
        //    //    //figure out where the line should hit the label box, and what borders to draw
        //    //    Vector2 labelConnectPoint = new Vector2();
        //    //    if (boatLocation.X < labelLocation.X)
        //    //    {
        //    //        labelConnectPoint.X = labelLocation.X;
        //    //    }
        //    //    else if (boatLocation.X >= labelLocation.X && boatLocation.X <= labelLocation.X + width)
        //    //    {
        //    //        labelConnectPoint.X = boatLocation.X;
        //    //    }
        //    //    else if (boatLocation.X > labelLocation.X)
        //    //    {
        //    //        labelConnectPoint.X = labelLocation.X + width;
        //    //    }

        //    //    if (boatLocation.Y < labelLocation.Y - height)
        //    //    {
        //    //        labelConnectPoint.Y = labelLocation.Y - height;
        //    //    }
        //    //    else if (boatLocation.Y >= labelLocation.Y - height && boatLocation.Y <= labelLocation.Y)
        //    //    {
        //    //        labelConnectPoint.Y = boatLocation.Y;
        //    //    }
        //    //    else if (boatLocation.Y > labelLocation.Y)
        //    //    {
        //    //        labelConnectPoint.Y = labelLocation.Y;
        //    //    }

        //    //    //background
        //    //    _line.ClearVectors();
        //    //    _line.Colour = new Color(new Vector4(1f, 1f, 1f, 0.4f));
        //    //    _line.Thickness = height;
        //    //    _line.AddVector(labelLocation + new Vector2(0, -height));
        //    //    _line.AddVector(labelLocation + new Vector2(width, -height));
        //    //    _line.Render(batch);

        //    //    //pointer
        //    //    _line.ClearVectors();
        //    //    _line.Colour = color;
        //    //    _line.Thickness = 1f;
        //    //    _line.AddVector(boatLocation);
        //    //    _line.AddVector(labelConnectPoint);
        //    //    _line.Render(batch);

        //    //    //border
        //    //    _line.ClearVectors();
        //    //    _line.Colour = color;
        //    //    _line.Thickness = 2f;
        //    //    _line.AddVector(labelLocation);
        //    //    _line.AddVector(new Vector2(labelLocation.X, labelLocation.Y - height));
        //    //    _line.AddVector(new Vector2(labelLocation.X + width, labelLocation.Y - height));
        //    //    _line.Render(batch);

        //    //    //draw our text
        //    //    for (int i = 0; i < text.Count; i++)
        //    //    {
        //    //        Color c;
        //    //        if (i == 0)
        //    //        {
        //    //            c = color;
        //    //        }
        //    //        else
        //    //        {
        //    //            c = normalTextColor;
        //    //        }
        //    //        Vector2 pos = new Vector2(labelLocation.X + hpad, (labelLocation.Y - height) + (i * lineHeight) + vpad);
        //    //        _batch.DrawString(_font, text[i], pos, c, 0.0f, new Vector2(0, 0), fontScale, SpriteEffects.None, 0);
        //    //    }

        //    //    //add the bounding box for this label to the collection so other labels can avoid us
        //    //    BoundingBox b = new BoundingBox();
        //    //    b.Min = new Vector3(labelLocation.X, labelLocation.Y, -1);
        //    //    b.Max = new Vector3(labelLocation.X + width, labelLocation.Y + height, 1);
        //    //    otherBoxes.Add(b);
        //    //}
        //}
        //private bool CheckBounds(List<BoundingBox> obstacles, Vector2 position, float width, float height, float viewportWidth, float viewportHeight)
        //{
        //    if (position.X >= 0 && position.X <= viewportWidth && position.Y >= 0 && position.Y <= viewportHeight && position.X + width >= 0 && position.X + width <= viewportWidth && position.Y - height >= 0 && position.Y <= viewportHeight)
        //    {
        //        BoundingBox pb = new BoundingBox();
        //        pb.Min = new Vector3(position.X, position.Y, -1);
        //        pb.Max = new Vector3(position.X + width, position.Y + height, 1);

        //        foreach (BoundingBox b in obstacles)
        //        {
        //            if (b.Contains(pb) != ContainmentType.Disjoint)
        //            {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        //bounds go off the screen
        //        return false;
        //    }
        //}
        //private void DrawInstrument(InstrumentDrawing drawingType, System.Drawing.Color color, Vector3 location, float rotation, float startDistance, float length)
        //{
        //    //float tipSize = 0.05f;
        //    //_instruments.World = Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(location);
        //    //_instruments.Begin();
        //    //foreach (EffectPass pass in _instruments.CurrentTechnique.Passes)
        //    //{
        //    //    pass.Begin();

        //    //    VertexPositionColor[] vs;
        //    //    vs = new VertexPositionColor[6];
        //    //    vs[0].Position = new Vector3(startDistance, 0, 0);
        //    //    vs[0].Color = color;
        //    //    vs[1].Position = new Vector3(startDistance + length, 0, 0);
        //    //    vs[1].Color = color;

        //    //    if (drawingType == InstrumentDrawing.InwardArrow)
        //    //    {
        //    //        vs[2].Position = new Vector3(startDistance, 0, 0);
        //    //        vs[2].Color = color;
        //    //        vs[3].Position = new Vector3(startDistance + (tipSize * 2), 0, tipSize);
        //    //        vs[3].Color = color;

        //    //        vs[4].Position = new Vector3(startDistance, 0, 0);
        //    //        vs[4].Color = color;
        //    //        vs[5].Position = new Vector3(startDistance + (tipSize * 2), 0, -tipSize);
        //    //        vs[5].Color = color;
        //    //    }
        //    //    else if (drawingType == InstrumentDrawing.OutwardArrow)
        //    //    {
        //    //        vs[2].Position = new Vector3(startDistance + length, 0, 0);
        //    //        vs[2].Color = color;
        //    //        vs[3].Position = new Vector3(startDistance + length - (tipSize * 2), 0, tipSize);
        //    //        vs[3].Color = color;

        //    //        vs[4].Position = new Vector3(startDistance + length, 0, 0);
        //    //        vs[4].Color = color;
        //    //        vs[5].Position = new Vector3(startDistance + length - (tipSize * 2), 0, -tipSize);
        //    //        vs[5].Color = color;
        //    //    }
        //    //    _device.RenderState.DepthBufferEnable = false;
        //    //    _device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
        //    //    _device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vs, 0, 3);
        //    //    _device.RenderState.DepthBufferEnable = true;

        //    //    pass.End();
        //    //}
        //    //_instruments.End();
        //}
        //private bool IsBoatOnScreen(Vector2 b, IViewPort vp)
        //{
        //    return (b.X > 0 && b.X < vp.RenderTarget.Width && b.Y > 0 && b.Y < vp.RenderTarget.Height);
        //}
        private void SetOffsets()
        {
            _placeHolderCP = new CoordinatePoint(new Coordinate(Replay.Race.Lake.South), new Coordinate(Replay.Race.Lake.West), 0.0);
            _eastingOffset = 0;
            _northingOffset = 0;
            
            if (this.Race.Lake.BottomLeftInMeters.Zone != this.Race.Lake.TopRightInMeters.Zone)
            {
                double zoneSize = this.Race.Lake.East - this.Race.Lake.West;
                if (zoneSize > 6.0)
                {
                    CoordinatePoint.LongitudeZoneSize = zoneSize;
                }
                CoordinatePoint.LongitudeOffset = this.Race.Lake.West;
            }

            if (this.Race.Lake.BottomLeftInMeters.Northing < 0 && this.Race.Lake.TopRightInMeters.Northing < 0)
            {
                _northingOffset = -(this.Race.Lake.TopRightInMeters.Northing * 2.0);
            }
            else if (this.Race.Lake.BottomLeftInMeters.Northing > 0 && this.Race.Lake.TopRightInMeters.Northing > 0)
            {
                _northingOffset = this.Race.Lake.BottomLeftInMeters.Northing;
            }

            if (this.Race.Lake.BottomLeftInMeters.Easting < 0 && this.Race.Lake.TopRightInMeters.Easting < 0)
            {
                _eastingOffset = -(this.Race.Lake.TopRightInMeters.Easting * 2.0);
            }
            else if (this.Race.Lake.BottomLeftInMeters.Easting > 0 && this.Race.Lake.TopRightInMeters.Easting > 0)
            {
                _eastingOffset = this.Race.Lake.BottomLeftInMeters.Easting;
            }
        }
        private Vector3 ProjectedPointToWorld(ProjectedPoint point,CameraMan cameraMan)
        {
            GdiCameraMan gdiCM=(GdiCameraMan)cameraMan;

            //note that the y/northing value is inverted to account for the fact that screen coordinates are inverted
            Vector3 v = new Vector3((float)(point.Easting-_eastingOffset),-(float)(point.Northing-_northingOffset),0f);
            
            //now zoom appropriatly
            v.X = v.X * gdiCM.Zoom;
            v.Y = v.Y * gdiCM.Zoom;

            //now offset so that the point is aligned with the camera correctly
            v.X = v.X - gdiCM.X;
            v.Y = v.Y - gdiCM.Y;

            return v;
        }
        //private ProjectedPoint ProjectedPointFromWorld(Vector3 v)
        //{
        //    ProjectedPoint pp = new ProjectedPoint();
        //    pp.northing = (v.X + _xOffset) * _coordinateDivisor;
        //    pp.easting = (v.Z + _zOffset) * _coordinateDivisor;
        //    pp.height = (v.Y * _coordinateDivisor);
        //    return pp;
        //}
        private string ExtractBoatStatisticString(ReplayBoat b, IViewPort target, string name, DateTime time)
        {
            string speedString = name + ": ";
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
        //private Color DrawingToXnaColor(System.Drawing.Color from)
        //{
        //    return new Color(from.R, from.G, from.B);
        //}
        private void RenderVideoTitle()
        {
            //System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(_xnaAviWriter.Buffer);
            //int width = _xnaAviWriter.Buffer.Width;
            //int height = _xnaAviWriter.Buffer.Height;
            //g.FillRectangle(System.Drawing.Brushes.Black, 0, 0, width, height);

            //float titleFontSize = 14;
            //int padding = 5;
            //System.Drawing.Font italic = new System.Drawing.Font("Arial", titleFontSize, System.Drawing.FontStyle.Italic);
            //System.Drawing.Font bold = new System.Drawing.Font("Arial", titleFontSize, System.Drawing.FontStyle.Bold);
            //System.Drawing.Font regular = new System.Drawing.Font("Arial", titleFontSize, System.Drawing.FontStyle.Regular);
            //System.Drawing.SizeF visualSize = g.MeasureString("visual", italic);
            //System.Drawing.SizeF sailSize = g.MeasureString("Sail", bold);
            //System.Drawing.SizeF dotComSize = g.MeasureString(".com", regular);
            //int titleOffset = (width - (int)(visualSize.Width + sailSize.Width + dotComSize.Width)) / 2;
            //g.DrawString("visual", italic, System.Drawing.Brushes.White, titleOffset, height - (visualSize.Height + padding));
            //g.DrawString("Sail", bold, System.Drawing.Brushes.White, titleOffset + visualSize.Width - padding, height - (visualSize.Height + padding));
            //g.DrawString(".com", regular, System.Drawing.Brushes.White, titleOffset + visualSize.Width + sailSize.Width - padding - padding, height - (visualSize.Height + padding));

            //int heightOffset = (int)((double)height * 0.4);

            //float raceNameFontSize = 30;
            //if (Replay.Race.Name != Race.DefaultName)
            //{
            //    System.Drawing.Font raceNameFont = null;
            //    System.Drawing.SizeF raceNameSize = new System.Drawing.SizeF();
            //    do
            //    {
            //        raceNameFont = new System.Drawing.Font("Arial", raceNameFontSize, System.Drawing.FontStyle.Regular);
            //        raceNameSize = g.MeasureString(Replay.Race.Name, raceNameFont);
            //        raceNameFontSize = raceNameFontSize - 2;
            //    }
            //    while ((raceNameSize.Width > width) && raceNameFontSize > 16f);
            //    g.DrawString(Replay.Race.Name, raceNameFont, System.Drawing.Brushes.White, (width - (int)raceNameSize.Width) / 2, heightOffset);
            //    heightOffset = heightOffset + (int)raceNameSize.Height + padding;
            //}

            //float lakeNameFontSize = raceNameFontSize / 2;
            //if (Replay.Race.Lake.Name != Lake.DefaultName)
            //{
            //    System.Drawing.Font lakeNameFont = null;
            //    System.Drawing.SizeF lakeNameSize = new System.Drawing.SizeF();
            //    do
            //    {
            //        lakeNameFont = new System.Drawing.Font("Arial", lakeNameFontSize, System.Drawing.FontStyle.Regular);
            //        lakeNameSize = g.MeasureString(Replay.Race.Lake.Name, lakeNameFont);
            //        lakeNameFontSize = lakeNameFontSize - 2;
            //    }
            //    while ((lakeNameSize.Width > width) && lakeNameFontSize > 16f);
            //    g.DrawString(Replay.Race.Lake.Name, lakeNameFont, System.Drawing.Brushes.White, (width - (int)lakeNameSize.Width) / 2, heightOffset);
            //    heightOffset = heightOffset + (int)lakeNameSize.Height + padding;
            //}

            //float raceDateFontSize = lakeNameFontSize;
            //System.Drawing.Font raceDateFont = null;
            //System.Drawing.SizeF raceDateSize = new System.Drawing.SizeF();
            //do
            //{
            //    raceDateFont = new System.Drawing.Font("Arial", raceDateFontSize, System.Drawing.FontStyle.Regular);
            //    raceDateSize = g.MeasureString(Replay.Race.LocalStart.ToShortDateString(), raceDateFont);
            //    raceDateFontSize = raceDateFontSize - 2;
            //}
            //while ((raceDateSize.Width > width) && raceDateFontSize > 16f);
            //g.DrawString(Replay.Race.LocalStart.ToShortDateString(), raceDateFont, System.Drawing.Brushes.White, (width - (int)raceDateSize.Width) / 2, heightOffset);
            //heightOffset = heightOffset + (int)raceDateSize.Height + padding;

            //g.Flush();
            ////_xnaAviWriter.CommitBufferChanges();
            ////record for 2 seconds
            //for (int i = 0; i < _xnaAviWriter.FrameRate * 2; i++)
            //{
            //    _xnaAviWriter.RepeatBuffer();
            //}

        }
    }
}
