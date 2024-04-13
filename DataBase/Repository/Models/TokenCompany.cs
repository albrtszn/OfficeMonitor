using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("TokenCompany")]
public partial class TokenCompany
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idCompany")]
    public int? IdCompany { get; set; }

    [Column("token")]
    [StringLength(250)]
    public string? Token { get; set; }

    [Column("dateOfCreation", TypeName = "datetime")]
    public DateTime? DateOfCreation { get; set; }

    [ForeignKey("IdCompany")]
    [InverseProperty("TokenCompanies")]
    public virtual Company? IdCompanyNavigation { get; set; }
}
