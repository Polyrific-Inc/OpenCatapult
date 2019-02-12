// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Shared.Dto.Project
{
    public class ProjectDto
    {
        /// <summary>
        /// Id of the project
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name of the project
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Client of the project
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Status of the project
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// DateTime when the project created
        /// </summary>
        public DateTime Created { get; set; }
    }
}
