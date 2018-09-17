// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GitHubRepositoryProvider.Helpers;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace GitHubRepositoryProvider
{
    public class CodeRepository : ICodeRepository
    {
        private const int MaxAttempt = 3;

        private readonly CodeRepositoryConfig _config;
        private readonly ILogger _logger;

        public CodeRepository(CodeRepositoryConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public Task<string> Clone()
        {
            if (Directory.Exists(_config.LocalRepository))
                Directory.Delete(_config.LocalRepository, true);

            Directory.CreateDirectory(_config.LocalRepository);

            var cloneOption = new CloneOptions()
            {
                OnTransferProgress = CloneTransferProgressHandler
            };

            if (_config.IsPrivateRepository)
            {
                if (_config.RemoteCredentialType == "userPassword")
                {
                    cloneOption.CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
                    {
                        Username = _config.RemoteUsername,
                        Password = _config.RemotePassword
                    };
                }
                else if (_config.RemoteCredentialType == "authToken")
                {
                    cloneOption.CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
                    {
                        Username = _config.RepoAuthToken,
                        Password = string.Empty
                    };
                }
            }

            var attempt = 1;

            // start attempt clone
            var repoLocation = "";
            var errorMessage = "";
            while (string.IsNullOrEmpty(repoLocation) && attempt <= MaxAttempt)
            {
                _logger.LogInformation($"Clone GitHub repository (attempt #{attempt}) from {_config.RemoteUrl}.");

                attempt++;

                try
                {
                    // clone GitHub repository
                    repoLocation = Repository.Clone(_config.RemoteUrl, _config.LocalRepository, cloneOption);

                    _logger.LogInformation($"GitHub repository has been successfully cloned from {_config.RemoteUrl}.");
                    return Task.FromResult("");
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    _logger.LogError(ex, ex.Message);
                    Thread.Sleep(30000);
                }
            }

            return Task.FromResult(errorMessage);
        }

        public Task<string> Push(string branch)
        {
            var errorMessage = "";

            var credential = new UsernamePasswordCredentials();

            // use GitHub token if provided
            if (_config.RemoteCredentialType == "userPassword")
            {
                // use user name and password if there is no GitHub token
                credential.Username = _config.RemoteUsername;
                credential.Password = _config.RemotePassword;
            }
            else if (_config.RemoteCredentialType == "authToken")
            {
                credential.Username = _config.RepoAuthToken;
                credential.Password = string.Empty;
            }
            else
            {
                errorMessage = "No credential provided. Please check Push Task definition.";
                _logger.LogError(errorMessage);
                return Task.FromResult(errorMessage);
            }

            // set push options based on credential type
            var options = new PushOptions
            {
                CredentialsProvider = (url, usernameFromUrl, types) => credential,
                OnPushTransferProgress = PushTransferProgressHandler
            };

            var attempt = 1;
            while (attempt <= MaxAttempt)
            {
                _logger.LogInformation($"Push {branch} into remote repository: {_config.RemoteUrl} (attempt #{attempt})");

                attempt++;

                try
                {
                    using (var repo = new Repository(_config.LocalRepository))
                    {
                        // Push the changes                     
                        repo.Network.Push(repo.Branches[branch], options);
                        _logger.LogInformation($"All changes from {branch} have been successfully pushed into: {_config.RemoteUrl}");

                        return Task.FromResult("");
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    _logger.LogError(ex, ex.Message);
                    Thread.Sleep(30000);
                }
            }

            return Task.FromResult(errorMessage);
        }

        private bool CloneTransferProgressHandler(TransferProgress progress)
        {
            if (progress.TotalObjects > 0)
            {
                double progressPercentage = progress.ReceivedObjects / (double)progress.TotalObjects;
                _logger.LogDebug($"Clone progress {progressPercentage:P2} : received objects: {progress.ReceivedObjects}/{progress.TotalObjects} objects)");
            }

            return true;
        }

        private bool PushTransferProgressHandler(int current, int total, long bytes)
        {
            if (total > 0)
            {
                double progressPercentage = current / (double)total;
                _logger.LogDebug($"Push progress {progressPercentage:P2} : {current}/{total} ({ConversionHelper.FormatBytes(bytes)})");
            }

            return true;
        }
    }
}
