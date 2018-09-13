// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DuplicateProjectDataModelPropertyException : Exception
    {
        public string Name { get; set; }

        public DuplicateProjectDataModelPropertyException(string name)
            : base($"Project Data Model Property \"{name}\" already exists.")
        {
            Name = name;
        }
    }
}