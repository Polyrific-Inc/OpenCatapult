// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.TaskProviders.GitHub
{
    public class GitAutomationConfig
    {
        /// <summary>
        /// Name of the product, the GitHub organization, or the GitHub user name
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// URL of the remote repository
        /// </summary>
        public string RemoteUrl { get; set; }

        /// <summary>
        /// Owner of the repository
        /// </summary>
        public string RepoOwner { get; set; }

        /// <summary>
        /// Credential type to be used to connect to remote repository
        /// (userPassword | authToken)
        /// </summary>
        public string RemoteCredentialType { get; set; }

        /// <summary>
        /// Username to connect to remote repository
        /// </summary>
        public string RemoteUsername { get; set; }

        /// <summary>
        /// Password to connect to remote repository
        /// </summary>
        public string RemotePassword { get; set; }

        /// <summary>
        /// Auth Token to connect to remote repository
        /// </summary>
        public string RepoAuthToken { get; set; }

        /// <summary>
        /// Is the repository private?
        /// </summary>
        public bool? IsPrivateRepository { get; set; }

        /// <summary>
        /// Location of the local repository
        /// </summary>
        public string LocalRepository { get; set; }

        /// <summary>
        /// List of the github usernames that are given access to the repository
        /// </summary>
        public List<string> Members { get; set; }
    }
}
