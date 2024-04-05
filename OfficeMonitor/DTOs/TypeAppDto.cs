using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class TypeAppDto
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
    }
}
