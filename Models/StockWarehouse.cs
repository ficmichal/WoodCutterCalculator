using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Enums;

namespace WoodCutterCalculator.Models
{
    public class StockWarehouse
    {
        public Dictionary<StockDetailsEnum, int> CuttedStocks { get; set; }

        public StockWarehouse()
        {
            CuttedStocks = new Dictionary<StockDetailsEnum, int>
            {
                { StockDetailsEnum.FirstClassStock40, 0},
                { StockDetailsEnum.FirstClassStock30, 0},
                { StockDetailsEnum.FirstClassStock20, 0},
                { StockDetailsEnum.SecondClassStock40, 0},
                { StockDetailsEnum.SecondClassStock30, 0},
                { StockDetailsEnum.SecondClassStock20, 0},
                { StockDetailsEnum.ThirdClassStock40, 0},
                { StockDetailsEnum.ThirdClassStock30, 0},
                { StockDetailsEnum.ThirdClassStock20, 0},
                { StockDetailsEnum.Useless, 0},
            };
        }

        public Dictionary<StockDetailsEnum, int> AddCuttedStock(StockDetailsEnum cuttedStockDetails)
        {
            CuttedStocks[cuttedStockDetails]++;
            return CuttedStocks;
        }
    }
}
