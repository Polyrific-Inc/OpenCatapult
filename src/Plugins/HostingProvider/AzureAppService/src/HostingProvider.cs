// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
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
        private readonly IDeployUtils _msDeployUtils;

        public HostingProvider()
        {
            
        }

        public HostingProvider(IAzureAutomation azure)
        {
            _azure = azure;
        }

        public HostingProvider(IAzureUtils azureUtils, IDeployUtils msDeployUtils)
        {
            _azureUtils = azureUtils;
            _msDeployUtils = msDeployUtils;
        }

        public string Name => "AzureAppService";

        public string[] RequiredServices => new[] {"AzureAppService"};

        public Task<string> BeforeDeploy(string projectName, DeployTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string hostLocation, Dictionary<string, string> outputValues, string errorMessage)> Deploy(string projectName, DeployTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            if (_azure == null)
                _azure = new AzureAutomation(GetAzureAppServiceConfig(additionalConfigs), _azureUtils, _msDeployUtils, logger);

            var subscriptionId = "";
            if (additionalConfigs.ContainsKey("SubscriptionId") && !string.IsNullOrEmpty(additionalConfigs["SubscriptionId"]))
                subscriptionId = additionalConfigs["SubscriptionId"];

            var resourceGroupName = "";
            if (additionalConfigs.ContainsKey("ResourceGroupName") && !string.IsNullOrEmpty(additionalConfigs["ResourceGroupName"]))
                resourceGroupName = additionalConfigs["ResourceGroupName"];

            var appServiceName = "";
            if (additionalConfigs.ContainsKey("AppServiceName") && !string.IsNullOrEmpty(additionalConfigs["AppServiceName"]))
                appServiceName = additionalConfigs["AppServiceName"];

            var deploymentSlot = "";
            if (additionalConfigs.ContainsKey("DeploymentSlot") && !string.IsNullOrEmpty(additionalConfigs["DeploymentSlot"]))
                deploymentSlot = additionalConfigs["DeploymentSlot"];

            var connectionString = "";
            if (additionalConfigs.ContainsKey("ConnectionString") && !string.IsNullOrEmpty(additionalConfigs["ConnectionString"]))
                connectionString = additionalConfigs["ConnectionString"];

            var artifactLocation = config.ArtifactLocation ?? Path.Combine(config.WorkingLocation, "artifact", $"{projectName}.zip");
            if (!Path.IsPathRooted(artifactLocation))
                artifactLocation = Path.Combine(config.WorkingLocation, artifactLocation);

            var (hostLocation, error) = await _azure.DeployWebsite(artifactLocation, subscriptionId, resourceGroupName, appServiceName, deploymentSlot, connectionString);
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);

            return (hostLocation, null, "");
        }

        public Task<string> AfterDeploy(string projectName, DeployTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        private AzureAppServiceConfig GetAzureAppServiceConfig(Dictionary<string, string> additionalConfigs)
        {
            var config = new AzureAppServiceConfig();

            if (additionalConfigs.ContainsKey("ApplicationId"))
                config.ApplicationId = additionalConfigs["ApplicationId"];

            if (additionalConfigs.ContainsKey("ApplicationKey"))
                config.ApplicationKey = additionalConfigs["ApplicationKey"];

            if (additionalConfigs.ContainsKey("TenantId"))
                config.TenantId = additionalConfigs["TenantId"];

            return config;
        }
    }
}
