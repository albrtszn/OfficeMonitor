using OfficeMonitor.Models.Profile;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Departments
{
    public class AddDepartmentCompanyModel
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(100)]
        [Required]
        public string Description { get; set; }
        [Required]
        //public string Profiles { get; set; }
        public List<string> Profiles { get; set; } = new List<string>();
        //public List<AddProfileSimpleModel> Profiles { get; set; } = new List<AddProfileSimpleModel>();
    }
}
