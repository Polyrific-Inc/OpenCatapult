// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IApplicationSettingService
    {
        Task<List<ApplicationSetting>> GetApplicationSettings(CancellationToken cancellationToken = default);
    }
}
