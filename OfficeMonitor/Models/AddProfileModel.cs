using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddProfileModel
    {
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "IdDepartment is required")]
        public int IdDepartment { get; set; }
    }
}
