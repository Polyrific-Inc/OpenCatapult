// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.JobQueue
{
    public class JobDto
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        
        public string Status { get; set; }

        public string CatapultEngineId { get; set; }

        public string JobType { get; set; }

        public string CatapultEngineMachineName { get; set; }

        public string CatapultEngineIPAddress { get; set; }

        public string CatapultEngineVersion { get; set; }

        public string OriginUrl { get; set; }

        public string Code { get; set; }

        public int? JobDefinitionId { get; set; }

        public string JobTasksStatus { get; set; }
    }
}