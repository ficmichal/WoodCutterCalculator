using MongoDB.Bson;

namespace WoodCutterCalculator.Models.Planks
{
    public class PlanksToCut
    {
        public ObjectId Id { get; set; }
        public BsonDateTime StartedCuttingDay { get; set; }
        public int[][] Planks { get; set; }
    }
}
