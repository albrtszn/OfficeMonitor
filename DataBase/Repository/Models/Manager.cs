using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Manager")]
[Index("Login", Name = "UQ__Manager__7838F2725989825E", IsUnique = true)]
public partial class Manager
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idClaimRole")]
    public int? IdClaimRole { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("surname")]
    [StringLength(50)]
    public string? Surname { get; set; }

    [Column("patronamic")]
    [StringLength(50)]
    public string? Patronamic { get; set; }

    [Column("login")]
    [StringLength(255)]
    public string Login { get; set; } = null!;

    [Column("password")]
    [StringLength(150)]
    public string Password { get; set; } = null!;

    [Column("idProfile")]
    public int? IdProfile { get; set; }

    [InverseProperty("IdManagerNavigation")]
    public virtual ICollection<DepartmentManager> DepartmentManagers { get; set; } = new List<DepartmentManager>();

    [ForeignKey("IdClaimRole")]
    [InverseProperty("Managers")]
    public virtual ClaimRole? IdClaimRoleNavigation { get; set; }

    [ForeignKey("IdProfile")]
    [InverseProperty("Managers")]
    public virtual Profile? IdProfileNavigation { get; set; }

    [InverseProperty("IdManagerNavigation")]
    public virtual ICollection<TokenManager> TokenManagers { get; set; } = new List<TokenManager>();
}
