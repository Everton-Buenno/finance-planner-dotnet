using Microsoft.EntityFrameworkCore;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Infrastructure.Repositories
{
    public class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(PlannerDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<BankAccount>> GetAccountsByUserId(Guid userId)
        {
            return await _dbSet
                .Include(t => t.Transactions.Where(x => !x.IsDeleted && !x.Ignored))
                .Where(ba => ba.UserId == userId)
                .ToListAsync();
        }
    }
}
