using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmphibianSoftware.VisualSail.Library
{
    public class VerbageHelper
    {
        public static string PositionString(int position)
        {
            if (position == 0)
            {
                return "";
            }
            else
            {
                if (position == 11 || position == 12 || position == 13)
                {
                    return position + "th";
                }
                else
                {
                    string posString = position.ToString();
                    char lastDigit = posString[posString.Length - 1];
                    string suffix = "";
                    if (lastDigit == '1')
                        suffix = "st";
                    else if (lastDigit == '2')
                        suffix = "nd";
                    else if (lastDigit == '3')
                        suffix = "rd";
                    else
                        suffix = "th";
                    return posString + suffix;
                }
            }
        }
    }
}
