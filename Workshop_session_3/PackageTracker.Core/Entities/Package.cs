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
        public PackageStatus Status { get; set; } = default!;

        public decimal? Weight;
        public decimal? Length;
        public decimal? Width;
        public decimal? Height;

        public required string RecipientAddress;
        public required string SenderAddress;

        public Guid CarrierId { get; set; }
        public Carrier Carrier { get; set; } = default!;

        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public required int ServiceTypeId { get; set; }
        public CarrierService ServiceType { get; set; } = default!;

        public DateTime CreatedAt = DateTime.Now;
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
