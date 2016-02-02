using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace AmphibianSoftware.VisualSail.Library
{
    public static class GeometryHelper
    {
        public static double LineMagnitude(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
        public static double DistancePointToLineSegment(double x1, double y1, double x2, double y2, double px, double py)
        {
            //px,py is the point to test.
            //x1,y1,x2,y2 is the line to check distance.
            //Returns distance from the line, or if the intersecting point on the line nearest
            //the point tested is outside the endpoints of the line, the distance to the
            //nearest endpoint.
            //Returns 9999 on 0 denominator conditions.

            double lineMag;
            double u;

            double ix;//intersecting point
            double iy;

            lineMag = LineMagnitude(x1, y1, x2, y2);
            if (lineMag < 0.00000001)
            {
                return 9999;
            }

            u = (((px - x1) * (x2 - x1)) + ((py - y1) * (y2 - y1)));
            u = u / (lineMag * lineMag);

            if(u < 0.00001 || u > 1)
            {
                ix = LineMagnitude(px, py, x1, y1);
                iy = LineMagnitude(px, py, x2, y2);
                if (ix > iy)
                {
                    return iy;
                }
                else
                {
                    return ix;
                }
            }
            else
            {
                ix = x1 + u * (x2 - x1);
                iy = y1 + u * (y2 - y1);
                return LineMagnitude(px, py, ix, iy);
            }
        }
        public static bool TryFindIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
        {
            float ua = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X);
            float ub = (a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X);
            float denominator = (b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y);

            //bool hasIntersection = false; 
            //bool hasCoincident = false;

            if (Math.Abs(denominator) <= 0.00001f)
            {
                if (Math.Abs(ua) <= 0.00001f && Math.Abs(ub) <= 0.00001f)
                {
                    //hasIntersection = true;
                    //hasCoincident = true;
                    intersection = (a1 + a2) / 2;
                    return true;
                }
                else
                {
                    intersection = new Vector2();
                    return false;
                }
            }
            else
            {
                ua /= denominator;
                ub /= denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    intersection = new Vector2();
                    intersection.X = a1.X + ua * (a2.X - a1.X);
                    intersection.Y = a1.Y + ua * (a2.Y - a1.Y);
                    return true;
                }
                else
                {
                    intersection = new Vector2();
                    return false;
                }
            }
        }
    }
}
