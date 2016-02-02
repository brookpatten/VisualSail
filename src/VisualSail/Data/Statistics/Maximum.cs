using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public class Maximum<T> : Statistic<T>
    {
        private T _maximum;
        public Maximum(string name, AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Calculator<T> calculator, T minValue, StatisticType type, StatisticUnit metricUnit, StatisticUnit standardUnit, string description, bool selectedByDefault)
            : base(name,calculator, type, metricUnit,standardUnit,description, selectedByDefault)
        {
            _maximum = minValue;
        }
        public override string StatisticCalculation
        {
            get
            {
                return "Maximum";
            }
        }
        public override void AddValue(T val, DateTime t)
        {
            if (Calculator.GreaterThan(val, _maximum))
            {
                _maximum = val;
            }
        }
        public override T Value
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
            }
        }
    }
}
