// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class UpdateJobDefinitionDto
    {
        /// <summary>
        /// Id of the job definition
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Name of the data model
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
