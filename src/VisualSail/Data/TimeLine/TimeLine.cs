using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public class TimeLine<T>
    {
        private string _name;
        private int _currentIndex = 0;
        private SortedList<DateTime, T> _timeline;
        public TimeLine()
        {
            _timeline = new SortedList<DateTime, T>();
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
            }
        }
        public DateTime CurrentTime
        {
            get
            {
                return _timeline.Keys[_currentIndex];
            }
        }
        public void AddValue(DateTime time,T val)
        {
            lock (_timeline)
            {
                if (!_timeline.ContainsKey(time))
                {
                    _timeline.Add(time, val);
                    _currentIndex = _timeline.IndexOfKey(time);
                }
            }
        }
        public T CurrentValue
        {
            get
            {
                return _timeline[_timeline.Keys[_currentIndex]];
            }
        }
        public IList<DateTime> Times
        {
            get
            {
                return _timeline.Keys;
            }
        }
        public T GetValue(DateTime time)
        {
            lock (_timeline)
            {
                if (_timeline.ContainsKey(time))
                {
                    return _timeline[time];
                }
                else
                {
                    if (_timeline.Count == 0)
                    {
                        return default(T);
                    }
                    else if (_timeline.Count == 1)
                    {
                        return _timeline[_timeline.Keys[_currentIndex]];
                    }
                    else
                    {
                        if (time > _timeline.Keys[_currentIndex])
                        {
                            while (_currentIndex < _timeline.Keys.Count && _timeline.Keys[_currentIndex] < time)
                            {
                                _currentIndex++;
                            }
                            _currentIndex = _currentIndex - 1;
                        }
                        else
                        {
                            while (_timeline.Keys[_currentIndex] > time && _currentIndex > 0)
                            {
                                _currentIndex--;
                            }
                        }
                        return _timeline[_timeline.Keys[_currentIndex]];
                    }
                }
            }
        }
    }
}
