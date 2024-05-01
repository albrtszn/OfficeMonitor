using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Manager
{
    public class UpdateManagerModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
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
        [Required]
        public int? IdProfile { get; set; }
    }
}
