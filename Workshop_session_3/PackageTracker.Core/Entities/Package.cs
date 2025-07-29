using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Entities
{
    public class Package
    {
        public Package() { }

        public Guid Id { get; set; }
        public required string TrackingNumber { get; set; }

        public Guid StatusId { get; set; }
        public required PackageStatus Status { get; set; }

        public decimal? Weight;
        public decimal? Length;
        public decimal? Width;
        public decimal? Height;

        public required string RecipientAddress;
        public required string SenderAddress;

        public Guid CarrierId { get; set; }
        public required Carrier Carrier { get; set; }

        public Guid UserId { get; set; }
        public required User User { get; set; }

        public required int ServiceTypeId { get; set; }
        public required CarrierService ServiceType { get; set; }

        public required DateTime CreatedAt = DateTime.Now;
        public required DateTime UpdatedAt;

        public static Package CreateEmpty()
        {
            return new Package
            {
                TrackingNumber = "",
                Status = new PackageStatus { Id = Guid.Empty, Name = "Unknown" },
                StatusId = Guid.Empty,
                Carrier = Carrier.CreateEmpty(),
                CarrierId = Guid.Empty,
                User = User.CreateEmpty(),
                UserId = Guid.Empty,
                ServiceType = CarrierService.createEmpty(),
                ServiceTypeId = 0,
                RecipientAddress = "",
                SenderAddress = "",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

    }
}
