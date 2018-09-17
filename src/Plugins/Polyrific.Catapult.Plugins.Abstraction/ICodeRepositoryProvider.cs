// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        /// Required service connections
        /// </summary>
        string[] RequiredServices { get; }

        /// <summary>
        /// Process to run before executing repository clone
        /// </summary>
        /// <param name="repositoryFolder">Local repository folder (this is relative to the working folder)</param>
        /// <param name="config">Clone task configuration</param>
        /// <param name="serviceProperties">Properties from the required services</param>
        /// <returns>Error message</returns>
        Task<string> BeforeClone(string repositoryFolder, CloneTaskConfig config, Dictionary<string, string> serviceProperties);

        /// <summary>
        /// Clone source code from remote repository
        /// </summary>
        /// <param name="repositoryFolder">Local repository folder (this is relative to the working folder)</param>
        /// <param name="config">Clone task configuration</param>
        /// <param name="serviceProperties">Properties from the required services</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Tuple of (returnValue, errorMessae)</returns>
        Task<(string returnValue, string errorMessage)> Clone(string repositoryFolder, CloneTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger);

        /// <summary>
        /// Process to run after executing repository clone
        /// </summary>
        /// <param name="repositoryFolder">Local repository folder (this is relative to the working folder)</param>
        /// <param name="config">Clone task configuration</param>
        /// <param name="serviceProperties">Properties from the required services</param>
        /// <returns>Error message</returns>
        Task<string> AfterClone(string repositoryFolder, CloneTaskConfig config, Dictionary<string, string> serviceProperties);

        /// <summary>
        /// Process to run before pushing code to remote repository
        /// </summary>
        /// <param name="repositoryFolder">Local repository folder (this is relative to the working folder)</param>
        /// <param name="config"></param>
        /// <param name="serviceProperties">Properties from the required services</param>
        /// <returns>Error message</returns>
        Task<string> BeforePush(string repositoryFolder, PushTaskConfig config, Dictionary<string, string> serviceProperties);

        /// <summary>
        /// Push source code to remote repository
        /// </summary>
        /// <param name="repositoryFolder">Local repository folder (this is relative to the working folder)</param>
        /// <param name="config">Push task configuration</param>
        /// <param name="serviceProperties">Properties from the required services</param>
        /// <param name="logger">Instance <see cref="ILogger"/></param>
        /// <returns>Tuple of (returnValue, errorMessage)/></returns>
        Task<(string returnValue, string errorMessage)> Push(string repositoryFolder, PushTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger);

        /// <summary>
        /// Process to run after pushing code to remote repository
        /// </summary>
        /// <param name="repositoryFolder">Local repository folder (this is relative to the working folder)</param>
        /// <param name="config"></param>
        /// <param name="serviceProperties">Properties from the required services</param>
        /// <returns>Error message</returns>
        Task<string> AfterPush(string repositoryFolder, PushTaskConfig config, Dictionary<string, string> serviceProperties);
    }
}
