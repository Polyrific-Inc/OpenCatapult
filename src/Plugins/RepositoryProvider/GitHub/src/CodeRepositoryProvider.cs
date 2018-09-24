// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace GitHub
{
    [Export(typeof(ICodeRepositoryProvider))]
    public class CodeRepositoryProvider : ICodeRepositoryProvider
    {
        private ICodeRepository _codeRepository;

        public CodeRepositoryProvider()
        {

        }

        public CodeRepositoryProvider(ICodeRepository codeRepository)
        {
            _codeRepository = codeRepository;
        }

        public string Name => "GitHubRepositoryProvider";

        public string[] RequiredServices => new[] { "GitHub" };

        public Task<string> BeforeClone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string returnValue, string errorMessage)> Clone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            var repoConfig = GetCodeRepositoryConfig(config.CloneLocation, config.Repository, additionalConfigs);

            if (_codeRepository == null)
                _codeRepository = new CodeRepository(repoConfig, logger);

            var error = await _codeRepository.Clone();
            if (!string.IsNullOrEmpty(error))
                return ("", error);

            return (config.CloneLocation, "");
        }

        public Task<string> AfterClone(CloneTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public Task<string> BeforePush(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string returnValue, string errorMessage)> Push(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            var repoConfig = GetCodeRepositoryConfig(config.SourceLocation, config.Repository, additionalConfigs);

            if (_codeRepository == null)
                _codeRepository = new CodeRepository(repoConfig, logger);

            var error = await _codeRepository.Push(config.Branch);
            if (!string.IsNullOrEmpty(error))
                return ("", error);

            return (repoConfig.RemoteUrl, "");
        }

        public Task<string> AfterPush(PushTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public Task<string> BeforeMerge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string returnValue, string errorMessage)> Merge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            var repoConfig = GetCodeRepositoryConfig("", config.Repository, additionalConfigs);

            if (_codeRepository == null)
                _codeRepository = new CodeRepository(repoConfig, logger);

            var success = await _codeRepository.MergePullRequest(prNumber);
            if (!success)
                return ("", "Failed to merge pull request.");

            return (config.Repository, "");
        }

        public Task<string> AfterMerge(string prNumber, MergeTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        private CodeRepositoryConfig GetCodeRepositoryConfig(string localRepository, string remoteUrl, Dictionary<string, string> additionalConfigs)
        {
            var config = new CodeRepositoryConfig
            {
                LocalRepository = localRepository,
                RemoteUrl = remoteUrl
            };

            if (additionalConfigs.ContainsKey("RemoteCredentialType"))
                config.RemoteCredentialType = additionalConfigs["RemoteCredentialType"];

            if (additionalConfigs.ContainsKey("RemoteUsername"))
                config.RemoteUsername = additionalConfigs["RemoteUsername"];

            if (additionalConfigs.ContainsKey("RemotePassword"))
                config.RemotePassword = additionalConfigs["RemotePassword"];

            if (additionalConfigs.ContainsKey("RepoAuthToken"))
                config.RepoAuthToken = additionalConfigs["RepoAuthToken"];

            return config;
        }
    }
}
