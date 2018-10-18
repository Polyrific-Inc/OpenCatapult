// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class CloneTaskConfig : BaseJobTaskConfig
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
        /// Location where the source code needs to cloned to
        /// </summary>
        public string CloneLocation { get; set; }

        /// <summary>
        /// Initial branch that will be worked on
        /// </summary>
        public string BaseBranch { get; set; }
    }
}
