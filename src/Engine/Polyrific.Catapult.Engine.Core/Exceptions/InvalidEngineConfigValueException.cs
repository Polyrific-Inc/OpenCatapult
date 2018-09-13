// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Engine.Core.Exceptions
{
    public class InvalidEngineConfigValueException : Exception
    {
        public InvalidEngineConfigValueException(string configName, string configValue)
            : base($"\"{configValue}\" is not a valid value for \"{configName}\" config.")
        {
            ConfigName = configName;
            ConfigValue = configValue;
        }

        public string ConfigName { get; }
        public string ConfigValue { get; }
    }
}