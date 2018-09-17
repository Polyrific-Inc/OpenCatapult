// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace GitHubRepositoryProvider
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

        public string[] RequiredServices => new string[] { "GitHub" };

        public Task<string> BeforeClone(string repositoryFolder, CloneTaskConfig config, Dictionary<string, string> serviceProperties)
        {
            return Task.FromResult("");
        }

        public async Task<(string returnValue, string errorMessage)> Clone(string repositoryFolder, CloneTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger)
        {
            var localRepo = Path.Combine(config.WorkingLocation, repositoryFolder);

            if (_codeRepository == null)
                _codeRepository = new CodeRepository(GetCodeRepositoryConfig(localRepo, serviceProperties), logger);

            var error = await _codeRepository.Clone();
            if (!string.IsNullOrEmpty(error))
                return ("", error);

            return (localRepo, "");
        }

        public Task<string> AfterClone(string repositoryFolder, CloneTaskConfig config, Dictionary<string, string> serviceProperties)
        {
            return Task.FromResult("");
        }

        public Task<string> BeforePush(string repositoryFolder, PushTaskConfig config, Dictionary<string, string> serviceProperties)
        {
            return Task.FromResult("");
        }

        public async Task<(string returnValue, string errorMessage)> Push(string repositoryFolder, PushTaskConfig config, Dictionary<string, string> serviceProperties, ILogger logger)
        {
            var localRepo = Path.Combine(config.WorkingLocation, repositoryFolder);
            var repoConfig = GetCodeRepositoryConfig(localRepo, serviceProperties);

            if (_codeRepository == null)
                _codeRepository = new CodeRepository(repoConfig, logger);

            var error = await _codeRepository.Push(config.Branch);
            if (!string.IsNullOrEmpty(error))
                return ("", error);

            return (repoConfig.RemoteUrl, "");
        }

        public Task<string> AfterPush(string repositoryFolder, PushTaskConfig config, Dictionary<string, string> serviceProperties)
        {
            return Task.FromResult("");
        }

        private CodeRepositoryConfig GetCodeRepositoryConfig(string localRepository, Dictionary<string, string> serviceProperties)
        {
            var config = new CodeRepositoryConfig();

            config.LocalRepository = localRepository;

            if (serviceProperties.ContainsKey("RemoteUrl"))
                config.RemoteUrl = serviceProperties["RemoteUrl"];

            if (serviceProperties.ContainsKey("RemoteCredentialType"))
                config.RemoteCredentialType = serviceProperties["RemoteCredentialType"];

            if (serviceProperties.ContainsKey("RemoteUsername"))
                config.RemoteUsername = serviceProperties["RemoteUsername"];

            if (serviceProperties.ContainsKey("RemotePassword"))
                config.RemotePassword = serviceProperties["RemotePassword"];

            if (serviceProperties.ContainsKey("RepoAuthToken"))
                config.RepoAuthToken = serviceProperties["RepoAuthToken"];

            return config;
        }
    }
}
