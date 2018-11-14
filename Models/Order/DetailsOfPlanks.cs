using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Enums;

namespace WoodCutterCalculator.Models.Order
{
    public class DetailsOfStocks
    {
        public const int MultipleConst = 10;
        public static Dictionary<StockClassEnum, double> LossesPerMinorUnit = new Dictionary<StockClassEnum, double>
        {
            { StockClassEnum.FirstClass, 2.0 },
            { StockClassEnum.SecondClass, 1.0 },
            { StockClassEnum.ThirdClass, 0.5 }
        };

        public static Dictionary<StockDetailsEnum, double> Prices = new Dictionary<StockDetailsEnum, double>
        {
            { StockDetailsEnum.FirstClassStock40, 35.0 },
            { StockDetailsEnum.FirstClassStock30, 24.0 },
            { StockDetailsEnum.FirstClassStock20, 15.0 },
            { StockDetailsEnum.SecondClassStock40, 25.0 },
            { StockDetailsEnum.SecondClassStock30, 18.0 },
            { StockDetailsEnum.SecondClassStock20, 11.0 },
            { StockDetailsEnum.ThirdClassStock40, 17.0 },
            { StockDetailsEnum.ThirdClassStock30, 8.0 },
            { StockDetailsEnum.ThirdClassStock20, 6.0 }
        };

        public static Dictionary<StockLengthEnum, int> MultipleMinorStockUnit = new Dictionary<StockLengthEnum, int>
        {
            { StockLengthEnum.Length20cm, 4 },
            { StockLengthEnum.Length30cm, 6 },
            { StockLengthEnum.Length40cm, 8 }
        };
    }
}
