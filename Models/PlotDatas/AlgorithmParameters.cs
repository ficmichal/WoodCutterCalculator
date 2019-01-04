using GalaSoft.MvvmLight;
using System;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.Utils;

namespace WoodCutterCalculator.Models.PlotDatas
{
    public class AlgorithmParameters : ObservableObject
    {
        #region Fields

        private int _sizeOfPopulation;
        private int _numberOfPlanksPerPack;
        private int _numberOfIterations;
        private string _timeOfExecuting;
        private double _promotionRate;
        private string _mutationRate;
        private string _percentageOfElite;
        private string _percentageOfParentsChosenToSelection;

        #endregion

        #region Properties

        public int NumberOfPlanksPerPack
        {
            get
            {
                return _numberOfPlanksPerPack;
            }

            set
            {
                if (_numberOfPlanksPerPack == value)
                {
                    return;
                }

                _numberOfPlanksPerPack = value;
                RaisePropertyChanged();
            }
        }

        public int SizeOfPopulation
        {
            get
            {
                return _sizeOfPopulation;
            }

            set
            {
                if (_sizeOfPopulation == value)
                {
                    return;
                }

                _sizeOfPopulation = value;
                RaisePropertyChanged();
            }
        }

        public int NumberOfIterations
        {
            get
            {
                return _numberOfIterations;
            }

            set
            {
                if (_numberOfIterations == value)
                {
                    return;
                }

                _numberOfIterations = value;
                RaisePropertyChanged();
            }
        }

        public string TimeOfExecuting
        {
            get
            {
                return _timeOfExecuting;
            }

            set
            {
                if (_timeOfExecuting == value)
                {
                    return;
                }

                _timeOfExecuting = value;
                RaisePropertyChanged();
            }
        }

        public double PromotionRate
        {
            get
            {
                return _promotionRate;
            }

            set
            {
                if (_promotionRate == value)
                {
                    return;
                }

                _promotionRate = value;
                RaisePropertyChanged();
            }
        }

        public string MutationRate
        {
            get
            {
                return _mutationRate;
            }

            set
            {
                if (_mutationRate == value)
                {
                    return;
                }

                _mutationRate = value;
                RaisePropertyChanged();
            }
        }

        public string PercentageOfElite
        {
            get
            {
                return _percentageOfElite;
            }

            set
            {
                if (_percentageOfElite == value)
                {
                    return;
                }

                _percentageOfElite = value;
                RaisePropertyChanged();
            }
        }

        public string PercentageOfParentsChosenToSelection
        {
            get
            {
                return _percentageOfParentsChosenToSelection;
            }

            set
            {
                if (_percentageOfParentsChosenToSelection == value)
                {
                    return;
                }

                _percentageOfParentsChosenToSelection = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public AlgorithmParameters(GeneticAlgorithmParameters algorithmParameters, long timeOfExecuting)
        {
            NumberOfIterations = algorithmParameters.NumberOfIterations;
            NumberOfPlanksPerPack = algorithmParameters.NumberOfPlanksPerPack;
            SizeOfPopulation = algorithmParameters.SizeOfPopulation;
            PercentageOfElite = MathUtils.PrintAsPercent(algorithmParameters.PercentageOfElite);
            PercentageOfParentsChosenToSelection = MathUtils.PrintAsPercent(algorithmParameters.PercentageOfParentsChosenToSelection);
            MutationRate = MathUtils.PrintAsPercent(algorithmParameters.MutationRate);
            PromotionRate = algorithmParameters.PromotionRate;
            TimeOfExecuting = TimeUtils.PrintTimeSpan(timeOfExecuting);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AlgorithmParameters))
                return false;

            var other = obj as AlgorithmParameters;

            if (NumberOfPlanksPerPack != other.NumberOfPlanksPerPack || SizeOfPopulation != other.SizeOfPopulation
                || NumberOfIterations != other.NumberOfIterations || PercentageOfElite != other.PercentageOfElite
                || MutationRate != other.MutationRate || PromotionRate != other.PromotionRate
                || PercentageOfParentsChosenToSelection != other.PercentageOfParentsChosenToSelection)
                return false;

            return true;
        }

        public static bool operator ==(AlgorithmParameters x, AlgorithmParameters y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(AlgorithmParameters x, AlgorithmParameters y)
        {
            return !(x == y);
        }
    }
}
