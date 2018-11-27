﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodCutterCalculator.Models.PlotDatas
{
    public class HistoryOfLearningPlot
    {
        public ObjectId Id { get; set; }
        public string OrderId { get; set; }
        public double[] YDatas { get; set; }
    }
}
