using Microsoft.EntityFrameworkCore;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Domain.ValueObjects;
using Planner.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PlannerDbContext context):base(context)
        {
            
        }
        public async Task<bool> ExistsByEmailAsync(Email email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email == email );
        }

        public async Task<User> GetByEmailAsync(Email email)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email );
        }
     
    }
}
