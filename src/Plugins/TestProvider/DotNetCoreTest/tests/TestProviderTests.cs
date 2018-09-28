// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using System.Text;
using System.Threading.Tasks;
using DotNetCoreTest.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Xunit;

namespace DotNetCoreTest.Tests
{
    public class TestProviderTests
    {
        private readonly Mock<ILogger> _logger;

        public TestProviderTests()
        {
            _logger = new Mock<ILogger>();
        }

        [Fact]
        public async void Test_Success()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working", "20180817.1");

            if (Directory.Exists(workingLocation))
                Directory.Delete(workingLocation, true);

            Directory.CreateDirectory(workingLocation);

            await GenerateSolutionWithTest("MyProject", Path.Combine(workingLocation, "src"));

            var provider = new TestProvider();

            var taskConfig = new TestTaskConfig
            {
                TestLocation = "src",
                WorkingLocation = workingLocation
            };

            var result = await provider.Test(taskConfig, null, _logger.Object);

            Assert.Equal("", result.errorMessage);
            Assert.True(File.Exists(result.testResultLocation));
        }

        [Fact]
        public async void Test_Failed()
        {
            var workingLocation = Path.Combine("c:\\opencatapult\\working", "20180817.1");

            if (Directory.Exists(workingLocation))
                Directory.Delete(workingLocation, true);

            Directory.CreateDirectory(workingLocation);

            await GenerateSolutionWithTest("MyProject", Path.Combine(workingLocation, "src"), true);

            var provider = new TestProvider();

            var taskConfig = new TestTaskConfig
            {
                TestLocation = "src",
                WorkingLocation = workingLocation
            };

            var result = await provider.Test(taskConfig, null, _logger.Object);

            Assert.Equal("Test failed", result.errorMessage);
            Assert.Empty(result.testResultLocation);
        }

        private async Task GenerateSolutionWithTest(string projectName, string outputLocation, bool makeFailed = false)
        {
            await CommandHelper.Execute("dotnet", $"new sln -n {projectName} -o {outputLocation}");
            await CommandHelper.Execute("dotnet", $"new xunit -n {projectName}.Test -o {Path.Combine(outputLocation, $"{projectName}.Test")}");
            await CommandHelper.Execute("dotnet", $"sln {Path.Combine(outputLocation,  $"{projectName}.sln")} add {Path.Combine(outputLocation, $"{projectName}.Test", $"{projectName}.Test.csproj")}");

            if (makeFailed)
                MakeTestFailed(Path.Combine(outputLocation, $"{projectName}.Test", "UnitTest1.cs"));
        }

        private void MakeTestFailed(string testFile)
        {
            string line = null;
            bool isTestMethod = false;
            var updatedContent = new StringBuilder();
            using (var reader = new StreamReader(testFile))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    switch (line.Trim())
                    {
                        case "public void Test1()":
                            isTestMethod = true;
                            updatedContent.AppendLine(line);
                            break;
                        case "}":
                            isTestMethod = false;
                            updatedContent.AppendLine("         Assert.True(false);");
                            updatedContent.AppendLine(line);
                            break;
                        default:
                            if (!isTestMethod)
                                updatedContent.AppendLine(line);
                            break;
                    }
                }
            }

            using (var writer = new StreamWriter(testFile))
            {
                writer.Write(updatedContent.ToString());
            }
        }
    }
}
