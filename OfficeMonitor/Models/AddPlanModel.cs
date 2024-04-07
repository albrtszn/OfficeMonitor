using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddPlanModel
    {
        [StringLength(50)]
        [Required]
        public string? Name { get; set; }
        [StringLength(100)]
        [Required]
        public string? Description { get; set; }
        [Required]
        public decimal? MonthCost { get; set; }
        [Required]
        public decimal? Yearcost { get; set; }
        [Required]
        public int? CountOfEmployees { get; set; }
    }
}
