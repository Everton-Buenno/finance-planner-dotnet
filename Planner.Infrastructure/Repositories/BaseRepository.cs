using Microsoft.EntityFrameworkCore;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.Data;

namespace Planner.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {

        protected readonly PlannerDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(PlannerDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            entity.Delete();
            await UpdateAsync(entity);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet
                 .AsNoTracking()
                 .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.ChangeTracker.Clear();
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

    }
}
