﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OfficeMonitor.Models.WorkTime
{
    public class AddWorkTimeModel
    {
        [Required]
        public int IdDepartment { get; set; }

        [Required]
        [Precision(0)]
        public TimeOnly StartTime { get; set; }
        [Required]
        [Precision(0)]
        public TimeOnly EndTime { get; set; }
    }
}
