using Microsoft.EntityFrameworkCore;
using AuditManagement.Domain.Entities;

namespace AuditManagement.Persistence;

/// <summary>
/// Main DbContext for the Audit Management application
/// </summary>
public class AuditManagementDbContext : DbContext
{
    public AuditManagementDbContext(DbContextOptions<AuditManagementDbContext> options)
        : base(options)
    {
    }

    // Tenant and User Management
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    // Framework and Control Management
    public DbSet<Framework> Frameworks => Set<Framework>();
    public DbSet<Control> Controls => Set<Control>();
    public DbSet<FrameworkControl> FrameworkControls => Set<FrameworkControl>();

    // Policy and Evidence
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<Evidence> Evidence => Set<Evidence>();
    public DbSet<EvidenceAuditLog> EvidenceAuditLogs => Set<EvidenceAuditLog>();

    // Risk Management
    public DbSet<Risk> Risks => Set<Risk>();
    public DbSet<RiskControl> RiskControls => Set<RiskControl>();

    // Vendor Management
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<VendorQuestionnaire> VendorQuestionnaires => Set<VendorQuestionnaire>();
    public DbSet<VendorQuestion> VendorQuestions => Set<VendorQuestion>();
    public DbSet<VendorRisk> VendorRisks => Set<VendorRisk>();

    // Task Management
    public DbSet<RemediationTask> RemediationTasks => Set<RemediationTask>();
    public DbSet<TaskNotification> TaskNotifications => Set<TaskNotification>();

    // Integrations
    public DbSet<Integration> Integrations => Set<Integration>();
    public DbSet<IntegrationEvent> IntegrationEvents => Set<IntegrationEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Tenant
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.TenantCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.TenantCode).IsUnique();
            entity.HasMany(e => e.Users).WithOne(u => u.Tenant).HasForeignKey(u => u.TenantId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Frameworks).WithOne(f => f.Tenant).HasForeignKey(f => f.TenantId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Controls).WithOne(c => c.Tenant).HasForeignKey(c => c.TenantId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Policies).WithOne(p => p.Tenant).HasForeignKey(p => p.TenantId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Risks).WithOne(r => r.Tenant).HasForeignKey(r => r.TenantId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Vendors).WithOne(v => v.Tenant).HasForeignKey(v => v.TenantId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Tasks).WithOne(t => t.Tenant).HasForeignKey(t => t.TenantId).OnDelete(DeleteBehavior.Cascade);
        });

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => new { e.TenantId, e.Email }).IsUnique();
            entity.HasMany(e => e.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Role
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.Cascade);
        });

        // Configure UserRole
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        // Configure Framework
        modelBuilder.Entity<Framework>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
            entity.HasMany(e => e.FrameworkControls).WithOne(fc => fc.Framework).HasForeignKey(fc => fc.FrameworkId).OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Control
        modelBuilder.Entity<Control>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
            entity.HasMany(e => e.FrameworkControls).WithOne(fc => fc.Control).HasForeignKey(fc => fc.ControlId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Evidence).WithOne(ev => ev.Control).HasForeignKey(ev => ev.ControlId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.RiskControls).WithOne(rc => rc.Control).HasForeignKey(rc => rc.ControlId).OnDelete(DeleteBehavior.Cascade);
        });

        // Configure FrameworkControl
        modelBuilder.Entity<FrameworkControl>(entity =>
        {
            entity.HasKey(e => new { e.FrameworkId, e.ControlId });
        });

        // Configure Policy
        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.Evidence).WithOne(ev => ev.Policy).HasForeignKey(ev => ev.PolicyId).OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Evidence
        modelBuilder.Entity<Evidence>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.AuditLogs).WithOne(log => log.Evidence).HasForeignKey(log => log.EvidenceId).OnDelete(DeleteBehavior.Cascade);
        });

        // Configure EvidenceAuditLog
        modelBuilder.Entity<EvidenceAuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Configure Risk
        modelBuilder.Entity<Risk>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.RiskControls).WithOne(rc => rc.Risk).HasForeignKey(rc => rc.RiskId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.RemediationTasks).WithOne(t => t.Risk).HasForeignKey(t => t.RiskId).OnDelete(DeleteBehavior.SetNull);
        });

        // Configure RiskControl
        modelBuilder.Entity<RiskControl>(entity =>
        {
            entity.HasKey(e => new { e.RiskId, e.ControlId });
        });

        // Configure Vendor
        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.Questionnaires).WithOne(q => q.Vendor).HasForeignKey(q => q.VendorId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.VendorRisks).WithOne(vr => vr.Vendor).HasForeignKey(vr => vr.VendorId).OnDelete(DeleteBehavior.Cascade);
        });

        // Configure VendorQuestionnaire
        modelBuilder.Entity<VendorQuestionnaire>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.Questions).WithOne(q => q.Questionnaire).HasForeignKey(q => q.QuestionnaireId).OnDelete(DeleteBehavior.Cascade);
        });

        // Configure VendorQuestion
        modelBuilder.Entity<VendorQuestion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Question).IsRequired();
        });

        // Configure VendorRisk
        modelBuilder.Entity<VendorRisk>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Configure RemediationTask
        modelBuilder.Entity<RemediationTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.Notifications).WithOne(n => n.Task).HasForeignKey(n => n.TaskId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Control).WithMany().HasForeignKey(e => e.ControlId).OnDelete(DeleteBehavior.SetNull);
        });

        // Configure TaskNotification
        modelBuilder.Entity<TaskNotification>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Configure Integration
        modelBuilder.Entity<Integration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.Events).WithOne(ie => ie.Integration).HasForeignKey(ie => ie.IntegrationId).OnDelete(DeleteBehavior.Cascade);
        });

        // Configure IntegrationEvent
        modelBuilder.Entity<IntegrationEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Add global query filters for soft delete
        modelBuilder.Entity<Tenant>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Framework>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Control>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Policy>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Evidence>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Risk>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Vendor>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Integration>().HasQueryFilter(e => !e.IsDeleted);
    }
}
