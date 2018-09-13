// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities
{
    public class FakeBuildProvider : IBuildProvider
    {
        private readonly (string returnValue, string errorMessage) _buildResult;

        public FakeBuildProvider(string returnValue, string errorMessage)
        {
            _buildResult = (returnValue, errorMessage);
        }

        public string Name => nameof(FakeBuildProvider);

        public Task<(string returnValue, string errorMessage)> Build(BuildTaskConfig config)
        {
            return Task.FromResult(_buildResult);
        }
    }
}