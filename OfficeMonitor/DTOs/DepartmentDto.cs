﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.DTOs
{
    public class DepartmentDto
    {
        [Required]
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string? Name { get; set; }
        [StringLength(100)]
        [Required]
        public string? Description { get; set; }
        [Required]
        public int? IdCompany { get; set; }
    }
}
