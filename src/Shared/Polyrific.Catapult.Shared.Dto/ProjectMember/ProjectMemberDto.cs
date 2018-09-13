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
        /// Id of the project member role
        /// </summary>
        public int ProjectMemberRoleId { get; set; }
    }
}
