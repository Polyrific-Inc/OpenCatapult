// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities
{
    public class FakeBuildProvider : IBuildProvider
    {
        private readonly (string outputLocation, Dictionary<string, string> outputValues, string errorMessage) _buildResult;
        private readonly string _preProcessError = "";
        private readonly string _postProcessError = "";

        /// <summary>
        /// Instantiate fake build provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="outputValues">Fake output values</param>
        /// <param name="errorMessage">Fake error message</param>
        public FakeBuildProvider(string returnValue, Dictionary<string, string> outputValues, string errorMessage)
        {
            _buildResult = (returnValue, outputValues, errorMessage);
        }

        /// <summary>
        /// Instantiate fake build provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="outputValues">Fake output values</param>
        /// <param name="errorMessage">Fake error message</param>
        /// <param name="preProcessError">Fake pre-process error message</param>
        /// <param name="postProcessError">Fake post-process error message</param>
        public FakeBuildProvider(string returnValue, Dictionary<string, string> outputValues, string errorMessage, string preProcessError, string postProcessError)
        {
            _buildResult = (returnValue, outputValues, errorMessage);
            _preProcessError = preProcessError;
            _postProcessError = postProcessError;
        }

        public string Name => nameof(FakeBuildProvider);

        public string[] RequiredServices => new string[0];

        public Task<string> BeforeBuild(string projectName, BuildTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string outputArtifact, Dictionary<string, string> outputValues, string errorMessage)> Build(string projectName, BuildTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_buildResult);
        }

        public Task<string> AfterBuild(string projectName, BuildTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_postProcessError);
        }
    }
}
