using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("CustomerRequest")]
[Index("Email", Name = "UQ__Customer__AB6E616460A04541", IsUnique = true)]
public partial class CustomerRequest
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("name")]
    [StringLength(150)]
    public string? Name { get; set; }

    [Column("isReplyed")]
    public bool? IsReplyed { get; set; }
}
