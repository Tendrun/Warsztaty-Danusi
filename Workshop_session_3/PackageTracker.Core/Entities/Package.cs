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
        public int Id { get; set; }
        public required string TrackingNumber { get; set; }
        public ICollection<PackageStatus> packageStatus { get; set; } = new List<PackageStatus>();
        public decimal? Weight;
        public decimal? Length;
        public decimal? Width;
        public decimal? Height;
        public required string RecipientAddress;
        public required string SenderAddress;
        public ICollection<Carrier> Carriers = new List<Carrier>();
        public ICollection<User> Users = new List<User>();
        public required DateTime CreatedAt = DateTime.Now;
        public required DateTime UpdatedAt;
    }
}
