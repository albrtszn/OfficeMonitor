using OfficeMonitor.DTOs;
using OfficeMonitor.Models.Profile;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Departments
{
    public class UpdateDepartmentCompanyModel
    {
        [Required]
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(100)]
        [Required]
        public string Description { get; set; }
        [Required]
        public List<ProfileModel> Profiles { get; set; } = new List<ProfileModel>();
    }
}
