using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Models.Employee
{
    public class GetEmployeeWithInfoModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [StringLength(50)]
        public string Patronamic { get; set; }

        [Required]
        [StringLength(255)]
        public string Login { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Password { get; set; } = null!;

        [Required]
        public ProfileDto Profile { get; set; }
        [Required]
        public decimal WorkPercentage { get; set; }
        [Required]
        public decimal IdlePercentage { get; set; }
    }
}
