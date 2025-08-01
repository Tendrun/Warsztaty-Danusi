namespace PackageTracker.Core.Entities
{
    public class Carrier
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Package> Packages { get; set; } = new List<Package>();

        public static Carrier CreateEmpty()
        {
            return new Carrier {
                Name = "None",
                Email = "None",
                PhoneNumber = "None",
                IsActive = false,
            };
        }
    }
}