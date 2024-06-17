using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Request
{
    public class AddCustomerRequestModel
    {        
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(150)]
        public string Password { get; set; }
        [Required]
        public int IdPlan {  get; set; }
    }
}
