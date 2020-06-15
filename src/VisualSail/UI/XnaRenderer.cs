
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Data.Statistics;


namespace AmphibianSoftware.VisualSail.UI
{
    public class XnaRenderer:Renderer
    {
        //private fields
        private bool _lakeTextureAvailible = false;
        private Notify _updateStatistics;
        private DateTime _previousRenderTime = DateTime.MinValue;

        private Dictionary<IViewPort, XnaCameraMan> _viewports;
        //private Dictionary<IViewPort, Dictionary<ReplayBoat, int>> _viewportOffsets;

        //used to convert projected points into 3d space to eliminate loss of precision
        private float _coordinateDivisor = 10f;//factor by which projected coordinates are divided to make minimize loss or precision with floats in xna
        private float _xOffset;//subtracted from the x value of projected coordinates to minimize loss of precision with floats
        private float _zOffset;//subtracted from the z value of projected coordinates to minimize loss of precision with floats
        private CoordinatePoint _placeHolderCP; //does nothing other than keep a CP in memory so that the static fields don't clear
        //private ProjectedPoint _placeHolder; //does nothing other than keep a pp in memory so that the static fields don't clear
        
        //boat specific stuff
        private Dictionary<ReplayBoat,VertexPositionColor[]> _boatPathCurve;
        private Dictionary<ReplayBoat,SortedList<int, int>> _boatDataRowsCurveMap;
        private Dictionary<ReplayBoat,List<float>> _boatDataRowsDistances;
        private Dictionary<ReplayBoat,List<Vector3>> _boatPathControlPoints;
        private Dictionary<ReplayBoat, Sail> _boatMains;
        private Dictionary<ReplayBoat, Sail> _boatJibs;


        //xna stuff
        private Game _game;
        private BoundingBox _worldBounds;
        private PrimitiveLine _line;
        private VertexPositionColor[] _gridLines;
        private VertexPositionTexture[] _skybox;
        private ContentManager _content;
        private GraphicsDeviceManager _graphics;
        private GraphicsDevice _device;
        private Model _boatModel;
        private Model _bouyModel;
        private BasicEffect _instruments;
        private BasicEffect _lakeTextureEffect;
        private BasicEffect _skyBoxEffect;
        private BasicEffect _sailEffect;
        private BasicEffect _hudEffect;
        private Texture2D _lakeTexture;
        private Texture2D _skyTexture;
        private Texture2D _mouseTexture;
        private Texture2D _mouseLeftTexture;
        private Texture2D _mouseRightTexture;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _batch;
        private SpriteFont _font;

        public XnaRenderer()
        {
        }

        public override void Initialize(Replay replay)
        {
            base.Initialize(replay);
            SetOffsets();
            _viewports = new Dictionary<IViewPort, XnaCameraMan>();
            _boatPathCurve = new Dictionary<ReplayBoat, VertexPositionColor[]>();
            _boatDataRowsCurveMap = new Dictionary<ReplayBoat,SortedList<int,int>>();
            _boatDataRowsDistances = new Dictionary<ReplayBoat, List<float>>();
            _boatPathControlPoints = new Dictionary<ReplayBoat, List<Vector3>>();
            _boatMains = new Dictionary<ReplayBoat, Sail>();
            _boatJibs = new Dictionary<ReplayBoat, Sail>();

            foreach (ReplayBoat boat in replay.Boats)
            {
                _boatPathCurve[boat]=new VertexPositionColor[0];
                _boatDataRowsCurveMap[boat]=new SortedList<int,int>();
                _boatDataRowsDistances[boat]=new List<float>();
                _boatPathControlPoints[boat]=new List<Vector3>();
                _boatMains[boat]=new Sail(Sail.SailType.Main,10, 4, 2, 2);
                _boatJibs[boat]=new Sail(Sail.SailType.Jib, 8, 3, 2,1);
                BuildBoatPathCurve(boat);
            }

            _game = new Game();
			_graphics = new GraphicsDeviceManager(_game);
            //_graphics.CreateDevice ();
            
			_graphics.IsFullScreen = false;
            _graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(_graphics_PreparingDeviceSettings);
            Resize();
            _content = new ContentManager(_game.Services);
			_content.RootDirectory = "";//GetContentPath("");
			_graphics.DeviceReset += new EventHandler<EventArgs>(graphics_DeviceReset);
        }

        void _graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
			e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.Reach;
			e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 0;
            //e.GraphicsDeviceInformation.DeviceType = DeviceType.Reference;
            //e.GraphicsDeviceInformation.PresentationParameters.MultiSampleType = MultiSampleType.None;
        }
        public override void Reset()
        {
            LoadContent();
            SetUpGridLines();
            SetupSkyBox();
        }
        private void graphics_DeviceReset(object sender, EventArgs e)
        {
            Resize();
        }
        public override void Resize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;//_backBufferWidth;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;//_backBufferHeight;
            
            _graphics.ApplyChanges();

			_device = _graphics.GraphicsDevice;
            
			if (_device == null) 
			{
				throw new InvalidOperationException ("GraphicsDevice is null");
			}

			//_device.RenderState.AlphaBlendEnable = true;
			//_device.RenderState.SourceBlend = Blend.SourceAlpha; // source rgb * source alpha
			if (_device.BlendState == null) 
			{
				_device.BlendState = BlendState.AlphaBlend;
			}
			//_device.BlendState.AlphaSourceBlend = Blend.SourceAlpha;
            //_device.RenderState.DestinationBlend = Blend.InverseSourceAlpha; // dest rgb * (255 - source alpha)
			//_device.RenderState.CullMode = CullMode.None;
			if (_device.RasterizerState == null) 
			{
				_device.RasterizerState = RasterizerState.CullNone;
			}
			//camera.Resize(_target.Width, _target.Height);

