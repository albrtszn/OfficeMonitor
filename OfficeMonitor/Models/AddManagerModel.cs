using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddManagerModel
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(50)]
        [Required]
        public string Surname { get; set; }
        [StringLength(50)]
        [Required]
        public string Patronamic { get; set; }
        [StringLength(255)]
        [Required]
        public string Login { get; set; } = null!;
        [StringLength(150)]
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public int IdProfile { get; set; }
    }
}
