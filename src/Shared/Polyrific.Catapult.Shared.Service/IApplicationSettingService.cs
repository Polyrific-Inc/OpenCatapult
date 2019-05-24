// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ApplicationSetting;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IApplicationSettingService
    {
        /// <summary>
        /// Get the application settings metadata
        /// </summary>
        /// <returns></returns>
        Task<List<ApplicationSettingDto>> GetApplicationSettings();

        /// <summary>
        /// Get the application setting values
        /// </summary>
        /// <returns></returns>
        Task<ApplicationSettingValueDto> GetApplicationSettingValue();

        /// <summary>
        /// Update the application setting values
        /// </summary>
        /// <param name="dto">The updated value of application settings</param>
        /// <returns></returns>
        Task UpdateApplicationSetting(UpdateApplicationSettingDto dto);
    }
}
