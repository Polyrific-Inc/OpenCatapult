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
        /// Deploy artifact
        /// </summary>
        /// <param name="config">Deploy task configuration</param>
        /// <returns></returns>
        Task<(string returnValue, string errorMessage)> Deploy(DeployTaskConfig config);
    }
}