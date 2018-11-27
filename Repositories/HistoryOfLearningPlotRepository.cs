using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Mongo;
using WoodCutterCalculator.Models.PlotDatas;

namespace WoodCutterCalculator.Repositories
{
    public class HistoryOfLearningPlotRepository : IHistoryOfLearningPlotRepository
    {
        private IMongoDBManager _mongoDBManager;
        private IMongoCollection<HistoryOfLearningPlot> Collection;

        public HistoryOfLearningPlotRepository(IMongoDBManager mongoDBManager)
        {
            _mongoDBManager = mongoDBManager;
            Collection = _mongoDBManager.HistoryOfLearningPlot;
        }

        public void Add(HistoryOfLearningPlot entity)
            => Collection.InsertOne(entity);

        public IEnumerable<HistoryOfLearningPlot> Find(Expression<Func<HistoryOfLearningPlot, bool>> predicate)
            => Collection.Find(predicate).ToList();
    }
}
