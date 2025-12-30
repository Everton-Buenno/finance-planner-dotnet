using Planner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planner.Domain.Interfaces
{
    public interface ICreditCardRepository : IBaseRepository<CreditCard>
    {
        Task<IEnumerable<CreditCard>> GetByUserIdAsync(Guid userId);

        Task<IEnumerable<CreditCard>> GetByAccountIdAsync(Guid accountId);

        Task<IEnumerable<CreditCard>> GetActiveByUserIdAsync(Guid userId);

        Task<bool> ExistsByDescriptionAndUserIdAsync(string description, Guid userId, Guid? excludeId = null);

        Task<CreditCard> GetByIdWithTransactionsAsync(Guid id);
    }
}