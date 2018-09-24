// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.Plugin
{
    public class NewPluginDto
    {
        /// <summary>
        /// Name of the plugin
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the plugin
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Author of the plugin
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Version of the plugin
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Required services of the plugin
        /// </summary>
        public string[] RequiredServices { get; set; }

        /// <summary>
        /// Plugin additional configs
        /// </summary>
        public PluginAdditionalConfigDto[] AdditionalConfigs { get; set; }
    }
}
