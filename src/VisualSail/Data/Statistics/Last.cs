﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics.Calculator;

namespace AmphibianSoftware.VisualSail.Data.Statistics
{
    public class Last<T> : Statistic<T>
    {
        private T _value;
        private DateTime _lastTime = DateTime.MinValue;
        public Last(string name, T value, AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Calculator<T> calculator, StatisticType type, StatisticUnit metricUnit, StatisticUnit standardUnit, string description, bool selectedByDefault)
            : base(name,calculator, type, metricUnit,standardUnit, description, selectedByDefault)
        {
            _value = value;
        }
        public override string StatisticCalculation
        {
            get
            {
                return "Last";
            }
        }
        public override void AddValue(T val, DateTime t)
        {
            if (t > _lastTime)
            {
                _value = val;
                _lastTime = t;
            }
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
