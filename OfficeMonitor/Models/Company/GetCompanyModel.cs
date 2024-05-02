using OfficeMonitor.DTOs;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Company
{
    public class GetCompanyModel
    {
        [StringLength(255)]
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        [StringLength(150)]
        public string Password { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        public PlanDto Plan { get; set; }
        [Required]
        public decimal? Balance { get; set; }
        //[Required]
        //public int? IdClaimRole { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
