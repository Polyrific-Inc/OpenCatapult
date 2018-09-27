// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace AzureAppService
{
    public interface IMsDeployUtils
    {
        /// <summary>
        /// Execute ms deploy
        /// </summary>
        /// <param name="url">Url of the target server</param>
        /// <param name="username">Username used to authenticate to the target server</param>
        /// <param name="password">Password used to authenticate to the target server</param>
        /// <param name="packagePath">The package path to be deployed</param>
        /// <returns></returns>
        bool ExecuteDeployWebsite(string url, string username, string password, string packagePath);
    }
}
