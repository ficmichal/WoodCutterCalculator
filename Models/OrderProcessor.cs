﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Enums;
using WoodCutterCalculator.Models.Extensions;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.Managers;
using WoodCutterCalculator.Models.Mongo;
using WoodCutterCalculator.Models.Order;
using WoodCutterCalculator.Models.Stock;
using WoodCutterCalculator.Models.Utils;

namespace WoodCutterCalculator.Models
{
    public class OrderProcessor
    {
        private MongoDBManager mongoDBManager;
        private Random _randomDouble;
        private const int _lackOfPiecesThisClass = 0;
        private readonly int _numberOfPossibleCutsPerPlank;
        private readonly int _coefficientOfCut;
        private readonly double _promotionRate;
        private GeneticAlgorithmParameters _algorithmParameters;

        public int[][] PlanksInTheWarehouse { get; set; }
        public double[] HistoryOfLearning { get; set; }
        public double BestSolution { get; set; }
        public StockWarehouse AllCuttedStocks { get; set; }
        public bool NeccesityOfChangeFitness { get; set; } = false;
        private IEnumerable<StockDetailsEnum> _classesOfNotEnoughCuttedStocks;

        public OrderProcessor(GeneticAlgorithmParameters algorithmParameters)
        {
            mongoDBManager = new MongoDBManager(new SettingsManager());
            PlanksInTheWarehouse = mongoDBManager.PlanksToCut.AsQueryable().OrderByDescending(x => x.StartedCuttingDay).FirstOrDefault().Planks;
            _randomDouble = new Random();
            _algorithmParameters = algorithmParameters;
            _coefficientOfCut = _algorithmParameters.CoefficientOfCut;
            _promotionRate = _algorithmParameters.PromotionRate;
            _numberOfPossibleCutsPerPlank = _algorithmParameters.MaxPossibleCutsPerPlank;
            AllCuttedStocks = new StockWarehouse();
            HistoryOfLearning = new double[_algorithmParameters.NumberOfIterations];
        }

        public object Calculate(ICollection<int> placedOrder)
        {
            var numberOfBigPacks = 10;
            var numberOfPlanksPerBigPack = PlanksInTheWarehouse.Length / numberOfBigPacks;
            var numberOfPlanksPerPack = _algorithmParameters.NumberOfPlanksPerPack;
            var countOfPlankPacks = numberOfPlanksPerBigPack / numberOfPlanksPerPack;

            var placedOrderDictionary = FitnessUtils.ConvertPlacedOrderToDictionary(placedOrder.ToArray());
            for (int i = 0; i < numberOfBigPacks; i++)
            {
                //Checking is a necessity to change a fitness.
                //If we don't cut stocks properly to order yet, we change also the fitness for this big package.
                _classesOfNotEnoughCuttedStocks = new List<StockDetailsEnum>();
                _classesOfNotEnoughCuttedStocks = IsNeccesityOfChangeFitness
                    (i / (double)numberOfBigPacks, placedOrderDictionary, AllCuttedStocks.CuttedStocks);

                //Split whole warehouse to pack of planks.
                var plankBigPack = SplitWarehouseToBigPacks(i, numberOfPlanksPerBigPack);
                for (int j = 0; j < countOfPlankPacks; j++)
                {
                    var plankPack = SplitBigPackToSmallPacks(plankBigPack, j, numberOfPlanksPerPack);

                    //Main function - create, update population and calculate fitness. 
                    var (theBestSolutionOfOnePack, theCuttedStocksOfOnePack) = CalculateOnePackOfPlanks(plankPack);

                    //Add best solution of package to global best solution.
                    BestSolution += theBestSolutionOfOnePack;
                    AllCuttedStocks.AddCuttedStocks(theCuttedStocksOfOnePack.CuttedStocks);
                }

            }

            return new object();
        }

        private (double theBestSolutionOfOnePack, StockWarehouse theCuttedStocksOfOnePack) 
            CalculateOnePackOfPlanks(int[][] packOfPlanks)
        {
            var numberOfIterations = _algorithmParameters.NumberOfIterations;
            var population = new SpecimenPopulation(_algorithmParameters);
            //First iteration
            var firstIteration = FindBestSolutionInCurrentPopulation(population, packOfPlanks);
            var theBestSolutionOfOnePack = firstIteration.bestSolutionOfOnePack;
            var theCuttedStocksOfOnePack = firstIteration.cuttedStocksOfOnePack;
            population.UpdatePopulation(firstIteration.specimenClassification);

            for (int j = 0; j < numberOfIterations; j++)
            {
                var (bestSolutionOfOnePack, cuttedStocksOfOnePack, specimenClassification) = 
                    FindBestSolutionInCurrentPopulation(population, packOfPlanks);
                if (bestSolutionOfOnePack > theBestSolutionOfOnePack)
                {
                    theBestSolutionOfOnePack = bestSolutionOfOnePack;
                    theCuttedStocksOfOnePack = cuttedStocksOfOnePack;
                }
                HistoryOfLearning[j] += bestSolutionOfOnePack;

                population.UpdatePopulation(specimenClassification);
            }
            return (theBestSolutionOfOnePack, theCuttedStocksOfOnePack);
        }

