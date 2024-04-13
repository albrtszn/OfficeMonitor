using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class AdminDto
    {
        public int Id { get; set; }
        public int? IdClaimRole { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(50)]
        public string? Surname { get; set; }
        [StringLength(50)]
        public string? Patronamic { get; set; }
        [StringLength(255)]
        public string Login { get; set; } = null!;
        [StringLength(150)]
        public string Password { get; set; } = null!;
    }
}
