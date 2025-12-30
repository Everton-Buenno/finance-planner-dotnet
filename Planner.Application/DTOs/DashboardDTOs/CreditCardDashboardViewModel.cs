using System;

namespace Planner.Application.DTOs.DashboardDTOs
{
    public class CreditCardDashboardViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string BankName { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal UsedLimit { get; set; }
        public decimal AvailableLimit { get; set; }
        public decimal UsagePercentage { get; set; }
        public decimal CurrentInvoiceAmount { get; set; }
        public DateTime CurrentInvoiceDueDate { get; set; }
        public int DaysUntilDue { get; set; }
        public bool IsInvoicePaid { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
    }
}
