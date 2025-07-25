namespace PackageTracker.Core.Entities
{
    public class Carrier
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required bool IsActive { get; set; }
        public ICollection<Package> Packages { get; set; } = new List<Package>();

        public static Carrier createEmpty()
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