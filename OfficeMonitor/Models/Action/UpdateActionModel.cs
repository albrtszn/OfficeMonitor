using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Action
{
    public class UpdateActionModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdEmployee { get; set; }
        [Required]
        public int IdApp { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        [Precision(0)]
        public TimeOnly StartTime { get; set; }
        [Required]
        [Precision(0)]
        public TimeOnly EndTime { get; set; }
    }
}
