using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.DTOs.Status
{
    public class StatusHistoryDto
    {
        public Guid Id { get; set; }

        public required Guid PackageId { get; set; }
        public required Package Package { get; set; }

        public required Guid StatusId { get; set; }
        public required PackageStatus Status { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
    }
}
