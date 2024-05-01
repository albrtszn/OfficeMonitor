using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddDepartmentManagerModel
    {
        [Required]
        public int IdDepartment { get; set; }
        [Required]
        public int IdManager { get; set; }
    }
}
