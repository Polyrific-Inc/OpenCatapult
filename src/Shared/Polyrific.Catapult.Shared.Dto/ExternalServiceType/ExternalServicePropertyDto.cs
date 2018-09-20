// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.ExternalServiceType
{
    public class ExternalServicePropertyDto
    {
        /// <summary>
        /// Name of the external service property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the external service property
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Allowed values of the external service property
        /// </summary>
        public string[] AllowedValues { get; set; }

        /// <summary>
        /// Indicates whether the external service property is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Indicates whether the external service property is a secret property that needs to be obfuscated in UI
        /// </summary>
        public bool IsSecret { get; set; }
    }
}
