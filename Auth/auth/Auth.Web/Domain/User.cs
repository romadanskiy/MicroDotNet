using System;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationServer.Web.Domain
{
    public sealed class User : IdentityUser<Guid>, ISoftDeletable
    {
        [Obsolete("EF Only", true)]
        // ReSharper disable once UnusedMember.Global
        private User() {}
        
        public User (string userName, string firstName, string lastName, string phoneNumber)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            UserName = userName;
            // ReSharper disable once VirtualMemberCallInConstructor
            NormalizedUserName = userName.ToUpperInvariant();
            FirstName = firstName;
            
            LastName = lastName;
            
            PhoneNumber = phoneNumber;
        }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        
        public DateOnly CreatedAt { get; set; } = Dates.Now();

        public DateOnly? DeletionDateTime { get; private set; }

        public void SoftDelete()
        {
            DeletionDateTime = Dates.Now();
        }
    }
}