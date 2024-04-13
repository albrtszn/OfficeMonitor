using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Company
{
    public class UpdateCompanyModel
    {
        [Required]
        public int Id { get; set; }
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
        public int IdPlan { get; set; }
        [Required]
        public decimal? Balance { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool? IsBanned { get; set; }
        /*public DateTime? DateOfRegister { get; set; }
        public DateTime? DateOfEndPayment { get; set; }*/
    }
}
