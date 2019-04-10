// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class Plugin : BaseEntity
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
        /// Required services of the plugin separated by comma
        /// </summary>
        public string RequiredServicesString { get; set; }

        /// <summary>
        /// The Tags of the plugin
        /// </summary>
        public ICollection<PluginTag> Tags { get; set; }

        /// <summary>
        /// Display name of the plugin
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description of the plugin
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Url of the plugin thumbnail
        /// </summary>
        public string ThumbnailUrl { get; set; }
    }
}
