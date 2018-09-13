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
        /// Clone source code from remote repository
        /// </summary>
        /// <returns></returns>
        Task<(string returnValue, string errorMessage)> Clone();

        /// <summary>
        /// Push source code to remote repository
        /// </summary>
        /// <param name="config">Push task configuration</param>
        /// <returns></returns>
        Task<(string returnValue, string errorMessage)> Push(PushTaskConfig config);
    }
}