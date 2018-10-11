// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Engine.Core.Exceptions
{
    public class InvalidExternalServiceTypeException : Exception
    {
        public InvalidExternalServiceTypeException(string serviceType, int taskId)
            : base($"Task Config \"{serviceType}ExternalService\" is required for task \"{taskId}\".")
        {
            ServiceType = serviceType;
            TaskId = taskId;
        }

        public string ServiceType { get; }
        public int TaskId { get; }
    }
}
