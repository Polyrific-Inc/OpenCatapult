// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities
{
    public class FakeDeployProvider : IDeployProvider
    {
        private readonly (string returnValue, string errorMessage) _deployResult;

        public FakeDeployProvider(string returnValue, string errorMessage)
        {
            _deployResult = (returnValue, errorMessage);
        }

        public string Name => nameof(FakeDeployProvider);

        public Task<(string returnValue, string errorMessage)> Deploy(DeployTaskConfig config)
        {
            return Task.FromResult(_deployResult);
        }
    }
}