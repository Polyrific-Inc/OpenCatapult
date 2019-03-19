using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class JobTaskDefinitionTypeException : Exception
    {
        public bool IsJobDeletion { get; set; }
        public string TaskDefinitionType { get; set; }

        public JobTaskDefinitionTypeException(bool isJobDeletion, string taskDefinitionType)
            : base($"Task with type \"{taskDefinitionType}\" cannot be added to {(isJobDeletion ? "deletion" : "normal")} job definition")
        {
            IsJobDeletion = isJobDeletion;
            TaskDefinitionType = taskDefinitionType;
        }
    }
}
