// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Engine.Core.Exceptions
{
    public class InvalidEngineConfigNameException : Exception
    {
        public InvalidEngineConfigNameException(string configName)
            : base($"\"{configName}\" is not a valid Engine config name.")
        {
            ConfigName = configName;
        }

        public string ConfigName { get; }
    }
}