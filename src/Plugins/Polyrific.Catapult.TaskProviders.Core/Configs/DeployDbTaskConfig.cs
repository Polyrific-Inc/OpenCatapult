// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.TaskProviders.Core.Configs
{
    public class DeployDbTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Location of the database migration
        /// </summary>
        public string MigrationLocation { get; set; }
    }
}
