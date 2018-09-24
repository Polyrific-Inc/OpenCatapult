// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace Polyrific.Catapult.Plugins.Abstraction
{
    public interface IStorageProvider
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
        /// Process to run before executing publish artifact
        /// </summary>
        /// <param name="config">Publish artifact task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> BeforePublishArtifact(PublishArtifactTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Publish artifact to a storage
        /// </summary>
        /// <param name="config">Publish artifact task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns></returns>
        Task<(string storageLocation, Dictionary<string, string> outputValues, string errorMessage)> PublishArtifact(PublishArtifactTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Process to run after executing publish artifact
        /// </summary>
        /// <param name="config">Publish artifact task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> AfterPublishArtifact(PublishArtifactTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);
    }
}
