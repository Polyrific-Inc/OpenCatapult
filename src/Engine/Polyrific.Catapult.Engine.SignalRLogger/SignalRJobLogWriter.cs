// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Polyrific.Catapult.Engine.Core.JobLogger;

namespace Polyrific.Catapult.Engine.SignalRLogger
{
    public class SignalRJobLogWriter : IJobLogWriter
    {
        private const string JobQueueHubEndpoint = "jobQueueHub";
        private readonly HubConnection _hubConnection;

        public SignalRJobLogWriter(SignalRClientOption option)
        {
            _hubConnection = GetConnection($"{option.ApiUrl.ToString()}{JobQueueHubEndpoint}", option.AuthorizationToken, option.ApiRequestTimeout);
            _hubConnection.StartAsync().Wait();
        }

        public async Task EndJobLog(int jobQueueId)
        {
            await _hubConnection.SendAsync("CompleteJob", jobQueueId);
        }

        public async Task WriteLog(int jobQueueId, string taskName, string message)
        {
            await _hubConnection.SendAsync("SendMessage", jobQueueId, taskName, message);
        }

        private HubConnection GetConnection(string hubUrl, string authorizationToken, TimeSpan timeout)
        {
            var connection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(authorizationToken);
                options.CloseTimeout = timeout;
            })
            .Build();

            return connection;
        }
    }
}
