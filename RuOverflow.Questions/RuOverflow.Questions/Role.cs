using Microsoft.AspNetCore.Identity;

namespace RuOverflow.Questions
{
    public class Role : IdentityRole<Guid>
    {
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        
        public DateOnly? DeletionDateTime { get; private set; }
    }
}
