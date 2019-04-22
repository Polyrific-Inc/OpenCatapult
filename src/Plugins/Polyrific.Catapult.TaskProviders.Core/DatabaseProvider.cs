// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.TaskProviders.Core
{
    public abstract class DatabaseProvider : TaskProvider
    {
        protected DatabaseProvider(string[] args) 
            : base(args)
        {
            ParseArguments();
        }

        public override string Type => TaskProviderType.DatabaseProvider;

        public sealed override void ParseArguments()
        {
            base.ParseArguments();

            foreach (var key in ParsedArguments.Keys)
            {
                switch (key.ToLower())
                {
                    case "project":
                        ProjectName = ParsedArguments[key].ToString();
                        break;
                    case "config":
                        Config = JsonConvert.DeserializeObject<DeployDbTaskConfig>(ParsedArguments[key].ToString());
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
                    var error = await BeforeDeployDatabase();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                case "main":
                    (string databaseLocation, Dictionary<string, string> outputValues, string errorMessage) = await DeployDatabase();
                    result.Add("databaseLocation", databaseLocation);
                    result.Add("outputValues", outputValues);
                    result.Add("errorMessage", errorMessage);
                    break;
                case "post":
                    error = await AfterDeployDatabase();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                default:
                    await BeforeDeployDatabase();
                    (databaseLocation, outputValues, errorMessage) = await DeployDatabase();
                    await AfterDeployDatabase();

                    result.Add("databaseLocation", databaseLocation);
                    result.Add("outputValues", outputValues);
                    result.Add("errorMessage", errorMessage);
                    break;
            }

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Name of the project
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// DeployDb task configuration
        /// </summary>
        public DeployDbTaskConfig Config { get; set; }

        /// <summary>
        /// Additional configurations for specific provider
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// Process to run before executing deploy database
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforeDeployDatabase()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Deploy database
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string databaseLocation, Dictionary<string, string> outputValues, string errorMessage)> DeployDatabase();

        /// <summary>
        /// Process to run after executing deploy database
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterDeployDatabase()
        {
            return Task.FromResult("");
        }
    }
}
