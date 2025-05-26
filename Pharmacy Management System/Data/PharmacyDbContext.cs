using Microsoft.EntityFrameworkCore;
using Pharmacy_Management_System.Models;

namespace Pharmacy_Management_System.Data
{
    public class PharmacyDbContext : DbContext
    {
        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base(options) { }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Medicine> Medicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique indexes
            modelBuilder.Entity<Agent>()
                .HasIndex(a => a.Phone).IsUnique();

            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Phone).IsUnique();

            // Relationship: Company (1) -> Medicine (many)
            modelBuilder.Entity<Medicine>()
                .HasOne(m => m.Company)
                .WithMany(c => c.Medicines)
                .HasForeignKey(m => m.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Check constraints
            modelBuilder.Entity<Medicine>()
                .HasCheckConstraint("CHK_ExpiryDate", "[ExpiryDate] > [ManufactureDate]");

            modelBuilder.Entity<Agent>()
                .HasCheckConstraint("CHK_Age", "[Age] > 0");

            modelBuilder.Entity<Company>()
                .HasCheckConstraint("CHK_Experience", "[Experience] >= 0");
        }
    }
}
