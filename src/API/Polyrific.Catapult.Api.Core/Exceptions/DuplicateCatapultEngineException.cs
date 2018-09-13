// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DuplicateCatapultEngineException : Exception
    {
        public string Name { get; set; }

        public DuplicateCatapultEngineException(string name)
            : base($"Catapult engine \"{name}\" already exists.")
        {
            Name = name;
        }
    }
}