// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace AzureAppService
{
    [Export(typeof(IHostingProvider))]
    public class HostingProvider : IHostingProvider
    {
        private IAzureAutomation _azure;
        private readonly IAzureUtils _azureUtils;
        private readonly IMsDeployUtils _msDeployUtils;

        public HostingProvider()
        {
            
        }

        public HostingProvider(IAzureAutomation azure)
        {
            _azure = azure;
        }

        public HostingProvider(IAzureUtils azureUtils, IMsDeployUtils msDeployUtils)
        {
            _azureUtils = azureUtils;
            _msDeployUtils = msDeployUtils;
        }

        public string Name => "AzureAppService";

        public string[] RequiredServices => new[] {"AzureAppService"};

        public Task<string> BeforeDeploy(DeployTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string hostLocation, Dictionary<string, string> outputValues, string errorMessage)> Deploy(DeployTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            if (_azure == null)
                _azure = new AzureAutomation(GetAzureAppServiceConfig(additionalConfigs), _azureUtils, _msDeployUtils, logger);

            var subscriptionId = "";
            if (additionalConfigs.ContainsKey("SubscriptionId"))
                subscriptionId = additionalConfigs["SubscriptionId"];

            var resourceGroupName = "";
            if (additionalConfigs.ContainsKey("ResourceGroupName"))
                resourceGroupName = additionalConfigs["ResourceGroupName"];

            var appServiceName = "";
            if (additionalConfigs.ContainsKey("AppServiceName"))
                appServiceName = additionalConfigs["AppServiceName"];

            var deploymentSlot = "";
            if (additionalConfigs.ContainsKey("DeploymentSlot"))
                deploymentSlot = additionalConfigs["DeploymentSlot"];

            var error = await _azure.DeployWebsite(config.ArtifactLocation, subscriptionId, resourceGroupName, appServiceName, deploymentSlot, config, out var hostLocation);
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);

            return (hostLocation, null, "");
        }

        public Task<string> AfterDeploy(DeployTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        private AzureAppServiceConfig GetAzureAppServiceConfig(Dictionary<string, string> additionalConfigs)
        {
            var config = new AzureAppServiceConfig();

            if (additionalConfigs.ContainsKey("AzureAppService.ApplicationId"))
                config.ApplicationId = additionalConfigs["AzureAppService.ApplicationId"];

            if (additionalConfigs.ContainsKey("AzureAppService.ApplicationKey"))
                config.ApplicationKey = additionalConfigs["AzureAppService.ApplicationKey"];

            if (additionalConfigs.ContainsKey("AzureAppService.TenantId"))
                config.TenantId = additionalConfigs["AzureAppService.TenantId"];

            return config;
        }
    }
}
