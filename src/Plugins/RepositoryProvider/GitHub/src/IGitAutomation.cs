// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace GitHub
{
    public interface IGitAutomation
    {
        /// <summary>
        /// Clone remote repository to local
        /// </summary>
        /// <returns></returns>
        Task<string> Clone();

        /// <summary>
        /// Checkout a branch
        /// </summary>
        /// <param name="branch">Branch to be checked out</param>
        /// <returns></returns>
        Task<bool> CheckoutBranch(string branch);

        /// <summary>
        /// Commit changes into a branch
        /// </summary>
        /// <param name="baseBranch">Base branch that will be used as Head for the new branch</param>
        /// <param name="branch">branch of the commit</param>
        /// <param name="commitMessage">Message of the commit</param>
        /// <param name="author">Author of the commit</param>
        /// <param name="email">Email of the comitter</param>
        /// <returns></returns>
        Task<string> Commit(string baseBranch, string branch, string commitMessage, string author, string email);

        /// <summary>
        /// Push local changes into remote repository
        /// </summary>
        /// <param name="branch">Remote branch</param>
        /// <returns></returns>
        Task<string> Push(string branch);

        /// <summary>
        /// Submit pull request
        /// </summary>
        /// <param name="branch">Source branch</param>
        /// <param name="targetBranch">Target branch</param>
        /// <returns>Pull Request number</returns>
        Task<int> SubmitPullRequest(string branch, string targetBranch);

        /// <summary>
        /// Merge pull request in remote repository
        /// </summary>
        /// <param name="prNumber">Pull Request number</param>
        /// <returns></returns>
        Task<bool> MergePullRequest(string prNumber);
    }
}
