using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("ClaimRole")]
public partial class ClaimRole
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("IdClaimRoleNavigation")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    [InverseProperty("IdClaimRoleNavigation")]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    [InverseProperty("IdClaimRoleNavigation")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [InverseProperty("IdClaimRoleNavigation")]
    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();
}
