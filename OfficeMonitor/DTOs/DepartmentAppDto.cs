using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeMonitor.DTOs
{
    public class DepartmentAppDto
    {
        public int Id { get; set; }
        public int? IdDepartment { get; set; }
        public int? IdApp { get; set; }
    }
}
