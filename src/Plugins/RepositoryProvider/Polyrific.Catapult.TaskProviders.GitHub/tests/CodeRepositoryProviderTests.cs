using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Xunit;

namespace Polyrific.Catapult.TaskProviders.GitHub.UnitTests
{
    public class CodeRepositoryProviderTests
    {
        private readonly Mock<IGitHubUtils> _gitHubUtils;

        public CodeRepositoryProviderTests()
        {
            _gitHubUtils = new Mock<IGitHubUtils>();
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
                Repository = "https://github.com/polyrific-inc/opencatapult"
            };
            var additionalConfigs = new Dictionary<string, string>();

            var provider = new Program(new string[] { GetCloneArgString("main", "TestProject", taskConfig, additionalConfigs) }, _gitHubUtils.Object);

            var result = await provider.Clone();

            Assert.Equal(cloneLocation, result.cloneLocation);
            Assert.Equal("", result.errorMessage);
            Assert.True(Directory.Exists(cloneLocation));
        }

        [Fact]
        public async void Push_Success()
        {
            var remoteUrl = "https://github.com/polyrific-inc/opencatapult";

            _gitHubUtils.Setup(u => u.Push(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _gitHubUtils.Setup(u => u.Commit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _gitHubUtils.Setup(u => u.CreatePullRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("100");

            var taskConfig = new PushTaskConfig
            {
                Repository = remoteUrl,
                CreatePullRequest = true
            };

            var provider = new Program(new string[] { GetPushArgString("main", "TestProject", taskConfig, new Dictionary<string, string>()) }, _gitHubUtils.Object);

            var result = await provider.Push();

            Assert.Equal(remoteUrl, result.remoteUrl);
            Assert.Equal("https://github.com/polyrific-inc/opencatapult/pull/100", result.pullRequestUrl);
            Assert.Equal("100", result.outputValues["PRNumber"]);
            Assert.Equal("", result.errorMessage);
        }

        [Fact]
        public async void Merge_Success()
        {
            var remoteUrl = "https://github.com/polyrific-inc/opencatapult";

            _gitHubUtils.Setup(u => u.MergePullRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var taskConfig = new MergeTaskConfig
            {
                Repository = remoteUrl
            };

            var provider = new Program(new string[] { GetMergeArgString("main", "TestProject", taskConfig, new Dictionary<string, string>(), "100") }, _gitHubUtils.Object);

            var result = await provider.Merge();

            Assert.Equal(remoteUrl, result.remoteUrl);
            Assert.Equal("", result.errorMessage);
        }

        [Fact]
        public async void DeleteRepository_Success()
        {
            var remoteUrl = "https://github.com/polyrific-inc/opencatapult";
            
            var taskConfig = new DeleteRepositoryTaskConfig
            {
                Repository = remoteUrl
            };

            var provider = new Program(new string[] { GetDeleteArgString("TestProject", taskConfig, new Dictionary<string, string>(), "100") }, _gitHubUtils.Object);

            var errorMessage = await provider.DeleteRepository();

            _gitHubUtils.Verify(s => s.DeleteRepository("opencatapult", "polyrific-inc"), Times.Once);

            Assert.Equal("", errorMessage);
        }

        private string GetCloneArgString(string process, string projectName, CloneTaskConfig taskConfig, Dictionary<string, string> additionalConfigs)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"project", projectName},
                {"cloneconfig", taskConfig},
                {"additional", additionalConfigs}
            };

            return JsonConvert.SerializeObject(dict);
        }

        private string GetPushArgString(string process, string projectName, PushTaskConfig taskConfig, Dictionary<string, string> additionalConfigs)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"project", projectName},
                {"pushconfig", taskConfig},
                {"additional", additionalConfigs}
            };

            return JsonConvert.SerializeObject(dict);
        }

        private string GetMergeArgString(string process, string projectName, MergeTaskConfig taskConfig, Dictionary<string, string> additionalConfigs, string prNumber)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"project", projectName},
                {"mergeconfig", taskConfig},
                {"additional", additionalConfigs},
                {"prnumber", prNumber }
            };

            return JsonConvert.SerializeObject(dict);
        }

        private string GetDeleteArgString(string projectName, DeleteRepositoryTaskConfig taskConfig, Dictionary<string, string> additionalConfigs, string prNumber)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", "delete"},
                {"project", projectName},
                {"deleteconfig", taskConfig},
                {"additional", additionalConfigs}
            };

            return JsonConvert.SerializeObject(dict);
        }
    }
}
