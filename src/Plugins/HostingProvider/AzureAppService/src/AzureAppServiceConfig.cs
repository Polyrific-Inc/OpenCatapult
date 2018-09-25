// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace AzureAppService
{
    public class AzureAppServiceConfig
    {
        /// <summary>
        /// Id of the registered application
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Secret key of the registered application
        /// </summary>
        public string ApplicationKey { get; set; }

        /// <summary>
        /// Id of the tenant
        /// </summary>
        public string TenantId { get; set; }
    }
}
