using System.Collections.Generic;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class Tag : BaseEntity
    {
        /// <summary>
        /// Name of the tag
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The provider tags mapping
        /// </summary>
        public virtual ICollection<TaskProviderTag> ProviderTags { get; set; }
    }
}
