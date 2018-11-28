// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Engine.Core;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core
{
    public class PluginManagerTests
    {
        private readonly Mock<ICatapultEngineConfig> _engineConfig;
        private readonly Mock<ILogger<PluginManager>> _logger;
        private readonly Dictionary<string, List<PluginItem>> _plugins;

        public PluginManagerTests()
        {
            _engineConfig = new Mock<ICatapultEngineConfig>();
            _logger = new Mock<ILogger<PluginManager>>();

            _plugins = new Dictionary<string, List<PluginItem>>
            {
                {"FakeProvider", new List<PluginItem> { new PluginItem("fake-plugin", "path/to/fake-plugin.dll", new string[]{}) }}
            };
        }

        [Fact]
        public void AddPluginLocation_Success()
        {
            var pluginManager = new PluginManager(_engineConfig.Object, _logger.Object);
            pluginManager.AddPluginLocation("path/to/new/location");

            Assert.Contains("path/to/new/location", pluginManager.PluginLocations);
        }

        [Fact]
        public void GetPlugin_ReturnsPluginItem()
        {
            var pluginManager = new PluginManager(_plugins, _engineConfig.Object, _logger.Object);
            var item = pluginManager.GetPlugin("FakeProvider", "fake-plugin");

            Assert.NotNull(item);
            Assert.Equal("fake-plugin", item.Name);
        }

        [Fact]
        public void GetPlugin_ReturnsNull()
        {
            var pluginManager = new PluginManager(_plugins, _engineConfig.Object, _logger.Object);
            var item = pluginManager.GetPlugin("NotExistProvider", "not-exist-plugin");

            Assert.Null(item);
        }

        [Fact]
        public void GetPlugins_ReturnsPluginItems()
        {
            var pluginManager = new PluginManager(_plugins, _engineConfig.Object, _logger.Object);
            var items = pluginManager.GetPlugins("FakeProvider");

            Assert.NotEmpty(items);
        }

        [Fact]
        public void GetPlugins_ReturnsEmpty()
        {
            var pluginManager = new PluginManager(_plugins, _engineConfig.Object, _logger.Object);
            var items = pluginManager.GetPlugins("NotExistProvider");

            Assert.Empty(items);
        }
    }
}
