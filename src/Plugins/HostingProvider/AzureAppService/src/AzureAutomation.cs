// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace AzureAppService
{
    public class AzureAutomation : IAzureAutomation
    {
        private readonly AzureAppServiceConfig _config;
        private readonly IAzureUtils _azureUtils;
        private readonly IMsDeployUtils _msDeployUtils;
        private readonly ILogger _logger;

        public AzureAutomation(AzureAppServiceConfig config, IAzureUtils azureUtils, IMsDeployUtils msDeployUtils, ILogger logger)
        {
            _config = config;
            _azureUtils = azureUtils ?? new AzureUtils(config.ApplicationId, config.ApplicationKey, config.TenantId, logger);
            _msDeployUtils = msDeployUtils ?? new MsDeployUtils(logger);
            _logger = logger;
        }

        public Task<string> DeployWebsite(string artifactLocation, string subscriptionId, string resourceGroupName, string appServiceName, string deploymentSlot, DeployTaskConfig taskConfig, out string hostLocation)
        {
            hostLocation = "";

            var website = _azureUtils.GetWebsite(subscriptionId, resourceGroupName, appServiceName);
            if (website == null)
            {
                var error = $"Website {appServiceName} is not found in {resourceGroupName}";
                _logger.LogError(error);
                return Task.FromResult(error);
            }
            
            _logger.LogDebug("Deploying artifact to Azure App Service.");


            if (!string.IsNullOrEmpty(deploymentSlot))
            {
                var slot = _azureUtils.GetSlot(website, deploymentSlot) ?? _azureUtils.CreateSlot(website, deploymentSlot);
                var publishProfile = _azureUtils.GetPublishingProfile(slot);
                if (!_msDeployUtils.ExecuteDeployWebsite(publishProfile.GitUrl, publishProfile.GitUsername, publishProfile.GitPassword, artifactLocation))
                {
                    var error = $"Failed to deploy website to {appServiceName}-{deploymentSlot}.";
                    _logger.LogError(error);
                    return Task.FromResult(error);
                }

                hostLocation = slot.DefaultHostName;
            }
            else
            {
                var publishProfile = _azureUtils.GetPublishingProfile(website);
                if (!_msDeployUtils.ExecuteDeployWebsite(publishProfile.GitUrl, publishProfile.GitUsername, publishProfile.GitPassword, artifactLocation))
                {
                    var error = $"Failed to deploy website to {appServiceName}.";
                    _logger.LogError(error);
                    return Task.FromResult(error);
                }

                hostLocation = website.DefaultHostName;
            }

            return Task.FromResult("");
        }
    }
}
