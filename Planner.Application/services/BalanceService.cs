using Planner.Application.DTOs;
using Planner.Application.DTOs.BalanceDTOs;
using Planner.Application.Helpers;
using Planner.Application.interfaces;
using Planner.Domain.Enums;
using Planner.Domain.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.Application.services
{
    public class BalanceService : IBalanceService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public BalanceService(ITransactionRepository transactionRepository, IBankAccountRepository bankAccountRepository)
        {
            _transactionRepository = transactionRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<ResultViewModel<MonthlyBalanceViewModel>> GetMonthlyBalanceAsync(Guid userId, int year, int month, Guid? bankAccountId = null)
        {
            if (userId == Guid.Empty)
                return ResultViewModel<MonthlyBalanceViewModel>.Error("UserId é obrigatório.");

            if (year < 2000 || year > 3000)
                return ResultViewModel<MonthlyBalanceViewModel>.Error("Ano deve estar entre 2000 e 3000.");

            if (month < 1 || month > 12)
                return ResultViewModel<MonthlyBalanceViewModel>.Error("Mês deve estar entre 1 e 12.");

            var startDate = DateTimeHelper.CreateUtcStartOfMonth(year, month);
            var endDate = DateTimeHelper.CreateUtcEndOfMonth(year, month);

            try
            {
                decimal initialBalance = 0;
                string? bankAccountDescription = null;

                if (bankAccountId.HasValue)
                {
                    var bankAccount = await _bankAccountRepository.GetByIdAsync(bankAccountId.Value);
                    if (bankAccount == null)
                        return ResultViewModel<MonthlyBalanceViewModel>.Error("Conta bancária não encontrada.");
                    
                    if (bankAccount.UserId != userId)
                        return ResultViewModel<MonthlyBalanceViewModel>.Error("Conta bancária não pertence ao usuário.");
                    
                    initialBalance = bankAccount.InitialBalance;
                    bankAccountDescription = bankAccount.Description;
                }
                else
                {
                    var bankAccounts = await _bankAccountRepository.GetAccountsByUserId(userId);
                    initialBalance = bankAccounts?.Sum(ba => ba.InitialBalance) ?? 0;
                }

                var allTransactionsUntilEndDate = await _transactionRepository.GetFilteredAsync(
                    userId: userId,
                    bankAccountId: bankAccountId,
                    categoryId: null,
                    startDate: null, 
                    endDate: endDate, 
                    isPaid: null,
                    ignored: false,
                    isRecurring: null
                );

                var monthTransactions = await _transactionRepository.GetFilteredAsync(
                    userId: userId,
                    bankAccountId: bankAccountId,
                    categoryId: null,
                    startDate: startDate,
                    endDate: endDate,
                    isPaid: null,
                    ignored: false,
                    isRecurring: null
                );

                if (allTransactionsUntilEndDate == null)
                    allTransactionsUntilEndDate = Enumerable.Empty<Domain.Entities.Transaction>();

                if (monthTransactions == null)
                    monthTransactions = Enumerable.Empty<Domain.Entities.Transaction>();

                var monthlyIncome = monthTransactions
                    .Where(t => t.Type == TypeTransaction.Income)
                    .Sum(t => t.Value);

                var monthlyExpense = monthTransactions
                    .Where(t => t.Type == TypeTransaction.Expense || t.Type == TypeTransaction.CreditCardExpense)
                    .Sum(t => t.Value);

                var totalIncomeAccumulated = allTransactionsUntilEndDate
                    .Where(t => t.Type == TypeTransaction.Income && t.IsPaid)
                    .Sum(t => t.Value);

                var totalExpenseAccumulated = allTransactionsUntilEndDate
                    .Where(t => t.IsPaid && (t.Type == TypeTransaction.Expense || t.Type == TypeTransaction.CreditCardExpense) )
                    .Sum(t => t.Value);

                var monthlyBalance = monthlyIncome - monthlyExpense;
                var totalBalance = initialBalance + totalIncomeAccumulated - totalExpenseAccumulated;

                var monthName = CultureInfo.CreateSpecificCulture("pt-BR")
                    .DateTimeFormat.GetMonthName(month);

                var balanceViewModel = new MonthlyBalanceViewModel
                {
                    Income = monthlyIncome,
                    Expense = monthlyExpense,
                    MonthlyBalance = monthlyBalance,
                    InitialBalance = initialBalance,
                    TotalBalance = totalBalance,
                    TotalIncomeAccumulated = totalIncomeAccumulated,
                    TotalExpenseAccumulated = totalExpenseAccumulated,
                    Year = year,
                    Month = month,
                    MonthName = monthName,
                    StartDate = startDate,
                    EndDate = endDate,
                    BankAccountDescription = bankAccountDescription
                };

                return ResultViewModel<MonthlyBalanceViewModel>.Success(balanceViewModel, "Balanço calculado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel<MonthlyBalanceViewModel>.Error($"Erro ao calcular balanço: {ex.Message}");
            }
        }
    }
}