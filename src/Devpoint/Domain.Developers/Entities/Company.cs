namespace Domain.Developers.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";

        public string ImagePath { get; set; } = "";

        public string Description { get; set; } = "";

        public int SubscriberCount { get; set; } = 0;
        
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public Guid OwnerId { get; set; }
        public Developer Owner { get; set; }
        public List<Developer> Developers { get; set; }
        public List<Project> Projects { get; set; }
        public List<Tag>? Tags { get; set; }

        public Company(string name, Developer owner)
        {
            Name = name;
            OwnerId = owner.Id;
            Owner = owner;
            Developers = new List<Developer> {owner};
        }
        
        private Company() {}
    }
}