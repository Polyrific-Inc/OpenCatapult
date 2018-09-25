// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.ExternalServiceType
{
    public class ExternalServiceTypeDto
    {
        /// <summary>
        /// Id of the external service type
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the external service type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Properties of the external service type
        /// </summary>
        public List<ExternalServicePropertyDto> ExternalServiceProperties { get; set; }
    }
}
