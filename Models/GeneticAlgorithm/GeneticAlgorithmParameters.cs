using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodCutterCalculator.Models.GeneticAlgorithm
{
    public class GeneticAlgorithmParameters : ObservableObject
    {
        #region Fields

        private int _sizeOfPopulation;
        private int _numberOfPlanksPerPack;
        private int _maxPossibleCutsPerPlank;
        private int _coefficientOfCut;
        private int _numberOfIterations;
        private double _mutationRate;
        private double _percentageOfChildrenFromPreviousGeneration;

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

        public int MaxPossibleCutsPerPlank
        {
            get
            {
                return _maxPossibleCutsPerPlank;
            }

            set
            {
                if (_maxPossibleCutsPerPlank == value)
                {
                    return;
                }

                _maxPossibleCutsPerPlank = value;
                RaisePropertyChanged();
            }
        }

        public int CoefficientOfCut
        {
            get
            {
                return _coefficientOfCut;
            }

            set
            {
                if (_coefficientOfCut == value)
                {
                    return;
                }

                _coefficientOfCut = value;
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

        public double PercentageOfChildrenFromPreviousGeneration
        {
            get
            {
                return _percentageOfChildrenFromPreviousGeneration;
            }

            set
            {
                if (_percentageOfChildrenFromPreviousGeneration == value)
                {
                    return;
                }

                _percentageOfChildrenFromPreviousGeneration = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}
