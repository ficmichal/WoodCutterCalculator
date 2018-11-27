using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Planks;

namespace WoodCutterCalculator.Repositories
{
    public interface IPlanksToCutRepository
    {
        Task<PlanksToCut> GetByDateAsync(DateTime date);
        PlanksToCut GetLastOrderInDay(DateTime date);
        PlanksToCut GetLastAdded();
        Task<IEnumerable<PlanksToCut>> FindAsync(Expression<Func<PlanksToCut, bool>> predicate);
        Task AddAsync(PlanksToCut entity);
        void Add(PlanksToCut entity);
        Task UpdateAsync(PlanksToCut entity);
    }
}
