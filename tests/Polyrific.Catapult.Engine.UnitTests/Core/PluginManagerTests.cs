﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

        [Fact]
        public async void RefreshPlugin_PluginLoaded()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working", "20180817.1");

            if (Directory.Exists(workingLocation))
                Directory.Delete(workingLocation, true);

            Directory.CreateDirectory(workingLocation);

            var pluginName = "Polyrific.Catapult.Plugins.TestPlugin";
            var pluginType = "TestPlugin";
            var publishLocation = Path.Combine(workingLocation, "publish");
            
            _engineConfig.SetupGet(e => e.PluginsLocation).Returns(publishLocation);

            await GenerateTestPlugin(pluginName, pluginType, workingLocation, publishLocation);
            var pluginManager = new PluginManager(_plugins, _engineConfig.Object, _logger.Object);
            pluginManager.RefreshPlugins();

            var newPlugin = pluginManager.GetPlugin(pluginType, pluginName);
            Assert.NotNull(newPlugin);
            Assert.Equal(Path.Combine(publishLocation, $"{pluginName}.dll"), newPlugin.DllPath);
        }

        #region Private methods
        private async Task GenerateTestPlugin(string pluginName, string pluginType, string outputLocation, string publishLocation)
        {
            var projectFile = Path.Combine(outputLocation, $"{pluginName}.csproj");
            var pluginCoreDll = Path.Combine(AppContext.BaseDirectory, "Polyrific.Catapult.Plugins.Core.dll");
            await Execute("dotnet", $"new console -n {pluginName} -o \"{outputLocation}\"");
            AddDllReference(projectFile, pluginCoreDll);
            WriteDummyPlugin(Path.Combine(outputLocation, "Program.cs"), pluginName, pluginType);

            await Execute("dotnet", $"publish \"{projectFile}\" --output \"{publishLocation}\" --configuration release");
        }


        private void WriteDummyPlugin(string programFile, string pluginName, string pluginType)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Polyrific.Catapult.Plugins.Core;");
            sb.AppendLine("");
            sb.AppendLine($"namespace {pluginName}");
            sb.AppendLine("{");
            sb.AppendLine("    public class Program : TaskProvider");
            sb.AppendLine("    {");
            sb.AppendLine($"        public override string Name => \"{pluginName}\";");
            sb.AppendLine("");
            sb.AppendLine($"        public override string Type => \"{pluginType}\";");
            sb.AppendLine("");
            sb.AppendLine("        public Program() : base(new string[] { })");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        public Program(string[] args) : base(args)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        public override Task<string> Execute()");
            sb.AppendLine("        {");
            sb.AppendLine("            throw new System.NotImplementedException();");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        private static async Task Main(string[] args)");
            sb.AppendLine("        {");
            sb.AppendLine("            var app = new Program(args);");
            sb.AppendLine("");
            sb.AppendLine("            var result = await app.Execute();");
            sb.AppendLine("            app.ReturnOutput(result);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(programFile, sb.ToString());
        }
               
        private void AddDllReference(string projectFile, string dllFile)
        {
            string line = null;
            var updatedContent = new StringBuilder();
            using (var reader = new StreamReader(projectFile))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    switch (line.Trim())
                    {
                        case "</Project>":
                            updatedContent.AppendLine("<ItemGroup>");
                            updatedContent.AppendLine($" <Reference Include=\"{Path.GetFileNameWithoutExtension(dllFile)}\">");
                            updatedContent.AppendLine($"    <HintPath>{dllFile}</HintPath>");
                            updatedContent.AppendLine("  </Reference>");
                            updatedContent.AppendLine("</ItemGroup>");
                            updatedContent.AppendLine(line);
                            break;
                        case "<OutputType>Exe</OutputType>":
                            updatedContent.AppendLine(line);
                            updatedContent.AppendLine("<LangVersion>7.1</LangVersion>");
                            break;
                        default:
                            updatedContent.AppendLine(line);
                            break;
                    }
                }
            }

            using (var writer = new StreamWriter(projectFile))
            {
                writer.Write(updatedContent.ToString());
            }
        }

        private static async Task<(string output, string error)> Execute(string fileName, string args, ILogger logger = null)
        {
            var outputBuilder = new StringBuilder();
            var error = "";

            var info = new ProcessStartInfo(fileName)
            {
                UseShellExecute = false,
                Arguments = args,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(info))
            {
                if (process != null)
                {
                    var reader = process.StandardOutput;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();

                        logger?.LogDebug(line);

                        outputBuilder.AppendLine(line);
                    }

                    error = await process.StandardError.ReadToEndAsync();
                }
            }

            return (outputBuilder.ToString(), error);
        }
        #endregion Private methods
    }
}
