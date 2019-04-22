// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class TaskProvider : BaseEntity
    {
        /// <summary>
        /// Name of the task provider
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the task provider
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Author of the task provider
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Version of the task provider
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Required services of the task provider separated by comma
        /// </summary>
        public string RequiredServicesString { get; set; }

        /// <summary>
        /// The Tags of the task provider
        /// </summary>
        public ICollection<TaskProviderTag> Tags { get; set; }

        /// <summary>
        /// Display name of the task provider
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description of the task provider
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Url of the task provider thumbnail
        /// </summary>
        public string ThumbnailUrl { get; set; }
    }
}
