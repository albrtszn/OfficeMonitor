using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class DayStatistic
    {
        public DateOnly Date { get; set; }
        [Required]
        public double WorkedPercent { get; set; }
        [Required]
        public double IdlePercent { get; set; }
        [Required]
        public double DiversionPercent { get; set; }
    }
}
