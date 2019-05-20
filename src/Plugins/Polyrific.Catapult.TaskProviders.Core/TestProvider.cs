// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.TaskProviders.Core
{
    public abstract class TestProvider : TaskProvider
    {
        protected TestProvider(string[] args) 
            : base(args)
        {
            ParseArguments();
        }

        public override string Type => TaskProviderType.TestProvider;

        public sealed override void ParseArguments()
        {
            base.ParseArguments();

            foreach (var key in ParsedArguments.Keys)
            {
                switch (key.ToLower())
                {
                    case "config":
                        Config = JsonConvert.DeserializeObject<TestTaskConfig>(ParsedArguments[key].ToString());
                        break;
                    case "additional":
                        AdditionalConfigs = JsonConvert.DeserializeObject<Dictionary<string, string>>(ParsedArguments[key].ToString());
                        break;
                }
            }
        }

        public override async Task<string> Execute()
        {
            var result = new Dictionary<string, object>();

            try
            {
                switch (ProcessToExecute)
                {
                    case "pre":
                        var error = await BeforeTest();
                        if (!string.IsNullOrEmpty(error))
                            result.Add("errorMessage", error);
                        break;
                    case "main":
                        (string testResultLocation, Dictionary<string, string> outputValues, string errorMessage) = await Test();
                        result.Add("testResultLocation", testResultLocation);
                        result.Add("outputValues", outputValues);
                        result.Add("errorMessage", errorMessage);
                        break;
                    case "post":
                        error = await AfterTest();
                        if (!string.IsNullOrEmpty(error))
                            result.Add("errorMessage", error);
                        break;
                    default:
                        await BeforeTest();
                        (testResultLocation, outputValues, errorMessage) = await Test();
                        await AfterTest();

                        result.Add("testResultLocation", testResultLocation);
                        result.Add("outputValues", outputValues);
                        result.Add("errorMessage", errorMessage);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                result.Add("errorMessage", ex.Message);
            }

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Test task configuration
        /// </summary>
        public TestTaskConfig Config { get; set; }

        /// <summary>
        /// Additional configurations for specific provider
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// Process to run before executing test
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforeTest()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Run test scenario
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string testResultLocation, Dictionary<string, string> outputValues, string errorMessage)> Test();

        /// <summary>
        /// Process to run after executing test
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterTest()
        {
            return Task.FromResult("");
        }
    }
}
