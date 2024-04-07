using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddDepartmentModel
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(100)]
        [Required]
        public string Description { get; set; }
        [Required]
        public int IdCompany { get; set; }
    }
}
