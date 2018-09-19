// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace Polyrific.Catapult.Plugins.Abstraction
{
    public interface IBuildProvider
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
        /// Process to run before executing build
        /// </summary>
        /// <param name="config">Build task confiuration</param>
        /// <returns>Error message</returns>
        Task<string> BeforeBuild(BuildTaskConfig config);

        /// <summary>
        /// Build the code, and produce a ready to deploy artifact
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="jobQueueCode">Job queue code</param>
        /// <param name="config">Build task configuration</param>
        /// <returns></returns>
        Task<(string returnValue, string errorMessage)> Build(string projectName, string jobQueueCode, BuildTaskConfig config);

        /// <summary>
        /// Process to run after executing build
        /// </summary>
        /// <param name="config">Build task configuration</param>
        /// <returns>Error message</returns>
        Task<string> AfterBuild(BuildTaskConfig config);
    }
}
