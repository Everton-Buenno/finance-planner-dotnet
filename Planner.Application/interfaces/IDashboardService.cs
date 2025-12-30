using Planner.Application.DTOs;
using Planner.Application.DTOs.DashboardDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planner.Application.interfaces
{
    public interface IDashboardService
    {
        Task<ResultViewModel<List<CategorySummaryViewModel>>> GetExpensesByCategoryAsync(Guid userId, int year, int month);
        Task<ResultViewModel<List<CategorySummaryViewModel>>> GetIncomeByCategoryAsync(Guid userId, int year, int month);
        Task<ResultViewModel<DashboardSummaryViewModel>> GetDashboardSummaryAsync(Guid userId, int year, int month);
        Task<ResultViewModel<List<CreditCardDashboardViewModel>>> GetCreditCardsSummaryAsync(Guid userId, int year, int month);
    }
}
