// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.TaskProviders.Core.Configs
{
    public class PullTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Remote repository
        /// </summary>
        public string Repository { get; set; }
        
        /// <summary>
        /// Whether the repository is private
        /// </summary>
        public bool IsPrivateRepository { get; set; }

        /// <summary>
        /// The repository local folder path
        /// </summary>
        public string RepositoryLocation { get; set; }

        /// <summary>
        /// Initial branch that will be worked on
        /// </summary>
        public string BaseBranch { get; set; }
    }
}
