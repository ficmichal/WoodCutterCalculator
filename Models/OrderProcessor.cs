using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Extensions;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.Matlab;

namespace WoodCutterCalculator.Models
{
    public class OrderProcessor
    {
        private byte _numberOfPossibleCutsPerPlank = 9;
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
                var plankPack = SplitWarehouseToPack(i, numberOfPlanksPerPack);
                CalculateOnePackOfPlanks(plankPack, numberOfIterations);
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
            return new object();
        }
        private double FindBestSolutionInCurrentPopulation(SpecimenPopulation population, int[][] packOfPlanks)
        {
            var bestSolution = 0;
            var sizeOfPopulation = population.Population.Length;
            var sizeOfSpecimen = population.PackOfPlanks.Length;
            var countOfSpecimens = sizeOfPopulation / sizeOfSpecimen;
            for (int k = 0; k < countOfSpecimens; k++)
            {
                var specimen = SplitPopulationToSpecimens(population, k, sizeOfSpecimen);
                for (int l = 0; l < packOfPlanks.Length; l++)
                {
                    var valueOfCuttedPlank = CalculateCuttedPlank(packOfPlanks[l], SplitSpecimenOfCutsToOnePlankCuts(specimen, l));
                }
            }
            return 1.0;
        }

        // 1 meter plank is registered as 20 of 5 cm pieces (int[]). It can be cut every 10 cm (byte[]).
        public double CalculateCuttedPlank(int[] plank, byte[] cuts)
        {
            var cuttedPlank = CutPlank(plank, cuts);

            return 1.0;
        }

        public int[][] CutPlank(int[] plank, byte[] cuts)
        {
            int maxCuttedPieces = 10;
            int[][] cuttedPiecesOfWood = new int[maxCuttedPieces][];
            int index = 0;
            int lastCutIndex = -1;

            var countOfPossibleCuts = cuts.Length;
            for (int i = 0; i < countOfPossibleCuts; i++)
            {
                if (cuts[i] == 1)
                {
                    cuttedPiecesOfWood[index] = CutPlankToStock(plank, i, lastCutIndex);
                    lastCutIndex = i;
                    index++;
                }
                if (i == countOfPossibleCuts - 1)
                {
                    if (cuts[i] == 1)
                    {
                        cuttedPiecesOfWood[index] = CutPlankToStock(plank, i, i - 1);
                    }
                    else
                    {
                        cuttedPiecesOfWood[index] = CutPlankToStock(plank, i + 1, lastCutIndex);
                    }
                }
            }

            return cuttedPiecesOfWood.Where(x => x?.Count() > 0).ToArray();
        }

        private int[][] SplitWarehouseToPack(int index, int numberOfPlanksPerPack)
            => PlanksInTheWarehouse.SubArray(index * numberOfPlanksPerPack, numberOfPlanksPerPack);

        private byte[] SplitPopulationToSpecimens(SpecimenPopulation population, int index, int sizeOfSpecimen)
            => population.Population.SubArray(index * sizeOfSpecimen, sizeOfSpecimen);

        private byte[] SplitSpecimenOfCutsToOnePlankCuts(byte[] specimen, int index)
            => specimen.SubArray(index * _numberOfPossibleCutsPerPlank, _numberOfPossibleCutsPerPlank);

        private int[] CutPlankToStock(int[] plank, int index, int lastCutIndex)
            => plank.SubArray(2 * (lastCutIndex + 1), 2 * (index - lastCutIndex));

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
