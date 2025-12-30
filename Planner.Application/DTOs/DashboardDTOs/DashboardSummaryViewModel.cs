using System.Collections.Generic;

namespace Planner.Application.DTOs.DashboardDTOs
{
    public class DashboardSummaryViewModel
    {
        public List<CategorySummaryViewModel> ExpensesByCategory { get; set; }
        public List<CategorySummaryViewModel> IncomeByCategory { get; set; }
        public MonthlyProjectionViewModel MonthlyProjection { get; set; }
    }
}
