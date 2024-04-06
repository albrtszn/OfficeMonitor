using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddCustomerRequestModel
    {
        [StringLength(255)]
        public string Email { get; set; } = null!;
        [StringLength(150)]
        public string? Name { get; set; }
    }
}
