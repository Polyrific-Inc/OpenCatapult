// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IHealthService
    {
        /// <summary>
        /// Check remote system health
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckHealth();

        /// <summary>
        /// Check remote system health securely
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckHealthSecure();
    }
}