        private (double bestSolutionOfOnePack, StockWarehouse cuttedStocksOfOnePack, SortedDictionary<double, int> specimenClassification) 
            FindBestSolutionInCurrentPopulation(SpecimenPopulation population, int[][] packOfPlanks)
        {
            var bestPackOfCuttedStock = new StockWarehouse();
            var bestSolutionInPackOfPlanks = 0.0;
            var sizeOfPopulation = population.Population.Length;

            //Every plank is calculated separately.
            var sizeOfSpecimen = population.PackOfPlanks.Length;
            var countOfSpecimens = sizeOfPopulation / sizeOfSpecimen;
            var specimenClassification = new SortedDictionary<double, int>(new DescendingComparer<double>());

            for (int i = 0; i < countOfSpecimens; i++)
            {
                var packOfCuttedStock = new StockWarehouse();
                var valueOfPack = 0.0;
                var promotedValueOfPack = 0.0;

                //Calculate every plank separately as a one specimen.
                var specimen = population.SplitPopulationToSpecimens(i);
                for (int j = 0; j < packOfPlanks.Length; j++)
                {
                    //Cut plank and calculate its value.
                    var (plankValue, promotedPlankValue, stocksOfCuttedPlank) 
                        = CalculateValueOfPlank(packOfPlanks[j], SplitSpecimenOfCutsToOnePlankCuts(specimen, j));
                    //Add calculated values of plank to the small pack of plank.
                    valueOfPack += plankValue;
                    promotedValueOfPack += promotedPlankValue;
                    packOfCuttedStock.AddCuttedStocks(stocksOfCuttedPlank.CuttedStocks);
                }

                //If we don't cut stocks properly to order yet, we checked fitness with promotion.
                if (_classesOfNotEnoughCuttedStocks.Count() > 0 
                    ? promotedValueOfPack > bestSolutionInPackOfPlanks : valueOfPack > bestSolutionInPackOfPlanks)
                {
                    bestSolutionInPackOfPlanks = valueOfPack;
                    bestPackOfCuttedStock = packOfCuttedStock;
                }
                //Add genotype and its fenotype to sorted specimen dictionary
                ClassifyTheSpecimens(specimenClassification, valueOfPack, i);
            }

            return (bestSolutionInPackOfPlanks, bestPackOfCuttedStock, specimenClassification);
        }

        //1 meter plank is registered as 20 of 5 cm pieces (int[]). It can be cut every 10 cm (byte[]).
        public (double plankValue, double promotedPlankValue, StockWarehouse stocksOfCuttedPlank) CalculateValueOfPlank(int[] plank, byte[] cuts)
        {
            var cuttedPlank = CutPlank(plank, cuts);

            return CalculateValueOfCuttedPlank(cuttedPlank);
        }

