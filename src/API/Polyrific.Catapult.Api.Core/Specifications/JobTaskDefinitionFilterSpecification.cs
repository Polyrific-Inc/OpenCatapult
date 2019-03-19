// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class JobTaskDefinitionFilterSpecification : BaseSpecification<JobTaskDefinition>
    {
        public int JobDefinitionId { get; set; }
        public int ExcludedJobTaskDefinitionId { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Filter job task definition by job definition id
        /// </summary>
        /// <param name="jobDefinitionId">The job definition id</param>
        public JobTaskDefinitionFilterSpecification(int jobDefinitionId, int jobTaskDefinitionId = 0)
            : base(m => m.JobDefinitionId == jobDefinitionId && (jobTaskDefinitionId == 0 || m.Id == jobTaskDefinitionId), m => m.Sequence)
        {
            JobDefinitionId = jobDefinitionId;
        }

        /// <summary>
        /// Filter job task definition by job definition id and task name
        /// </summary>
        /// <param name="jobDefinitionId">Id of the job definition</param>
        /// <param name="name">Name of the job task definition</param>
        /// <param name="excludedJobTaskDefinitionId">Id of the excluded job task definition</param>
        public JobTaskDefinitionFilterSpecification(int jobDefinitionId, string name, int excludedJobTaskDefinitionId)
           : base(m => m.JobDefinitionId == jobDefinitionId && m.Name == name && (excludedJobTaskDefinitionId == 0 || m.Id != excludedJobTaskDefinitionId))
        {
            JobDefinitionId = jobDefinitionId;
            Name = name;
            ExcludedJobTaskDefinitionId = excludedJobTaskDefinitionId;
        }
    }
}
