// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.JobQueue
{
    public class UpdateJobDto
    {
        /// <summary>
        /// Id of the job queue
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Status of the job queue
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Engine Id that runs the job
        /// </summary>
        public string CatapultEngineId { get; set; }

        /// <summary>
        /// Type of the job queue
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Machine name of the catapult engine
        /// </summary>
        public string CatapultEngineMachineName { get; set; }

        /// <summary>
        /// IP address of the catapult engine
        /// </summary>
        public string CatapultEngineIPAddress { get; set; }

        /// <summary>
        /// Version of the catapult engine
        /// </summary>
        public string CatapultEngineVersion { get; set; }

        /// <summary>
        /// Status of the job tasks
        /// </summary>
        public List<JobTaskStatusDto> JobTasksStatus { get; set; }
        
        /// <summary>
        /// Output values of the job
        /// </summary>
        public Dictionary<string, string> OutputValues { get; set; }
    }
}
