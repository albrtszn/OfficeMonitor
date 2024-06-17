using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Employee
{
    public class GetEmployeeStatistic
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string DateRange { get; set; }
    }
}
