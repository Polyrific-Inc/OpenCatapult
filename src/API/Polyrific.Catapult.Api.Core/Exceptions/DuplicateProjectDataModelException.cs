// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DuplicateProjectDataModelException : Exception
    {
        public string Name { get; set; }

        public DuplicateProjectDataModelException(string name)
            : base($"Project Data Model \"{name}\" already exists.")
        {
            Name = name;
        }
    }
}