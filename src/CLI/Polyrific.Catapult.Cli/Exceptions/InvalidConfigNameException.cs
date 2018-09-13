// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Cli.Exceptions
{
    public class InvalidConfigNameException : Exception
    {
        public InvalidConfigNameException(string message) : base(message)
        {

        }
    }
}
