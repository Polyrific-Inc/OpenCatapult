// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

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

        public string[] RequiredServices => new string[0];

        public Task<string> BeforeClone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string returnValue, string errorMessage)> Clone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_actionResult);
        }

        public Task<string> AfterClone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_postProcessError);
        }

        public Task<string> BeforePush(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string returnValue, string errorMessage)> Push(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_actionResult);
        }

        public Task<string> AfterPush(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_postProcessError);
        }

        public Task<string> BeforeMerge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string returnValue, string errorMessage)> Merge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_actionResult);
        }

        public Task<string> AfterMerge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_postProcessError);
        }
    }
}
