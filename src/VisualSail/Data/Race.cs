using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace AmphibianSoftware.VisualSail.Data
{
    public class Race
    {
        private int _id;
        private string _name;
        private int _lakeId;
        private int _courseId;
        private DateTime _replayStart;
        private DateTime _replayEnd;
        private DateTime _start;
        private DateTime _end;
        private List<Boat> _boats;
        private TimeSpan _startSequence;

        private bool _new;
        private bool _changed;

        private static DateTime _defaultStart;
        private static DateTime _defaultEnd;

        public Race()
        {
            SetDefaults();
            _boats = new List<Boat>();
            _startSequence = new TimeSpan(0, 0, 0);
            _start = _defaultStart;
            _end = _defaultEnd;
            SetReplayTimes();
            _new = true;
            _changed = false;
        }
        private Race(SkipperDataSet.RaceRow row)
        {
            _id = row.id;
            _name = row.name;
            _lakeId = row.lake_id;
            _courseId = row.course_id;
            _start = row.start;
            _end = row.end;
            if (row.start_sequence_length != null)
            {
                _startSequence = TimeSpan.Parse(row.start_sequence_length);
            }
            else
            {
                _startSequence = new TimeSpan(0, 0, 0);
            }
            SetReplayTimes();
            _boats = FindBoats(this);

            _new = false;
            _changed = false;
        }
        public Race(string name, Lake lake, Course course, DateTime start, DateTime end)
        {
            _name = name;
            _lakeId = lake.Id;
            _courseId = course.Id;
            _start = start;
            _end = end;
            _startSequence = new TimeSpan(0, 0, 0);
            SetReplayTimes();
            _boats = new List<Boat>();

            _new = true;
            _changed = true;
        }
        private void SetDefaults()
        {
            DateTime today = DateTime.Now;
            today = today - new TimeSpan(0, today.Hour, today.Minute, today.Second, today.Millisecond);
            _defaultStart = today + new TimeSpan(12, 0, 0);
            _defaultEnd = today + new TimeSpan(13, 0, 0);
        }
        public void SetDatesToDefault()
        {
            _start = _defaultStart;
            _end = _defaultEnd;
        }
        public bool AreDatesDefault
        {
            get
            {
                return _start == _defaultStart && _end == _defaultEnd;
            }
        }
        public void Save()
        {
            if (_new && _changed)
            {
                Insert();
                SetBoats(this, _boats);
                _new = false;
                _changed = false;
            }
            else if (!_new && _changed)
            {
                Update();
                SetBoats(this, _boats);
                _changed = false;
            }
        }
        private void Insert()
        {
            Persistance.Data.Race.AddRaceRow(_name, Lake.Row,Course.Row , _start, _end,_startSequence.ToString());
            _id = ((SkipperDataSet.RaceRow)Persistance.Data.Race.Rows[Persistance.Data.Race.Rows.Count - 1]).id;
        }
        private void Update()
        {
            try
            {
                Row.BeginEdit();
                Row.name = _name;
                Row.lake_id = _lakeId;
                Row.course_id = _courseId;
                Row.start = _start;
                Row.end = _end;
                Row.start_sequence_length = _startSequence.ToString();
                Row.EndEdit();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Row.CancelEdit();
            }
        }
        public void Delete()
        {
            Row.Delete();
        }
        private static SkipperDataSet.RaceRow FindRowById(int id)
        {
            return (from r in Persistance.Data.Race.AsEnumerable()
                         where (int)r["id"] == id
                         select r).First();
        }
        public static Race FindById(int id)
        {
            return new Race(FindRowById(id));
        }
        public static List<Race> FindAll()
        {
            var query = from r in Persistance.Data.Race.AsEnumerable()
                        select r;

            List<Race> races = new List<Race>();
            foreach (SkipperDataSet.RaceRow rr in query)
            {
                races.Add(new Race(rr));
            }
            return races;
        }
        public SkipperDataSet.RaceRow Row
        {
            get
            {
                return FindRowById(_id);
            }
        }
        public int Id
        {
            get
            {
                return _id;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                _changed = true;
            }
        }
        public void SetReplayTimes()
        {
            _replayStart = UtcCountdownStart;
            _replayEnd = UtcEnd;
        }
        public TimeSpan StartSequence
        {
            get
            {
                return _startSequence;
            }
            set
            {
                _startSequence = value;
                _changed = true;
            }
        }
        public DateTime UtcReplayStart
        {
            get
            {
                return _replayStart;
            }
            set
            {
                _replayStart = value;
            }
        }
        public DateTime UtcReplayEnd
        {
            get
            {
                return _replayEnd;
            }
            set
            {
                _replayEnd = value;
            }
        }
        public DateTime UtcCountdownStart
        {
            get
            {
                return UtcStart - StartSequence;
            }
        }
        public DateTime UtcStart
        {
            get
            {
                return _start;
            }
            set
            {
                _start=value;
                _changed = true;
            }
        }
        public DateTime UtcEnd
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                _changed = true;
            }
        }
        public DateTime LocalCountdownStart
        {
            get
            {
                return LocalStart - StartSequence;
            }
        }
        public DateTime LocalStart
        {
            get
            {
                TimeZoneInfo tzi;
                if (Lake != null)
                {
                    tzi = Lake.TimeZone;
                }
                else
                {
                    tzi = TimeZoneInfo.Local;
                }
                _start = new DateTime(_start.Ticks, DateTimeKind.Unspecified);
                return TimeZoneInfo.ConvertTimeFromUtc(_start,tzi);
            }
            set
            {
                TimeZoneInfo tzi;
                if (Lake != null)
                {
                    tzi = Lake.TimeZone;
                }
                else
                {
                    tzi = TimeZoneInfo.Local;
                }
                _start = TimeZoneInfo.ConvertTimeToUtc(value, tzi);
                _changed = true;
            }
        }
        public DateTime LocalEnd
        {
            get
            {
                TimeZoneInfo tzi;
                if (Lake != null)
                {
                    tzi = Lake.TimeZone;
                }
                else
                {
                    tzi = TimeZoneInfo.Local;
                }
                _end = new DateTime(_end.Ticks, DateTimeKind.Unspecified);
                return TimeZoneInfo.ConvertTimeFromUtc(_end, tzi);
            }
            set
            {
                TimeZoneInfo tzi;
                if (Lake != null)
                {
                    tzi = Lake.TimeZone;
                }
                else
                {
                    tzi = TimeZoneInfo.Local;
                }
                _end = TimeZoneInfo.ConvertTimeToUtc(value, tzi);
                _changed = true;
            }
        }
        public Lake Lake
        {
            get
            {
                if (_lakeId != 0)
                {
                    return Lake.FindById(_lakeId);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    _lakeId = value.Id;
                }
                else
                {
                    _lakeId = 0;
                }
                _changed = true;
            }
        }
        public Course Course
        {
            get
            {
                if (_courseId != 0)
                {
                    return Course.FindById(_courseId);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _courseId = value.Id;
                _changed = true;
            }
        }
        public override string ToString()
        {
            return _name;
        }
        public List<Boat> Boats
        {
            get
            {
                return _boats;
            }
            set
            {
                _boats = value;
                _changed = true;
            }
        }
        private static List<Boat> FindBoats(Race race)
        {
                List<Boat> boats = new List<Boat>();
                var query = from r in Persistance.Data.RaceBoat.AsEnumerable()
                            where (int)r["race_id"] == race.Id
                            select r;
                foreach (SkipperDataSet.RaceBoatRow r in query)
                {
                    boats.Add(Boat.FindById(r.boat_id));
                }
                return boats;
        }
        private static void SetBoats(Race race,List<Boat> boats)
        {
            List<SkipperDataSet.RaceBoatRow> raceBoats = new List<SkipperDataSet.RaceBoatRow>();
            var query = from r in Persistance.Data.RaceBoat.AsEnumerable()
                        where (int)r["race_id"] == race.Id
                        select r;
            foreach (SkipperDataSet.RaceBoatRow r in query)
            {
                raceBoats.Add(r);
            }
            foreach (SkipperDataSet.RaceBoatRow r in raceBoats)
            {
                r.Delete();
            }

            foreach (Boat r in boats)
            {
                Persistance.Data.RaceBoat.AddRaceBoatRow(race.Row, r.Row);
            }
        }
        public static string DefaultName
        {
            get
            {
                return "New Race";
            }
        }
    }
}
