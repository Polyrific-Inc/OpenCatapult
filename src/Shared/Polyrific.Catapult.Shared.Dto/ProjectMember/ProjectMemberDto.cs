// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Catapult.Shared.Dto.ProjectMember
{
    public class ProjectMemberDto
    {
        /// <summary>
        /// Id of the project member
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Id of the user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Username (email) of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Id of the project member role
        /// </summary>
        public int ProjectMemberRoleId { get; set; }

        /// <summary>
        /// Name of the project member role
        /// </summary>
        public string ProjectMemberRoleName { get; set; }

        /// <summary>
        /// The collection of external account id of the user
        /// </summary>
        public Dictionary<string, string> ExternalAccountIds { get; set; }
    }
}
