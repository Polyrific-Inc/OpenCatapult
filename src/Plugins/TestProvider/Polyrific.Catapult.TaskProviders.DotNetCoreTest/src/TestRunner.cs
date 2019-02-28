// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.TaskProviders.DotNetCoreTest.Helpers;

namespace Polyrific.Catapult.TaskProviders.DotNetCoreTest
{
    public class TestRunner : ITestRunner
    {
        private const int FailedExitCode = 1;
        private const string TestResultFile = "testresult.trx";

        private ILogger _logger;

        public TestRunner(ILogger logger)
        {
            _logger = logger;
        }
        
        public async Task<(string error, string resultFilePath)> RunTest(string projectPath, string resultPath, bool continueWhenFailed)
        {
            var args = $"test \"{projectPath}\" --logger trx;LogFileName={TestResultFile} --results-directory \"{resultPath}\"";

            var result = await RunDotnet(args);

            if (!string.IsNullOrEmpty(result.error))
                return (result.error, null);

            if (result.exitCode == FailedExitCode && !continueWhenFailed)
                return ("Test failed", null);

            return ("", Path.Combine(resultPath, TestResultFile));
        }

        private async Task<(string error, int exitCode)> RunDotnet(string args)
        {
            var result = await CommandHelper.Execute("dotnet", args, _logger);

            return (result.error, result.exitCode);
        }
    }
}
