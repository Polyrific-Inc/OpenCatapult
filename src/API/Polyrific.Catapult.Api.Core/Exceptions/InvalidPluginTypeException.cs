// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class InvalidPluginTypeException : Exception
    {
        public string PluginType { get; }
        public string PluginName { get; }
        public string[] TaskTypes { get; }

        public InvalidPluginTypeException(string pluginType, string pluginName) : 
            base($"Plugin type \"{pluginType}\" of provider \"{pluginName}\" is not valid.")
        {
            PluginType = pluginType;
            PluginName = pluginName;
        }

        public InvalidPluginTypeException(string pluginType, string pluginName, string[] taskTypes) : 
            base($"Plugin type \"{pluginType}\" of provider \"{pluginName}\" is not valid. It can only be used for the following task type: {string.Join(DataDelimiter.Comma.ToString(), taskTypes)}")
        {
            PluginType = pluginType;
            PluginName = pluginName;
            TaskTypes = taskTypes;
        }
    }
}
