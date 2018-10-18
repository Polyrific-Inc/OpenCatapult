// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using GitHub.Helpers;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Octokit;
using Repository = LibGit2Sharp.Repository;

namespace GitHub
{
    public class GitHubUtils : IGitHubUtils
    {
        private readonly string _credentialType;
        private readonly string _userName;
        private readonly string _password;
        private readonly ILogger _logger;

        public GitHubUtils(string credentialType, string userName, string password, ILogger logger)
        {
            _credentialType = credentialType;
            _userName = userName;
            _password = password;
            _logger = logger;
        }

        public async Task<string> Clone(string remoteUrl, string localRepository, bool isPrivateRepository)
        {
            var cloneOption = new CloneOptions()
            {
                OnTransferProgress = CloneTransferProgressHandler
            };

            if (isPrivateRepository)
            {
                if (_credentialType == "userPassword")
                {
                    cloneOption.CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
                    {
                        Username = _userName,
                        Password = _password
                    };
                }
                else if (_credentialType == "authToken")
                {
                    cloneOption.CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
                    {
                        Username = _userName,
                        Password = string.Empty
                    };
                }
            }

            try
            {
                return await Task.Run(() => Repository.Clone(remoteUrl, localRepository, cloneOption));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return "";
            }
        }

        public async Task<string> CreatePullRequest(string projectName, string repositoryOwner, string branch, string targetBranch, string title)
        {
            var client = new GitHubClient(new ProductHeaderValue(projectName))
            {
                Credentials = _credentialType == "userPassword"
                    ? new Octokit.Credentials(_userName, _password)
                    : new Octokit.Credentials(_userName)
            };

            var newPr = new NewPullRequest(title, branch, targetBranch);

            try
            {
                var prList = await client.PullRequest.GetAllForRepository(repositoryOwner, projectName);

                var pr = prList.FirstOrDefault(x => x.Head.Label.Split(':')[1] == branch);
                if (pr != null)
                {
                    return pr.Number.ToString();
                }

                var prResult = await client.PullRequest.Create(repositoryOwner, projectName, newPr);
                if (prResult?.Number > 0)
                {
                    return prResult.Number.ToString();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return "";
        }

        public async Task<bool> MergePullRequest(string projectName, string repositoryOwner, string prNumber)
        {
            var client = new GitHubClient(new ProductHeaderValue(projectName))
            {
                Credentials = _credentialType == "userPassword"
                    ? new Octokit.Credentials(_userName, _password)
                    : new Octokit.Credentials(_userName)
            };

            var currentRepo = await client.Repository.Get(repositoryOwner, projectName);

            var mergedRequest = new MergePullRequest();

            if (currentRepo.AllowSquashMerge ?? false) // Use squash merge if allowed
            {
                mergedRequest.MergeMethod = PullRequestMergeMethod.Squash;
            }
            else if (!currentRepo.AllowMergeCommit ?? true) // otherwise, use Merge Commit. But if it does not allowed either, then use Rebase
            {
                mergedRequest.MergeMethod = PullRequestMergeMethod.Rebase;
            }

            var pr = await client.PullRequest.Get(repositoryOwner, projectName, int.Parse(prNumber));
            if (pr != null)
            {
                if (pr.Merged)
                {
                    return pr.Merged;
                }

                if (pr.State == ItemState.Closed)
                {
                    _logger.LogInformation($"#PR{prNumber} has already been closed");
                    return true;
                }
            }

            try
            {
                var mergeResult = await client.PullRequest.Merge(repositoryOwner, projectName, int.Parse(prNumber), mergedRequest);
                if (mergeResult.Merged)
                {
                    if (pr != null)
                    {
                        var prBranchRef = $"heads/{pr.Head.Ref}";
                        var prBranch = await client.Git.Reference.Get(repositoryOwner, projectName, prBranchRef);

                        if (prBranch != null)
                        {
                            await client.Git.Reference.Delete(repositoryOwner, projectName, prBranchRef);
                        }
                    }
                    
                    return mergeResult.Merged;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return false;
            }

            return false;
        }

        public async Task<bool> Push(string remoteUrl, string localRepository, string branch)
        {
            var credential = new UsernamePasswordCredentials();

            // use GitHub token if provided
            if (_credentialType == "userPassword")
            {
                // use user name and password if there is no GitHub token
                credential.Username = _userName;
                credential.Password = _password;
            }
            else if (_credentialType == "authToken")
            {
                credential.Username = _userName;
                credential.Password = string.Empty;
            }
            
            var options = new PushOptions
            {
                CredentialsProvider = (url, usernameFromUrl, types) => credential,
                OnPushTransferProgress = PushTransferProgressHandler
            };

            var repo = new Repository(localRepository);
            try
            {
                // Push the changes                     
                await Task.Run(() => repo.Network.Push(repo.Branches[branch], options));

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return false;
            }
        }

        public Task<bool> CheckoutBranch(string localRepository, string branch)
        {
            var repo = new Repository(localRepository);
            var branchObj = repo.Branches[branch] != null ? repo.Branches[branch] : repo.Branches[$"origin/{branch}"];
            Commands.Checkout(repo, branchObj);

            return Task.FromResult(true);
        }

        public Task<bool> Commit(string localRepository, string baseBranch, string branch, string commitMessage, string author, string email)
        {
            try
            {
                var repo = new Repository(localRepository);
                var signature = new LibGit2Sharp.Signature(author, email, DateTimeOffset.UtcNow);

                // checkout base branch
                var branchObj = repo.Branches[baseBranch] != null ? repo.Branches[baseBranch] : repo.Branches[$"origin/{baseBranch}"];
                if (repo.Head.Commits.FirstOrDefault() != branchObj.Commits.FirstOrDefault())
                    Commands.Checkout(repo, branchObj);

                if (repo.Branches[branch] == null)
                {
                    var newBranch = repo.CreateBranch(branch);

                    var remote = repo.Network.Remotes["origin"];
                    repo.Branches.Update(newBranch,
                        b => b.Remote = remote.Name,
                        b => b.UpstreamBranch = newBranch.CanonicalName);

                    _logger.LogInformation("Branch {branch} has been created.", branch);
                }

                Commands.Checkout(repo, repo.Branches[branch]);
                Commands.Stage(repo, "*");
                var commit = repo.Commit(commitMessage, signature, signature);

                return Task.FromResult(commit != null);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.FromResult(false);
            }
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
