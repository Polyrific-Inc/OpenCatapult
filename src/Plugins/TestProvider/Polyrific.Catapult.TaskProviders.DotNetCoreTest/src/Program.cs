// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Polyrific.Catapult.TaskProviders.Core;
using Polyrific.Catapult.TaskProviders.DotNetCoreTest.Helpers;

namespace Polyrific.Catapult.TaskProviders.DotNetCoreTest
{
    public class Program : TestProvider
    {
        private const string TaskProviderName = "Polyrific.Catapult.TaskProviders.DotNetCoreTest";

        private ITestRunner _testRunner;

        public Program(string[] args) : base(args)
        {
        }

        public override string Name => TaskProviderName;
        
        public override async Task<(string testResultLocation, Dictionary<string, string> outputValues, string errorMessage)> Test()
        {
            var (outputVersion, err, exitCode) = await CommandHelper.Execute("dotnet", "--list-sdks");
            if (string.IsNullOrWhiteSpace(outputVersion) || !string.IsNullOrEmpty(err))
            {
                return (null, null, $"Task Provider {TaskProviderName} require dotnet sdk to be installed");
            }

            var testLocation = Config.TestLocation ?? string.Empty;
            if (!Path.IsPathRooted(testLocation))
                testLocation = Path.Combine(Config.WorkingLocation, testLocation);
            
            var testOutputLocation = Path.Combine(Config.WorkingLocation, "tests");
            
            if (_testRunner == null)
                _testRunner = new TestRunner(Logger);

            var result = await _testRunner.RunTest(testLocation, testOutputLocation, Config.ContinueWhenFailed);
            if (!string.IsNullOrEmpty(result.error))
                return ("", null, result.error);
            
            return (result.resultFilePath, null, "");
        }

        private static async Task Main(string[] args)
        {
            var app = new Program(args);
            
            var result = await app.Execute();
            app.ReturnOutput(result);
        }
    }
}
