using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.App
{
    public class AddAppModel
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int IdTypeApp { get; set; }
    }
}
