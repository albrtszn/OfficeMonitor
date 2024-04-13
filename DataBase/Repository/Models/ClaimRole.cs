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
    public virtual ICollection<TokenAdmin> TokenAdmins { get; set; } = new List<TokenAdmin>();

    [InverseProperty("IdClaimRoleNavigation")]
    public virtual ICollection<TokenCompany> TokenCompanies { get; set; } = new List<TokenCompany>();

    [InverseProperty("IdClaimRoleNavigation")]
    public virtual ICollection<TokenEmployee> TokenEmployees { get; set; } = new List<TokenEmployee>();

    [InverseProperty("IdClaimRoleNavigation")]
    public virtual ICollection<TokenManager> TokenManagers { get; set; } = new List<TokenManager>();
}
