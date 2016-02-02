using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace AmphibianSoftware.VisualSail.Library
{
    public static class BezierHelper
    {
        public static List<Vector3> CreateBezier(int PointCount, List<Vector3> controlPoints)
        {
            List<Vector3> points = new List<Vector3>(PointCount);
            for (int i = 0; i < PointCount; i++)
            {
                points.Add(BezierRecurse((float)i / (float)PointCount, controlPoints));
            }
            //set first and last points to be the same as first and last control points
            points[0] = controlPoints[0];
            points.Add(controlPoints[controlPoints.Count - 1]);
            return points;
        }

        public static Vector3 BezierRecurse(float step, List<Vector3> points)
        {
            //recursive until 1 point is returned
            List<Vector3> newPoints = new List<Vector3>(points.Count - 1);
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 p1 = points[i];
                Vector3 p2 = points[i + 1];
                newPoints.Add(PositionOnLine(step, p1, p2));
            }
            if (newPoints.Count == 1)
            {
                return newPoints[0];
            }
            else
            {
                return BezierRecurse(step, newPoints);
            }
        }

        private static Vector3 PositionOnLine(float step, Vector3 p1, Vector3 p2)
        {
            Vector3 newPoint = new Vector3();
            newPoint.X = (p1.X + ((p2.X - p1.X) * step));
            newPoint.Y = (p1.Y + ((p2.Y - p1.Y) * step));
            newPoint.Z = (p1.Z + ((p2.Z - p1.Z) * step));
            return newPoint;
        }

        public static List<Vector3> CreateSmoothedLine(List<Vector3> linePoints, out SortedList<int, int> pointMapping, out List<Vector3> allControlPoints,out List<float> distances)
        {
            pointMapping = new SortedList<int, int>();

            float scale = 0.25f;
            float pointCountDivisor = 1f;
            int pointCountMaximum = 20;
            int pointCountMinimum = 5;

            List<Vector3> points = new List<Vector3>();
            allControlPoints = new List<Vector3>();
            distances = new List<float>();
            for (int i = 0; i < linePoints.Count - 1; i++)
            {
                List<Vector3> controlPoints = new List<Vector3>();
                Vector3 start = linePoints[i];
                Vector3 control;
                Vector3 end = linePoints[i + 1];
                if (i > 0)
                {
                    float angle = -(float)Math.Atan2(start.Z - allControlPoints[i - 1].Z, start.X - allControlPoints[i - 1].X);
                    float distance = (float)Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Z - start.Z), 2));
                    float controlX = start.X + (float)Math.Cos(angle) * distance * scale;//- (float)Math.Sin(flippedAngle) * dz;
                    float controlZ = start.Z - (float)Math.Sin(angle) * distance * scale;//- (float)Math.Cos(flippedAngle) * dz;
                    control = new Vector3(controlX, 0, controlZ);
                    distances.Add(distance);
                }
                else
                {
                    control = start;
                    distances.Add(0f);
                }

                int pc = (int)(Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Z - start.Z), 2)) / pointCountDivisor);
                if (pc > pointCountMaximum)
                {
                    pc = pointCountMaximum;
                }
                if (pc < pointCountMinimum)
                {
                    pc = pointCountMinimum;
                }

                controlPoints.Add(start);
                controlPoints.Add(control);
                controlPoints.Add(end);
                pointMapping.Add(i, points.Count);
                List<Vector3> temp = CreateBezier(pc, controlPoints);
                temp.RemoveAt(temp.Count - 1);
                points.AddRange(temp);
                allControlPoints.Add(control);
            }
            return points;
        }
        public static List<Vector3> CreateSmoothedLine(List<Vector3> linePoints)
        {
            List<Vector3> allControlPoints;
            List<float> distances;
            SortedList<int, int> mapping;
            return CreateSmoothedLine(linePoints, out mapping,out allControlPoints,out distances);
        }
    }
}
