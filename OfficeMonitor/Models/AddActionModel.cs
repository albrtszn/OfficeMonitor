using Microsoft.EntityFrameworkCore;

namespace OfficeMonitor.Models
{
    public class AddActionModel
    {
        public int? IdEmployee { get; set; }
        public int? IdApp { get; set; }
        public DateOnly? Date { get; set; }
        [Precision(0)]
        public TimeOnly? StartTime { get; set; }
        [Precision(0)]
        public TimeOnly? EndTime { get; set; }
    }
}
