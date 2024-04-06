using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddAppModel
    {
        [StringLength(50)]
        public string? Name { get; set; }
        public int? IdTypeApp { get; set; }
    }
}
