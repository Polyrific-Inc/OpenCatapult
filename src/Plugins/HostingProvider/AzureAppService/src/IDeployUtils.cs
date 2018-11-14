// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace AzureAppService
{
    public interface IDeployUtils
    {
        /// <summary>
        /// Execute ms deploy
        /// </summary>
        /// <param name="appServiceName">Azure AppService name</param>
        /// <param name="username">Username used to authenticate to the target server</param>
        /// <param name="password">Password used to authenticate to the target server</param>
        /// <param name="csProjToDeploy">Location of the csproj file to be deployed</param>
        /// <returns></returns>
        Task<bool> ExecuteDeployWebsiteAsync(string appServiceName, string username, string password, string csProjToDeploy);
    }
}
