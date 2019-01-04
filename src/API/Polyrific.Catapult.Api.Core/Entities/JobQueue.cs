// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class JobQueue : BaseEntity
    {
        /// <summary>
        /// Id of the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Project object
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Status of the job
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Id of the catapult engine
        /// </summary>
        public string CatapultEngineId { get; set; }

        /// <summary>
        /// Type of the job
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Machine name of the catapult engine
        /// </summary>
        public string CatapultEngineMachineName { get; set; }

        /// <summary>
        /// Ip address of the catapult engine
        /// </summary>
        public string CatapultEngineIPAddress { get; set; }

        /// <summary>
        /// Version of the catapult engine
        /// </summary>
        public string CatapultEngineVersion { get; set; }

        /// <summary>
        /// Origin url where the job is created
        /// </summary>
        public string OriginUrl { get; set; }

        /// <summary>
        /// Code of the job
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Id of the job definition
        /// </summary>
        public int? JobDefinitionId { get; set; }

        public virtual JobDefinition JobDefinition { get; set; }

        /// <summary>
        /// JSON string of the job task status
        /// </summary>
        public string JobTasksStatus { get; set; }
        
        /// <summary>
        /// JSON string of the output values
        /// </summary>
        public string OutputValues { get; set; }

        /// <summary>
        /// Remarks related to the job
        /// </summary>
        public string Remarks { get; set; }
    }
}
