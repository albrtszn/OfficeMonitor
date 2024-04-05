using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfficeMonitor.DataBase.Models;

[Table("Department")]
public partial class Department
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("description")]
    [StringLength(100)]
    public string? Description { get; set; }

    [InverseProperty("IdDepartmentNavigation")]
    public virtual ICollection<DepartmentApp> DepartmentApps { get; set; } = new List<DepartmentApp>();

    [InverseProperty("IdDepartmentNavigation")]
    public virtual ICollection<DepartmentManager> DepartmentManagers { get; set; } = new List<DepartmentManager>();

    [InverseProperty("IdDepartmentNavigation")]
    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    [InverseProperty("IdDepartmentNavigation")]
    public virtual ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();
}
