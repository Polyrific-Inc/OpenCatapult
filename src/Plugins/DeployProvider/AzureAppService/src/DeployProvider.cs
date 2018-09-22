// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace AzureAppService
{
    [Export(typeof(IDeployProvider))]
    public class DeployProvider : IDeployProvider
    {
        private IAzureAutomation _azure;

        public DeployProvider()
        {
            
        }

        public DeployProvider(IAzureAutomation azure)
        {
            _azure = azure;
        }

        public string Name => "AzureAppService";

        public string[] RequiredServices => new[] {"AzureAppService"};

        public Task<string> BeforeDeploy(DeployTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string returnValue, string errorMessage)> Deploy(string artifactLocation, DeployTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger)
        {
            if (_azure == null)
                _azure = new AzureAutomation(GetAzureAppServiceConfig(serviceProperties), logger);

            var error = await _azure.DeployWebsite(artifactLocation, config);
            if (!string.IsNullOrEmpty(error))
                return ("", error);

            return ("", "");
        }

        public Task<string> AfterDeploy(DeployTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger)
        {
            return Task.FromResult("");
        }

        private AzureAppServiceConfig GetAzureAppServiceConfig(Dictionary<string, string> serviceProperties)
        {
            var config = new AzureAppServiceConfig();

            if (serviceProperties.ContainsKey("ApplicationId"))
                config.ApplicationId = serviceProperties["ApplicationId"];

            if (serviceProperties.ContainsKey("ApplicationKey"))
                config.ApplicationKey = serviceProperties["ApplicationKey"];

            if (serviceProperties.ContainsKey("TenantId"))
                config.TenantId = serviceProperties["TenantId"];

            return config;
        }
    }
}
