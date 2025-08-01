using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageTracker.Core.Entities;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;


namespace PackageTracker.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Carrier> carriers { get; set; }
        public DbSet<CarrierService> carrierServices { get; set; }
        public DbSet<Package> packages { get; set; }
        public DbSet<PackageStatus> PackageStatuses { get; set; }
        public DbSet<PackageStatusHistory> PackageStatusHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CarrierSupportedServices> CarrierSupportedServices { get; set; }

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PackageTrackerDb;Integrated Security=True;");

            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // konfiguracja precyzji Price dla każdego Entity
            modelBuilder.Entity<CarrierService>()
                .Property(c => c.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Package>()
                .Property(p => p.Weight)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Package>()
                .Property(p => p.Length)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Package>()
                .Property(p => p.Width)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Package>()
                .Property(p => p.Height)
                .HasPrecision(10, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
