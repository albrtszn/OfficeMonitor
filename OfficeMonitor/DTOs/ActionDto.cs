using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeMonitor.DTOs
{
    public class ActionDto
    {
        public int Id { get; set; }
        public int? IdEmployee { get; set; }
        public int? IdApp { get; set; }
        public DateOnly? Date { get; set; }
        [Precision(0)]
        public TimeOnly? StartTime { get; set; }
        [Precision(0)]
        public TimeOnly? EndTime { get; set; }
    }
}
