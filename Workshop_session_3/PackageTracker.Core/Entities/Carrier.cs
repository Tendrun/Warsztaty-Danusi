namespace PackageTracker.Core.Entities
{
    public class Carrier
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required bool isActive { get; set; }
        public ICollection<Package> Packages { get; set; } = new List<Package>();
    }
}