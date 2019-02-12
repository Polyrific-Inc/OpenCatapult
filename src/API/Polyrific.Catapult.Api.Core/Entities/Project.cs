// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class Project : BaseEntity
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Client of the project
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Display name of the project
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Is project archived?
        /// </summary>
        public bool IsArchived { get; set; }
        
        /// <summary>
        /// Project data models of the project
        /// </summary>
        public virtual ICollection<ProjectDataModel> Models { get; set; }

        /// <summary>
        /// Job definitions of the project
        /// </summary>
        public virtual ICollection<JobDefinition> Jobs { get; set; }

        /// <summary>
        /// Members of the project
        /// </summary>
        public virtual ICollection<ProjectMember> Members { get; set; }
    }
}
