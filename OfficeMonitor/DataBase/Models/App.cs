using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfficeMonitor.DataBase.Models;

[Table("App")]
public partial class App
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("idTypeApp")]
    public int? IdTypeApp { get; set; }

    [InverseProperty("IdAppNavigation")]
    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    [InverseProperty("IdAppNavigation")]
    public virtual ICollection<DepartmentApp> DepartmentApps { get; set; } = new List<DepartmentApp>();

    [ForeignKey("IdTypeApp")]
    [InverseProperty("Apps")]
    public virtual TypeApp? IdTypeAppNavigation { get; set; }
}
