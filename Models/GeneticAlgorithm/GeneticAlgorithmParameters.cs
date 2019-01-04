using GalaSoft.MvvmLight;

namespace WoodCutterCalculator.Models.GeneticAlgorithm
{
    public class GeneticAlgorithmParameters : ObservableObject
    {
        #region Fields

        private int _sizeOfPopulation;
        private int _numberOfPlanksPerPack;
        private int _lengthOfPlank;
        private int _numberOfIterations;
        private double _promotionRate;
        private double _mutationRate;
        private double _percentageOfElite;
        private double _percentageOfParentsChosenToSelection;

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

        public int LengthOfPlank
        {
            get
            {
                return _lengthOfPlank;
            }

            set
            {
                if (_lengthOfPlank == value)
                {
                    return;
                }

                _lengthOfPlank = value;
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

        public double MutationRate
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

        public double PercentageOfElite
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

        public double PercentageOfParentsChosenToSelection
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

        public GeneticAlgorithmParameters(int numberOfPlanksPerPack, int sizeOfPopulation, int numberOfIterations, double mutationRate,
            double percentageOfElite, int lengthOfPlank, double percentageOfParentsChosenToSelection, double promotionRate)
        {
            NumberOfPlanksPerPack = numberOfPlanksPerPack;
            SizeOfPopulation = sizeOfPopulation;
            NumberOfIterations = numberOfIterations;
            MutationRate = mutationRate;
            PercentageOfElite = percentageOfElite;
            LengthOfPlank = lengthOfPlank;
            PercentageOfParentsChosenToSelection = percentageOfParentsChosenToSelection;
            PromotionRate = promotionRate;
        }
    }
}
