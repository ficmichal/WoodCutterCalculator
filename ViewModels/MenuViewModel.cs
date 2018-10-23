using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.ViewModels.Helpers;

namespace WoodCutterCalculator.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        #region Fields

        private IFrameNavigationService _navigationService;
        private int[] _placedOrder;
        private OrderProcessor _orderProcessor;
        private RelayCommand _goToPlotsCommand;
        private RelayCommand _startCalculations;
        private GeneticAlgorithmParameters _parameters;

        #endregion

        #region Properties

        public object Result { get; set; }
        public int[] PlacedOrder
        {
            get
            {
                return _placedOrder;
            }

            set
            {
                if (_placedOrder == value)
                {
                    return;
                }

                _placedOrder = value;
                RaisePropertyChanged();
            }
        }

        public GeneticAlgorithmParameters Parameters
        {
            get
            {
                return _parameters;
            }

            set
            {
                if (_parameters == value)
                {
                    return;
                }

                _parameters = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand StartCalculations
        {
            get
            {
                return _startCalculations
                    ?? (_startCalculations = new RelayCommand(
                    () =>
                    {
                        Result = _orderProcessor.Calculate(PlacedOrder);
                    }));
            }
        }

        public RelayCommand GoToPlotsCommand
        {
            get
            {
                return _goToPlotsCommand
                    ?? (_goToPlotsCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Plots", Result);
                    }));
            }
        }

        #endregion

        #region Constructor

        public MenuViewModel(IFrameNavigationService navigationService)//, OrderProcessor orderProcessor)
        {
            _navigationService = navigationService;
            Parameters = new GeneticAlgorithmParameters();
            _orderProcessor = new OrderProcessor(Parameters);
            PlacedOrder = new int[9] { 1, 2, 3, 3, 6, 8, 11, 3, 22 };
        }

        #endregion
    }
}
