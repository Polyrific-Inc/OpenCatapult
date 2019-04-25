// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        /// The collection of external account id of the user
        /// </summary>
        public Dictionary<string, string> ExternalAccountIds { get; set; }
    }
}
