// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.Project
{
    public class CloneProjectOptionDto
    {
        /// <summary>
        /// Name of the new project
        /// </summary>
        [Required]
        public string NewProjectName { get; set; }

        /// <summary>
        /// Display name of the project
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Client of the project
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Copy project members into the new project
        /// </summary>
        public bool IncludeMembers { get; set; }

        /// <summary>
        /// Copy job definitions into the new project
        /// </summary>
        public bool IncludeJobDefinitions { get; set; }
    }
}
