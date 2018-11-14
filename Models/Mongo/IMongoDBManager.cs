using MongoDB.Driver;
using WoodCutterCalculator.Models.Planks;

namespace WoodCutterCalculator.Models.Mongo
{
    public interface IMongoDBManager
    {
        IMongoCollection<PlanksToCut> PlanksToCut { get; }
    }
}
