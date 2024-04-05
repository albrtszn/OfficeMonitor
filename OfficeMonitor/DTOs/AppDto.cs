using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class AppDto
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public int? IdTypeApp { get; set; }
    }
}
