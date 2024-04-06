using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddProfileModel
    {
        [StringLength(50)]
        public string? Name { get; set; }
        public int? IdCompany { get; set; }
        public int? IdDepartment { get; set; }
    }
}
