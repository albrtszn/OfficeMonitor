using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeMonitor.DTOs
{
    public class WorkTimeDto
    {
        public int Id { get; set; }
        public int? IdDepartment { get; set; }

        [Precision(0)]
        public TimeOnly? StartTime { get; set; }

        [Precision(0)]
        public TimeOnly? EndTime { get; set; }
    }
}
