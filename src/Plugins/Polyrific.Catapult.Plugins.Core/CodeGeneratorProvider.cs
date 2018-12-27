// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.Plugins.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.Plugins.Core
{
    public abstract class CodeGeneratorProvider : TaskProvider
    {
        protected CodeGeneratorProvider(string[] args, string taskProviderName) 
            : base(args, taskProviderName)
        {
            ParseArguments();
        }

        public override string Type => PluginType.GeneratorProvider;

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
                    case "models":
                        Models = JsonConvert.DeserializeObject<List<ProjectDataModelDto>>(ParsedArguments[key].ToString());
                        break;
                    case "config":
                        Config = JsonConvert.DeserializeObject<GenerateTaskConfig>(ParsedArguments[key].ToString());
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
                    var error = await BeforeGenerate();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                case "main":
                    (string outputLocation, Dictionary<string, string> outputValues, string errorMessage) = await Generate();
                    result.Add("outputLocation", outputLocation);
                    result.Add("outputValues", outputValues);
                    result.Add("errorMessage", errorMessage);
                    break;
                case "post":
                    error = await AfterGenerate();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                default:
                    await BeforeGenerate();
                    (outputLocation, outputValues, errorMessage) = await Generate();
                    await AfterGenerate();

                    result.Add("outputLocation", outputLocation);
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
        /// Project data models
        /// </summary>
        public List<ProjectDataModelDto> Models { get; set; }

        /// <summary>
        /// Generate task configuration
        /// </summary>
        public GenerateTaskConfig Config { get; set; }

        /// <summary>
        /// Additional configurations for specific provider
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// Process to run before executing the code generation
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforeGenerate()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Generate code from data models
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate();

        /// <summary>
        /// Process to run after executing code generation
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterGenerate()
        {
            return Task.FromResult("");
        }
    }
}
