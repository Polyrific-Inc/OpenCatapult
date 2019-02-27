// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.TaskProviders.Core
{
    public abstract class StorageProvider : TaskProvider
    {
        protected StorageProvider(string[] args) 
            : base(args)
        {
            ParseArguments();
        }

        public override string Type => PluginType.StorageProvider;

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
                    var error = await BeforePublishArtifact();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                case "main":
                    (string storageLocation, Dictionary<string, string> outputValues, string errorMessage) = await PublishArtifact();
                    result.Add("storageLocation", storageLocation);
                    result.Add("outputValues", outputValues);
                    result.Add("errorMessage", errorMessage);
                    break;
                case "post":
                    error = await AfterPublishArtifact();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
                    break;
                default:
                    await BeforePublishArtifact();
                    (storageLocation, outputValues, errorMessage) = await PublishArtifact();
                    await AfterPublishArtifact();

                    result.Add("storageLocation", storageLocation);
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
        /// Process to run before publishing artifact
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforePublishArtifact()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Publish artifact to a storage
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string storageLocation, Dictionary<string, string> outputValues, string errorMessage)> PublishArtifact();

        /// <summary>
        /// Process to run after publishing artifact
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterPublishArtifact()
        {
            return Task.FromResult("");
        }
    }
}
