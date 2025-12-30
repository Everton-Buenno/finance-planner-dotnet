using Planner.Domain.Enums;
using System;

namespace Planner.Application.DTOs.BankAccount
{
    public class AddBankAccountInputModel
    {
        public Guid UserId { get; set; }
        public int BankId { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public TypeAccount Type { get; set; }
        public decimal InitialBalance { get; set; }

    }
} 