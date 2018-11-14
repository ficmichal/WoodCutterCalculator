using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.Mongo;
using WoodCutterCalculator.Models.Planks;

namespace WoodCutterCalculator.Models.Stock
{
    public class StockWarehouseProcessor
    {
        private readonly MongoDBManager _mongoDBManager;
        private Random _randomNumber;
        public int[][] Planks;

        public StockWarehouseProcessor(GeneticAlgorithmParameters parameters, MongoDBManager mongoDBManager)
        {
            _mongoDBManager = mongoDBManager;
            _randomNumber = new Random();
            Planks = new int[100][];
        }

        public void SeedStockWarehouse()
        {
            var planksLength = Planks.Length;
            for (int i = 0; i < planksLength; i++)
            {
                var classOfPieceOfPlank = _randomNumber.Next(1, 3);
                Planks[i] = new int[20];
                Planks[i][0] = classOfPieceOfPlank;
                for (int j = 1; j < 20; j++)
                {
                    var classes = new List<int> { 1, 2, 3 };
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
            _mongoDBManager.PlanksToCut.InsertOne(new PlanksToCut { StartedCuttingDay = DateTime.UtcNow, Planks = Planks });
        }
    }
}
