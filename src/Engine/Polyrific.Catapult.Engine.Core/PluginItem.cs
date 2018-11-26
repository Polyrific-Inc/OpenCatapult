// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Engine.Core
{
    public class PluginItem
    {
        public PluginItem()
        {
            
        }

        public PluginItem(string name, string dllPath, string[] requiredServices)
        {
            Name = name;
            DllPath = dllPath;
            RequiredServices = requiredServices;
        }

        /// <summary>
        /// Name of the plugin
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path location to the plugin DLL file
        /// </summary>
        public string DllPath { get; set; }

        /// <summary>
        /// Names of the required external service connections
        /// </summary>
        public string[] RequiredServices { get; set; }
    }
}
