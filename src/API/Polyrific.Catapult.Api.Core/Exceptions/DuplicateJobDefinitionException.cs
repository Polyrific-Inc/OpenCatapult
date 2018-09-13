// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DuplicateJobDefinitionException : Exception
    {
        public string Name { get; set; }

        public DuplicateJobDefinitionException(string name)
            : base($"Job Definition \"{name}\" already exists.")
        {
            Name = name;
        }
    }
}