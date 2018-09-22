// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace AzureAppService
{
    public interface IAzureAutomation
    {
        /// <summary>
        /// Deploy website
        /// </summary>
        /// <param name="artifactLocation">Location of the artifact package</param>
        /// <param name="config">Deploy task configuration</param>
        /// <returns>Error message</returns>
        Task<string> DeployWebsite(string artifactLocation, DeployTaskConfig config);
    }
}
