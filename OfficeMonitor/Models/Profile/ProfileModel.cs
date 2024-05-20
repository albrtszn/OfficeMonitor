using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Profile
{
    public class ProfileModel
    {
        [Required]
        public int Id { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
