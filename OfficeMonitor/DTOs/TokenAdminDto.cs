using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class TokenAdminDto
    {
        public int Id { get; set; }
        public int? IdAdmin { get; set; }
        [StringLength(250)]
        public string? Token { get; set; }
        public DateTime? DateOfCreation { get; set; }
    }
}
