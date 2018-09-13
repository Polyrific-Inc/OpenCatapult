// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;
using System.Linq;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class JobQueueFilterSpecification : BaseSpecification<JobQueue>
    {
        public int ProjectId { get; set; }
        public string Status { get; set; }
        public string[] StatusArray { get; set; }
        public bool UnassignedOnly { get; set; }

        /// <summary>
        /// Filter to get unassigned queued job only
        /// </summary>
        public JobQueueFilterSpecification()
            : base(m => m.Status == JobStatus.Queued && m.CatapultEngineId == null, m => m.Created)
        {
            Status = JobStatus.Queued;
            UnassignedOnly = true;
        }

        /// <summary>
        /// Filter by the project
        /// </summary>
        /// <param name="projectId"></param>
        public JobQueueFilterSpecification(int projectId)
            : base(m => m.ProjectId == projectId)
        {
            ProjectId = projectId;
        }

        /// <summary>
        /// Filter by the status
        /// </summary>
        /// <param name="status"></param>
        public JobQueueFilterSpecification(string status)
            : base(m => m.Status == status, m => m.Created)
        {
            Status = status;
        }

        /// <summary>
        /// Filter by the project and certain status
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="status"></param>
        public JobQueueFilterSpecification(int projectId, string status)
            : base(m => m.ProjectId == projectId && m.Status == status)
        {
            ProjectId = projectId;
            Status = status;
        }

        /// <summary>
        /// Filter by the project and a range of status
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="statusArray"></param>
        public JobQueueFilterSpecification(int projectId, string[] statusArray)
            : base(m => m.ProjectId == projectId && statusArray.Contains(m.Status))
        {
            ProjectId = projectId;
            StatusArray = statusArray;
        }
    }
}
