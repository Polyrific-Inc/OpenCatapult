// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class PushTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Location of the source code to push
        /// </summary>
        public string SourceLocation { get; set; }

        /// <summary>
        /// Remote repository
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// Remote branch
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Whether to create pull request as well after pushing the code
        /// </summary>
        public bool CreatePullRequest { get; set; }

        /// <summary>
        /// Branch which will be the target of the pull request
        /// </summary>
        public string PullRequestTargetBranch { get; set; }

        /// <summary>
        /// Message set when committing changes
        /// </summary>
        public string CommitMessage { get; set; }
               
        /// <summary>
        /// Author of the commit
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Email of the commiters
        /// </summary>
        public string Email { get; set; }
    }
}
