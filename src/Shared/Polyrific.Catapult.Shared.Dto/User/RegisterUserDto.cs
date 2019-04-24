// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.User
{
    public class RegisterUserDto
    {
        /// <summary>
        /// First name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// The collection of external account id of the user
        /// </summary>
        public Dictionary<string, string> ExternalAccountIds { get; set; }
    }
}
