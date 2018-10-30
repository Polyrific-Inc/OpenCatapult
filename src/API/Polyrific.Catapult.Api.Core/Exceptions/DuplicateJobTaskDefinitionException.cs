// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DuplicateJobTaskDefinitionException : Exception
    {
        public string Name { get; set; }

        public DuplicateJobTaskDefinitionException(string name)
            : base($"Job Task Definition \"{name}\" already exists.")
        {
            Name = name;
        }
    }
}
