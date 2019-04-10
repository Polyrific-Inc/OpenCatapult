using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class Tag : BaseEntity
    {
        /// <summary>
        /// Name of the tag
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The plugin tags mapping
        /// </summary>
        public virtual ICollection<PluginTag> PluginTags { get; set; }
    }
}
