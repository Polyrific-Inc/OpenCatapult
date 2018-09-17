// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
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
        /// <returns>Error message</returns>
        Task<string> BeforeDeploy(DeployTaskConfig config);

        /// <summary>
        /// Deploy artifact
        /// </summary>
        /// <param name="config">Deploy task configuration</param>
        /// <returns></returns>
        Task<(string returnValue, string errorMessage)> Deploy(DeployTaskConfig config);

        /// <summary>
        /// Process to run after executing deploy
        /// </summary>
        /// <param name="config">Deploy task configuration</param>
        /// <returns>Error message</returns>
        Task<string> AfterDeploy(DeployTaskConfig config);
    }
}
