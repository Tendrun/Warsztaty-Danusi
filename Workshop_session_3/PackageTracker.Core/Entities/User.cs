namespace PackageTracker.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Package> Packages { get; set; } = new List<Package>();

        public static User createEmpty()
        {
            return new User
            {
                Username = "None",
                FirstName = "Unknown",
                LastName = "Unknown",
                Address = "",
                CreatedAt = DateTime.Now,
                PasswordHash = "None"
            };
        }
    }
}