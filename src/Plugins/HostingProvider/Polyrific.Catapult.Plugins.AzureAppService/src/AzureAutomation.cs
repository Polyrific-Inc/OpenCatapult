// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.Plugins.AzureAppService
{
    public class AzureAutomation : IAzureAutomation
    {
        private readonly AzureAppServiceConfig _config;
        private readonly IAzureUtils _azureUtils;
        private readonly IDeployUtils _deployUtils;
        private readonly ILogger _logger;

        public AzureAutomation(AzureAppServiceConfig config, IAzureUtils azureUtils, IDeployUtils deployUtils, ILogger logger)
        {
            _config = config;
            _azureUtils = azureUtils ?? new AzureUtils(config.ApplicationId, config.ApplicationKey, config.TenantId, logger);
            _deployUtils = deployUtils ?? new KuduDeployUtils(logger);
            _logger = logger;
        }

        public async Task<(string, string)> DeployWebsite(string artifactLocation, 
            string subscriptionId, string resourceGroupName, string appServiceName, 
            string deploymentSlot, string connectionString, string regionName, string planName)
        {
            try
            {
                var hostLocation = "";

                var website = _azureUtils.GetOrCreateWebsite(subscriptionId, resourceGroupName, appServiceName, regionName, planName);
                if (website == null)
                {
                    var error = $"Website {appServiceName} is not found in {resourceGroupName}";
                    _logger.LogError(error);
                    return (hostLocation, error);
                }

                _logger.LogDebug("Deploying artifact to Azure App Service.");


                if (!string.IsNullOrEmpty(deploymentSlot))
                {
                    var slot = _azureUtils.GetSlot(website, deploymentSlot) ?? _azureUtils.CreateSlot(website, deploymentSlot);

                    if (!string.IsNullOrEmpty(connectionString))
                        _azureUtils.SetConnectionString(slot, "DefaultConnection", connectionString);

                    var publishProfile = _azureUtils.GetPublishingProfile(slot);
                    if (!(await _deployUtils.ExecuteDeployWebsiteAsync(publishProfile.GitUrl, publishProfile.GitUsername, publishProfile.GitPassword, artifactLocation)))
                    {
                        var error = $"Failed to deploy website to {appServiceName}-{deploymentSlot}.";
                        _logger.LogError(error);
                        return (hostLocation, error);
                    }

                    hostLocation = slot.DefaultHostName;
                }
                else
                {
                    var publishProfile = _azureUtils.GetPublishingProfile(website);

                    if (!string.IsNullOrEmpty(connectionString))
                        _azureUtils.SetConnectionString(website, "DefaultConnection", connectionString);

                    if (!(await _deployUtils.ExecuteDeployWebsiteAsync(publishProfile.GitUrl, publishProfile.GitUsername, publishProfile.GitPassword, artifactLocation)))
                    {
                        var error = $"Failed to deploy website to {appServiceName}.";
                        _logger.LogError(error);
                        return (hostLocation, error);
                    }

                    hostLocation = website.DefaultHostName;
                }

                return (hostLocation, "");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (null, ex.Message);
            }
        }
    }
}
