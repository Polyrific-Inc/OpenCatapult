// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

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
        /// Client of the project
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Status of the project
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Config of the project
        /// </summary>
        public Dictionary<string, string> Config { get; set; }
    }
}
