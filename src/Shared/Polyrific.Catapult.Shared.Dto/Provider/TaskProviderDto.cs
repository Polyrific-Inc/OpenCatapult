// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Shared.Dto.Provider
{
    public class TaskProviderDto
    {
        /// <summary>
        /// Id of the task provider
        /// </summary>
        public int Id { get; set; }

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
        /// Required services of the task provider
        /// </summary>
        public string[] RequiredServices { get; set; }

        /// <summary>
        /// Provider additional configs
        /// </summary>
        public TaskProviderAdditionalConfigDto[] AdditionalConfigs { get; set; }

        /// <summary>
        /// The Tags of the task provider
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// Created date of the task provider
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Updated date of the task provider
        /// </summary>
        public DateTime? Updated { get; set; }

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
