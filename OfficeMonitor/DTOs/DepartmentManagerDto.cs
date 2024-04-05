using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeMonitor.DTOs
{
    public class DepartmentManagerDto
    {
        public int Id { get; set; }
        public int? IdDepartment { get; set; }
        public int? IdManager { get; set; }
    }
}
