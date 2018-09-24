// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace Polyrific.Catapult.Plugins.Abstraction
{
    public interface ITestProvider
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
        /// Process to run before executing test
        /// </summary>
        /// <param name="config">Test task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> BeforeTest(TestTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Run test scenario
        /// </summary>
        /// <param name="config">Test task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns></returns>
        Task<(string testResultLocation, Dictionary<string, string> outputValues, string errorMessage)> Test(TestTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);

        /// <summary>
        /// Process to run after executing test
        /// </summary>
        /// <param name="config">Test task configuration</param>
        /// <param name="additionalConfigs">Additional configurations for specific provider</param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        /// <returns>Error message</returns>
        Task<string> AfterTest(TestTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger);
    }
}
