// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class InvalidProviderTypeException : Exception
    {
        public string ProviderType { get; }
        public string ProviderName { get; }
        public string[] TaskTypes { get; }

        public InvalidProviderTypeException(string providerType, string providerName) : 
            base($"Provider type \"{providerType}\" of provider \"{providerName}\" is not valid.")
        {
            ProviderType = providerType;
            ProviderName = providerName;
        }

        public InvalidProviderTypeException(string providerType, string providerName, string[] taskTypes) : 
            base($"Provider type \"{providerType}\" of provider \"{providerName}\" is not valid. It can only be used for the following task type: {string.Join(DataDelimiter.Comma.ToString(), taskTypes)}")
        {
            ProviderType = providerType;
            ProviderName = providerName;
            TaskTypes = taskTypes;
        }
    }
}
