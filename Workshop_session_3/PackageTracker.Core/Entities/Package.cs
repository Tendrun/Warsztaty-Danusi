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

        public int Id { get; set; }
        public required string TrackingNumber { get; set; }

        public int StatusId { get; set; }
        public required PackageStatus Status { get; set; }

        public decimal? Weight;
        public decimal? Length;
        public decimal? Width;
        public decimal? Height;

        public required string RecipientAddress;
        public required string SenderAddress;

        public int CarrierId { get; set; }
        public required Carrier Carrier { get; set; }

        public int UserId { get; set; }
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
                Status = new PackageStatus { Id = 0, Name = "Unknown" },
                StatusId = 0,
                Carrier = Carrier.createEmpty(),
                CarrierId = 0,
                User = User.createEmpty(),
                UserId = 0,
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
