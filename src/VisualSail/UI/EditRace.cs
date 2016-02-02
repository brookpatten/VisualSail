using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using AmphibianSoftware.VisualSail.Data;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class EditRace : Form
    {
        private Race _race;
        private DateTime? _gpsDataStart;
        private DateTime? _gpsDataEnd;
        private List<Boat> _boats=null;
        private bool _enableEvents = true;
        
        public EditRace(Race race)
        {
            _race = race;
            InitializeComponent();
            LoadBoats();
            this.LoadCourses();
            LoadRace();
        }
        private void LoadRace()
        {
            _enableEvents = false;
            startDP.Value = _race.LocalStart;
            endDP.Value = _race.LocalEnd;
            startSequenceDTP.Value = new DateTime(2001, 1, 1, 0, _race.StartSequence.Minutes, _race.StartSequence.Seconds);
            raceNameTB.Text = _race.Name;
            _enableEvents = true;
        }
        private void LoadBoats()
        {
            _enableEvents = false;
            int? selectedIndex = null;
            if (boatsLV.SelectedIndices.Count > 0)
            {
                selectedIndex = boatsLV.SelectedIndices[0];
            }
            boatsLV.Items.Clear();
            //boatsCLB.Items.Clear();
            if (_boats == null)
            {
                _boats = Boat.FindAll();
                foreach (Boat b in _boats)
                {
                    b.RefreshGpsBounds();
                }
            }
            List<Boat> selectedBoats = _race.Boats;

            DateTime? oldGpsDataStart = _gpsDataStart;
            DateTime? oldGpsDataEnd = _gpsDataEnd;

            _gpsDataStart = null;
            _gpsDataEnd = null;
            foreach (Boat b in _boats)
            {
                bool selected=false;
                for(int i=0;i<_race.Boats.Count;i++)
                {
                    if(_race.Boats[i].Id ==b.Id)
                    {
                        selected=true;
                        _race.Boats[i] = b;
                    }
                }
                
                //boatsCLB.Items.Add(b, selected);

                DateTime? start = b.GpsDataStart;
                DateTime? end = b.GpsDataEnd;

                if (_gpsDataStart == null || start < _gpsDataStart)
                {
                    _gpsDataStart = start;
                }
                if (_gpsDataEnd == null || end > _gpsDataEnd)
                {
                    _gpsDataEnd = end;
                }

                string startString = "";
                string endString = "";
                if (start != null)
                {
                    if (_race.Lake != null)
                    {
                        start = TimeZoneInfo.ConvertTimeFromUtc((DateTime)start, _race.Lake.TimeZone);
                    }
                    startString = ((DateTime)start).ToShortDateString() + " " + ((DateTime)start).ToShortTimeString();
                }
                if (end != null)
                {
                    if (_race.Lake != null)
                    {
                        end = TimeZoneInfo.ConvertTimeFromUtc((DateTime)end, _race.Lake.TimeZone);
                    }
                    endString = ((DateTime)end).ToShortDateString() + " " + ((DateTime)end).ToShortTimeString();
                }

                string[] sub = {b.Name,b.Number,"",/*b.BoatType.Name,*/startString,endString,b.Id.ToString() };
                ListViewItem lvi = new ListViewItem(sub);
                lvi.Checked = selected;
                lvi.UseItemStyleForSubItems = false;
                lvi.SubItems[2].BackColor = Color.FromArgb(b.Color);
                boatsLV.Items.Add(lvi);
            }
            UIHelper.AutoResizeListViewColumns(boatsLV);

            if (_gpsDataStart != null && _gpsDataEnd != null)
            {
                if (_race.AreDatesDefault)
                {
                    _race.UtcStart = (DateTime)_gpsDataStart+_race.StartSequence;
                    _race.UtcEnd = (DateTime)_gpsDataEnd;
                    LoadRace();//loadrace also re-enables events
                }
                else
                {
                    if (oldGpsDataStart!=null && _race.UtcStart-_race.StartSequence == (DateTime)oldGpsDataStart)
                    {
                        _race.UtcStart = (DateTime)_gpsDataStart+_race.StartSequence;
                    }
                    if (oldGpsDataEnd!=null && _race.UtcEnd == (DateTime)oldGpsDataEnd)
                    {
                        _race.UtcEnd = (DateTime)_gpsDataEnd;
                    }
                    LoadRace();//loadrace also re-enables events
                }
            }
            else
            {
                _race.SetDatesToDefault();
            }
            _enableEvents = true;
            if (selectedIndex != null)
            {
                boatsLV.Items[selectedIndex.Value].Selected = true;
            }
        }
        private void LoadCourses()
        {
            if (_race.Lake != null)
            {
                courseCB.Items.Clear();
                List<Course> courses = Course.FindAllByLake(_race.Lake);
                foreach (Course c in courses)
                {
                    courseCB.Items.Add(c);
                }
            }
            if (_race.Course != null)
            {
                foreach (object o in courseCB.Items)
                {
                    if (((Course)o).Id == _race.Course.Id)
                    {
                        courseCB.SelectedItem = o;
                    }
                }
            }
            else if (courseCB.Items.Count > 0)
            {
                courseCB.SelectedIndex = 0;
            }
        }
        private void okBTN_Click(object sender, EventArgs e)
        {
            if (lakeResizer.Lake != null)
            {
                _race.Lake = lakeResizer.Lake;
                _race.Lake.Save();
            }
            SaveBoats();

            if (GpsDataEntered)
            {
                //lake first, we need the timezone to do times
                if (_race.Lake == null)
                {
                    _race.Lake=AutoGenerateLake();
                }
                //race times second, we need the times to set the right date on the course
                if (_race.AreDatesDefault && _gpsDataStart!=null && _gpsDataEnd!=null)
                {
                    _race.UtcStart = (DateTime)_gpsDataStart;
                    _race.UtcEnd = (DateTime)_gpsDataEnd;
                }
                //course last
                if (_race.Course == null)
                {
                    _race.Course = AutoGenerateCourse();
                }
                _race.Save();
                if (sender == okBTN)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.DialogResult = DialogResult.Yes;
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("You must add at least one boat with gps data");
            }
        }
        private void raceNameTB_TextChanged(object sender, EventArgs e)
        {
            if (_enableEvents)
            {
                _race.Name = raceNameTB.Text;
            }
        }
        private void courseCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (courseCB.SelectedItem != null)
            {
                _race.Course = (Course)courseCB.SelectedItem;
            }
        }
        private void startDP_ValueChanged(object sender, EventArgs e)
        {
            if (_enableEvents)
            {
                ValidateRaceTimes();
                _race.LocalStart = startDP.Value;
            }
        }
        private void endDP_ValueChanged(object sender, EventArgs e)
        {
            if (_enableEvents)
            {
                ValidateRaceTimes();
                _race.LocalEnd = endDP.Value;
            }
        }
        public Race Race
        {
            get
            {
                return _race;
            }
        }
        private void SaveBoats()
        {
            List<Boat> boats = new List<Boat>();
            foreach (ListViewItem lvi in boatsLV.CheckedItems)
            {
                int id = int.Parse(lvi.SubItems[5].Text);
                Boat b = Boat.FindById(id);
                boats.Add(b);
            }
            //foreach (object o in boatsCLB.CheckedItems)
            //{
                //boats.Add((Boat)o);
            //}
            _race.Boats = boats;
        }
        private void SyncGpsTimes()
        {
            _race.Save();

            if (_race.Boats.Count > 1)
            {
                DateTime dataStart = _race.Boats[0].GetSensorReadings()[0].datetime;
                long dataStartTicks = dataStart.Ticks;
                for (int i = 1; i < _race.Boats.Count; i++)
                {
                    SkipperDataSet.SensorReadingsDataTable dt = _race.Boats[i].GetSensorReadings();
                    long difference = dt[0].datetime.Ticks - dataStartTicks;

                    var query = from r in Persistance.Data.BoatFile.AsEnumerable()
                                join f in Persistance.Data.SensorReadings.AsEnumerable() on r.sensorfile_id equals f.sensorfile_id
                                where
                                    r.boat_id == _race.Boats[i].Id
                                //&& (/*start==null ||*/ f.datetime >=start)
                                //&& (/*end == null ||*/ f.datetime <= end)
                                orderby f.datetime ascending
                                select f;
                    foreach (SkipperDataSet.SensorReadingsRow r in query)
                    {
                        r.BeginEdit();
                        r.datetime = r.datetime.Subtract(new TimeSpan(difference));
                        r.EndEdit();
                    }
                }
            }
            _race.Save();
        }
        private void ValidateRaceTimes()
        {
            if (_gpsDataStart.HasValue && _gpsDataEnd.HasValue)
            {
                DateTime earliestPossibleStart = _gpsDataStart.Value + _race.StartSequence;

                //validate the current race times
                if (_race.UtcStart < earliestPossibleStart)
                {
                    _race.UtcStart = earliestPossibleStart;
                }
                if (_race.UtcStart > _gpsDataEnd.Value)
                {
                    _race.UtcStart = _gpsDataEnd.Value;
                }

                if (_race.UtcEnd < earliestPossibleStart)
                {
                    _race.UtcEnd = earliestPossibleStart;
                }
                if (_race.UtcEnd > _gpsDataEnd.Value)
                {
                    _race.UtcEnd = _gpsDataEnd.Value;
                }

                //validate the selected start time
                TimeZoneInfo tzi = _race.Lake.TimeZone;
                if (startDP.Value < TimeZoneInfo.ConvertTimeFromUtc(earliestPossibleStart, tzi))
                {
                    startDP.Value = TimeZoneInfo.ConvertTimeFromUtc(earliestPossibleStart, tzi);
                }
                if (startDP.Value > TimeZoneInfo.ConvertTimeFromUtc(_gpsDataEnd.Value, tzi))
                {
                    startDP.Value = TimeZoneInfo.ConvertTimeFromUtc(_gpsDataEnd.Value, tzi);
                }

                //validate the selected end time
                if (endDP.Value < TimeZoneInfo.ConvertTimeFromUtc(earliestPossibleStart, tzi))
                {
                    endDP.Value = TimeZoneInfo.ConvertTimeFromUtc(earliestPossibleStart, tzi);
                }
                if (endDP.Value > TimeZoneInfo.ConvertTimeFromUtc(_gpsDataEnd.Value, tzi))
                {
                    endDP.Value = TimeZoneInfo.ConvertTimeFromUtc(_gpsDataEnd.Value, tzi);
                }
            }
        }
        private bool FindGpsBounds(ref CoordinatePoint nw, ref CoordinatePoint se, ref double alt)
        {
            double? minLat=null;// = double.MaxValue;
            double? maxLat = null;// = double.MinValue;
            double? minLong = null;// = double.MaxValue;
            double? maxLong = null;// = double.MinValue;
            //double altitude = 0;
            //int altcount = 0;
            foreach (AmphibianSoftware.VisualSail.Data.Boat b in _boats)
            {
                //bool increment=false;
                if (b.GpsMinimumLatitude!=null && (minLat==null || b.GpsMinimumLatitude < minLat))
                {
                    minLat = b.GpsMinimumLatitude;
                }
                if (b.GpsMaximumLatitude!=null && (maxLat==null || b.GpsMaximumLatitude > maxLat))
                {
                    maxLat = b.GpsMaximumLatitude;
                }
                if (b.GpsMinimumLongitude!=null && (minLong==null || b.GpsMinimumLongitude < minLong))
                {
                    minLong = b.GpsMinimumLongitude;
                }
                if (b.GpsMaximumLongitude!=null && (maxLong==null || b.GpsMaximumLongitude > maxLong))
                {
                    maxLong = b.GpsMaximumLongitude;
                }
                //if (r.altitude != 0)
                //{
                //    if (altitude == 0)
                //    {
                //        altitude = r.altitude;
                //    }
                //    else
                //    {
                //        altitude = ((altitude * altcount) + r.altitude) / (altcount + 1);
                //    }
                //    altcount++;
                //}
                //count++;
            }

            if (minLat!=null&&maxLat!=null&&minLong!=null&&maxLong!=null)
            {
                //alt = altitude;
                nw = new CoordinatePoint(new Coordinate((double)maxLat), new Coordinate((double)minLong), alt);
                se = new CoordinatePoint(new Coordinate((double)minLat), new Coordinate((double)maxLong), alt);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ShowPlayButton
        {
            get
            {
                return playBTN.Visible;
            }
            set
            {
                playBTN.Visible = value;
            }
        }
        private Lake AutoGenerateLake()
        {
            CoordinatePoint nw=null;
            CoordinatePoint se=null;
            double altitude=0;
            if (FindGpsBounds(ref nw, ref se, ref altitude))
            {
                double minLat = se.Latitude.Value;
                double maxLat = nw.Latitude.Value;
                double minLong = nw.Longitude.Value;
                double maxLong = se.Longitude.Value;

                List<Lake> existingLakes = Lake.FindByBoundingBox(minLat, maxLat, minLong, maxLong);
                if (existingLakes.Count > 0)
                {
                    return existingLakes[0];
                }
                else
                {
                    Lake l;
                    if (CheckForExistingLakeImagery(ref minLat, ref maxLat, ref minLong, ref maxLong).Count == 0)
                    {
                        //no existing images, so we just take a guess at the size we want
                        double padding = 0.05;//this is totally arbitrary based on what looked right
                        l = new Lake(Lake.DefaultName, maxLat + padding, minLat - padding, maxLong + padding, minLong - padding, altitude, "", TimeZoneInfo.Local);
                    }
                    else
                    {
                        //images already exist locally, so we leave the numbers alone
                        l = new Lake(Lake.DefaultName, maxLat, minLat, maxLong, minLong, altitude, "", TimeZoneInfo.Local);
                    }
                    l.Save();
                    return l;
                }
            }
            else
            {
                //throw new Exception("Failed to generate lake");
                return null;
            }
        }
        private List<string> CheckForExistingLakeImagery(ref double minLat, ref double maxLat, ref double minLon, ref double maxLon)
        {
            double boundsMinLat = minLat;
            double boundsMaxLat = maxLat;
            double boundsMinLon = minLon;
            double boundsMaxLon = maxLon;



            DirectoryInfo contentDir = new DirectoryInfo(AmphibianSoftware.VisualSail.Library.ContentHelper.DynamicContentPath);
            List<string> paths = new List<string>();
            foreach (FileInfo fi in contentDir.GetFiles("*.jpg"))
            {
                string path = fi.FullName;
                try
                {
                    double fileMinLat;
                    double fileMaxLat;
                    double fileMinLon;
                    double fileMaxLon;
                    DecodeImageFileName(fi.Name, out fileMinLat, out fileMaxLat, out fileMinLon, out fileMaxLon);

                    if (
                            boundsMinLat >= fileMinLat && boundsMinLat <= fileMaxLat &&
                            boundsMaxLat >= fileMinLat && boundsMaxLat <= fileMaxLat &&
                            boundsMinLon >= fileMinLon && boundsMinLon <= fileMaxLon &&
                            boundsMaxLon >= fileMinLon && boundsMaxLon <= fileMaxLon
                       )
                    {
                        minLat = fileMinLat;
                        maxLat = fileMaxLat;
                        minLon = fileMinLon;
                        maxLon = fileMaxLon;
                        paths.Add(path);
                    }
                }
                catch//(Exception e)
                { 
                }
            }
            return paths;
        }
        private void DecodeImageFileName(string filename, out double minLat, out double maxLat, out double minLon, out double maxLon)
        {
            filename = filename.ToLower();
            string boundString = filename.Replace(".jpg", "");
            char[] splitter = { '_' };
            string[] parts = boundString.Split(splitter);
            System.Globalization.CultureInfo numberCulture = System.Globalization.CultureInfo.GetCultureInfo("en-us");
            minLat = double.Parse(parts[1], numberCulture.NumberFormat);
            maxLat = double.Parse(parts[0], numberCulture.NumberFormat);
            minLon = double.Parse(parts[3], numberCulture.NumberFormat);
            maxLon = double.Parse(parts[2], numberCulture.NumberFormat);
        }
        private Course AutoGenerateCourse()
        {
            Course c = new Course(_race.Name + " Course", _race.LocalStart, _race.Lake);
            c.Save();
            return c;
        }
        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void boatsLV_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_enableEvents)
            {
                SaveBoats();
            }
        }
        private void boatsLV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_enableEvents)
            {
                boatEditBTN.Enabled = boatsLV.SelectedIndices.Count > 0;
                gpsDataBTN.Enabled = boatsLV.SelectedIndices.Count > 0;
            }
        }
        private void gpsDataBTN_Click(object sender, EventArgs e)
        {
            //int id = int.Parse(boatsLV.Items[boatsLV.SelectedIndices[0]].SubItems[5].Text);
            //EditSensorFiles esf = new EditSensorFiles(_boats[boatsLV.SelectedIndices[0]]);
            //esf.ShowDialog();
            ImportFiles imfi = new ImportFiles(_boats[boatsLV.SelectedIndices[0]]);
            imfi.ShowDialog();
            _boats[boatsLV.SelectedIndices[0]].RefreshGpsBounds();
            LoadBoats();
        }
        private void boatEditBTN_Click(object sender, EventArgs e)
        {
            //int id = int.Parse(boatsLV.Items[boatsLV.SelectedIndices[0]].SubItems[5].Text);
            Boat b = _boats[boatsLV.SelectedIndices[0]];//Boat.FindById(id);
            EditBoat eb = new EditBoat(b);
            eb.ShowDialog();
            if (eb.Boat != null)
            {
                b = eb.Boat;
                b.Save();
                LoadBoats();
            }
        }
        private void newBoatBTN_Click_1(object sender, EventArgs e)
        {
            Boat b = new Boat("New Boat", "Sail Number", AmphibianSoftware.VisualSail.Library.ColorHelper.AutoColorPick(_race.Boats.Count).ToArgb(), BoatType.FindAll()[0]);
            EditBoat eb = new EditBoat(b);
            eb.ShowDialog();
            if (eb.DialogResult==DialogResult.OK)
            {
                b = eb.Boat;
                b.Save();
                
                //EditSensorFiles esf = new EditSensorFiles(b);
                //esf.ShowDialog();
                ImportFiles imfi = new ImportFiles(b);
                imfi.ShowDialog();
                b.RefreshGpsBounds();

                _boats.Add(b);
                _race.Boats.Add(b);

                LoadBoats();
            }
        }
        private void UpdateValidGpsDataRange()
        {
            if (_gpsDataStart != null && _gpsDataEnd != null)
            {
                DateTime start = TimeZoneInfo.ConvertTimeFromUtc((DateTime)_gpsDataStart, _race.Lake.TimeZone);
                DateTime end = TimeZoneInfo.ConvertTimeFromUtc((DateTime)_gpsDataEnd, _race.Lake.TimeZone);
                gpsDataLBL.Text = "Valid range is " + start.ToShortTimeString() + " to " + end.ToShortTimeString();
            }
            else
            {
                gpsDataLBL.Text = "";
            }
        }
        private void editTC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_enableEvents)
            {
                if (editTC.SelectedIndex != 0 && !GpsDataEntered)
                {
                    _enableEvents = false;
                    editTC.SelectedIndex = 0;
                    _enableEvents = true;
                    MessageBox.Show("You must import gps data for at least one boat first.");
                }
                else
                {
                    if (_race.Lake == null)
                    {
                        _race.Lake = AutoGenerateLake();
                        LoadBoats();
                        LoadRace();
                    }
                    if (_race.Lake != null)
                    {
                        if (editTC.SelectedIndex == 1)
                        {
                            UpdateValidGpsDataRange();
                            _enableEvents = false;
                            for (int i = 0; i < timezoneCB.Items.Count; i++)
                            {
                                if (_race.Lake.TimeZone.DisplayName == ((TimeZoneInfo)timezoneCB.Items[i]).DisplayName)
                                {
                                    timezoneCB.SelectedIndex = i;
                                    break;
                                }
                            }
                            ValidateRaceTimes();
                            _enableEvents = true;
                        }
                        if (editTC.SelectedIndex == 2)
                        {
                            Lake lake = _race.Lake;
                            lakeResizer.Lake = lake;
                            CoordinatePoint nw = null;
                            CoordinatePoint se = null;
                            double alt = 0;
                            if (FindGpsBounds(ref nw, ref se, ref alt))
                            {
                                lakeResizer.SetGpsBounds(nw, se);
                            }
                            _enableEvents = false;
                            lakeNameTB.Text = lake.Name;
                            lakeAltNUD.Value = (decimal)lake.Altitude;
                            _enableEvents = true;

                            LoadCourses();
                            if (courseCB.Items.Count == 0 && _race.Lake != null)
                            {
                                AutoGenerateCourse();
                                LoadCourses();
                            }
                        }
                    }
                    else
                    {
                        editTC.SelectedIndex = 0;
                        MessageBox.Show("You must import gps data for at least one boat first.");
                    }
                }
            }
        }
        private void existingBTN_Click(object sender, EventArgs e)
        {
            CoordinatePoint nw=null;
            CoordinatePoint se=null;
            double alt=0;
            if (FindGpsBounds(ref nw, ref se, ref alt))
            {
                double minLat = se.Latitude.Value;
                double maxLat = nw.Latitude.Value;
                double minLong = nw.Longitude.Value;
                double maxLong = se.Longitude.Value;
                List<string> paths = CheckForExistingLakeImagery(ref minLat, ref maxLat, ref minLong, ref maxLong);
                if (paths.Count > 0)
                {
                    ExistingImageSelect eis = new ExistingImageSelect(paths);
                    eis.ShowDialog();
                    if (eis.SelectedPath != "")
                    {
                        FileInfo fi = new FileInfo(eis.SelectedPath);
                        string name = fi.Name;
                        DecodeImageFileName(name, out minLat, out maxLat, out minLong, out maxLong);
                        Lake l = _race.Lake;
                        l.South = minLat;
                        l.North = maxLat;
                        l.West = minLong;
                        l.East = maxLong;
                        l.Save();
                        lakeResizer.Lake = l;
                    }
                }
                else
                {
                    MessageBox.Show("No Suitable Cached Images Found");
                }
            }
            else
            {
                MessageBox.Show("Could not determine a region for the gps data, make sure at least one boat had gps data.");
            }
            
        }
        private void lakeNameTB_TextChanged(object sender, EventArgs e)
        {
            _race.Lake.Name = lakeNameTB.Text;
            _race.Lake.Save();
        }
        private void lakeAltNUD_ValueChanged(object sender, EventArgs e)
        {
            _race.Lake.Altitude = (double)lakeAltNUD.Value;
            _race.Lake.Save();
        }
        private void EditRace_Load(object sender, EventArgs e)
        {
            timezoneCB.Items.Clear();
            foreach (TimeZoneInfo tzi in TimeZoneInfo.GetSystemTimeZones())
            {
                timezoneCB.Items.Add(tzi);
            }
        }
        private void timezoneCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lake lake = _race.Lake;
            lake.TimeZone = (TimeZoneInfo)timezoneCB.SelectedItem;
            lake.Save();
            LoadRace();
            LoadBoats();
            UpdateValidGpsDataRange();
        }
        private void editCourseBTN_Click_2(object sender, EventArgs e)
        {
            BusyDialogManager.Show("Loading Course");
            EditCourses ec = new EditCourses(_race);
            BusyDialogManager.Hide();
            ec.ShowDialog(this.Parent);
            LoadCourses();
        }
        private void newCourseBTN_Click(object sender, EventArgs e)
        {
            Course oldCourse = _race.Course;
            Course c = new Course("New Course", _race.UtcStart, _race.Lake);
            c.Save();
            _race.Course = c;
            _race.Save();
            EditCourses ec = new EditCourses(_race);
            ec.ShowDialog();
            if (ec.DialogResult == DialogResult.OK)
            {
                LoadCourses();
            }
            else
            {
                if (oldCourse != null)
                {
                    _race.Course = oldCourse;
                    _race.Save();
                    c.Delete();
                }
            }
        }
        private bool GpsDataEntered
        {
            get
            {
                foreach (Boat b in _boats)
                {
                    if (b.GpsDataStart != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void startSequenceDTP_ValueChanged(object sender, EventArgs e)
        {
            if (_enableEvents)
            {
                _race.StartSequence = new TimeSpan(0, startSequenceDTP.Value.Minute, startSequenceDTP.Value.Second);
                ValidateRaceTimes();
            }
        }
    }
}