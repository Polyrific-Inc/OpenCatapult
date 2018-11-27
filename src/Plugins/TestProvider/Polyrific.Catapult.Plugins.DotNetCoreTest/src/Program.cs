﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Core;

namespace Polyrific.Catapult.Plugins.DotNetCoreTest
{
    internal class Program : TestProvider
    {
        private ITestRunner _testRunner;

        public Program() : base(new string[0])
        {
        }

        public Program(string[] args) : base(args)
        {
        }

        public override string Name => "Polyrific.Catapult.Plugins.DotNetCoreTest";
        
        public override async Task<(string testResultLocation, Dictionary<string, string> outputValues, string errorMessage)> Test()
        {
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
