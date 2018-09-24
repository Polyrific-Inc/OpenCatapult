// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.Plugin
{
    public class PluginAdditionalConfigDto
    {
        /// <summary>
        /// Name of the configuration
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display label of the configuration
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Data type of the configuration (string, number, boolean)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Is the configuration mandatory?
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Is the configuration contains sensitive value?
        /// </summary>
        public bool IsSecret { get; set; }
    }
}
