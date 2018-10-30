// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class CreateJobTaskDefinitionDto
    {
        /// <summary>
        /// Name of the job task definition
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Type of the job task definition
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Provider of the job task definition
        /// </summary>
        [Required]
        public string Provider { get; set; }

        /// <summary>
        /// Config of the job task definition
        /// </summary>
        public Dictionary<string, string> Configs { get; set; }

        /// <summary>
        /// Additional configurations which are required by specific providers
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// Sequence of the job task definition
        /// </summary>
        public int? Sequence { get; set; }
    }
}
