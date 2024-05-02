using Microsoft.EntityFrameworkCore;
using OfficeMonitor.DTOs;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.WorkTime
{
    public class GetWorkTimeModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DepartmentDto Department { get; set; }

        [Required]
        [Precision(0)]
        public TimeOnly StartTime { get; set; }
        [Required]
        [Precision(0)]
        public TimeOnly EndTime { get; set; }
    }
}
