using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public class Sum<T>:Statistic<T>
    {
        private T _sum;
        public Sum(string name, AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Calculator<T> calculator, StatisticType type, StatisticUnit metricUnit, StatisticUnit standardUnit, string description, bool selectedByDefault) : base(name, calculator, type, metricUnit, standardUnit, description, selectedByDefault) { }
        public override string StatisticCalculation
        {
            get
            {
                return "Sum";
            }
        }
        public override void AddValue(T val, DateTime t)
        {
            _sum=Calculator.Add(_sum,val);
        }
        public override T Value
        {
            get
            {
                return _sum;
            }
            set
            {
                _sum = value;
            }
        }
    }
}
