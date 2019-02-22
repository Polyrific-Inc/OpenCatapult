using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class RelatedProjectDataModelException : Exception
    {
        public string DataModel { get; set; }

        public string[] RelatedDataModels { get; set; }

        public RelatedProjectDataModelException(string dataModel, string[] relatedDataModels)
            : base($"The data model \"{dataModel}\" is referenced by the following models: {string.Join($"DataDelimiter.Comma.ToString() ", relatedDataModels)}")
        {
            DataModel = dataModel;
            RelatedDataModels = relatedDataModels;
        }
    }
}
