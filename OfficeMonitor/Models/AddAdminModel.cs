using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddAdminModel
    {
        [StringLength(50)]
        [Required]
        public string? Name { get; set; }
        [Required]
        [StringLength(50)]
        public string? Surname { get; set; }
        [Required]
        [StringLength(50)]
        public string? Patronamic { get; set; }
        [Required]
        [StringLength(255)]
        public string Login { get; set; } = null!;
        [Required]
        [StringLength(150)]
        public string Password { get; set; } = null!;
    }
}
