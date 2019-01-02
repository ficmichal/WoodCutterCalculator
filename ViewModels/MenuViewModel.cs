using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using WoodCutterCalculator.Models;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.PlotDatas;
using WoodCutterCalculator.Models.Stock;
using WoodCutterCalculator.ViewModels.Helpers;

namespace WoodCutterCalculator.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        #region Fields

        private IFrameNavigationService _navigationService;
        private OrderProcessor _orderProcessor;
        private StockWarehouseProcessor _stockWarehouseProcessor;

        private GeneticAlgorithmParameters _parameters;
        private ObservableCollection<int> _placedOrder;

        private RelayCommand _goToPlotsCommand;
        private RelayCommand _startCalculations;
        private RelayCommand _fillPlankWarehouse;


        #endregion

        #region Properties

        public AllPlotDatas Result { get; set; }
        public ObservableCollection<int> PlacedOrder
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
                        Result = _orderProcessor.Create(Parameters)
                            .Calculate(PlacedOrder);
                    }));
            }
        }

        public RelayCommand FillPlankWarehouse
        {
            get
            {
                return _fillPlankWarehouse
                    ?? (_fillPlankWarehouse = new RelayCommand(
                    () =>
                    {
                        _stockWarehouseProcessor.Create(Parameters)
                            .SeedStockWarehouse();
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

        public MenuViewModel(IFrameNavigationService navigationService, 
            OrderProcessor orderProcessor, StockWarehouseProcessor stockWarehouseProcessor)
        {
            _navigationService = navigationService;
            _orderProcessor = orderProcessor;
            _stockWarehouseProcessor = stockWarehouseProcessor;

            Parameters = new GeneticAlgorithmParameters();
            PlacedOrder = new ObservableCollection<int> { 10, 20, 30, 10, 20, 30, 10, 20, 30 };
        }

        #endregion
    }
}
