using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.ClaimRole
{
    public class AddClaimRoleModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
