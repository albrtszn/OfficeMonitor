using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Company")]
[Index("Login", Name = "UQ__Company__7838F272626A30BD", IsUnique = true)]
public partial class Company
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("login")]
    [StringLength(255)]
    public string Login { get; set; } = null!;

    [Column("password")]
    [StringLength(150)]
    public string Password { get; set; } = null!;

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("description")]
    [StringLength(100)]
    public string? Description { get; set; }

    [Column("idPlan")]
    public int? IdPlan { get; set; }

    [Column("balance", TypeName = "decimal(15, 2)")]
    public decimal? Balance { get; set; }

    [Column("isActive")]
    public bool? IsActive { get; set; }

    [Column("isBanned")]
    public bool? IsBanned { get; set; }

    [Column("dateOfRegister", TypeName = "datetime")]
    public DateTime? DateOfRegister { get; set; }

    [Column("dateOfEndPayment", TypeName = "datetime")]
    public DateTime? DateOfEndPayment { get; set; }

    [InverseProperty("IdCompanyNavigation")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    [ForeignKey("IdPlan")]
    [InverseProperty("Companies")]
    public virtual Plan? IdPlanNavigation { get; set; }

    [InverseProperty("IdCompanyNavigation")]
    public virtual ICollection<TokenCompany> TokenCompanies { get; set; } = new List<TokenCompany>();
}
