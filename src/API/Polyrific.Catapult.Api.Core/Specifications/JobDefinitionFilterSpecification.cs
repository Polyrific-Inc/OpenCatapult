// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class JobDefinitionFilterSpecification : BaseSpecification<JobDefinition>
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int ExcludedJobDefinitionId { get; set; }
        public int[] JobDefinitionIds { get; set; }

        /// <summary>
        /// Filter the job definition by project
        /// </summary>
        /// <param name="projectId">The project id</param>
        public JobDefinitionFilterSpecification(int projectId)
            : base(m => m.ProjectId == projectId)
        {
            ProjectId = projectId;
        }

        /// <summary>
        /// Filter the job definition in a project by the ids
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="jobDefinitionIds">The definition ids</param>
        public JobDefinitionFilterSpecification(int projectId, int[] jobDefinitionIds)
            : base(m => m.ProjectId == projectId && jobDefinitionIds.Contains(m.Id))
        {
            ProjectId = projectId;
            JobDefinitionIds = jobDefinitionIds;
        }

        /// <summary>
        /// Filter the job definition by name
        /// </summary>
        /// <param name="name">The job definition name</param>
        /// <param name="projectId">The project id</param>
        public JobDefinitionFilterSpecification(string name, int projectId)
            : base(m => m.Name == name && m.ProjectId == projectId)
        {
            Name = name;
            ProjectId = projectId;
        }

        /// <summary>
        /// Filter the job definition by name that is not have the provided id
        /// </summary>
        /// <param name="name">The job definition name</param>
        /// <param name="excludedJobDefinitionId">The job definition id that is excluded</param>
        public JobDefinitionFilterSpecification(string name, int projectId, int excludedJobDefinitionId)
            : base(m => m.Name == name && m.ProjectId == projectId && m.Id != excludedJobDefinitionId)
        {
            Name = name;
            ExcludedJobDefinitionId = excludedJobDefinitionId;
            ProjectId = projectId;
        }
    }
}
