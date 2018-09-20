// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class PluginFilterSpecification : BaseSpecification<Plugin>
    {
        /// <summary>
        /// Name of the plugin
        /// </summary>
        public string PluginName { get; set; }

        /// <summary>
        /// Type of the plugin
        /// </summary>
        public string PluginType { get; set; }

        /// <summary>
        /// Id of the plugin
        /// </summary>
        public int PluginId { get; set; }

        /// <summary>
        /// Filter plugins by Name and Type
        /// </summary>
        /// <param name="name">Name of the plugin (set null if you don't want to search by Name)</param>
        /// <param name="type">Type of the plugin (set null if you don't want to search by Type)</param>
        public PluginFilterSpecification(string name, string type) 
            : base(m => (name == null || m.Name.Contains(name)) && (type == null || m.Type == type))
        {
            PluginName = name;
            PluginType = type;
        }

        /// <summary>
        /// Filter plugins by id
        /// </summary>
        /// <param name="id">Id of the plugin</param>
        public PluginFilterSpecification(int id)
            : base(m => m.Id == id)
        {
            PluginId = id;
        }
    }
}
