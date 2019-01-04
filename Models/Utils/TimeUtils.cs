using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodCutterCalculator.Models.Utils
{
    public static class TimeUtils
    {
        public static string PrintTimeSpan(long milliseconds)
        {
            var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            var minutes = (int)timeSpan.TotalMinutes % 60;
            if (minutes == 0)
            {
                return $"{Math.Round(timeSpan.TotalSeconds, 2)} sec.";
            }
            else
            {
                var seconds = (int)timeSpan.TotalSeconds % 60;
                return $"{minutes} min. {seconds} sec.";
            }
        }
    }
}
