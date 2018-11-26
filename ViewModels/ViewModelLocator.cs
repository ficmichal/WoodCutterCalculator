using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using WoodCutterCalculator.Models;
using WoodCutterCalculator.Models.GeneticAlgorithm;
using WoodCutterCalculator.Models.Managers;
using WoodCutterCalculator.Models.Mongo;
using WoodCutterCalculator.Models.Stock;
using WoodCutterCalculator.ViewModels.Helpers;

namespace WoodCutterCalculator.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            //ViewModels
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MenuViewModel>();
            SimpleIoc.Default.Register<PlotsViewModel>();
            //Processors
            SimpleIoc.Default.Register<OrderProcessor>();
            SimpleIoc.Default.Register<StockWarehouseProcessor>();
            //Managers
            SimpleIoc.Default.Register<IMongoDBManager, MongoDBManager>();
            SimpleIoc.Default.Register<ISettingsManager, SettingsManager>();
            SetupNavigation();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public MenuViewModel MenuViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MenuViewModel>();
            }
        }

        public PlotsViewModel PlotsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlotsViewModel>();
            }
        }

        private static void SetupNavigation()
        {
            var navigationService = new FrameNavigationService();
            navigationService.Configure("Menu", new Uri("../Views/Menu.xaml", UriKind.Relative));
            navigationService.Configure("Plots", new Uri("../Views/Plots.xaml", UriKind.Relative));
            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
        }

        public static void ResetPlotsViewModel()
        {
            SimpleIoc.Default.Unregister<PlotsViewModel>();
            SimpleIoc.Default.Register<PlotsViewModel>();
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}