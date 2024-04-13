using DataBase.Repository.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class TokenEmployeeDto
    {
        public int Id { get; set; }
        public int? IdEmployee { get; set; }
        public int? IdClaimRole { get; set; }
        [StringLength(250)]
        public string? Token { get; set; }
        public DateTime? DateOfCreation { get; set; }
    }
}
