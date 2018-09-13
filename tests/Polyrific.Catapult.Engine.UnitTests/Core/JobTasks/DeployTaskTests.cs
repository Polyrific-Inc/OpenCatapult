// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using System.Collections.Generic;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks
{
    public class DeployTaskTests
    {
        private readonly Mock<ILogger<DeployTask>> _logger;

        public DeployTaskTests()
        {
            _logger = new Mock<ILogger<DeployTask>>();
        }

        [Fact]
        public async void RunMainTask_Success()
        {
            var config = new DeployTaskConfig()
            {
                ProviderName = "FakeDeployProvider"
            };
            var configString = JsonConvert.SerializeObject(config);

            var providers = new List<IDeployProvider>
            {
                new FakeDeployProvider("good-result", "")
            };

            var task = new DeployTask(_logger.Object) {DeployProviders = providers};
            task.SetConfig(configString);

            var result = await task.RunMainTask();

            Assert.True(result.IsSuccess);
            Assert.Equal("good-result", result.ReturnValue);
        }

        [Fact]
        public async void RunMainTask_Failed()
        {
            var config = new DeployTaskConfig()
            {
                ProviderName = "FakeDeployProvider"
            };
            var configString = JsonConvert.SerializeObject(config);

            var providers = new List<IDeployProvider>
            {
                new FakeDeployProvider("", "error-message")
            };

            var task = new DeployTask(_logger.Object) {DeployProviders = providers};
            task.SetConfig(configString);

            var result = await task.RunMainTask();

            Assert.False(result.IsSuccess);
            Assert.Equal("error-message", result.ErrorMessage);
        }

        [Fact]
        public async void RunMainTask_NoProvider()
        {
            var config = new DeployTaskConfig()
            {
                ProviderName = "FakeDeployProvider"
            };
            var configString = JsonConvert.SerializeObject(config);

            var task = new DeployTask(_logger.Object);
            task.SetConfig(configString);

            var result = await task.RunMainTask();

            Assert.False(result.IsSuccess);
            Assert.Equal("Deploy provider \"FakeDeployProvider\" could not be found.", result.ErrorMessage);
        }
    }
}