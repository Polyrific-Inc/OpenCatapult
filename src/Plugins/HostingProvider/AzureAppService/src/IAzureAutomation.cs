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
        /// <param name="subscriptionId">Azure subscription id</param>
        /// <param name="resourceGroupName">Name of the resource group</param>
        /// <param name="appServiceName">Name of the App Service</param>
        /// <param name="deploymentSlot">Deployment slot. If empty, it will deploy to production slot.</param>
        /// <param name="config">Deploy task configuration</param>
        /// <returns>Error message</returns>
        Task<string> DeployWebsite(string artifactLocation, string subscriptionId, string resourceGroupName, string appServiceName, string deploymentSlot, DeployTaskConfig config);
    }
}
