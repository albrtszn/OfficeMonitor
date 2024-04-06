using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Profile")]
public partial class Profile
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("idDepartment")]
    public int? IdDepartment { get; set; }

    [InverseProperty("IdProfileNavigation")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [ForeignKey("IdDepartment")]
    [InverseProperty("Profiles")]
    public virtual Department? IdDepartmentNavigation { get; set; }

    [InverseProperty("IdProfileNavigation")]
    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();
}
