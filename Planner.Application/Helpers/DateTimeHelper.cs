using System;

namespace Planner.Application.Helpers
{
 
    public static class DateTimeHelper
    {
      
        public static DateTime EnsureUtc(DateTime dateTime)
        {
            return dateTime.Kind switch
            {
                DateTimeKind.Unspecified => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc),
                DateTimeKind.Local => dateTime.ToUniversalTime(),
                DateTimeKind.Utc => dateTime,
                _ => dateTime
            };
        }

        public static DateTime? EnsureUtc(DateTime? dateTime)
        {
            return dateTime.HasValue ? EnsureUtc(dateTime.Value) : null;
        }

    
        public static DateTime CreateUtcStartOfMonth(int year, int month)
        {
            return DateTime.SpecifyKind(new DateTime(year, month, 1), DateTimeKind.Utc);
        }

    
        public static DateTime CreateUtcEndOfMonth(int year, int month)
        {
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            return DateTime.SpecifyKind(endOfMonth, DateTimeKind.Utc);
        }
    }
}