using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class TokenCompanyDto
    {
        public int Id { get; set; }
        public int? IdCompany { get; set; }
        [StringLength(250)]
        public string? Token { get; set; }
        public DateTime? DateOfCreation { get; set; }
    }
}
