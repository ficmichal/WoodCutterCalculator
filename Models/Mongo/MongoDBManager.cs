using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using WoodCutterCalculator.Models.Managers;
using WoodCutterCalculator.Models.Planks;

namespace WoodCutterCalculator.Models.Mongo
{
    public class MongoDBManager : IMongoDBManager
    {
        private ISettingsManager _settings;
        private readonly IMongoDatabase _database = null;

        public MongoDBManager(ISettingsManager settings)
        {
            _settings = settings;
            try
            {
                var mongoSettings = _settings.GetSettings();

                var client = new MongoClient(mongoSettings.ConnectionString);
                if (client != null)
                {
                    _database = client.GetDatabase(mongoSettings.Database);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public IMongoCollection<PlanksToCut> PlanksToCut => _database.GetCollection<PlanksToCut>("PlanksToCut");
    }
}
