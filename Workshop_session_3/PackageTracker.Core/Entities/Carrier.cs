namespace PackageTracker.Core.Entities
{
    public class Carrier
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public ICollection<Package> Packages { get; set; } = new List<Package>();
    }
}