using Planner.Domain.Enums;
using System;

namespace Planner.Application.DTOs.BankAccount
{
    public class UpdateBankAccountInputModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public TypeAccount Type { get; set; }
    }
} 