// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.ProjectDataModel
{
    public class ProjectDataModelDto
    {
        /// <summary>
        /// Id of the project data model
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Name of the data model
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the data model
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Label of the data model
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Is the model managed in the UI?
        /// </summary>
        public bool? IsManaged { get; set; }

        /// <summary>
        /// The property name used as the key for select control
        /// </summary>
        public string SelectKey { get; set; }

        /// <summary>
        /// Properties of the data model
        /// </summary>
        public List<ProjectDataModelPropertyDto> Properties { get; set; }
    }
}
