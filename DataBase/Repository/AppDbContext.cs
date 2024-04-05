using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OfficeMonitor.DataBase.Models;

namespace OfficeMonitor.DataBase;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DataBase.Models.Action> Actions { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<App> Apps { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CustomerRequest> CustomerRequests { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentApp> DepartmentApps { get; set; }

    public virtual DbSet<DepartmentManager> DepartmentManagers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<TypeApp> TypeApps { get; set; }

    public virtual DbSet<WorkTime> WorkTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-VV5F8B8;Database=OfficeMonitorDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DataBase.Models.Action>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Action__3213E83FECD7C6A8");

            entity.HasOne(d => d.IdAppNavigation).WithMany(p => p.Actions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Action__idApp__59063A47");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Actions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Action__idEmploy__5812160E");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3213E83F314A2E1C");
        });

        modelBuilder.Entity<App>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__App__3213E83F6AFE4313");

            entity.HasOne(d => d.IdTypeAppNavigation).WithMany(p => p.Apps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__App__idTypeApp__5165187F");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company__3213E83F46E623D5");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.Companies)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Company__idPlan__403A8C7D");
        });

        modelBuilder.Entity<CustomerRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3213E83FC35E80C9");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83FB03BAD44");
        });

        modelBuilder.Entity<DepartmentApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83FB8209132");

            entity.HasOne(d => d.IdAppNavigation).WithMany(p => p.DepartmentApps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idApp__5535A963");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.DepartmentApps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idDep__5441852A");
        });

        modelBuilder.Entity<DepartmentManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83FB4316F6A");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.DepartmentManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idDep__656C112C");

            entity.HasOne(d => d.IdManagerNavigation).WithMany(p => p.DepartmentManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idMan__66603565");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FE4419652");

            entity.HasOne(d => d.IdProfileNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idProf__4CA06362");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3213E83F5BD2B439");

            entity.HasOne(d => d.IdProfileNavigation).WithMany(p => p.Managers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Manager__idProfi__628FA481");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Plans__3213E83FA3F4830B");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Profile__3213E83F702E3E66");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Profiles)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Profile__idCompa__47DBAE45");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.Profiles)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Profile__idDepar__48CFD27E");
        });

        modelBuilder.Entity<TypeApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeApp__3213E83F856B09BE");
        });

        modelBuilder.Entity<WorkTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkTime__3213E83FAF186B64");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.WorkTimes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__WorkTime__idDepa__44FF419A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
