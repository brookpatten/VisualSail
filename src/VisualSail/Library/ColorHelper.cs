using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AmphibianSoftware.VisualSail.Library
{
    public static class ColorHelper
    {
        public static Color Darken(Color c)
        {
            return Color.FromArgb(c.R / 2, c.G / 2, c.B / 2);
        }
        public static Color AutoColorPick(int s)
        {
            Color[] colors = { Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Purple, Color.Brown, Color.Pink, Color.LightBlue, Color.LightGreen, Color.Yellow, Color.YellowGreen, Color.Gray, Color.Turquoise, Color.Khaki };
            if (s < colors.Length)
            {
                return colors[s];
            }
            else
            {
                Random rand = new Random(DateTime.Now.Millisecond);
                return Color.FromArgb(rand.Next());
            }
        }
    }
}
