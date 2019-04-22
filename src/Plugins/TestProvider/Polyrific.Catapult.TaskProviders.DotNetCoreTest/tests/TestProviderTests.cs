using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Polyrific.Catapult.TaskProviders.DotNetCoreTest.Helpers;
using Xunit;

namespace Polyrific.Catapult.TaskProviders.DotNetCoreTest.UnitTests
{
    public class TestProviderTests
    {
        [Fact]
        public async void Test_Success()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working", "20180817.1");

            if (Directory.Exists(workingLocation))
                Directory.Delete(workingLocation, true);

            Directory.CreateDirectory(workingLocation);

            await GenerateSolutionWithTest("MyProject", Path.Combine(workingLocation, "src"));

            var taskConfig = new TestTaskConfig
            {
                TestLocation = "src",
                WorkingLocation = workingLocation
            };

            var provider = new Program(new string[] { GetArgString("main", taskConfig, new Dictionary<string, string>()) });

            var result = await provider.Test();

            Assert.Equal("", result.errorMessage);

            // TODO: re-enable this assert when the issue (https://github.com/Microsoft/vstest/issues/1951) is solved
            //Assert.True(File.Exists(result.testResultLocation));
        }

        [Fact]
        public async void Test_Failed()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working", "20180817.1");

            if (Directory.Exists(workingLocation))
                Directory.Delete(workingLocation, true);

            Directory.CreateDirectory(workingLocation);

            await GenerateSolutionWithTest("MyProject", Path.Combine(workingLocation, "src"), true);

            var taskConfig = new TestTaskConfig
            {
                TestLocation = "src",
                WorkingLocation = workingLocation
            };

            var provider = new Program(new string[] { GetArgString("main", taskConfig, new Dictionary<string, string>()) });

            var result = await provider.Test();

            Assert.Equal("Test failed", result.errorMessage);
            Assert.Empty(result.testResultLocation);
        }

        private async Task GenerateSolutionWithTest(string projectName, string outputLocation, bool makeFailed = false)
        {
            await CommandHelper.Execute("dotnet", $"new sln -n {projectName} -o \"{outputLocation}\"");
            await CommandHelper.Execute("dotnet", $"new xunit -n {projectName}.Test -o \"{Path.Combine(outputLocation, $"{projectName}.Test")}\"");
            await CommandHelper.Execute("dotnet", $"sln \"{Path.Combine(outputLocation, $"{projectName}.sln")}\" add \"{Path.Combine(outputLocation, $"{projectName}.Test", $"{projectName}.Test.csproj")}\"");

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

        private string GetArgString(string process, TestTaskConfig taskConfig, Dictionary<string, string> additionalConfigs)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"config", taskConfig},
                {"additional", additionalConfigs}
            };

            return JsonConvert.SerializeObject(dict);
        }
    }
}
