using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.TaskProviders.AzureAppService.Helpers;

namespace Polyrific.Catapult.TaskProviders.AzureAppService
{
    public class DotnetDeployUtils : IDeployUtils
    {
        private readonly ILogger _logger;

        public DotnetDeployUtils(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> ExecuteDeployWebsiteAsync(string appServiceName, string username, string password, string csProjToDeploy)
        {
            var publishProfileFile = CreatePublishProfile(csProjToDeploy, appServiceName, username, password);
            var publishArgs = $"publish \"{csProjToDeploy}\" /p:PublishProfile=Azure /p:Configuration=Release";

            var output = await CommandHelper.Execute("dotnet", publishArgs, _logger);

            publishProfileFile.Delete();

            if (string.IsNullOrEmpty(output.error) && output.output.Contains("Publish Succeeded."))
            {
                _logger.LogDebug("Website deployed");
            }
            else
            {
                _logger.LogError(output.error);
                return false;
            }

            return true;
        }

        private FileInfo CreatePublishProfile(string csProjToDeploy, string appServiceName, string username, string password)
        {
            var projectFile = new FileInfo(csProjToDeploy);

            var sb = new StringBuilder();
            sb.AppendLine("<Project>");
            sb.AppendLine("  <PropertyGroup>");
            sb.AppendLine("    <PublishProtocol>Kudu</PublishProtocol>");
            sb.AppendLine($"    <PublishSiteName>{appServiceName}</PublishSiteName>");
            sb.AppendLine($"    <UserName>{username}</UserName>");
            sb.AppendLine($"    <Password>{password}</Password>");
            sb.AppendLine("  </PropertyGroup>");
            sb.AppendLine("</Project>");

            var publishProfileFile = new FileInfo(Path.Combine(projectFile.Directory.FullName, "Properties", "PublishProfiles", "Azure.pubxml"));
            publishProfileFile.Directory.Create();

            File.WriteAllText(publishProfileFile.FullName, sb.ToString());

            return publishProfileFile;
        }
    }
}
