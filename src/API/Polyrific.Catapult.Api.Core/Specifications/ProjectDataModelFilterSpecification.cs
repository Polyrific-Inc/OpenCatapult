// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{

    public class ProjectDataModelFilterSpecification : BaseSpecification<ProjectDataModel>
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int ExcludedModelId { get; set; }

        /// <summary>
        /// Filter the data model by project
        /// </summary>
        /// <param name="projectId">The project id</param>
        public ProjectDataModelFilterSpecification(int projectId)
            : base(m => m.ProjectId == projectId)
        {
            ProjectId = projectId;
        }

        /// <summary>
        /// Filter the data model by name
        /// </summary>
        /// <param name="name">The model name</param>
        public ProjectDataModelFilterSpecification(string name, int projectId)
            : base(m => m.Name == name && m.ProjectId == projectId)
        {
            Name = name;
            ProjectId = projectId;
        }

        /// <summary>
        /// Filter the data model by name that is not have the provided id
        /// </summary>
        /// <param name="name">The model name</param>
        /// <param name="excludedModelId">The model id that is excluded</param>
        public ProjectDataModelFilterSpecification(string name, int projectId, int excludedModelId)
            : base(m => m.Name == name && m.ProjectId == projectId && m.Id != excludedModelId)
        {
            Name = name;
            ExcludedModelId = excludedModelId;
            ProjectId = projectId;
        }
    }
}
