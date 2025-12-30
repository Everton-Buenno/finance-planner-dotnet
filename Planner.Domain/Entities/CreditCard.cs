using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.Entities
{
    public class CreditCard : BaseEntity
    {
        public CreditCard() { }

        public CreditCard(
            string description,
            decimal creditLimit,
            int dueDay,
            int closingDay,
            Guid accountId)
        {
            Description = description;
            CreditLimit = creditLimit;
            DueDay = dueDay;
            ClosingDay = closingDay;
            AccountId = accountId;
            IsActive = true;
        }

        public string Description { get; set; } = string.Empty;

        public decimal CreditLimit { get; set; }

        public int DueDay { get; set; }

        public int ClosingDay { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid AccountId { get; set; }
        public BankAccount Account { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public void UpdateCreditCard(string description, decimal creditLimit, int dueDay, int closingDay)
        {
            Description = description;
            CreditLimit = creditLimit;
            DueDay = dueDay;
            ClosingDay = closingDay;
            MarkAsUpdated();
        }

        public decimal GetAvailableLimit()
        {
            // O limite disponível deve considerar apenas transações:
            // - NÃO deletadas (!IsDeleted)
            // - NÃO pagas (!IsPaid) - faturas em aberto
            // - NÃO ignoradas (!Ignored) - transações que devem ser consideradas
            // - Do tipo CreditCardExpense
            var usedLimit = Transactions?
                .Where(t => !t.IsDeleted && 
                           !t.IsPaid && 
                           !t.Ignored &&  // 🆕 Ignorar transações marcadas como ignoradas
                           t.Type == Enums.TypeTransaction.CreditCardExpense)
                .Sum(t => t.Value) ?? 0;

            return CreditLimit - usedLimit;
        }

        public decimal GetCurrentInvoiceAmount(DateTime currentDate)
        {
            var invoiceStartDate = GetInvoiceStartDate(currentDate);
            var invoiceEndDate = GetInvoiceEndDate(currentDate);

            // Soma todas as transações do período da fatura atual
            // NÃO ignora transações marcadas como ignoradas aqui, pois queremos mostrar o total da fatura
            return Transactions?
                .Where(t => !t.IsDeleted &&
                           !t.Ignored &&  // 🆕 Transações ignoradas não devem aparecer na fatura
                           t.DateTransaction >= invoiceStartDate && 
                           t.DateTransaction <= invoiceEndDate &&
                           t.Type == Enums.TypeTransaction.CreditCardExpense)
                .Sum(t => t.Value) ?? 0;
        }

        public decimal GetUnpaidInvoicesAmount()
        {
            // Retorna o total de faturas não pagas (transações não pagas e não ignoradas)
            return Transactions?
                .Where(t => !t.IsDeleted && 
                           !t.IsPaid && 
                           !t.Ignored &&  // 🆕 Não considerar ignoradas
                           t.Type == Enums.TypeTransaction.CreditCardExpense)
                .Sum(t => t.Value) ?? 0;
        }

        public decimal GetInvoiceAmountByPeriod(DateTime startDate, DateTime endDate)
        {
            // Retorna o valor total da fatura em um período específico
            return Transactions?
                .Where(t => !t.IsDeleted &&
                           !t.Ignored &&  // 🆕 Não considerar ignoradas
                           t.DateTransaction >= startDate && 
                           t.DateTransaction <= endDate &&
                           t.Type == Enums.TypeTransaction.CreditCardExpense)
                .Sum(t => t.Value) ?? 0;
        }

        public bool IsInvoicePaid(DateTime referenceDate)
        {
            // Verifica se todas as transações da fatura do período estão pagas
            var invoiceStartDate = GetInvoiceStartDate(referenceDate);
            var invoiceEndDate = GetInvoiceEndDate(referenceDate);

            var invoiceTransactions = Transactions?
                .Where(t => !t.IsDeleted &&
                           !t.Ignored &&  // 🆕 Não considerar ignoradas
                           t.DateTransaction >= invoiceStartDate && 
                           t.DateTransaction <= invoiceEndDate &&
                           t.Type == Enums.TypeTransaction.CreditCardExpense)
                .ToList();

            if (invoiceTransactions == null || !invoiceTransactions.Any())
                return true; // Sem transações = fatura paga

            return invoiceTransactions.All(t => t.IsPaid);
        }

        public void PayInvoice(DateTime referenceDate)
        {
            // Marca todas as transações da fatura como pagas
            var invoiceStartDate = GetInvoiceStartDate(referenceDate);
            var invoiceEndDate = GetInvoiceEndDate(referenceDate);

            var invoiceTransactions = Transactions?
                .Where(t => !t.IsDeleted &&
                           !t.Ignored &&  // 🆕 Não pagar transações ignoradas
                           t.DateTransaction >= invoiceStartDate && 
                           t.DateTransaction <= invoiceEndDate &&
                           t.Type == Enums.TypeTransaction.CreditCardExpense &&
                           !t.IsPaid)
                .ToList();

            if (invoiceTransactions != null)
            {
                foreach (var transaction in invoiceTransactions)
                {
                    transaction.IsPaid = true;
                }
            }

            MarkAsUpdated();
        }

        private DateTime GetInvoiceStartDate(DateTime currentDate)
        {
            var closingDate = new DateTime(currentDate.Year, currentDate.Month, ClosingDay);
            
            if (currentDate <= closingDate)
            {
                return closingDate.AddMonths(-1).AddDays(1);
            }
            else
            {
                return closingDate.AddDays(1);
            }
        }

        private DateTime GetInvoiceEndDate(DateTime currentDate)
        {
            var closingDate = new DateTime(currentDate.Year, currentDate.Month, ClosingDay);
            
            if (currentDate <= closingDate)
            {
                return closingDate;
            }
            else
            {
                return closingDate.AddMonths(1);
            }
        }

        public DateTime GetCurrentInvoiceDueDate(DateTime currentDate)
        {
            var closingDate = new DateTime(currentDate.Year, currentDate.Month, ClosingDay);
            
            if (currentDate <= closingDate)
            {
                return new DateTime(currentDate.Year, currentDate.Month, DueDay);
            }
            else
            {
                return new DateTime(currentDate.Year, currentDate.Month, DueDay).AddMonths(1);
            }
        }
    }
}
