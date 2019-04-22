// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Dto.Provider;

namespace Polyrific.Catapult.Shared.Dto.Version
{
    public class VersionDto
    {
        public string ApiVersion { get; set; }
        public List<CatapultEngineDto> Engines { get; set; }
        public List<TaskProviderDto> TaskProviders { get; set; }
    }
}
