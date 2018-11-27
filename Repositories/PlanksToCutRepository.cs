using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Mongo;
using WoodCutterCalculator.Models.Planks;

namespace WoodCutterCalculator.Repositories
{
    public class PlanksToCutRepository : IPlanksToCutRepository
    {
        private IMongoDBManager _mongoDBManager;
        private IMongoCollection<PlanksToCut> Collection;

        public PlanksToCutRepository(IMongoDBManager mongoDBManager)
        {
            _mongoDBManager = mongoDBManager;
            Collection = _mongoDBManager.PlanksToCut;
        }

        public async Task AddAsync(PlanksToCut entity)
            => await Collection.InsertOneAsync(entity);

        public void Add(PlanksToCut entity)
            => Collection.InsertOne(entity);

        public async Task<IEnumerable<PlanksToCut>> FindAsync(Expression<Func<PlanksToCut, bool>> predicate)
            => await Collection.Find(predicate).ToListAsync();

        public async Task<PlanksToCut> GetByDateAsync(DateTime date)
            => await Collection.Find(x => x.StartedCuttingDay.ToUniversalTime().Date == date.ToUniversalTime().Date)
                .FirstOrDefaultAsync();

        public PlanksToCut GetLastAdded()
            => Collection.AsQueryable().OrderByDescending(x => x.StartedCuttingDay).FirstOrDefault();

        public async Task UpdateAsync(PlanksToCut entity)
            => await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);

        public PlanksToCut GetLastOrderInDay(DateTime date)
            => Collection.Find(x => x.StartedCuttingDay == date).SortByDescending(x => x.OrderId).FirstOrDefault();
    }
}
