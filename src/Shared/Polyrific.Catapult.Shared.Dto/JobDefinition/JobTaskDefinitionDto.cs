// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class JobTaskDefinitionDto
    {
        /// <summary>
        /// Id of the job task definition
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the job task definition
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id of the job definition
        /// </summary>
        public int JobDefinitionId { get; set; }

        /// <summary>
        /// Type of the job task definition
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Provider of the job task definition
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Common configurations of the job task definition
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

        /// <summary>
        /// Is the job task definition valid?
        /// </summary>
        public bool? Valid { get; set; }

        /// <summary>
        /// Validation error message
        /// </summary>
        public string ValidationError { get; set; }
    }
}
