// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.ExternalService
{
    public class CreateExternalServiceDto
    {
        /// <summary>
        /// Name of the external service
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Description of the external service
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Type of the external service
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Config of the external service
        /// </summary>
        [Required]
        public Dictionary<string, string> Config { get; set; }
    }
}
