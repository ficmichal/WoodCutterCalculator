using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WoodCutterCalculator.Models.Enums;
using WoodCutterCalculator.Models.Extensions;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.Order;
using WoodCutterCalculator.Models.PlotDatas;
using WoodCutterCalculator.Models.Stock;
using WoodCutterCalculator.Models.Utils;
using WoodCutterCalculator.Repositories;

namespace WoodCutterCalculator.Models
{
    public class OrderProcessor
    {
        private IPlanksToCutRepository _planksToCutRepository;
        private Random _randomDouble =  new Random();
        private const int _lackOfPiecesThisClass = 0;
        private int _numberOfPossibleCutsPerPlank;
        private double _promotionRate;
        private double _percentageOfElite;
        private GeneticAlgorithmParameters _algorithmParameters;
        private List<StockDetailsEnum> _possibleCuttedStocks;
        private string _orderId;
        private Stopwatch _recorderTimeOfAlgorithmExecuting;

        public int[][] PlanksInTheWarehouse { get; set; }
        public double[] HistoryOfLearning { get; set; }
        public double BestSolution { get; set; }
        public StockWarehouse AllCuttedStocks { get; set; } =  new StockWarehouse();
        public bool NeccesityOfChangeFitness { get; set; } = false;
        private IEnumerable<StockDetailsEnum> _classesOfNotEnoughCuttedStocks;
        private SortedDictionary<double, int> ActualSpecimenClassification = new SortedDictionary<double, int>(new DescendingComparer<double>());

        public OrderProcessor(IPlanksToCutRepository planksToCutRepository)
        {
            _planksToCutRepository = planksToCutRepository;
        }

        public OrderProcessor Create(GeneticAlgorithmParameters algorithmParameters, string idOfOrderToPlot = null)
        {
            var planksInTheWarehouse = idOfOrderToPlot != null ? _planksToCutRepository.GetByOrderId(idOfOrderToPlot) ??
                _planksToCutRepository?.GetLastAdded() : _planksToCutRepository?.GetLastAdded();
            _orderId = planksInTheWarehouse?.OrderId;
            PlanksInTheWarehouse = planksInTheWarehouse?.Planks;

            _algorithmParameters = algorithmParameters;
            _promotionRate = _algorithmParameters.PromotionRate;
            _percentageOfElite = _algorithmParameters.PercentageOfElite;
            _numberOfPossibleCutsPerPlank = _algorithmParameters.LengthOfPlank - 1;
            HistoryOfLearning = new double[_algorithmParameters.NumberOfIterations];

            _possibleCuttedStocks = DetailsOfStocks.Prices.Keys.ToList();
            _recorderTimeOfAlgorithmExecuting = new Stopwatch();
            return this;
        }

        public AllPlotDatas Calculate(ICollection<int> placedOrder, bool picturedMode = false)
        {
            _recorderTimeOfAlgorithmExecuting.Start();
            var numberOfBigPacks = 10;
            var numberOfPlanksPerBigPack = PlanksInTheWarehouse.Length / numberOfBigPacks;
            var numberOfPlanksPerPack = _algorithmParameters.NumberOfPlanksPerPack;
            var countOfPlankPacks = numberOfPlanksPerBigPack / numberOfPlanksPerPack;
            int[] cuttedStocks = new int[placedOrder.Count];
            StocksToPicture firstAndLastStocksToPictureIt = null;

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
                    if (!picturedMode)
                    {
                        var (theBestSolutionOfOnePack, theCuttedStocksOfOnePack) = CalculateOnePackOfPlanks(plankPack);
                        //Add best solution of package to global best solution.
                        BestSolution += theBestSolutionOfOnePack;
                        cuttedStocks = theCuttedStocksOfOnePack.CuttedStocks.Values.ToArray();
                        AllCuttedStocks.AddCuttedStocks(theCuttedStocksOfOnePack.CuttedStocks);
                    }
                    else
                    {
                        var (theBestSolutionOfOnePack, theCuttedStocksOfOnePack, 
                            firstAndLastStocksToPicture) = CalculateOnePackOfPlanksToPicturedIt(plankPack);
                        //Add best solution of package to global best solution.
                        BestSolution += theBestSolutionOfOnePack;
                        cuttedStocks = theCuttedStocksOfOnePack.CuttedStocks.Values.ToArray();
                        AllCuttedStocks.AddCuttedStocks(theCuttedStocksOfOnePack.CuttedStocks);
                        firstAndLastStocksToPictureIt = firstAndLastStocksToPicture;
                    }
                }
            }
            _recorderTimeOfAlgorithmExecuting.Stop();

