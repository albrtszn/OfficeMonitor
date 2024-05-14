using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class TimeSummaryModel
    {
        [Required]
        private string _Hours = "00";
        public string Hours {
            get { return _Hours; }
            set
            {
                if (value.IsNullOrEmpty())
                    _Hours = "00";
                if (value.Length == 1)
                    _Hours = "0" + value;
                else
                    _Hours = value;
            } 
        }
        [Required]
        private string _Minutes = "00";
        public string Minutes
        {
            get { return _Minutes; }
            set
            {                    
                if (value.IsNullOrEmpty())
                    _Minutes = "00";
                if (value.Length == 1)
                    _Minutes = "0" + value;
                else
                    _Minutes = value;
            }
        }
    }
}
