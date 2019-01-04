using System;
using System.Collections.Generic;
using System.Linq;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.Planks;
using WoodCutterCalculator.Repositories;

namespace WoodCutterCalculator.Models.Stock
{
    public class StockWarehouseProcessor
    {
        private IPlanksToCutRepository _planksToCutRepository;
        private Random _randomNumber = new Random();
        public int[][] Planks;
        private GeneticAlgorithmParameters _parameters;

        public StockWarehouseProcessor(IPlanksToCutRepository planksToCutRepository)
        {
            _planksToCutRepository = planksToCutRepository;
        }

        public StockWarehouseProcessor Create(GeneticAlgorithmParameters parameters, int sizeOfWarehouse)
        {
            _parameters = parameters;
            Planks = new int[sizeOfWarehouse][];

            return this;
        }

        public void SeedStockWarehouse()
        {
            var planksLength = Planks.Length;
            for (int i = 0; i < planksLength; i++)
            {
                var classOfPieceOfPlank = _randomNumber.Next(1, 4);
                Planks[i] = new int[_parameters.LengthOfPlank];
                Planks[i][0] = classOfPieceOfPlank;
                for (int j = 1; j < Planks[i].Length; j++)
                {
                    var classes = RandomListOfClass();
                    var probabilityCoefficient = _randomNumber.NextDouble();
                    // 80%
                    if (probabilityCoefficient < 0.8)
                    {
                        Planks[i][j] = classOfPieceOfPlank;
                    }
                    // 10%
                    else if (probabilityCoefficient >= 0.8 && probabilityCoefficient < 0.9)
                    {
                        classes.Remove(classOfPieceOfPlank);
                        var newclass = classes.FirstOrDefault();
                        Planks[i][j] = newclass;
                        classOfPieceOfPlank = newclass;
                    }
                    // 10%
                    else
                    {
                        classes.Remove(classOfPieceOfPlank);
                        classes.RemoveAt(0);
                        var newclass = classes.FirstOrDefault();
                        Planks[i][j] = newclass;
                        classOfPieceOfPlank = newclass;
                    }
                }
            }
            var now = DateTime.UtcNow.Date;
            var numberOfCalculate = int.Parse(_planksToCutRepository.GetLastOrderInDay(now)?.OrderId?.Split('_')?.FirstOrDefault() ?? "0");
            _planksToCutRepository.Add(
            new PlanksToCut
                {
                    StartedCuttingDay = now,
                    Planks = Planks,
                    OrderId = $"{(++numberOfCalculate).ToString()}_{now.ToShortDateString()}"
            });
        }

        private List<int> RandomListOfClass()
        {
            var classes = new List<int> { 1, 2, 3 };
            var randomList = new List<int>();
            
            while (randomList.Count < 3)
            {
                var randomClass = _randomNumber.Next(1, 4);
                if (!randomList.Contains(randomClass))
                {
                    randomList.Add(randomClass);
                }
            }

            return randomList;
        }
    }
}
