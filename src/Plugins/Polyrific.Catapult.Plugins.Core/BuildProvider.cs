// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.Plugins.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Plugins.Core
{
    public abstract class BuildProvider : TaskProvider
    {
        protected BuildProvider(string[] args) : base(args)
        {
            ParseArguments();
        }

        public override string Type => PluginType.BuildProvider;

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
                        Config = JsonConvert.DeserializeObject<BuildTaskConfig>(ParsedArguments[key].ToString());
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
                    var error = await BeforeBuild();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                case "main":
                    (string outputArtifact, Dictionary<string, string> outputValues, string errorMessage) = await Build();
                    result.Add("outputArtifact", outputArtifact);
                    result.Add("outputValues", outputValues);
                    result.Add("errorMessage", errorMessage);
                    break;
                case "post":
                    error = await AfterBuild();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                default:
                    await BeforeBuild();
                    (outputArtifact, outputValues, errorMessage) = await Build();
                    await AfterBuild();

                    result.Add("outputArtifact", outputArtifact);
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
        /// Build task configuration
        /// </summary>
        public BuildTaskConfig Config { get; set; }

        /// <summary>
        /// Additional configurations for specific provider
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// Process to run before executing build
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforeBuild()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Build the code, and produce a ready to deploy artifact
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string outputArtifact, Dictionary<string, string> outputValues, string errorMessage)> Build();

        /// <summary>
        /// Process to run after executing build
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterBuild()
        {
            return Task.FromResult("");
        }
    }
}
