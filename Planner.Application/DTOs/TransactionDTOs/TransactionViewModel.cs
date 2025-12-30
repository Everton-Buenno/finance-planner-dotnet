using Planner.Domain.Entities;
using Planner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Application.DTOs.TransactionDTOs
{
    public class TransactionViewModel
    {
        public Guid Id { get;  set; }
        public DateTime DateTransaction { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public bool IsPaid { get; set; }
        public bool Ignored { get; set; }
        public TypeTransaction Type { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }
        public Guid? CategoryId { get; set; }
        public string BankName { get; set; }
        public Guid AccountId { get; set; }
        
        public Guid? CreditCardId { get; set; }
        
        public string? CreditCardDescription { get; set; }
    }
}
