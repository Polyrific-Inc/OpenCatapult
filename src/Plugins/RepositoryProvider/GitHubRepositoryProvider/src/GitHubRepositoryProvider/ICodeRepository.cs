// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace GitHubRepositoryProvider
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
        /// <returns></returns>
        Task<string> Push(string branch);
    }
}
