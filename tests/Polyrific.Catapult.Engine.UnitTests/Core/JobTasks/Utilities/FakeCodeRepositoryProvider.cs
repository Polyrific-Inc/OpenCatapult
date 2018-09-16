// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities
{
    public class FakeCodeRepositoryProvider : ICodeRepositoryProvider
    {
        private readonly (string returnValue, string errorMessage) _actionResult;
        private readonly string _preProcessError = "";
        private readonly string _postProcessError = "";

        /// <summary>
        /// Instantiate fake code repository provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="errorMessage">Fake error message</param>
        public FakeCodeRepositoryProvider(string returnValue, string errorMessage)
        {
            _actionResult = (returnValue, errorMessage);
        }

        /// <summary>
        /// Instantiate fake code repository provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="errorMessage">Fake error message</param>
        /// <param name="preProcessError">Fake pre-process error message</param>
        /// <param name="postProcessError">Fake post-process error message</param>
        public FakeCodeRepositoryProvider(string returnValue, string errorMessage, string preProcessError, string postProcessError)
        {
            _actionResult = (returnValue, errorMessage);
            _preProcessError = preProcessError;
            _postProcessError = postProcessError;
        }

        public string Name => nameof(FakeCodeRepositoryProvider);

        public Task<string> BeforeClone()
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string returnValue, string errorMessage)> Clone()
        {
            return Task.FromResult(_actionResult);
        }

        public Task<string> AfterClone()
        {
            return Task.FromResult(_postProcessError);
        }

        public Task<string> BeforePush(PushTaskConfig config)
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string returnValue, string errorMessage)> Push(PushTaskConfig config)
        {
            return Task.FromResult(_actionResult);
        }

        public Task<string> AfterPush(PushTaskConfig config)
        {
            return Task.FromResult(_postProcessError);
        }
    }
}
