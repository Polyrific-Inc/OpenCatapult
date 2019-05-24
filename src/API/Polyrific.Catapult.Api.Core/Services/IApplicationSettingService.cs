// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IApplicationSettingService
    {
        /// <summary>
        /// Get the application settings
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<List<ApplicationSetting>> GetApplicationSettings(CancellationToken cancellationToken = default);

        /// <summary>
        /// Update the application settings
        /// </summary>
        /// <param name="applicationSettings">Application settings keys and values</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateApplicationSettings(Dictionary<string, string> applicationSettings, CancellationToken cancellationToken = default);
    }
}
