// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class ProjectMember : BaseEntity
    {
        /// <summary>
        /// Id of the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Related Project object
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Id of the user
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Id of the project member role
        /// </summary>
        public int ProjectMemberRoleId { get; set; }

        /// <summary>
        /// Related project member role object
        /// </summary>
        public virtual ProjectMemberRole ProjectMemberRole { get; set; }
    }
}