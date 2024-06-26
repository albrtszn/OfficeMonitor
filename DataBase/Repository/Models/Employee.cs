﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Employee")]
[Index("Login", Name = "UQ__Employee__7838F272C42D4C36", IsUnique = true)]
public partial class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idClaimRole")]
    public int? IdClaimRole { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("surname")]
    [StringLength(50)]
    public string? Surname { get; set; }

    [Column("patronamic")]
    [StringLength(50)]
    public string? Patronamic { get; set; }

    [Column("login")]
    [StringLength(255)]
    public string Login { get; set; } = null!;

    [Column("password")]
    [StringLength(150)]
    public string Password { get; set; } = null!;

    [Column("idProfile")]
    public int? IdProfile { get; set; }

    [InverseProperty("IdEmployeeNavigation")]
    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    [ForeignKey("IdClaimRole")]
    [InverseProperty("Employees")]
    public virtual ClaimRole? IdClaimRoleNavigation { get; set; }

    [ForeignKey("IdProfile")]
    [InverseProperty("Employees")]
    public virtual Profile? IdProfileNavigation { get; set; }

    [InverseProperty("IdEmployeeNavigation")]
    public virtual ICollection<TokenEmployee> TokenEmployees { get; set; } = new List<TokenEmployee>();
}
