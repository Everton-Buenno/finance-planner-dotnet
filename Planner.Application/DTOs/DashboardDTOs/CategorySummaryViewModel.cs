using System;

namespace Planner.Application.DTOs.DashboardDTOs
{
    public class CategorySummaryViewModel
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }
        public string CategoryColor { get; set; }
        public decimal TotalValue { get; set; }
        public int TransactionCount { get; set; }
        public decimal Percentage { get; set; }
    }
}
