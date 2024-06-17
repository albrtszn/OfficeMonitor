using OfficeMonitor.DTOs;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Employee
{
    public class EmployeeStatistic
    {
        public EmployeeDto Employee { get; set; }
        [Required]
        public WorkTimeDto? WorkTime { get; set; }
        [Required]
        public TimeSummaryModel RequiredTotalHours { get; set; }
        [Required]
        public TimeSummaryModel TotalHours { get; set; }
        public List<DayStatistic> Actions = new List<DayStatistic>();
    }
}
