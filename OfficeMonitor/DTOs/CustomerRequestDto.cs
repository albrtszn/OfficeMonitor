using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class CustomerRequestDto
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Email { get; set; } = null!;
        [StringLength(150)]
        public string? Name { get; set; }
        public bool? IsReplyed { get; set; }
    }
}
