using Microsoft.EntityFrameworkCore;
using Metrix.Domain.Entities;

namespace Metrix.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> AdminUsers => Set<User>();
    public DbSet<HRUser> HRUsers => Set<HRUser>();
    public DbSet<SecurityUser> SecurityUsers => Set<SecurityUser>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<VisitLog> VisitLogs => Set<VisitLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Metrix_Seed");
        
        base.OnModelCreating(modelBuilder);

        // ================= ADMIN USER =================
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.PasswordHash)
                  .IsRequired();

            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.UpdatedAt).IsRequired(false);
        });

        // ================= HR USER =================
        modelBuilder.Entity<HRUser>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.PasswordHash)
                  .IsRequired();

            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.UpdatedAt).IsRequired(false);
        });

        // ================= SECURITY USER =================
        modelBuilder.Entity<SecurityUser>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.PasswordHash)
                  .IsRequired();

            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.UpdatedAt).IsRequired(false);
        });

        // ================= INVITATION =================
        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Token).IsUnique();

            entity.Property(x => x.VisitorName)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.VisitorEmail)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.Purpose)
                  .HasMaxLength(300);

            entity.Property(x => x.Token)
                  .IsRequired();

            entity.Property(x => x.Status)
                  .HasConversion<string>()
                  .IsRequired();

            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.UpdatedAt).IsRequired(false);

            entity.HasOne(x => x.CreatedByHR)
                  .WithMany()
                  .HasForeignKey(x => x.CreatedByHRId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ================= VISITOR =================
        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Phone)
                  .IsRequired()
                  .HasMaxLength(15);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.IDProofType)
                  .HasConversion<string>()
                  .IsRequired();

            entity.Property(x => x.IDProofNumber)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(x => x.PhotoPath)
                  .HasMaxLength(300);

            entity.Property(x => x.RegistrationId)
                  .HasMaxLength(50);

            entity.Property(x => x.Status)
                  .HasConversion<string>()
                  .IsRequired();

            entity.Property(x => x.SubmittedAt).IsRequired();
            entity.Property(x => x.UpdatedAt).IsRequired(false);

            // 1-1 Invitation
            entity.HasOne(x => x.Invitation)
                  .WithOne()
                  .HasForeignKey<Visitor>(x => x.InvitationId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Approved By HR
            entity.HasOne(x => x.ApprovedByHR)
                  .WithMany()
                  .HasForeignKey(x => x.ApprovedByHRId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ================= VISIT LOG =================
        modelBuilder.Entity<VisitLog>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EntryTime).IsRequired();
            entity.Property(x => x.ExitTime);

            entity.HasOne(x => x.Visitor)
                  .WithMany(v => v.VisitLogs)
                  .HasForeignKey(x => x.VisitorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.VerifiedBySecurity)
                  .WithMany()
                  .HasForeignKey(x => x.VerifiedBySecurityId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    // 🔥 AUTO UPDATE UpdatedAt
    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e =>
                e.State == EntityState.Modified &&
                e.Properties.Any(p => p.Metadata.Name == "UpdatedAt"));

        foreach (var entry in entries)
        {
            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}