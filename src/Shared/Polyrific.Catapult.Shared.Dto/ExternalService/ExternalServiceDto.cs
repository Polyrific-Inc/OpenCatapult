// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.ExternalService
{
    public class ExternalServiceDto
    {
        /// <summary>
        /// Id of the external service
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the external service
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the external service
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Id of the external service type
        /// </summary>
        public int ExternalServiceTypeId { get; set; }

        /// <summary>
        /// Config of the external service
        /// </summary>
        public Dictionary<string, string> Config { get; set; }
    }
}
