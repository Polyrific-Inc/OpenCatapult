// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{

    public class ProjectDataModelFilterSpecification : BaseSpecification<ProjectDataModel>
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int ExcludedModelId { get; set; }
        public int RelatedModelId { get; set; }
        public int?[] RelatedModelsId { get; set; }
        public int[] ModelIds { get; set; }

        /// <summary>
        /// Filter the data model by project
        /// </summary>
        /// <param name="projectId">The project id</param>
        public ProjectDataModelFilterSpecification(int projectId)
            : base(m => m.ProjectId == projectId, m => m.Name)
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

        /// <summary>
        /// Filter the data model that is related to the provided model id
        /// </summary>
        /// <param name="projectId">The project where the models will be checked</param>
        /// <param name="relatedModelId">The model to be searched for the other related models</param>
        public ProjectDataModelFilterSpecification(int projectId, int relatedModelId)
            : base(m => m.ProjectId == projectId && m.Id != relatedModelId && m.Properties.Select(p => p.RelatedProjectDataModelId).Contains(relatedModelId))
        {
            ProjectId = projectId;
            RelatedModelId = relatedModelId;
        }

        /// <summary>
        /// Filter the data model that is related to the provided models id
        /// </summary>
        /// <param name="projectId">The project where the models will be checked</param>
        /// <param name="relatedModelsId">The model to be searched for the other related models</param>
        public ProjectDataModelFilterSpecification(int projectId, int?[] relatedModelsId)
            : base(m => m.ProjectId == projectId && !relatedModelsId.Contains(m.Id) && 
                m.Properties.Any(p => relatedModelsId.Contains(p.RelatedProjectDataModelId)))
        {
            ProjectId = projectId;
            RelatedModelsId = relatedModelsId;
        }

        /// <summary>
        /// Filter the data models that have the id listed in modelIds
        /// </summary>
        /// <param name="modelIds">The model id list to check</param>
        public ProjectDataModelFilterSpecification(int[] modelIds)
            : base(m => modelIds.Contains(m.Id))
        {
            ModelIds = modelIds;
        }
    }
}
