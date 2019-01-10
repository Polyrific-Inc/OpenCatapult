// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Rest.Azure;

namespace Polyrific.Catapult.Plugins.AzureAppService
{
    public class AzureUtils : IAzureUtils
    {
        private readonly ILogger _logger;
        private readonly Azure.IAuthenticated _authenticatedAzure;

        public AzureUtils(string applicationId, string applicationKey, string tenantId, ILogger logger)
        {
            _logger = logger;

            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(applicationId,
                applicationKey, tenantId, AzureEnvironment.AzureGlobalCloud);

            _authenticatedAzure = Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.BodyAndHeaders)
                .Authenticate(credentials);
        }

        public IWebApp GetWebsite(string subscriptionId, string resourceGroupName, string name)
        {
            IWebApp webApp = null;

            try
            {
                webApp = ExecuteWithRetry(() => _authenticatedAzure.WithSubscription(subscriptionId).WebApps.GetByResourceGroup(resourceGroupName, name));
            }
            catch (CloudException cex)
            {
                if (cex.Body.Code != "ResourceNotFound")
                    throw;
            }

            return webApp;
        }

        public IDeploymentSlot GetSlot(IWebApp webApp, string name)
        {
            IDeploymentSlot slot = null;

            try
            {
                slot = ExecuteWithRetry(() => webApp.DeploymentSlots.GetByName(name));
            }
            catch (CloudException cex)
            {
                if (cex.Body.Code != "ResourceNotFound")
                    throw;
            }

            return slot;
        }

        public IDeploymentSlot CreateSlot(IWebApp webApp, string slotName)
        {
            _logger.LogDebug("Creating a slot " + slotName);

            var slot = ExecuteWithRetry(() => webApp.DeploymentSlots
                    .Define(slotName)
                    .WithConfigurationFromParent()
                    .Create());

            _logger.LogDebug("Created slot " + slot.Name);

            return slot;
        }

        public IPublishingProfile GetPublishingProfile(IWebAppBase deployTarget)
        {
            return ExecuteWithRetry(() => deployTarget.GetPublishingProfile());
        }

        public IWebAppBase SetConnectionString(IWebAppBase deployTarget, string connectionStringName, string connectionString)
        {
            _logger.LogInformation("Setting connection string...");
            IWebAppBase web;
            if (deployTarget is IWebApp webApp)
                web = ExecuteWithRetry(() => webApp.Update().WithConnectionString(connectionStringName, connectionString, Microsoft.Azure.Management.AppService.Fluent.Models.ConnectionStringType.Custom).Apply());
            else if (deployTarget is IDeploymentSlot slot)
                web = ExecuteWithRetry(() => slot.Update().WithConnectionString(connectionStringName, connectionString, Microsoft.Azure.Management.AppService.Fluent.Models.ConnectionStringType.Custom).Apply());
            else
                web = null;

            var connStrings = ExecuteWithRetry(() => web?.GetConnectionStrings());
            if (connStrings == null || !connStrings.ContainsKey(connectionStringName))
            {
                throw new Exception("Failed setting connection string. Please check for the user permission");
            }

            return web;
        }

        #region Private Methods
        private T ExecuteWithRetry<T>(Func<T> function)
        {
            var attempt = 0;
            var returnValue = default(T);

            while (attempt < 5 && Equals(returnValue, default(T)))
            {
                attempt++;

                try
                {
                    returnValue = function();
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _logger.LogInformation($"Connection attempt {attempt} failed");

                    if (attempt < 5)
                    {
                        var seconds = 30;
                        _logger.LogInformation($"Retrying in {seconds} seconds...");
                        Thread.Sleep(seconds * 1000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return returnValue;
        }
        #endregion Private Methods
    }
}
