using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;
using AmphibianSoftware.VisualSail.PostBuild;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    [DoNotObfuscate()]
    public enum StatisticType { distance, angle, speed, other };
    [DoNotObfuscate()]
    public enum StatisticUnitType { metric, standard };
    [DoNotObfuscate()]
    public enum StatisticUnit { yards, meters, miles, kilometers, degrees, kmh, mph, other, knot };
    public abstract class Statistic<T>
    {
        private string _name;
        private StatisticType _type;
        private StatisticUnit _metricUnit;
        private StatisticUnit _standardUnit;
        private string _description;
        private bool _selectedByDefault;
        private AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Calculator<T> _calculator;
        public Statistic(string name, AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Calculator<T> calculator, StatisticType type, StatisticUnit metricUnit, StatisticUnit standardUnit, string description, bool selectedByDefault)
        {
            _name = name;
            _calculator = calculator;
            _type = type;
            _metricUnit = metricUnit;
            _standardUnit = standardUnit;
            _description = description;
            _selectedByDefault = selectedByDefault;
        }
        public bool SelectedByDefault
        {
            get
            {
                return _selectedByDefault;
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
            }
        }
        public StatisticType Type
        {
            get
            {
                return _type;
            }
        }
        public StatisticUnit MetricUnit
        {
            get
            {
                return _metricUnit;
            }
        }
        public StatisticUnit StandardUnit
        {
            get
            {
                return _standardUnit;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
        }
        public abstract string StatisticCalculation { get; }
        public abstract T Value { get; set; }
        public abstract void AddValue(T val,DateTime now);
        public Calculator<T> Calculator
        {
            get
            {
                return _calculator;
            }
        }
        public double GetValueAsDouble()
        {
            return _calculator.ToDouble(Value);
        }
    }
}
