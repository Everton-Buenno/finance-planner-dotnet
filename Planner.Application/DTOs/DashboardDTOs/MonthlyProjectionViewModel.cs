using System;
using System.Collections.Generic;

namespace Planner.Application.DTOs.DashboardDTOs
{
    public class MonthlyProjectionViewModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance { get; set; }
        public decimal PaidIncome { get; set; }
        public decimal PaidExpenses { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal PendingIncome { get; set; }
        public decimal PendingExpenses { get; set; }
        public int TotalTransactions { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
