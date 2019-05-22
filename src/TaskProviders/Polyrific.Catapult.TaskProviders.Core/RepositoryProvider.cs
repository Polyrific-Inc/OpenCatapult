// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectMember;
using System;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.TaskProviders.Core
{
    public abstract class RepositoryProvider : TaskProvider
    {
        protected RepositoryProvider(string[] args) 
            : base(args)
        {
            ParseArguments();
        }

        public override string Type => TaskProviderType.RepositoryProvider;

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
                    case "projectmembers":
                        ProjectMembers = JsonConvert.DeserializeObject<List<ProjectMemberDto>>(ParsedArguments[key].ToString());
                        break;
                    case "pullconfig":
                        PullTaskConfig = JsonConvert.DeserializeObject<PullTaskConfig>(ParsedArguments[key].ToString());
                        break;
                    case "pushconfig":
                        PushTaskConfig = JsonConvert.DeserializeObject<PushTaskConfig>(ParsedArguments[key].ToString());
                        break;
                    case "mergeconfig":
                        MergeTaskConfig = JsonConvert.DeserializeObject<MergeTaskConfig>(ParsedArguments[key].ToString());
                        break;
                    case "deleteconfig":
                        DeleteTaskConfig = JsonConvert.DeserializeObject<DeleteRepositoryTaskConfig>(ParsedArguments[key].ToString());
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

            try
            {
                if (PullTaskConfig != null)
                {
                    switch (ProcessToExecute)
                    {
                        case "pre":
                            var error = await BeforePull();
                            if (!string.IsNullOrEmpty(error))
                                result.Add("errorMessage", error);
                            break;
                        case "main":
                            (string repositoryLocation, Dictionary<string, string> outputValues, string errorMessage) = await Pull();
                            result.Add("repositoryLocation", repositoryLocation);
                            result.Add("outputValues", outputValues);
                            result.Add("errorMessage", errorMessage);
                            break;
                        case "post":
                            error = await AfterPull();
                            if (!string.IsNullOrEmpty(error))
                                result.Add("errorMessage", error);
                            break;
                        default:
                            await BeforePull();
                            (repositoryLocation, outputValues, errorMessage) = await Pull();
                            await AfterPull();

                            result.Add("repositoryLocation", repositoryLocation);
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
                            (string remoteUrl, string pullRequestUrl, Dictionary<string, string> outputValues, string errorMessage) = await Push();
                            result.Add("remoteUrl", remoteUrl);
                            result.Add("pullRequestUrl", pullRequestUrl);
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
                            (remoteUrl, pullRequestUrl, outputValues, errorMessage) = await Push();
                            await AfterPush();

                            result.Add("remoteUrl", remoteUrl);
                            result.Add("pullRequestUrl", pullRequestUrl);
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
                else if (DeleteTaskConfig != null)
                {
                    var error = await DeleteRepository();
                    if (!string.IsNullOrEmpty(error))
                        result.Add("errorMessage", error);
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
        /// Name of the project
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Members of the project
        /// </summary>
        public List<ProjectMemberDto> ProjectMembers { get; set; }

        /// <summary>
        /// Pull task configuration
        /// </summary>
        public PullTaskConfig PullTaskConfig { get; set; }

        /// <summary>
        /// Push task configuration
        /// </summary>
        public PushTaskConfig PushTaskConfig { get; set; }

        /// <summary>
        /// Merge task configuration
        /// </summary>
        public MergeTaskConfig MergeTaskConfig { get; set; }

        /// <summary>
        /// Delete task configuration
        /// </summary>
        public DeleteRepositoryTaskConfig DeleteTaskConfig { get; set; }

        /// <summary>
        /// Additional configurations for specific provider
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// The PR number to be used in merge process
        /// </summary>
        public string PrNumber { get; set; }

        /// <summary>
        /// Process to run before executing the pull
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> BeforePull()
        {
            return Task.FromResult("");
        }

        /// <summary>
        /// Pull the code from remote repository
        /// </summary>
        /// <returns></returns>
        public abstract Task<(string repositoryLocation, Dictionary<string, string> outputValues, string errorMessage)> Pull();

        /// <summary>
        /// Process to run after executing pull repository
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> AfterPull()
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
        public abstract Task<(string remoteUrl, string pullRequestUrl, Dictionary<string, string> outputValues, string errorMessage)> Push();

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

        /// <summary>
        /// Delete the remote repository
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> DeleteRepository();
    }
}
