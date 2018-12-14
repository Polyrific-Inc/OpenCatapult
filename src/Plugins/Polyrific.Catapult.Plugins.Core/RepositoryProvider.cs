// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.Plugins.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Plugins.Core
{
    public abstract class RepositoryProvider : TaskProvider
    {
        protected RepositoryProvider(string[] args) : base(args)
        {
            ParseArguments();
        }

        public override string Type => PluginType.RepositoryProvider;

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
                    case "cloneconfig":
                        CloneTaskConfig = JsonConvert.DeserializeObject<CloneTaskConfig>(ParsedArguments[key].ToString());
                        break;
                    case "pushconfig":
                        PushTaskConfig = JsonConvert.DeserializeObject<PushTaskConfig>(ParsedArguments[key].ToString());
                        break;
                    case "mergeconfig":
                        MergeTaskConfig = JsonConvert.DeserializeObject<MergeTaskConfig>(ParsedArguments[key].ToString());
                        break;
                    case "prnumber":
                        PrNumber = ParsedArguments[key].ToString();
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

            if (CloneTaskConfig != null)
            {
                switch (ProcessToExecute)
                {
                    case "pre":
                        var error = await BeforeClone();
                        if (!string.IsNullOrEmpty(error))
                            result.Add("errorMessage", error);
                        break;
                    case "main":
                        (string cloneLocation, Dictionary<string, string> outputValues, string errorMessage) = await Clone();
                        result.Add("cloneLocation", cloneLocation);
                        result.Add("outputValues", outputValues);
                        result.Add("errorMessage", errorMessage);
                        break;
                    case "post":
                        error = await AfterClone();
                        if (!string.IsNullOrEmpty(error))
                            result.Add("errorMessage", error);
                        break;
                    default:
                        await BeforeClone();
                        (cloneLocation, outputValues, errorMessage) = await Clone();
                        await AfterClone();

                        result.Add("cloneLocation", cloneLocation);
                        result.Add("outputValues", outputValues);
                        result.Add("errorMessage", errorMessage);
                        break;
                }
            }
            else if (PushTaskConfig != null)
            {
                switch (ProcessToExecute)
                {
                    case "pre":
                        var error = await BeforePush();
                        if (!string.IsNullOrEmpty(error))
                            result.Add("errorMessage", error);
                        break;
                    case "main":
                        (string remoteUrl, Dictionary<string, string> outputValues, string errorMessage) = await Push();
                        result.Add("remoteUrl", remoteUrl);
                        result.Add("outputValues", outputValues);
                        result.Add("errorMessage", errorMessage);
                        break;
                    case "post":
                        error = await AfterPush();
                        if (!string.IsNullOrEmpty(error))
                            result.Add("errorMessage", error);
                        break;
                    default:
                        await BeforePush();
                        (remoteUrl, outputValues, errorMessage) = await Push();
                        await AfterPush();

                        result.Add("remoteUrl", remoteUrl);
                        result.Add("outputValues", outputValues);
                        result.Add("errorMessage", errorMessage);
                        break;
                }
            }
            else if (MergeTaskConfig != null && !string.IsNullOrEmpty(PrNumber))
            {
                switch (ProcessToExecute)
                {
                    case "pre":
                        var error = await BeforeMerge();
                        if (!string.IsNullOrEmpty(error))
                            result.Add("errorMessage", error);
                        break;
                    case "main":
                        (string remoteUrl, Dictionary<string, string> outputValues, string errorMessage) = await Merge();
                        result.Add("remoteUrl", remoteUrl);
                        result.Add("outputValues", outputValues);
                        result.Add("errorMessage", errorMessage);
                        break;
                    case "post":
                        error = await AfterMerge();
                        if (!string.IsNullOrEmpty(error))
                            result.Add("errorMessage", error);
                        break;
                    default:
                        await BeforeMerge();
                        (remoteUrl, outputValues, errorMessage) = await Merge();
                        await AfterMerge();

                        result.Add("remoteUrl", remoteUrl);
                        result.Add("outputValues", outputValues);
                        result.Add("errorMessage", errorMessage);
                        break;
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Name of the project
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Clone task configuration
        /// </summary>
        public CloneTaskConfig CloneTaskConfig { get; set; }

        /// <summary>
        /// Push task configuration
        /// </summary>
        public PushTaskConfig PushTaskConfig { get; set; }

        /// <summary>
        /// Merge task configuration
        /// </summary>
        public MergeTaskConfig MergeTaskConfig { get; set; }

        /// <summary>
        /// Additional configurations for specific provider
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// The PR number to be used in merge process
        /// </summary>
        public string PrNumber { get; set; }

        /// <summary>
        /// Process to run before executing the clone
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforeClone()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Clone the code from remote repository
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string cloneLocation, Dictionary<string, string> outputValues, string errorMessage)> Clone();

        /// <summary>
        /// Process to run after executing clone repository
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterClone()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Process to run before executing the Push
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforePush()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Push the code to remote repository
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string remoteUrl, Dictionary<string, string> outputValues, string errorMessage)> Push();

        /// <summary>
        /// Process to run after executing Push repository
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterPush()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Process to run before executing the Merge
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforeMerge()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Merge the code to remote repository
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string remoteUrl, Dictionary<string, string> outputValues, string errorMessage)> Merge();

        /// <summary>
        /// Process to run after executing Merge repository
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterMerge()
        {
            return Task.FromResult("");
        }
    }
}
