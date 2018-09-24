// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities
{
    public class FakeCodeGeneratorProvider : ICodeGeneratorProvider
    {
        private readonly (string outputLocation, Dictionary<string, string> outputValues, string errorMessage) _generateResult;
        private readonly string _preProcessError = "";
        private readonly string _postProcessError = "";

        /// <summary>
        /// Instantiate fake code generator provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="outputValues">Fake output values</param>
        /// <param name="errorMessage">Fake error message</param>
        public FakeCodeGeneratorProvider(string returnValue, Dictionary<string, string> outputValues, string errorMessage)
        {
            _generateResult = (returnValue, outputValues, errorMessage);
        }

        /// <summary>
        /// Instantiate fake code generator provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="outputValues">Fake output values</param>
        /// <param name="errorMessage">Fake error message</param>
        /// <param name="preProcessError">Fake pre-process error message</param>
        /// <param name="postProcessError">Fake post-process error message</param>
        public FakeCodeGeneratorProvider(string returnValue, Dictionary<string, string> outputValues, string errorMessage, string preProcessError, string postProcessError)
        {
            _generateResult = (returnValue, outputValues, errorMessage);
            _preProcessError = preProcessError;
            _postProcessError = postProcessError;
        }

        public string Name => nameof(FakeCodeGeneratorProvider);

        public string[] RequiredServices => new string[0];

        public Task<string> BeforeGenerate(string projectName, List<ProjectDataModelDto> models, GenerateTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate(string projectName, List<ProjectDataModelDto> models, GenerateTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_generateResult);
        }

        public Task<string> AfterGenerate(string projectName, List<ProjectDataModelDto> models, GenerateTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult(_postProcessError);
        }
    }
}
