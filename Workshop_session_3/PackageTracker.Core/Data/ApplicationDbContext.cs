using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageTracker.Core.Entities;

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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        
        }
    }
}
