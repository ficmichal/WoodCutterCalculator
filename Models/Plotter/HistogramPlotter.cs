using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.PlotDatas;
using WoodCutterCalculator.Models.Utils;

namespace WoodCutterCalculator.Models.Plotter
{
    public static class HistogramPlotter
    {
        public static Collection<HistogramDataValue> Plot(HistogramData data)
        {
            var plotData = new Collection<HistogramDataValue>();
            var labels = StockDescriptionUtils.GetStockDescription().OrderBy(x => x).ToArray();
            var uselessLabel = labels.Count() - 1;
            for (int i = 0; i < uselessLabel; i++)
            {
                plotData.Add(new HistogramDataValue
                {
                    KindOfStock = labels[i],
                    OrderedStock = data.OrderedStocks[i],
                    CuttedStock = data.CuttedStocks[i]
                });
            }

            return plotData;
        }
    }
}
