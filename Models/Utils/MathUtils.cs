using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodCutterCalculator.Models.Utils
{
    public static class MathUtils
    {
        public static string PrintAsPercent(double number)
        {
            return $"{number * 100} %";
        }
    }
}
