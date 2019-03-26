// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;
using System.Linq;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class JobQueueFilterSpecification : BaseSpecification<JobQueue>
    {
        public int ProjectId { get; set; }
        public int JobQueueId { get; set; }
        public string QueueCode { get; set; }
        public string Status { get; set; }
        public string[] StatusArray { get; set; }
        public bool UnassignedOnly { get; set; }
        public string EngineId { get; set; }

        /// <summary>
        /// Filter to get unassigned queued job only
        /// </summary>
        public JobQueueFilterSpecification()
            : base(m => m.Status == JobStatus.Queued && m.CatapultEngineId == null && 
                (m.Project.Status == ProjectStatusFilterType.Active || m.Project.Status == ProjectStatusFilterType.Deleting), m => m.Created)
        {
            Status = JobStatus.Queued;
            UnassignedOnly = true;
        }

        /// <summary>
        /// Filter by the job queue id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="jobQueueId"></param>
        public JobQueueFilterSpecification(int projectId, int jobQueueId)
            : base(m => (m.ProjectId == projectId || projectId == 0) && m.Id == jobQueueId)
        {
            ProjectId = projectId;
            JobQueueId = jobQueueId;
        }

        /// <summary>
        /// Filter by the project and a range of status
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="statusArray"></param>
        public JobQueueFilterSpecification(int projectId, string[] statusArray)
            : base(m => m.ProjectId == projectId && (statusArray == null || statusArray.Contains(m.Status)))
        {
            ProjectId = projectId;
            StatusArray = statusArray;
        }

        /// <summary>
        /// Filter by the queue code or status
        /// </summary>
        /// <param name="projectId">The Id of the project</param>
        /// <param name="queueCode">Code of the job queue</param>
        /// <param name="status">Status of the job queue</param>
        /// <param name="engineId">Id of the Catapult Engine which executes the queue</param>
        public JobQueueFilterSpecification(int projectId, string queueCode, string status, string engineId = null)
            : base(m => (projectId == 0 || m.ProjectId == projectId)
                && (queueCode == null || m.Code == queueCode) 
                && (status == null || m.Status == status) 
                && (engineId == null || m.CatapultEngineId == engineId), m => m.Created)
        {
            ProjectId = projectId;
            QueueCode = queueCode;
            Status = status;
            EngineId = engineId;
        }
    }
}
