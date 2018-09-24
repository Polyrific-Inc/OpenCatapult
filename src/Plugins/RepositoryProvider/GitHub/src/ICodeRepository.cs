// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace GitHub
{
    public interface ICodeRepository
    {
        /// <summary>
        /// Clone remote repository to local
        /// </summary>
        /// <returns></returns>
        Task<string> Clone();

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
