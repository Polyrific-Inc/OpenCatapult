// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ApplicationSettingService : IApplicationSettingService
    {
        private readonly IApplicationSettingRepository _ApplicationSettingRepository;

        public ApplicationSettingService(IApplicationSettingRepository ApplicationSettingRepository)
        {
            _ApplicationSettingRepository = ApplicationSettingRepository;
        }

        public async Task<List<ApplicationSetting>> GetApplicationSettings(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var spec = new ApplicationSettingFilterSpecification();
            var result = await _ApplicationSettingRepository.GetBySpec(spec);

            return result.ToList();
        }
    }
}
