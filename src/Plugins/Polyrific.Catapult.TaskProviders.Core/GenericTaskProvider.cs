// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.TaskProviders.Core.Configs;

namespace Polyrific.Catapult.TaskProviders.Core
{
    public abstract class GenericTaskProvider : TaskProvider
    {
        protected GenericTaskProvider(string[] args) : base(args)
        {
            ParseArguments();
        }

        public override string Type => PluginType.GenericTaskProvider;

        public sealed override void ParseArguments()
        {
            base.ParseArguments();

            foreach (var key in ParsedArguments.Keys)
            {
                switch (key.ToLower())
                {
                    case "config":
                        Config = JsonConvert.DeserializeObject<BaseJobTaskConfig>(ParsedArguments[key].ToString());
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

            switch (ProcessToExecute)
            {
                case "pre":
                    var error = await BeforeGenericExecution();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                case "main":
                    (var outputValues, string errorMessage) = await GenericExecution();
                    result.Add("outputValues", outputValues);
                    result.Add("errorMessage", errorMessage);
                    break;
                case "post":
                    error = await AfterGenericExecution();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                default:
                    await BeforeGenericExecution();
                    (outputValues, errorMessage) = await GenericExecution();
                    await AfterGenericExecution();
                    result.Add("outputValues", outputValues);
                    result.Add("errorMessage", errorMessage);
                    break;
            }

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Generic task provider config
        /// </summary>
        public BaseJobTaskConfig Config { get; set; }

        /// <summary>
        /// Additional configurations for specific provider
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// Process to run before executing a generic action
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforeGenericExecution()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Execute a generic action
        /// </summary>
        /// <returns></returns>
        public abstract Task<(Dictionary<string, string> outputValues, string errorMessage)> GenericExecution();

        /// <summary>
        /// Process to run after executing a generic action
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterGenericExecution()
        {
            return Task.FromResult("");
        }
    }
}
