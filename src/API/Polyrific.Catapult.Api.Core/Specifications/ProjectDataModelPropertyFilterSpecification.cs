// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{

    public class ProjectDataModelPropertyFilterSpecification : BaseSpecification<ProjectDataModelProperty>
    {
        public int ProjectDataModelId { get; set; }
        public string Name { get; set; }
        public int ExcludedPropertyId { get; set; }

        public ProjectDataModelPropertyFilterSpecification(int projectDataModelId)
            : base(m => m.ProjectDataModelId == projectDataModelId, m => m.Sequence)
        {
            ProjectDataModelId = projectDataModelId;
        }

        public ProjectDataModelPropertyFilterSpecification(string name, int projectDataModelId)
            : base(m => m.Name == name && m.ProjectDataModelId == projectDataModelId)
        {
            Name = name;
            ProjectDataModelId = projectDataModelId;
        }

        public ProjectDataModelPropertyFilterSpecification(string name, int projectDataModelId, int excludedPropertyId)
            : base(m => m.Name == name && m.ProjectDataModelId == projectDataModelId && m.Id != excludedPropertyId)
        {
            Name = name;
            ProjectDataModelId = projectDataModelId;
            ExcludedPropertyId = excludedPropertyId;
        }
    }
}
