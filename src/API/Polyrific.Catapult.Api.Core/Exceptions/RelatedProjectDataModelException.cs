using System;
using System.Collections.Generic;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class RelatedProjectDataModelException : Exception
    {
        public string DataModel { get; set; }

        public string[] RelatedDataModels { get; set; }

        public List<ProjectDataModel> RelatedDataModelEntities { get; set; }

        public RelatedProjectDataModelException(string dataModel, string[] relatedDataModels)
            : base($"The data model \"{dataModel}\" is referenced by the following models: {string.Join($"{DataDelimiter.Comma.ToString()} ", relatedDataModels)}")
        {
            DataModel = dataModel;
            RelatedDataModels = relatedDataModels;
        }

        public RelatedProjectDataModelException(string[] relatedDataModels)
            : base($"Cannot perform bulk delete because the following models depend on the models to be deleted: {string.Join($"{ DataDelimiter.Comma.ToString()} ", relatedDataModels)}")
        {
            RelatedDataModels = relatedDataModels;
        }
    }
}
