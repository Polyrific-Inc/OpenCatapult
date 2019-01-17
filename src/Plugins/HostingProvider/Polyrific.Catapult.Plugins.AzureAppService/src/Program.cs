using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Core;

namespace Polyrific.Catapult.Plugins.AzureAppService
{
    public class Program : HostingProvider
    {
        private const string TaskProviderName = "Polyrific.Catapult.Plugins.AzureAppService";

        private IAzureAutomation _azure;
        private readonly IAzureUtils _azureUtils;
        private readonly IDeployUtils _deployUtils;

        public Program() : base(new string[0], TaskProviderName)
        {
        }

        public Program(string[] args) : base(args, TaskProviderName)
        {
        }

        public Program(string[] args, IAzureUtils azureUtils, IDeployUtils deployUtils)
            : this(args)
        {
            _azureUtils = azureUtils;
            _deployUtils = deployUtils;
        }

        public override string Name => TaskProviderName;

        public override string[] RequiredServices => new[] { "Azure" };

        public override async Task<(string hostLocation, Dictionary<string, string> outputValues, string errorMessage)> Deploy()
        {
            if (_azure == null)
                _azure = new AzureAutomation(GetAzureAppServiceConfig(AdditionalConfigs), _azureUtils, _deployUtils, Logger);

            var subscriptionId = "";
            if (AdditionalConfigs.ContainsKey("SubscriptionId") && !string.IsNullOrEmpty(AdditionalConfigs["SubscriptionId"]))
                subscriptionId = AdditionalConfigs["SubscriptionId"];

            var resourceGroupName = "";
            if (AdditionalConfigs.ContainsKey("ResourceGroupName") && !string.IsNullOrEmpty(AdditionalConfigs["ResourceGroupName"]))
                resourceGroupName = AdditionalConfigs["ResourceGroupName"];

            var appServiceName = "";
            if (AdditionalConfigs.ContainsKey("AppServiceName") && !string.IsNullOrEmpty(AdditionalConfigs["AppServiceName"]))
                appServiceName = AdditionalConfigs["AppServiceName"];

            var deploymentSlot = "";
            if (AdditionalConfigs.ContainsKey("DeploymentSlot") && !string.IsNullOrEmpty(AdditionalConfigs["DeploymentSlot"]))
                deploymentSlot = AdditionalConfigs["DeploymentSlot"];

            var connectionString = "";
            if (AdditionalConfigs.ContainsKey("ConnectionString") && !string.IsNullOrEmpty(AdditionalConfigs["ConnectionString"]))
                connectionString = AdditionalConfigs["ConnectionString"];

            var region = "southcentralus";
            if (AdditionalConfigs.ContainsKey("Region") && !string.IsNullOrEmpty(AdditionalConfigs["Region"]))
                region = AdditionalConfigs["Region"];

            var appServicePlan = "";
            if (AdditionalConfigs.ContainsKey("AppServicePlan") && !string.IsNullOrEmpty(AdditionalConfigs["AppServicePlan"]))
                appServicePlan = AdditionalConfigs["AppServicePlan"];

            var artifactLocation = Config.ArtifactLocation ?? Path.Combine(Config.WorkingLocation, "artifact", $"{ProjectName}.zip");
            if (!Path.IsPathRooted(artifactLocation))
                artifactLocation = Path.Combine(Config.WorkingLocation, artifactLocation);

            var (hostLocation, error) = await _azure.DeployWebsite(artifactLocation, subscriptionId, resourceGroupName, appServiceName, deploymentSlot, connectionString, region, appServicePlan);
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);

            return (hostLocation, null, "");
        }

        private static async Task Main(string[] args)
        {
            var app = new Program(args);

            var result = await app.Execute();
            app.ReturnOutput(result);
        }

        private AzureAppServiceConfig GetAzureAppServiceConfig(Dictionary<string, string> AdditionalConfigs)
        {
            var Config = new AzureAppServiceConfig();

            if (AdditionalConfigs.ContainsKey("ApplicationId"))
                Config.ApplicationId = AdditionalConfigs["ApplicationId"];

            if (AdditionalConfigs.ContainsKey("ApplicationKey"))
                Config.ApplicationKey = AdditionalConfigs["ApplicationKey"];

            if (AdditionalConfigs.ContainsKey("TenantId"))
                Config.TenantId = AdditionalConfigs["TenantId"];

            return Config;
        }
    }
}
