using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.TypeApp
{
    public class UpdateTypeAppModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
    }
}
