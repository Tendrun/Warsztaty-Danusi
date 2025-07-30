using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.DTOs
{
    public class CreatePackageDto
    {
        public required string TrackingNumber { get; set; }
        public Guid StatusId { get; set; }

        public decimal? Weight;
        public decimal? Length;
        public decimal? Width;
        public decimal? Height;

        public required string RecipientAddress;
        public required string SenderAddress;

        public Guid CarrierId { get; set; }
        public Guid UserId { get; set; }
        public required Guid ServiceTypeId { get; set; }
    }
}
