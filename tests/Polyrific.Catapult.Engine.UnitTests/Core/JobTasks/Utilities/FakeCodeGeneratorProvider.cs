// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities
{
    public class FakeCodeGeneratorProvider : ICodeGeneratorProvider
    {
        private readonly (string returnValue, string errorMessage) _generateResult;

        public FakeCodeGeneratorProvider(string returnValue, string errorMessage)
        {
            _generateResult = (returnValue, errorMessage);
        }

        public string Name => nameof(FakeCodeGeneratorProvider);

        public Task<(string outputLocation, string errorMessage)> Generate(string projectName, List<ProjectDataModelDto> models, string outputFolderName, GenerateTaskConfig config)
        {
            return Task.FromResult(_generateResult);
        }
    }
}