// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class CreateJobDefinitionDto
    {
        /// <summary>
        /// Name of the data model
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
