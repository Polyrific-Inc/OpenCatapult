// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace Polyrific.Catapult.Plugins.Abstraction
{
    public interface IDatabaseProvider
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
        /// Process to run before executing deploy database
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="config">Deploy database task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> BeforeDeployDatabase(string projectName, DeployDbTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Deploy database
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="config">Deploy database task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns></returns>
        Task<(string databaseLocation, Dictionary<string, string> outputValues, string errorMessage)> DeployDatabase(string projectName, DeployDbTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Process to run after executing deploy database
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="config">Deploy database task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> AfterDeployDatabase(string projectName, DeployDbTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);
    }
}
