using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Enums;
using WoodCutterCalculator.Models.PlotDatas;
using WoodCutterCalculator.Models.Plotter;
using WoodCutterCalculator.Models.Stock;
using WoodCutterCalculator.ViewModels.Helpers;

namespace WoodCutterCalculator.ViewModels
{
    public class CuttedStocksViewModel : ViewModelBase
    {
        #region Fields

        private IFrameNavigationService _navigationService;
        private RelayCommand _goBackToMenu;

        private string _profitAfterFirstIteration;
        private string _profitAfterLastIteration;
        private int[] _cuttedStocksAfterFirstIteration;
        private int[] _cuttedStocksAfterLastIteration;

        #endregion

        #region Properties

        public ObservableCollection<MinorElementOfStock> FirstIteration { get; set; }
        public ObservableCollection<MinorElementOfStock> LastIteration { get; set; }
        public PicturedDatas PicturedDatas { get; set; }

        public string ProfitAfterFirstIteration
        {
            get
            {
                return _profitAfterFirstIteration;
            }

            set
            {
                if (_profitAfterFirstIteration == value)
                {
                    return;
                }

                _profitAfterFirstIteration = value;
                RaisePropertyChanged();
            }
        }

        public string ProfitAfterLastIteration
        {
            get
            {
                return _profitAfterLastIteration;
            }

            set
            {
                if (_profitAfterLastIteration == value)
                {
                    return;
                }

                _profitAfterLastIteration = value;
                RaisePropertyChanged();
            }
        }

        public int[] CuttedStocksAfterFirstIteration
        {
            get
            {
                return _cuttedStocksAfterFirstIteration;
            }

            set
            {
                if (_cuttedStocksAfterFirstIteration == value)
                {
                    return;
                }

                _cuttedStocksAfterFirstIteration = value;
                RaisePropertyChanged();
            }
        }

        public int[] CuttedStocksAfterLastIteration
        {
            get
            {
                return _cuttedStocksAfterLastIteration;
            }

            set
            {
                if (_cuttedStocksAfterLastIteration == value)
                {
                    return;
                }

                _cuttedStocksAfterLastIteration = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GoBackToMenu
        {
            get
            {
                return _goBackToMenu
                    ?? (_goBackToMenu = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Menu");
                    }));
            }
        }

        #endregion

        public CuttedStocksViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            PicturedDatas = (PicturedDatas)_navigationService.Parameter;
            CreatePictures();
            GetValuesOfStocks();
            GetCuttesStocks();
        }

        private void CreatePictures()
        {
            FirstIteration = CuttedStockPictureCreator.CreatePicture(PicturedDatas, IterationsEnum.FirstIteration);
            LastIteration = CuttedStockPictureCreator.CreatePicture(PicturedDatas, IterationsEnum.LastIteration);
        }

        private void GetValuesOfStocks()
        {
            ProfitAfterFirstIteration = $"Zysk: {PicturedDatas.FirstAndLastStocksToPicture.Values[0]}";
            ProfitAfterLastIteration = $"Zysk: {PicturedDatas.FirstAndLastStocksToPicture.Values[1]}";
        }

        private void GetCuttesStocks()
        {
            CuttedStocksAfterFirstIteration = PicturedDatas.FirstAndLastStocksToPicture.CuttedStocks[0];
            CuttedStocksAfterLastIteration = PicturedDatas.FirstAndLastStocksToPicture.CuttedStocks[1];
        }
    }
}
