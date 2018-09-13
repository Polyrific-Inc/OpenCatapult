// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities
{
    public class FakeCodeRepositoryProvider : ICodeRepositoryProvider
    {
        private readonly (string returnValue, string errorMessage) _actionResult;

        public FakeCodeRepositoryProvider(string returnValue, string errorMessage)
        {
            _actionResult = (returnValue, errorMessage);
        }

        public string Name => nameof(FakeCodeRepositoryProvider);

        public Task<(string returnValue, string errorMessage)> Clone()
        {
            return Task.FromResult(_actionResult);
        }

        public Task<(string returnValue, string errorMessage)> Push(PushTaskConfig config)
        {
            return Task.FromResult(_actionResult);
        }
    }
}