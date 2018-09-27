// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using AzureAppService.Helpers;
using Microsoft.Extensions.Logging;

namespace AzureAppService
{
    public class MsDeployUtils : IMsDeployUtils
    {
        private const string MsDeployLocation = @"C:\Program Files\IIS\Microsoft Web Deploy V3\";

        private readonly ILogger _logger;

        public MsDeployUtils(ILogger logger)
        {
            _logger = logger;
        }

        public bool ExecuteDeployWebsite(string url, string username, string password, string packagePath)
        {
            _logger.LogDebug("Deploying website. Source: " + packagePath);
            var msDeploy = "msdeploy.exe";
            var args = $@"-verb:sync -source:package='{packagePath}' -dest:auto,ComputerName='{GetDeployUrl(url)}',UserName='{username}',Password='{password}',AuthType='Basic' -enableRule:DoNotDeleteRule -setParam:name='IIS Web Application Name',value='Default Web Site'";

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

        private string GetDeployUrl(string gitUrl)
        {
            // enforce https
            gitUrl = gitUrl.Replace("http://", "https://");
            var url = !gitUrl.Contains("https://") ? $"https://{gitUrl}" : gitUrl;

            return $"{url}/msdeploy.axd";
        }
    }
}
