﻿using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models
{
    public class AddDepartmentModel
    {
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
    }
}