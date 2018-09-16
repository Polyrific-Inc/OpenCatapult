// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace Polyrific.Catapult.Plugins.Abstraction
{
    public interface ICodeRepositoryProvider
    {
        /// <summary>
        /// Name of the provider
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Process to run before executing repository clone
        /// </summary>
        /// <returns>Error message</returns>
        Task<string> BeforeClone();

        /// <summary>
        /// Clone source code from remote repository
        /// </summary>
        /// <returns>Tuple of (returnValue, errorMessae)</returns>
        Task<(string returnValue, string errorMessage)> Clone();

        /// <summary>
        /// Process to run after executing repository clone
        /// </summary>
        /// <returns>Error message</returns>
        Task<string> AfterClone();

        /// <summary>
        /// Process to run before pushing code to remote repository
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Error message</returns>
        Task<string> BeforePush(PushTaskConfig config);

        /// <summary>
        /// Push source code to remote repository
        /// </summary>
        /// <param name="config">Push task configuration</param>
        /// <returns>Tuple of (returnValue, errorMessage)/></returns>
        Task<(string returnValue, string errorMessage)> Push(PushTaskConfig config);

        /// <summary>
        /// Process to run after pushing code to remote repository
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Error message</returns>
        Task<string> AfterPush(PushTaskConfig config);
    }
}
