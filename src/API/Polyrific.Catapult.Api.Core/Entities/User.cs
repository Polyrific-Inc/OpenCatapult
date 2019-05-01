// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class User : BaseEntity
    {
        /// <summary>
        /// Initialize a new instance of <see cref="User"/>.
        /// </summary>
        public User()
        {
            
        }

        /// <summary>
        /// Initialize a new instance of <see cref="User"/>.
        /// </summary>
        /// <param name="userName">Username of the user</param>
        public User(string userName)
        {
            UserName = userName;
        }

        /// <summary>
        /// Username of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First Name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Is user active?
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Is user email confirmed?
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Role of the user
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// The managed file id for the user's avatar
        /// </summary>
        public int? AvatarFileId { get; set; }

        /// <summary>
        /// The collection of external account id of the user
        /// </summary>
        public Dictionary<string, string> ExternalAccountIds { get; set; }
    }
}
