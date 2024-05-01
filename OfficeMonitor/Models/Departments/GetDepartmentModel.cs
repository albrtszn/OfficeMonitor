using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.Departments
{
    public class GetDepartmentModel
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
        public int IdCompany { get; set; }
        [Required]
        public int CountOfWorkers { get; set; }
        [Required]
        public int CountOfManagers { get; set;}
    }
}
