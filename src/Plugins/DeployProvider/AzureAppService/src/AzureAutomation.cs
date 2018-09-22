// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AzureAppService.Helpers;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Rest.Azure;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace AzureAppService
{
    public class AzureAutomation : IAzureAutomation
    {
        private const string MsDeployLocation = @"C:\Program Files\IIS\Microsoft Web Deploy V3\";

        private readonly AzureAppServiceConfig _config;
        private readonly ILogger _logger;

        public AzureAutomation(AzureAppServiceConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public Task<string> DeployWebsite(string artifactLocation, DeployTaskConfig taskConfig)
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(_config.ApplicationId,
                _config.ApplicationKey, _config.TenantId, AzureEnvironment.AzureGlobalCloud);

            var azure = Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.BodyAndHeaders)
                .Authenticate(credentials)
                .WithSubscription(taskConfig.SubscriptionId);

            var website = GetWebsite(azure, taskConfig.ResourceGroupName, taskConfig.AppServiceName);
            if (website == null)
            {
                var error = $"Website {taskConfig.AppServiceName} is not found in {taskConfig.ResourceGroupName}";
                _logger.LogError(error);
                return Task.FromResult(error);
            }
            
            _logger.LogDebug("Deploying artifact to Azure App Service.");

            if (!string.IsNullOrEmpty(taskConfig.DeploymentSlot))
            {
                var slot = GetSlot(website, taskConfig.DeploymentSlot) ?? CreateSlot(website, taskConfig.DeploymentSlot);

                if (!ExecuteDeployWebsite(ExecuteAzureFunction(() => slot.GetPublishingProfile()), artifactLocation))
                {
                    var error = $"Failed to deploy website to {taskConfig.DeploymentSlot}.";
                    _logger.LogError(error);
                    return Task.FromResult(error);
                }
            }
            else
            {
                if (!ExecuteDeployWebsite(ExecuteAzureFunction(() => website.GetPublishingProfile()), artifactLocation))
                {
                    var error = $"Failed to deploy website to {taskConfig.DeploymentSlot}.";
                    _logger.LogError(error);
                    return Task.FromResult(error);
                }
            }

            return Task.FromResult("");
        }

        #region Private Methods
        private IWebApp GetWebsite(IAzure azure, string resourceGroupName, string name)
        {
            IWebApp webApp = null;

            try
            {
                webApp = ExecuteAzureFunction(() => azure.WebApps.GetByResourceGroup(resourceGroupName, name));
            }
            catch (CloudException cex)
            {
                if (cex.Body.Code != "ResourceNotFound")
                    throw;
            }

            return webApp;
        }

        private IDeploymentSlot GetSlot(IWebApp webApp, string name)
        {
            IDeploymentSlot slot = null;

            try
            {
                slot = ExecuteAzureFunction(() => webApp.DeploymentSlots.GetByName(name));
            }
            catch (CloudException cex)
            {
                if (cex.Body.Code != "ResourceNotFound")
                    throw;
            }

            return slot;
        }

        private bool ExecuteDeployWebsite(IPublishingProfile publishProfile, string packagePath)
        {
            _logger.LogDebug("Deploying website. Source: " + packagePath);
            var msDeploy = "msdeploy.exe";
            var args = $@"-verb:sync -source:package='{packagePath}' -dest:auto,ComputerName='{GetDeployUrl(publishProfile.GitUrl)}',UserName='{publishProfile.GitUsername}',Password='{publishProfile.GitPassword}',AuthType='Basic' -enableRule:DoNotDeleteRule -setParam:name='IIS Web Application Name',value='Default Web Site'";
            
            var output = CommandHelper.Execute(Path.Combine(MsDeployLocation, msDeploy), args, _logger).Result;

            if (output.Contains("Total changes:"))
            {
                _logger.LogDebug("Website deployed");
            }
            else
            {
                return false;
            }

            return true;
        }
        
        private IDeploymentSlot CreateSlot(IWebApp webApp, string slotName)
        {
            _logger.LogDebug("Creating a slot " + slotName);

            var slot = ExecuteAzureFunction(() => webApp.DeploymentSlots
                    .Define(slotName)
                    .WithConfigurationFromParent()
                    .Create());

            _logger.LogDebug("Created slot " + slot.Name);

            return slot;
        }

        private string GetDeployUrl(string gitUrl)
        {
            // enforce https
            gitUrl = gitUrl.Replace("http://", "https://");
            var url = !gitUrl.Contains("https://") ? $"https://{gitUrl}" : gitUrl;
            
            return $"{url}/msdeploy.axd";
        }

        private T ExecuteAzureFunction<T>(Func<T> function)
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
                catch (Exception ex)
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
