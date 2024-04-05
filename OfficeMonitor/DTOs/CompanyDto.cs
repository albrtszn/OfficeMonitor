using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Login { get; set; } = null!;
        [StringLength(150)]
        public string Password { get; set; } = null!;
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
        public int? IdPlan { get; set; }
        public decimal? Balance { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBanned { get; set; }
        public DateTime? DateOfRegister { get; set; }
        public DateTime? DateOfEndPayment { get; set; }
    }
}
