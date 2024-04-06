using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("WorkTime")]
public partial class WorkTime
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idDepartment")]
    public int? IdDepartment { get; set; }

    [Column("startTime")]
    [Precision(0)]
    public TimeOnly? StartTime { get; set; }

    [Column("endTime")]
    [Precision(0)]
    public TimeOnly? EndTime { get; set; }

    [ForeignKey("IdDepartment")]
    [InverseProperty("WorkTimes")]
    public virtual Department? IdDepartmentNavigation { get; set; }
}
