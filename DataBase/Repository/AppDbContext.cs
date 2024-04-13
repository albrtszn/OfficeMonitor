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

    public virtual DbSet<ClaimRole> ClaimRoles { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CustomerRequest> CustomerRequests { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentApp> DepartmentApps { get; set; }

    public virtual DbSet<DepartmentManager> DepartmentManagers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<TokenAdmin> TokenAdmins { get; set; }

    public virtual DbSet<TokenCompany> TokenCompanies { get; set; }

    public virtual DbSet<TokenEmployee> TokenEmployees { get; set; }

    public virtual DbSet<TokenManager> TokenManagers { get; set; }

    public virtual DbSet<TypeApp> TypeApps { get; set; }

    public virtual DbSet<WorkTime> WorkTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-VV5F8B8;Database=OfficeMonitorDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Action__3213E83F9869D344");

            entity.HasOne(d => d.IdAppNavigation).WithMany(p => p.Actions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Action__idApp__619B8048");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Actions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Action__idEmploy__60A75C0F");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3213E83FFE0E42DA");

            entity.HasOne(d => d.IdClaimRoleNavigation).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Admin__idClaimRo__3A81B327");
        });

        modelBuilder.Entity<App>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__App__3213E83FD2C1EDCC");

            entity.HasOne(d => d.IdTypeAppNavigation).WithMany(p => p.Apps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__App__idTypeApp__59FA5E80");
        });

        modelBuilder.Entity<ClaimRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClaimRol__3213E83FE8BC1807");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company__3213E83F6DFF4EFE");

            entity.HasOne(d => d.IdClaimRoleNavigation).WithMany(p => p.Companies)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Company__idClaim__44FF419A");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.Companies)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Company__idPlan__46E78A0C");
        });

        modelBuilder.Entity<CustomerRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3213E83F4EA98EB7");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83F49763EC3");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idCom__49C3F6B7");
        });

        modelBuilder.Entity<DepartmentApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83F05D3704F");

            entity.HasOne(d => d.IdAppNavigation).WithMany(p => p.DepartmentApps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idApp__5DCAEF64");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.DepartmentApps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idDep__5CD6CB2B");
        });

        modelBuilder.Entity<DepartmentManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83F98A0128F");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.DepartmentManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idDep__6A30C649");

            entity.HasOne(d => d.IdManagerNavigation).WithMany(p => p.DepartmentManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idMan__6B24EA82");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FCFE031C6");

            entity.HasOne(d => d.IdClaimRoleNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idClai__534D60F1");

            entity.HasOne(d => d.IdProfileNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idProf__5535A963");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3213E83F87B7637A");

            entity.HasOne(d => d.IdClaimRoleNavigation).WithMany(p => p.Managers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Manager__idClaim__656C112C");

            entity.HasOne(d => d.IdProfileNavigation).WithMany(p => p.Managers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Manager__idProfi__6754599E");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Plans__3213E83F309994EE");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Profile__3213E83F26A4B1C2");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.Profiles)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Profile__idDepar__4F7CD00D");
        });

        modelBuilder.Entity<TokenAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenAdm__3213E83F980AAFAF");

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.TokenAdmins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenAdmi__idAdm__73BA3083");
        });

        modelBuilder.Entity<TokenCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenCom__3213E83F77827610");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.TokenCompanies)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenComp__idCom__76969D2E");
        });

        modelBuilder.Entity<TokenEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenEmp__3213E83F25EC08EB");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TokenEmployees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenEmpl__idEmp__6E01572D");
        });

        modelBuilder.Entity<TokenManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenMan__3213E83F9AA0BED7");

            entity.HasOne(d => d.IdManagerNavigation).WithMany(p => p.TokenManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenMana__idMan__70DDC3D8");
        });

        modelBuilder.Entity<TypeApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeApp__3213E83F3882A751");
        });

        modelBuilder.Entity<WorkTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkTime__3213E83FB206D91E");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.WorkTimes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__WorkTime__idDepa__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
