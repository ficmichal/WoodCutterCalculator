using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using WoodCutterCalculator.Models;
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
            //Data = _navigationService.Parameter as Collection<CollectionDataValue>;
            Data = new Collection<CollectionDataValue>(_navigationService.Parameter as Collection<CollectionDataValue>);
        }

        #endregion
    }
}
