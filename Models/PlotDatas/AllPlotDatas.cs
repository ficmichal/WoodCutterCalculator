using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodCutterCalculator.Models.PlotDatas
{
    public class AllPlotDatas
    {
        public ObjectId Id { get; set; }
        public string OrderId { get; set; }
        public double[] HistoryOfLearning { get; set; }
        public HistogramData HistogramData { get; set; }
        public AlgorithmParameters AlgorithmParameters { get; set; }
        public int NumberOfCuttedPlanks { get; set; }
    }
}
