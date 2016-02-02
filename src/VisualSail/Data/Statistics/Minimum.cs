using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public class Minimum<T> : Statistic<T>
    {
        private T _minimum;
        public Minimum(string name, AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Calculator<T> calculator, T maxValue, StatisticType type, StatisticUnit metricUnit, StatisticUnit standardUnit, string description, bool selectedByDefault)
            : base(name,calculator, type, metricUnit,standardUnit, description, selectedByDefault) 
        {
            _minimum = maxValue;
        }
        public override string StatisticCalculation
        {
            get
            {
                return "Minimum";
            }
        }
        public override void AddValue(T val, DateTime t)
        {
            if (Calculator.LessThan(val,_minimum))
            {
                _minimum = val;
            }
        }
        public override T Value
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
            }
        }
    }
}
