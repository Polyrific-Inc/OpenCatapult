// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace GitHub
{
    public interface IGitHubUtils
    {
        /// <summary>
        /// Clone source code to local repository
        /// </summary>
        /// <param name="remoteUrl">URL of the remote repository</param>
        /// <param name="localRepository">Location of the local repository</param>
        /// <param name="isPrivateRepository">Is remote repository private?</param>
        /// <returns>Location of the cloned source code (local repository)</returns>
        Task<string> Clone(string remoteUrl, string localRepository, bool isPrivateRepository);

        /// <summary>
        /// Push source code changes to remote repository
        /// </summary>
        /// <param name="remoteUrl">URL of the remote repository</param>
        /// <param name="localRepository">Location of the local repository</param>
        /// <param name="branch">Name of the branch</param>
        /// <returns>Success status</returns>
        Task<bool> Push(string remoteUrl, string localRepository, string branch);

        /// <summary>
        /// Create pull request in remote repository
        /// </summary>
        /// <param name="projectName">Name of the remote repository</param>
        /// <param name="repositoryOwner">Owner of the repository. It could be username or organization.</param>
        /// <param name="branch">Name of the pull request branch</param>
        /// <param name="targetBranch">Name of the pull request target branch</param>
        /// <param name="title">Title of the pull request</param>
        /// <returns>Pull request number</returns>
        Task<string> CreatePullRequest(string projectName, string repositoryOwner, string branch, string targetBranch, string title);

        /// <summary>
        /// Merge pull request in remote repository
        /// </summary>
        /// <param name="projectName">Name of the remote repository</param>
        /// <param name="repositoryOwner">Owner of the repository. It could be username or organization.</param>
        /// <param name="prNumber">Pull request number</param>
        /// <returns>Success status</returns>
        Task<bool> MergePullRequest(string projectName, string repositoryOwner, string prNumber);
    }
}
