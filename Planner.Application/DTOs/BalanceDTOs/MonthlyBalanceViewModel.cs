using System;

namespace Planner.Application.DTOs.BalanceDTOs
{
  
    public class MonthlyBalanceViewModel
    {
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal MonthlyBalance { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalIncomeAccumulated { get; set; }
        public decimal TotalExpenseAccumulated { get; set; }
        
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? BankAccountDescription { get; set; }
    }
}