using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Zima.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Packet> Packets { get; set; }
        public DbSet<Project> Projects { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration config)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Packet>()
                .HasIndex(p => p.Id)
                .IsUnique(true);
            builder.Entity<Project>()
                .HasIndex(p => p.Id)
                .IsUnique(true);
            builder.Entity<Project>()
                .HasMany(p => p.Versions)
                .WithOne(p => p.Project);

            base.OnModelCreating(builder);
        }
    }
}
