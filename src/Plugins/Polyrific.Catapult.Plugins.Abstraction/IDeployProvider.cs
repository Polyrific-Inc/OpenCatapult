// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace Polyrific.Catapult.Plugins.Abstraction
{
    public interface IDeployProvider
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
        /// Process to run before executing deploy
        /// </summary>
        /// <param name="config">Deploy task configuration</param>
        /// <param name="serviceProperties">Properties from the required services</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> BeforeDeploy(DeployTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger);

        /// <summary>
        /// Deploy artifact
        /// </summary>
        /// <param name="artifactLocation">Location of the artifact package</param>
        /// <param name="config">Deploy task configuration</param>
        /// <param name="serviceProperties">Properties from the required services</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns></returns>
        Task<(string returnValue, string errorMessage)> Deploy(string artifactLocation, DeployTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger);

        /// <summary>
        /// Process to run after executing deploy
        /// </summary>
        /// <param name="config">Deploy task configuration</param>
        /// <param name="serviceProperties">Properties from the required services</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> AfterDeploy(DeployTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger);
    }
}
