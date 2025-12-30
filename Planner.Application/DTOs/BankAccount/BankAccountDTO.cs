using Planner.Domain.Entities;
using Planner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Application.DTOs.BankAccount
{
    public class BankAccountDTO
    {

        public Guid Id { get; set; }
        public string BankName { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public TypeAccount Type { get; set; }
        public decimal Balance { get; set; }
        public decimal ForecastBalance { get; set; }
        public string IconBank { get; set; }
        public decimal InitialBalance { get; set; }
        public int NumberOfTransfers { get; set; }
        public int QuantityOfRevenue { get; set; }
        public int AmountOfExpenses { get; set; }
        public int BankId { get; set; }

    }
}
