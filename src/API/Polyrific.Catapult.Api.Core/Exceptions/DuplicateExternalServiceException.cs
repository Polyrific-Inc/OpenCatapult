// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DuplicateExternalServiceException : Exception
    {
        public string Name { get; set; }

        public DuplicateExternalServiceException(string name)
            : base($"External service \"{name}\" already exists.")
        {
            Name = name;
        }
    }
}