			VertexDeclarationHelper.Add(typeof(VertexPositionColor), VertexPositionColor.VertexDeclaration);
            VertexDeclarationHelper.Add(typeof(VertexPositionNormalColored), new VertexDeclaration(VertexPositionNormalColored.VertexElements));
            //VertexDeclarationHelper.Add(typeof(AmphibianSoftware.VisualSail.UI.Ocean.VertexMultitextured), new VertexDeclaration(device, AmphibianSoftware.VisualSail.UI.Ocean.VertexMultitextured.VertexElements));
			VertexDeclarationHelper.Add(typeof(VertexPositionTexture),  VertexPositionTexture.VertexDeclaration);
        }
        private void LoadContent()
        {
            //LoadBouys();
            //foreach (AmphibianSoftware.VisualSail.Data.Boat dboat in this.Race.Boats)
            //{
            //    AmphibianSoftware.VisualSail.Library.Boat b = new AmphibianSoftware.VisualSail.Library.Boat(dboat, this.Race.UtcReplayStart, this.Race.UtcReplayEnd/*, new Notify(this.UpdateStatistics)*/);
            //    b.LoadResources(_device, _content, this.Race.UtcReplayStart);
            //    _boats.Add(b);
            //}

            //foreach (AmphibianSoftware.VisualSail.Library.Bouy b in _bouys)
            //{
            //    b.LoadResources(_device, _content);
            //}
            _skyTexture = LoadAndScaleTexture("average_day.jpg",false, _device);

			_mouseTexture = LoadAndScaleTexture("mouse.png",true, _device);
			_mouseLeftTexture = LoadAndScaleTexture("mouse-left.png",true, _device);
			_mouseRightTexture = LoadAndScaleTexture("mouse-right.png",true, _device);

            _lakeTextureEffect = new BasicEffect(_device);
//            if (!File.Exists(ContentHelper.DynamicContentPath + SatelliteImageryHelper.GetFileName(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West)))
//            {
//                try
//                {
//                    string lakeFile = SatelliteImageryHelper.GetSatelliteImage(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West, (int)this.Race.Lake.WidthInMeters / 10, (int)this.Race.Lake.HeightInMeters / 10);
//                    //FileInfo fi = new FileInfo(lakeFile);
//                    //fi.MoveTo(ContentHelper.ContentPath + SatelliteImageryHelper.GetFileName(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West));
//                }
//                catch //(Exception e)
//                {
//                }
//            }
			//imagery doesn't work anymore since the nasa service went down
//            try
//            {
//                _lakeTexture = LoadAndScaleTexture(SatelliteImageryHelper.GetFileName(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West), _device);
//                //_lakeTexture = Texture2D.FromFile(device, ContentHelper.ContentPath + SatelliteImageryHelper.GetFileName(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West));
//                //_lakeTexture = content.Load<Texture2D>(ContentHelper.ContentPath + SatelliteImageryHelper.GetFileName(this.Race.Lake.North, this.Race.Lake.South, this.Race.Lake.East, this.Race.Lake.West));
//                _lakeTextureEffect.Texture = _lakeTexture;
//                _lakeTextureEffect.TextureEnabled = true;
//                _lakeTextureAvailible = true;
//
//                //lakeTextureEffect.FogEnabled = true;
//                //lakeTextureEffect.FogColor = Color.White.ToVector3();
//                //lakeTextureEffect.FogStart = Camera.FarClipDistance - 300;
//                //lakeTextureEffect.FogEnd = Camera.FarClipDistance;
//                //MessageBox.Show("Loaded lake texture:"+_lakeTexture.Width+"x"+_lakeTexture.Height);
//            }
//            catch (Exception /*e*/)
//            {
                //MessageBox.Show("Failed to load area texture." + e.Message + ":" + e.StackTrace);
                _lakeTextureAvailible = false;
            //}

            _skyBoxEffect = new BasicEffect(_device);
            _skyBoxEffect.Texture = _skyTexture;
            _skyBoxEffect.TextureEnabled = true;
            _skyBoxEffect.FogEnabled = true;
            _skyBoxEffect.FogColor = Color.White.ToVector3();
            _skyBoxEffect.FogStart = Camera.FarClipDistance - 300;
            _skyBoxEffect.FogEnd = Camera.FarClipDistance;

            //water = new BasicEffect(device, null);
            //water.TextureEnabled = true;
            //water.Alpha = 0.5f;
            //water.Texture = waterTexture;

            //effect = new BasicEffect(device, null);
            //effect.EnableDefaultLighting();

            _instruments = new BasicEffect(_device);
            _instruments.VertexColorEnabled = true;

            //text = new BasicEffect(device, null);

			_font=_content.Load<SpriteFont> (GetContentPath ("tahoma"));
            //_font = _content.Load<SpriteFont>(ContentHelper.StaticContentPath + "tahoma");
            _batch = new SpriteBatch(_device);
            _line = new PrimitiveLine(_device);

            //_photos = Photo.FindInDateRange(this.Race.LocalCountdownStart, this.Race.LocalEnd);
            //_bouyModel = _content.Load<Model>(ContentHelper.StaticContentPath + "bouy");
            //_boatModel = _content.Load<Model>(ContentHelper.StaticContentPath + "ship");

			_bouyModel =_content.Load<Model> (GetContentPath ("bouy"));
			_boatModel = _content.Load<Model> (GetContentPath ("ship"));

			_sailEffect = new BasicEffect(_device);
            _sailEffect.EnableDefaultLighting();
            //System.Drawing.Color c=System.Drawing.Color.FromArgb(_boatData.Color);
            //_color = new Vector3((float)c.R / 255.0f, (float)c.G / 255.0f, (float)c.B / 255.0f);
            _sailEffect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
            _sailEffect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
            //sailEffect.AmbientLightColor = _color;
            _sailEffect.VertexColorEnabled = true;
            _hudEffect = new BasicEffect(_device);
            _hudEffect.VertexColorEnabled = true;
        }
        private void SetUpGridLines()
        {
            float maxX = (float)ProjectedPointToWorld(this.Race.Course.Lake.TopLeftInMeters).X;
            float minX = (float)ProjectedPointToWorld(this.Race.Course.Lake.BottomRightInMeters).X;
            float maxZ = (float)ProjectedPointToWorld(this.Race.Course.Lake.BottomRightInMeters).Z;
            float minZ = (float)ProjectedPointToWorld(this.Race.Course.Lake.TopLeftInMeters).Z;

            float horizontalCount = (Math.Abs((maxX - minX)) * _coordinateDivisor);
            float verticalCount = (Math.Abs((maxZ - minZ)) * _coordinateDivisor);

            _gridLines = new VertexPositionColor[(int)((horizontalCount * 2.0) + (verticalCount * 2.0))];

            int index = 0;
            float y = -0.01f;

            //horizontal lines
            for (float c = minX; c < maxX; c = c + (1f /*/ coordinateDivisor*/))
            {
                _gridLines[index].Position = new Vector3(c, y, minZ);
                _gridLines[index].Color = Color.Blue;
                _gridLines[index + 1].Position = new Vector3(c, y, maxZ);
                _gridLines[index + 1].Color = Color.Blue;
                index = index + 2;
            }

            //horizontal lines
            for (float c = minZ; c < maxZ && index < _gridLines.Length - 1; c = c + (1f /*/ coordinateDivisor*/))
            {
                _gridLines[index].Position = new Vector3(minX, y, c);
                _gridLines[index].Color = Color.Blue;
                _gridLines[index + 1].Position = new Vector3(maxX, y, c);
                _gridLines[index + 1].Color = Color.Blue;
                index = index + 2;
            }
        }
        private void SetupSkyBox()
        {
            _skybox = new VertexPositionTexture[10];

            //texture height  skybox height
            //------------- = ----------------
            //texture width   skybox perimeter

            //

            double perimeter = this.Race.Lake.WidthInMeters * 2.0 + this.Race.Lake.HeightInMeters * 2.0;
            double height = (_skyTexture.Height * perimeter) / _skyTexture.Width;
            height = height / 5;
            double widthToHeightRatio = this.Race.Lake.WidthInMeters / this.Race.Lake.HeightInMeters;

            double w = this.Race.Lake.WidthInMeters / perimeter;
            double h = this.Race.Lake.HeightInMeters / perimeter;

            _skybox[0] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.TopLeftInMeters), new Vector2(0, 0));
            _skybox[1] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.TopLeftInMeters), new Vector2(0, 1));
            _skybox[0].Position.Y = (float)height;

            _skybox[2] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.TopRightInMeters), new Vector2((float)w, 0));
            _skybox[3] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.TopRightInMeters), new Vector2((float)w, 1));
            _skybox[2].Position.Y = (float)height;

            _skybox[4] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.BottomRightInMeters), new Vector2((float)w + (float)h, 0));
            _skybox[5] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.BottomRightInMeters), new Vector2((float)w + (float)h, 1));
            _skybox[4].Position.Y = (float)height;

            _skybox[6] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters), new Vector2((float)w + (float)h + (float)w, 0));
            _skybox[7] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters), new Vector2((float)w + (float)h + (float)w, 1));
            _skybox[6].Position.Y = (float)height;

            _skybox[8] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.TopLeftInMeters), new Vector2((float)w + (float)h + (float)w + (float)h, 0));
            _skybox[9] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.TopLeftInMeters), new Vector2((float)w + (float)h + (float)w + (float)h, 1));
            _skybox[8].Position.Y = (float)height;

            float padding = 2f;
            float minX;

            if (ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).X > ProjectedPointToWorld(this.Race.Lake.BottomRightInMeters).X)
            {
                minX = ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).X;
            }
            else
            {
                minX = ProjectedPointToWorld(this.Race.Lake.BottomRightInMeters).X;
            }

            float minZ;
            if (ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).Z > ProjectedPointToWorld(this.Race.Lake.TopLeftInMeters).Z)
            {
                minZ = ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).Z;
            }
            else
            {
                minZ = ProjectedPointToWorld(this.Race.Lake.TopLeftInMeters).Z;
            }
            minX = minX + padding;
            minZ = minZ + padding;


            float maxX;
            if (ProjectedPointToWorld(this.Race.Lake.TopLeftInMeters).X > ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).X)
            {
                maxX = ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).X;
            }
            else
            {
                maxX = ProjectedPointToWorld(this.Race.Lake.TopLeftInMeters).X;
            }

            float maxZ;
            if (ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).Z > ProjectedPointToWorld(this.Race.Lake.BottomRightInMeters).Z)
            {
                maxZ = ProjectedPointToWorld(this.Race.Lake.BottomRightInMeters).Z;
            }
            else
            {
                maxZ = ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).Z;
            }
            maxX = maxX - padding;
            maxZ = maxZ - padding;

            Vector3 min = new Vector3(minX, 0, minZ);
            Vector3 max = new Vector3(maxX, (float)height, maxZ);
            _worldBounds = new BoundingBox(min, max);
        }
        public override void Shutdown()
        {
            VertexDeclarationHelper.Clear();
            //try
            //{
				//_device.EvictManagedResources();
            //}
            //catch { };
            _device.Dispose();
            _content.Dispose();
            _instruments.Dispose();
            _lakeTextureEffect.Dispose();
            _skyBoxEffect.Dispose();
            if (_lakeTexture != null)
            {
                _lakeTexture.Dispose();
            }
            if (_lakeTexture != null)
            {
                _skyTexture.Dispose();
            }
            _line.Dispose();
            _batch.Dispose();
        }
        private void RefreshViewports()
        {
            foreach (IViewPort vp in _viewports.Keys)
            {
                /*Dictionary<ReplayBoat, int> offsets = */vp.SetBoatList(this.Replay.Boats);
                //_viewportOffsets[vp] = offsets;
            }
        }
        public override void AddViewPort(IViewPort viewport)
        {
            viewport.SetMaxSize(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            XnaCameraMan man = new XnaCameraMan(new Camera(_worldBounds),0f,(float)(Math.PI) + (((float)Math.PI) / 20f) + (((float)Math.PI) / 20f),10f);
            man.SelectedBoat = 0;
            man.DrawSatelliteImagery = _lakeTextureAvailible;
            viewport.CameraMan = man;
            //List<string> boatNames = new List<string>();
            //List<string> boatNumbers = new List<string>();
            //List<System.Drawing.Color> boatColors = new List<System.Drawing.Color>();
            //foreach (AmphibianSoftware.VisualSail.Library.Boat b in boats)
            //{
            //    boatNames.Add(b.Name);
            //    boatNumbers.Add(b.Number);
            //    boatColors.Add(b.Color);
            //}
            /*Dictionary<ReplayBoat, int> offsets = */
            viewport.SetBoatList(this.Replay.Boats);
            viewport.Shutdown = new ShutdownViewPort(this.RemoveViewPort);
            lock (_viewports)
            {
                _viewports.Add(viewport, man);
            }
            //lock (_viewportOffsets)
            //{
            //    _viewportOffsets.Add(viewport, offsets);
            //}
        }
        public override void RemoveViewPort(IViewPort viewport)
        {
            lock (_viewports)
            {
                if (_viewports.ContainsKey(viewport) && _viewports[viewport].CurrentPhotoTexture != null)
                {
                    _viewports[viewport].CurrentPhotoTexture.Dispose();
                    _viewports[viewport].CurrentPhotoTexture = null;
                }
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
            if (n - _previousRenderTime >= new TimeSpan(0, 0, 0, 0, 15))
            {
                lock (_viewports)
                {
                    IViewPort recordingViewport = null;
                    foreach (IViewPort vp in _viewports.Keys)
                    {
                        if (vp.Record == RecorderState.Recording || vp.Record == RecorderState.Paused)
                        {
                            recordingViewport = vp;
                            break;
                        }
                    }

                    //if we found somebody recording
                    if (recordingViewport != null)
                    {
                        foreach (IViewPort vp in _viewports.Keys)
                        {
                            if (vp != recordingViewport)
                            {
                                vp.Record = RecorderState.Disabled;
                            }
                        }
                    }
                    else
                    {
                        foreach (IViewPort vp in _viewports.Keys)
                        {
                            vp.RecordingPath = null;
                            vp.Record = RecorderState.Ready;
                        }
                    }

                    foreach (IViewPort vp in _viewports.Keys)
                    {
                        _device.PresentationParameters.DeviceWindowHandle = vp.RenderTarget.Handle;
                        _graphics.ApplyChanges();
                        Render(vp);
                        if (vp.ScreenshotPath != null && vp.RecordingSize.Width > 0 && vp.RecordingSize.Height > 0)
                        {
                            vp.ScreenshotPath = null;
                        }
                    }
                }
                _previousRenderTime = n;
            }

        }
        private void Render(IViewPort target)
        {
            XnaCameraMan cameraMan = _viewports[target];
            if (cameraMan.DrawSatelliteImagery && _lakeTextureAvailible)
            {
                _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.White, 1.0f, 0);
            }
            else
            {
                _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            }
            if (this.Replay.Boats[_viewports[target].SelectedBoat].ProjectedPoint != null)
            {
                cameraMan.FollowBoat(ProjectedPointToWorld(this.Replay.Boats[_viewports[target].SelectedBoat].ProjectedPoint));
                Camera camera = cameraMan.Camera;
                camera.UpdatePosition();
                camera.ConfigureBasicEffect(_instruments);

                Viewport viewport = _device.Viewport;
                if (target.RenderTarget.Height <= _device.PresentationParameters.BackBufferHeight)
                {
                    viewport.Height = target.RenderTarget.Height;
                }
                else
                {
                    viewport.Height = _device.PresentationParameters.BackBufferHeight;
                }
                if (target.RenderTarget.Width <= _device.PresentationParameters.BackBufferWidth)
                {
                    viewport.Width = target.RenderTarget.Width;
                }
                else
                {
                    viewport.Width = _device.PresentationParameters.BackBufferWidth;
                }
                viewport.X = 0;
                viewport.Y = 0;
                _device.Viewport = viewport;

                if (cameraMan.DrawSatelliteImagery)
                {
                    if (_lakeTextureAvailible)
                    {
                        VertexPositionTexture[] textureTris = new VertexPositionTexture[4];
                        textureTris[0] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.TopLeftInMeters), new Vector2(0, 0));
                        textureTris[1] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters), new Vector2(0, 1));
                        textureTris[2] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.TopRightInMeters), new Vector2(1, 0));
                        textureTris[3] = new VertexPositionTexture(ProjectedPointToWorld(this.Race.Lake.BottomRightInMeters), new Vector2(1, 1));
                        textureTris[0].Position.Y = -0.5f;
                        textureTris[0].Position.Y = -0.5f;
                        textureTris[0].Position.Y = -0.5f;
                        textureTris[0].Position.Y = -0.5f;
                        camera.ConfigureBasicEffect(_lakeTextureEffect);
                        _lakeTextureEffect.World = Matrix.Identity;
						//_lakeTextureEffect.Begin();
                        foreach (EffectPass pass in _lakeTextureEffect.CurrentTechnique.Passes)
                        {
							pass.Apply ();
                            //pass.Begin();
							//_device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionTexture));
                            _device.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, textureTris, 0, 2);
                            //pass.End();
                        }
                        //_lakeTextureEffect.End();
                    }
                    camera.ConfigureBasicEffect(_skyBoxEffect);
                    _skyBoxEffect.World = Matrix.Identity;
                    //_skyBoxEffect.Begin();
                    foreach (EffectPass pass in _skyBoxEffect.CurrentTechnique.Passes)
                    {
						pass.Apply ();
                        //pass.Begin();
                        //_device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionTexture));
                        _device.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, _skybox, 0, 8);
                        //pass.End();
                    }
                    //_skyBoxEffect.End();
                }

                if (cameraMan.DrawGrid)
                {
                    _instruments.World = Matrix.Identity;
                    //_instruments.Begin();
                    foreach (EffectPass pass in _instruments.CurrentTechnique.Passes)
                    {
						pass.Apply ();
                        //pass.Begin();
                        //_device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
                        _device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, _gridLines, 0, _gridLines.Length / 2);
                        //pass.End();
                    }
                    //_instruments.End();
                }
                Dictionary<ReplayBoat, Vector2> locations = new Dictionary<ReplayBoat, Vector2>();
                foreach (ReplayBoat b in this.Replay.Boats)
                {
                    DrawHUD(b,_device, cameraMan, this.Replay.SimulationTime, _coordinateDivisor);
                    if (cameraMan.ShowAnyIdentifiers || target.ClickedPoint != null || cameraMan.PhotoMode != CameraMan.PhotoDisplayMode.Disabled)
                    {
                        if (b.ProjectedPoint != null)
                        {
                            Vector3 twoD = viewport.Project(ProjectedPointToWorld(b.ProjectedPoint), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
                            if (twoD.Z < 1f)
                            {
                                locations.Add(b, new Vector2(twoD.X, twoD.Y));
                            }
                        }
                    }
                }
                foreach (ReplayBoat b in this.Replay.Boats)
                {
                    DrawBoat(b,_device, cameraMan, this.Replay.SimulationTime);
                }

                Dictionary<Bouy, Vector2> bouyLocations = new Dictionary<Bouy, Vector2>();
                foreach (Mark m in this.Replay.Race.Course.Marks)
                {
                    foreach (Bouy b in m.Bouys)
                    {
                        DrawBouy(b, _device, camera);
                        if (cameraMan.ShowMarkNames)
                        {
                            Vector3 twoD = viewport.Project(ProjectedPointToWorld(b.CoordinatePoint.Project()), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
                            if (twoD.Z < 1f)
                            {
                                bouyLocations.Add(b, new Vector2(twoD.X, twoD.Y));
                            }
                        }
                    }
                }

                ////course navigation debug lines
                //List<VertexPositionColor> markRoundLines = new List<VertexPositionColor>();
                //for (int i = 0; i < this.Race.Course.Route.Count; i++)
                //{
                //    if (i > 0 && i < this.Race.Course.Route.Count - 1)
                //    {
                //        Vector3 a = ProjectedPointToWorld(this.Race.Course.Route[i].AveragedLocation.Project());
                //        Vector3 b = ProjectedPointToWorld(this.Race.Course.Route[i].FindMarkRoundPoint(this.Race.Course.Route[i - 1], this.Race.Course.Route[i + 1]));

                //        VertexPositionColor vpcA = new VertexPositionColor(a, Color.Red);
                //        VertexPositionColor vpcB = new VertexPositionColor(b, Color.Red);
                //        markRoundLines.Add(vpcA);
                //        markRoundLines.Add(vpcB);
                //    }
                //}
                //_instruments.World = Matrix.Identity;
                //_instruments.Begin();
                //foreach (EffectPass pass in _instruments.CurrentTechnique.Passes)
                //{
                //    pass.Begin();
                //    _device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
                //    _device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, markRoundLines.ToArray(), 0, markRoundLines.Count / 2);
                //    pass.End();
                //}
                //_instruments.End();
                ////end course navigation debug

                DrawMouseInstructions(DateTime.Now - target.CreatedAt, viewport.Width, viewport.Height);

                if (cameraMan.ShowAnyIdentifiers || cameraMan.ShowMarkNames || cameraMan.DrawPlaybackSpeed || cameraMan.PhotoMode != CameraMan.PhotoDisplayMode.Disabled)
                {
					_batch.Begin (SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    //TODO: what was savestatemode and how is it implemented in mono
					//_batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
                    List<BoundingBox> labelBoxBounds = new List<BoundingBox>();

                    //if (cameraMan.PhotoMode != CameraMan.PhotoDisplayMode.Disabled)
                    //{
                    //    BoundingBox box = DrawPhoto(locations, target, cameraMan);
                    //    if (box != default(BoundingBox))
                    //    {
                    //        labelBoxBounds.Add(box);
                    //    }
                    //}

                    if (cameraMan.DrawPlaybackSpeed)
                    {
                        
                        string playString = "";
                        if (Replay.Speed == 0 /*|| _play==false*/)
                        {
                            playString = "||";
                        }
                        else if (Replay.Speed == 1)
                        {
                            playString = ">";
                        }
                        else if (Replay.Speed > 1)
                        {
                            playString = ">> " + Replay.Speed + "x";
                        }
                        else if (Replay.Speed == -1)
                        {
                            playString = "<";
                        }
                        else if (Replay.Speed < -1)
                        {
                            playString = "<< " + Replay.Speed + "x";
                        }
                        else
                        {
                            playString = "?";
                        }
                        _batch.DrawString(_font, playString, new Vector2(50, 50), new Color(new Vector4(1, 1, 1, .75f)));
                    }

                    if (cameraMan.ShowAnyIdentifiers)
                    {
                        foreach (ReplayBoat b in locations.Keys)
                        {
                            Vector2 p = locations[b];
                            if (IsBoatOnScreen(p, target))
                            {
                                List<string> text = new List<string>();
                                string topRow = "";

                                if (cameraMan.ShowName)
                                {
                                    topRow = topRow + b.Name;
                                }

                                if (cameraMan.ShowNumber)
                                {
                                    topRow = topRow + " " + b.Number;
                                }

                                if (cameraMan.ShowPosition)
                                {
                                    int pos = b.GetCurrentPosition(this.Replay.SimulationTime);
                                    string v = VerbageHelper.PositionString(pos);
                                    if (v.Length > 0)
                                    {
                                        topRow = topRow + " (" + v + ")";
                                    }
                                }

                                if (topRow.Length > 0)
                                {
                                    text.Add(topRow);
                                }

                                if (cameraMan.ShowSpeed)
                                {
                                    
                                    text.Add(ExtractBoatStatisticString(b, target, "Speed", this.Replay.SimulationTime));
                                }
                                if (cameraMan.ShowVMGToCourse)
                                {
                                    text.Add(ExtractBoatStatisticString(b, target, "VMG to Course", this.Replay.SimulationTime));
                                }
                                if (cameraMan.ShowDistanceToMark)
                                {
                                    text.Add(ExtractBoatStatisticString(b, target, "Distance to Mark", this.Replay.SimulationTime));
                                }
                                if (cameraMan.ShowDistanceToCourse)
                                {
                                    text.Add(ExtractBoatStatisticString(b, target, "Distance to Course", this.Replay.SimulationTime));
                                }
                                if (cameraMan.ShowAngleToMark)
                                {
                                    text.Add(ExtractBoatStatisticString(b, target, "Angle to Mark", this.Replay.SimulationTime));
                                }
                                if (cameraMan.ShowAngleToWind)
                                {
                                    text.Add(ExtractBoatStatisticString(b, target, "Angle to Wind", this.Replay.SimulationTime));
                                }
                                if (cameraMan.ShowAngleToCourse)
                                {
                                    text.Add(ExtractBoatStatisticString(b, target, "Angle to Course", this.Replay.SimulationTime));
                                }

                                DrawLabelBox(p, _batch, new Color(b.Color.R, b.Color.G, b.Color.B, b.Color.A), text, ref labelBoxBounds, viewport.Width, viewport.Height);
                            }
                        }
                    }

                    if (cameraMan.ShowMarkNames)
                    {
                        foreach (Bouy b in bouyLocations.Keys)
                        {
                            Vector2 p = bouyLocations[b];
                            if (p.X > 0 && p.X < viewport.Width && p.Y > 0 && p.Y < viewport.Height)
                            {
                                List<string> text = new List<string>();
                                text.Add(b.Mark.Name);
                                DrawLabelBox(p, _batch, new Color(new Vector3(1, 0.5f, 0)), text, ref labelBoxBounds, viewport.Width, viewport.Height);
                            }
                        }
                    }

                    _batch.End();
                }

                if (target.ClickedPoint != null)
                {
                    Vector2 clicked = new Vector2((float)target.ClickedPoint.Value.X, (float)target.ClickedPoint.Value.Y);
                    float minDistance = float.MaxValue;
                    int selectedIndex = -1;
                    for (int i = 0; i < locations.Keys.Count; i++)
                    {
                        float distance = Vector2.Distance(clicked, locations[locations.Keys.ElementAt(i)]);
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

                try
                {
                    //make sure we have a handle before we try and draw
                    if (target.HasHandle)
                    {
                        lock (target.RenderTarget)
                        {
							_device.PresentationParameters.DeviceWindowHandle = target.RenderTarget.Handle;
							_device.PresentationParameters.BackBufferWidth = _device.Viewport.Width;
							_device.PresentationParameters.BackBufferHeight = _device.Viewport.Height;
                            _graphics.ApplyChanges();
                            
							//TODO: make this render to a texture and render it in GDI?
                            
                            //_device.Present(new Rectangle(0, 0, _device.Viewport.Width, _device.Viewport.Height), new Rectangle(0, 0, _device.Viewport.Width, _device.Viewport.Height), target.RenderTarget.Handle);
							_device.Present();
                            
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("No Handle");
                    }
                }
                catch (Exception ex)
                {
                    //if the windows are being docked/undocked or similiar, the handle might get destroyed mid-draw
                    //it's ok if this happens, we'll just draw again on the next frame.
                    //this is hacky, but i can't find a better way to prevent or catch this issue
                }

            }
        }
        public void DrawHUD(ReplayBoat boat, GraphicsDevice device, XnaCameraMan cameraMan, DateTime time, float coordinateDivisor)
        {
			//device.DepthStencilState.DepthBufferEnable = false;
            //device.RenderState.DepthBufferEnable = false;
            Camera camera = cameraMan.Camera;
            float angle = boat.Angle;
            if (boat.Direction == ReplayBoat.BoatDirection.Backwards)
            {
                angle = AngleHelper.NormalizeAngle(boat.Angle + MathHelper.Pi);
            }



            camera.ConfigureBasicEffect(_hudEffect);
            _hudEffect.World = Matrix.Identity;
            //_hudEffect.Begin();

            foreach (EffectPass pass in _hudEffect.CurrentTechnique.Passes)
            {
				pass.Apply ();
                //pass.Begin();
                if (cameraMan.DrawPastPath || cameraMan.DrawFuturePath)
                {
                    DateTime startTime = boat.SensorReadings[boat.CurrentSensorReadingIndex].datetime;
                    DateTime endTime = boat.SensorReadings[boat.CurrentSensorReadingIndex].datetime;
                    int startIndex = boat.CurrentSensorReadingIndex;
                    int endIndex = boat.CurrentSensorReadingIndex;
                    if (cameraMan.DrawPastPath)
                    {
                        startTime = startTime - new TimeSpan(0, 0, cameraMan.DrawPathLength);
                        while (startIndex > 0 && boat.SensorReadings[startIndex].datetime > startTime)
                        {
                            startIndex--;
                        }
                    }
                    if (cameraMan.DrawFuturePath)
                    {
                        endTime = endTime + new TimeSpan(0, 0, cameraMan.DrawPathLength);
                        while (endIndex < boat.SensorReadings.Count && boat.SensorReadings[endIndex].datetime < endTime)
                        {
                            endIndex++;
                        }
                    }

                    if (_boatDataRowsCurveMap[boat].ContainsKey(startIndex) && _boatDataRowsCurveMap[boat].ContainsKey(endIndex))
                    {

                        int curveStart = _boatDataRowsCurveMap[boat][startIndex] * 2;
                        int curveEnd = _boatDataRowsCurveMap[boat][endIndex] * 2;

                        int curveLength = (curveEnd - curveStart) / 2;
                        int curveMidPoint = _boatDataRowsCurveMap[boat][boat.CurrentSensorReadingIndex] * 2;
                        for (int i = curveStart; i < curveEnd; i++)
                        {
                            float distance;
                            if (i < boat.CurrentSensorReadingIndex)
                            {
                                distance = (float)Math.Abs(i - curveMidPoint) / (float)Math.Abs(curveStart - curveMidPoint);
                            }
                            else if (i > boat.CurrentSensorReadingIndex)
                            {
                                distance = (float)Math.Abs(i - curveMidPoint) / (float)Math.Abs(curveEnd - curveMidPoint);
                            }
                            else
                            {
                                distance = 1f;
                            }

                            float alpha = 1 - distance;

                            Color c = new Color(new Vector4(boat.Color.R, boat.Color.G, boat.Color.B, alpha));
                            _boatPathCurve[boat][i].Color = c;
                        }
                        //device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
                        device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, _boatPathCurve[boat], curveStart, curveLength * 2);
                    }
                }
                //pass.End();
            }

            //_hudEffect.End();

            if (boat.CurrentRacingStatus != ReplayBoat.RacingStatus.Finished && cameraMan.DrawAngleToMark)
            {
                //float angleToMark = AngleHelper.FindAngle(CurrentMarkLocation.ToWorld(), ProjectedPoint.ToWorld());
                DrawInstrument(device, InstrumentDrawing.OutwardArrow, Microsoft.Xna.Framework.Color.Orange, ProjectedPointToWorld(boat.ProjectedPoint), boat.Angle, 0.5f, 0.5f);
            }

            if (cameraMan.DrawAngleToWind)
            {
                DrawInstrument(device, InstrumentDrawing.InwardArrow, Microsoft.Xna.Framework.Color.Green, ProjectedPointToWorld(boat.ProjectedPoint), Replay.WindAngle + MathHelper.Pi, 0.5f, 0.5f);
            }

            if (cameraMan.DrawAbsoluteAngleReference || cameraMan.DrawRelativeAngleReference)
            {
                int line = 0;
                for (float a = 0f; a < MathHelper.TwoPi; a = a + (MathHelper.PiOver4 / 2.0f))
                {
                    float length = 0.1f;
                    if (line == 0)
                        length = 0.4f;
                    else if (line % 4 == 0)
                        length = 0.3f;
                    else if (line % 2 == 0)
                        length = 0.2f;

                    if (cameraMan.DrawRelativeAngleReference)
                    {
                        InstrumentDrawing id = InstrumentDrawing.Line;
                        if (line == 0)
                        {
                            id = InstrumentDrawing.OutwardArrow;
                        }
                        DrawInstrument(device, id, Microsoft.Xna.Framework.Color.Gray, ProjectedPointToWorld(boat.ProjectedPoint), a + boat.Angle, 1.0f - length, length);
                    }
                    if (cameraMan.DrawAbsoluteAngleReference)
                    {
                        DrawInstrument(device, InstrumentDrawing.Line, Microsoft.Xna.Framework.Color.LightGray, ProjectedPointToWorld(boat.ProjectedPoint), a, 1.0f, length);
                    }
                    line++;
                }
            }
			//device.DepthStencilState.DepthBufferEnable = true;
            //device.RenderState.DepthBufferEnable = true;
        }
        private void DrawInstrument(GraphicsDevice device, InstrumentDrawing drawingType, Color color, Vector3 location, float rotation, float startDistance, float length)
        {
            float tipSize = 0.05f;
            _hudEffect.World = Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(location);
            //_hudEffect.Begin();
            foreach (EffectPass pass in _hudEffect.CurrentTechnique.Passes)
            {
				pass.Apply ();
                //pass.Begin();

                VertexPositionColor[] vs;
                vs = new VertexPositionColor[6];
                vs[0].Position = new Vector3(startDistance, 0, 0);
                vs[0].Color = color;
                vs[1].Position = new Vector3(startDistance + length, 0, 0);
                vs[1].Color = color;

                if (drawingType == InstrumentDrawing.InwardArrow)
                {
                    vs[2].Position = new Vector3(startDistance, 0, 0);
                    vs[2].Color = color;
                    vs[3].Position = new Vector3(startDistance + (tipSize * 2), 0, tipSize);
                    vs[3].Color = color;

                    vs[4].Position = new Vector3(startDistance, 0, 0);
                    vs[4].Color = color;
                    vs[5].Position = new Vector3(startDistance + (tipSize * 2), 0, -tipSize);
                    vs[5].Color = color;
                }
                else if (drawingType == InstrumentDrawing.OutwardArrow)
                {
                    vs[2].Position = new Vector3(startDistance + length, 0, 0);
                    vs[2].Color = color;
                    vs[3].Position = new Vector3(startDistance + length - (tipSize * 2), 0, tipSize);
                    vs[3].Color = color;

                    vs[4].Position = new Vector3(startDistance + length, 0, 0);
                    vs[4].Color = color;
                    vs[5].Position = new Vector3(startDistance + length - (tipSize * 2), 0, -tipSize);
                    vs[5].Color = color;
                }

                //device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vs, 0, 3);

                //pass.End();
            }
            //_hudEffect.End();
        }
        public void BuildBoatPathCurve(ReplayBoat boat)
        {
            SortedList<int, int> tempBoatDataRowsCurveMap = new SortedList<int, int>();
            List<Vector3> tempControlPoints = new List<Vector3>();
            List<float> tempDistances = new List<float>();

            List<Vector3> controlPoints = new List<Vector3>();
            for (int i = 0; i < boat.SensorReadings.Count; i++)
            {
                CoordinatePoint cp = new CoordinatePoint(new Coordinate(boat.SensorReadings[i].latitude), new Coordinate(boat.SensorReadings[i].longitude), 0);
                controlPoints.Add(ProjectedPointToWorld(cp.Project()));
            }
            List<Vector3> curvePoints = BezierHelper.CreateSmoothedLine(controlPoints, out tempBoatDataRowsCurveMap, out tempControlPoints, out tempDistances);
            _boatDataRowsCurveMap[boat] = tempBoatDataRowsCurveMap;
            _boatPathCurve[boat] = new VertexPositionColor[(curvePoints.Count * 2) - 2];
            _boatPathControlPoints[boat] = tempControlPoints;
            _boatDataRowsDistances[boat] = tempDistances;

            float maxWidth = 0.2f;//meters/coord divisor
            float minWidth = 0.01f;//meters/coord divisor
            float maxSpeed = 20f;//kmh
            //float minSpeed = 0f;//kmh

            int vertexIndex = 0;

            for (int i = 0; i < boat.SensorReadings.Count - 2; i++)
            {
                int curveStart = _boatDataRowsCurveMap[boat][i];
                int curveEnd = _boatDataRowsCurveMap[boat][i + 1];

                float startDistance = _boatDataRowsDistances[boat][i];
                float endDistance = _boatDataRowsDistances[boat][i + 1];

                float startSpeed = (startDistance / 1000) / (float)(boat.SensorReadings[i + 1].datetime - boat.SensorReadings[i].datetime).TotalHours;
                float endSpeed = (endDistance / 1000) / (float)(boat.SensorReadings[i + 2].datetime - boat.SensorReadings[i + 1].datetime).TotalHours;

                float speedDelta = endSpeed - startSpeed;
                int curvePointCount = curveEnd - curveStart;

                for (int c = curveStart; c < curveEnd; c++)
                {
                    float percentThrough = ((float)c - (float)curveStart) / (float)curvePointCount;
                    float speed = startSpeed + (percentThrough * speedDelta);

                    float width = minWidth + ((speed / maxSpeed) * (maxWidth - minWidth));

                    width = maxWidth - width;

                    float angleToNext = -(float)Math.Atan2(curvePoints[c + 1].Z - curvePoints[c].Z, curvePoints[c + 1].X - curvePoints[c].X);
                    float angleRight = angleToNext + MathHelper.PiOver2;
                    float angleLeft = angleToNext - MathHelper.PiOver2;

                    float rightX = curvePoints[c].X + (float)Math.Cos(angleRight) * (width / 2f);
                    float rightZ = curvePoints[c].Z - (float)Math.Sin(angleRight) * (width / 2f);
                    VertexPositionColor right = new VertexPositionColor();
                    right.Position = new Vector3(rightX, 0, rightZ);
                    _boatPathCurve[boat][vertexIndex] = right;
                    vertexIndex++;

                    float leftX = curvePoints[c].X + (float)Math.Cos(angleLeft) * (width / 2f);
                    float leftZ = curvePoints[c].Z - (float)Math.Sin(angleLeft) * (width / 2f);
                    VertexPositionColor left = new VertexPositionColor();
                    left.Position = new Vector3(leftX, 0, leftZ);
                    _boatPathCurve[boat][vertexIndex] = left;
                    vertexIndex++;
                }
            }
        }
        public void DrawBoat(ReplayBoat boat, GraphicsDevice device, XnaCameraMan cameraMan, DateTime time)
        {
            if (boat.ProjectedPoint != null)
            {
                Camera camera = cameraMan.Camera;
                float angle = boat.Angle;
                if (boat.Direction == ReplayBoat.BoatDirection.Backwards)
                {
                    angle = AngleHelper.NormalizeAngle(boat.Angle + MathHelper.Pi);
                }

                foreach (ModelMesh mesh in _boatModel.Meshes)
                {
                    foreach (BasicEffect mfx in mesh.Effects)
                    {
                        mfx.EnableDefaultLighting();
                        mfx.AmbientLightColor = DrawingToXnaColor(boat.Color).ToVector3();
                        mfx.World = Matrix.CreateScale(0.02f) * Matrix.CreateTranslation(new Vector3(0f, 0f, -0.25f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(boat.Heel)) * Matrix.CreateRotationY(MathHelper.ToRadians(90.0f)) * Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(ProjectedPointToWorld(boat.ProjectedPoint));
                        camera.ConfigureBasicEffect(mfx);
                    }
                    mesh.Draw();
                }

                camera.ConfigureBasicEffect(_sailEffect);
                _sailEffect.World = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(boat.BoomAngle) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0.25f)) * Matrix.CreateTranslation(new Vector3(0, 0, -0.36f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-boat.Heel)) * Matrix.CreateRotationY(MathHelper.ToRadians(270.0f) + angle) * Matrix.CreateTranslation(ProjectedPointToWorld(boat.ProjectedPoint));
                //_sailEffect.Begin();
                foreach (EffectPass pass in _sailEffect.CurrentTechnique.Passes)
                {
					pass.Apply ();
                    //pass.Begin();
                    _boatMains[boat].Draw(device, boat.SailCurve, 0);
                    //pass.End();
                }
                //_sailEffect.End();

                camera.ConfigureBasicEffect(_sailEffect);
                _sailEffect.World = Matrix.CreateScale(0.1f) /** Matrix.CreateRotationY(currentBoomAngle)*/ * Matrix.CreateTranslation(new Vector3(0f, 0f, 0.25f)) * Matrix.CreateTranslation(new Vector3(0, 0, -0.36f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-boat.Heel)) * Matrix.CreateRotationY(MathHelper.ToRadians(270.0f) + angle) * Matrix.CreateTranslation(ProjectedPointToWorld(boat.ProjectedPoint));
                //_sailEffect.Begin();
                foreach (EffectPass pass in _sailEffect.CurrentTechnique.Passes)
                {
                	pass.Apply();
                    //pass.Begin();
                    _boatJibs[boat].Draw(device, boat.SailCurve, (boat.BoomAngle * 80f) * 0.02f);
                    //pass.End();
                }
                //_sailEffect.End();
            }
        }
        public void DrawBouy(Bouy bouy,GraphicsDevice device, Camera camera)
        {
            foreach (ModelMesh mesh in _bouyModel.Meshes)
            {
                foreach (BasicEffect mfx in mesh.Effects)
                {
                    //mfx.DiffuseColor = new Vector3(255, 128, 64);
                    mfx.EnableDefaultLighting();
                    mfx.AmbientLightColor = new Vector3(1, 0.5f, 0);
                    mfx.DiffuseColor = new Vector3(1, 0.5f, 0);
                    mfx.SpecularColor = new Vector3(1, 0.5f, 0);
                    mfx.World = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(ProjectedPointToWorld(bouy.CoordinatePoint.Project()));
                    camera.ConfigureBasicEffect(mfx);
                }
                mesh.Draw();
            }
        }

		private string GetContentPath(string name)
		{
			string exePath = System.Reflection.Assembly.GetEntryAssembly().CodeBase;
    		if(exePath.StartsWith("file:///"))
    		{
                exePath = exePath = exePath.Replace("file:///", "");
    		}
    		string exeDir = Path.GetDirectoryName(exePath);
    		string imageDir = Path.Combine(exeDir,"Content");
    		string filePath = Path.Combine(imageDir,name);
    		return filePath;
		}

        private Texture2D LoadAndScaleTexture(string texturePath,bool embedded, GraphicsDevice device)
        {
        	if(embedded)
        	{
				using(var image = EmbeddedResourceHelper.LoadImage(texturePath))
				{
					using(var mem = new MemoryStream())
					{
						image.Save(mem,System.Drawing.Imaging.ImageFormat.Bmp);
						mem.Position=0;
						var texture = Texture2D.FromStream(device,mem);
						mem.Close();
						return texture;
					}
				}
			}
        	else
        	{
        		string exePath = System.Reflection.Assembly.GetEntryAssembly().CodeBase;
        		if(exePath.StartsWith("file:///"))
        		{
                    exePath = exePath = exePath.Replace("file:///", "");
        		}
        		string exeDir = Path.GetDirectoryName(exePath);
        		string imageDir = Path.Combine(exeDir,"Images");
        		string filePath = Path.Combine(imageDir,texturePath);
	        	Texture2D texture;
				using(var fs = new FileStream(filePath,FileMode.Open))
				{
					texture = Texture2D.FromStream(device,fs);
					fs.Close();
					return texture;
				}

			}

			//it seems like monogame does the resizing for us where xna didn't?
			//we shall see
            /*TextureInformation ti = Texture2D.GetTextureInformation(texturePath);
            int bigTexture = ti.Width >= ti.Height ? ti.Width : ti.Height;
            int smallDevice = device.GraphicsDeviceCapabilities.MaxTextureWidth <= device.GraphicsDeviceCapabilities.MaxTextureHeight ? device.GraphicsDeviceCapabilities.MaxTextureWidth : device.GraphicsDeviceCapabilities.MaxTextureHeight;
            
            int desiredSize = bigTexture >= smallDevice ? smallDevice : bigTexture;


            int desiredSizePow2 = desiredSize;
            desiredSizePow2--;
            desiredSizePow2 |= desiredSizePow2 >> 1;
            desiredSizePow2 |= desiredSizePow2 >> 2;
            desiredSizePow2 |= desiredSizePow2 >> 4;
            desiredSizePow2 |= desiredSizePow2 >> 8;
            desiredSizePow2 |= desiredSizePow2 >> 16;
            desiredSizePow2++;
            
            Texture2D ret = Texture2D.FromFile(device, texturePath, desiredSizePow2, desiredSizePow2);
            return ret;*/
        }
        private float ScaleFontToFitWidth(string s, float desiredWidth)
        {
            Vector2 size = _font.MeasureString(s);
            float multiplier = desiredWidth / size.X;
            if (multiplier > 0.75f)
            {
                multiplier = 0.75f;
            }
            return multiplier;
        }
        private void DrawMouseInstructions(TimeSpan time, float viewportWidth, float viewportHeight)
        {
            if (time < new TimeSpan(0, 0, 12) && viewportWidth >= 320 && viewportHeight >= 240)
            {
				_batch.Begin (sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend);
				//TODO: figure out what savestatemode did in xna
                //_batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

                string instructions;
                Texture2D texture;

                float maxBackgroundAlpha = 0.3f;
                float backgroundAlpha = maxBackgroundAlpha;
                float messageAlpha = 1f;

                if (time < new TimeSpan(0, 0, 4))
                {
                    if (time < new TimeSpan(0, 0, 1))
                    {
                        backgroundAlpha = ((float)time.Milliseconds / 1000) * maxBackgroundAlpha;
                        messageAlpha = ((float)time.Milliseconds / 1000);
                    }
                    else if (time > new TimeSpan(0, 0, 3))
                    {
                        messageAlpha = ((float)(1000 - time.Milliseconds) / 1000);
                    }

                    //left click
                    instructions = "Left click and drag to rotate the camera.";
                    texture = _mouseLeftTexture;
                }
                else if (time < new TimeSpan(0, 0, 8))
                {
                    if (time < new TimeSpan(0, 0, 5))
                    {
                        messageAlpha = ((float)time.Milliseconds / 1000);
                    }
                    else if (time > new TimeSpan(0, 0, 7))
                    {
                        messageAlpha = ((float)(1000 - time.Milliseconds) / 1000);
                    }

                    //right click
                    instructions = "Right click and drag to zoom in and out.";
                    texture = _mouseRightTexture;
                }
                else if (time < new TimeSpan(0, 0, 12))
                {
                    if (time < new TimeSpan(0, 0, 9))
                    {
                        messageAlpha = ((float)time.Milliseconds / 1000);
                    }
                    else if (time > new TimeSpan(0, 0, 11))
                    {
                        backgroundAlpha = ((float)(1000 - time.Milliseconds) / 1000) * maxBackgroundAlpha;
                        messageAlpha = ((float)(1000 - time.Milliseconds) / 1000);
                    }

                    //double click
                    instructions = "Double click on a boat to follow it.";
                    if (time.Seconds % 2 == 0)
                    {
                        texture = _mouseLeftTexture;
                    }
                    else
                    {
                        texture = _mouseTexture;
                    }
                }
                else
                {
                    throw new Exception("Unknown instruction state");
                }

                //find the dimensions of the text and image
                float imageWidth = (float)texture.Width;
                float imageHeight = (float)texture.Height;
                float fontScale = ScaleFontToFitWidth(instructions, viewportWidth - imageWidth - 50);
                float instructionsWidth = _font.MeasureString(instructions).X * fontScale;
                float instructionsHeight = _font.MeasureString(instructions).Y * fontScale;
                //find the height and width of our message box
                float height = imageHeight * 1.5f;
                float width = viewportWidth;

                //draw the gray background
                _line.ClearVectors();
                _line.Colour = new Color(new Vector4(1f, 1f, 1f, backgroundAlpha));
                _line.Thickness = height;
                _line.AddVector(new Vector2(0, viewportHeight - height));
                _line.AddVector(new Vector2(viewportWidth, viewportHeight - height));
                _line.Render(_batch);
                //draw thew mouse texture
                _batch.Draw(texture, new Vector2(25, (viewportHeight - height) + (height - imageHeight) / 2), new Color(1f, 1f, 1f, messageAlpha));
                //draw the instructions text
                _batch.DrawString(_font, instructions, new Vector2(25 + imageWidth + 25, (viewportHeight - height) + (height - instructionsHeight) / 2), new Color(1f, 1f, 1f, messageAlpha), 0.0f, new Vector2(0, 0), fontScale, SpriteEffects.None, 0);
                _batch.End();
            }
        }
        private void DrawLabelBox(Vector2 boatLocation, SpriteBatch batch, Color color, List<string> text, ref List<BoundingBox> otherBoxes, float viewportWidth, float viewportHieght)
        {
            float hpad = 5;
            float vpad = 2.5f;
            float width = 0;
            float height = 0;
            float lineHeight = 0;
            Color normalTextColor = Color.Black;

            float fontScale = 0.5f;

            foreach (string s in text)
            {
                Vector2 size = _font.MeasureString(s);
                size = size * fontScale;
                if (size.X + (2 * hpad) > width)
                {
                    width = size.X + (2 * hpad);
                }
                if (size.Y + (2 * vpad) > lineHeight)
                {
                    lineHeight = size.Y + (2 * vpad);
                }
            }
            height = lineHeight * text.Count;

            //find an appropriate offset so that the label is towards the outer edge of the viewport
            Vector2 labelLocation = new Vector2(0f, 0f);
            Vector2 labelOffset = boatLocation - new Vector2(viewportWidth / 2f, viewportHieght / 2f);
            labelOffset.X = (labelOffset.X / (viewportWidth / 2f)) * 200f;
            labelOffset.Y = (labelOffset.Y / (viewportHieght / 2f)) * 200f;
            //use the offset to figure out where we would LIKE to put the label
            labelLocation.X = boatLocation.X + labelOffset.X - (width / 2f);
            labelLocation.Y = boatLocation.Y + labelOffset.Y - (height / 2f) - 50;

            //check and make sure we're not drawing the label off the screen
            //if we are, fix it
            if (labelLocation.X < 0)
                labelLocation.X = 0;
            if (labelLocation.X + width > viewportWidth)
                labelLocation.X = viewportWidth - width;
            if (labelLocation.Y > viewportHieght)
                labelLocation.Y = viewportHieght;
            if (labelLocation.Y - height < 0)
                labelLocation.Y = height;


            ////see if it's a valid place to put it
            bool locationSet = CheckBounds(otherBoxes, labelLocation, width, height, viewportWidth, viewportWidth);
            float offset = 5;


            //we intentionally exhause all possibilities looking "up" before we look "down"
            //to avoid the labels jumping around when it gets stuck on another label
            //if it's not valid, look "up" for a clear spot
            if (!locationSet)
            {
                while (offset < viewportHieght)
                {
                    if (CheckBounds(otherBoxes, labelLocation + new Vector2(0, -offset), width, height, viewportWidth, viewportWidth))
                    {
                        labelLocation = labelLocation + new Vector2(0, -offset);
                        locationSet = true;
                        break;
                    }
                    offset = offset + 5;
                }
            }
            //if it's still not valid, look "down" for a clear spot
            if (!locationSet)
            {
                offset = 5;
                while (offset < viewportHieght)
                {
                    //up
                    if (CheckBounds(otherBoxes, labelLocation + new Vector2(0, offset), width, height, viewportWidth, viewportWidth))
                    {
                        labelLocation = labelLocation + new Vector2(0, offset);
                        locationSet = true;
                        break;
                    }
                    offset = offset + 5;
                }
            }

            //if we found a spot draw the label
            if (locationSet)
            {
                //figure out where the line should hit the label box, and what borders to draw
                Vector2 labelConnectPoint = new Vector2();
                if (boatLocation.X < labelLocation.X)
                {
                    labelConnectPoint.X = labelLocation.X;
                }
                else if (boatLocation.X >= labelLocation.X && boatLocation.X <= labelLocation.X + width)
                {
                    labelConnectPoint.X = boatLocation.X;
                }
                else if (boatLocation.X > labelLocation.X)
                {
                    labelConnectPoint.X = labelLocation.X + width;
                }

                if (boatLocation.Y < labelLocation.Y - height)
                {
                    labelConnectPoint.Y = labelLocation.Y - height;
                }
                else if (boatLocation.Y >= labelLocation.Y - height && boatLocation.Y <= labelLocation.Y)
                {
                    labelConnectPoint.Y = boatLocation.Y;
                }
                else if (boatLocation.Y > labelLocation.Y)
                {
                    labelConnectPoint.Y = labelLocation.Y;
                }

                //background
                _line.ClearVectors();
                _line.Colour = new Color(new Vector4(1f, 1f, 1f, 0.4f));
                _line.Thickness = height;
                _line.AddVector(labelLocation + new Vector2(0, -height));
                _line.AddVector(labelLocation + new Vector2(width, -height));
                _line.Render(batch);

                //pointer
                _line.ClearVectors();
                _line.Colour = color;
                _line.Thickness = 1f;
                _line.AddVector(boatLocation);
                _line.AddVector(labelConnectPoint);
                _line.Render(batch);

                //border
                _line.ClearVectors();
                _line.Colour = color;
                _line.Thickness = 2f;
                _line.AddVector(labelLocation);
                _line.AddVector(new Vector2(labelLocation.X, labelLocation.Y - height));
                _line.AddVector(new Vector2(labelLocation.X + width, labelLocation.Y - height));
                _line.Render(batch);

                //draw our text
                for (int i = 0; i < text.Count; i++)
                {
                    Color c;
                    if (i == 0)
                    {
                        c = color;
                    }
                    else
                    {
                        c = normalTextColor;
                    }
                    Vector2 pos = new Vector2(labelLocation.X + hpad, (labelLocation.Y - height) + (i * lineHeight) + vpad);
                    _batch.DrawString(_font, text[i], pos, c, 0.0f, new Vector2(0, 0), fontScale, SpriteEffects.None, 0);
                }

                //add the bounding box for this label to the collection so other labels can avoid us
                BoundingBox b = new BoundingBox();
                b.Min = new Vector3(labelLocation.X, labelLocation.Y, -1);
                b.Max = new Vector3(labelLocation.X + width, labelLocation.Y + height, 1);
                otherBoxes.Add(b);
            }
        }
        private bool CheckBounds(List<BoundingBox> obstacles, Vector2 position, float width, float height, float viewportWidth, float viewportHeight)
        {
            if (position.X >= 0 && position.X <= viewportWidth && position.Y >= 0 && position.Y <= viewportHeight && position.X + width >= 0 && position.X + width <= viewportWidth && position.Y - height >= 0 && position.Y <= viewportHeight)
            {
                BoundingBox pb = new BoundingBox();
                pb.Min = new Vector3(position.X, position.Y, -1);
                pb.Max = new Vector3(position.X + width, position.Y + height, 1);

                foreach (BoundingBox b in obstacles)
                {
                    if (b.Contains(pb) != ContainmentType.Disjoint)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                //bounds go off the screen
                return false;
            }
        }
        private void DrawInstrument(InstrumentDrawing drawingType, Color color, Vector3 location, float rotation, float startDistance, float length)
        {
            float tipSize = 0.05f;
            _instruments.World = Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(location);
            //_instruments.Begin();
            foreach (EffectPass pass in _instruments.CurrentTechnique.Passes)
            {
            	pass.Apply();
                //pass.Begin();

                VertexPositionColor[] vs;
                vs = new VertexPositionColor[6];
                vs[0].Position = new Vector3(startDistance, 0, 0);
                vs[0].Color = color;
                vs[1].Position = new Vector3(startDistance + length, 0, 0);
                vs[1].Color = color;

                if (drawingType == InstrumentDrawing.InwardArrow)
                {
                    vs[2].Position = new Vector3(startDistance, 0, 0);
                    vs[2].Color = color;
                    vs[3].Position = new Vector3(startDistance + (tipSize * 2), 0, tipSize);
                    vs[3].Color = color;

                    vs[4].Position = new Vector3(startDistance, 0, 0);
                    vs[4].Color = color;
                    vs[5].Position = new Vector3(startDistance + (tipSize * 2), 0, -tipSize);
                    vs[5].Color = color;
                }
                else if (drawingType == InstrumentDrawing.OutwardArrow)
                {
                    vs[2].Position = new Vector3(startDistance + length, 0, 0);
                    vs[2].Color = color;
                    vs[3].Position = new Vector3(startDistance + length - (tipSize * 2), 0, tipSize);
                    vs[3].Color = color;

                    vs[4].Position = new Vector3(startDistance + length, 0, 0);
                    vs[4].Color = color;
                    vs[5].Position = new Vector3(startDistance + length - (tipSize * 2), 0, -tipSize);
                    vs[5].Color = color;
                }
                _device.DepthStencilState.DepthBufferEnable = false;
				//_device.RenderState.DepthBufferEnable = false;
				//_device.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionColor));
                _device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vs, 0, 3);
				_device.DepthStencilState.DepthBufferEnable=true;
				//_device.RenderState.DepthBufferEnable = true;

                //pass.End();
            }
            //_instruments.End();
        }
        private bool IsBoatOnScreen(Vector2 b, IViewPort vp)
        {
            return (b.X > 0 && b.X < vp.RenderTarget.Width && b.Y > 0 && b.Y < vp.RenderTarget.Height);
        }
        private void SetOffsets()
        {

            _placeHolderCP = new CoordinatePoint(new Coordinate(Replay.Race.Lake.South), new Coordinate(Replay.Race.Lake.West), 0.0);
            _xOffset = 0f;
            _zOffset = 0f;
            //ProjectedPoint.projectedPointToWorld = new ProjectedPointToWorld(this.ProjectedPointToWorld);
            //CoordinatePoint.ProjectedPoint.projectedPointFromWorld = new ProjectedPointFromWorld(this.ProjectedPointFromWorld);


            if (this.Race.Lake.BottomLeftInMeters.Zone != this.Race.Lake.TopRightInMeters.Zone)
            {
                double zoneSize = this.Race.Lake.East - this.Race.Lake.West;
                if (zoneSize > 6.0)
                {
                    CoordinatePoint.LongitudeZoneSize = zoneSize;
                }
                CoordinatePoint.LongitudeOffset = this.Race.Lake.West;
            }

            if (ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).X < 0 && ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).X < 0)
            {
                _xOffset = -(ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).X * 2f);
            }
            else if (ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).X > 0 && ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).X > 0)
            {
                _xOffset = ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).X;
            }

            if (ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).Z < 0 && ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).Z < 0)
            {
                _zOffset = -(ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).Z * 2f);
            }
            else if (ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).Z > 0 && ProjectedPointToWorld(this.Race.Lake.TopRightInMeters).Z > 0)
            {
                _zOffset = ProjectedPointToWorld(this.Race.Lake.BottomLeftInMeters).Z;
            }
        }
        private Vector3 ProjectedPointToWorld(ProjectedPoint point)
        {
            //this is passed via delagate to ProjectedPoint, and is used to convert projected points to 3d points we can use.
            Vector3 v = new Vector3((float)point.Northing / _coordinateDivisor, (float)point.Height / _coordinateDivisor, (float)point.Easting / _coordinateDivisor);
            v.X = v.X - _xOffset;
            v.Z = v.Z - _zOffset;
            return v;
        }
        //private CoordinatePoint.ProjectedPoint ProjectedPointFromWorld(Vector3 v)
        //{
        //    CoordinatePoint.ProjectedPoint pp = new CoordinatePoint.ProjectedPoint();
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
        private Color DrawingToXnaColor(System.Drawing.Color from)
        {
            return new Color(from.R, from.G, from.B);
        }
    }
}
