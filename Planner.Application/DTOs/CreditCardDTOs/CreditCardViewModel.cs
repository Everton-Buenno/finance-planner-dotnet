using System;

namespace Planner.Application.DTOs.CreditCardDTOs
{
    public class CreditCardViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal CreditLimit { get; set; }
        public decimal AvailableLimit { get; set; }
        public decimal UsedLimit { get; set; }
        public decimal CurrentInvoiceAmount { get; set; }
        public DateTime CurrentInvoiceDueDate { get; set; }
        public int DueDay { get; set; }
        public int ClosingDay { get; set; }
        public bool IsActive { get; set; }
        public Guid AccountId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string AccountDescription { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public decimal UsagePercentage => CreditLimit > 0 ? Math.Round((UsedLimit / CreditLimit) * 100, 2) : 0;

        public string Status
        {
            get
            {
                if (!IsActive) return "Inativo";
                if (UsagePercentage >= 90) return "Limite Crítico";
                if (UsagePercentage >= 70) return "Limite Alto";
                if (UsagePercentage >= 50) return "Limite Médio";
                return "Limite Baixo";
            }
        }

        public string StatusColor
        {
            get
            {
                if (!IsActive) return "#6c757d";
                if (UsagePercentage >= 90) return "#dc3545";
                if (UsagePercentage >= 70) return "#fd7e14";
                if (UsagePercentage >= 50) return "#ffc107";
                return "#28a745";
            }
        }
    }
}