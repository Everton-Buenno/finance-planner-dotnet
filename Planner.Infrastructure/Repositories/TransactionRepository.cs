using Microsoft.EntityFrameworkCore;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.Data;

namespace Planner.Infrastructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(PlannerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Transaction>> GetFilteredAsync(
            Guid userId,
            Guid? bankAccountId = null, 
            Guid? categoryId = null, 
            DateTime? startDate = null,
            DateTime? endDate = null, 
            bool? isPaid = null,
            bool? ignored = null, 
            bool? isRecurring = null)
        {
            IQueryable<Transaction> query = _dbSet.AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.Account);

            query.Where(t => t.Account.UserId == userId);

            if (bankAccountId.HasValue)
                query = query.Where(t => t.AccountId == bankAccountId.Value);


            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if(startDate.HasValue)
                query = query.Where(t => t.DateTransaction >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.DateTransaction <= endDate.Value);

            if(isPaid.HasValue)
                query = query.Where(t => t.IsPaid == isPaid.Value);

            if(ignored.HasValue)
                query = query.Where(t => t.Ignored == ignored.Value);


            return await query.ToListAsync();
        }
    }
}
