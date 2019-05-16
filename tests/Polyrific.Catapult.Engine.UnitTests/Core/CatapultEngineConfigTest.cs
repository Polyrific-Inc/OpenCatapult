// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core
{
    public class CatapultEngineConfigTest
    {
        private const string EngineConfigFile = "engineconfig.json";
        private readonly Dictionary<string, string> _configs;
        private readonly Mock<ILogger<CatapultEngineConfig>> _logger;

        public CatapultEngineConfigTest()
        {
            _configs = new Dictionary<string, string>
            {
                {"ApiUrl", "https://localhost/test"},
                {"ApiRequestTimeout", "01:00:00"},
                {"AuthorizationToken", "abcdefghijklmnopqrstuvwxyz"},
                {"JobCheckingInterval", "5"}
            };

            _logger = new Mock<ILogger<CatapultEngineConfig>>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void Load_ConfigFile(bool configFileExists)
        {
            if (configFileExists)
            {
                await File.WriteAllTextAsync(EngineConfigFile, JsonConvert.SerializeObject(new { EngineConfig = _configs }));
            }
            else
            {
                if (File.Exists(EngineConfigFile))
                    File.Delete(EngineConfigFile);
            }

            var config = new CatapultEngineConfig(_logger.Object);
            await config.Load();

            if (configFileExists)
                Assert.Equal(_configs["ApiUrl"], config.ApiUrl);
            else
                Assert.Equal("https://localhost:44305", config.ApiUrl);
        }

        [Fact]
        public async void Save_ToConfigFile()
        {
            if (File.Exists(EngineConfigFile))
                File.Delete(EngineConfigFile);

            var config = new CatapultEngineConfig(_logger.Object);

            config.Configs["ApiUrl"] = "https://localhost/test";
            await config.Save();

            var obj = JObject.Parse(await File.ReadAllTextAsync(EngineConfigFile));
            var loadedConfigs = obj["EngineConfig"].ToObject<Dictionary<string, string>>();

            Assert.Equal("https://localhost/test", loadedConfigs["ApiUrl"]);
        }

        [Theory]
        [InlineData("ApiUrl")]
        [InlineData("UnrecognizedConfig")]
        public void GetValue_WithoutDefault(string configName)
        {
            if (File.Exists(EngineConfigFile))
                File.Delete(EngineConfigFile);

            var config = new CatapultEngineConfig(_logger.Object);

            if (configName == "ApiUrl")
            {
                var value = config.GetValue(configName);
                Assert.Equal("https://localhost:44305", value);
            }
            else
            {
                var exception = Record.Exception(() => config.GetValue(configName));
                Assert.IsType<InvalidEngineConfigNameException>(exception);
            }
        }

        [Theory]
        [InlineData("ApiUrl")]
        [InlineData("UnrecognizedConfig")]
        public void GetValue_WithDefault(string configName)
        {
            if (File.Exists(EngineConfigFile))
                File.Delete(EngineConfigFile);

            var config = new CatapultEngineConfig(_logger.Object);

            var value = config.GetValueOrDefault(configName, "DefaultValue");

            if (configName == "ApiUrl")
                Assert.Equal("https://localhost:44305", value);
            else
                Assert.Equal("DefaultValue", value);
        }

        [Theory]
        [InlineData("ApiUrl", "https://localhost/test")]
        [InlineData("UnrecognizedConfig", "Unrecognized config value")]
        public void SetValue(string configName, string configValue)
        {
            if (File.Exists(EngineConfigFile))
                File.Delete(EngineConfigFile);

            var config = new CatapultEngineConfig(_logger.Object);
            config.SetValue(configName, configValue);

            Assert.Equal(configValue, config.Configs[configName]);
        }

        [Theory]
        [InlineData("ApiUrl")]
        [InlineData("UnrecognizedConfig")]
        public void RemoveValue(string configName)
        {
            if (File.Exists(EngineConfigFile))
                File.Delete(EngineConfigFile);

            var config = new CatapultEngineConfig(_logger.Object);
            config.RemoveValue(configName);

            if (configName == "ApiUrl")
            {
                Assert.Equal("https://localhost:44305", config.ApiUrl);
            }
            else
            {
                Assert.False(config.Configs.ContainsKey(configName));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void InitConfigFile(bool reset)
        {
            await File.WriteAllTextAsync(EngineConfigFile, JsonConvert.SerializeObject(new { EngineConfig = _configs }));
            
            await CatapultEngineConfig.InitConfigFile(reset, _logger.Object);
            
            var expected = reset ? "https://localhost:44305" : "https://localhost/test";
            var obj = JObject.Parse(await File.ReadAllTextAsync(EngineConfigFile));
            var loadedConfigs = obj["EngineConfig"].ToObject<Dictionary<string, string>>();
            
            Assert.Equal(expected, loadedConfigs["ApiUrl"]);
        }

        [Fact]
        public void TestPropertyValues()
        {
            if (File.Exists(EngineConfigFile))
                File.Delete(EngineConfigFile);

            var config = new CatapultEngineConfig(_logger.Object);

            Assert.Equal("https://localhost:44305", config.ApiUrl);
            Assert.Equal(TimeSpan.Parse("00:01:00"), config.ApiRequestTimeout);
            Assert.Equal("", config.AuthorizationToken);
            Assert.Equal(30, config.JobCheckingInterval);
        }
    }
}
