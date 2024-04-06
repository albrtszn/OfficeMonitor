using System;
using System.Collections.Generic;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Action = DataBase.Repository.Models.Action;

namespace DataBase.Repository;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Action> Actions { get; set; }

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
        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Action__3213E83F6FA3F5BD");

            entity.HasOne(d => d.IdAppNavigation).WithMany(p => p.Actions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Action__idApp__114A936A");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Actions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Action__idEmploy__10566F31");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3213E83F6D11D175");
        });

        modelBuilder.Entity<App>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__App__3213E83FFE78196A");

            entity.HasOne(d => d.IdTypeAppNavigation).WithMany(p => p.Apps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__App__idTypeApp__09A971A2");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company__3213E83F97F90639");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.Companies)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Company__idPlan__787EE5A0");
        });

        modelBuilder.Entity<CustomerRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3213E83F04E7CC7C");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83F5960DE38");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idCom__7B5B524B");
        });

        modelBuilder.Entity<DepartmentApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83F93C4BBB2");

            entity.HasOne(d => d.IdAppNavigation).WithMany(p => p.DepartmentApps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idApp__0D7A0286");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.DepartmentApps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idDep__0C85DE4D");
        });

        modelBuilder.Entity<DepartmentManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83F98FB60AE");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.DepartmentManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idDep__17F790F9");

            entity.HasOne(d => d.IdManagerNavigation).WithMany(p => p.DepartmentManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idMan__18EBB532");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F6442A706");

            entity.HasOne(d => d.IdProfileNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idProf__04E4BC85");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3213E83FDAF641B2");

            entity.HasOne(d => d.IdProfileNavigation).WithMany(p => p.Managers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Manager__idProfi__151B244E");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Plans__3213E83FE2C43149");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Profile__3213E83F95CE82B3");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.Profiles)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Profile__idDepar__01142BA1");
        });

        modelBuilder.Entity<TypeApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeApp__3213E83FCA0EC5C0");
        });

        modelBuilder.Entity<WorkTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkTime__3213E83FC19CF333");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.WorkTimes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__WorkTime__idDepa__7E37BEF6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
