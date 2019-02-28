// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Polyrific.Catapult.TaskProviders.GitHub.Helpers;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Octokit;
using Repository = LibGit2Sharp.Repository;
using System.IO;

namespace Polyrific.Catapult.TaskProviders.GitHub
{
    public class GitHubUtils : IGitHubUtils
    {
        private readonly UsernamePasswordCredentials _gitCredential;
        private readonly Octokit.Credentials _gitHubCredential;
        private readonly ILogger _logger;

        public GitHubUtils(string credentialType, string userName, string password, ILogger logger)
        {
            _logger = logger;
            _gitCredential = GetGitCredentials(credentialType, userName, password);
            _gitHubCredential = GetGitHubCredentials(credentialType, userName, password);
        }

        public async Task<string> Clone(string remoteUrl, string localRepository, bool isPrivateRepository)
        {
            var cloneOption = new CloneOptions()
            {
                OnTransferProgress = CloneTransferProgressHandler
            };

            if (isPrivateRepository)
            {
                cloneOption.CredentialsProvider = (url, user, cred) => _gitCredential;
            }

            try
            {
                return await Task.Run(() => Repository.Clone(remoteUrl, localRepository, cloneOption));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return "";
            }
        }

        public async Task<string> CreatePullRequest(string projectName, string repositoryOwner, string branch, string targetBranch, string title)
        {
            var client = new GitHubClient(new ProductHeaderValue(projectName))
            {
                Credentials = _gitHubCredential
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return "";
        }

        public async Task<bool> MergePullRequest(string projectName, string repositoryOwner, string prNumber)
        {
            var client = new GitHubClient(new ProductHeaderValue(projectName))
            {
                Credentials = _gitHubCredential
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return false;
            }

            return false;
        }

        public async Task<bool> Push(string remoteUrl, string localRepository, string branch)
        {
            var options = new PushOptions
            {
                CredentialsProvider = (url, usernameFromUrl, types) => _gitCredential,
                OnPushTransferProgress = PushTransferProgressHandler
            };

            var repo = new Repository(localRepository);
            try
            {
                var masterBranch = repo.Branches["master"];
                if (masterBranch.TrackingDetails.CommonAncestor == null)
                {
                    _logger.LogInformation("Pushing master branch");
                    await Task.Run(() => repo.Network.Push(masterBranch, options));
                }

                // Push the changes      
                _logger.LogInformation($"Pushing {branch} branch");
                await Task.Run(() => repo.Network.Push(repo.Branches[branch], options));

                return true;
            }
            catch (Exception ex)
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

                // if we're working on empty repository, create master branch
                if (repo.Branches == null || repo.Branches.Count() == 0)
                {
                    var readmeFile = Path.Combine(localRepository, "README.md");

                    if (!File.Exists(readmeFile))
                        File.WriteAllText(Path.Combine(localRepository, "README.md"), "# Catapult-generated");

                    Commands.Stage(repo, "README.md");

                    if (File.Exists(Path.Combine(localRepository, ".gitignore")))
                        Commands.Stage(repo, ".gitignore");

                    repo.Commit("Initial commit", signature, signature);
                    var masterBranch = repo.Branches["master"];

                    var remote = repo.Network.Remotes["origin"];
                    repo.Branches.Update(masterBranch,
                        b => b.Remote = remote.Name,
                        b => b.UpstreamBranch = masterBranch.CanonicalName);

                    _logger.LogInformation("Repository has been initialized");
                }

                // checkout base branch
                var branchObj = repo.Branches[baseBranch] != null ? repo.Branches[baseBranch] : repo.Branches[$"origin/{baseBranch}"];

                if (branch == null)
                {
                    _logger.LogError($"Base branch {baseBranch} was not found");
                    return Task.FromResult(false);
                }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.FromResult(false);
            }
        }

        public async Task<string> CreateRepositoryIfNotExists(string projectName, string repositoryOwner, bool isPrivateRepository = true)
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue(projectName))
                {
                    Credentials = _gitHubCredential
                };

                var repository = await GetGitHubRepository(projectName, repositoryOwner);

                if (repository == null)
                {
                    _logger.LogInformation($"Creating repository {repositoryOwner}/{projectName}...");
                    var newRepository = new NewRepository(projectName)
                    {
                        Private = isPrivateRepository
                    };

                    var currentUser = await client.User.Current();

                    if (repositoryOwner.Equals(currentUser.Login, StringComparison.InvariantCultureIgnoreCase))
                    {
                        await client.Repository.Create(newRepository);
                    }
                    else
                    {
                        await client.Repository.Create(repositoryOwner, newRepository);
                    }
                }
                else
                {
                    _logger.LogInformation($"Repository {repositoryOwner}/{projectName} is already exists");
                }

                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex.Message;
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

        private UsernamePasswordCredentials GetGitCredentials(string credentialType, string userName, string password)
        {
            var credential = new UsernamePasswordCredentials();

            if (credentialType == "userPassword")
            {
                credential.Username = userName;
                credential.Password = password;
            }
            else
            {
                credential.Username = userName;
                credential.Password = string.Empty;
            }

            return credential;
        }

        private Octokit.Credentials GetGitHubCredentials(string credentialType, string userName, string password)
        {
            return credentialType == "userPassword"
                    ? new Octokit.Credentials(userName, password)
                    : new Octokit.Credentials(userName);
        }

        private async Task<Octokit.Repository> GetGitHubRepository(string projectName, string repositoryOwner)
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue(projectName))
                {
                    Credentials = _gitHubCredential
                };

                var repository = await client.Repository.Get(repositoryOwner, projectName);

                return repository;
            }
            catch (Octokit.NotFoundException)
            {
                return null;
            }
        }
    }
}
