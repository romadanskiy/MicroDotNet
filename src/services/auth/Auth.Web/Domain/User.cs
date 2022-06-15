using System;
using Microsoft.AspNetCore.Identity;
using NodaTime;

namespace AuthorizationServer.Web.Domain
{
    public class User : IdentityUser<Guid>, ISoftDeletable
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

        public DateOnly CreatedAt { get; set; } = Dates.Now();

        public DateOnly? DeletionDateTime { get; private set; }

        public void SoftDelete()
        {
            DeletionDateTime = Dates.Now();
        }
    }
}