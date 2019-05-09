// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class CreateJobDefinitionDto
    {
        /// <summary>
        /// Name of the job definition
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Is the job definition for resource deletion?
        /// </summary>
        public bool IsDeletion { get; set; }

        /// <summary>
        /// Is the job definition is a default job in the project?
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
