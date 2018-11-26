﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Core;

namespace Polyrific.Catapult.Plugins.GitHub
{
    internal class Program : RepositoryProvider
    {
        private IGitAutomation _gitAutomation;

        private const string DefaultAuthor = "OpenCatapult";
        private const string DefaultEmail = "admin@opencatapult.net";
        private const string DefaultCommitMessage = "Changes by OpenCatapult";
        private const string DefaultBaseBranch = "master";
        private const string DefaultWorkingBranch = "OpenCatapultGenerated";

        public override string Name => "Polyrific.Catapult.Plugins.GitHubRepositoryProvider";

        public Program() : base(new string[0])
        {
        }

        public Program(string[] args) : base(args)
        {
        }

        public override async Task<(string cloneLocation, Dictionary<string, string> outputValues, string errorMessage)> Clone()
        {
            var repoConfig = GetGitAutomationConfig(CloneTaskConfig.CloneLocation ?? CloneTaskConfig.WorkingLocation, CloneTaskConfig.Repository, AdditionalConfigs, CloneTaskConfig.IsPrivateRepository);

            if (_gitAutomation == null)
                _gitAutomation = new GitAutomation(repoConfig, Logger);

            var error = await _gitAutomation.Clone();
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);

            if (!string.IsNullOrEmpty(CloneTaskConfig.BaseBranch))
                await _gitAutomation.CheckoutBranch(CloneTaskConfig.BaseBranch);

            return (CloneTaskConfig.CloneLocation, null, "");
        }

        public override async Task<(string remoteUrl, Dictionary<string, string> outputValues, string errorMessage)> Push()
        {
            var repoConfig = GetGitAutomationConfig(PushTaskConfig.SourceLocation ?? PushTaskConfig.WorkingLocation, PushTaskConfig.Repository, AdditionalConfigs);

            if (_gitAutomation == null)
                _gitAutomation = new GitAutomation(repoConfig, Logger);

            string baseBranch = PushTaskConfig.PullRequestTargetBranch ?? DefaultBaseBranch;
            string workingBranch = PushTaskConfig.Branch ?? (PushTaskConfig.CreatePullRequest ? DefaultWorkingBranch : DefaultBaseBranch);

            var commitError = await _gitAutomation.Commit(baseBranch, workingBranch, PushTaskConfig.CommitMessage ?? DefaultCommitMessage, PushTaskConfig.Author ?? DefaultAuthor, PushTaskConfig.Email ?? DefaultEmail);
            if (!string.IsNullOrEmpty(commitError))
                return ("", null, commitError);

            var error = await _gitAutomation.Push(workingBranch);
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);

            Dictionary<string, string> outputValues = null;

            if (PushTaskConfig.CreatePullRequest)
            {
                var prNumber = await _gitAutomation.SubmitPullRequest(workingBranch, baseBranch);
                if (prNumber > 0)
                {
                    outputValues = new Dictionary<string, string>
                    {
                        {"PRNumber", prNumber.ToString()}
                    };
                }
            }

            return (repoConfig.RemoteUrl, outputValues, "");
        }

        public override async Task<(string remoteUrl, Dictionary<string, string> outputValues, string errorMessage)> Merge()
        {
            var repoConfig = GetGitAutomationConfig("", MergeTaskConfig.Repository, AdditionalConfigs);

            if (_gitAutomation == null)
                _gitAutomation = new GitAutomation(repoConfig, Logger);

            var success = await _gitAutomation.MergePullRequest(PrNumber);
            if (!success)
                return ("", null, "Failed to merge pull request.");

            return (MergeTaskConfig.Repository, null, "");
        }

        private GitAutomationConfig GetGitAutomationConfig(string localRepository, string remoteUrl, Dictionary<string, string> additionalConfigs, bool isPrivateRepository = false)
        {
            var config = new GitAutomationConfig
            {
                LocalRepository = localRepository,
                RemoteUrl = remoteUrl,
                IsPrivateRepository = isPrivateRepository,
            };

            var remoteUrlBrokenDown = new Uri(remoteUrl).AbsolutePath?.Trim(' ', '/').Split('/');
            if (remoteUrlBrokenDown != null && remoteUrlBrokenDown.Length == 2)
            {
                config.RepoOwner = remoteUrlBrokenDown[0];
                config.ProjectName = remoteUrlBrokenDown[1];
            }

            if (additionalConfigs != null)
            {
                if (additionalConfigs.ContainsKey("RemoteCredentialType"))
                    config.RemoteCredentialType = additionalConfigs["RemoteCredentialType"];

                if (additionalConfigs.ContainsKey("RemoteUsername"))
                    config.RemoteUsername = additionalConfigs["RemoteUsername"];

                if (additionalConfigs.ContainsKey("RemotePassword"))
                    config.RemotePassword = additionalConfigs["RemotePassword"];

                if (additionalConfigs.ContainsKey("RepoAuthToken"))
                    config.RepoAuthToken = additionalConfigs["RepoAuthToken"];
            }

            return config;
        }

        private static async Task Main(string[] args)
        {
            var app = new Program(args);

            var result = await app.Execute();
            app.ReturnOutput(result);
        }
    }
}
