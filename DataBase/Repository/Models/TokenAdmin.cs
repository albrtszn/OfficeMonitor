using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("TokenAdmin")]
public partial class TokenAdmin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idAdmin")]
    public int? IdAdmin { get; set; }

    [Column("token")]
    [StringLength(250)]
    public string? Token { get; set; }

    [Column("dateOfCreation", TypeName = "datetime")]
    public DateTime? DateOfCreation { get; set; }

    [ForeignKey("IdAdmin")]
    [InverseProperty("TokenAdmins")]
    public virtual Admin? IdAdminNavigation { get; set; }
}
