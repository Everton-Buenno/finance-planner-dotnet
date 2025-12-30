using Microsoft.EntityFrameworkCore;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.Infrastructure.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly PlannerDbContext _context;

        public CreditCardRepository(PlannerDbContext context)
        {
            _context = context;
        }

        public async Task<CreditCard> GetByIdAsync(Guid id)
        {
            return await _context.CreditCards
                .Include(c => c.Account)
                .Include(c => c.Transactions.Where(t => !t.IsDeleted))
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<IEnumerable<CreditCard>> GetAllAsync()
        {
            return await _context.CreditCards
                .Include(c => c.Account)
                .Include(c => c.Transactions.Where(t => !t.IsDeleted))
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<CreditCard> AddAsync(CreditCard entity)
        {
            _context.CreditCards.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(CreditCard entity)
        {
            _context.CreditCards.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CreditCard entity)
        {
            entity.Delete();
            _context.CreditCards.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CreditCard>> GetByUserIdAsync(Guid userId)
        {
            return await _context.CreditCards
                .Include(c => c.Account)
                .Include(c => c.Transactions.Where(t => !t.IsDeleted))
                .Where(c => c.Account.UserId == userId && !c.IsDeleted)
                .OrderBy(c => c.Description)
                .ToListAsync();
        }

        public async Task<IEnumerable<CreditCard>> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.CreditCards
                .Include(c => c.Account)
                .Include(c => c.Transactions.Where(t => !t.IsDeleted))
                .Where(c => c.AccountId == accountId && !c.IsDeleted)
                .OrderBy(c => c.Description)
                .ToListAsync();
        }

        public async Task<IEnumerable<CreditCard>> GetActiveByUserIdAsync(Guid userId)
        {
            return await _context.CreditCards
                .Include(c => c.Account)
                .Include(c => c.Transactions.Where(t => !t.IsDeleted))
                .Where(c => c.Account.UserId == userId && c.IsActive && !c.IsDeleted)
                .OrderBy(c => c.Description)
                .ToListAsync();
        }

        public async Task<bool> ExistsByDescriptionAndUserIdAsync(string description, Guid userId, Guid? excludeId = null)
        {
            var query = _context.CreditCards
                .Include(c => c.Account)
                .Where(c => c.Account.UserId == userId && 
                           c.Description.ToLower() == description.ToLower() && 
                           !c.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<CreditCard> GetByIdWithTransactionsAsync(Guid id)
        {
            return await _context.CreditCards
                .Include(c => c.Account)
                .Include(c => c.Transactions.Where(t => !t.IsDeleted))
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }
    }
}