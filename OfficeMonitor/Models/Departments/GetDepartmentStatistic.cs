using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Departments
{
    public class GetDepartmentStatistic
    {
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public string DateRange { get; set; }
    }
}
