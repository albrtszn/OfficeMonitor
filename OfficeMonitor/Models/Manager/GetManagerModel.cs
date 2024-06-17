using OfficeMonitor.DTOs;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Manager
{
    public class GetManagerModel
    {
        [Required]
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(50)]
        [Required]
        public string Surname { get; set; }
        [StringLength(50)]
        [Required]
        public string Patronamic { get; set; }
        [StringLength(255)]
        [Required]
        public string Login { get; set; } = null!;
        [StringLength(150)]
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public DepartmentDto Department { get; set; }
        [Required]
        public ProfileDto Profile { get; set; }
        [Required]
        public List<DepartmentDto> ManagedDepartments { get; set; }
    }
}
