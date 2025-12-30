using Planner.Application.DTOs.TransactionDTOs;
using Planner.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planner.Application.interfaces
{
    public interface ITransactionService
    {
        Task<ResultViewModel<IEnumerable<TransactionViewModel>>> GetFilteredAsync(Guid userId, Guid? bankAccountId, Guid? categoryId, DateTime? startDate, DateTime? endDate, bool? isPaid, bool? ignored, bool? isRecurring);
        Task<ResultViewModel> AddAsync(TransactionInputModel input);
        Task<ResultViewModel> UpdateAsync(Guid id, TransactionInputModel input);
        Task<ResultViewModel> DeleteAsync(Guid id);
    }
} 