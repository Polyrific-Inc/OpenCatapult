// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.Constants
{
    public class JobTaskDefinitionType
    {
        public const string Build = "Build";
        public const string Deploy = "Deploy";
        public const string DeployDb = "DeployDb";
        public const string Generate = "Generate";
        public const string Merge = "Merge";
        public const string Pull = "Pull";
        public const string Push = "Push";
        public const string PublishArtifact = "PublishArtifact";
        public const string Test = "Test";
        public const string DeleteRepository = "DeleteRepository";
        public const string DeleteHosting = "DeleteHosting";
        public const string CustomTask = "CustomTask";
    }
}
