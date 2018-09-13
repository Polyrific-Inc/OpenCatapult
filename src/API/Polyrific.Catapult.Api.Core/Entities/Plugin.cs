// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
    }
}
