using MongoDB.Driver;
using WoodCutterCalculator.Models.Planks;
using WoodCutterCalculator.Models.PlotDatas;

namespace WoodCutterCalculator.Models.Mongo
{
    public interface IMongoDBManager
    {
        IMongoCollection<PlanksToCut> PlanksToCut { get; }
        IMongoCollection<AllPlotDatas> AllPlotDatas { get; }
    }
}
