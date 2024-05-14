using OfficeMonitor.DTOs;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Departments
{
    public class DepartmentStatistic
    {
        [Required]
        public string Name { get; set; }
        [Required] 
        public WorkTimeDto? WorkTime { get; set; }
        [Required]
        public decimal WorkedPercent { get; set; }
        [Required]
        public decimal IdlePercent { get; set; }        
        [Required]
        public decimal DiversionPercent { get; set; }
        [Required]
        public TimeSummaryModel TotalHours { get; set; }
    }
}
