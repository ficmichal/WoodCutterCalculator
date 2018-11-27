using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WoodCutterCalculator.Models.PlotDatas;

namespace WoodCutterCalculator.Repositories
{
    public interface IHistoryOfLearningPlotRepository
    {
        IEnumerable<HistoryOfLearningPlot> Find(Expression<Func<HistoryOfLearningPlot, bool>> predicate);
        void Add(HistoryOfLearningPlot entity);
    }
}
