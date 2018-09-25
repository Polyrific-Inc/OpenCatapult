// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class PluginAdditionalConfigFilterSpecification : BaseSpecification<PluginAdditionalConfig>
    {
        /// <summary>
        /// Filter Plugin additional configs by plugin id
        /// </summary>
        /// <param name="pluginId"></param>
        public PluginAdditionalConfigFilterSpecification(int pluginId) 
            : base(m => m.PluginId == pluginId)
        {
        }

        /// <summary>
        /// Filter Plugin additional configs by plugin name
        /// </summary>
        /// <param name="pluginId"></param>
        public PluginAdditionalConfigFilterSpecification(string pluginName)
            : base(m => m.Plugin.Name == pluginName)
        {
        }
    }
}
