// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.ProjectMember
{
    public class UpdateProjectMemberDto
    {
        /// <summary>
        /// Id of the project member
        /// </summary>
        public int Id { get; set; }

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