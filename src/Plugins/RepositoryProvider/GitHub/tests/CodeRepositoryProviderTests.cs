// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Xunit;

namespace GitHub.Tests
{
    public class CodeRepositoryProviderTests
    {
        private readonly Mock<IGitHubUtils> _gitHubUtils;
        private readonly Mock<ILogger> _logger;
        
        public CodeRepositoryProviderTests()
        { 
            _gitHubUtils = new Mock<IGitHubUtils>();
            _logger = new Mock<ILogger>();
        }

        [Fact]
        public async void Clone_Success()
        {
            var cloneLocation = Path.Combine(AppContext.BaseDirectory, "clone");

            _gitHubUtils.Setup(u => u.Clone(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(cloneLocation);
            
            var taskConfig = new CloneTaskConfig
            {
                CloneLocation = cloneLocation,
                Repository = "https//github.com/polyrific-inc/opencatapult"
            };
            var additionalConfigs = new Dictionary<string, string>();
            
            var provider = new CodeRepositoryProvider(_gitHubUtils.Object);

            var result = await provider.Clone(taskConfig, additionalConfigs, _logger.Object);

            Assert.Equal(cloneLocation, result.cloneLocation);
            Assert.Equal("", result.errorMessage);
            Assert.True(Directory.Exists(cloneLocation));
        }

        [Fact]
        public async void Push_Success()
        {
            var remoteUrl = "https//github.com/polyrific-inc/opencatapult";

            _gitHubUtils.Setup(u => u.Push(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _gitHubUtils.Setup(u => u.CreatePullRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("100");

            var taskConfig = new PushTaskConfig
            {
                Repository = remoteUrl,
                CreatePullRequest = true
            };

            var provider = new CodeRepositoryProvider(_gitHubUtils.Object);
            
            var result = await provider.Push(taskConfig, null, _logger.Object);

            Assert.Equal(remoteUrl, result.remoteUrl);
            Assert.Equal("100", result.outputValues["PRNumber"]);
            Assert.Equal("", result.errorMessage);
        }

        [Fact]
        public async void Merge_Success()
        {
            var remoteUrl = "https//github.com/polyrific-inc/opencatapult";

            _gitHubUtils.Setup(u => u.MergePullRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var taskConfig = new MergeTaskConfig
            {
                Repository = remoteUrl
            };

            var provider = new CodeRepositoryProvider(_gitHubUtils.Object);

            var result = await provider.Merge("100", taskConfig, null, _logger.Object);

            Assert.Equal(remoteUrl, result.remoteUrl);
            Assert.Equal("", result.errorMessage);
        }
    }
}
