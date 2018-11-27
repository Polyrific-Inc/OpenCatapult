// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.Plugins.AzureAppService
{
    public interface IDeployUtils
    {
        /// <summary>
        /// Execute ms deploy
        /// </summary>
        /// <param name="url">Url of the target server</param>
        /// <param name="username">Username used to authenticate to the target server</param>
        /// <param name="password">Password used to authenticate to the target server</param>
        /// <param name="artifactLocation">Location of the artifact package</param>
        /// <returns></returns>
        Task<bool> ExecuteDeployWebsiteAsync(string url, string username, string password, string artifactLocation);
    }
}
