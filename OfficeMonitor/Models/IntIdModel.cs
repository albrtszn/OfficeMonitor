using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class IntIdModel
    {
        //[RegularExpression("[a-z,0-9]{5}")]
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }
    }
}
