﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Azure.Management.AppService.Fluent;

namespace Polyrific.Catapult.Plugins.AzureAppService
{
    public interface IAzureUtils
    {
        /// <summary>
        /// Get the azure deployment slot
        /// </summary>
        /// <param name="webApp">The azure website</param>
        /// <param name="name">Name of the slot</param>
        /// <returns>The azure deployment slot</returns>
        IDeploymentSlot GetSlot(IWebApp webApp, string name);
        
        /// <summary>
        /// Create a new slot if it does not exists
        /// </summary>
        /// <param name="webApp">The azure website</param>
        /// <param name="slotName">Name of the slot</param>
        /// <returns>The azure deployment slot</returns>
        IDeploymentSlot CreateSlot(IWebApp webApp, string slotName);

        /// <summary>
        /// Get the publish profile for a web or slot
        /// </summary>
        /// <param name="deployTarget">Azure website or slot</param>
        /// <returns>The publish profile</returns>
        IPublishingProfile GetPublishingProfile(IWebAppBase deployTarget);

        /// <summary>
        /// Set a web or slot connection string
        /// </summary>
        /// <param name="deployTarget">Azure website or slot</param>
        /// <param name="connectionStringName">Connection string name</param>
        /// <param name="connectionString">Connection string</param>
        /// <returns>The updated web or slot</returns>
        IWebAppBase SetConnectionString(IWebAppBase deployTarget, string connectionStringName, string connectionString);

        /// <summary>
        /// Create an azure web app if it's not yet exist
        /// </summary>
        /// <param name="subscriptionId">Subscription Id used to access</param>
        /// <param name="resourceGroupName">Resource group name where the website located</param>
        /// <param name="name">instance name of the azure app service</param>
        /// <param name="regionName">The region for which the web will be created</param>
        /// <param name="planName">The plan name to be used for web create</param>
        /// <returns></returns>
        IWebApp CreateWebsiteIfNotExists(string subscriptionId, string resourceGroupName, string appName, string regionName, string planName);
    }
}
