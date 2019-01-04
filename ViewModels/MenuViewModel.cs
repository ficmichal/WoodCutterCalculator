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
        private RelayCommand _goToPicturedStocks;

        private int _sizeOfWarehouse;
        private string _idOfOrderToPlot;

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

        public int SizeOfWarehouse
        {
            get
            {
                return _sizeOfWarehouse;
            }

            set
            {
                if (_sizeOfWarehouse == value)
                {
                    return;
                }

                _sizeOfWarehouse = value;
                RaisePropertyChanged();
            }
        }

        public string IdOfOrderToPlot
        {
            get
            {
                return _idOfOrderToPlot;
            }

            set
            {
                if (_idOfOrderToPlot == value)
                {
                    return;
                }

                _idOfOrderToPlot = value;
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
                        Result = _orderProcessor.Create(Parameters, IdOfOrderToPlot)
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
                        _stockWarehouseProcessor.Create(Parameters, SizeOfWarehouse)
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

        public RelayCommand GoToPicturedStocks
        {
            get
            {
                return _goToPicturedStocks
                    ?? (_goToPicturedStocks = new RelayCommand(
                    () =>
                    {
                        Result = _orderProcessor.Create(Parameters).Calculate(PlacedOrder, true);
                        _navigationService.NavigateTo("CuttedStocks", Result);
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

            Parameters = new GeneticAlgorithmParameters(10, 30, 60, 0.1, 0.1, 20, 0.8, 6);
            PlacedOrder = new ObservableCollection<int> { 30, 20, 10, 30, 20, 10, 30, 20, 10 };
        }

        #endregion
    }
}
