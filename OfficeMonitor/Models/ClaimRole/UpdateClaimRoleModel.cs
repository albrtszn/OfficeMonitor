using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.ClaimRole
{
    public class UpdateClaimRoleModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
