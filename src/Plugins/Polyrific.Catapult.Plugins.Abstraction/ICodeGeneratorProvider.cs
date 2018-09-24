// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.Plugins.Abstraction
{
    public interface ICodeGeneratorProvider
    {
        /// <summary>
        /// Name of the provider
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Required service connections
        /// </summary>
        string[] RequiredServices { get; }

        /// <summary>
        /// Process to run before executing the code generation
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="models">Project data models</param>
        /// <param name="config">Generate task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> BeforeGenerate(string projectName, List<ProjectDataModelDto> models, GenerateTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Generate code from data models
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="models">Project data models</param>
        /// <param name="config">Generate task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns></returns>
        Task<(string outputLocation, string errorMessage)> Generate(string projectName, List<ProjectDataModelDto> models, GenerateTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Process to run after executing code generation
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="models">Project data models</param>
        /// <param name="config">Generate task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> AfterGenerate(string projectName, List<ProjectDataModelDto> models, GenerateTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);
    }
}
