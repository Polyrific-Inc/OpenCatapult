// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class InvalidTaskProviderTypeException : Exception
    {
        public string TaskProviderType { get; }
        public string ProviderName { get; }
        public string[] TaskTypes { get; }

        public InvalidTaskProviderTypeException(string providerType, string providerName) : 
            base($"Task Provider type \"{providerType}\" of provider \"{providerName}\" is not valid.")
        {
            TaskProviderType = providerType;
            ProviderName = providerName;
        }

        public InvalidTaskProviderTypeException(string providerType, string providerName, string[] taskTypes) : 
            base($"Task Provider type \"{providerType}\" of provider \"{providerName}\" is not valid. It can only be used for the following task type: {string.Join(DataDelimiter.Comma.ToString(), taskTypes)}")
        {
            TaskProviderType = providerType;
            ProviderName = providerName;
            TaskTypes = taskTypes;
        }
    }
}
