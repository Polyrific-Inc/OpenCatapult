// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.TaskProviders.GitHub
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
        /// Commit changes into a branch
        /// </summary>
        /// <param name="localRepository">Location of the local repository</param>
        /// <param name="baseBranch">Base branch that will be used as Head for the new branch</param>
        /// <param name="branch">Branch of the commit</param>
        /// <param name="commitMessage">Message of the commit</param>
        /// <param name="author">Author of the commit</param>
        /// <param name="email">Email of the committer</param>
        /// <returns></returns>
        Task<bool> Commit(string localRepository, string baseBranch, string branch, string commitMessage, string author, string email);

        /// <summary>
        /// Checkout a branch
        /// </summary>
        /// <param name="localRepository">Location of the local repository</param>
        /// <param name="branch">Branch to be checked out</param>
        /// <returns></returns>
        Task<bool> CheckoutBranch(string localRepository, string branch);

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

        /// <summary>
        /// Create a repository if it's not yet exists
        /// </summary>
        /// <param name="projectName">Name of the remote repository</param>
        /// <param name="repositoryOwner">Owner of the repository. It could be username or organization.</param>
        /// <param name="isPrivateRepository">Is remote repository private?</param>
        /// <returns></returns>
        Task<string> CreateRepositoryIfNotExists(string projectName, string repositoryOwner, bool isPrivateRepository);

        /// <summary>
        /// Delete a repository if it's exists
        /// </summary>
        /// <param name="projectName">Name of the remote repository</param>
        /// <param name="repositoryOwner">Owner of the repository. It could be username or organization.</param>
        /// <returns></returns>
        Task<string> DeleteRepository(string projectName, string repositoryOwner);
    }
}
