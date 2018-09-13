// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class ProjectFilterSpecification : BaseSpecification<Project>
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public int ExcludedProjectId { get; set; }

        public ProjectFilterSpecification(string projectName)
            : base(m => m.Name == projectName)
        {
            ProjectName = projectName;
        }

        public ProjectFilterSpecification(int projectId)
            : base(m => m.Id == projectId)
        {
            ProjectId = projectId;
        }

        public ProjectFilterSpecification(string projectName, int excludedProjectId)
            : base(m => m.Name == projectName && m.Id != excludedProjectId)
        {
            ProjectName = projectName;
            ExcludedProjectId = excludedProjectId;
        }
    }
}