using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Enums;
using WoodCutterCalculator.Models.Extensions;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.Matlab;
using WoodCutterCalculator.Models.Order;

namespace WoodCutterCalculator.Models
{
    public class OrderProcessor
    {
        private const int _lackOfPiecesThisClass = 0;
        private int _numberOfPossibleCutsPerPlank;
        private int _coefficientOfCut;
        private GeneticAlgorithmParameters _algorithmParameters;

        public int[][] PlanksInTheWarehouse { get; set; }
        public double[] HistoryOfLearning { get; set; }
        public double BestSolution { get; set; }
        public StockWarehouse AllCuttedPlanks { get; set; }

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
            _coefficientOfCut = _algorithmParameters.CoefficientOfCut;
            _numberOfPossibleCutsPerPlank = _algorithmParameters.MaxPossibleCutsPerPlank;
            AllCuttedPlanks = new StockWarehouse();
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
            for (int i = 0; i < countOfSpecimens; i++)
            {
                var specimen = SplitPopulationToSpecimens(population, i, sizeOfSpecimen);
                for (int j = 0; j < packOfPlanks.Length; j++)
                {
                    var (plankValue, stocksOfCuttedPlank) = CalculateValueOfPlank(packOfPlanks[j], SplitSpecimenOfCutsToOnePlankCuts(specimen, j));
                    bestSolution = 1;
                }
            }
            return 1.0;
        }

        // 1 meter plank is registered as 20 of 5 cm pieces (int[]). It can be cut every 10 cm (byte[]).
        public (double plankValue, StockWarehouse stocksOfCuttedPlank) CalculateValueOfPlank(int[] plank, byte[] cuts)
        {
            var cuttedPlank = CutPlank(plank, cuts);

            return CalculateValueOfCuttedPlank(cuttedPlank);
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

        public (double, StockWarehouse) CalculateValueOfCuttedPlank(int[][] cuttedPlank)
        {
            var calculatedStocks = new StockWarehouse();
            var totalValue = 0.0;

            var cuttedPlankLength = cuttedPlank.Length;
            for (int i = 0; i < cuttedPlankLength; i++)
            {
                var smallPieciesOfPlank = new Dictionary<StockClassEnum, int>
                {
                    { StockClassEnum.FirstClass, 0 },
                    { StockClassEnum.SecondClass, 0 },
                    { StockClassEnum.ThirdClass, 0 },
                };
                var stockLength = cuttedPlank[i].Length;
                for (int j = 0; j < stockLength; j++)
                {
                    if (cuttedPlank[i][j] == 1)
                    {
                        smallPieciesOfPlank[StockClassEnum.FirstClass]++;
                    }
                    else if (cuttedPlank[i][j] == 2)
                    {
                        smallPieciesOfPlank[StockClassEnum.SecondClass]++;
                    }
                    else
                    {
                        smallPieciesOfPlank[StockClassEnum.ThirdClass]++;
                    }
                }
                var (valueOfStock, kindOfStock) = CalculateStock(stockLength, smallPieciesOfPlank);
                totalValue += valueOfStock;
                calculatedStocks.AddCuttedStock(kindOfStock);
            }

            return (totalValue, calculatedStocks);
        }

        public (double valueOfStock, StockDetailsEnum kindOfStock) CalculateStock(int stockLength, 
            Dictionary<StockClassEnum, int> smallPieciesOfPlank)
        {
            double realValueOfStock;
            StockDetailsEnum kindOfStock;

            //too long or too short stocks
            if (stockLength > 8 || stockLength < 4)
            {
                kindOfStock = StockDetailsEnum.Useless;
                realValueOfStock = -(smallPieciesOfPlank[StockClassEnum.FirstClass] * DetailsOfStocks.LossesPer5Cm[StockClassEnum.FirstClass]
                    + smallPieciesOfPlank[StockClassEnum.SecondClass] * DetailsOfStocks.LossesPer5Cm[StockClassEnum.SecondClass]
                    + smallPieciesOfPlank[StockClassEnum.ThirdClass] * DetailsOfStocks.LossesPer5Cm[StockClassEnum.ThirdClass]);
                return (realValueOfStock, kindOfStock);
            }
            else
            {
                var allClasses = Enum.GetValues(typeof(StockClassEnum)).Cast<int>();
                var theWorstClass = allClasses.Max();
                for (int i = theWorstClass; i > 0; i--)
                {
                    if (smallPieciesOfPlank[(StockClassEnum)i] != _lackOfPiecesThisClass)
                    {
                        kindOfStock = (StockDetailsEnum)(DetailsOfStocks.MultipleConst * stockLength + i);
                        var valueOfStockWithoutLoss = DetailsOfStocks.Prices[kindOfStock];

                        var classPossibleLossOfStock = allClasses.Where(e => e < i);
                        var lossValue = CalculateLoss(classPossibleLossOfStock.ToArray(), smallPieciesOfPlank);

                        realValueOfStock = valueOfStockWithoutLoss - lossValue;
                        return (realValueOfStock, kindOfStock);
                    }
                }

                //something go wrong
                return (-10000, StockDetailsEnum.Useless);
            }
        }

        private double CalculateLoss(int[] classPossibleLossOfStock, Dictionary<StockClassEnum, int> smallPieciesOfPlank)
        {
            var loss = 0.0;
            var lengthOfClassPossibleLossOfStock = classPossibleLossOfStock.Count();
            for (int i = 0; i < lengthOfClassPossibleLossOfStock; i++)
            {
                var stockClass = (StockClassEnum)classPossibleLossOfStock[i];
                loss += DetailsOfStocks.LossesPer5Cm[stockClass] * smallPieciesOfPlank[stockClass];
            }

            return loss;
        }

        private int[][] SplitWarehouseToPack(int index, int numberOfPlanksPerPack)
            => PlanksInTheWarehouse.SubArray(index * numberOfPlanksPerPack, numberOfPlanksPerPack);

        private byte[] SplitPopulationToSpecimens(SpecimenPopulation population, int index, int sizeOfSpecimen)
            => population.Population.SubArray(index * sizeOfSpecimen, sizeOfSpecimen);

        private byte[] SplitSpecimenOfCutsToOnePlankCuts(byte[] specimen, int index)
            => specimen.SubArray(index * _numberOfPossibleCutsPerPlank, _numberOfPossibleCutsPerPlank);

        private int[] CutPlankToStock(int[] plank, int index, int lastCutIndex)
            => plank.SubArray(_coefficientOfCut * (lastCutIndex + 1), _coefficientOfCut * (index - lastCutIndex));
    }
}
