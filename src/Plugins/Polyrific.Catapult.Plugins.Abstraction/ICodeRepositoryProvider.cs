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
        /// <param name="config">Clone task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> BeforeClone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Clone source code from remote repository
        /// </summary>
        /// <param name="config">Clone task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Tuple of (returnValue, errorMessage)</returns>
        Task<(string cloneLocation, Dictionary<string, string> outputValues, string errorMessage)> Clone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Process to run after executing repository clone
        /// </summary>
        /// <param name="config">Clone task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> AfterClone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Process to run before pushing code to remote repository
        /// </summary>
        /// <param name="config">Push task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> BeforePush(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Push source code to remote repository
        /// </summary>
        /// <param name="config">Push task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Tuple of (returnValue, errorMessage)/></returns>
        Task<(string remoteUrl, Dictionary<string, string> outputValues, string errorMessage)> Push(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Process to run after pushing code to remote repository
        /// </summary>
        /// <param name="config">Push task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> AfterPush(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Process to run before executing pull request merge
        /// </summary>
        /// <param name="prNumber">Pull request number</param>
        /// <param name="config">Clone task configuration</param>
        /// <param name="additionalConfigs">Addition configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> BeforeMerge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Merge pull request
        /// </summary>
        /// <param name="prNumber">Pull request number</param>
        /// <param name="config">Clone task configuration</param>
        /// <param name="additionalConfigs">Addition configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Tuple of (returnValue, errorMessage)</returns>
        Task<(string remoteUrl, Dictionary<string, string> outputValues, string errorMessage)> Merge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Process to run after executing pull request merge
        /// </summary>
        /// <param name="prNumber">Pull request number</param>
        /// <param name="config">Clone task configuration</param>
        /// <param name="additionalConfigs">Addition configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> AfterMerge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);
    }
}