        public int[][] CutPlank(int[] plank, byte[] cuts)
        {
            int[][] cuttedPiecesOfWood = new int[_algorithmParameters.MaxPossibleCutsPerPlank + 1][];
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

        public (double, double, StockWarehouse) CalculateValueOfCuttedPlank(int[][] cuttedPlank)
        {
            var calculatedStocks = new StockWarehouse();
            var totalValue = 0.0;
            var promotedTotalValue = 0.0;

            var cuttedPlankLength = cuttedPlank.Length;
            for (int i = 0; i < cuttedPlankLength; i++)
            {
                var smallPieciesOfPlank = new Dictionary<StockClassEnum, int>
                {
                    { StockClassEnum.FirstClass, 0 },
                    { StockClassEnum.SecondClass, 0 },
                    { StockClassEnum.ThirdClass, 0 },
                };
                //Counting smallest piecies of plank grouped by class
                var stockLength = cuttedPlank[i].Length;
                for (int j = 0; j < stockLength; j++)
                {
                    smallPieciesOfPlank[(StockClassEnum)cuttedPlank[i][j]]++;
                }

                var (realValueOfStock, promotedValueOfStock, kindOfStock) = CalculateStock(stockLength, smallPieciesOfPlank);
                totalValue += realValueOfStock;
                promotedTotalValue += promotedValueOfStock;
                calculatedStocks.AddCuttedStock(kindOfStock);
            }

            return (totalValue, promotedTotalValue, calculatedStocks);
        }

        public (double realValueOfStock, double promotedValueOfStock, StockDetailsEnum kindOfStock) CalculateStock(int stockLength, 
            Dictionary<StockClassEnum, int> smallPieciesOfPlank)
        {
            double realValueOfStock;
            StockDetailsEnum kindOfStock;
            double promotedValueOfStock;
            var minLength = _coefficientOfCut * DetailsOfStocks.MultipleMinorStockUnit.FirstOrDefault().Value;
            var maxLength = _coefficientOfCut * DetailsOfStocks.MultipleMinorStockUnit.LastOrDefault().Value;

            //Too long or too short stocks are useless.
            if (stockLength > maxLength || stockLength < minLength)
            {
                kindOfStock = StockDetailsEnum.Useless;
                realValueOfStock = -(smallPieciesOfPlank[StockClassEnum.FirstClass] * DetailsOfStocks.LossesPer5Cm[StockClassEnum.FirstClass]
                    + smallPieciesOfPlank[StockClassEnum.SecondClass] * DetailsOfStocks.LossesPer5Cm[StockClassEnum.SecondClass]
                    + smallPieciesOfPlank[StockClassEnum.ThirdClass] * DetailsOfStocks.LossesPer5Cm[StockClassEnum.ThirdClass]);
                return (realValueOfStock, realValueOfStock, kindOfStock);
            }
            else
            {
                var allClasses = Enum.GetValues(typeof(StockClassEnum)).Cast<int>();
                //Loop starts from the worst class. If even only a one element is this class that means all stock must be this class.
                //If condition is false we iterate to better class.
                var theWorstClass = allClasses.Max();
                for (int i = theWorstClass; i > 0; i--)
                {
                    //Is there any element which is [i] class?
                    if (smallPieciesOfPlank[(StockClassEnum)i] != _lackOfPiecesThisClass)
                    {
                        //Get a details about stock (length and class)
                        kindOfStock = (StockDetailsEnum)(DetailsOfStocks.MultipleConst * stockLength + i);

                        var valueOfStockWithoutLoss = DetailsOfStocks.Prices[kindOfStock];

                        var classPossibleLossOfStock = allClasses.Where(e => e < i);
                        var lossValue = CalculateLoss(classPossibleLossOfStock.ToArray(), smallPieciesOfPlank);

                        realValueOfStock = promotedValueOfStock = valueOfStockWithoutLoss - lossValue;

                        //Is there a necessity to change fitness?
                        if (_classesOfNotEnoughCuttedStocks.Contains(kindOfStock))
                        {
                            promotedValueOfStock = DetailsOfStocks.Prices[kindOfStock] * _promotionRate;
                        }

                        return (realValueOfStock, promotedValueOfStock, kindOfStock);
                    }
                }

                //something go wrong
                return (-10000, -10000, StockDetailsEnum.Useless);
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

        private int[][] SplitBigPackToSmallPacks(int[][] bigPack, int index, int numberOfPlanksPerPack)
            => bigPack.SubArray(index * numberOfPlanksPerPack, numberOfPlanksPerPack);

        private int[][] SplitWarehouseToBigPacks(int index, int numberOfPlanksPerPack)
            => PlanksInTheWarehouse.SubArray(index * numberOfPlanksPerPack, numberOfPlanksPerPack);

        private byte[] SplitSpecimenOfCutsToOnePlankCuts(byte[] specimen, int index)
            => specimen.SubArray(index * _numberOfPossibleCutsPerPlank, _numberOfPossibleCutsPerPlank);

        private int[] CutPlankToStock(int[] plank, int index, int lastCutIndex)
            => plank.SubArray(_coefficientOfCut * (lastCutIndex + 1), _coefficientOfCut * (index - lastCutIndex));

        private void ClassifyTheSpecimens(SortedDictionary<double, int> specimenClassification, double valueOfPack, int index)
        {
            if (specimenClassification.Keys.Contains(valueOfPack))
            {
                specimenClassification.Add(valueOfPack + _randomDouble.NextDouble(), index);
            }
            else
            {
                specimenClassification.Add(valueOfPack, index);
            }
        }

        private List<StockDetailsEnum> IsNeccesityOfChangeFitness(double percentOfProceedAlgorithm,
            Dictionary<StockDetailsEnum, int> placedOrder, Dictionary<StockDetailsEnum, int> alreadyCuttedStocks)
        {
            var classesOfNotEnoughCuttedStocks = new List<StockDetailsEnum>();
            var changingFitnessDictionary = FitnessUtils.IsNecessityToChangeFitness(percentOfProceedAlgorithm, placedOrder, alreadyCuttedStocks);

            foreach(var item in changingFitnessDictionary)
            {
                if (item.Value)
                {
                    classesOfNotEnoughCuttedStocks.Add(item.Key);
                }
            }

            return classesOfNotEnoughCuttedStocks;
        }
    }
}
