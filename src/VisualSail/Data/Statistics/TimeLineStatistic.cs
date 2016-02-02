using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public delegate void TimeLineStatisticNewValueAdded(TimeLineStatisticCollection collection, DateTime dt);
    public class TimeLineStatistic<T>
    {
        private TimeLineStatisticNewValueAdded _newValueAdded;
        private Statistic<T> _stat;
        private TimeLine<T> _time;
        private DateTime _low=DateTime.MaxValue;
        private DateTime _high=DateTime.MinValue;
        private TimeLineStatisticCollection _collection;
        public TimeLineStatistic(Statistic<T> stat)
        {
            _stat = stat;
            _time = new TimeLine<T>();
        }
        public Statistic<T> Statistic
        {
            get
            {
                return _stat;
            }
        }
        public TimeLineStatisticCollection Collection
        {
            get
            {
                return _collection;
            }
            set
            {
                _collection = value;
            }
        }
        public void AddValue(DateTime time,T value)
        {
            if ((_low == null && _high == null) || time<_low || time > _high)
            {
                if (_low == null && _high == null)
                {
                    _low = time;
                    _high = time;
                }
                else if (time > _high)
                {
                    _high = time;
                }
                else if (time < _low)
                {
                    _low = time;
                }
                _stat.AddValue(value,time);
                if (_stat.Calculator.GreaterThan(_stat.Value, _time.GetValue(time)) || _stat.Calculator.GreaterThan(_time.GetValue(time), _stat.Value))
                {
                    _time.AddValue(time, _stat.Value);
                    if (_newValueAdded != null)
                    {
                        _newValueAdded(this.Collection,time);
                    }
                }
            }
        }
        public event TimeLineStatisticNewValueAdded NewValueAdded
        {
            add
            {
                _newValueAdded -= value;
                _newValueAdded += value;
            }
            remove
            {
                _newValueAdded -= value;
            }
        }
        public T GetValue(StatisticUnitType sut,DateTime t)
        {
            if (sut == StatisticUnitType.metric)
            {
                return _time.GetValue(t);
            }
            else if (sut == StatisticUnitType.standard)
            {
                return _stat.Calculator.ConvertToStandardUnits(_time.GetValue(t), _stat.MetricUnit,_stat.StandardUnit);
            }
            else
            {
                throw new Exception("Unspecified statistic unit type");
            }
        }
        public T GetValue(DateTime t)
        {
            return GetValue(StatisticUnitType.metric, t);
        }
        public IList<DateTime> Times
        {
            get
            {
                return _time.Times;
            }
        }
    }
}
