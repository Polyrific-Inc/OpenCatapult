// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class JobDefinitionDto
    {
        /// <summary>
        /// Id of the job definition
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Name of the data model
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is the job definition for resource deletion?
        /// </summary>
        public bool IsDeletion { get; set; }

        /// <summary>
        /// Is the job definition is a default job in the project?
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Name of the data model
        /// </summary>
        public List<JobTaskDefinitionDto> Tasks { get; set; }
    }
}
