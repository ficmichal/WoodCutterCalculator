using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Enums;
using WoodCutterCalculator.Models.Extensions;

namespace WoodCutterCalculator.Models.Utils
{
    public static class StockDescriptionUtils
    {
        public static IEnumerable<string> GetStockDescription()
        {
            var enums = Enum.GetValues(typeof(StockDetailsEnum)).Cast<int>().ToArray();
            for (int i = 0; i < enums.Count(); i++)
            {
                yield return ((StockDetailsEnum)enums[i]).DescriptionAttr();
            }
        }
    }
}
