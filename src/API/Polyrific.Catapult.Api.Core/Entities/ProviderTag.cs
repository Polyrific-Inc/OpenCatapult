// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class ProviderTag : BaseEntity
    {
        /// <summary>
        /// Id of the task provider
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// The task provider entity
        /// </summary>
        public virtual Provider Provider { get; set; }

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
