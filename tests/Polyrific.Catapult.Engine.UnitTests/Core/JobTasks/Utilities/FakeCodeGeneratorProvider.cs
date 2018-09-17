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
        private readonly string _preProcessError = "";
        private readonly string _postProcessError = "";

        /// <summary>
        /// Instantiate fake code generator provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="errorMessage">Fake error message</param>
        public FakeCodeGeneratorProvider(string returnValue, string errorMessage)
        {
            _generateResult = (returnValue, errorMessage);
        }

        /// <summary>
        /// Instantiate fake code generator provider
        /// </summary>
        /// <param name="returnValue">Fake return value</param>
        /// <param name="errorMessage">Fake error message</param>
        /// <param name="preProcessError">Fake pre-process error message</param>
        /// <param name="postProcessError">Fake post-process error message</param>
        public FakeCodeGeneratorProvider(string returnValue, string errorMessage, string preProcessError, string postProcessError)
        {
            _generateResult = (returnValue, errorMessage);
            _preProcessError = preProcessError;
            _postProcessError = postProcessError;
        }

        public string Name => nameof(FakeCodeGeneratorProvider);

        public string[] RequiredServices => new string[0];

        public Task<string> BeforeGenerate(string projectName, List<ProjectDataModelDto> models, string outputFolderName, GenerateTaskConfig config)
        {
            return Task.FromResult(_preProcessError);
        }

        public Task<(string outputLocation, string errorMessage)> Generate(string projectName, List<ProjectDataModelDto> models, string outputFolderName, GenerateTaskConfig config)
        {
            return Task.FromResult(_generateResult);
        }

        public Task<string> AfterGenerate(string projectName, List<ProjectDataModelDto> models, string outputFolderName, GenerateTaskConfig config)
        {
            return Task.FromResult(_postProcessError);
        }
    }
}
