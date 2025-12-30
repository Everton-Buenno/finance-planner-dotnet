using Planner.Application.DTOs;
using Planner.Application.DTOs.DashboardDTOs;
using Planner.Application.Helpers;
using Planner.Application.interfaces;
using Planner.Domain.Enums;
using Planner.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.Application.services
{
    public class DashboardService : IDashboardService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IBankAccountService _bankAccountService;

        public DashboardService(
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            ICreditCardRepository creditCardRepository,
            IBankAccountService bankAccountService)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _creditCardRepository = creditCardRepository;
            _bankAccountService = bankAccountService;
        }

        public async Task<ResultViewModel<List<CategorySummaryViewModel>>> GetExpensesByCategoryAsync(Guid userId, int year, int month)
        {
            var startDate = DateTimeHelper.CreateUtcStartOfMonth(year, month);
            var endDate = DateTimeHelper.CreateUtcEndOfMonth(year, month);

            var transactions = await _transactionRepository.GetFilteredAsync(
                userId,
                null,
                null,
                startDate,
                endDate,
                null,
                false,
                null);

            if (transactions == null || !transactions.Any())
            {
                return ResultViewModel<List<CategorySummaryViewModel>>.Success(
                    new List<CategorySummaryViewModel>(),
                    "Nenhuma transação encontrada para o período especificado.");
            }

            var expenses = transactions
                .Where(t => t.Type == TypeTransaction.Expense || t.Type == TypeTransaction.CreditCardExpense)
                .ToList();

            if (!expenses.Any())
            {
                return ResultViewModel<List<CategorySummaryViewModel>>.Success(
                    new List<CategorySummaryViewModel>(),
                    "Nenhuma despesa encontrada para o período especificado.");
            }

            var totalExpenses = expenses.Sum(t => t.Value);

            var groupedByCategory = expenses
                .GroupBy(t => new
                {
                    CategoryId = t.CategoryId ?? Guid.Empty,
                    CategoryName = t.Category?.Name ?? "Sem Categoria",
                    CategoryIcon = t.Category?.Icon ?? "pi pi-question-circle",
                    CategoryColor = t.Category?.Color ?? "#D3D3D3"
                })
                .Select(g => new CategorySummaryViewModel
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    CategoryIcon = g.Key.CategoryIcon,
                    CategoryColor = g.Key.CategoryColor,
                    TotalValue = g.Sum(t => t.Value),
                    TransactionCount = g.Count(),
                    Percentage = totalExpenses > 0 ? (g.Sum(t => t.Value) / totalExpenses) * 100 : 0
                })
                .OrderByDescending(c => c.TotalValue)
                .ToList();

            return ResultViewModel<List<CategorySummaryViewModel>>.Success(
                groupedByCategory,
                "Despesas por categoria obtidas com sucesso.");
        }

        public async Task<ResultViewModel<List<CategorySummaryViewModel>>> GetIncomeByCategoryAsync(Guid userId, int year, int month)
        {
            var startDate = DateTimeHelper.CreateUtcStartOfMonth(year, month);
            var endDate = DateTimeHelper.CreateUtcEndOfMonth(year, month);

            var transactions = await _transactionRepository.GetFilteredAsync(
                userId,
                null,
                null,
                startDate,
                endDate,
                null,
                false,
                null);

            if (transactions == null || !transactions.Any())
            {
                return ResultViewModel<List<CategorySummaryViewModel>>.Success(
                    new List<CategorySummaryViewModel>(),
                    "Nenhuma transação encontrada para o período especificado.");
            }

            var income = transactions
                .Where(t => t.Type == TypeTransaction.Income)
                .ToList();

            if (!income.Any())
            {
                return ResultViewModel<List<CategorySummaryViewModel>>.Success(
                    new List<CategorySummaryViewModel>(),
                    "Nenhuma receita encontrada para o período especificado.");
            }

            var totalIncome = income.Sum(t => t.Value);

            var groupedByCategory = income
                .GroupBy(t => new
                {
                    CategoryId = t.CategoryId ?? Guid.Empty,
                    CategoryName = t.Category?.Name ?? "Sem Categoria",
                    CategoryIcon = t.Category?.Icon ?? "pi pi-question-circle",
                    CategoryColor = t.Category?.Color ?? "#D3D3D3"
                })
                .Select(g => new CategorySummaryViewModel
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    CategoryIcon = g.Key.CategoryIcon,
                    CategoryColor = g.Key.CategoryColor,
                    TotalValue = g.Sum(t => t.Value),
                    TransactionCount = g.Count(),
                    Percentage = totalIncome > 0 ? (g.Sum(t => t.Value) / totalIncome) * 100 : 0
                })
                .OrderByDescending(c => c.TotalValue)
                .ToList();

            return ResultViewModel<List<CategorySummaryViewModel>>.Success(
                groupedByCategory,
                "Receitas por categoria obtidas com sucesso.");
        }

        public async Task<ResultViewModel<DashboardSummaryViewModel>> GetDashboardSummaryAsync(Guid userId, int year, int month)
        {
            var startDate = DateTimeHelper.CreateUtcStartOfMonth(year, month);
            var endDate = DateTimeHelper.CreateUtcEndOfMonth(year, month);

            var transactions = await _transactionRepository.GetFilteredAsync(
                userId,
                null,
                null,
                startDate,
                endDate,
                null,
                false,
                null);

            var expensesByCategory = await GetExpensesByCategoryAsync(userId, year, month);
            var incomeByCategory = await GetIncomeByCategoryAsync(userId, year, month);

            var monthlyProjection = new MonthlyProjectionViewModel
            {
                Year = year,
                Month = month,
                TotalTransactions = transactions?.Count() ?? 0
            };

            if (transactions != null && transactions.Any())
            {
                var income = transactions.Where(t => t.Type == TypeTransaction.Income);
                var expenses = transactions.Where(t => t.Type == TypeTransaction.Expense || t.Type == TypeTransaction.CreditCardExpense);

                monthlyProjection.TotalIncome = income.Sum(t => t.Value);
                monthlyProjection.TotalExpenses = expenses.Sum(t => t.Value);
                monthlyProjection.Balance = monthlyProjection.TotalIncome - monthlyProjection.TotalExpenses;

                monthlyProjection.PaidIncome = income.Where(t => t.IsPaid).Sum(t => t.Value);
                monthlyProjection.PaidExpenses = expenses.Where(t => t.IsPaid).Sum(t => t.Value);
                monthlyProjection.CurrentBalance = monthlyProjection.PaidIncome - monthlyProjection.PaidExpenses;

                monthlyProjection.PendingIncome = monthlyProjection.TotalIncome - monthlyProjection.PaidIncome;
                monthlyProjection.PendingExpenses = monthlyProjection.TotalExpenses - monthlyProjection.PaidExpenses;
            }

            var summary = new DashboardSummaryViewModel
            {
                ExpensesByCategory = expensesByCategory.Data ?? new List<CategorySummaryViewModel>(),
                IncomeByCategory = incomeByCategory.Data ?? new List<CategorySummaryViewModel>(),
                MonthlyProjection = monthlyProjection
            };

            return ResultViewModel<DashboardSummaryViewModel>.Success(
                summary,
                "Resumo do dashboard obtido com sucesso.");
        }

        public async Task<ResultViewModel<List<CreditCardDashboardViewModel>>> GetCreditCardsSummaryAsync(Guid userId, int year, int month)
        {
            try
            {
                var creditCards = await _creditCardRepository.GetActiveByUserIdAsync(userId);

                if (creditCards == null || !creditCards.Any())
                {
                    return ResultViewModel<List<CreditCardDashboardViewModel>>.Success(
                        new List<CreditCardDashboardViewModel>(),
                        "Nenhum cartão de crédito ativo encontrado.");
                }

                var referenceDate = new DateTime(year, month, 1);
                var dashboardCards = new List<CreditCardDashboardViewModel>();

                foreach (var card in creditCards)
                {
                    var bankNameResult = await _bankAccountService.GetBankNameByBankIdAsync(card.Account.BankId);
                    var availableLimit = card.GetAvailableLimit();
                    var usedLimit = card.CreditLimit - availableLimit;
                    var usagePercentage = card.CreditLimit > 0 ? Math.Round((usedLimit / card.CreditLimit) * 100, 2) : 0;
                    var currentInvoiceAmount = card.GetCurrentInvoiceAmount(referenceDate);
                    var currentInvoiceDueDate = card.GetCurrentInvoiceDueDate(referenceDate);
                    var daysUntilDue = (currentInvoiceDueDate.Date - DateTime.UtcNow.Date).Days;
                    var isOverdue = DateTime.UtcNow.Date > currentInvoiceDueDate.Date && currentInvoiceAmount > 0;
                    var isPaid = card.IsInvoicePaid(referenceDate);

                    string status;
                    string statusColor;

                    if (!card.IsActive)
                    {
                        status = "Inativo";
                        statusColor = "#6c757d";
                    }
                    else if (isOverdue && !isPaid)
                    {
                        status = "Fatura Vencida";
                        statusColor = "#dc3545";
                    }
                    else if (isPaid)
                    {
                        status = "Fatura Paga";
                        statusColor = "#28a745";
                    }
                    else if (usagePercentage >= 90)
                    {
                        status = "Limite Crítico";
                        statusColor = "#dc3545";
                    }
                    else if (usagePercentage >= 70)
                    {
                        status = "Limite Alto";
                        statusColor = "#fd7e14";
                    }
                    else if (usagePercentage >= 50)
                    {
                        status = "Limite Médio";
                        statusColor = "#ffc107";
                    }
                    else
                    {
                        status = "Normal";
                        statusColor = "#28a745";
                    }

                    dashboardCards.Add(new CreditCardDashboardViewModel
                    {
                        Id = card.Id,
                        Description = card.Description,
                        BankName = bankNameResult?.Data ?? "-",
                        CreditLimit = card.CreditLimit,
                        UsedLimit = usedLimit,
                        AvailableLimit = availableLimit,
                        UsagePercentage = usagePercentage,
                        CurrentInvoiceAmount = currentInvoiceAmount,
                        CurrentInvoiceDueDate = currentInvoiceDueDate,
                        DaysUntilDue = daysUntilDue,
                        IsInvoicePaid = isPaid,
                        IsOverdue = isOverdue && !isPaid,
                        IsActive = card.IsActive,
                        Status = status,
                        StatusColor = statusColor
                    });
                }

                return ResultViewModel<List<CreditCardDashboardViewModel>>.Success(
                    dashboardCards.OrderByDescending(c => c.UsagePercentage).ToList(),
                    "Resumo de cartões de crédito obtido com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel<List<CreditCardDashboardViewModel>>.Error($"Erro ao buscar resumo de cartões de crédito: {ex.Message}");
            }
        }
    }
}
