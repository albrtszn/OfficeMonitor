using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfficeMonitor.DataBase.Models;

[Table("TypeApp")]
public partial class TypeApp
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("description")]
    [StringLength(100)]
    public string? Description { get; set; }

    [InverseProperty("IdTypeAppNavigation")]
    public virtual ICollection<App> Apps { get; set; } = new List<App>();
}
