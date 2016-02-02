using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public class TimeLineStatisticCollection
    {
        public AmphibianSoftware.VisualSail.Library.ReplayBoat ReplayBoat;
        public int? legIndex;
        public int? tackIndex;

        private Dictionary<Type, object> _stats;
        private Dictionary<Type, List<string>> _statDirectory;
        public TimeLineStatisticCollection()
        {
            _stats = new Dictionary<Type, object>();
            _statDirectory = new Dictionary<Type, List<string>>();
        }
        public void AddStatistic<T>(Statistic<T> f)
        {
            if (!_statDirectory.ContainsKey(typeof(T)))
            {
                _statDirectory.Add(typeof(T), new List<string>());
            }
            _statDirectory[typeof(T)].Add(f.Name);

            if (!_stats.ContainsKey(typeof(T)))
            {
                _stats.Add(typeof(T), new Dictionary<string, TimeLineStatistic<T>>());
            }
            TimeLineStatistic<T> tls = new TimeLineStatistic<T>(f);
            tls.Collection = this;
            ((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)]).Add(f.Name, tls);
        }
        public Statistic<T> GetStatistic<T>(string name)
        {
            if (_stats.ContainsKey(typeof(T)))
            {
                if (((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)]).ContainsKey(name))
                {
                    return ((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)])[name].Statistic;
                }
                else
                {
                    throw new Exception("Specified statistic name does not exist");
                }
            }
            else
            {
                throw new Exception("Specified statistic type does not exist");
            }
        }
        public void AddStatisticListener(TimeLineStatisticNewValueAdded callback, string name)
        {
            bool found = false;
            foreach (Type type in _statDirectory.Keys)
            {
                if (type == typeof(int) && ((Dictionary<string, TimeLineStatistic<int>>)_stats[type]).ContainsKey(name))
                {
                    ((Dictionary<string, TimeLineStatistic<int>>)_stats[type])[name].NewValueAdded += callback;
                    found = true;
                    break;
                }
                else if (type == typeof(float) && ((Dictionary<string, TimeLineStatistic<float>>)_stats[type]).ContainsKey(name))
                {
                    ((Dictionary<string, TimeLineStatistic<float>>)_stats[type])[name].NewValueAdded += callback;
                    found = true;
                    break;
                }
                else if (type == typeof(DateTime) && ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type]).ContainsKey(name))
                {
                    ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type])[name].NewValueAdded += callback;
                    found = true;
                    break;
                }
                else if (type == typeof(TimeSpan) && ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type]).ContainsKey(name))
                {
                    ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type])[name].NewValueAdded += callback;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new Exception("Could not find specified statistic name");
            }
        }
        public void RemoveStatisticListener(TimeLineStatisticNewValueAdded callback, string name)
        {
            bool found = false;
            foreach (Type type in _statDirectory.Keys)
            {
                if (type == typeof(int) && ((Dictionary<string, TimeLineStatistic<int>>)_stats[type]).ContainsKey(name))
                {
                    ((Dictionary<string, TimeLineStatistic<int>>)_stats[type])[name].NewValueAdded -= callback;
                    found = true;
                    break;
                }
                else if (type == typeof(float) && ((Dictionary<string, TimeLineStatistic<float>>)_stats[type]).ContainsKey(name))
                {
                    ((Dictionary<string, TimeLineStatistic<float>>)_stats[type])[name].NewValueAdded -= callback;
                    found = true;
                    break;
                }
                else if (type == typeof(DateTime) && ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type]).ContainsKey(name))
                {
                    ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type])[name].NewValueAdded -= callback;
                    found = true;
                    break;
                }
                else if (type == typeof(TimeSpan) && ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type]).ContainsKey(name))
                {
                    ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type])[name].NewValueAdded -= callback;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new Exception("Could not find specified statistic name");
            }
        }
        public StatisticUnit GetStatisticUnit(string name,StatisticUnitType unitType)
        {
            foreach (Type type in _statDirectory.Keys)
            {
                if (unitType == StatisticUnitType.metric)
                {
                    if (type == typeof(int) && ((Dictionary<string, TimeLineStatistic<int>>)_stats[type]).ContainsKey(name))
                    {
                        return ((Dictionary<string, TimeLineStatistic<int>>)_stats[type])[name].Statistic.MetricUnit;
                    }
                    else if (type == typeof(float) && ((Dictionary<string, TimeLineStatistic<float>>)_stats[type]).ContainsKey(name))
                    {
                        return ((Dictionary<string, TimeLineStatistic<float>>)_stats[type])[name].Statistic.MetricUnit;
                    }
                    else if (type == typeof(DateTime) && ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type]).ContainsKey(name))
                    {
                        return ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type])[name].Statistic.MetricUnit;
                    }
                    else if (type == typeof(TimeSpan) && ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type]).ContainsKey(name))
                    {
                        return ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type])[name].Statistic.MetricUnit;
                    }
                }
                else if (unitType == StatisticUnitType.standard)
                {
                    if (type == typeof(int) && ((Dictionary<string, TimeLineStatistic<int>>)_stats[type]).ContainsKey(name))
                    {
                        return ((Dictionary<string, TimeLineStatistic<int>>)_stats[type])[name].Statistic.StandardUnit;
                    }
                    else if (type == typeof(float) && ((Dictionary<string, TimeLineStatistic<float>>)_stats[type]).ContainsKey(name))
                    {
                        return ((Dictionary<string, TimeLineStatistic<float>>)_stats[type])[name].Statistic.StandardUnit;
                    }
                    else if (type == typeof(DateTime) && ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type]).ContainsKey(name))
                    {
                        return ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type])[name].Statistic.StandardUnit;
                    }
                    else if (type == typeof(TimeSpan) && ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type]).ContainsKey(name))
                    {
                        return ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type])[name].Statistic.StandardUnit;
                    }
                }
            }
            throw new Exception("Could not find specified statistic");
        }
        public double GetValue(string name,StatisticUnitType sut,DateTime dt)
        {
            foreach (Type type in _statDirectory.Keys)
            {
                if (type == typeof(int) && ((Dictionary<string, TimeLineStatistic<int>>)_stats[type]).ContainsKey(name))
                {
                    return (double)((Dictionary<string, TimeLineStatistic<int>>)_stats[type])[name].GetValue(sut, dt);
                }
                else if (type == typeof(float) && ((Dictionary<string, TimeLineStatistic<float>>)_stats[type]).ContainsKey(name))
                {
                    return (double)((Dictionary<string, TimeLineStatistic<float>>)_stats[type])[name].GetValue(sut, dt);
                }
                else if (type == typeof(DateTime) && ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type]).ContainsKey(name))
                {
                    return (double)((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type])[name].GetValue(sut, dt).Ticks;
                }
                else if (type == typeof(TimeSpan) && ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type]).ContainsKey(name))
                {
                    return (double)((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type])[name].GetValue(sut, dt).Ticks;
                }
            }
            throw new Exception("Could not find specified statistic");
        }
        public bool IsStatisticSelectedByDefault(string name)
        {
            foreach (Type type in _statDirectory.Keys)
            {
                if (type == typeof(int) && ((Dictionary<string, TimeLineStatistic<int>>)_stats[type]).ContainsKey(name))
                {
                    return ((Dictionary<string, TimeLineStatistic<int>>)_stats[type])[name].Statistic.SelectedByDefault;
                }
                else if (type == typeof(float) && ((Dictionary<string, TimeLineStatistic<float>>)_stats[type]).ContainsKey(name))
                {
                    return ((Dictionary<string, TimeLineStatistic<float>>)_stats[type])[name].Statistic.SelectedByDefault;
                }
                else if (type == typeof(DateTime) && ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type]).ContainsKey(name))
                {
                    return ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type])[name].Statistic.SelectedByDefault;
                }
                else if (type == typeof(TimeSpan) && ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type]).ContainsKey(name))
                {
                    return ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type])[name].Statistic.SelectedByDefault;
                }
            }
            throw new Exception("Could not find specified statistic");
        }
        public StatisticUnit GetStatisticMetricUnit(Type type,string name)
        {
            if (_stats.ContainsKey(type))
            {
                if (type == typeof(int))
                {
                    return ((Dictionary<string, TimeLineStatistic<int>>)_stats[type])[name].Statistic.MetricUnit;
                }
                else if (type == typeof(float))
                {
                    return ((Dictionary<string, TimeLineStatistic<float>>)_stats[type])[name].Statistic.MetricUnit;
                }
                else if (type == typeof(DateTime))
                {
                    return ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type])[name].Statistic.MetricUnit;
                }
                else if (type == typeof(TimeSpan))
                {
                    return ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type])[name].Statistic.MetricUnit;
                }
                else
                {
                    throw new Exception("Specified statistic name does not exist");
                }
            }
            else
            {
                throw new Exception("Specified statistic type does not exist");
            }
        }
        public StatisticUnit GetStatisticStandardUnit(Type type, string name)
        {
            if (_stats.ContainsKey(type))
            {
                if (type == typeof(int))
                {
                    return ((Dictionary<string, TimeLineStatistic<int>>)_stats[type])[name].Statistic.StandardUnit;
                }
                else if (type == typeof(float))
                {
                    return ((Dictionary<string, TimeLineStatistic<float>>)_stats[type])[name].Statistic.StandardUnit;
                }
                else if (type == typeof(DateTime))
                {
                    return ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[type])[name].Statistic.StandardUnit;
                }
                else if (type == typeof(TimeSpan))
                {
                    return ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[type])[name].Statistic.StandardUnit;
                }
                else
                {
                    throw new Exception("Specified statistic name does not exist");
                }
            }
            else
            {
                throw new Exception("Specified statistic type does not exist");
            }
        }
        public SortedList<DateTime, double> GetGraphableTimeline(string name, StatisticUnitType sut)
        {
            SortedList<DateTime, double> line = new SortedList<DateTime, double>();

            Type statType=null;
            foreach (Type t in _statDirectory.Keys)
            {
                if (_statDirectory[t].Contains(name))
                {
                    statType = t;
                }
            }

            IList<DateTime> times;
            if (statType != null)
            {
                if (statType == typeof(float))
                {
                    times = ((Dictionary<string, TimeLineStatistic<float>>)_stats[statType])[name].Times;
                    foreach (DateTime dt in times)
                    {
                        line.Add(dt, (double)(((Dictionary<string, TimeLineStatistic<float>>)_stats[statType])[name].GetValue(sut,dt)));
                    }
                }
                else if (statType == typeof(int))
                {
                    times = ((Dictionary<string, TimeLineStatistic<int>>)_stats[statType])[name].Times;
                    foreach (DateTime dt in times)
                    {
                        line.Add(dt, (double)(((Dictionary<string, TimeLineStatistic<int>>)_stats[statType])[name].GetValue(sut, dt)));
                    }
                }
                else if (statType == typeof(DateTime))
                {
                    times = ((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[statType])[name].Times;
                    foreach (DateTime dt in times)
                    {
                        line.Add(dt, (double)(((Dictionary<string, TimeLineStatistic<DateTime>>)_stats[statType])[name].GetValue(sut, dt).Ticks));
                    }
                }
                else if (statType == typeof(TimeSpan))
                {
                    times = ((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[statType])[name].Times;
                    foreach (DateTime dt in times)
                    {
                        line.Add(dt, (double)(((Dictionary<string, TimeLineStatistic<TimeSpan>>)_stats[statType])[name].GetValue(sut, dt).Ticks));
                    }
                }
                else
                {
                    throw new Exception("Unknown Column Type");
                }
            }
            else
            {
                throw new Exception("Could not find statistic with specified name");
            }
            return line;
        }
        public void AddValue<T>(string name,DateTime now, T value)
        {
            ((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)])[name].AddValue(now, value);
        }
        public T GetValue<T>(string name, DateTime now)
        {
            return ((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)])[name].GetValue(now);
        }
        public object GetValue(Type type,string name, DateTime now)
        {
            if (type == typeof(float))
            {
                return GetValue<float>(name, now);
            }
            else if (type == typeof(int))
            {
                return GetValue<int>(name, now);
            }
            else if (type == typeof(DateTime))
            {
                return GetValue<DateTime>(name, now);
            }
            else if (type == typeof(TimeSpan))
            {
                return GetValue<TimeSpan>(name, now);
            }
            else
            {
                throw new Exception("Unknown Column Type");
            }
        }
        public T GetValue<T>(string name, DateTime now, StatisticUnitType sut)
        {
            if (sut == StatisticUnitType.metric)
            {
                return ((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)])[name].GetValue(now);
            }
            else if (sut == StatisticUnitType.standard)
            {
                return ((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)])[name].Statistic.Calculator.ConvertToStandardUnits(((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)])[name].GetValue(now), ((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)])[name].Statistic.MetricUnit, ((Dictionary<string, TimeLineStatistic<T>>)_stats[typeof(T)])[name].Statistic.StandardUnit);
            }
            else
            {
                throw new Exception("Unkown Statistic Unit Type");
            }
        }
        public object GetValue(Type type, string name, DateTime now, StatisticUnitType sut) //i just picked a type here for the statistic unit type, kinda ugly...
        {
            if (type == typeof(float))
            {
                if (sut == StatisticUnitType.metric)
                {
                    return GetValue<float>(name, now, StatisticUnitType.metric);
                }
                else if (sut == StatisticUnitType.standard)
                {
                    return GetValue<float>(name, now, StatisticUnitType.standard);
                }
                else
                {
                    throw new Exception("Unkown Statistic Unit Type");
                }
            }
            else if (type == typeof(int))
            {
                if (sut == StatisticUnitType.metric)
                {
                    return GetValue<int>(name, now, StatisticUnitType.metric);
                }
                else if (sut == StatisticUnitType.standard)
                {
                    return GetValue<int>(name, now, StatisticUnitType.standard);
                }
                else
                {
                    throw new Exception("Unkown Statistic Unit Type");
                }
            }
            else if (type == typeof(DateTime))
            {
                return GetValue<DateTime>(name, now);
            }
            else if (type == typeof(TimeSpan))
            {
                return GetValue<TimeSpan>(name, now);
            }
            else
            {
                throw new Exception("Unknown Column Type");
            }
        }
        public Dictionary<Type, List<string>> StatisticDirectory
        {
            get
            {
                return _statDirectory;
            }
        }
        public static TimeLineStatisticCollection CreateDefaultStatisticCollection()
        {
            TimeLineStatisticCollection c = new TimeLineStatisticCollection();

            c.AddStatistic<int>(new Raw<int>("Current Leg", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Int32.Calculator(), 0, StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",false));
            c.AddStatistic<int>(new Raw<int>("Current Tack", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Int32.Calculator(), 0, StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",false));
            c.AddStatistic<int>(new Raw<int>("Position", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Int32.Calculator(), 0, StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",true));
            c.AddStatistic<int>(new Sum<int>("Position Changes", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Int32.Calculator(), StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",false));
            c.AddStatistic<int>(new First<int>("Start Position", 0, new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Int32.Calculator(), StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",false));
            c.AddStatistic<int>(new Last<int>("End Position", 0, new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Int32.Calculator(), StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",false));
            
            c.AddStatistic<float>(new Raw<float>("Speed",new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0f, StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",true));
            //c.AddStatistic<float>(new SetTimeAverage<float>("Speed (10 Second Average)", new TimeSpan(0, 0, 10), new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0, StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, ""));
            c.AddStatistic<float>(new Raw<float>("VMG to Wind", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0f, StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",false));
            c.AddStatistic<float>(new Raw<float>("VMG to Mark", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0f, StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",false));
            c.AddStatistic<float>(new Raw<float>("VMG to Course", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0f, StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",true));
            c.AddStatistic<float>(new Average<float>("Average VMG to Wind", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",false));
            c.AddStatistic<float>(new Average<float>("Average VMG to Mark", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",false));
            c.AddStatistic<float>(new Average<float>("Average VMG to Course", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",true));
            c.AddStatistic<float>(new Raw<float>("Distance to Mark", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0f, StatisticType.distance, StatisticUnit.meters, StatisticUnit.yards, "",true));
            c.AddStatistic<float>(new Raw<float>("Distance to Course", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0f, StatisticType.distance, StatisticUnit.meters, StatisticUnit.yards, "",true));
            c.AddStatistic<float>(new Raw<float>("Angle to Mark", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0f, StatisticType.angle, StatisticUnit.degrees, StatisticUnit.degrees, "",true));
            c.AddStatistic<float>(new Raw<float>("Angle to Wind", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0f, StatisticType.angle, StatisticUnit.degrees, StatisticUnit.degrees, "",false));
            c.AddStatistic<float>(new Raw<float>("Angle to Course", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), 0f, StatisticType.angle, StatisticUnit.degrees, StatisticUnit.degrees, "",true));
            c.AddStatistic<float>(new Average<float>("Average Speed", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",true));
            c.AddStatistic<float>(new Minimum<float>("Minimum Speed", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), float.MaxValue, StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",false));
            c.AddStatistic<float>(new Maximum<float>("Maximum Speed", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), float.MinValue, StatisticType.speed, StatisticUnit.kmh, StatisticUnit.mph, "",true));
            c.AddStatistic<float>(new Average<float>("Average Angle to Wind", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), StatisticType.angle, StatisticUnit.degrees, StatisticUnit.degrees, "",false));
            c.AddStatistic<float>(new Average<float>("Average Angle to Course", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), StatisticType.angle, StatisticUnit.degrees, StatisticUnit.degrees, "",true));
            c.AddStatistic<float>(new First<float>("Start Distance to Mark", 0f, new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), StatisticType.distance, StatisticUnit.meters, StatisticUnit.yards, "",false));
            c.AddStatistic<float>(new Last<float>("End Distance to Mark", 0f, new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), StatisticType.distance, StatisticUnit.meters, StatisticUnit.yards, "",false));
            c.AddStatistic<float>(new Sum<float>("Distance Covered", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float.Calculator(), StatisticType.distance, StatisticUnit.kilometers, StatisticUnit.miles, "",true));
            
            c.AddStatistic<DateTime>(new Minimum<DateTime>("Start Time", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.DateTime.Calculator(), DateTime.MaxValue, StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",false));
            c.AddStatistic<DateTime>(new Maximum<DateTime>("End Time", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.DateTime.Calculator(), DateTime.MinValue, StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",false));

            c.AddStatistic<TimeSpan>(new Raw<TimeSpan>("Elapsed", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.TimeSpan.Calculator(), new TimeSpan(), StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",true));
            c.AddStatistic<TimeSpan>(new Raw<TimeSpan>("Lag Behind Leader", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.TimeSpan.Calculator(),new TimeSpan(), StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",true));
            c.AddStatistic<TimeSpan>(new Raw<TimeSpan>("Lag Behind Next", new AmphibianSoftware.VisualSail.Data.Statistics.Calculator.TimeSpan.Calculator(),new TimeSpan(), StatisticType.other, StatisticUnit.other, StatisticUnit.other, "",true));
            
            return c;
        }
    }
}