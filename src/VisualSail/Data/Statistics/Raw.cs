using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public class Raw<T> : Statistic<T>
    {
        private T _value;
        public Raw(string name, AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Calculator<T> calculator, T value, StatisticType type, StatisticUnit metricUnit, StatisticUnit standardUnit, string description, bool selectedByDefault)
            : base(name,calculator, type, metricUnit,standardUnit, description, selectedByDefault)
        {
            _value = value;
        }
        public override string StatisticCalculation
        {
            get
            {
                return "Raw";
            }
        }
        public override void AddValue(T val, DateTime t)
        {
            _value = val;
        }
        public override T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
