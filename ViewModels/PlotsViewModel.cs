using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
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
        private IHistoryOfLearningPlotRepository _historyOfLearningPlotRepository;

        #endregion

        #region Properties

        public Collection<CollectionDataValue> Data { get; set; }

        #endregion

        #region Constructor

        public PlotsViewModel(IFrameNavigationService navigationService, IHistoryOfLearningPlotRepository historyOfLearningPlotRepository)
        {
            _navigationService = navigationService;
            _historyOfLearningPlotRepository = historyOfLearningPlotRepository;

            var allPlotDatas = _navigationService.Parameter as AllPlotDatas;
            Data = Plotter.Plot(allPlotDatas.HistoryOfLearning);
            SavePlots(allPlotDatas);
        }

        private void SavePlots(AllPlotDatas plotDataToSave)
        {
            _historyOfLearningPlotRepository.Add(new HistoryOfLearningPlot
                { OrderId = plotDataToSave.OrderId, YDatas = plotDataToSave.HistoryOfLearning });
        }

        #endregion
    }
}
