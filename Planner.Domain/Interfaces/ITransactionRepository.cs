

using Planner.Domain.Entities;

namespace Planner.Domain.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetFilteredAsync(
            Guid userId,
            Guid? bankAccountId = null,
            Guid? categoryId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool? isPaid = null,
            bool? ignored = null,
            bool? isRecurring = null
            );
    }
}
