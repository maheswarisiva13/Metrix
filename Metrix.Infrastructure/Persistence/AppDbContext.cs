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
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Email)
                  .IsUnique();

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.PasswordHash)
                  .IsRequired();
        });

        // ================= HR USER =================
        modelBuilder.Entity<HRUser>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Email)
                  .IsUnique();

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.PasswordHash)
                  .IsRequired();
        });

        // ================= SECURITY USER =================
        modelBuilder.Entity<SecurityUser>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Email)
                  .IsUnique();

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.PasswordHash)
                  .IsRequired();
        });

        // ================= INVITATION =================
        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Token)
                  .IsUnique();

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
                  .HasMaxLength(10);

            entity.Property(x => x.Email)
                  .HasMaxLength(150);

            entity.Property(x => x.IDProofType)
                  .HasConversion<string>();

            entity.Property(x => x.IDProofNumber)
                  .HasMaxLength(100);

            entity.Property(x => x.Status)
                  .HasConversion<string>();

            entity.HasOne(x => x.Invitation)
                  .WithOne()
                  .HasForeignKey<Visitor>(x => x.InvitationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ================= VISIT LOG =================
        modelBuilder.Entity<VisitLog>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EntryTime)
                  .IsRequired();

            entity.Property(x => x.ExitTime);

            entity.HasOne(x => x.Visitor)
                  .WithMany()
                  .HasForeignKey(x => x.VisitorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
