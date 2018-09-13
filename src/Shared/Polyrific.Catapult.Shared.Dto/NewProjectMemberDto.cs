// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto
{
    public class NewProjectMemberDto
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Id of the member role
        /// </summary>
        public int ProjectMemberRoleId { get; set; }
    }
}