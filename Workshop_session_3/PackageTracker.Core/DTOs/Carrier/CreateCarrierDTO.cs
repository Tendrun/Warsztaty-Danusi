using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.DTOs.Carrier
{
    public class CreateCarrierDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public required ICollection<SupportedServicesDto> supportedServices { get; set; } =
            new List<SupportedServicesDto>();

    }

    public class SupportedServicesDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? description { get; set; }
        public required decimal price { get; set; }
    }
}
