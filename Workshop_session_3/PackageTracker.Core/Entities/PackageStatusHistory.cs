using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Entities
{
    public class PackageStatusHistory
    {
        public int Id { get; set; }
        public ICollection<Package> Packages { get; set; } = new List<Package>();
        public ICollection<PackageStatus> packageStatuses { get; set; } = new List<PackageStatus>();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public required string Notes { get; set; }
    }
}
