using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class ProfileDto
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public int? IdDepartment { get; set; }
    }
}
