// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.ApiClient.Options
{
    public class RetryPolicyOptions
    {
        /// <summary>
        /// Number of retries when transient http errors are detected
        /// </summary>
        public int Count { get; set; } = 3;

        /// <summary>
        /// The duration to wait / exponential backoff (in seconds) before proceeding to the next retry attempt
        /// </summary>
        public int BackoffPower { get; set; } = 2;
    }
}