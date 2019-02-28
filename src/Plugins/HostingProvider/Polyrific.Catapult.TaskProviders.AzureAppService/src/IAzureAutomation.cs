// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.TaskProviders.AzureAppService
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
        /// <param name="connectionString">The connection string</param>
        /// <param name="planName">The plan name to be used for web create</param>
        /// <param name="regionName">The region for which the web will be created</param>
        /// <param name="allowAutomaticRename">Allows renaming the app service automatically if the original app service name is not available?</param>
        /// <returns>Host location and Error message if any</returns>
        Task<(string hostLocation, string error)> DeployWebsite(string artifactLocation, string subscriptionId, string resourceGroupName, string appServiceName, string deploymentSlot, string connectionString, string regionName, string planName, bool allowAutomaticRename);
    }
}
