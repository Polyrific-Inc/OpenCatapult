// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using Polyrific.Catapult.Shared.Dto.ManagedFile;

namespace Polyrific.Catapult.Shared.Dto.User
{
    public class UpdateUserDto
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// First Name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The managed file for the user's avatar
        /// </summary>
        public ManagedFileDto AvatarFile { get; set; }
    }
}
