using Microsoft.EntityFrameworkCore;

namespace OfficeMonitor.Models
{
    public class AddWorkTimeModel
    {
        public int? IdDepartment { get; set; }

        [Precision(0)]
        public TimeOnly? StartTime { get; set; }

        [Precision(0)]
        public TimeOnly? EndTime { get; set; }
    }
}
