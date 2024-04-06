using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddPlanModel
    {
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
        public decimal? MonthCost { get; set; }
        public decimal? Yearcost { get; set; }
        public int? CountOfEmployees { get; set; }
    }
}
