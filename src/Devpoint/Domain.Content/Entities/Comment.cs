using Domain.Developers.Entities;

namespace Domain.Content.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Developer Developer { get; set; }
        public Post Post { get; set; }

        public Comment(string text, Developer developer, Post post)
        {
            Developer = developer;
            Text = text;
            Post = post;
        }
        
        private Comment() {}
    }
}