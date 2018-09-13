// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Shared.ApiClient.Options
{
    public class ApiClientOptions
    {
        /// <summary>
        /// Authorization token
        /// </summary>
        public string AuthorizationToken { get; set; }

        /// <summary>
        /// Base address of the API
        /// </summary>
        public Uri ApiUrl { get; set; }

        /// <summary>
        /// Request timeout
        /// </summary>
        public TimeSpan ApiRequestTimeout { get; set; }
    }
}