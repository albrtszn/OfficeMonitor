using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class ClaimRoleDto
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
    }
}
