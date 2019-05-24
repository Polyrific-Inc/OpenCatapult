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
        private readonly IApplicationSettingRepository _applicationSettingRepository;

        public ApplicationSettingService(IApplicationSettingRepository applicationSettingRepository)
        {
            _applicationSettingRepository = applicationSettingRepository;
        }

        public async Task<List<ApplicationSetting>> GetApplicationSettings(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var spec = new ApplicationSettingFilterSpecification();
            var result = await _applicationSettingRepository.GetBySpec(spec, cancellationToken);

            return result.ToList();
        }

        public async Task UpdateApplicationSettings(Dictionary<string, string> applicationSettings, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var applicationSetting in applicationSettings)
            {

                var spec = new ApplicationSettingFilterSpecification(applicationSetting.Key);
                var result = await _applicationSettingRepository.GetSingleBySpec(spec);
                result.Value = applicationSetting.Value;

                await _applicationSettingRepository.Update(result, cancellationToken);
            }
        }
    }
}
