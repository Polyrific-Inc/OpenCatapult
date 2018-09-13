// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Engine.Core.Exceptions
{
    public class InvalidJobTaskTypeException : Exception
    {
        public InvalidJobTaskTypeException(string type)
            : base($"{type} is not a valid type of job task.")
        {
            Type = type;
        }

        public string Type { get; }
    }
}