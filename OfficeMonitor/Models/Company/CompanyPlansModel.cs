using OfficeMonitor.DTOs;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Company
{
    public class CompanyPlansModel
    {
        [Required]
        public PlanDto CompanyPlan { get; set; }
        [Required]
        public List<PlanDto> Plans { get; set; }
    }
}
