// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
    }
}