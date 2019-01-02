using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Extensions;
using WoodCutterCalculator.Models.Mongo;
using WoodCutterCalculator.Models.PlotDatas;

namespace WoodCutterCalculator.Repositories
{
    public class AllPlotDatasRepository : IAllPlotDatasRepository
    {
        private IMongoDBManager _mongoDBManager;
        private IMongoCollection<AllPlotDatas> Collection;

        public AllPlotDatasRepository(IMongoDBManager mongoDBManager)
        {
            _mongoDBManager = mongoDBManager;
            Collection = _mongoDBManager.AllPlotDatas;
        }

        public void Add(AllPlotDatas entity)
            => Collection.InsertOne(entity);

        public IEnumerable<AllPlotDatas> Find(Expression<Func<AllPlotDatas, bool>> predicate)
            => Collection.Find(predicate).ToList();

        public AllPlotDatas FindOrderDataWithTheSameAlgorithmParameters(string orderId, AlgorithmParameters algorithmParameters)
        {
            var predicatedOrders = Collection.Find(x => x.OrderId == orderId && x.AlgorithmParameters == algorithmParameters).ToEnumerable();

            return new AllPlotDatas
            {
                AlgorithmParameters = algorithmParameters,
                OrderId = orderId,
                HistoryOfLearning = predicatedOrders.Select(x => x.HistoryOfLearning).MultiAverage()
            };
        }
    }
}
