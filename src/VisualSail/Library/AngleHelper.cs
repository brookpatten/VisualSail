using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmphibianSoftware.VisualSail.Data;

using Microsoft.Xna.Framework;

namespace AmphibianSoftware.VisualSail.Library
{
    public static class AngleHelper
    {
        public static float NormalizeAngle(float angle)
        {
            angle = angle % MathHelper.TwoPi;
            if (angle < 0)
            {
                angle = MathHelper.TwoPi + angle;
            }
            return angle;
        }
        public static Vector2 PolarToRectangular(Vector2 origin, float theta, float r)
        {
            Vector2 result = new Vector2();
            result.X = r * (float)Math.Cos(theta);
            result.Y = r * (float)Math.Sin(theta);
            result.X += origin.X;
            result.Y += origin.Y;
            return result;
        }
        public static ProjectedPoint PolarToRectangular(ProjectedPoint origin, double theta, double r)
        {
            ProjectedPoint result = new ProjectedPoint();
            result.Easting = r * Math.Cos(theta);
            result.Northing = r * Math.Sin(theta);
            result.Easting += origin.Easting;
            result.Northing += origin.Northing;
            return result;
        }
        public static float FindHalfwayCounterClockwiseAngle(float previous, float next)
        {
            float roundingAngle = 0f;
            if (Math.Abs(AngleHelper.AngleDifference(previous, next)) > Math.Abs(AngleHelper.AngleDifference(next, previous)))
            {
                roundingAngle = previous + (AngleHelper.AngleDifference(previous, next) / 2f);
            }
            else
            {
                roundingAngle = previous + (AngleHelper.AngleDifference(next, previous) / 2f);
            }
            roundingAngle = AngleHelper.NormalizeAngle(roundingAngle + MathHelper.Pi);
            return roundingAngle;
        }
        public static float FindAngle(Vector3 a, Vector3 b)
        {
            return -(float)Math.Atan2(a.Z - b.Z, a.X - b.X);
        }
        public static double FindAngle(ProjectedPoint a, ProjectedPoint b)
        {
            return -Math.Atan2( a.Easting - b.Easting,a.Northing - b.Northing);
        }
        public static double FindAngleWTF(ProjectedPoint a, ProjectedPoint b)
        {
            return -Math.Atan2( a.Northing - b.Northing,a.Easting - b.Easting);
        }
        public static float AngleDifference(float a, float b)
        {
            a = NormalizeAngle(a);
            b = NormalizeAngle(b);

            float c = a - b;

            if (c < 0)
            {
                if (c < -MathHelper.Pi)
                {
                    return (MathHelper.TwoPi + c);
                }
                else
                {
                    return c;
                }
            }
            else
            {
                if (c > MathHelper.Pi)
                {
                    return -(MathHelper.TwoPi - c);
                }
                else
                {
                    return c;
                }
            }
        }
    }
}
