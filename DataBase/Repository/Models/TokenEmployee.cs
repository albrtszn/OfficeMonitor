using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("TokenEmployee")]
public partial class TokenEmployee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idEmployee")]
    public int? IdEmployee { get; set; }

    [Column("idClaimRole")]
    public int? IdClaimRole { get; set; }

    [Column("token")]
    [StringLength(250)]
    public string? Token { get; set; }

    [Column("dateOfCreation", TypeName = "datetime")]
    public DateTime? DateOfCreation { get; set; }

    [ForeignKey("IdClaimRole")]
    [InverseProperty("TokenEmployees")]
    public virtual ClaimRole? IdClaimRoleNavigation { get; set; }

    [ForeignKey("IdEmployee")]
    [InverseProperty("TokenEmployees")]
    public virtual Employee? IdEmployeeNavigation { get; set; }
}
