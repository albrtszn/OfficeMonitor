using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfficeMonitor.DataBase.Models;

[Table("Action")]
public partial class Action
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idEmployee")]
    public int? IdEmployee { get; set; }

    [Column("idApp")]
    public int? IdApp { get; set; }

    [Column("date")]
    public DateOnly? Date { get; set; }

    [Column("startTime")]
    [Precision(0)]
    public TimeOnly? StartTime { get; set; }

    [Column("endTime")]
    [Precision(0)]
    public TimeOnly? EndTime { get; set; }

    [ForeignKey("IdApp")]
    [InverseProperty("Actions")]
    public virtual App? IdAppNavigation { get; set; }

    [ForeignKey("IdEmployee")]
    [InverseProperty("Actions")]
    public virtual Employee? IdEmployeeNavigation { get; set; }
}
