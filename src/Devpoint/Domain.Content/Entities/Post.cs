using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;

namespace Domain.Content.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        public int RequiredSubscriptionLevelId { get; set; }
        public SubscriptionLevel RequiredSubscriptionLevel { get; set; }
        
        public Guid DeveloperId { get; set; }
        public Developer Developer { get; set; }
        
        public EntityType EntityType { get; set; }
        
        public Guid OwnerId { get; set; }
        public List<Comment> Comments { get; set; }

        public Post(string content, SubscriptionLevel requiredSubscriptionLevel, Developer developer, EntityType type, Guid ownerId)
        {
            Content = content;
            RequiredSubscriptionLevelId = requiredSubscriptionLevel.Id;
            RequiredSubscriptionLevel = requiredSubscriptionLevel;
            DeveloperId = developer.Id;
            Developer = developer;
            EntityType = type;
            OwnerId = ownerId;
        }
        
        private Post() {}
    }
}