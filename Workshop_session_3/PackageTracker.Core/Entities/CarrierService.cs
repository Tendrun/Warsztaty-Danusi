using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Entities
{
    public class CarrierService
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }

        public static CarrierService createEmpty()
        {
            return new CarrierService
            {
                Name = "None",
                Description = "None",
                Price = 0,
            };
        }
    }
}
