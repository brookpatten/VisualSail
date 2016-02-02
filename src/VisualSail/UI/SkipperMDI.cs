using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.Threading;

using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Data;

using WeifenLuo.WinFormsUI.Docking;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class SkipperMDI : Form
    {
        private string _version;
        private string _aboutLicense;
        private string _loadedFile=string.Empty;
        StatisticsForm _statisticsForm;
        TimeForm _timeForm;
        BookMarksForm _bookmarkForm;
        Replay _replay;
        GettingStarted _gettingStartedForm;

        
        List<string> _gpsDataFileParameters = null;

        public SkipperMDI(string version,string aboutLicense)
        {
            _version = version;
            _aboutLicense = aboutLicense;
            Splash s = new Splash(_version, _aboutLicense);
            s.ShowDialog();
            InitializeComponent();
            mainDP.ActiveAutoHideContent = null;
            mainDP.Name = "mainDP";
            mainDP.BringToFront();
            BusyDialogManager.SetParent(this);
            BusyDialogManager.Hide();
            this.Text = "VisualSail " + _version;
            this.Show();
            saveFD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            ShowGettingStarted();
        }

        public SkipperMDI(string path,string version,string aboutLicense)
        {
            _version = version;
            _aboutLicense = aboutLicense;
            Splash s = new Splash(_version, _aboutLicense);
            s.ShowDialog();
            InitializeComponent();
            mainDP.ActiveAutoHideContent = null;
            mainDP.Name = "mainDP";
            mainDP.BringToFront();
            BusyDialogManager.SetParent(this);
            BusyDialogManager.Hide();
            this.Text = "VisualSail " + _version;
            this.Show();
            saveFD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            
            if (path.EndsWith(".sail"))
            {
                _loadedFile = path;
                TryLoadFile();
            }
            else
            {
                //setting this value invokes special code in LoadFile() which 
                //will try to import it
                List<string> files = new List<string>();
                files.Add(path);
                NewFromGpsFiles(files);
            }
        }

        private void ShowGettingStarted()
        {
            _gettingStartedForm = new GettingStarted();
            _gettingStartedForm.OpenClick += this.openToolStripMenuItem_Click;
            _gettingStartedForm.NewClick += this.newToolStripMenuItem_Click;
            _gettingStartedForm.GpsClick += this.newSeriesFromGPSFilesToolStripMenuItem_Click;
            _gettingStartedForm.DockPanel = this.mainDP;
            _gettingStartedForm.Show(mainDP, DockState.Document);
            //start.Show(mainDP, UIHelper.FindCenteredPosition(mainDP, start));
        }
        private void HideGettingStarted()
        {
            if (_gettingStartedForm!=null && _gettingStartedForm.Visible)
            {
                _gettingStartedForm.Hide();
            }
        }
        
        private void UpdateStatistics()
        {
            if (_statisticsForm!=null && _statisticsForm.Visible)
            {
                _statisticsForm.UpdateStatistics(false);
            }
        }
        private void simulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void UpdateTime()
        {
            _timeForm.UpdateTime();
        }

        private IViewPort OpenNewViewport()
        {
            ViewForm viewForm = new ViewForm(_replay);
            //viewForm.MdiParent = this;
            //viewForm.Bounds = new Rectangle(0, 0, 584, 354);
            viewForm.Show(mainDP,UIHelper.FindCenteredPosition(mainDP,viewForm));
            //viewForm.DockState = DockState.Document;
            _replay.AddViewPort(viewForm);
            return viewForm;
        }

        private void SkipperMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeToolStripMenuItem_Click(sender, e);
            SatelliteImageryHelper.Shutdown();
        }

        public Replay Replay
        {
            get
            {
                return _replay;
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewViewport();
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _statisticsForm.Show();
            _statisticsForm.BringToFront();
        }

        private void timeControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _timeForm.Show();
            _timeForm.BringToFront();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New();
        }

        public void New()
        {
            if (saveFD.ShowDialog()==DialogResult.OK)
            {
                HideGettingStarted();
                _loadedFile = saveFD.FileName;
                Persistance.CreateNew();
                Persistance.SaveToFile(_loadedFile);
                ConfigureMenu(true);
                LoadFile();
            }
        }

        public void NewFromGpsFiles(List<string> files)
        {
            HideGettingStarted();
            _gpsDataFileParameters = files;
            Persistance.CreateNew();
            ConfigureMenu(true);
            LoadFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_loadedFile==string.Empty || (_loadedFile!=string.Empty && MessageBox.Show("If you open a new series your current series will be closed.  Are you sure this is what you want to do?", "File Closing", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                Open();
            }
        }

        public void Open()
        {
            if (openFD.ShowDialog()==DialogResult.OK)
            {
                if (_loadedFile != string.Empty)
                {
                    closeToolStripMenuItem_Click(null, null);
                }
                _loadedFile = openFD.FileName;
                TryLoadFile();
            }
        }

        private void TryLoadFile()
        {
            try
            {
                BusyDialogManager.Show("Loading " + BusyDialog.CleanPath(_loadedFile));
                Persistance.LoadFromFile(_loadedFile);
                LoadFile();
                ConfigureMenu(true);
                HideGettingStarted();
            }
            catch (Exception ex)
            {
                ConfigureMenu(false);
                _loadedFile = string.Empty;
                MessageBox.Show("A problem occured loading the file."+Environment.NewLine + ex.Message);
                ShowGettingStarted();
            }
            finally
            {
                BusyDialogManager.Hide();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_loadedFile == string.Empty)
            {
                if (saveFD.ShowDialog()==DialogResult.OK)
                {
                    BusyDialogManager.Show("Saving");
                    _loadedFile = saveFD.FileName;
                    Persistance.SaveToFile(_loadedFile);
                    BusyDialogManager.Hide();
                }
            }
            else
            {
                BusyDialogManager.Show("Saving");
                Persistance.SaveToFile(_loadedFile);
                BusyDialogManager.Hide();
            }
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFD.ShowDialog()==DialogResult.OK)
            {
                BusyDialogManager.Show("Saving");
                _loadedFile = saveFD.FileName;
                Persistance.SaveToFile(_loadedFile);
                BusyDialogManager.Hide();
            }
        }

        private void AutoImportGpsDataFileParameters(Race race)
        {
            int number = 0;
            foreach (string gpsDataFile in _gpsDataFileParameters)
            {
                try
                {
                    AmphibianSoftware.VisualSail.Data.Boat b = new AmphibianSoftware.VisualSail.Data.Boat();
                    b.BoatType = BoatType.FindAll()[0];
                    System.IO.FileInfo file = new System.IO.FileInfo(gpsDataFile);
                    if (file.Name.Contains("."))
                    {
                        b.Name = file.Name.Substring(0, file.Name.LastIndexOf("."));
                    }
                    else
                    {
                        b.Name = file.Name;
                    }
                    b.Color = ColorHelper.AutoColorPick(number).ToArgb();
                    number++;
                    b.Number = number.ToString();
                    b.Save();
                    if (System.IO.File.Exists(gpsDataFile))
                    {
                        AmphibianSoftware.VisualSail.Data.Import.FileImporter fi = AmphibianSoftware.VisualSail.Data.Import.FileImporter.DetectFileType(gpsDataFile);
                        //BusyDialogManager.Show("Importing Data");
                        SensorFile sf = fi.ImportFile(gpsDataFile, b);
                        sf.Save();
                        //BusyDialogManager.HideAll();
                    }
                    race.Boats.Add(b);
                }
                catch (Exception e)
                {
                    MessageBox.Show("A problem occured while loading " + gpsDataFile+"."+Environment.NewLine+e.Message);
                }
            }
            _gpsDataFileParameters.Clear();
            _gpsDataFileParameters = null;
        }
        

        private void LoadFile()
        {
            //_busy.Show();
            SelectRace sr = new SelectRace();
            sr.Owner = this;
            //BusyDialogManager.HideAll();
            DialogResult editResult = DialogResult.Cancel;
            DialogResult selectResult=sr.ShowDialog(this);
            Race race = sr.SelectedRace;

            //kindof a hack, but it gets file one click file loading working
            if (race.Boats.Count == 0 && _gpsDataFileParameters != null)
            {
                AutoImportGpsDataFileParameters(race);
            }

            while (selectResult != DialogResult.Yes && editResult != DialogResult.Yes && selectResult != DialogResult.Cancel)
            {
                if (selectResult == DialogResult.OK)
                {
                    BusyDialogManager.Show("Loading Race");
                    EditRace er = new EditRace(race);
                    er.Owner = this;
                    BusyDialogManager.Hide();
                    editResult = er.ShowDialog(this);
                }
                if (editResult == DialogResult.OK || (editResult == DialogResult.Cancel && Persistance.Data.Race.Count > 0))//go back to the select dialog
                {
                    selectResult = sr.ShowDialog(this);
                    race = sr.SelectedRace;
                }
                else if (editResult == DialogResult.Cancel && Persistance.Data.Race.Count == 0)
                {
                    //if there's no other races just cancel everything
                    selectResult = DialogResult.Cancel;
                    break;
                }
            }

            if (selectResult != DialogResult.Cancel)
            {
                BusyDialogManager.Show("Starting Race");

#if RENDERER_AUTO || (!RENDERER_GDI && !RENDERER_XNA && !RENDERER_NULL && !RENDERER_AUTO)
                try
                {
                    _replay = new Replay(race, new XnaRenderer(), new Notify(this.UpdateStatistics), new Notify(this.UpdateTime));
                }
                catch (Exception e)
                {
                    MessageBox.Show("VisualSail encountered an exception intializing the 3D renderer. "+e.Message+Environment.NewLine+"Switching to 2D Renderer");
                    _replay = new Replay(race, new GdiRenderer(), new Notify(this.UpdateStatistics), new Notify(this.UpdateTime));
                }
#endif
#if RENDERER_GDI
                _replay = new Replay(race, new GdiRenderer(), new Notify(this.UpdateStatistics), new Notify(this.UpdateTime));
#endif
#if RENDERER_XNA
                _replay = new Replay(race, new XnaRenderer(), new Notify(this.UpdateStatistics), new Notify(this.UpdateTime));
#endif
#if RENDERER_NULL
                _replay = new Replay(race, new NullRenderer(), new Notify(this.UpdateStatistics), new Notify(this.UpdateTime));
#endif

                _statisticsForm = new StatisticsForm(_replay);
                _timeForm = new TimeForm(_replay);
                _bookmarkForm = new BookMarksForm(_replay);
                _replay.Start();

                _statisticsForm.Show(mainDP, DockState.DockBottom);
                double prop = (double)_timeForm.Width / (double)_statisticsForm.Width;
                _bookmarkForm.Show(_statisticsForm.Pane, DockAlignment.Left, prop);
                _timeForm.Show(_bookmarkForm.PanelPane,_bookmarkForm);
                ViewForm vf = (ViewForm)OpenNewViewport();
                vf.DockState = DockState.Document;
                _statisticsForm.CreateDefaultGraphs();

                BusyDialogManager.Hide();
            }
            else
            {
                ShowGettingStarted();
            }
        }

        
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_replay != null)
            {
                _replay.Stop();
                _replay.Dispose();
            }
            CloseAllChildren();
            ConfigureMenu(false);
            Persistance.UnloadFile();
            _loadedFile = string.Empty;
            ShowGettingStarted();
        }

        private void racesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _replay.Stop();
            DateTime currentTime = _replay.SimulationTime;
            double currentSpeed=_replay.Speed;
            if (currentSpeed == 0.0)
            {
                currentSpeed = 1.0;
            }
            EditRace er = new EditRace(_replay.Race);
            er.ShowPlayButton = false;
            er.Owner = this;
            er.ShowDialog();
            if (er.DialogResult == DialogResult.OK)
            {
                BusyDialogManager.Show("Restarting Race");
                _replay.Reset();
                _statisticsForm.Reset();
                foreach (DockContent dc in mainDP.Contents)
                {
                    if (dc is GraphForm)
                    {
                        ((GraphForm)dc).Reset();
                    }
                }

                _replay.Start();
                if (currentTime >= _replay.Race.UtcCountdownStart && currentTime <= _replay.Race.UtcEnd)
                {
                    _replay.TargetTime = currentTime;
                }
                else if (currentTime < _replay.Race.UtcCountdownStart)
                {
                    _replay.TargetTime = _replay.Race.UtcCountdownStart;
                }
                else if (currentTime > _replay.Race.UtcEnd)
                {
                    _replay.TargetTime = _replay.Race.UtcEnd;
                }
                _replay.Speed = currentSpeed;
                _replay.Play();
                _replay.RefreshViewports();
                BusyDialogManager.Hide();
            }
            else
            {
                _replay.Start();
                _replay.TargetTime = currentTime;
                _replay.Speed = currentSpeed;
                _replay.Play();
            }
        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //engine.Pause();
            //EditCourses ec = new EditCourses();
            //ec.ShowDialog();
        }

        private void ConfigureMenu(bool fileLoaded)
        {
            saveToolStripMenuItem.Enabled = fileLoaded;
            saveAsToolStripMenuItem.Enabled = fileLoaded;
            racesToolStripMenuItem.Enabled = fileLoaded;
            viewToolStripMenuItem.Enabled = fileLoaded;
            statisticsToolStripMenuItem.Enabled = fileLoaded;
            timeControlToolStripMenuItem.Enabled = fileLoaded;
            resultsToolStripMenuItem.Enabled = fileLoaded;
            seriesToolStripMenuItem.Enabled = fileLoaded;
            closeToolStripMenuItem.Enabled = fileLoaded;
            arrangeWindowsToolStripMenuItem.Enabled = fileLoaded;
            selectRaceToolStripMenuItem.Enabled = fileLoaded;
            bookmarksToolStripMenuItem.Enabled = fileLoaded;
        }

        private void seriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_replay != null)
            {
                _replay.Stop();
                _replay.Dispose();
            }
            CloseAllChildren();
            LoadFile();
        }

        private void CloseAllChildren()
        {
            if (_timeForm != null)
            {
                _timeForm.Close();
            }
            if (_statisticsForm != null)
            {
                _statisticsForm.Close();
            }
            if (_bookmarkForm != null)
            {
                _bookmarkForm.Close();
            }
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }
            for (int i = 0; i < mainDP.Contents.Count; i++)
            {
                if (mainDP.Contents[i] is GraphForm)
                {
                    ((GraphForm)mainDP.Contents[i]).Shutdown();
                    ((GraphForm)mainDP.Contents[i]).Close();
                    i--;
                }
                //if (mainDP.Contents[i] is StatisticsForm)
                //{
                //    ((StatisticsForm)mainDP.Contents[i]).Close();
                //    i--;
                //}
                //if (mainDP.Contents[i] is TimeForm)
                //{
                //    ((TimeForm)mainDP.Contents[i]).Close();
                //    i--;
                //}
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StringBuilder aboutText = new StringBuilder();
            aboutText.Append("VisualSail ");
            aboutText.Append(_version);
            aboutText.Append(Environment.NewLine);
            aboutText.Append("Copyright 2008-");
            aboutText.Append(DateTime.Now.Year);
            aboutText.Append(" Amphibian Software");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("All Rights Reserved");
            aboutText.Append(Environment.NewLine);
            aboutText.Append(Environment.NewLine);

            aboutText.Append("Graphing Capability Created With...");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("ZedGraph 5.1.5.28844");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("Copyright 2003-2007 John Champion");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("Licensed under the GNU Lesser Public General License");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("(See ZedGraph.License.txt for the complete LGPL)");
            aboutText.Append(Environment.NewLine);
            aboutText.Append(Environment.NewLine);

            aboutText.Append("YouTube Upload Capability Created With...");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("Google-GData 1.3.1.0");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("Copyright 2006 Google Inc.");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("Licensed under the Apache License 2.0");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("(See Google.GData.License.txt for the complete Apache License 2.0)");
            aboutText.Append(Environment.NewLine);
            aboutText.Append(Environment.NewLine);

            aboutText.Append("Icons Provided By...");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("FamFamFam Silk Icons 1.3");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("famfamfam.com/lab/icons/silk");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("Copyright Mark James");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("Licensed under the Creative Commons 2.5 Generic License");
            aboutText.Append(Environment.NewLine);
            aboutText.Append("(See creativecommons.org/licenses/by/2.5 for the complete license)");
            aboutText.Append(Environment.NewLine);
            aboutText.Append(Environment.NewLine);

            MessageBox.Show(this, aboutText.ToString(), "About");
        }

        private void gettingStartedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowGettingStarted();
        }

        private void resultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        public AmphibianSoftware.VisualSail.Data.Statistics.StatisticUnitType SelectedUnitType
        {
            get
            {
                if (_statisticsForm != null)
                {
                    return _statisticsForm.SelectedUnitType;
                }
                else
                {
                    return AmphibianSoftware.VisualSail.Data.Statistics.StatisticUnitType.standard;
                }
            }
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _bookmarkForm.Show();
            _bookmarkForm.BringToFront();
        }

        private void newSeriesFromGPSFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gpsOpenFD.ShowDialog() == DialogResult.OK)
            {
                List<string> paths = new List<string>(gpsOpenFD.FileNames);
                NewFromGpsFiles(paths);
            }
        }
    }
}
