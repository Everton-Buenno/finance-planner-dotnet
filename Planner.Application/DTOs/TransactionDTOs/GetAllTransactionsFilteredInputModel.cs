using System;

namespace Planner.Application.DTOs.TransactionDTOs
{
    public class GetAllTransactionsFilteredInputModel
    {
        public Guid UserId { get; set; }
        public Guid? BankAccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsPaid { get; set; }
        public bool? Ignored { get; set; }
        public bool? IsRecurring { get; set; }
    }
} 