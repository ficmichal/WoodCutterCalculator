using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.PlotDatas;

namespace WoodCutterCalculator.Repositories
{
    public interface IAllPlotDatasRepository
    {
        IEnumerable<AllPlotDatas> Find(Expression<Func<AllPlotDatas, bool>> predicate);
        AllPlotDatas GetByObjectId(ObjectId id);
        AllPlotDatas FindOrderDataWithTheSameAlgorithmParameters(string orderId, AlgorithmParameters algorithmParameters);
        void Add(AllPlotDatas entity);
    }
}
