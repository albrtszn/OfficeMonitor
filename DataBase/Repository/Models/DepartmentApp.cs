using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("DepartmentApp")]
public partial class DepartmentApp
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idDepartment")]
    public int? IdDepartment { get; set; }

    [Column("idApp")]
    public int? IdApp { get; set; }

    [ForeignKey("IdApp")]
    [InverseProperty("DepartmentApps")]
    public virtual App? IdAppNavigation { get; set; }

    [ForeignKey("IdDepartment")]
    [InverseProperty("DepartmentApps")]
    public virtual Department? IdDepartmentNavigation { get; set; }
}
