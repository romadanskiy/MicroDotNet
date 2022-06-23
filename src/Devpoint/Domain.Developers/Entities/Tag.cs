namespace Domain.Developers.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<Developer> Developers { get; set; }
        public List<Project> Projects { get; set; }
        public List<Company> Companies { get; set; }

        public Tag(string text)
        {
            Text = text;
        }
    }
}