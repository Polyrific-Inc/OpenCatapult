// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
    public class TaskProviderManagerTests
    {
        private readonly Mock<ICatapultEngineConfig> _engineConfig;
        private readonly Mock<ITaskProviderProcess> _taskProviderProcess;
        private readonly Mock<ILogger<TaskProviderManager>> _logger;
        private readonly Dictionary<string, List<TaskProviderItem>> _taskProviders;

        public TaskProviderManagerTests()
        {
            _engineConfig = new Mock<ICatapultEngineConfig>();
            _logger = new Mock<ILogger<TaskProviderManager>>();

            _taskProviders = new Dictionary<string, List<TaskProviderItem>>
            {
                {"FakeProvider", new List<TaskProviderItem> { new TaskProviderItem("fake-taskProvider", "path/to/fake-taskProvider.dll", new string[]{}) }}
            };

            _taskProviderProcess = new Mock<ITaskProviderProcess>();
        }

        [Fact]
        public void AddTaskProviderLocation_Success()
        {
            var taskProviderManager = new TaskProviderManager(_engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            taskProviderManager.AddTaskProviderLocation("path/to/new/location");

            Assert.Contains("path/to/new/location", taskProviderManager.TaskProviderLocations);
        }

        [Fact]
        public void GetTaskProvider_ReturnsTaskProviderItem()
        {
            var taskProviderManager = new TaskProviderManager(_taskProviders, _engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            var item = taskProviderManager.GetTaskProvider("FakeProvider", "fake-taskProvider");

            Assert.NotNull(item);
            Assert.Equal("fake-taskProvider", item.Name);
        }

        [Fact]
        public void GetTaskProvider_ReturnsNull()
        {
            var taskProviderManager = new TaskProviderManager(_taskProviders, _engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            var item = taskProviderManager.GetTaskProvider("NotExistProvider", "not-exist-taskProvider");

            Assert.Null(item);
        }

        [Fact]
        public void GetTaskProviders_ReturnsTaskProviderItems()
        {
            var taskProviderManager = new TaskProviderManager(_taskProviders, _engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            var items = taskProviderManager.GetTaskProviders("FakeProvider");

            Assert.NotEmpty(items);
        }

        [Fact]
        public void GetTaskProviders_ReturnsEmpty()
        {
            var taskProviderManager = new TaskProviderManager(_taskProviders, _engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            var items = taskProviderManager.GetTaskProviders("NotExistProvider");

            Assert.Empty(items);
        }

        [Fact]
        public async void RefreshTaskProvider_TaskProviderLoaded()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working", "20180817.1");

            if (Directory.Exists(workingLocation))
                Directory.Delete(workingLocation, true);

            Directory.CreateDirectory(workingLocation);

            var taskProviderName = "Polyrific.Catapult.TaskProviders.TestTaskProvider";
            var taskProviderType = "TestTaskProvider";
            var publishLocation = Path.Combine(workingLocation, "publish");
            
            _engineConfig.SetupGet(e => e.TaskProvidersLocation).Returns(publishLocation);

            await GenerateTestTaskProvider(taskProviderName, taskProviderType, workingLocation, publishLocation);
            var taskProviderManager = new TaskProviderManager(_taskProviders, _engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            taskProviderManager.RefreshTaskProviders();

            var newTaskProvider = taskProviderManager.GetTaskProvider(taskProviderType, taskProviderName);
            Assert.NotNull(newTaskProvider);
            Assert.Equal(Path.Combine(publishLocation, $"{taskProviderName}.dll"), newTaskProvider.StartFilePath);
        }

        [SkipOnNonWindowsFact]
        public async void RefreshTaskProvider_Exe_TaskProviderLoaded()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working", "20180817.2");

            if (Directory.Exists(workingLocation))
                Directory.Delete(workingLocation, true);

            Directory.CreateDirectory(workingLocation);

            var taskProviderName = "Polyrific.Catapult.TaskProviders.TestTaskProvider";
            var taskProviderType = "TestTaskProvider";
            var publishLocation = Path.Combine(workingLocation, "publish");

            _engineConfig.SetupGet(e => e.TaskProvidersLocation).Returns(publishLocation);

            await GenerateTestTaskProvider(taskProviderName, taskProviderType, workingLocation, publishLocation, "win-x64");
            var taskProviderManager = new TaskProviderManager(_taskProviders, _engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            taskProviderManager.RefreshTaskProviders();

            var newTaskProvider = taskProviderManager.GetTaskProvider(taskProviderType, taskProviderName);
            Assert.NotNull(newTaskProvider);
            Assert.Equal(Path.Combine(publishLocation, $"{taskProviderName}.exe"), newTaskProvider.StartFilePath);
        }

        [Fact]
        public async void InvokeTaskProvider_Success()
        {
            _taskProviderProcess.Setup(p => p.Start(It.IsAny<ProcessStartInfo>())).Returns((ProcessStartInfo startInfo) => new Process
            {
                StartInfo = startInfo
            });
            _taskProviderProcess.Setup(p => p.GetStandardOutput(It.IsAny<Process>()))
                .Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("[OUTPUT] {\"output\":\"success\"}\n[LOG][Information]Logged"))));
            _taskProviderProcess.Setup(p => p.GetStandardError(It.IsAny<Process>()))
                .Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(""))));

            var taskProviderManager = new TaskProviderManager(_taskProviders, _engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            var result = await taskProviderManager.InvokeTaskProvider("path/to/taskProvider.dll", "taskProvider args");
            Assert.Equal("success", result["output"]);
        }

        [Fact]
        public async void InvokeTaskProvider_Exe_Success()
        {
            _taskProviderProcess.Setup(p => p.Start(It.IsAny<ProcessStartInfo>())).Returns((ProcessStartInfo startInfo) => new Process
            {
                StartInfo = startInfo
            });
            _taskProviderProcess.Setup(p => p.GetStandardOutput(It.IsAny<Process>()))
                .Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("[OUTPUT] {\"output\":\"success\"}\n[LOG][Information]Logged"))));
            _taskProviderProcess.Setup(p => p.GetStandardError(It.IsAny<Process>()))
                .Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(""))));

            var taskProviderManager = new TaskProviderManager(_taskProviders, _engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            var result = await taskProviderManager.InvokeTaskProvider(Path.Combine(AppContext.BaseDirectory, "taskProvider.exe"), "taskProvider args");
            Assert.Equal("success", result["output"]);
        }

        [Fact]
        public async void InvokeTaskProvider_Error()
        {
            _taskProviderProcess.Setup(p => p.Start(It.IsAny<ProcessStartInfo>())).Returns((ProcessStartInfo startInfo) => new Process
            {
                StartInfo = startInfo
            });
            _taskProviderProcess.Setup(p => p.GetStandardOutput(It.IsAny<Process>()))
                .Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(""))));
            _taskProviderProcess.Setup(p => p.GetStandardError(It.IsAny<Process>()))
                .Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("test error"))));

            var taskProviderManager = new TaskProviderManager(_taskProviders, _engineConfig.Object, _taskProviderProcess.Object, _logger.Object);
            var result = await taskProviderManager.InvokeTaskProvider("path/to/taskProvider.dll", "taskProvider args");
            Assert.Equal("test error", result["errorMessage"]);
        }

        #region Private methods
        private async Task GenerateTestTaskProvider(string taskProviderName, string taskProviderType, string outputLocation, string publishLocation, string runtime = null)
        {
            var projectFile = Path.Combine(outputLocation, $"{taskProviderName}.csproj");
            
            await Execute("dotnet", $"new console -n {taskProviderName} -o \"{outputLocation}\"");
            var taskProviderCoreDll = Path.Combine(AppContext.BaseDirectory, "Polyrific.Catapult.TaskProviders.Core.dll");
            AddDllReference(projectFile, taskProviderCoreDll);
            WriteDummyTaskProvider(Path.Combine(outputLocation, "Program.cs"), taskProviderName, taskProviderType);

            var publishArgs = $"publish \"{projectFile}\" --output \"{publishLocation}\" --configuration release";
            if (!string.IsNullOrEmpty(runtime))
                publishArgs += $" --runtime {runtime} --self-contained false";

            await Execute("dotnet", publishArgs);
        }

        private void WriteDummyTaskProvider(string programFile, string taskProviderName, string taskProviderType)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Polyrific.Catapult.TaskProviders.Core;");
            sb.AppendLine("");
            sb.AppendLine($"namespace {taskProviderName}");
            sb.AppendLine("{");
            sb.AppendLine("    public class Program : TaskProvider");
            sb.AppendLine("    {");
            sb.AppendLine($"        private const string TaskProviderName = \"{taskProviderName}\";");
            sb.AppendLine($"        public override string Name => TaskProviderName;");
            sb.AppendLine("");
            sb.AppendLine($"        public override string Type => \"{taskProviderType}\";");
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
            var updatedContent = new StringBuilder();
            using (var reader = new StreamReader(projectFile))
            {
                string line;
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
