﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.JobQueue
{
    public class JobDto
    {
        /// <summary>
        /// Id of the job
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Status of the project
        /// </summary>
        public string ProjectStatus { get; set; }

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

        /// <summary>
        /// Name of the job definition
        /// </summary>
        public string JobDefinitionName { get; set; }

        /// <summary>
        /// Is the job definition for resource deletion?
        /// </summary>
        public bool IsDeletion { get; set; }

        /// <summary>
        /// Status of the job tasks
        /// </summary>
        public List<JobTaskStatusDto> JobTasksStatus { get; set; }

        /// <summary>
        /// Output values of the job
        /// </summary>
        public Dictionary<string, string> OutputValues { get; set; }

        /// <summary>
        /// Remarks related to the job
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// The date time of the job created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The date time of the job updated
        /// </summary>
        public DateTime? Updated { get; set; }
    }
}
