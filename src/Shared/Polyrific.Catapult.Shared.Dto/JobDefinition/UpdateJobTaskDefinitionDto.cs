// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class UpdateJobTaskDefinitionDto
    {
        /// <summary>
        /// Id of the job task definition
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Name of the job task definition
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Type of the job task definition
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Provider of the job task definition
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Config of the job task definition
        /// </summary>
        public Dictionary<string, string> Configs { get; set; }

        /// <summary>
        /// Sequence of the job task definition
        /// </summary>
        public int? Sequence { get; set; }
    }
}
