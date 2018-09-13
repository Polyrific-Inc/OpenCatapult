// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.JobQueue
{
    public class NewJobDto
    {
        [Required]
        public int ProjectId { get; set; }

        public string JobType { get; set; }
        
        public string OriginUrl { get; set; }
        
        public int? JobDefinitionId { get; set; }
    }
}