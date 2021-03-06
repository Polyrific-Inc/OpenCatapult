﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
        /// Name of the external service type
        /// </summary>
        public string ExternalServiceTypeName { get; set; }

        /// <summary>
        /// Config of the external service
        /// </summary>
        public Dictionary<string, string> Config { get; set; }

        /// <summary>
        /// Is the external service can be access globally?
        /// </summary>
        public bool IsGlobal { get; set; }
    }
}
