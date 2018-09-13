// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Shared.ApiClient.Options
{
    public class CircuitBreakerPolicyOptions
    {
        /// <summary>
        /// The duration the circuit will stay open before resetting.
        /// </summary>
        public TimeSpan DurationOfBreak { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// The number of exceptions or handled results that are allowed before opening the circuit.
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 20;
    }
}