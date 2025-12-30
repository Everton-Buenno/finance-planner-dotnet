using Planner.Application.DTOs.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Application.interfaces
{
    public interface IBankAccountService
    {
        ResultViewModel<List<Bank>> GetAllBanks();
        ResultViewModel<List<Bank>> GetBaseBanks();
        Task<ResultViewModel<string>> GetBankNameByBankIdAsync(int bankId);
        Task<ResultViewModel<Bank>> GetBankByBankIdAsync(int bankId);
        Task<ResultViewModel> AddAsync(AddBankAccountInputModel input);
        Task<ResultViewModel> UpdateAsync(UpdateBankAccountInputModel input);
        Task<ResultViewModel> DeleteAsync(Guid id);
        Task<ResultViewModel<List<BankAccountDTO>>> GetAccountsByUserIdAsync(Guid userId);
    }
}
