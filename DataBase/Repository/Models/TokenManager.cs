using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("TokenManager")]
public partial class TokenManager
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idManager")]
    public int? IdManager { get; set; }

    [Column("idClaimRole")]
    public int? IdClaimRole { get; set; }

    [Column("token")]
    [StringLength(250)]
    public string? Token { get; set; }

    [Column("dateOfCreation", TypeName = "datetime")]
    public DateTime? DateOfCreation { get; set; }

    [ForeignKey("IdClaimRole")]
    [InverseProperty("TokenManagers")]
    public virtual ClaimRole? IdClaimRoleNavigation { get; set; }

    [ForeignKey("IdManager")]
    [InverseProperty("TokenManagers")]
    public virtual Manager? IdManagerNavigation { get; set; }
}
