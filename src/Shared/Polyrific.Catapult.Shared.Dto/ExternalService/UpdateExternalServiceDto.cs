// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.ExternalService
{
    public class UpdateExternalServiceDto
    {
        /// <summary>
        /// Description of the external service
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Config of the external service
        /// </summary>
        public Dictionary<string, string> Config { get; set; }
    }
}
