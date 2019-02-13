// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.Project
{
    public class NewProjectDto
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Display name of the project
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Members of the project
        /// </summary>
        [Required]
        public List<NewProjectMemberDto> Members { get; set; }
        
        /// <summary>
        /// Client of the project
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Data models of the project
        /// </summary>
        public List<CreateProjectDataModelWithPropertiesDto> Models { get; set; }

        /// <summary>
        /// Job definitions of the project
        /// </summary>
        public List<CreateJobDefinitionWithTasksDto> Jobs { get; set; }
    }
}
