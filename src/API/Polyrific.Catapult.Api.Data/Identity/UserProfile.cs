// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.Identity
{
    public class UserProfile : BaseEntity
    {
        /// <summary>
        /// First Name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Indicates whether a user is active or not
        /// </summary>
        public bool IsActive { get; set; }

        public int? ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        
        public int? AvatarFileId { get; set; }
    }
}
