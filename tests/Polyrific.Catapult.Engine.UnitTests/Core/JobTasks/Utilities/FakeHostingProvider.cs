// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities
{
    public class FakeHostingProvider : IHostingProvider
    {
        private readonly (string returnValue, string errorMessage) _deployResult;
        private readonly string _preProcessError = "";
        private readonly string _postProcessError = "";

        /// <summary>
        /// Instantiate fake deploy provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="errorMessage">Fake error message</param>
        public FakeHostingProvider(string returnValue, string errorMessage)
        {
            _deployResult = (returnValue, errorMessage);
        }

        /// <summary>
        /// Instantiate fake deploy provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="errorMessage">Fake error message</param>
        /// <param name="preProcessError">Fake pre-process error message</param>
        /// <param name="postProcessError">Fake post-process error message</param>
        public FakeHostingProvider(string returnValue, string errorMessage, string preProcessError, string postProcessError)
        {
            _deployResult = (returnValue, errorMessage);
            _preProcessError = preProcessError;
            _postProcessError = postProcessError;
        }

        public string Name => nameof(FakeHostingProvider);

        public string[] RequiredServices => new string[0];

        public Task<string> BeforeDeploy(DeployTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string returnValue, string errorMessage)> Deploy(DeployTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_deployResult);
        }

        public Task<string> AfterDeploy(DeployTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_postProcessError);
        }
    }
}
