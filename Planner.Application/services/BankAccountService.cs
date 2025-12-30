using Planner.Application.DTOs.BankAccount;
using Planner.Application.interfaces;
using System.Text.Json;
using Planner.Domain.Interfaces;
using Planner.Domain.Entities;


namespace Planner.Application.services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly string _jsonFilePath;
        private readonly IBankAccountRepository _bankAccountRepository;
        
        public BankAccountService(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
            
            var possiblePaths = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bancos", "bancos.json"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "bancos", "bancos.json"),
                Path.Combine(AppContext.BaseDirectory, "wwwroot", "bancos", "bancos.json")
            };
            
            _jsonFilePath = possiblePaths.FirstOrDefault(File.Exists) ?? possiblePaths[0];
        }

        public ResultViewModel<List<Bank>> GetAllBanks()
        {
            if (File.Exists(_jsonFilePath))
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                var banks = JsonSerializer.Deserialize<List<Bank>>(jsonData);
                return ResultViewModel<List<Bank>>.Success(banks ?? new List<Bank>(), "Bancos retornados com sucesso.");
            }
            return ResultViewModel<List<Bank>>.Success(new List<Bank>(), "Nenhum banco encontrado.");
        }

        public Task<ResultViewModel<string>> GetBankNameByBankIdAsync(int bankId)
        {
            if(bankId <= 0)
                return Task.FromResult(ResultViewModel<string>.Error("Id do banco inválido."));
            var banksResult = GetAllBanks();
            var bank = banksResult.Data.FirstOrDefault(b => b.BankId == bankId);
            return Task.FromResult(ResultViewModel<string>.Success(bank?.Name ?? string.Empty, bank != null ? "Nome do banco encontrado." : "Banco não encontrado."));
        }

        public Task<ResultViewModel<Bank>> GetBankByBankIdAsync(int bankId)
        {
            if (bankId <= 0)
                return Task.FromResult(ResultViewModel<Bank>.Error("Id do banco inválido."));
            var banksResult = GetAllBanks();
            var bank = banksResult.Data.FirstOrDefault(b => b.BankId == bankId);
            if (bank is null)
                return Task.FromResult(ResultViewModel<Bank>.Error("Banco não encontrado."));
            return Task.FromResult(ResultViewModel<Bank>.Success(bank, "Banco encontrado com sucesso."));
        }

        public ResultViewModel<List<Bank>> GetBaseBanks()
        {
            if (File.Exists(_jsonFilePath))
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                var banks = JsonSerializer.Deserialize<List<Bank>>(jsonData);
                var banksIds = new List<int> { 214, 179, 50, 81, 139, 72, 194, 44 };

                var baseBanks = banks.Where(b => banksIds.Contains(b.BankId)).ToList();

                return ResultViewModel<List<Bank>>.Success(baseBanks, "Bancos base retornados com sucesso.");
            }
            return ResultViewModel<List<Bank>>.Success(new List<Bank>(), "Nenhum banco base encontrado.");
        }

        public async Task<ResultViewModel> AddAsync(AddBankAccountInputModel input)
        {
            if (input == null)
                return ResultViewModel.Error("Dados inválidos.");

            var entity = new BankAccount(
                input.BankId,
                input.Description,
                input.UserId,
                input.Color,
                input.Type,
                input.InitialBalance 
            );

            await _bankAccountRepository.AddAsync(entity);

            return ResultViewModel.Success("Conta bancária criada com sucesso.");
        }

        public async Task<ResultViewModel> UpdateAsync(UpdateBankAccountInputModel input)
        {
            if (input == null)
                return ResultViewModel.Error("Dados inválidos.");

            var entity = await _bankAccountRepository.GetByIdAsync(input.Id);
            if (entity == null)
                return ResultViewModel.Error("Conta bancária não encontrada.");

            entity.UpdateDescription(input.Description);
            entity.UpdateColor(input.Color);
            entity.UpdateType(input.Type);

            await _bankAccountRepository.UpdateAsync(entity);

            return ResultViewModel.Success("Conta bancária atualizada com sucesso.");
        }

        public async Task<ResultViewModel> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return ResultViewModel.Error("Id inválido.");

            var entity = await _bankAccountRepository.GetByIdAsync(id);
            if (entity == null)
                return ResultViewModel.Error("Conta bancária não encontrada.");

            await _bankAccountRepository.DeleteAsync(entity);

            return ResultViewModel.Success("Conta bancária excluída com sucesso.");
        }

        public async Task<ResultViewModel<List<BankAccountDTO>>> GetAccountsByUserIdAsync(Guid userId)
        {
            var accounts = await _bankAccountRepository.GetAccountsByUserId(userId);
            if (accounts == null || !accounts.Any())
                return ResultViewModel<List<BankAccountDTO>>.Error("Nenhuma conta encontrada para o usuário.");

            var banksResult = GetAllBanks();
            var banks = banksResult.Data;
            var endOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            
            var dtos = accounts.Select(acc =>
            {
                var transactions = acc.Transactions?.Where(t => !t.IsDeleted).ToList() ?? new List<Transaction>();

                var paidIncome = transactions
                    .Where(t => t.IsPaid && !t.Ignored && t.Type == Domain.Enums.TypeTransaction.Income)
                    .Sum(t => t.Value);

                var paidExpenses = transactions
                    .Where(t => t.IsPaid && !t.Ignored && (t.Type == Domain.Enums.TypeTransaction.Expense || t.Type == Domain.Enums.TypeTransaction.CreditCardExpense))
                    .Sum(t => t.Value);

                var paidTransfers = transactions
                    .Where(t => t.IsPaid && !t.Ignored && t.Type == Domain.Enums.TypeTransaction.Transfer)
                    .Sum(t => t.Value);

                var allIncome = transactions
                    .Where(t => !t.Ignored && t.Type == Domain.Enums.TypeTransaction.Income && t.DateTransaction <= endOfCurrentMonth)
                    .Sum(t => t.Value);

                var allExpenses = transactions
                    .Where(t => !t.Ignored && (t.Type == Domain.Enums.TypeTransaction.Expense || t.Type == Domain.Enums.TypeTransaction.CreditCardExpense) && t.DateTransaction <= endOfCurrentMonth)
                    .Sum(t => t.Value);

                var allTransfers = transactions
                    .Where(t => !t.Ignored && t.Type == Domain.Enums.TypeTransaction.Transfer && t.DateTransaction <= endOfCurrentMonth)
                    .Sum(t => t.Value);

                return new BankAccountDTO
                {
                    Id = acc.Id,
                    BankId = acc.BankId,
                    BankName = banks.FirstOrDefault(b => b.BankId == acc.BankId)?.Name ?? string.Empty,
                    IconBank = banks.FirstOrDefault(b => b.BankId == acc.BankId)?.PathImage ?? string.Empty,
                    Description = acc.Description,
                    Color = acc.Color,
                    Type = acc.Type,
                    InitialBalance = acc.InitialBalance,
                    Balance = acc.InitialBalance + paidIncome - paidExpenses - paidTransfers,
                    ForecastBalance = acc.InitialBalance + allIncome - allExpenses - allTransfers,
                    NumberOfTransfers = transactions.Count(t => !t.Ignored && t.Type == Domain.Enums.TypeTransaction.Transfer),
                    QuantityOfRevenue = transactions.Count(t => !t.Ignored && t.Type == Domain.Enums.TypeTransaction.Income),
                    AmountOfExpenses = transactions.Count(t => !t.Ignored && (t.Type == Domain.Enums.TypeTransaction.Expense || t.Type == Domain.Enums.TypeTransaction.CreditCardExpense))
                };
            }).ToList();
            return ResultViewModel<List<BankAccountDTO>>.Success(dtos, "Contas encontradas com sucesso.");
        }
    }
}
