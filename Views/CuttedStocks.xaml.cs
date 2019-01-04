using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WoodCutterCalculator.ViewModels;

namespace WoodCutterCalculator.Views
{
    /// <summary>
    /// Interaction logic for CuttedStocks.xaml
    /// </summary>
    public partial class CuttedStocks : Page
    {
        public CuttedStocks()
        {
            InitializeComponent();
            Loaded += CuttedStocksViewModelReset;
        }

        private void CuttedStocksViewModelReset(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.ResetCuttedStocksViewModel();
        }
    }
}
