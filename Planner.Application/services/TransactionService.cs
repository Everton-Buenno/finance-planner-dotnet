using Planner.Application.DTOs.TransactionDTOs;
using Planner.Application.DTOs;
using Planner.Application.Helpers;
using Planner.Application.interfaces;
using Planner.Domain.Entities;
using Planner.Domain.Enums;
using Planner.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.Application.services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBankAccountService _bankAccountService;
        private readonly ICreditCardRepository _creditCardRepository;

        public TransactionService(ITransactionRepository transactionRepository, IBankAccountService bankAccountService, ICreditCardRepository creditCardRepository)
        {
            _transactionRepository = transactionRepository;
            _bankAccountService = bankAccountService;
            _creditCardRepository = creditCardRepository;
        }

        public async Task<ResultViewModel<IEnumerable<TransactionViewModel>>> GetFilteredAsync(Guid userId, Guid? bankAccountId, Guid? categoryId, DateTime? startDate, DateTime? endDate, bool? isPaid, bool? ignored, bool? isRecurring)
        {
            DateTime? utcStartDate = DateTimeHelper.EnsureUtc(startDate);
            DateTime? utcEndDate = DateTimeHelper.EnsureUtc(endDate);

            var res = await _transactionRepository.GetFilteredAsync(userId, bankAccountId, categoryId, utcStartDate, utcEndDate, isPaid, ignored, isRecurring);
            if (res is null)
                return ResultViewModel<IEnumerable<TransactionViewModel>>.Error("Nenhuma transação encontrada com os filtros fornecidos.");

            var transactions = new List<TransactionViewModel>();
            foreach (var t in res)
            {
                var bankNameResult = await _bankAccountService.GetBankNameByBankIdAsync(t.Account.BankId);

                transactions.Add(new TransactionViewModel
                {
                    Id = t.Id,
                    DateTransaction = t.DateTransaction,
                    Value = t.Value,
                    Description = t.Description,
                    IsPaid = t.IsPaid,
                    Ignored = t.Ignored,
                    Type = t.Type,
                    CategoryName = t.Category?.Name ?? "Sem Categoria",
                    CategoryIcon = t.Category?.Icon ?? "default-icon.png",
                    CategoryId = t.CategoryId,
                    BankName = bankNameResult?.Data ?? "-",
                    AccountId = t.AccountId,
                    CreditCardId = t.CreditCardId,
                    CreditCardDescription = t.CreditCard?.Description
                });
            }
            return ResultViewModel<IEnumerable<TransactionViewModel>>.Success(transactions);
        }

        public async Task<ResultViewModel> AddAsync(TransactionInputModel input)
        {
            if (input.Type == TypeTransaction.CreditCardExpense)
            {
                if (!input.CreditCardId.HasValue)
                    return ResultViewModel.Error("Cartão de crédito é obrigatório para transações do tipo CreditCardExpense.");

                var creditCard = await _creditCardRepository.GetByIdAsync(input.CreditCardId.Value);
                if (creditCard == null)
                    return ResultViewModel.Error("Cartão de crédito não encontrado.");

                if (!creditCard.IsActive)
                    return ResultViewModel.Error("Cartão de crédito está inativo.");
            }

            string message = string.Empty;
            bool isTransfer = input.Type == TypeTransaction.Transfer && input.AccountDestinationId != null;
            if (isTransfer)
            {
                message = await HandleTransfer(input);
            }
            else
            {
                message = await HandleRepetition(input);
            }
            return ResultViewModel.Success(message);
        }

        private static DateTime CalculateRepeatDateByType(RepeatType recurrenceType, int index, DateTime dateTransaction)
        {
            if (index == 0)
                return dateTransaction;
            switch (recurrenceType)
            {
                case RepeatType.Daily:
                    return dateTransaction.AddDays(index);
                case RepeatType.Weekly:
                    return dateTransaction.AddDays(7 * index);
                case RepeatType.Monthly:
                    return dateTransaction.AddMonths(index);
                case RepeatType.Yearly:
                    return dateTransaction.AddYears(index);
                default:
                    return dateTransaction;
            }
        }

        private async Task<string> HandleTransfer(TransactionInputModel input)
        {
            var utcDate = DateTimeHelper.EnsureUtc(input.DateTransaction);

            var transactionAccountOrigin = new Transaction(
                utcDate,
                input.Value,
                input.Description ?? "Transferência entre conta",
                TypeTransaction.Expense,
                input.AccountId,
                Guid.Empty,
                input.IsPaid,
                input.Ignored,
                null,
                RepeatType.None
            );
            var transactionAccountDestination = new Transaction(
                utcDate,
                input.Value,
                input.Description ?? "Transferência entre conta",
                TypeTransaction.Income,
                input.AccountDestinationId ?? Guid.Empty,
                input.CategoryId ?? Guid.Empty,
                input.IsPaid,
                input.Ignored,
                null,
                RepeatType.None
            );
            await _transactionRepository.AddAsync(transactionAccountOrigin);
            await _transactionRepository.AddAsync(transactionAccountDestination);
            return "Transferência realizada com sucesso.";
        }

        private async Task<string> HandleRepetition(TransactionInputModel input)
        {
            Guid? originId = null;
            var baseUtcDate = DateTimeHelper.EnsureUtc(input.DateTransaction);

            for (int i = 0; i < input.RepeatCount; i++)
            {
                var transactionDate = input.RepeatCount > 1 && input.RepeatType.HasValue
                    ? DateTimeHelper.EnsureUtc(CalculateRepeatDateByType(input.RepeatType.Value, i, baseUtcDate))
                    : baseUtcDate;

                var transaction = new Transaction(
                    dateTransaction: transactionDate,
                    value: input.Value,
                    description: input.Description,
                    type: input.Type,
                    accountId: input.AccountId,
                    categoryId: (Guid)(input.CategoryId ?? Guid.Empty),
                    isPaid: input.IsPaid,
                    ignored: input.Ignored,
                    transactionOrigemId: (input.RepeatCount > 1 && i > 0 && originId.HasValue) ? originId.Value : (Guid?)null,
                    repeatType: input.RepeatType,
                    creditCardId: input.CreditCardId
                );
                var res = await _transactionRepository.AddAsync(transaction);
                if (i == 0)
                {
                    originId = res.Id;
                }
            }
            return "Transação(s) adicionada(s) com sucesso.";
        }

        public async Task<ResultViewModel> UpdateAsync(Guid id, TransactionInputModel input)
        {
            try
            {


                var transaction = await _transactionRepository.GetByIdAsync(id);
                if (transaction == null)
                {
                    return ResultViewModel.Error("Transação não encontrada");
                }

                if (input.Type == TypeTransaction.CreditCardExpense)
                {
                    if (!input.CreditCardId.HasValue)
                        return ResultViewModel.Error("Cartão de crédito é obrigatório para transações do tipo CreditCardExpense.");

                    var creditCard = await _creditCardRepository.GetByIdAsync(input.CreditCardId.Value);
                    if (creditCard == null)
                        return ResultViewModel.Error("Cartão de crédito não encontrado.");

                    if (!creditCard.IsActive)
                        return ResultViewModel.Error("Cartão de crédito está inativo.");
                }

                var utcDate = DateTimeHelper.EnsureUtc(input.DateTransaction);
                transaction.UpdateTransaction(utcDate, input.Value, input.Description, input.IsPaid, input.Ignored, input.Type, input.AccountId, (Guid)(input.CategoryId ?? Guid.Empty), input.CreditCardId);
                await _transactionRepository.UpdateAsync(transaction);
                return ResultViewModel.Success("Transação atualizada com sucesso.");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ResultViewModel> DeleteAsync(Guid id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                return ResultViewModel.Error("Transação não encontrada");
            }
            await _transactionRepository.DeleteAsync(transaction);
            return ResultViewModel.Success("Sucesso");
        }
    }
}