using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using WoodCutterCalculator.Models;
using WoodCutterCalculator.Models.Plotter;
using WoodCutterCalculator.ViewModels.Helpers;

namespace WoodCutterCalculator.ViewModels
{
    public class PlotsViewModel : ViewModelBase
    {
        #region Fields

        private IFrameNavigationService _navigationService;

        #endregion

        #region Properties

        public Collection<CollectionDataValue> Data { get; set; }

        #endregion

        #region Constructor

        public PlotsViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            Data = Plotter.Plot(_navigationService.Parameter as double[]);
        }

        #endregion
    }
}
