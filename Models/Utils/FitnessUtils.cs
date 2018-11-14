using System.Collections.Generic;
using System.Linq;
using WoodCutterCalculator.Models.Enums;

namespace WoodCutterCalculator.Models.Utils
{
    public static class FitnessUtils
    {
        public static Dictionary<StockDetailsEnum, bool> IsNecessityToChangeFitness
            (double percentOfProceedAlgorithm, Dictionary<StockDetailsEnum, int> placedOrder, Dictionary<StockDetailsEnum, int> alreadyCuttedStocks)
        {
            var changingFitnessDictionary = new Dictionary<StockDetailsEnum, bool>
            {
                { StockDetailsEnum.FirstClassStock40, false },
                { StockDetailsEnum.FirstClassStock30, false },
                { StockDetailsEnum.FirstClassStock20, false },
                { StockDetailsEnum.SecondClassStock40, false },
                { StockDetailsEnum.SecondClassStock30, false },
                { StockDetailsEnum.SecondClassStock20, false },
                { StockDetailsEnum.ThirdClassStock40, false },
                { StockDetailsEnum.ThirdClassStock30, false },
                { StockDetailsEnum.ThirdClassStock20, false }
            };

            var keys = alreadyCuttedStocks.Keys.ToArray();
            var countOfDictionary = placedOrder.Keys.Count;
            for (int i = 0; i < countOfDictionary; i++)
            {
                //Is already cutted not enough stocks? ((percent of algorithm * placed order) - 5% of placed order)
                if(alreadyCuttedStocks[keys[i]] < percentOfProceedAlgorithm * placedOrder[keys[i]] * 0.95)
                {
                    //necessity of change a priority of fitness
                    changingFitnessDictionary[keys[i]] = true;
                }
            }

            return changingFitnessDictionary;
        }

        public static Dictionary<StockDetailsEnum, int> ConvertPlacedOrderToDictionary(int[] placedOrder)
        {
            var placedOrderDictionary = new Dictionary<StockDetailsEnum, int>
            {
                { StockDetailsEnum.FirstClassStock40, 0 },
                { StockDetailsEnum.FirstClassStock30, 0 },
                { StockDetailsEnum.FirstClassStock20, 0 },
                { StockDetailsEnum.SecondClassStock40, 0 },
                { StockDetailsEnum.SecondClassStock30, 0 },
                { StockDetailsEnum.SecondClassStock20, 0 },
                { StockDetailsEnum.ThirdClassStock40, 0 },
                { StockDetailsEnum.ThirdClassStock30, 0 },
                { StockDetailsEnum.ThirdClassStock20, 0 }
            };

            var placedOrderKeys = placedOrderDictionary.Keys.ToArray();
            var placedOrderLength = placedOrder.Length;
            for (int i = 0; i < placedOrderLength; i++)
            {
                placedOrderDictionary[placedOrderKeys[i]] = placedOrder[i];
            }

            return placedOrderDictionary;
        }
    }
}
