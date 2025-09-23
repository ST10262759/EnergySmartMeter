using Microsoft.EntityFrameworkCore;
using SmartEnergyMeterAPI.Models;

namespace SmartEnergyMeterAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EnergyReading> EnergyReadings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnergyReading>()
                .Property(e => e.DateTime)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
