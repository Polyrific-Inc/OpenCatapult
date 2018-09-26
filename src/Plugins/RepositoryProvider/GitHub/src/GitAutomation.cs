// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GitHub
{
    public class GitAutomation : IGitAutomation
    {
        private const int MaxAttempt = 3;

        private readonly GitAutomationConfig _config;
        private readonly IGitHubUtils _gitHubUtils;
        private readonly ILogger _logger;
        
        public GitAutomation(GitAutomationConfig config, IGitHubUtils gitHubUtils, ILogger logger)
        {
            _config = config;
            _gitHubUtils = gitHubUtils ?? new GitHubUtils(config.RemoteCredentialType, config.RemoteCredentialType == "userPassword" ? config.RemoteUsername : config.RepoAuthToken, config.RemotePassword, logger);
            _logger = logger;
        }

        public async Task<string> Clone()
        {
            if (Directory.Exists(_config.LocalRepository))
                Directory.Delete(_config.LocalRepository, true);

            Directory.CreateDirectory(_config.LocalRepository);

            
            var attempt = 1;

            // start attempt clone
            while (attempt <= MaxAttempt)
            {
                _logger.LogInformation($"Clone GitHub repository (attempt #{attempt}) from {_config.RemoteUrl}.");

                attempt++;

                var repoLocation = await _gitHubUtils.Clone(_config.RemoteUrl, _config.LocalRepository, _config.IsPrivateRepository);
                if (!string.IsNullOrEmpty(repoLocation))
                {
                    _logger.LogInformation($"GitHub repository has been successfully cloned from {_config.RemoteUrl}.");
                    return "";
                }
                
                Thread.Sleep(30000);
            }

            return $"Failed to clone source code from remote repository after {MaxAttempt} attempts.";
        }

        public async Task<string> Push(string branch)
        {
            var attempt = 1;
            while (attempt <= MaxAttempt)
            {
                _logger.LogInformation($"Push {branch} into remote repository: {_config.RemoteUrl} (attempt #{attempt})");

                attempt++;

                var success = await _gitHubUtils.Push(_config.RemoteUrl, _config.LocalRepository, branch);
                if (success)
                {
                    _logger.LogInformation($"All changes from {branch} have been successfully pushed into: {_config.RemoteUrl}");
                    return "";
                }

                Thread.Sleep(30000);
            }

            return $"Failed to push changes to remote repository after {MaxAttempt} attempt";
        }

        public async Task<int> SubmitPullRequest(string branch, string targetBranch)
        {
            if (branch == "master")
                return 0;

            var prTitle = $"PR Branch {branch}";

            var attempt = 1;           

            while (attempt <= MaxAttempt)
            {
                _logger.LogInformation($"Submit pull request from {branch} branch into {targetBranch} branch on GitHub repository : {_config.RemoteUrl} #{attempt}.");

                attempt++;

                var prNumber = await _gitHubUtils.CreatePullRequest(_config.ProjectName, _config.RepoOwner, branch, targetBranch, prTitle);
                if (!string.IsNullOrEmpty(prNumber))
                {
                    _logger.LogInformation($"The pull request has been successfully submitted on GitHub repository: {_config.RemoteUrl}");
                    return int.Parse(prNumber);
                }
                    
                Thread.Sleep(30000);
            }

            // return 0 as default pull request number
            return 0;
        }

        public async Task<bool> MergePullRequest(string prNumber)
        {
            var attempt = 1;           

            while (attempt <= MaxAttempt)
            {
                _logger.LogInformation($"Merge #PR{prNumber} on GitHub repository : {_config.RemoteUrl} #{attempt}.");

                attempt++;

                var success = await _gitHubUtils.MergePullRequest(_config.ProjectName, _config.RepoOwner, prNumber);
                if (success)
                {
                    _logger.LogInformation($"The #PR{prNumber} has been successfully merged.");

                    return true;
                }
                Thread.Sleep(30000);
            }

            return false;
        }
    }
}
