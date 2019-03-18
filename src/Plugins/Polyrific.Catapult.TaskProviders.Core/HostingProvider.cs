// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.TaskProviders.Core
{
    public abstract class HostingProvider : TaskProvider
    {
        protected HostingProvider(string[] args) 
            : base(args)
        {
            ParseArguments();
        }

        public override string Type => PluginType.HostingProvider;

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
                        Config = JsonConvert.DeserializeObject<DeployTaskConfig>(ParsedArguments[key].ToString());
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
                    var error = await BeforeDeploy();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                case "main":
                    (string hostLocation, Dictionary<string, string> outputValues, string errorMessage) = await Deploy();
                    result.Add("hostLocation", hostLocation);
                    result.Add("outputValues", outputValues);
                    result.Add("errorMessage", errorMessage);
                    break;
                case "post":
                    error = await AfterDeploy();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                case "delete":
                    error = await DeleteHostingResources();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                default:
                    await BeforeDeploy();
                    (hostLocation, outputValues, errorMessage) = await Deploy();
                    await AfterDeploy();

                    result.Add("hostLocation", hostLocation);
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
        /// Deploy task configuration
        /// </summary>
        public DeployTaskConfig Config { get; set; }

        /// <summary>
        /// Additional configurations for specific provider
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// Process to run before executing the deployment
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforeDeploy()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Deploy artifact
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string hostLocation, Dictionary<string, string> outputValues, string errorMessage)> Deploy();

        /// <summary>
        /// Delete the hostring resources
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> DeleteHostingResources();

        /// <summary>
        /// Process to run after executing deployment
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterDeploy()
        {
            return Task.FromResult("");
        }
    }
}
