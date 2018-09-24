// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class PluginNotFoundException : Exception
    {
        public PluginNotFoundException(int pluginId)
            : base($"Plugin \"{pluginId}\" was not found.")
        {
            PluginId = pluginId;
        }

        public int PluginId { get; }
    }
}
