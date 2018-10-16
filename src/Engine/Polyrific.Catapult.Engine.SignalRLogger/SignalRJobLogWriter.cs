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
        private readonly SignalRClientOption _option;
        private HubConnection _hubConnection;

        public SignalRJobLogWriter(SignalRClientOption option)
        {
            _option = option;
        }

        public async Task EndJobLog(int jobQueueId)
        {
            if (_hubConnection == null)
                _hubConnection = await GetConnection();

            await _hubConnection.SendAsync("CompleteJob", jobQueueId);
        }

        public async Task WriteLog(int jobQueueId, string taskName, string message)
        {
            if (_hubConnection == null)
                _hubConnection = await GetConnection();

            await _hubConnection.SendAsync("SendMessage", jobQueueId, taskName, message);
        }

        private async Task<HubConnection> GetConnection()
        {
            var connection = new HubConnectionBuilder()
            .WithUrl($"{_option.ApiUrl.ToString()}{JobQueueHubEndpoint}", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(_option.AuthorizationToken);
                options.CloseTimeout = _option.ApiRequestTimeout;
            })
            .Build();

            await connection.StartAsync();

            return connection;
        }
    }
}
