using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodCutterCalculator.Models.Plotter
{
    public static class Plotter
    {
        public static Collection<CollectionDataValue> Plot(double[] toPlotData)
        {
            var plotData = new Collection<CollectionDataValue>();

            for (int i = 0; i < toPlotData.Length; i++)
            {
                plotData.Add(new CollectionDataValue
                {
                    XData = i + 1,
                    YData = toPlotData[i] / 100
                });
            }

            return plotData;
        }
    }
}
