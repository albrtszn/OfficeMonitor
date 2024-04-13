using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Admin")]
[Index("Login", Name = "UQ__Admin__7838F272CBE7396C", IsUnique = true)]
public partial class Admin
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

    [ForeignKey("IdClaimRole")]
    [InverseProperty("Admins")]
    public virtual ClaimRole? IdClaimRoleNavigation { get; set; }

    [InverseProperty("IdAdminNavigation")]
    public virtual ICollection<TokenAdmin> TokenAdmins { get; set; } = new List<TokenAdmin>();
}
