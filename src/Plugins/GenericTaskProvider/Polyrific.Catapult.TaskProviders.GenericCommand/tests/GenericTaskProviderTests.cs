// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Xunit;

namespace Polyrific.Catapult.TaskProviders.GenericCommand.UnitTests
{
    public class GenericTaskProviderTests
    {
        [Fact]
        public async void GenericExecution_Success()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working");

            var config = new BaseJobTaskConfig
            {
                WorkingLocation = workingLocation
            };

            if (Directory.Exists(workingLocation))
                Directory.Delete(workingLocation, true);

            Directory.CreateDirectory(workingLocation);

            var additionalConfig = new Dictionary<string, string>();

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            var sb = new StringBuilder();
            sb.AppendLine("echo test-text > test.txt");
            if (isWindows)
            {
                sb.AppendLine("copy test.txt test2.txt");
            }
            else
            {
                sb.AppendLine("cp test.txt test2.txt");
            }
            
            additionalConfig["Commands"] = sb.ToString();

            var provider = new Program(new string[] { GetArgString("main", config, additionalConfig) });
            var result = await provider.GenericExecution();

            Assert.True(File.Exists(Path.Combine(workingLocation, "test.txt")));
            Assert.True(File.Exists(Path.Combine(workingLocation, "test2.txt")));
        }

        private string GetArgString(string process, BaseJobTaskConfig taskConfig, Dictionary<string, string> additionalConfigs)
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
