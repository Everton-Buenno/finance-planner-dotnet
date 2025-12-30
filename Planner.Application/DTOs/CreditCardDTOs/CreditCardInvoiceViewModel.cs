using System;

namespace Planner.Application.DTOs.CreditCardDTOs
{
    public class CreditCardInvoiceViewModel
    {
        public Guid CreditCardId { get; set; }
        public string CreditCardDescription { get; set; } = string.Empty;
        public decimal InvoiceAmount { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public DateTime InvoiceStartDate { get; set; }
        public DateTime InvoiceEndDate { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysUntilDue { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal AvailableLimit { get; set; }
        public bool IsPaid { get; set; }

        public string InvoiceStatus
        {
            get
            {
                if (IsPaid) return "Paga";
                if (IsOverdue) return "Vencida";
                if (DaysUntilDue <= 3) return "Vencimento Próximo";
                if (DaysUntilDue <= 7) return "A Vencer";
                return "Em Aberto";
            }
        }

        public string InvoiceStatusColor
        {
            get
            {
                if (IsPaid) return "#28a745";
                if (IsOverdue) return "#dc3545";
                if (DaysUntilDue <= 3) return "#fd7e14";
                if (DaysUntilDue <= 7) return "#ffc107";
                return "#17a2b8";
            }
        }
    }
}