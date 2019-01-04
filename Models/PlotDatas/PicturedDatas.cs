using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Stock;

namespace WoodCutterCalculator.Models.PlotDatas
{
    public class PicturedDatas : AllPlotDatas
    {
        public StocksToPicture FirstAndLastStocksToPicture { get; set; }
    }
}
