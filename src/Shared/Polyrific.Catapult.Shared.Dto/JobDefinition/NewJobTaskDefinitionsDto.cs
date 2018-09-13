// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class NewJobTaskDefinitionsDto
    {
        /// <summary>
        /// List of new job task definitions
        /// </summary>
        public List<JobTaskDefinitionDto> JobTaskDefinitions { get; set; }
    }
}
