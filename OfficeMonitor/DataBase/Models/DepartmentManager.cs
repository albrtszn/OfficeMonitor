using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfficeMonitor.DataBase.Models;

[Table("DepartmentManager")]
public partial class DepartmentManager
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idDepartment")]
    public int? IdDepartment { get; set; }

    [Column("idManager")]
    public int? IdManager { get; set; }

    [ForeignKey("IdDepartment")]
    [InverseProperty("DepartmentManagers")]
    public virtual Department? IdDepartmentNavigation { get; set; }

    [ForeignKey("IdManager")]
    [InverseProperty("DepartmentManagers")]
    public virtual Manager? IdManagerNavigation { get; set; }
}
