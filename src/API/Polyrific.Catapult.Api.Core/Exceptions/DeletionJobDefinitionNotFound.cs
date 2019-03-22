using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DeletionJobDefinitionNotFound : Exception
    {
        public int ProjectId { get; set; }

        public DeletionJobDefinitionNotFound(int projectId)
            : base($"The project \"{projectId}\" does not have a deletion job definition.")
        {
            ProjectId = projectId;
        }
    }
}
