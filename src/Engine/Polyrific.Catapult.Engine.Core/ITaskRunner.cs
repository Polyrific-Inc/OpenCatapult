// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Dto.JobQueue;

namespace Polyrific.Catapult.Engine.Core
{
    public interface ITaskRunner
    {
        /// <summary>
        /// Run job tasks
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="job">Job queue</param>
        /// <param name="jobTask">Job tasks to run</param>
        /// <param name="pluginsLocation">Location of the plugins folder</param>
        /// <param name="workingLocation">Location of the working directory</param>
        /// <returns></returns>
        Task<Dictionary<int, TaskRunnerResult>> Run(int projectId, 
            JobDto job, 
            List<JobTaskDefinitionDto> jobTask,
            string pluginsLocation, 
            string workingLocation);
    }
}
