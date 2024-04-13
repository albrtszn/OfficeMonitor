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
            entity.HasKey(e => e.Id).HasName("PK__Action__3213E83F1E0304AF");

            entity.HasOne(d => d.IdAppNavigation).WithMany(p => p.Actions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Action__idApp__5CD6CB2B");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Actions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Action__idEmploy__5BE2A6F2");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3213E83F7275672E");
        });

        modelBuilder.Entity<App>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__App__3213E83FF07B5596");

            entity.HasOne(d => d.IdTypeAppNavigation).WithMany(p => p.Apps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__App__idTypeApp__5535A963");
        });

        modelBuilder.Entity<ClaimRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClaimRol__3213E83F113B7205");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company__3213E83FF116A134");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.Companies)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Company__idPlan__4316F928");
        });

        modelBuilder.Entity<CustomerRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3213E83F0F6C5BDE");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83F131F6EB0");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idCom__45F365D3");
        });

        modelBuilder.Entity<DepartmentApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83F56EAF32D");

            entity.HasOne(d => d.IdAppNavigation).WithMany(p => p.DepartmentApps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idApp__59063A47");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.DepartmentApps)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idDep__5812160E");
        });

        modelBuilder.Entity<DepartmentManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3213E83FF9106335");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.DepartmentManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idDep__6477ECF3");

            entity.HasOne(d => d.IdManagerNavigation).WithMany(p => p.DepartmentManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Departmen__idMan__656C112C");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F2EDCB01A");

            entity.HasOne(d => d.IdProfileNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idProf__5070F446");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3213E83F1CE6E7DA");

            entity.HasOne(d => d.IdProfileNavigation).WithMany(p => p.Managers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Manager__idProfi__619B8048");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Plans__3213E83F779A65BD");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Profile__3213E83F314DDE95");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.Profiles)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Profile__idDepar__4BAC3F29");
        });

        modelBuilder.Entity<TokenAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenAdm__3213E83F0B8C3B52");

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.TokenAdmins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenAdmi__idAdm__71D1E811");

            entity.HasOne(d => d.IdClaimRoleNavigation).WithMany(p => p.TokenAdmins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenAdmi__idCla__72C60C4A");
        });

        modelBuilder.Entity<TokenCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenCom__3213E83F054E9B0C");

            entity.HasOne(d => d.IdClaimRoleNavigation).WithMany(p => p.TokenCompanies)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenComp__idCla__76969D2E");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.TokenCompanies)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenComp__idCom__75A278F5");
        });

        modelBuilder.Entity<TokenEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenEmp__3213E83F2A1DAE6A");

            entity.HasOne(d => d.IdClaimRoleNavigation).WithMany(p => p.TokenEmployees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenEmpl__idCla__6B24EA82");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TokenEmployees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenEmpl__idEmp__6A30C649");
        });

        modelBuilder.Entity<TokenManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenMan__3213E83FDBB96162");

            entity.HasOne(d => d.IdClaimRoleNavigation).WithMany(p => p.TokenManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenMana__idCla__6EF57B66");

            entity.HasOne(d => d.IdManagerNavigation).WithMany(p => p.TokenManagers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenMana__idMan__6E01572D");
        });

        modelBuilder.Entity<TypeApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeApp__3213E83F083F70B3");
        });

        modelBuilder.Entity<WorkTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkTime__3213E83F2084F89F");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.WorkTimes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__WorkTime__idDepa__48CFD27E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
