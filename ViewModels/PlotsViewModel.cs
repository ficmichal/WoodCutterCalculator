using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Linq;
using WoodCutterCalculator.Models;
using WoodCutterCalculator.Models.PlotDatas;
using WoodCutterCalculator.Models.Plotter;
using WoodCutterCalculator.Repositories;
using WoodCutterCalculator.ViewModels.Helpers;

namespace WoodCutterCalculator.ViewModels
{
    public class PlotsViewModel : ViewModelBase
    {
        #region Fields

        private IFrameNavigationService _navigationService;
        private IAllPlotDatasRepository _allPlotDatasRepository;
        private double _bestSolution;

        #endregion

        #region Properties

        public Collection<CollectionDataValue> Data { get; set; }
        public Collection<HistogramDataValue> StocksData { get; set; }
        public AlgorithmParameters UsedAlgorithmParameters { get; set; }

        public double BestSolution
        {
            get
            {
                return _bestSolution;
            }

            set
            {
                if (_bestSolution == value)
                {
                    return;
                }

                _bestSolution = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor

        public PlotsViewModel(IFrameNavigationService navigationService, IAllPlotDatasRepository allPlotDatasRepository)
        {
            _navigationService = navigationService;
            _allPlotDatasRepository = allPlotDatasRepository;
            var allPlotDatas = _navigationService.Parameter as AllPlotDatas;

            Data = Plotter.Plot(allPlotDatas.HistoryOfLearning);
            StocksData = HistogramPlotter.Plot(allPlotDatas.HistogramData);
            UsedAlgorithmParameters = allPlotDatas.AlgorithmParameters;
            BestSolution = allPlotDatas.HistoryOfLearning.ToList().Max();

            SavePlots(allPlotDatas);
        }

        private void SavePlots(AllPlotDatas plotDataToSave)
        {
            _allPlotDatasRepository.Add(plotDataToSave);
        }

        #endregion
    }
}
