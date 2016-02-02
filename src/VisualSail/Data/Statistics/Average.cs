using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public class Average<T> : Statistic<T>
    {
        private T _average;
        private int _count;
        public Average(string name, AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Calculator<T> calculator, StatisticType type, StatisticUnit metricUnit, StatisticUnit standardUnit, string description, bool selectedByDefault)
            : base(name,calculator, type, metricUnit,standardUnit, description, selectedByDefault) 
        {
            _count = 0;
        }
        public override string StatisticCalculation
        {
            get
            {
                return "Average";
            }
        }
        public override void AddValue(T val,DateTime t)
        {
            _average = Calculator.DivideByInt((Calculator.Add(Calculator.MultiplyByInt(_average, _count) , val)), _count + 1);
            _count++;
        }
        public override T Value
        {
            get
            {
                return _average;
            }
            set
            {
                _average = value;
            }
        }
    }
}
