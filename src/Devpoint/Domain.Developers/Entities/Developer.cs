namespace Domain.Developers.Entities
{
    public class Developer
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = "";

        public string ImageFullPath { get; set; } = "";

        public string Description { get; set; } = "";

        public int SubscriberCount { get; set; } = 0;

        public List<Project> OwnedProjects { get; set; }
        public List<Project> Projects { get; set; }
        public List<Company> OwnedCompanies { get; set; }
        public List<Company> Companies { get; set; }
        public List<Tag>? Tags { get; set; }
        
        public Developer(string fullName)
        {
            FullName = fullName;
        }
        
        private Developer() {}
    }
}