// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace DotNetCoreTest
{
    [Export(typeof(ITestProvider))]
    public class TestProvider : ITestProvider
    {
        private ITestRunner _testRunner;

        public TestProvider()
        {

        }

        public TestProvider(ITestRunner testRunner)
        {
            _testRunner = testRunner;
        }

        public string Name => "DotNetCoreTestProvider";

        public string[] RequiredServices => new string[0];

        public Task<string> BeforeTest(TestTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string testResultLocation, Dictionary<string, string> outputValues, string errorMessage)> Test(TestTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            var testLocation = config.TestLocation ?? string.Empty; ;
            if (!Path.IsPathRooted(testLocation))
                testLocation = Path.Combine(config.WorkingLocation, testLocation);
            
            var testOutputLocation = Path.Combine(config.WorkingLocation, "tests");
            
            if (_testRunner == null)
                _testRunner = new TestRunner(logger);

            var result = await _testRunner.RunTest(testLocation, testOutputLocation, config.ContinueWhenFailed);
            if (!string.IsNullOrEmpty(result.error))
                return ("", null, result.error);
            
            return (result.resultFilePath, null, "");
        }

        public Task<string> AfterTest(TestTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }
    }
}
