// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Cli
{
    public class SignalRJobQueueLogListener : IJobQueueLogListener
    {
        private const string JobQueueHubEndpoint = "jobQueueHub";

        private readonly ITokenStore _tokenStore;
        private readonly CatapultCliConfig _config;

        public SignalRJobQueueLogListener(CatapultCliConfig config, ITokenStore tokenStore)
        {
            _config = config;
            _tokenStore = tokenStore;
        }

        public async Task Listen(int projectId, int jobQueueId, Action<string> onLogReceived, Action<string> onError)
        {
            var jobQueueCompleted = new TaskCompletionSource<bool>();
            var connection = GetConnection(new Uri(_config.ApiUrl, $"{JobQueueHubEndpoint}?projectId={projectId}&jobQueueId={jobQueueId}").AbsoluteUri);

            connection.On<string>("ReceiveInitialMessage", (initialMessage) =>
            {
                onLogReceived(initialMessage);

                connection.On<string, string>("ReceiveMessage", (taskName, message) =>
                {
                    onLogReceived(message);
                });
            });

            connection.On("JobCompleted", () =>
            {
                onLogReceived("Job has completed");

                jobQueueCompleted.SetResult(true);
            });

            await connection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    onError($"There was an error opening the connection:{task.Exception.GetBaseException()}");
                }
            });

            if (await jobQueueCompleted.Task)
            {
                await connection.StopAsync();
            }
        }

        private HubConnection GetConnection(string hubUrl)
        {
            var connection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => _tokenStore.GetSavedToken();
            })
            .Build();

            return connection;
        }
    }
}
