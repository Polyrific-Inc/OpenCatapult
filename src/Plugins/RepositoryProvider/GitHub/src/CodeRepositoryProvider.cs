// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace GitHub
{
    [Export(typeof(ICodeRepositoryProvider))]
    public class CodeRepositoryProvider : ICodeRepositoryProvider
    {
        private IGitAutomation _gitAutomation;
        private readonly IGitHubUtils _gitHubUtils;

        private const string DefaultAuthor = "OpenCatapult";
        private const string DefaultEmail = "admin@opencatapult.net";
        private const string DefaultCommitMessage = "Changes by OpenCatapult";

        public CodeRepositoryProvider()
        {

        }

        public CodeRepositoryProvider(IGitAutomation gitAutomation)
        {
            _gitAutomation = gitAutomation;
        }

        public CodeRepositoryProvider(IGitHubUtils gitHubUtils)
        {
            _gitHubUtils = gitHubUtils;
        }

        public string Name => "GitHubRepositoryProvider";

        public string[] RequiredServices => new[] { "GitHub" };

        public Task<string> BeforeClone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string cloneLocation, Dictionary<string, string> outputValues, string errorMessage)> Clone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            var repoConfig = GetGitAutomationConfig(config.CloneLocation, config.Repository, additionalConfigs, config.IsPrivateRepository);

            if (_gitAutomation == null)
                _gitAutomation = new GitAutomation(repoConfig, _gitHubUtils, logger);

            var error = await _gitAutomation.Clone();
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);

            if (!string.IsNullOrEmpty(config.BaseBranch))
                await _gitAutomation.CheckoutBranch(config.BaseBranch);

            return (config.CloneLocation, null, "");
        }

        public Task<string> AfterClone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public Task<string> BeforePush(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string remoteUrl, Dictionary<string, string> outputValues, string errorMessage)> Push(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            var repoConfig = GetGitAutomationConfig(config.SourceLocation, config.Repository, additionalConfigs);

            if (_gitAutomation == null)
                _gitAutomation = new GitAutomation(repoConfig, _gitHubUtils, logger);

            var commitError = await _gitAutomation.Commit(config.PullRequestTargetBranch, config.Branch, config.CommitMessage ?? DefaultCommitMessage, config.Author ?? DefaultAuthor, config.Email ?? DefaultEmail);
            if (!string.IsNullOrEmpty(commitError))
                return ("", null, commitError);

            var error = await _gitAutomation.Push(config.Branch);
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);

            Dictionary<string, string> outputValues = null;

            if (config.CreatePullRequest)
            {
                var prNumber = await _gitAutomation.SubmitPullRequest(config.Branch, config.PullRequestTargetBranch);
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

        public Task<string> AfterPush(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public Task<string> BeforeMerge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string remoteUrl, Dictionary<string, string> outputValues, string errorMessage)> Merge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            var repoConfig = GetGitAutomationConfig("", config.Repository, additionalConfigs);

            if (_gitAutomation == null)
                _gitAutomation = new GitAutomation(repoConfig, _gitHubUtils, logger);

            var success = await _gitAutomation.MergePullRequest(prNumber);
            if (!success)
                return ("", null, "Failed to merge pull request.");

            return (config.Repository, null, "");
        }

        public Task<string> AfterMerge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        private GitAutomationConfig GetGitAutomationConfig(string localRepository, string remoteUrl, Dictionary<string, string> additionalConfigs, bool isPrivateRepository = false)
        {
            var remoteUrlCleaned = remoteUrl?.Trim(' ', '/');
            var config = new GitAutomationConfig
            {
                LocalRepository = localRepository,
                RemoteUrl = remoteUrlCleaned,
                IsPrivateRepository = isPrivateRepository,
            };

            var remoteUrlBrokenDown = remoteUrlCleaned?.Split('/');
            if (remoteUrlBrokenDown.Length >= 2)
            {
                config.ProjectName = remoteUrlBrokenDown.Last();
                config.RepoOwner = remoteUrlBrokenDown[remoteUrlBrokenDown.Length - 2];
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
    }
}
