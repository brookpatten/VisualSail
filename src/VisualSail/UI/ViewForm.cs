using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using AmphibianSoftware.VisualSail.Library;

using WeifenLuo.WinFormsUI.Docking;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class ViewForm : DockContent,IViewPort
    {
        CameraMan _cameraMan;
        ShutdownViewPort _shutdown;
        private bool _leftMouseDown;
        private bool _rightMouseDown;
        private Point _lastMousePoint;
        private Point? _clickedPoint=null;
        private DateTime _createdAt;
        private bool _hasHandle = false;
        private RecorderState _record = RecorderState.Ready;
        private string _recordingPath;
        private string _screenshotPath = null;
        private Replay _replay;
        public ViewForm(Replay replay)
        {
            _replay = replay;
            InitializeComponent();
            //Panel.CheckForIllegalCrossThreadCalls = false;
            _createdAt = DateTime.Now;
            RenderTarget.HandleDestroyed += new EventHandler(RenderTarget_HandleDestroyed);
            RenderTarget.HandleCreated += new EventHandler(RenderTarget_HandleCreated);
            
        }

        void RenderTarget_HandleCreated(object sender, EventArgs e)
        {
            _hasHandle = true;
        }

        void RenderTarget_HandleDestroyed(object sender, EventArgs e)
        {
            _hasHandle = false;
        }
        public bool HasHandle
        {
            get
            {
                return _hasHandle;
            }
        }
        public Control RenderTarget
        {
            get
            {
                return drawPNL;
            }
        }
        public ShutdownViewPort Shutdown
        {
            set
            {
                _shutdown = value;
            }
        }
        public Point? ClickedPoint
        {
            get
            {
                return _clickedPoint;
            }
            set
            {
                _clickedPoint = value;
            }
        }
        public DateTime CreatedAt
        {
            get
            {
                return _createdAt;
            }
        }
        public /*Dictionary<ReplayBoat, int>*/void SetBoatList(List<ReplayBoat> boats)
        {
            int selected = -1;
            if (boatsLV.SelectedIndices.Count > 0)
            {
                selected = boatsLV.SelectedIndices[0];
            }
            boatsLV.Items.Clear();

            //Dictionary<ReplayBoat, int> offsets = new Dictionary<ReplayBoat, int>();
            //int offset = 0;
            //Control c = boatsLV;
            //while (c != null)
            //{
            //    if (c.Parent != null)
            //    {
            //        if (c.Parent.Parent != null)
            //        {
            //            if (c.Parent.Parent.Parent != null)
            //            {
            //                if (c.Parent.Parent.Parent.Parent != null)
            //                {
            //                    offset = offset + c.Bounds.Top;
            //                }
            //            }
            //        }
            //    }
            //    c = c.Parent;
            //}
            
            for(int i=0;i<boats.Count;i++)
            {
                string[] sub = { boats[i].Name, boats[i].Number };
                ListViewItem lvi = new ListViewItem(sub);
                lvi.ForeColor = boats[i].Color;
                boatsLV.Items.Add(lvi);

                //offsets.Add(boats[i], offset + boatsLV.Items[i].Bounds.Top + ((boatsLV.Items[i].Bounds.Bottom - boatsLV.Items[i].Bounds.Top) / 2));
            }
            if (selected >= 0 && selected < boatsLV.Items.Count)
            {
                boatsLV.Items[selected].Selected = true;
            }
            else if(boatsLV.Items.Count>0)
            {
                boatsLV.Items[0].Selected = true;
            }

            //return offsets;
        }
        public void SetSelectedBoatIndex(int index)
        {
            if (index >= 0 && index < boatsLV.Items.Count)
            {
                boatsLV.Items[index].Selected = true;
                
            }
        }
        private void drawPNL_Paint(object sender, PaintEventArgs e)
        {
        }
        

        public CameraMan CameraMan
        {
            get
            {
                return _cameraMan;
            }
            set
            {
                _cameraMan = value;
                SaveCameraConfig();
            }
        }
        private void moveUpBTN_Click(object sender, EventArgs e)
        {
            _cameraMan.CameraUp();
        }

        private void moveDownBTN_Click(object sender, EventArgs e)
        {
            _cameraMan.CameraDown();
        }

        private void moveLeftBTN_Click(object sender, EventArgs e)
        {
            _cameraMan.CameraLeft();
        }

        private void moveRightBTN_Click(object sender, EventArgs e)
        {
            _cameraMan.CameraRight();
        }

        private void zoomInBTN_Click(object sender, EventArgs e)
        {
            _cameraMan.CameraIn();
        }

        private void zoomOutBTN_Click(object sender, EventArgs e)
        {
            _cameraMan.CameraOut();
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _shutdown(this);
        }

        private void relativeAngleReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void LoadCameraConfig()
        {
        }

        private void SaveCameraConfig()
        {
            _cameraMan.DrawSatelliteImagery = satelliteImaToolStripMenuItem.Checked;
            _cameraMan.DrawRelativeAngleReference = relativeAngleReferenceToolStripMenuItem.Checked;
            _cameraMan.DrawAbsoluteAngleReference = absoluteAngleReferenceToolStripMenuItem.Checked;
            _cameraMan.DrawAngleToWind = angleToWindToolStripMenuItem.Checked;
            _cameraMan.DrawAngleToMark = angleToMarkToolStripMenuItem.Checked;
            _cameraMan.DrawPathLength = SelectedPathSeconds;
            _cameraMan.DrawPastPath = SelectedPathSeconds > 0;
            _cameraMan.DrawFuturePath = SelectedPathSeconds > 0;
            _cameraMan.DrawGrid = gridlinesToolStripMenuItem.Checked;
            _cameraMan.ShowName = showIdentifierToolStripMenuItem.Checked;
            _cameraMan.ShowNumber = numberToolStripMenuItem.Checked;
            _cameraMan.ShowPosition = positionToolStripMenuItem.Checked;
            _cameraMan.ShowMarkNames = identifyMarksToolStripMenuItem.Checked;
            _cameraMan.DrawPlaybackSpeed = playbackSpeedToolStripMenuItem.Checked;

            _cameraMan.ShowSpeed = speedToolStripMenuItem.Checked;
            _cameraMan.ShowVMGToCourse = vMGToCourseToolStripMenuItem.Checked;
            _cameraMan.ShowDistanceToMark = distanceToMarkToolStripMenuItem.Checked;
            _cameraMan.ShowDistanceToCourse = distanceToCourseToolStripMenuItem.Checked;
            _cameraMan.ShowAngleToMark = angleToMarkToolStripMenuItem1.Checked;
            _cameraMan.ShowAngleToWind = angleToWindToolStripMenuItem1.Checked;
            _cameraMan.ShowAngleToCourse = angleToCourseToolStripMenuItem.Checked;
        }

        private void absoluteAngleReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void angleToWindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void angleToMarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void noneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void CheckPath(object sender)
        {
            foreach (ToolStripMenuItem mi in pathToolStripMenuItem.DropDownItems)
            {
                mi.Checked = mi == sender;
            }
        }

        

        private int SelectedPathSeconds
        {
            get
            {
                string selectedText="";
                foreach (ToolStripMenuItem mi in pathToolStripMenuItem.DropDownItems)
                {
                    if (mi.Checked)
                    {
                        selectedText = mi.Text;
                    }
                }

                if (selectedText == "None")
                {
                    return 0;
                }
                else
                {
                    string unit = selectedText.Substring(selectedText.Length - 1);
                    string value = selectedText.Substring(0, selectedText.Length - 1);

                    int parsedValue = int.Parse(value);
                    if (unit == "m")
                    {
                        parsedValue = parsedValue * 60;
                    }

                    return parsedValue;
                }
            }
        }

        private void sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void sToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void sToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void mToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void mToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void gridlinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void boatsLV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(boatsLV.SelectedIndices.Count>0)
            {
                _cameraMan.SelectedBoat = boatsLV.SelectedIndices[0];
                this.Text = "View - " + boatsLV.SelectedItems[0].SubItems[0].Text + " " + boatsLV.SelectedItems[0].SubItems[1].Text;
                this.TabText = this.Text;
                this.DockPanel.Invalidate();
            }
        }

        private void drawPNL_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _leftMouseDown = true;
                _lastMousePoint = new Point(e.X, e.Y);
            }
            if (e.Button == MouseButtons.Right)
            {
                _rightMouseDown = true;
                _lastMousePoint = new Point(e.X, e.Y);
            }
        }

        private void drawPNL_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _leftMouseDown = false;
                _lastMousePoint = new Point(e.X, e.Y);
            }
            if (e.Button == MouseButtons.Right)
            {
                _rightMouseDown = false;
                _lastMousePoint = new Point(e.X, e.Y);
            }
        }

        private void drawPNL_MouseMove(object sender, MouseEventArgs e)
        {
            if (_leftMouseDown)
            {
                _cameraMan.CameraMove(_lastMousePoint.X - e.X, _lastMousePoint.Y - e.Y);
                _lastMousePoint = new Point(e.X, e.Y);
            }
            if (_rightMouseDown)
            {
                _cameraMan.CameraZoom(e.Y - _lastMousePoint.Y);
                _lastMousePoint = new Point(e.X, e.Y);
            }
        }

        void drawPNL_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _cameraMan.CameraZoom(-e.Delta/10);
        }

        private void satelliteImaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void mToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void mToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void mToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void mToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            CheckPath(sender);
            SaveCameraConfig();
        }

        private void hideBTN_Click(object sender, EventArgs e)
        {
            controlPNL.Visible = false;
            showBTN.Visible = true;
        }

        private void showBTN_Click(object sender, EventArgs e)
        {
            controlPNL.Visible = true;
            showBTN.Visible = false;
        }

        private void x480ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SizeToPanel(320, 240);
        }

        private void x480ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SizeToPanel(640, 480);
        }

        private void x600ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SizeToPanel(800, 600);
        }

        private void x768ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SizeToPanel(1024, 768);
        }

        private void identifyBoatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void numberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void positionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }
        
        private void drawPNL_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _clickedPoint = new Point(e.X, e.Y);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _createdAt = DateTime.Now;
        }

        private void identifyMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void showIdentifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void playbackSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        public AmphibianSoftware.VisualSail.Data.Statistics.StatisticUnitType StatisticUnitType
        {
            get
            {
                return ((SkipperMDI)this.DockPanel.Parent).SelectedUnitType;
            }
        }

        private void speedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void vMGToCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void distanceToMarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void distanceToCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void angleToMarkToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void angleToWindToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        private void angleToCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCameraConfig();
        }

        public void SetMaxSize(int width,int height)
        {
            this.MaximumSize = new Size(width, height);
        }

        public RecorderState Record 
        { 
            get
            {
                return _record;
            } 
            set
            {
                _record = value;
                switch (_record)
                {
                    case RecorderState.Disabled:
                        startRecordingToolStripMenuItem.Enabled = false;
                        pauseRecordingToolStripMenuItem.Enabled = false;
                        pauseRecordingToolStripMenuItem.Visible = true;
                        resumeRecordingToolStripMenuItem.Visible = false;
                        endRecordingToolStripMenuItem.Enabled = false;
                        endRecordingAndUploadToYouTubeToolStripMenuItem.Enabled = false;
                        imageToolStripMenuItem.Enabled = false;
                        break;
                    case RecorderState.Paused:
                        startRecordingToolStripMenuItem.Enabled = false;
                        pauseRecordingToolStripMenuItem.Visible = false;
                        pauseRecordingToolStripMenuItem.Enabled = false;
                        resumeRecordingToolStripMenuItem.Visible = true;
                        endRecordingToolStripMenuItem.Enabled = true;
                        endRecordingAndUploadToYouTubeToolStripMenuItem.Enabled = true;
                        imageToolStripMenuItem.Enabled = true;
                        break;
                    case RecorderState.Ready:
                        startRecordingToolStripMenuItem.Enabled = true;
                        pauseRecordingToolStripMenuItem.Enabled = false;
                        pauseRecordingToolStripMenuItem.Visible = true;
                        resumeRecordingToolStripMenuItem.Visible = false;
                        endRecordingToolStripMenuItem.Enabled = false;
                        endRecordingAndUploadToYouTubeToolStripMenuItem.Enabled = false;
                        imageToolStripMenuItem.Enabled = true;
                        break;
                    case RecorderState.Recording:
                        startRecordingToolStripMenuItem.Enabled = false;
                        pauseRecordingToolStripMenuItem.Visible = true;
                        pauseRecordingToolStripMenuItem.Enabled = true;
                        resumeRecordingToolStripMenuItem.Visible = false;
                        endRecordingToolStripMenuItem.Enabled = true;
                        endRecordingAndUploadToYouTubeToolStripMenuItem.Enabled = true;
                        imageToolStripMenuItem.Enabled = false;
                        break;
                }
            }
        }
        public Size RecordingSize 
        {
            get
            {
                return this.drawPNL.ClientSize;
            }
            set
            {
                SizeToPanel(value.Width, value.Height);
            }
        }
        public string RecordingPath 
        {
            get
            {
                return _recordingPath;
            }
            set
            {
                _recordingPath = value;
            }
        }
        public string ScreenshotPath
        {
            get
            {
                return _screenshotPath;
            }
            set
            {
                _screenshotPath = value;
            }
        }

        private void x240ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartRecording(320, 240);
        }

        private void x480ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            StartRecording(640, 480);
        }

        private void SizeToPanel(int width, int height)
        {
            int totalWidth = this.Width + (width - drawPNL.Bounds.Width);
            int totalHeight = this.Height + (height - drawPNL.Bounds.Height);
            //this.Width = totalWidth;
            //this.Height = totalHeight;
            this.MinimumSize = new Size(totalWidth, totalHeight);
            this.MaximumSize = new Size(totalWidth, totalHeight);
            this.Show(this.DockPanel, new Rectangle(0, 0, totalWidth, totalHeight));
        }

        private void StartRecording(int width, int height)
        {
            saveFD.Filter = "Avi Videos|*.avi";
            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                SizeToPanel(width, height);
                this.AllowEndUserDocking = false;
                _recordingPath = saveFD.FileName;
                this.Record = RecorderState.Recording;
            }
        }

        private void pauseRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Record = RecorderState.Paused;
        }

        private void endRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Record = RecorderState.Ready;
        }

        private void resumeRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AllowEndUserDocking = true;
            this.Record = RecorderState.Recording;
        }

        private void ViewForm_Resize(object sender, EventArgs e)
        {
            
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFD.Filter = "Jpg Images|*.jpg";
            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                _screenshotPath = saveFD.FileName;
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = true;
            selectedBoatOnlyToolStripMenuItem.Checked = false;
            allBoatsInViewToolStripMenuItem.Checked = false;
            allToolStripMenuItem.Checked = false;
            CameraMan.PhotoMode = CameraMan.PhotoDisplayMode.Disabled;
        }

        private void selectedBoatOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            selectedBoatOnlyToolStripMenuItem.Checked = true;
            allBoatsInViewToolStripMenuItem.Checked = false;
            allToolStripMenuItem.Checked = false;
            CameraMan.PhotoMode = CameraMan.PhotoDisplayMode.SelectedBoat;
        }

        private void allBoatsInViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            selectedBoatOnlyToolStripMenuItem.Checked = false;
            allBoatsInViewToolStripMenuItem.Checked = true;
            allToolStripMenuItem.Checked = false;
            CameraMan.PhotoMode = CameraMan.PhotoDisplayMode.BoatsInView;
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            selectedBoatOnlyToolStripMenuItem.Checked = false;
            allBoatsInViewToolStripMenuItem.Checked = false;
            allToolStripMenuItem.Checked = true;
            CameraMan.PhotoMode = CameraMan.PhotoDisplayMode.All;
        }

        private void size25ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            size25ToolStripMenuItem.Checked = true;
            size50ToolStripMenuItem.Checked = false;
            CameraMan.PhotoViewportRatio = 0.25;
        }

        private void size50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            size25ToolStripMenuItem.Checked = false;
            size50ToolStripMenuItem.Checked = true;
            CameraMan.PhotoViewportRatio = 0.50;
        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void endRecordingAndUploadToYouTubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = _recordingPath;
            this.Record = RecorderState.Ready;
            VideoUploadDialog vud = new VideoUploadDialog(path,_replay.Race.Name,_replay.Race.Lake.Name,_replay.Race.LocalStart,_replay.Race.LocalEnd);
            vud.Show();
        }

        private void x720ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartRecording(1280, 720);
        }

        private void drawPNL_Resize(object sender, EventArgs e)
        {

        }
    }
}
