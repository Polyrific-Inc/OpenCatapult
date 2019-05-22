// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.TaskProviders.Core.Configs
{
    public class DeployTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Location of the artifact to deploy
        /// </summary>
        public string ArtifactLocation { get; set; }
    }
}
