using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.DTOs.Status
{
    public class UpdateStatusDto
    {
        public required string Status { get; set; }
        public string? Notes { get; set; }

    }
}
