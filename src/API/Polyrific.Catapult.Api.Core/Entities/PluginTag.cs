// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class PluginTag : BaseEntity
    {
        /// <summary>
        /// Id of the Plugin
        /// </summary>
        public int PluginId { get; set; }
        /// <summary>
        /// The Plugin entity
        /// </summary>
        public virtual Plugin Plugin { get; set; }

        /// <summary>
        /// Id of the Tag
        /// </summary>
        public int TagId { get; set; }
        /// <summary>
        /// The Tag entity
        /// </summary>
        public virtual Tag Tag { get; set; }
    }
}
