using Microsoft.EntityFrameworkCore;
using Metrix.Domain.Entities;

namespace Metrix.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<HRUser> HRUsers => Set<HRUser>();
    public DbSet<SecurityUser> SecurityUsers => Set<SecurityUser>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<VisitLog> VisitLogs => Set<VisitLog>();
}
