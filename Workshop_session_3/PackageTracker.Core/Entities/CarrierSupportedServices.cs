using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Entities
{
    [Keyless]
    public class CarrierSupportedServices
    {
        public required Guid CarrierId { get; set; }
        public required Guid ServiceId { get; set; }

        public static CarrierSupportedServices CreateEmpty()
        {
            return new CarrierSupportedServices
            {
                CarrierId = Guid.Empty,
                ServiceId = Guid.Empty
            };
        }
    }
}