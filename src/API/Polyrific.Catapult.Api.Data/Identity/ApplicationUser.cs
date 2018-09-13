// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity;

namespace Polyrific.Catapult.Api.Data.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            
        }

        public ApplicationUser(string userName) : base(userName)
        {
            
        }

        public ApplicationUser(int userId, string userEmail) : base(userEmail)
        {
            Id = userId;
            NormalizedUserName = userEmail.ToUpper();
            Email = userEmail;
            NormalizedEmail = userEmail.ToUpper();
        }

        public virtual UserProfile UserProfile { get; set; }

        public bool? IsCatapultEngine { get; set; }
        public virtual CatapultEngineProfile CatapultEngineProfile { get; set; }
    }
}