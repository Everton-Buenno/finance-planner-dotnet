using Planner.Application.DTOs;
using Planner.Application.DTOs.BalanceDTOs;
using System;
using System.Threading.Tasks;

namespace Planner.Application.interfaces
{
    public interface IBalanceService
    {
        Task<ResultViewModel<MonthlyBalanceViewModel>> GetMonthlyBalanceAsync(Guid userId, int year, int month, Guid? bankAccountId = null);
    }
}