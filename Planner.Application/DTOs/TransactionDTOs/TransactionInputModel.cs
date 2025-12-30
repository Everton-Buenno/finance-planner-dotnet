using Planner.Domain.Enums;
using System;

namespace Planner.Application.DTOs.TransactionDTOs
{
    public class TransactionInputModel
    {
        public DateTime DateTransaction { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public bool IsPaid { get; set; }
        public bool Ignored { get; set; }
        public TypeTransaction Type { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? AccountDestinationId { get; set; }
        public int RepeatCount { get; set; } = 1;
        public RepeatType? RepeatType { get; set; }
     
        public Guid? CreditCardId { get; set; }
    }
}