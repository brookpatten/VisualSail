using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data.Statistics;

namespace AmphibianSoftware.VisualSail.Data.Statistics.Calculator
{
    public abstract class Calculator<T>
    {
        public abstract T Add(T a, T b);
        public abstract T Subtract(T a, T b);
        public abstract T Multiply(T a, T b);
        public abstract T Divide(T a, T b);
        public abstract T MultiplyByInt(T a, int b);
        public abstract T DivideByInt(T a, int b);
        public abstract bool LessThan(T a, T b);
        public abstract bool GreaterThan(T a, T b);
        public abstract string ToString(T v);
        public abstract T ConvertToStandardUnits(T value, StatisticUnit from, StatisticUnit to);
        public abstract double ToDouble(T v);
    }
    public static class UnitConversionHelper
    {
        public static double KmhToMph(double i)
        {
            return KilometersToMiles(i);
        }
        public static double KilometersToMiles(double i)
        {
            return i * 0.621371192;
        }
        public static double MetersToYards(double i)
        {
            return i * 1.0936133;
        }
        public static double MetersToFeet(double i)
        {
            return i * 3.2808399;
        }
    }
}
namespace AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Int32
{
    public class Calculator : Calculator<int>
    {
        public override int Add(int a, int b)
        {
            return a + b;
        }
        public override int Subtract(int a, int b)
        {
            return a - b;
        }
        public override int Multiply(int a, int b)
        {
            return a * b;
        }
        public override int Divide(int a, int b)
        {
            return a / b;
        }
        public override int MultiplyByInt(int a, int b)
        {
            return Multiply(a, b);
        }
        public override int DivideByInt(int a, int b)
        {
            return Divide(a, b);
        }
        public override bool LessThan(int a, int b)
        {
            return a < b;
        }
        public override bool GreaterThan(int a, int b)
        {
            return a > b;
        }
        public override string ToString(int v)
        {
            return v.ToString();
        }
        public override double ToDouble(int v)
        {
            return (double)v;
        }
        public override int ConvertToStandardUnits(int value, StatisticUnit from, StatisticUnit to)
        {
            if (from == StatisticUnit.kilometers)
            {
                return (int)UnitConversionHelper.KilometersToMiles((double)value);
            }
            else if (from == StatisticUnit.meters)
            {
                return (int)UnitConversionHelper.MetersToYards((double)value);
            }
            else if (from == StatisticUnit.kmh)
            {
                return (int)UnitConversionHelper.KmhToMph((double)value);
            }
            else
            {
                return value;
            }
        }
    }
}
namespace AmphibianSoftware.VisualSail.Data.Statistics.Calculator.Float
{
    public class Calculator : Calculator<float>
    {
        public override float Add(float a, float b)
        {
            return a + b;
        }
        public override float Subtract(float a, float b)
        {
            return a - b;
        }
        public override float Multiply(float a, float b)
        {
            return a * b;
        }
        public override float Divide(float a, float b)
        {
            return a / b;
        }
        public override float MultiplyByInt(float a, int b)
        {
            return a * (float)b;
        }
        public override float DivideByInt(float a, int b)
        {
            return a / (float)b;
        }
        public override bool LessThan(float a, float b)
        {
            return a < b;
        }
        public override bool GreaterThan(float a, float b)
        {
            return a > b;
        }
        public override string ToString(float v)
        {
            return string.Format("{0:F2}", v);
        }
        public override double ToDouble(float v)
        {
            return (double)v;
        }
        public override float ConvertToStandardUnits(float value, StatisticUnit from, StatisticUnit to)
        {
            if (from == StatisticUnit.kilometers)
            {
                return (float)UnitConversionHelper.KilometersToMiles((double)value);
            }
            else if (from == StatisticUnit.meters)
            {
                return (float)UnitConversionHelper.MetersToYards((double)value);
            }
            else if (from == StatisticUnit.kmh)
            {
                return (float)UnitConversionHelper.KmhToMph((double)value);
            }
            else
            {
                return value;
            }
        }
    }
}
namespace AmphibianSoftware.VisualSail.Data.Statistics.Calculator.DateTime
{
    public class Calculator : Calculator<System.DateTime>
    {
        public override System.DateTime Add(System.DateTime a, System.DateTime b)
        {
            throw new Exception("Adding Dates is not implemented");
        }
        public override System.DateTime Subtract(System.DateTime a, System.DateTime b)
        {
            throw new Exception("Subtracting Dates is not implemented");
        }
        public override System.DateTime Multiply(System.DateTime a, System.DateTime b)
        {
            throw new Exception("Multiplication of dates is not implemented");
        }
        public override System.DateTime Divide(System.DateTime a, System.DateTime b)
        {
            throw new Exception("Division of dates is not implemented");
        }
        public override System.DateTime MultiplyByInt(System.DateTime a, int b)
        {
            return new System.DateTime(a.Ticks * b);
        }
        public override System.DateTime DivideByInt(System.DateTime a, int b)
        {
            return new System.DateTime(a.Ticks / b);
        }
        public override bool LessThan(System.DateTime a, System.DateTime b)
        {
            return a < b;
        }
        public override bool GreaterThan(System.DateTime a, System.DateTime b)
        {
            return a > b;
        }
        public override string ToString(System.DateTime v)
        {
            return v.Hour + ":" + v.Minute + ":" + v.Second + "." + v.Millisecond;
        }
        public override double ToDouble(System.DateTime v)
        {
            return (double)v.Ticks;
        }
        public override System.DateTime ConvertToStandardUnits(System.DateTime value, StatisticUnit from, StatisticUnit to)
        {
            return value;
        }
    }
}
namespace AmphibianSoftware.VisualSail.Data.Statistics.Calculator.TimeSpan
{
    public class Calculator : Calculator<System.TimeSpan>
    {
        public override System.TimeSpan Add(System.TimeSpan a, System.TimeSpan b)
        {
            return a + b;
        }
        public override System.TimeSpan Subtract(System.TimeSpan a, System.TimeSpan b)
        {
            return a - b;
        }
        public override System.TimeSpan Multiply(System.TimeSpan a, System.TimeSpan b)
        {
            throw new Exception("Multiplication of TimeSpans is not implemented");
        }
        public override System.TimeSpan Divide(System.TimeSpan a, System.TimeSpan b)
        {
            throw new Exception("Division of TimeSpans is not implemented");
        }
        public override System.TimeSpan MultiplyByInt(System.TimeSpan a, int b)
        {
            return new System.TimeSpan(a.Ticks * b);
        }
        public override System.TimeSpan DivideByInt(System.TimeSpan a, int b)
        {
            return new System.TimeSpan(a.Ticks / b);
        }
        public override bool LessThan(System.TimeSpan a, System.TimeSpan b)
        {
            return a < b;
        }
        public override bool GreaterThan(System.TimeSpan a, System.TimeSpan b)
        {
            return a > b;
        }
        public override string ToString(System.TimeSpan v)
        {
            return v.Hours + ":" + v.Minutes + ":" + v.Seconds + "." + v.Milliseconds;
        }
        public override double ToDouble(System.TimeSpan v)
        {
            return (double)v.Ticks;
        }
        public override System.TimeSpan ConvertToStandardUnits(System.TimeSpan value, StatisticUnit from, StatisticUnit to)
        {
            return value;
        }
    }
}