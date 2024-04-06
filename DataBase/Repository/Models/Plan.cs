using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

public partial class Plan
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

    [Column("monthCost", TypeName = "decimal(15, 2)")]
    public decimal? MonthCost { get; set; }

    [Column("yearcost", TypeName = "decimal(15, 2)")]
    public decimal? Yearcost { get; set; }

    [Column("countOfEmployees")]
    public int? CountOfEmployees { get; set; }

    [InverseProperty("IdPlanNavigation")]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
