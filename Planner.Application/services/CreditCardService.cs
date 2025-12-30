using Planner.Application.DTOs;
using Planner.Application.DTOs.CreditCardDTOs;
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
    public class CreditCardService : ICreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankAccountService _bankAccountService;

        public CreditCardService(
            ICreditCardRepository creditCardRepository,
            IBankAccountRepository bankAccountRepository,
            IBankAccountService bankAccountService)
        {
            _creditCardRepository = creditCardRepository;
            _bankAccountRepository = bankAccountRepository;
            _bankAccountService = bankAccountService;
        }

        public async Task<ResultViewModel<IEnumerable<CreditCardViewModel>>> GetByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return ResultViewModel<IEnumerable<CreditCardViewModel>>.Error("UserId é obrigatório.");

            try
            {
                var creditCards = await _creditCardRepository.GetByUserIdAsync(userId);
                var viewModels = new List<CreditCardViewModel>();

                foreach (var card in creditCards)
                {
                    var viewModel = await MapToViewModel(card);
                    viewModels.Add(viewModel);
                }

                return ResultViewModel<IEnumerable<CreditCardViewModel>>.Success(viewModels, "Cartões de crédito encontrados com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel<IEnumerable<CreditCardViewModel>>.Error($"Erro ao buscar cartões de crédito: {ex.Message}");
            }
        }

        public async Task<ResultViewModel<IEnumerable<CreditCardViewModel>>> GetActiveByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return ResultViewModel<IEnumerable<CreditCardViewModel>>.Error("UserId é obrigatório.");

            try
            {
                var creditCards = await _creditCardRepository.GetActiveByUserIdAsync(userId);
                var viewModels = new List<CreditCardViewModel>();

                foreach (var card in creditCards)
                {
                    var viewModel = await MapToViewModel(card);
                    viewModels.Add(viewModel);
                }

                return ResultViewModel<IEnumerable<CreditCardViewModel>>.Success(viewModels, "Cartões de crédito ativos encontrados com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel<IEnumerable<CreditCardViewModel>>.Error($"Erro ao buscar cartões de crédito ativos: {ex.Message}");
            }
        }

        public async Task<ResultViewModel<CreditCardViewModel>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return ResultViewModel<CreditCardViewModel>.Error("ID é obrigatório.");

            try
            {
                var creditCard = await _creditCardRepository.GetByIdWithTransactionsAsync(id);
                if (creditCard == null)
                    return ResultViewModel<CreditCardViewModel>.Error("Cartão de crédito não encontrado.");

                var viewModel = await MapToViewModel(creditCard);
                return ResultViewModel<CreditCardViewModel>.Success(viewModel, "Cartão de crédito encontrado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel<CreditCardViewModel>.Error($"Erro ao buscar cartão de crédito: {ex.Message}");
            }
        }

        public async Task<ResultViewModel<Guid>> CreateAsync(CreditCardInputModel input)
        {
            if (input == null)
                return ResultViewModel<Guid>.Error("Dados de entrada são obrigatórios.");

            try
            {
                var account = await _bankAccountRepository.GetByIdAsync(input.AccountId);
                if (account == null)
                    return ResultViewModel<Guid>.Error("Conta bancária não encontrada.");

                var exists = await _creditCardRepository.ExistsByDescriptionAndUserIdAsync(input.Description, account.UserId);
                if (exists)
                    return ResultViewModel<Guid>.Error("Já existe um cartão de crédito com esta descrição.");

                if (input.DueDay == input.ClosingDay)
                    return ResultViewModel<Guid>.Error("Dia de vencimento não pode ser igual ao dia de fechamento.");

                var creditCard = new CreditCard(
                    input.Description,
                    input.CreditLimit,
                    input.DueDay,
                    input.ClosingDay,
                    input.AccountId);

                var result = await _creditCardRepository.AddAsync(creditCard);
                return ResultViewModel<Guid>.Success(result.Id, "Cartão de crédito criado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel<Guid>.Error($"Erro ao criar cartão de crédito: {ex.Message}");
            }
        }

        public async Task<ResultViewModel> UpdateAsync(UpdateCreditCardInputModel input)
        {
            if (input == null)
                return ResultViewModel.Error("Dados de entrada são obrigatórios.");

            try
            {
                var creditCard = await _creditCardRepository.GetByIdAsync(input.Id);
                if (creditCard == null)
                    return ResultViewModel.Error("Cartão de crédito não encontrado.");

                var account = await _bankAccountRepository.GetByIdAsync(creditCard.AccountId);
                var exists = await _creditCardRepository.ExistsByDescriptionAndUserIdAsync(input.Description, account.UserId, input.Id);
                if (exists)
                    return ResultViewModel.Error("Já existe um cartão de crédito com esta descrição.");

                if (input.DueDay == input.ClosingDay)
                    return ResultViewModel.Error("Dia de vencimento não pode ser igual ao dia de fechamento.");

                creditCard.UpdateCreditCard(input.Description, input.CreditLimit, input.DueDay, input.ClosingDay);
                await _creditCardRepository.UpdateAsync(creditCard);

                return ResultViewModel.Success("Cartão de crédito atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel.Error($"Erro ao atualizar cartão de crédito: {ex.Message}");
            }
        }

        public async Task<ResultViewModel> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return ResultViewModel.Error("ID é obrigatório.");

            try
            {
                var creditCard = await _creditCardRepository.GetByIdWithTransactionsAsync(id);
                if (creditCard == null)
                    return ResultViewModel.Error("Cartão de crédito não encontrado.");

                if (creditCard.Transactions?.Any() == true)
                    return ResultViewModel.Error("Não é possível excluir um cartão de crédito que possui transações.");

                await _creditCardRepository.DeleteAsync(creditCard);
                return ResultViewModel.Success("Cartão de crédito excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel.Error($"Erro ao excluir cartão de crédito: {ex.Message}");
            }
        }

        public async Task<ResultViewModel<IEnumerable<CreditCardInvoiceViewModel>>> GetInvoicesByUserAndMonthAsync(Guid userId, int year, int month)
        {
            if (userId == Guid.Empty)
                return ResultViewModel<IEnumerable<CreditCardInvoiceViewModel>>.Error("UserId é obrigatório.");

            try
            {
                var creditCards = await _creditCardRepository.GetActiveByUserIdAsync(userId);
                var invoices = new List<CreditCardInvoiceViewModel>();
                var currentDate = new DateTime(year, month, 1);

                foreach (var card in creditCards)
                {
                    var invoice = CreateInvoiceViewModel(card, currentDate);
                    invoices.Add(invoice);
                }

                return ResultViewModel<IEnumerable<CreditCardInvoiceViewModel>>.Success(invoices, "Faturas encontradas com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel<IEnumerable<CreditCardInvoiceViewModel>>.Error($"Erro ao buscar faturas: {ex.Message}");
            }
        }

        public async Task<ResultViewModel<CreditCardInvoiceViewModel>> GetInvoiceByCardAndMonthAsync(Guid creditCardId, int year, int month)
        {
            if (creditCardId == Guid.Empty)
                return ResultViewModel<CreditCardInvoiceViewModel>.Error("ID do cartão é obrigatório.");

            try
            {
                var creditCard = await _creditCardRepository.GetByIdWithTransactionsAsync(creditCardId);
                if (creditCard == null)
                    return ResultViewModel<CreditCardInvoiceViewModel>.Error("Cartão de crédito não encontrado.");

                var currentDate = new DateTime(year, month, 1);
                var invoice = CreateInvoiceViewModel(creditCard, currentDate);

                return ResultViewModel<CreditCardInvoiceViewModel>.Success(invoice, "Fatura encontrada com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel<CreditCardInvoiceViewModel>.Error($"Erro ao buscar fatura: {ex.Message}");
            }
        }

        public async Task<ResultViewModel> PayInvoiceAsync(Guid creditCardId, int year, int month)
        {
            if (creditCardId == Guid.Empty)
                return ResultViewModel.Error("ID do cartão é obrigatório.");

            try
            {
                var creditCard = await _creditCardRepository.GetByIdWithTransactionsAsync(creditCardId);
                if (creditCard == null)
                    return ResultViewModel.Error("Cartão de crédito não encontrado.");

                var referenceDate = new DateTime(year, month, 1);
                var invoiceAmount = creditCard.GetCurrentInvoiceAmount(referenceDate);

                if (invoiceAmount == 0)
                    return ResultViewModel.Error("Não há valor a pagar nesta fatura.");

                if (creditCard.IsInvoicePaid(referenceDate))
                    return ResultViewModel.Error("Esta fatura já foi paga.");

                creditCard.PayInvoice(referenceDate);
                await _creditCardRepository.UpdateAsync(creditCard);

                return ResultViewModel.Success($"Fatura no valor de {invoiceAmount:C} paga com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultViewModel.Error($"Erro ao pagar fatura: {ex.Message}");
            }
        }

        private async Task<CreditCardViewModel> MapToViewModel(CreditCard creditCard)
        {
            var currentDate = DateTime.UtcNow;
            var availableLimit = creditCard.GetAvailableLimit();
            var usedLimit = creditCard.CreditLimit - availableLimit;
            var currentInvoiceAmount = creditCard.GetCurrentInvoiceAmount(currentDate);
            var currentInvoiceDueDate = creditCard.GetCurrentInvoiceDueDate(currentDate);

            var bankNameResult = await _bankAccountService.GetBankNameByBankIdAsync(creditCard.Account.BankId);

            return new CreditCardViewModel
            {
                Id = creditCard.Id,
                Description = creditCard.Description,
                CreditLimit = creditCard.CreditLimit,
                AvailableLimit = availableLimit,
                UsedLimit = usedLimit,
                CurrentInvoiceAmount = currentInvoiceAmount,
                CurrentInvoiceDueDate = currentInvoiceDueDate,
                DueDay = creditCard.DueDay,
                ClosingDay = creditCard.ClosingDay,
                IsActive = creditCard.IsActive,
                AccountId = creditCard.AccountId,
                BankName = bankNameResult?.Data ?? "-",
                AccountDescription = creditCard.Account?.Description ?? "",
                CreatedAt = creditCard.CreatedAt,
                UpdatedAt = creditCard.UpdatedAt
            };
        }

        private CreditCardInvoiceViewModel CreateInvoiceViewModel(CreditCard creditCard, DateTime referenceDate)
        {
            var invoiceAmount = creditCard.GetCurrentInvoiceAmount(referenceDate);
            var invoiceDueDate = creditCard.GetCurrentInvoiceDueDate(referenceDate);
            var availableLimit = creditCard.GetAvailableLimit();
            var daysUntilDue = (invoiceDueDate.Date - DateTime.UtcNow.Date).Days;
            var isOverdue = DateTime.UtcNow.Date > invoiceDueDate.Date && invoiceAmount > 0;
            var isPaid = creditCard.IsInvoicePaid(referenceDate);
            
            // Calcula as datas de início e fim da fatura
            var invoiceStartDate = GetInvoiceStartDateHelper(referenceDate, creditCard.ClosingDay);
            var invoiceEndDate = GetInvoiceEndDateHelper(referenceDate, creditCard.ClosingDay);

            return new CreditCardInvoiceViewModel
            {
                CreditCardId = creditCard.Id,
                CreditCardDescription = creditCard.Description,
                InvoiceAmount = invoiceAmount,
                InvoiceDueDate = invoiceDueDate,
                InvoiceStartDate = invoiceStartDate,
                InvoiceEndDate = invoiceEndDate,
                IsOverdue = isOverdue && !isPaid,
                DaysUntilDue = daysUntilDue,
                CreditLimit = creditCard.CreditLimit,
                AvailableLimit = availableLimit,
                IsPaid = isPaid
            };
        }

        private DateTime GetInvoiceStartDateHelper(DateTime currentDate, int closingDay)
        {
            var closingDate = new DateTime(currentDate.Year, currentDate.Month, closingDay);
            
            if (currentDate <= closingDate)
            {
                return closingDate.AddMonths(-1).AddDays(1);
            }
            else
            {
                return closingDate.AddDays(1);
            }
        }

        private DateTime GetInvoiceEndDateHelper(DateTime currentDate, int closingDay)
        {
            var closingDate = new DateTime(currentDate.Year, currentDate.Month, closingDay);
            
            if (currentDate <= closingDate)
            {
                return closingDate;
            }
            else
            {
                return closingDate.AddMonths(1);
            }
        }
    }
}