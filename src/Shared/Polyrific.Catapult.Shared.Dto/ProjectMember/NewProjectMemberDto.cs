// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.ProjectMember
{
    public class NewProjectMemberDto
    {
        /// <summary>
        /// Id of the project
        /// </summary>
        [Required]
        public int ProjectId { get; set; }

        /// <summary>
        /// Id of the user
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Id of the project member role
        /// </summary>
        [Required]
        public int ProjectMemberRoleId { get; set; }

        /// <summary>
        /// Email of the new user
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// First Name  of the new user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the new user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The collection of external account id of the user
        /// </summary>
        public Dictionary<string, string> ExternalAccountIds { get; set; }

    }
}
