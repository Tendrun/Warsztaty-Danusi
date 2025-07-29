using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Entities
{
    public class PackageStatus
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Package> Packages { get; set; } = new List<Package>();
        public ICollection<PackageStatusHistory> PackageStatusHistories { get; set; } = new List<PackageStatusHistory>();

        public static PackageStatus CreateEmpty()
        {
            return new PackageStatus
            {
                Id = Guid.Empty,
                Name = "",
                Packages = new List<Package>(),
                PackageStatusHistories = new List<PackageStatusHistory>(),
            };
        }

    }
}
