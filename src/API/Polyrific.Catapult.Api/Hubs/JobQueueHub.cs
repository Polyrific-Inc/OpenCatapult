// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Polyrific.Catapult.Shared.Common.Interface;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Hubs
{
    [Authorize]
    public class JobQueueHub : Hub
    {
        private const string GroupPrefix = "jobQueue_";
        private const string JobQueueIdQueryParamKey = "jobQueueId";

        private readonly ITextWriter _textWriter;

        public JobQueueHub(ITextWriter textWriter)
        {
            _textWriter = textWriter;
        }

        public async Task SendMessage(int jobQueueId, string taskName, string message)
        {
            var group = GetGroupName(jobQueueId.ToString());
            List<string> groups = new List<string>() { group };
            await _textWriter.Write($"{JobQueueLog.FolderNamePrefix}{jobQueueId}", taskName ?? "job", message);
            await Clients.Groups(groups).SendAsync("ReceiveMessage", taskName, message);
        }

        public async Task CompleteJob(int jobQueueId)
        {
            var group = GetGroupName(jobQueueId.ToString());
            List<string> groups = new List<string>() { group };
            await Clients.Groups(groups).SendAsync("JobCompleted");
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var jobQueueId = httpContext.Request.Query[JobQueueIdQueryParamKey];

            if (!string.IsNullOrEmpty(jobQueueId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(jobQueueId));

                var previousMessages = await _textWriter.Read($"{JobQueueLog.FolderNamePrefix}{jobQueueId}", null);
                await Clients.Caller.SendAsync("ReceiveInitialMessage", previousMessages);
            }
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var jobQueueId = httpContext.Request.Query[JobQueueIdQueryParamKey];

            if (!string.IsNullOrEmpty(jobQueueId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(jobQueueId));
            }

            await base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName(string jobQueueId)
        {
            return $"{GroupPrefix}{jobQueueId}";
        }
    }
}
