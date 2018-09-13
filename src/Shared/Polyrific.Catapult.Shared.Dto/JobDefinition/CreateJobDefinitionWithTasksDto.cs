// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class CreateJobDefinitionWithTasksDto : CreateJobDefinitionDto
    {
        /// <summary>
        /// Job task definitions of the new job definition
        /// </summary>
        public List<CreateJobTaskDefinitionDto> Tasks { get; set; }
    }
}
