using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public class SetTimeAverage<T> : Statistic<T>
    {
        private SortedList<DateTime, T> _values;
        private TimeSpan _timespan;
        private T _zero;
        public SetTimeAverage(string name,TimeSpan span, AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Calculator<T> calculator, T zero, StatisticType type, StatisticUnit metricUnit,StatisticUnit standardUnit, string description,bool selectedByDefault)
            : base(name, calculator, type, metricUnit, standardUnit, description, selectedByDefault)
        {
            _zero = zero;
            _timespan = span;
            _values = new SortedList<DateTime, T>();
        }
        public override string StatisticCalculation
        {
            get
            {
                return _timespan.TotalSeconds+ "Second Average";
            }
        }
        public override void AddValue(T val,DateTime now)
        {
            for (int i = 0; i < _values.Keys.Count; i++)
            {
                if (_values.Keys[i] < now - _timespan || _values.Keys[i] > now)
                {
                    _values.Remove(_values.Keys[i]);
                    i = i - 1;
                }
            }
            _values.Add(now, val);
        }
        public override T Value
        {
            get
            {
                if (_values.Count > 0)
                {
                    T total = _zero;
                    for (int i = 0; i < _values.Keys.Count; i++)
                    {
                        total = Calculator.Add(total, _values[_values.Keys[i]]);
                    }
                    return Calculator.DivideByInt(total, _values.Count);
                }
                else
                {
                    return _zero;
                }
            }
            set
            {
                //_average = value;
                throw new Exception("The value of this statistic cannot be set");
            }
        }
    }
}