            if (!picturedMode)
            {
                return new AllPlotDatas
                {
                    HistoryOfLearning = HistoryOfLearning,
                    OrderId = _orderId,
                    HistogramData = new HistogramData { OrderedStocks = placedOrder.ToArray(), CuttedStocks = AllCuttedStocks.ConvertDictionaryToArray() },
                    AlgorithmParameters = new AlgorithmParameters(_algorithmParameters, _recorderTimeOfAlgorithmExecuting.ElapsedMilliseconds),
                    NumberOfCuttedPlanks = PlanksInTheWarehouse.Length
                };
            }
            else
            {
                return new PicturedDatas
                {
                    FirstAndLastStocksToPicture = firstAndLastStocksToPictureIt,
                    HistoryOfLearning = HistoryOfLearning,
                    OrderId = _orderId,
                    HistogramData = new HistogramData { OrderedStocks = placedOrder.ToArray(), CuttedStocks = AllCuttedStocks.ConvertDictionaryToArray() },
                    AlgorithmParameters = new AlgorithmParameters(_algorithmParameters, _recorderTimeOfAlgorithmExecuting.ElapsedMilliseconds),
                    NumberOfCuttedPlanks = PlanksInTheWarehouse.Length
            };
            }
        }

        private (double theBestSolutionOfOnePack, StockWarehouse theCuttedStocksOfOnePack) 
            CalculateOnePackOfPlanks(int[][] packOfPlanks)
        {
            var numberOfIterations = _algorithmParameters.NumberOfIterations;
            var population = new SpecimenPopulation(_algorithmParameters);
            //First iteration
            var firstIteration = FindBestSolutionInCurrentPopulation(population, packOfPlanks);
            var theBestSolutionOfOnePack = HistoryOfLearning[0] = firstIteration.bestSolutionOfOnePack;
            var theCuttedStocksOfOnePack = firstIteration.cuttedStocksOfOnePack;

            population.UpdatePopulation(firstIteration.specimenClassification);

            for (int j = 1; j < numberOfIterations; j++)
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

        private (double theBestSolutionOfOnePack, StockWarehouse theCuttedStocksOfOnePack, StocksToPicture firstAndLastStocksToPicture)
            CalculateOnePackOfPlanksToPicturedIt(int[][] packOfPlanks)
        {
            var numberOfIterations = _algorithmParameters.NumberOfIterations;
            var population = new SpecimenPopulation(_algorithmParameters);
            //First iteration
            var firstIteration = FindBestSolutionInCurrentPopulation(population, packOfPlanks);
            var theBestSolutionOfOnePack = HistoryOfLearning[0] = firstIteration.bestSolutionOfOnePack;
            var theCuttedStocksOfOnePack = firstIteration.cuttedStocksOfOnePack;
            var theBestSpecimen = population.SplitPopulationToSpecimens(firstIteration.specimenClassification.ElementAt(0).Value);
            var firstAndLastStocksToPicture = new StocksToPicture(theBestSpecimen, packOfPlanks, 
                theCuttedStocksOfOnePack.ConvertDictionaryToArray(), theBestSolutionOfOnePack);
            population.UpdatePopulation(firstIteration.specimenClassification);

            for (int j = 1; j < numberOfIterations; j++)
            {
                var (bestSolutionOfOnePack, cuttedStocksOfOnePack, specimenClassification) =
                    FindBestSolutionInCurrentPopulation(population, packOfPlanks);
                if (bestSolutionOfOnePack > theBestSolutionOfOnePack)
                {
                    theBestSolutionOfOnePack = bestSolutionOfOnePack;
                    theCuttedStocksOfOnePack = cuttedStocksOfOnePack;
                }
                HistoryOfLearning[j] += bestSolutionOfOnePack;

                if (j == numberOfIterations - 1)
                {
                    theBestSpecimen = population.SplitPopulationToSpecimens(firstIteration.specimenClassification.ElementAt(0).Value);
                    firstAndLastStocksToPicture.Add(theBestSpecimen, theCuttedStocksOfOnePack.ConvertDictionaryToArray(), theBestSolutionOfOnePack);
                }
                population.UpdatePopulation(specimenClassification);
            }
            return (theBestSolutionOfOnePack, theCuttedStocksOfOnePack, firstAndLastStocksToPicture);
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
                if (_classesOfNotEnoughCuttedStocks.Count() > 0)
                {
                    ClassifyTheSpecimens(specimenClassification, promotedValueOfPack, i);
                }
                else
                {
                    ClassifyTheSpecimens(specimenClassification, valueOfPack, i);
                }
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
            int[][] cuttedPiecesOfWood = new int[_algorithmParameters.LengthOfPlank][];
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
            var minLength = DetailsOfStocks.MultipleMinorStockUnit.FirstOrDefault().Value;
            var maxLength = DetailsOfStocks.MultipleMinorStockUnit.LastOrDefault().Value;

            //Too long or too short stocks are useless.
            if (stockLength > maxLength || stockLength < minLength)
            {
                kindOfStock = StockDetailsEnum.Useless;
                realValueOfStock = -(smallPieciesOfPlank[StockClassEnum.FirstClass] * DetailsOfStocks.LossesPerMinorUnit[StockClassEnum.FirstClass]
                    + smallPieciesOfPlank[StockClassEnum.SecondClass] * DetailsOfStocks.LossesPerMinorUnit[StockClassEnum.SecondClass]
                    + smallPieciesOfPlank[StockClassEnum.ThirdClass] * DetailsOfStocks.LossesPerMinorUnit[StockClassEnum.ThirdClass]);
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
                        //Condition checking existance that stock
                        if(_possibleCuttedStocks.Contains(kindOfStock))
                        {
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
                        //Wrong dimension or class
                        else
                        {
                            return (0, 0, StockDetailsEnum.Useless);
                        }
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
                loss += DetailsOfStocks.LossesPerMinorUnit[stockClass] * smallPieciesOfPlank[stockClass];
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
            => plank.SubArray(lastCutIndex + 1, index - lastCutIndex);

        private void ClassifyTheSpecimens(SortedDictionary<double, int> specimenClassification, double valueOfPack, int index)
        {
            if (specimenClassification.Keys.Contains(valueOfPack))
            {
                var tempValueOfPack = valueOfPack + _randomDouble.NextDouble();
                while (specimenClassification.Keys.Contains(tempValueOfPack))
                {
                    tempValueOfPack = valueOfPack + _randomDouble.NextDouble();
                }
                specimenClassification.Add(tempValueOfPack, index);
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
