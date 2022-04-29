using Microsoft.AspNetCore.Identity;

namespace RuOverflow.Questions
{
    public class User : IdentityUser<Guid>
    {
        [Obsolete("EF Only", true)]
        // ReSharper disable once UnusedMember.Global
        protected User() {}
        
        public User (string userName, string firstName, string lastName)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            UserName = userName;
            // ReSharper disable once VirtualMemberCallInConstructor
            NormalizedUserName = userName.ToUpperInvariant();
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; protected set; } = null!;

        public string LastName { get; protected set; } = null!;
        
    }
}