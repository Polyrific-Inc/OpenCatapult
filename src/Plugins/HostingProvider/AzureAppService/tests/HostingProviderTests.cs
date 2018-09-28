// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Xunit;

namespace AzureAppService.Tests
{
    public class HostingProviderTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IAzureUtils> _azureUtils;
        private readonly Mock<IMsDeployUtils> _msDeployUtils;

        public HostingProviderTests()
        {
            _logger = new Mock<ILogger>();
            _azureUtils = new Mock<IAzureUtils>();
            _msDeployUtils = new Mock<IMsDeployUtils>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("dev")]
        public async void Deploy_Success(string slotName)
        {
            var webSite = new Mock<IWebApp>();
            webSite.SetupGet(x => x.DefaultHostName).Returns("https://test.azurewebsites.net");

            var slot = new Mock<IDeploymentSlot>();
            slot.SetupGet(x => x.DefaultHostName).Returns("https://test.azurewebsites.net");

            _azureUtils.Setup(x => x.GetWebsite(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(webSite.Object);
            _azureUtils.Setup(x => x.GetSlot(It.IsAny<IWebApp>(), slotName)).Returns(slot.Object);
            _azureUtils.Setup(x => x.GetPublishingProfile(It.IsAny<IWebAppBase>())).Returns(new Mock<IPublishingProfile>().Object);
            _msDeployUtils.Setup(x => x.ExecuteDeployWebsite(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var artifact = Path.Combine(AppContext.BaseDirectory, "working", "20180817.1");
            
            var taskConfig = new DeployTaskConfig
            {
                ArtifactLocation = artifact,

            };
            var additionalConfigs = new Dictionary<string, string>
            {
                { "ApplicationId", "123" },
                { "ApplicationKey", "xxx" },
                { "TenantId", "123" },
                { "SubscriptionId", "subsid" },
                { "ResourceGroupName", "resourcegroup" },
                { "AppServiceName", "myproject" },
                { "DeploymentSlot", slotName }
            };

            var provider = new HostingProvider(_azureUtils.Object, _msDeployUtils.Object);

            var result = await provider.Deploy(taskConfig, additionalConfigs, _logger.Object);

            Assert.Equal("https://test.azurewebsites.net", result.hostLocation);
            Assert.Equal("", result.errorMessage);
        }
    }
}
