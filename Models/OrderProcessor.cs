using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.Matlab;

namespace WoodCutterCalculator.Models
{
    public class OrderProcessor
    {
        public int[][] PlanksInTheWarehouse { get; set; }
        public double[] HistoryOfLearning { get; set; }
        public double BestSolution { get; set; }
        private GeneticAlgorithmParameters _algorithmParameters;

        public OrderProcessor(GeneticAlgorithmParameters algorithmParameters)
        {
            PlanksInTheWarehouse = new int[][] 
            {
                new int[] {1, 1, 1, 1 ,2 ,1, 2, 2, 2, 2, 1, 1, 1, 1, 2, 1, 2, 2, 2, 2 },
                new int[] {3, 3, 3, 3 ,3 ,3, 3, 2, 2, 2, 1, 1, 1, 1, 2, 1, 2, 2, 2, 2 },
                new int[] {1, 2, 1, 2 ,2 ,1, 2, 2, 2, 2, 1, 1, 3, 3, 3, 3, 3, 3, 2, 2 },
                new int[] {1, 3, 1, 1 ,2 ,3, 2, 3, 2, 3, 1, 1, 3, 1, 2, 2, 2, 2, 2, 2 },
                new int[] {1, 1, 1, 3 ,2 ,1, 2, 2, 2, 2, 3, 3, 3, 3, 2, 1, 1, 1, 1, 1 },
                new int[] {1, 1, 1, 1 ,1 ,1, 1, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2 },
                new int[] {1, 1, 1, 1 ,1 ,1, 1, 1, 1, 3, 3, 3, 3, 3, 2, 1, 2, 2, 2, 2 },
                new int[] {2, 2, 2, 2 ,2 ,1, 2, 2, 1, 1, 1, 1, 1, 1, 2, 1, 2, 2, 2, 2 },
                new int[] {1, 1, 1, 1 ,2 ,1, 2, 2, 2, 2, 1, 1, 1, 1, 2, 1, 2, 2, 2, 2 },
                new int[] {2, 2, 2, 2 ,2 ,1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3 }
            };
            _algorithmParameters = algorithmParameters;
        }
        public object Calculate(ICollection<int> placedOrder)
        {
            var numberOfPlanksPerPack = _algorithmParameters.NumberOfPlanksPerPack;
            var numberOfIterations = _algorithmParameters.NumberOfIterations;
            HistoryOfLearning = new double[numberOfIterations];
            var countOfPlankPacks = PlanksInTheWarehouse.Length / numberOfPlanksPerPack;

            for (int i = 0; i < countOfPlankPacks; i++)
            {
                var plankpack = SplitWarehouseToPack(i, numberOfPlanksPerPack);
                CalculateOnePackOfPlanks(plankpack, numberOfIterations);
            }

            return new object();
        }

        private object CalculateOnePackOfPlanks(int[][] packOfPlanks, int numberOfIterations)
        {
            var population = new SpecimenPopulation(_algorithmParameters);
            for (int j = 0; j < numberOfIterations; j++)
            {
                var bestSolutionInCurrentPopulation = FindBestSolutionInCurrentPopulation(population, packOfPlanks);

            }
        }
        private double FindBestSolutionInCurrentPopulation(SpecimenPopulation population, int[][] packOfPlanks)
        {
            var sizeOfPopulation = population.Population.Length;
            var sizeOfSpecimen = population.PackOfPlanks.Length;
            for (int k = 0; k < sizeOfPopulation / sizeOfSpecimen; k++)
            {
                var specimen = SplitPopulationToSpecimens(population, k, sizeOfSpecimen);

            }
]
        }

        private int[][] SplitWarehouseToPack(int index, int numberOfPlanksPerPack)
            => PlanksInTheWarehouse.Skip(index * numberOfPlanksPerPack).Take(numberOfPlanksPerPack).ToArray();

        private byte[] SplitPopulationToSpecimens(SpecimenPopulation population, int index, int sizeOfSpecimen)
            => population.Population.Skip(index * sizeOfSpecimen).Take(sizeOfSpecimen).ToArray();

        //private int[,] CalculateInMatlab(ICollection<int> placedOrder)
        //{
        //    var matlab = MatlabClient.Create(@"c:\temp\example");
        //    object result = null;
        //    matlab.Feval("Power", 1, out result, placedOrder);
        //    var res = result as object[];
        //    return res[0] as int[,];
        //}

        //private Collection<CollectionDataValue> PrepareToPlot(ICollection<int> xDatas, int[,] yDatas)
        //{
        //    var plotResult = new Collection<CollectionDataValue>();
        //    var xDatasCount = xDatas.Count;
        //    if (xDatasCount != yDatas.Length)
        //    {
        //        return null;
        //    }

        //    var concatDatas = xDatas.Concat(RemoveOneDimensionFromYDatas(yDatas)).ToArray();

        //    for (int i = 0; i < xDatasCount; i++)
        //    {
        //        var point = new CollectionDataValue
        //        {
        //            XData = concatDatas[i],
        //            YData = concatDatas[i + xDatasCount]
        //        };
        //        plotResult.Add(point);
        //    }
        //    return plotResult;
        //}

        //private ICollection<int> RemoveOneDimensionFromYDatas(int[,] yDatas)
        //{
        //    var result = new int[yDatas.Length];

        //    for (int j = 0; j < yDatas.Length; j++)
        //    {
        //        result[j] = yDatas[0, j];
        //    }
        //    return result;
        //}
    }
}
