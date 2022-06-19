using System;
using Microsoft.AspNetCore.Identity;
using NodaTime;

namespace AuthorizationServer.Web.Domain
{
    public class Role : IdentityRole<Guid>, ISoftDeletable
    {
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        
        public DateOnly? DeletionDateTime { get; private set; }

        public void SoftDelete()
        {
            DeletionDateTime = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
