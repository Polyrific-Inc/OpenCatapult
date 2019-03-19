// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class JobDefinition : BaseEntity
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
        /// Name of the job definition
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is the job definition for resource deletion?
        /// </summary>
        public bool IsDeletion { get; set; }

        /// <summary>
        /// Tasks of the job definition
        /// </summary>
        public virtual ICollection<JobTaskDefinition> Tasks { get; set; }
    }
}
