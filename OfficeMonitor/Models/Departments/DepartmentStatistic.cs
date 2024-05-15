using OfficeMonitor.DTOs;
using OfficeMonitor.Models.Employee;
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
        public TimeSummaryModel RequiredTotalHours { get; set; }
        [Required]
        public TimeSummaryModel TotalHours{ get; set; }
        [Required]
        public double WorkedPercent { get; set; }
        [Required]
        public double IdlePercent { get; set; }        
        [Required]
        public double DiversionPercent { get; set; }
        [Required]
        public List<GetEmployeeWithInfoModel> Employees { get; set; }

        public DepartmentStatistic()
        {
            this.Employees = new List<GetEmployeeWithInfoModel>();
        }
    }
}
