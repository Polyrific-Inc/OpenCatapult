// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.JobDefinition;

namespace Polyrific.Catapult.Engine.Core
{
    public interface ITaskRunner
    {
        /// <summary>
        /// Run job tasks
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobTask">Job tasks to run</param>
        /// <param name="queueCode">Code of the job queue</param>
        /// <param name="pluginsLocation">Location of the plugins folder</param>
        /// <returns></returns>
        Task<Dictionary<int, TaskRunnerResult>> Run(int projectId, string queueCode, List<JobTaskDefinitionDto> jobTask, string pluginsLocation);
    }
}