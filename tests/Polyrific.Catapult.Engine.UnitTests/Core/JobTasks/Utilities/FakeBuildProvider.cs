// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities
{
    public class FakeBuildProvider : IBuildProvider
    {
        private readonly (string returnValue, string errorMessage) _buildResult;
        private readonly string _preProcessError = "";
        private readonly string _postProcessError = "";

        /// <summary>
        /// Instantiate fake build provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="errorMessage">Fake error message</param>
        public FakeBuildProvider(string returnValue, string errorMessage)
        {
            _buildResult = (returnValue, errorMessage);
        }

        /// <summary>
        /// Instantiate fake build provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="errorMessage">Fake error message</param>
        /// <param name="preProcessError">Fake pre-process error message</param>
        /// <param name="postProcessError">Fake post-process error message</param>
        public FakeBuildProvider(string returnValue, string errorMessage, string preProcessError, string postProcessError)
        {
            _buildResult = (returnValue, errorMessage);
            _preProcessError = preProcessError;
            _postProcessError = postProcessError;
        }

        public string Name => nameof(FakeBuildProvider);

        public Task<string> BeforeBuild(BuildTaskConfig config)
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string returnValue, string errorMessage)> Build(BuildTaskConfig config)
        {
            return Task.FromResult(_buildResult);
        }

        public Task<string> AfterBuild(BuildTaskConfig config)
        {
            return Task.FromResult(_postProcessError);
        }
    }
}
