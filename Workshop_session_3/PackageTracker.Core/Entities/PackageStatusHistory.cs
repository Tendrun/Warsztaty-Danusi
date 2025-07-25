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

        public required int PackageId { get; set; }
        public required Package Package { get; set; }

        public required int StatusId { get; set; }
        public required PackageStatus Status { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
    }

}
